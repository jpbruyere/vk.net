using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;
using VK;
using VKE;

namespace PbrSample {
	class Program : VkWindow{	
		static void Main (string[] args) {
			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}

		VkSampleCountFlags samples = VkSampleCountFlags.SampleCount1;

		public struct Matrices {
			public Matrix4x4 projection;
			public Matrix4x4 view;
			public Matrix4x4 model;
			public Vector4 lightPos;
			public float gamma;
			public float exposure;
		}

		public Matrices matrices = new Matrices {
			lightPos = new Vector4 (1.0f, 0.0f, 0.0f, 1.0f),
			gamma = 1.0f,
			exposure = 2.0f,
		};

#if DEBUG
		PipelineStatisticsQueryPool statPool;
		TimestampQueryPool timestampQPool;
		ulong[] results;
#endif

		protected override void configureEnabledFeatures (ref VkPhysicalDeviceFeatures features) {
			base.configureEnabledFeatures (ref features);
#if DEBUG
			features.pipelineStatisticsQuery = true;
#endif
		}

		Program () {
			camera.Model = Matrix4x4.CreateRotationX (Utils.DegreesToRadians (-90)) * Matrix4x4.CreateRotationY (Utils.DegreesToRadians (180));
			camera.SetRotation (-0.1f,-0.4f);
			camera.SetPosition (0, 0, -3);

			init ();

#if DEBUG
			statPool = new PipelineStatisticsQueryPool (dev,
				VkQueryPipelineStatisticFlags.InputAssemblyVertices |
				VkQueryPipelineStatisticFlags.InputAssemblyPrimitives |
				VkQueryPipelineStatisticFlags.ClippingInvocations |
				VkQueryPipelineStatisticFlags.ClippingPrimitives |
				VkQueryPipelineStatisticFlags.FragmentShaderInvocations);

			timestampQPool = new TimestampQueryPool (dev);
#endif
		}

		Framebuffer[] frameBuffers;
		Image gbColorRough, gbEmitMetal, gbN, gbPos;

		DescriptorPool descriptorPool;
		DescriptorSetLayout descLayoutMain, descLayoutModelTextures, descLayoutGBuff;
		DescriptorSet dsMain, dsGBuff;

		Pipeline gBuffPipeline, composePipeline;

		HostBuffer uboMats;

		RenderPass renderPass;

		Model model;
		EnvironmentCube envCube;

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
				new VkDescriptorSetLayoutBinding (3, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler));

			descLayoutModelTextures = new DescriptorSetLayout (dev,
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

			PipelineConfig cfg = PipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, samples);
			cfg.Layout = new PipelineLayout (dev, descLayoutMain, descLayoutModelTextures, descLayoutGBuff);
			cfg.Layout.AddPushConstants (
				new VkPushConstantRange (VkShaderStageFlags.Vertex, (uint)Marshal.SizeOf<Matrix4x4> ()),
				new VkPushConstantRange (VkShaderStageFlags.Fragment, (uint)Marshal.SizeOf<Model.PbrMaterial> (), 64)
			);
			cfg.RenderPass = renderPass;
			cfg.blendAttachments.Add (new VkPipelineColorBlendAttachmentState (false));
			cfg.blendAttachments.Add (new VkPipelineColorBlendAttachmentState (false));
			cfg.blendAttachments.Add (new VkPipelineColorBlendAttachmentState (false));

			cfg.AddVertexBinding<Model.Vertex> (0);
			cfg.SetVertexAttributes (0, VkFormat.R32g32b32Sfloat, VkFormat.R32g32b32Sfloat, VkFormat.R32g32Sfloat);
			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/pbrtest.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/GBuffPbr.frag.spv");

			gBuffPipeline = new Pipeline (cfg);
			cfg.blendAttachments.Clear ();
			cfg.blendAttachments.Add (new VkPipelineColorBlendAttachmentState (false));
			cfg.ResetShadersAndVerticesInfos ();
			cfg.SubpassIndex = 1;
			cfg.Layout = gBuffPipeline.Layout;
			cfg.depthStencilState.depthTestEnable = false;
			cfg.depthStencilState.depthWriteEnable = false;
			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/FullScreenQuad.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/pbrtest.frag.spv");
			composePipeline = new Pipeline (cfg);

			envCube = new EnvironmentCube (presentQueue, renderPass);

			uboMats = new HostBuffer (dev, VkBufferUsageFlags.UniformBuffer, (ulong)Marshal.SizeOf<Matrices> () * 2);
			uboMats.Map ();//permanent map

			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (descLayoutMain);
			uboUpdate.Write (dev, dsMain, uboMats.Descriptor,
				envCube.lutBrdf.Descriptor,
				envCube.irradianceCube.Descriptor,
				envCube.prefilterCube.Descriptor);
			uboMats.Descriptor.offset = (ulong)Marshal.SizeOf<Matrices> ();
			envCube.WriteDesc (uboMats.Descriptor);

			model = new Model (presentQueue, "../data/models/DamagedHelmet/glTF/DamagedHelmet.gltf");
			//model = new Model (presentQueue, "../data/models/chess.gltf");
			//model = new Model (presentQueue, "../data/models/Sponza/glTF/Sponza.gltf");
			//model = new Model (dev, presentQueue, "../data/models/icosphere.gltf");
			//model = new Model (dev, presentQueue, cmdPool, "../data/models/cube.gltf");
			model.WriteMaterialsDescriptorSets (descLayoutModelTextures,
				ShaderBinding.Color,
				ShaderBinding.Normal,
				ShaderBinding.AmbientOcclusion,
				ShaderBinding.MetalRoughness,
				ShaderBinding.Emissive);

		}

		void buildCommandBuffers () {
			for (int i = 0; i < swapChain.ImageCount; ++i) {
				cmds[i]?.Free ();
				cmds[i] = cmdPool.AllocateCommandBuffer ();
				cmds[i].Start ();

#if DEBUG
				statPool.Begin (cmds[i]);
				recordDraw (cmds[i], frameBuffers[i]);
				statPool.End (cmds[i]);
#else
				recordDraw (cmds[i], frameBuffers[i]);
#endif

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
		void updateMatrices () {
			camera.AspectRatio = (float)swapChain.Width / swapChain.Height;

			matrices.projection = camera.Projection;
			matrices.view = camera.View;
			matrices.model = camera.Model;
			uboMats.Update (matrices, (uint)Marshal.SizeOf<Matrices> ());
			matrices.view *= Matrix4x4.CreateTranslation (-matrices.view.Translation);
			matrices.model = Matrix4x4.Identity;
			uboMats.Update (matrices, (uint)Marshal.SizeOf<Matrices> (), (uint)Marshal.SizeOf<Matrices> ());
		}

		public override void UpdateView () {
			updateMatrices ();
			updateViewRequested = false;
		}
		public override void Update () {
#if DEBUG
			results = statPool.GetResults ();
#endif
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
			updateMatrices ();

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
				case Key.F1:
					if (modifiers.HasFlag (Modifier.Shift))
						matrices.exposure -= 0.3f;
					else
						matrices.exposure += 0.3f;
					break;
				case Key.F2:
					if (modifiers.HasFlag (Modifier.Shift))
						matrices.gamma -= 0.1f;
					else
						matrices.gamma += 0.1f;
					break;
				case Key.F3:
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

					gBuffPipeline.Dispose ();
					composePipeline.Dispose ();

					descLayoutMain.Dispose ();
					descLayoutModelTextures.Dispose ();
					descLayoutGBuff.Dispose ();

					descriptorPool.Dispose ();
#if DEBUG
					timestampQPool?.Dispose ();
					statPool?.Dispose ();
#endif
				}
			}

			base.Dispose (disposing);
		}
	}
}
