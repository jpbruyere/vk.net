﻿using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;
using VK;
using CVKL;

namespace deferred {
	class Program : VkWindow{	
		static void Main (string[] args) {
			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}

		VkSampleCountFlags samples = VkSampleCountFlags.SampleCount1;

		public struct Matrices {
			public Matrix4x4 projection;
			public Matrix4x4 model;
			public Matrix4x4 view;
			public Vector4 camPos;
			public Vector4 lightDir;
			public float exposure;
			public float gamma;
			public float prefilteredCubeMipLevels;
			public float scaleIBLAmbient;
			public float debugViewInputs;
			public float debugViewEquation;
		}

		public Matrices matrices = new Matrices {
			lightDir = Vector4.Normalize (new Vector4 (0.7f, 0.6f, 0.2f, 0.0f)),
			gamma = 2.2f,
			exposure = 4.5f,
			scaleIBLAmbient = 1f,
			debugViewInputs = 0,
			debugViewEquation = 0
		};

		Program () {
			camera.SetPosition (0, 0, 5);

			init ();

			camera.Model = Matrix4x4.CreateScale (1f / Math.Max (Math.Max (modelAABB.max.X, modelAABB.max.Y), modelAABB.max.Z));
		}

		Framebuffer[] frameBuffers;
		Image gbColorRough, gbEmitMetal, gbN, gbPos;

		DescriptorPool descriptorPool;
		DescriptorSetLayout descLayoutMain, descLayoutTextures, descLayoutGBuff;
		DescriptorSet dsMain, dsGBuff;

		Pipeline gBuffPipeline, composePipeline;

		HostBuffer uboMats;

		RenderPass renderPass;

		PbrModel model;
		EnvironmentCube envCube;

		Vector4 lightPos = new Vector4 (1, 0, 0, 0);
		BoundingBox modelAABB;

		void init () {
			renderPass = new RenderPass (dev);
			renderPass.AddAttachment (swapChain.ColorFormat, VkImageLayout.PresentSrcKHR, VkSampleCountFlags.SampleCount1);
			renderPass.AddAttachment (dev.GetSuitableDepthFormat(), VkImageLayout.DepthStencilAttachmentOptimal, samples);
			renderPass.AddAttachment (VkFormat.R8g8b8a8Unorm, VkImageLayout.ColorAttachmentOptimal);
			renderPass.AddAttachment (VkFormat.R8g8b8a8Unorm, VkImageLayout.ColorAttachmentOptimal);
			renderPass.AddAttachment (VkFormat.R16g16b16a16Sfloat, VkImageLayout.ColorAttachmentOptimal);
			renderPass.AddAttachment (VkFormat.R16g16b16a16Sfloat, VkImageLayout.ColorAttachmentOptimal);

			renderPass.ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0.0f, 0.0f, 0.0f) });
            	renderPass.ClearValues.Add (new VkClearValue { depthStencil = new VkClearDepthStencilValue (1.0f, 0) });
			renderPass.ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0.0f, 0.0f, 0.0f) });
			renderPass.ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0.0f, 0.0f, 0.0f) });
			renderPass.ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0.0f, 0.0f, 0.0f) });
			renderPass.ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0.0f, 0.0f, 0.0f) });

			SubPass[] subpass = { new SubPass (), new SubPass () };
			subpass[0].AddColorReference (	new VkAttachmentReference (2, VkImageLayout.ColorAttachmentOptimal),
									new VkAttachmentReference (3, VkImageLayout.ColorAttachmentOptimal),
									new VkAttachmentReference (4, VkImageLayout.ColorAttachmentOptimal),
									new VkAttachmentReference (5, VkImageLayout.ColorAttachmentOptimal));
			subpass[0].SetDepthReference (1, VkImageLayout.DepthStencilAttachmentOptimal);

			subpass[1].AddColorReference (0, VkImageLayout.ColorAttachmentOptimal);
			subpass[1].AddInputReference (	new VkAttachmentReference (2, VkImageLayout.ShaderReadOnlyOptimal),
									new VkAttachmentReference (3, VkImageLayout.ShaderReadOnlyOptimal),
									new VkAttachmentReference (4, VkImageLayout.ShaderReadOnlyOptimal),
									new VkAttachmentReference (5, VkImageLayout.ShaderReadOnlyOptimal));
			renderPass.AddSubpass (subpass);

			renderPass.AddDependency (Vk.SubpassExternal, 0,
                VkPipelineStageFlags.BottomOfPipe, VkPipelineStageFlags.ColorAttachmentOutput,
                VkAccessFlags.MemoryRead, VkAccessFlags.ColorAttachmentRead | VkAccessFlags.ColorAttachmentWrite);
			renderPass.AddDependency (0, 1,
                VkPipelineStageFlags.ColorAttachmentOutput, VkPipelineStageFlags.FragmentShader,
                VkAccessFlags.ColorAttachmentWrite, VkAccessFlags.ShaderRead);
            	renderPass.AddDependency (1, Vk.SubpassExternal,
                VkPipelineStageFlags.ColorAttachmentOutput, VkPipelineStageFlags.BottomOfPipe,
                VkAccessFlags.ColorAttachmentRead | VkAccessFlags.ColorAttachmentWrite, VkAccessFlags.MemoryRead);

			 
			descriptorPool = new DescriptorPool (dev, 3,
				new VkDescriptorPoolSize (VkDescriptorType.UniformBuffer, 2),
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler, 3),
				new VkDescriptorPoolSize (VkDescriptorType.InputAttachment, 4)
			);

			descLayoutMain = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment, VkDescriptorType.UniformBuffer),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (2, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (3, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (4, VkShaderStageFlags.Fragment, VkDescriptorType.UniformBuffer));

			descLayoutTextures = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (2, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (3, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (4, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler)
			);

			descLayoutGBuff = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.InputAttachment),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.InputAttachment),
				new VkDescriptorSetLayoutBinding (2, VkShaderStageFlags.Fragment, VkDescriptorType.InputAttachment),
				new VkDescriptorSetLayoutBinding (3, VkShaderStageFlags.Fragment, VkDescriptorType.InputAttachment));

			dsMain = descriptorPool.Allocate (descLayoutMain);
			dsGBuff = descriptorPool.Allocate (descLayoutGBuff);

			GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, samples);
			cfg.Layout = new PipelineLayout (dev, descLayoutMain, descLayoutTextures, descLayoutGBuff);
			cfg.Layout.AddPushConstants (
				new VkPushConstantRange (VkShaderStageFlags.Vertex, (uint)Marshal.SizeOf<Matrix4x4> ()),
				new VkPushConstantRange (VkShaderStageFlags.Fragment, sizeof (int), 64)
			);
			cfg.RenderPass = renderPass;
			cfg.blendAttachments.Add (new VkPipelineColorBlendAttachmentState (false));
			cfg.blendAttachments.Add (new VkPipelineColorBlendAttachmentState (false));
			cfg.blendAttachments.Add (new VkPipelineColorBlendAttachmentState (false));

			cfg.AddVertexBinding<PbrModel.Vertex> (0);
			cfg.SetVertexAttributes (0, VkFormat.R32g32b32Sfloat, VkFormat.R32g32b32Sfloat, VkFormat.R32g32Sfloat, VkFormat.R32g32Sfloat);
			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/GBuffPbr.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/GBuffPbr.frag.spv");

			gBuffPipeline = new GraphicPipeline (cfg);

			cfg.blendAttachments.Clear ();
			cfg.blendAttachments.Add (new VkPipelineColorBlendAttachmentState (false));
			cfg.ResetShadersAndVerticesInfos ();
			cfg.SubpassIndex = 1;
			cfg.Layout = gBuffPipeline.Layout;
			cfg.depthStencilState.depthTestEnable = false;
			cfg.depthStencilState.depthWriteEnable = false;
			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/FullScreenQuad.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/compose.frag.spv");

			composePipeline = new GraphicPipeline (cfg);

			envCube = new EnvironmentCube (dsMain, gBuffPipeline.Layout, presentQueue, renderPass);

			matrices.prefilteredCubeMipLevels = envCube.prefilterCube.CreateInfo.mipLevels;
			uboMats = new HostBuffer (dev, VkBufferUsageFlags.UniformBuffer, matrices, true);

			model = new PbrModel (presentQueue, "../data/models/DamagedHelmet/glTF/DamagedHelmet.gltf",
				descLayoutTextures,
				AttachmentType.Color,
				AttachmentType.PhysicalProps,
				AttachmentType.Normal,
				AttachmentType.AmbientOcclusion,
				AttachmentType.Emissive);
			//model = new Model (presentQueue, "../data/models/chess.gltf");
			//model = new Model (presentQueue, "../data/models/Sponza/glTF/Sponza.gltf");
			//model = new Model (dev, presentQueue, "../data/models/icosphere.gltf");
			//model = new Model (dev, presentQueue, cmdPool, "../data/models/cube.gltf");
			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (descLayoutMain);
			uboUpdate.Write (dev, dsMain,
				uboMats.Descriptor,
				envCube.irradianceCube.Descriptor,
				envCube.prefilterCube.Descriptor,
				envCube.lutBrdf.Descriptor,
				model.materialUBO.Descriptor);

			modelAABB = model.DefaultScene.AABB;
		}

		void buildCommandBuffers () {
			for (int i = 0; i < swapChain.ImageCount; ++i) {
				cmds[i]?.Free ();
				cmds[i] = cmdPool.AllocateAndStart ();

				recordDraw (cmds[i], frameBuffers[i]);

				cmds[i].End ();
			}
		}
		void recordDraw (CommandBuffer cmd, Framebuffer fb) {
			renderPass.Begin (cmd, fb);

			cmd.SetViewport (fb.Width, fb.Height);
			cmd.SetScissor (fb.Width, fb.Height);

			cmd.BindDescriptorSet (gBuffPipeline.Layout, dsMain);
			gBuffPipeline.Bind (cmd);
			model.Bind (cmd);
			model.DrawAll (cmd, gBuffPipeline.Layout);

			renderPass.BeginSubPass (cmd);

			cmd.BindDescriptorSet (composePipeline.Layout, dsGBuff, 2);
			composePipeline.Bind (cmd);

			cmd.Draw (3, 1, 0, 0);

			renderPass.End (cmd);
		}

		#region update
		public override void UpdateView () {
			camera.AspectRatio = (float)swapChain.Width / swapChain.Height;

			matrices.lightDir = lightPos;
			matrices.projection = camera.Projection;
			matrices.view = camera.View;
			matrices.model = camera.Model;


			matrices.camPos = new Vector4 (
				-camera.Position.Z * (float)Math.Sin (camera.Rotation.Y) * (float)Math.Cos (camera.Rotation.X),
				 camera.Position.Z * (float)Math.Sin (camera.Rotation.X),
				 camera.Position.Z * (float)Math.Cos (camera.Rotation.Y) * (float)Math.Cos (camera.Rotation.X),
				 0
			);
			//matrices.debugViewInputs = (float)currentDebugView;

			uboMats.Update (matrices, (uint)Marshal.SizeOf<Matrices> ());

			updateViewRequested = false;
		}

		#endregion



		void createGBuff () {
			gbColorRough?.Dispose ();
			gbEmitMetal?.Dispose ();
			gbN?.Dispose ();
			gbPos?.Dispose ();

			gbColorRough = new Image (dev, VkFormat.R8g8b8a8Unorm, VkImageUsageFlags.InputAttachment | VkImageUsageFlags.ColorAttachment, VkMemoryPropertyFlags.DeviceLocal, swapChain.Width, swapChain.Height);
			gbEmitMetal = new Image (dev, VkFormat.R8g8b8a8Unorm, VkImageUsageFlags.InputAttachment | VkImageUsageFlags.ColorAttachment, VkMemoryPropertyFlags.DeviceLocal, swapChain.Width, swapChain.Height);
			gbN = new Image (dev, VkFormat.R16g16b16a16Sfloat, VkImageUsageFlags.InputAttachment | VkImageUsageFlags.ColorAttachment, VkMemoryPropertyFlags.DeviceLocal, swapChain.Width, swapChain.Height);
			gbPos = new Image (dev, VkFormat.R16g16b16a16Sfloat, VkImageUsageFlags.InputAttachment | VkImageUsageFlags.ColorAttachment, VkMemoryPropertyFlags.DeviceLocal, swapChain.Width, swapChain.Height);

			gbColorRough.CreateView ();
			gbColorRough.CreateSampler ();
			gbEmitMetal.CreateView ();
			gbEmitMetal.CreateSampler ();
			gbN.CreateView ();
			gbN.CreateSampler ();
			gbPos.CreateView ();
			gbPos.CreateSampler ();

			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (descLayoutGBuff);
			uboUpdate.Write (dev, dsGBuff,	gbColorRough.Descriptor,
										gbEmitMetal.Descriptor,
										gbN.Descriptor,
										gbPos.Descriptor);
			gbColorRough.SetName ("GBuffColorRough");
			gbEmitMetal.SetName ("GBuffEmitMetal");
			gbN.SetName ("GBuffN");
			gbPos.SetName ("GBuffPos");
		}

		protected override void OnResize () {
			UpdateView ();

			if (frameBuffers != null)
				for (int i = 0; i < swapChain.ImageCount; ++i)
					frameBuffers[i]?.Dispose ();

			createGBuff ();

			frameBuffers = new Framebuffer[swapChain.ImageCount];

			for (int i = 0; i < swapChain.ImageCount; ++i) {
				frameBuffers[i] = new Framebuffer (renderPass, swapChain.Width, swapChain.Height, new Image[] {
					swapChain.images[i], null, gbColorRough, gbEmitMetal, gbN, gbPos});
			}

			buildCommandBuffers ();
		}


		#region Mouse and keyboard
		protected override void onKeyDown (Key key, int scanCode, Modifier modifiers) {
			switch (key) {
				case Key.Up:
					if (modifiers.HasFlag (Modifier.Shift))
						lightPos -= Vector4.UnitZ;
					else
						camera.Move (0, 0, 1);
					break;
				case Key.Down:
					if (modifiers.HasFlag (Modifier.Shift))
						lightPos += Vector4.UnitZ;
					else
						camera.Move (0, 0, -1);
					break;
				case Key.Left:
					if (modifiers.HasFlag (Modifier.Shift))
						lightPos -= Vector4.UnitX;
					else
						camera.Move (1, 0, 0);
					break;
				case Key.Right:
					if (modifiers.HasFlag (Modifier.Shift))
						lightPos += Vector4.UnitX;
					else
						camera.Move (-1, 0, 0);
					break;
				case Key.PageUp:
					if (modifiers.HasFlag (Modifier.Shift))
						lightPos += Vector4.UnitY;
					else
						camera.Move (0, 1, 0);
					break;
				case Key.PageDown:
					if (modifiers.HasFlag (Modifier.Shift))
						lightPos -= Vector4.UnitY;
					else
						camera.Move (0, -1, 0);
					break;
				case Key.F2:
					if (modifiers.HasFlag (Modifier.Shift))
						matrices.exposure -= 0.3f;
					else
						matrices.exposure += 0.3f;
					break;
				case Key.F3:
					if (modifiers.HasFlag (Modifier.Shift))
						matrices.gamma -= 0.1f;
					else
						matrices.gamma += 0.1f;
					break;
				case Key.F4:
					if (camera.Type == Camera.CamType.FirstPerson)
						camera.Type = Camera.CamType.LookAt;
					else
						camera.Type = Camera.CamType.FirstPerson;
					Console.WriteLine ($"camera type = {camera.Type}");
					break;
				default:
					base.onKeyDown (key, scanCode, modifiers);
					return;
			}
			updateViewRequested = true;
		}
#endregion

		protected override void Dispose (bool disposing) {
			if (disposing) {
				if (!isDisposed) {
					dev.WaitIdle ();
					for (int i = 0; i < swapChain.ImageCount; ++i)
						frameBuffers[i]?.Dispose ();

					gbColorRough.Dispose ();
					gbEmitMetal.Dispose ();
					gbN.Dispose ();
					gbPos.Dispose ();

					gBuffPipeline.Dispose ();
					composePipeline.Dispose ();

					descLayoutMain.Dispose ();
					descLayoutTextures.Dispose ();
					descLayoutGBuff.Dispose ();

					uboMats.Dispose ();
					model.Dispose ();
					envCube.Dispose ();

					descriptorPool.Dispose ();
				}
			}

			base.Dispose (disposing);
		}
	}
}
