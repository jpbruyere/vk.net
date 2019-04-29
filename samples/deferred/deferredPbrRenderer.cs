﻿using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;
using VK;
using CVKL;

namespace deferred {
	public class DeferredPbrRenderer : IDisposable {
		Device dev;
		SwapChain swapChain;
		public PresentQueue presentQueue;

		public static VkSampleCountFlags NUM_SAMPLES = VkSampleCountFlags.SampleCount4;
		public static VkFormat HDR_FORMAT = VkFormat.R32g32b32a32Sfloat;
		public static VkFormat MRT_FORMAT = VkFormat.R32g32b32a32Sfloat;

		public enum DebugView {
			none,
			color,
			normal,
			pos,
			occlusion,
			emissive,
			metallic,
			roughness,
			depth,
			prefill,
			irradiance,
			shadowMap
		}

		public DebugView currentDebugView = DebugView.none;
		public int lightNumDebug = 0;

		public struct Matrices {
			public Matrix4x4 projection;
			public Matrix4x4 model;
			public Matrix4x4 view;
			public Vector4 camPos;
			public float exposure;
			public float gamma;
			public float prefilteredCubeMipLevels;
			public float scaleIBLAmbient;
		}

		public struct Light {
			public Vector4 position;
			public Vector4 color;
			public Matrix4x4 mvp;
		}

		public Matrices matrices = new Matrices {
			gamma = 1.2f,
			exposure = 2.0f,
			scaleIBLAmbient = 0.5f,
		};

		public Light[] lights = {
			new Light {
				position = new Vector4(2.5f,3.5f,2,0f),
				color = new Vector4(1,0.8f,0.8f,1)
			},
			new Light {
				position = new Vector4(-2.5f,2.5f,2,0f),
				color = new Vector4(0.8f,0.8f,1,1)
			}
		};

		const float lightMoveSpeed = 0.1f;

		Framebuffer[] frameBuffers;
		Image gbColorRough, gbEmitMetal, gbN_AO, gbPos, hdrImg;

		DescriptorPool descriptorPool;
		DescriptorSetLayout descLayoutMain, descLayoutTextures, descLayoutGBuff;
		DescriptorSet dsMain, dsGBuff;

		public PipelineCache pipelineCache;
		Pipeline gBuffPipeline, composePipeline, toneMappingPipeline, debugPipeline;

		public HostBuffer uboMatrices { get; private set; }
		public HostBuffer<Light> uboLights { get; private set; }

		RenderPass renderPass;

		public PbrModel model { get; private set; }
		public EnvironmentCube envCube;
		public ShadowMapRenderer shadowMapRenderer;

		public BoundingBox modelAABB;

		const int SP_SKYBOX 		= 0;
		const int SP_MODELS 		= 1;
		const int SP_COMPOSE 		= 2;
		const int SP_TONE_MAPPING 	= 3;

		public DeferredPbrRenderer (Device dev, SwapChain swapChain, PresentQueue presentQueue, float nearPlane, float farPlane) {
			this.dev = dev;
			this.swapChain = swapChain;
			this.presentQueue = presentQueue;

			pipelineCache = new PipelineCache (dev);

			descriptorPool = new DescriptorPool (dev, 3,
				new VkDescriptorPoolSize (VkDescriptorType.UniformBuffer, 3),
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler, 5),
				new VkDescriptorPoolSize (VkDescriptorType.InputAttachment, 6)
			);

			uboMatrices = new HostBuffer (dev, VkBufferUsageFlags.UniformBuffer, matrices, true);
			uboLights = new HostBuffer<Light> (dev, VkBufferUsageFlags.UniformBuffer, lights, true);

#if WITH_SHADOWS
			shadowMapRenderer = new ShadowMapRenderer (dev, this);
#endif

			init (nearPlane, farPlane);
		}

		void init (float nearPlane, float farPlane) {

			renderPass = new RenderPass (dev, NUM_SAMPLES);

			renderPass.AddAttachment (swapChain.ColorFormat, VkImageLayout.PresentSrcKHR, VkSampleCountFlags.SampleCount1);//swapchain image
			renderPass.AddAttachment (dev.GetSuitableDepthFormat(), VkImageLayout.DepthStencilAttachmentOptimal, NUM_SAMPLES);
			renderPass.AddAttachment (swapChain.ColorFormat, VkImageLayout.ColorAttachmentOptimal, NUM_SAMPLES, VkAttachmentLoadOp.Clear, VkAttachmentStoreOp.DontCare);//GBuff0 (color + roughness) and final color before resolve
			renderPass.AddAttachment (VkFormat.R8g8b8a8Unorm, VkImageLayout.ColorAttachmentOptimal, NUM_SAMPLES, VkAttachmentLoadOp.Clear, VkAttachmentStoreOp.DontCare);//GBuff1 (emit + metal)
			renderPass.AddAttachment (MRT_FORMAT, VkImageLayout.ColorAttachmentOptimal, NUM_SAMPLES, VkAttachmentLoadOp.Clear, VkAttachmentStoreOp.DontCare);//GBuff2 (normals + AO)
			renderPass.AddAttachment (MRT_FORMAT, VkImageLayout.ColorAttachmentOptimal, NUM_SAMPLES, VkAttachmentLoadOp.Clear, VkAttachmentStoreOp.DontCare);//GBuff3 (Pos + depth)
			renderPass.AddAttachment (HDR_FORMAT, VkImageLayout.ColorAttachmentOptimal, NUM_SAMPLES);//hdr color

			renderPass.ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0.0f, 0.0f, 0.0f) });
			renderPass.ClearValues.Add (new VkClearValue { depthStencil = new VkClearDepthStencilValue (1.0f, 0) });
			renderPass.ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0.0f, 0.0f, 0.0f) });
			renderPass.ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0.0f, 0.0f, 0.0f) });
			renderPass.ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0.0f, 0.0f, 0.0f) });
			renderPass.ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0.0f, 0.0f, 0.0f) });
			renderPass.ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0.0f, 0.0f, 0.0f) });

			SubPass[] subpass = { new SubPass (), new SubPass (), new SubPass(), new SubPass() };
			//skybox
			subpass[SP_SKYBOX].AddColorReference (6, VkImageLayout.ColorAttachmentOptimal);
			//models
			subpass[SP_MODELS].AddColorReference (new VkAttachmentReference (2, VkImageLayout.ColorAttachmentOptimal),
									new VkAttachmentReference (3, VkImageLayout.ColorAttachmentOptimal),
									new VkAttachmentReference (4, VkImageLayout.ColorAttachmentOptimal),
									new VkAttachmentReference (5, VkImageLayout.ColorAttachmentOptimal));
			subpass[SP_MODELS].SetDepthReference (1, VkImageLayout.DepthStencilAttachmentOptimal);
			subpass[SP_MODELS].AddPreservedReference (0);

			//compose
			subpass[SP_COMPOSE].AddColorReference (6, VkImageLayout.ColorAttachmentOptimal);
			subpass[SP_COMPOSE].AddInputReference (new VkAttachmentReference (2, VkImageLayout.ShaderReadOnlyOptimal),
									new VkAttachmentReference (3, VkImageLayout.ShaderReadOnlyOptimal),
									new VkAttachmentReference (4, VkImageLayout.ShaderReadOnlyOptimal),
									new VkAttachmentReference (5, VkImageLayout.ShaderReadOnlyOptimal));
	         	//tone mapping
			subpass[SP_TONE_MAPPING].AddColorReference ((NUM_SAMPLES == VkSampleCountFlags.SampleCount1) ? 0u : 2u, VkImageLayout.ColorAttachmentOptimal);
			subpass[SP_TONE_MAPPING].AddInputReference (new VkAttachmentReference (6, VkImageLayout.ShaderReadOnlyOptimal));
			if (NUM_SAMPLES != VkSampleCountFlags.SampleCount1) 
				subpass[SP_TONE_MAPPING].AddResolveReference (0, VkImageLayout.ColorAttachmentOptimal);
			
			renderPass.AddSubpass (subpass);

			renderPass.AddDependency (Vk.SubpassExternal, SP_SKYBOX,
                VkPipelineStageFlags.BottomOfPipe, VkPipelineStageFlags.ColorAttachmentOutput,
                VkAccessFlags.MemoryRead, VkAccessFlags.ColorAttachmentRead | VkAccessFlags.ColorAttachmentWrite);
			renderPass.AddDependency (SP_SKYBOX, SP_MODELS,
				VkPipelineStageFlags.ColorAttachmentOutput, VkPipelineStageFlags.FragmentShader,
				VkAccessFlags.ColorAttachmentWrite, VkAccessFlags.ShaderRead);
			renderPass.AddDependency (SP_MODELS, SP_COMPOSE,
				VkPipelineStageFlags.ColorAttachmentOutput, VkPipelineStageFlags.FragmentShader,
				VkAccessFlags.ColorAttachmentWrite, VkAccessFlags.ShaderRead);
			renderPass.AddDependency (SP_COMPOSE, SP_TONE_MAPPING,
				VkPipelineStageFlags.ColorAttachmentOutput, VkPipelineStageFlags.FragmentShader,
				VkAccessFlags.ColorAttachmentWrite, VkAccessFlags.ShaderRead);
			renderPass.AddDependency (SP_TONE_MAPPING, Vk.SubpassExternal,
	                VkPipelineStageFlags.ColorAttachmentOutput, VkPipelineStageFlags.BottomOfPipe,
	                VkAccessFlags.ColorAttachmentRead | VkAccessFlags.ColorAttachmentWrite, VkAccessFlags.MemoryRead);


			descLayoutMain = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment, VkDescriptorType.UniformBuffer),//matrices and params
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (2, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (3, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (4, VkShaderStageFlags.Fragment, VkDescriptorType.UniformBuffer),//lights
				new VkDescriptorSetLayoutBinding (5, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),//ui overlay
				new VkDescriptorSetLayoutBinding (6, VkShaderStageFlags.Fragment, VkDescriptorType.UniformBuffer));//materials
#if WITH_SHADOWS
			descLayoutMain.Bindings.Add (new VkDescriptorSetLayoutBinding (7, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler));
#endif

			descLayoutTextures = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (2, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (3, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (4, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler)
			);

			descLayoutGBuff = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.InputAttachment),//color + roughness
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.InputAttachment),//emit + metal
				new VkDescriptorSetLayoutBinding (2, VkShaderStageFlags.Fragment, VkDescriptorType.InputAttachment),//normals + AO
				new VkDescriptorSetLayoutBinding (3, VkShaderStageFlags.Fragment, VkDescriptorType.InputAttachment),//Pos + depth
				new VkDescriptorSetLayoutBinding (4, VkShaderStageFlags.Fragment, VkDescriptorType.InputAttachment));//hdr


			GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, NUM_SAMPLES);
			cfg.rasterizationState.cullMode = VkCullModeFlags.Back;
			if (NUM_SAMPLES != VkSampleCountFlags.SampleCount1) {
				cfg.multisampleState.sampleShadingEnable = true;
				cfg.multisampleState.minSampleShading = 0.5f;
			}
			cfg.Cache = pipelineCache;
			cfg.Layout = new PipelineLayout (dev, descLayoutMain, descLayoutTextures, descLayoutGBuff);
			cfg.Layout.AddPushConstants (
				new VkPushConstantRange (VkShaderStageFlags.Vertex, (uint)Marshal.SizeOf<Matrix4x4> ()),
				new VkPushConstantRange (VkShaderStageFlags.Fragment, sizeof (int), 64)
			);
			cfg.RenderPass = renderPass;
			cfg.SubpassIndex = SP_MODELS;
			cfg.blendAttachments.Add (new VkPipelineColorBlendAttachmentState (false));
			cfg.blendAttachments.Add (new VkPipelineColorBlendAttachmentState (false));
			cfg.blendAttachments.Add (new VkPipelineColorBlendAttachmentState (false));
			//cfg.blendAttachments.Add (new VkPipelineColorBlendAttachmentState (false));

			cfg.AddVertexBinding<PbrModel.Vertex> (0);
			cfg.SetVertexAttributes (0, VkFormat.R32g32b32Sfloat, VkFormat.R32g32b32Sfloat, VkFormat.R32g32Sfloat, VkFormat.R32g32Sfloat);

			using (SpecializationInfo constants = new SpecializationInfo (
						new SpecializationConstant<float> (0, nearPlane),
						new SpecializationConstant<float> (1, farPlane))) {

				cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/GBuffPbr.vert.spv");
				cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/GBuffPbr.frag.spv", constants);

				gBuffPipeline = new GraphicPipeline (cfg);
			}
			cfg.rasterizationState.cullMode = VkCullModeFlags.Front;
			//COMPOSE PIPELINE
			cfg.blendAttachments.Clear ();
			cfg.blendAttachments.Add (new VkPipelineColorBlendAttachmentState (false));
			cfg.ResetShadersAndVerticesInfos ();
			cfg.SubpassIndex = SP_COMPOSE;
			cfg.Layout = gBuffPipeline.Layout;
			cfg.depthStencilState.depthTestEnable = false;
			cfg.depthStencilState.depthWriteEnable = false;
			using (SpecializationInfo constants = new SpecializationInfo (
				new SpecializationConstant<uint> (0, (uint)lights.Length))) {
				cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/FullScreenQuad.vert.spv");
#if WITH_SHADOWS
				cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/compose_with_shadows.frag.spv", constants);
#else
				cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/compose.frag.spv", constants);
#endif
				composePipeline = new GraphicPipeline (cfg);
			}
			//DEBUG DRAW use subpass of compose
			cfg.shaders[1] = new ShaderInfo (VkShaderStageFlags.Fragment, "shaders/show_gbuff.frag.spv");
			cfg.SubpassIndex = SP_COMPOSE;
			debugPipeline = new GraphicPipeline (cfg);
			//TONE MAPPING
			cfg.shaders[1] = new ShaderInfo (VkShaderStageFlags.Fragment, "shaders/tone_mapping.frag.spv");
			cfg.SubpassIndex = SP_TONE_MAPPING;
			toneMappingPipeline = new GraphicPipeline (cfg);

			dsMain = descriptorPool.Allocate (descLayoutMain);
			dsGBuff = descriptorPool.Allocate (descLayoutGBuff);

			envCube = new EnvironmentCube (dsMain, gBuffPipeline.Layout, presentQueue, renderPass);

			matrices.prefilteredCubeMipLevels = envCube.prefilterCube.CreateInfo.mipLevels;

			DescriptorSetWrites dsMainWrite = new DescriptorSetWrites (dsMain, descLayoutMain.Bindings.GetRange (0, 5).ToArray ());
			dsMainWrite.Write (dev, 
				uboMatrices.Descriptor,
				envCube.irradianceCube.Descriptor,
				envCube.prefilterCube.Descriptor,
				envCube.lutBrdf.Descriptor,
				uboLights.Descriptor);

#if WITH_SHADOWS
			dsMainWrite = new DescriptorSetWrites (dsMain, descLayoutMain.Bindings[7]);
			dsMainWrite.Write (dev, shadowMapRenderer.shadowMap.Descriptor);
#endif
		}

		public void LoadModel (Queue transferQ, string path) {
			dev.WaitIdle ();
			model?.Dispose ();
			model = new PbrModel (transferQ, path,
				descLayoutTextures,
				AttachmentType.Color,
				AttachmentType.PhysicalProps,
				AttachmentType.Normal,
				AttachmentType.AmbientOcclusion,
				AttachmentType.Emissive);

			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (dsMain, descLayoutMain.Bindings[6]);
			uboUpdate.Write (dev, model.materialUBO.Descriptor);

			modelAABB = model.DefaultScene.AABB;
		}
		Image uiImage;
		public void WriteUiImgDesciptor (Image uiImage) {
			this.uiImage = uiImage;
			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (dsMain, descLayoutMain.Bindings[5]);
			uboUpdate.Write (dev, uiImage.Descriptor);
		}

		public void buildCommandBuffers (CommandBuffer[] cmds) {
			for (int i = 0; i < swapChain.ImageCount; ++i) {
				cmds[i].Start ();
#if WITH_VKVG
				uiImage.SetLayout (cmds[i], VkImageAspectFlags.Color, VkImageLayout.Undefined, VkImageLayout.ShaderReadOnlyOptimal,
					VkPipelineStageFlags.TopOfPipe, VkPipelineStageFlags.FragmentShader);
#endif
				recordDraw (cmds[i], frameBuffers[i]);
				cmds[i].End ();
			}
		}

		void recordDraw (CommandBuffer cmd, Framebuffer fb) {
			renderPass.Begin (cmd, fb);

			cmd.SetViewport (fb.Width, fb.Height);
			cmd.SetScissor (fb.Width, fb.Height);

			cmd.BindDescriptorSet (gBuffPipeline.Layout, dsMain);

			envCube.RecordDraw (cmd);
			renderPass.BeginSubPass (cmd);

			if (model != null) {
				gBuffPipeline.Bind (cmd);
				model.Bind (cmd);
				model.DrawAll (cmd, gBuffPipeline.Layout);
			}
			renderPass.BeginSubPass (cmd);

			cmd.BindDescriptorSet (composePipeline.Layout, dsGBuff, 2);

			if (currentDebugView == DebugView.none)
				composePipeline.Bind (cmd);
			else {
				debugPipeline.Bind (cmd);
				uint debugValue = (uint)currentDebugView - 1;
				if (currentDebugView == DebugView.shadowMap)
					debugValue += (uint)((lightNumDebug << 8));
				else
					debugValue += (uint)((envCube.debugFace << 8) + (envCube.debugMip << 16));
				cmd.PushConstant (debugPipeline.Layout, VkShaderStageFlags.Fragment, debugValue, (uint)Marshal.SizeOf<Matrix4x4> ());
			}

			cmd.Draw (3, 1, 0, 0);

			renderPass.BeginSubPass (cmd);
			toneMappingPipeline.Bind (cmd);
			cmd.Draw (3, 1, 0, 0);

			renderPass.End (cmd);
		}

		public void MoveLight (Vector4 dir) {
			lights[lightNumDebug].position += dir * lightMoveSpeed;
#if WITH_SHADOWS
			shadowMapRenderer.updateShadowMap = true;
#endif
		}

#region update
		public void UpdateView (Camera camera) {
			camera.AspectRatio = (float)swapChain.Width / swapChain.Height;

			matrices.projection = camera.Projection;
			matrices.view = camera.View;
			matrices.model = camera.Model;


			matrices.camPos = new Vector4 (
				-camera.Position.Z * (float)Math.Sin (camera.Rotation.Y) * (float)Math.Cos (camera.Rotation.X),
				 camera.Position.Z * (float)Math.Sin (camera.Rotation.X),
				 camera.Position.Z * (float)Math.Cos (camera.Rotation.Y) * (float)Math.Cos (camera.Rotation.X),
				 0
			);

			uboMatrices.Update (matrices, (uint)Marshal.SizeOf<Matrices> ());
		}

#endregion

		void createGBuff () {
			gbColorRough?.Dispose ();
			gbEmitMetal?.Dispose ();
			gbN_AO?.Dispose ();
			gbPos?.Dispose ();
			hdrImg?.Dispose ();

			gbColorRough = new Image (dev, swapChain.ColorFormat, VkImageUsageFlags.InputAttachment | VkImageUsageFlags.ColorAttachment | VkImageUsageFlags.TransientAttachment, VkMemoryPropertyFlags.DeviceLocal, swapChain.Width, swapChain.Height, VkImageType.Image2D, NUM_SAMPLES);
			gbEmitMetal = new Image (dev, VkFormat.R8g8b8a8Unorm, VkImageUsageFlags.InputAttachment | VkImageUsageFlags.ColorAttachment | VkImageUsageFlags.TransientAttachment, VkMemoryPropertyFlags.DeviceLocal, swapChain.Width, swapChain.Height, VkImageType.Image2D, NUM_SAMPLES);
			gbN_AO = new Image (dev, MRT_FORMAT, VkImageUsageFlags.InputAttachment | VkImageUsageFlags.ColorAttachment | VkImageUsageFlags.TransientAttachment, VkMemoryPropertyFlags.DeviceLocal, swapChain.Width, swapChain.Height, VkImageType.Image2D, NUM_SAMPLES);
			gbPos = new Image (dev, MRT_FORMAT, VkImageUsageFlags.InputAttachment | VkImageUsageFlags.ColorAttachment | VkImageUsageFlags.TransientAttachment, VkMemoryPropertyFlags.DeviceLocal, swapChain.Width, swapChain.Height, VkImageType.Image2D, NUM_SAMPLES);
			hdrImg = new Image (dev, HDR_FORMAT, VkImageUsageFlags.InputAttachment | VkImageUsageFlags.ColorAttachment | VkImageUsageFlags.TransientAttachment, VkMemoryPropertyFlags.DeviceLocal | VkMemoryPropertyFlags.DeviceLocal, swapChain.Width, swapChain.Height, VkImageType.Image2D, NUM_SAMPLES);

			gbColorRough.CreateView ();
			gbColorRough.CreateSampler ();
			gbColorRough.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
			gbEmitMetal.CreateView ();
			gbEmitMetal.CreateSampler ();
			gbEmitMetal.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
			gbN_AO.CreateView ();
			gbN_AO.CreateSampler ();
			gbN_AO.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
			gbPos.CreateView ();
			gbPos.CreateSampler ();
			gbPos.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
			hdrImg.CreateView ();
			hdrImg.CreateSampler ();
			hdrImg.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;

			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (descLayoutGBuff);
			uboUpdate.Write (dev, dsGBuff,	gbColorRough.Descriptor,
										gbEmitMetal.Descriptor,
										gbN_AO.Descriptor,
										gbPos.Descriptor,
										hdrImg.Descriptor);
			gbColorRough.SetName ("GBuffColorRough");
			gbEmitMetal.SetName ("GBuffEmitMetal");
			gbN_AO.SetName ("GBuffN");
			gbPos.SetName ("GBuffPos");
			hdrImg.SetName ("HDRimg");
		}

		public void Resize () {
			if (frameBuffers != null)
				for (int i = 0; i < swapChain.ImageCount; ++i)
					frameBuffers[i]?.Dispose ();

			createGBuff ();

			frameBuffers = new Framebuffer[swapChain.ImageCount];

			for (int i = 0; i < swapChain.ImageCount; ++i) {
				frameBuffers[i] = new Framebuffer (renderPass, swapChain.Width, swapChain.Height, new Image[] {
					swapChain.images[i], null, gbColorRough, gbEmitMetal, gbN_AO, gbPos, hdrImg});
			}
		}

		public void Dispose () {
			dev.WaitIdle ();
			for (int i = 0; i < swapChain.ImageCount; ++i)
				frameBuffers[i]?.Dispose ();

			gbColorRough.Dispose ();
			gbEmitMetal.Dispose ();
			gbN_AO.Dispose ();
			gbPos.Dispose ();
			hdrImg.Dispose ();

			gBuffPipeline.Dispose ();
			composePipeline.Dispose ();
			toneMappingPipeline.Dispose ();
			debugPipeline.Dispose ();

			descLayoutMain.Dispose ();
			descLayoutTextures.Dispose ();
			descLayoutGBuff.Dispose ();

			uboMatrices.Dispose ();
			uboLights.Dispose ();
			model.Dispose ();
			envCube.Dispose ();

#if WITH_SHADOWS
			shadowMapRenderer.Dispose ();
#endif

			descriptorPool.Dispose ();
		}
	}
}
