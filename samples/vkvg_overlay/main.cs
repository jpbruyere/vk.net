using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;
using VKE;
using Vulkan;
using static VKE.Camera;
using Buffer = VKE.Buffer;

namespace ModelSample {
	class Program : VkWindow {
		static void Main (string[] args) {
			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}

#if DEBUG && DEBUG_MARKER
		public override string[] EnabledExtensions =>  new string[] {"VK_KHR_swapchain","VK_EXT_debug_marker"};
#endif
		PipelineStatisticsQueryPool statPool;
		ulong[] results;

		protected override void configureEnabledFeatures (ref VkPhysicalDeviceFeatures features) {
			base.configureEnabledFeatures (ref features);
			features.pipelineStatisticsQuery = true;
		}
		public struct Matrices {
			public Matrix4x4 projection;
			public Matrix4x4 view;
			public Matrix4x4 model;
			public Vector4 lightPos;
			public float gamma;
			public float exposure;
		}

		public Matrices matrices = new Matrices { 
			lightPos = new Vector4 (0.0f, 0.0f, -2.0f, 1.0f),
			gamma = 1.0f,
			exposure = 2.0f,
		};

		HostBuffer uboMats;

		DescriptorPool descriptorPool;
		DescriptorSetLayout descLayoutMatrix;
		DescriptorSetLayout descLayoutTextures;
		DescriptorSet dsMats;

		Pipeline pipeline;
		Pipeline uiPipeline;
		Framebuffer[] frameBuffers;

		Model model;

		Camera2 Camera = new Camera2 (Utils.DegreesToRadians (60f), 1f);

		vkvg.Device vkvgDev;
        vkvg.Surface vkvgSurf;
		Image vkvgImage;

        void vkvgDraw () {

            using (vkvg.Context ctx = new vkvg.Context (vkvgSurf)) {
				ctx.Operator = vkvg.Operator.Clear;
				ctx.Paint ();
				ctx.Operator = vkvg.Operator.Over;

				ctx.LineWidth = 1;
				ctx.SetSource (0.1, 0.1, 0.1, 0.3);
				ctx.Rectangle (5.5, 5.5, 400, 250);
				ctx.FillPreserve ();
				ctx.Flush ();
				ctx.SetSource (0.8, 0.8, 0.8);
				ctx.Stroke ();

				ctx.FontFace = "mono";
				ctx.FontSize = 10;
				int x = 16;
				int y = 40, dy = 16;
				ctx.MoveTo (x, y);
				ctx.ShowText (string.Format ($"fps:     {fps,5} "));
				ctx.MoveTo (x + 200, y - 0.5);
				ctx.Rectangle (x + 200, y - 8.5, 0.1 * fps, 10);
				ctx.SetSource (0.1, 0.9, 0.1);
				ctx.Fill ();
				ctx.SetSource (0.8, 0.8, 0.8);
				y += dy;
				ctx.MoveTo (x, y);
				ctx.ShowText (string.Format ($"Exposure:{matrices.exposure,5} "));
				y += dy;
				ctx.MoveTo (x, y);
				ctx.ShowText (string.Format ($"Gamma:   {matrices.gamma,5} "));
				if (results == null)
					return;

				y += dy*2;
				ctx.MoveTo (x, y);
				ctx.ShowText ("Pipeline Statistics");
				ctx.MoveTo (x-2, 2.5+y);
				ctx.LineTo (x+160, 2.5+y);
				ctx.Stroke ();
				y += 4;
				x += 20;

				for (int i = 0; i < statPool.RequestedStats.Length; i++) {
					y += dy;
					ctx.MoveTo (x, y);
					ctx.ShowText (string.Format ($"{statPool.RequestedStats[i].ToString(),-30} :{results[i],12:0,0} "));
				}
			}
		}

		Program () : base () {
			vkvgDev = new vkvg.Device (instance.Handle, phy.Handle, dev.VkDev.Handle, presentQueue.qFamIndex,
				vkvg.SampleCount.Sample_8, presentQueue.index);
					
			init ();
			//model = new Model (dev, presentQueue, cmdPool, "/mnt/devel/vkChess/data/chess.gltf");
			//model = new Model (dev, presentQueue, cmdPool, "/mnt/devel/tmp/gltf-pbr-datasave/models/chess.gltf");
			model = new Model (dev, presentQueue, cmdPool, "../data/models/DamagedHelmet/glTF/DamagedHelmet.gltf");
			model.WriteMaterialsDescriptorSets (descLayoutTextures,
				ShaderBinding.Color,
				ShaderBinding.Normal,
				ShaderBinding.AmbientOcclusion,
				ShaderBinding.MetalRoughness,
				ShaderBinding.Emissive);
		}

		void init (VkSampleCountFlags samples = VkSampleCountFlags.Count4) { 
			descriptorPool = new DescriptorPool (dev, 2,
				new VkDescriptorPoolSize (VkDescriptorType.UniformBuffer),
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler)
			);

			descLayoutMatrix = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Vertex|VkShaderStageFlags.Fragment, VkDescriptorType.UniformBuffer),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler)
			);

			descLayoutTextures = new DescriptorSetLayout (dev, 
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (2, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (3, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (4, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler)
			);

			dsMats = descriptorPool.Allocate (descLayoutMatrix);

			PipelineConfig cfg = PipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, samples);

			cfg.Layout = new PipelineLayout (dev, descLayoutMatrix, descLayoutTextures);
			cfg.Layout.AddPushConstants (
				new VkPushConstantRange (VkShaderStageFlags.Vertex, (uint)Marshal.SizeOf<Matrix4x4> ()),
				new VkPushConstantRange (VkShaderStageFlags.Fragment, (uint)Marshal.SizeOf<Model.PbrMaterial> (), 64)
			);
			cfg.RenderPass = new RenderPass (dev, swapChain.ColorFormat, dev.GetSuitableDepthFormat (), samples);

			cfg.AddVertexBinding<Model.Vertex> (0);
			cfg.SetVertexAttributes (0, VkFormat.R32g32b32Sfloat, VkFormat.R32g32b32Sfloat, VkFormat.R32g32Sfloat);

			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/pbrtest.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/pbrtest.frag.spv");

			pipeline = new Pipeline (cfg);

			cfg.ResetShadersAndVerticesInfos ();
			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/FullScreenQuad.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/simpletexture.frag.spv");

			cfg.blendAttachments[0] = new VkPipelineColorBlendAttachmentState (true);

			uiPipeline = new Pipeline (cfg);

			uboMats = new HostBuffer (dev, VkBufferUsageFlags.UniformBuffer, (ulong)Marshal.SizeOf<Matrices>());
			uboMats.Map ();//permanent map

			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (dsMats, descLayoutMatrix.Bindings[0]);				
			uboUpdate.Write (dev, uboMats.Descriptor);

#if DEBUG && DEBUG_MARKER
			cfg.Layout.SetName ("Main Pipeline layout");
			uboMats.SetName ("uboMats");
			descriptorPool.SetName ("main pool");
			descLayoutTextures.SetName ("descLayoutTextures");
#endif

			statPool = new PipelineStatisticsQueryPool (dev,
				VkQueryPipelineStatisticFlags.InputAssemblyVertices |
				VkQueryPipelineStatisticFlags.InputAssemblyPrimitives |
				VkQueryPipelineStatisticFlags.ClippingInvocations |
				VkQueryPipelineStatisticFlags.ClippingPrimitives |
				VkQueryPipelineStatisticFlags.FragmentShaderInvocations);

		}

		void buildCommandBuffers () {
			for (int i = 0; i < swapChain.ImageCount; ++i) { 								
                cmds[i]?.Free ();

				cmds[i] = cmdPool.AllocateCommandBuffer ();
				cmds[i].Start ();

				statPool.Begin (cmds[i]);
#if DEBUG && DEBUG_MARKER
				cmds[i].BeginRegion ("draw" + i, 0.5f, 1f, 0f);
				cmds[i].Handle.SetDebugMarkerName (dev, "cmd Draw" + i); 
#endif

				recordDraw (cmds[i], frameBuffers[i]);

#if DEBUG && DEBUG_MARKER
				cmds[i].EndRegion ();
#endif
				statPool.End (cmds[i]);			
				cmds[i].End ();
			}
		} 
		void recordDraw (CommandBuffer cmd, Framebuffer fb) {
#if DEBUG && DEBUG_MARKER
			cmd.BeginRegion ("models", 0.5f, 1f, 0f);
#endif 
			pipeline.RenderPass.Begin (cmd, fb);

			cmd.SetViewport (fb.Width, fb.Height);
			cmd.SetScissor (fb.Width, fb.Height);

			cmd.BindDescriptorSet (pipeline.Layout, dsMats);
			pipeline.Bind (cmd);
			model.Bind (cmd);
			model.DrawAll (cmd, pipeline.Layout);

#if DEBUG && DEBUG_MARKER
			cmd.EndRegion ();
			cmd.BeginRegion ("vkvg", 0.5f, 1f, 0f);
#endif 
			uiPipeline.Bind (cmd);

			vkvgImage.SetLayout (cmd, VkImageAspectFlags.Color, VkImageLayout.ColorAttachmentOptimal, VkImageLayout.ShaderReadOnlyOptimal,
				VkPipelineStageFlags.ColorAttachmentOutput, VkPipelineStageFlags.FragmentShader);

			cmd.Draw (3, 1, 0, 0);

			vkvgImage.SetLayout (cmd, VkImageAspectFlags.Color, VkImageLayout.ShaderReadOnlyOptimal, VkImageLayout.ColorAttachmentOptimal,
				VkPipelineStageFlags.FragmentShader, VkPipelineStageFlags.BottomOfPipe);

			pipeline.RenderPass.End (cmd);
#if DEBUG && DEBUG_MARKER
			cmd.EndRegion ();
#endif
		}
		void recordCopyVkvgSurf (CommandBuffer cmd, VkImage srcImg, VkImage dstImg) {
			Utils.setImageLayout (cmd.Handle, dstImg, VkImageAspectFlags.Color,
                    VkImageLayout.PresentSrcKHR, VkImageLayout.TransferDstOptimal,
                    VkPipelineStageFlags.BottomOfPipe, VkPipelineStageFlags.Transfer);
            Utils.setImageLayout (cmd.Handle, srcImg, VkImageAspectFlags.Color,
                    VkImageLayout.ColorAttachmentOptimal, VkImageLayout.TransferSrcOptimal,
                    VkPipelineStageFlags.ColorAttachmentOutput, VkPipelineStageFlags.Transfer);

			VkImageSubresourceLayers imgSubResLayer = new VkImageSubresourceLayers (VkImageAspectFlags.Color);
			VkImageCopy cregion = new VkImageCopy {
			    srcSubresource = imgSubResLayer,
			    dstSubresource = imgSubResLayer,
			    extent = new VkExtent3D { width = (uint)vkvgSurf.Width, height = (uint)vkvgSurf.Height }
			};
			VulkanNative.vkCmdCopyImage (cmd.Handle, srcImg, VkImageLayout.TransferSrcOptimal,
			    dstImg, VkImageLayout.TransferDstOptimal, 1, ref cregion);

			Utils.setImageLayout (cmd.Handle, dstImg, VkImageAspectFlags.Color,
			    VkImageLayout.TransferDstOptimal, VkImageLayout.PresentSrcKHR,
			    VkPipelineStageFlags.Transfer, VkPipelineStageFlags.BottomOfPipe);
			Utils.setImageLayout (cmd.Handle, srcImg, VkImageAspectFlags.Color,
			    VkImageLayout.TransferSrcOptimal, VkImageLayout.ColorAttachmentOptimal,
			    VkPipelineStageFlags.Transfer, VkPipelineStageFlags.ColorAttachmentOutput);		 
		}

		void updateMatrices () {

			Camera.AspectRatio = (float)swapChain.Width / swapChain.Height;

			matrices.projection = Camera.Projection;
			matrices.view = Camera.View;
			matrices.model = Camera.Model;
			uboMats.Update (matrices, (uint)Marshal.SizeOf<Matrices> ());
		}
			
		public override void UpdateView () {
			updateMatrices ();
			updateViewRequested = false;
		}



		protected override void onMouseMove (double xPos, double yPos) {
			double diffX = lastMouseX - xPos;
			double diffY = lastMouseY - yPos;
			if (MouseButton[0]) {
				Camera.Rotate ((float)-diffX,(float)-diffY);
			} else if (MouseButton[1]) {
				Camera.Zoom ((float)diffY);
			}

			updateViewRequested = true;
		}

		protected override void onKeyDown (Key key, int scanCode, Modifier modifiers) {
			switch (key) {
				case Key.Up:
					Camera.Rotate (0, 0, 1);
					break;
				case Key.Down:
					Camera.Move (0, 0, -1);
					break;
				case Key.Left:
					Camera.Move (1, 0, 0);
					break;
				case Key.Right:
					Camera.Move (-1, 0, 0);
					break;
				case Key.PageUp:
					Camera.Move (0, 1, 0);
					break;
				case Key.PageDown:
					Camera.Move (0, -1, 0);
					break;
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
					if (Camera.Type == CamType.FirstPerson)
						Camera.Type = CamType.LookAt;
					else
						Camera.Type = CamType.FirstPerson;
					break;
				default:
					base.onKeyDown (key, scanCode, modifiers);
					return;
			}
			updateViewRequested = true;
		}


		public override void Update () {
			results = statPool.GetResults ();
			vkvgDraw ();
		}
		protected override void OnResize () {

			vkvgImage?.Dispose ();
			vkvgSurf?.Dispose ();
			vkvgSurf = new vkvg.Surface (vkvgDev, (int)swapChain.Width, (int)swapChain.Height);
			vkvgImage = new Image (dev, new VkImage ((ulong)vkvgSurf.VkImage.ToInt64 ()), VkFormat.R8g8b8a8Unorm,
				VkImageUsageFlags.ColorAttachment, (uint)vkvgSurf.Width, (uint)vkvgSurf.Height);
			vkvgImage.CreateView ();
			vkvgImage.CreateSampler (VkFilter.Nearest,VkFilter.Nearest, VkSamplerMipmapMode.Nearest, VkSamplerAddressMode.ClampToBorder);

			vkvgImage.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (dsMats, descLayoutMatrix.Bindings[1]);				
			uboUpdate.Write (dev, vkvgImage.Descriptor);

			updateMatrices ();

			if (frameBuffers!=null)
				for (int i = 0; i < swapChain.ImageCount; ++i)
					frameBuffers[i]?.Dispose ();

			frameBuffers = new Framebuffer[swapChain.ImageCount];

			for (int i = 0; i < swapChain.ImageCount; ++i) {
				frameBuffers[i] = new Framebuffer (pipeline.RenderPass, swapChain.Width, swapChain.Height,
					(pipeline.Samples == VkSampleCountFlags.Count1) ? new Image[] {
						swapChain.images[i],
						null
					} : new Image[] {
						null,
						null,
						swapChain.images[i]
					});
				frameBuffers[i].SetName ("main FB " + i);

			}

			buildCommandBuffers ();
		}	

		protected override void Dispose (bool disposing) {
			if (disposing) {
				if (!isDisposed) {
					dev.WaitIdle ();
					for (int i = 0; i < swapChain.ImageCount; ++i)
						frameBuffers[i]?.Dispose ();
					model.Dispose ();
					pipeline.Dispose ();
					descLayoutMatrix.Dispose ();
					descLayoutTextures.Dispose ();
					descriptorPool.Dispose ();

					uboMats.Dispose ();
					vkvgSurf?.Dispose ();
					vkvgDev.Dispose ();
				}
			}

			base.Dispose (disposing);
		}
	}
}
