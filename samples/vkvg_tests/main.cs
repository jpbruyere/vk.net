using System;
using System.IO;
using System.Threading;
using CVKL;
using VK;

namespace vkvg_test {
	class Program : VkWindow {
		static void Main (string[] args) {
#if DEBUG
			Instance.VALIDATION = true;
			Instance.DEBUG_UTILS = true;
			Instance.RENDER_DOC_CAPTURE = false;
#endif
			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}

		DescriptorPool descriptorPool;
		DescriptorSetLayout descLayout;
		DescriptorSet dsVkvg;

		GraphicPipeline uiPipeline;
		Framebuffer[] frameBuffers;

		#region vkvg tests
		vkvg.Device vkvgDev;
        vkvg.Surface vkvgSurf;
		Image vkvgImage;

		void clearAndClip (vkvg.Context ctx) {
			ctx.ClipPreserve ();
			ctx.Operator = vkvg.Operator.Clear;
			ctx.Fill ();
			ctx.Operator = vkvg.Operator.Over;
		}
		void vkvgDraw () {		
			using (vkvg.Context ctx = new vkvg.Context (vkvgSurf)) {
				ctx.SetSource (1.0, 1.0, 1.0);
				ctx.Paint ();
				ctx.Translate (100, 100);
				ctx.Scale (3, 3);
				ctx.SetSource (0.1, 0.1, 0.1);
				ctx.Arc (100, 100, 10.0, 0, Math.PI * 2.0);
				ctx.LineWidth = 1.0;
				ctx.Stroke ();
			}

		}
		//void vkvgDrawSVG () {
		//	using (vkvg.Surface svgSurf = new vkvg.Surface (vkvgDev, 300, 300)) {
		//		using (vkvg.Context ctx = new vkvg.Context (svgSurf)) {
		//			IntPtr nsvg = IntPtr.Zero;

		//			using (Stream nsvgStream = Crow.Interface.StaticGetStreamFromPath ("../../../samples/data/tiger.svg")) {
		//				using (StreamReader sr = new StreamReader (nsvgStream)) {
		//					nsvg = vkvgDev.LoadSvgFragment (sr.ReadToEnd ());
		//				}
		//			}


		//			ctx.SetSource (0.8f, 0.8f, 0.8f);
		//			ctx.Paint ();

		//			ctx.Scale (0.2f, 0.2f);
		//			ctx.RenderSvg (nsvg, null);

		//			vkvgDev.DestroySvg (nsvg);
		//		}

		//		//svgSurf.WriteToPng ("/mnt/data/test.png");

		//		using (vkvg.Context ctx = new vkvg.Context (vkvgSurf)) {
		//			ctx.SetSourceSurface (svgSurf, 0, 0);
		//			ctx.Paint ();
		//		}
		//	}
		//}
		#region fps print
		void vkvgDraw1 () {

			using (vkvg.Context ctx = new vkvg.Context (vkvgSurf)) {
				//ctx.SetSource (1.0, 0.1, 0.1, 0.2);
				//ctx.Paint ();

				//ctx.Rectangle (50, 50, 200, 200);
				//ctx.Rectangle (50, 50, 250, 250);
				//clearAndClip (ctx);
				////ctx.Rectangle (60, 60, 200, 200);
				////clearAndClip (ctx);

				//ctx.LineWidth = 1;
				//ctx.SetSource (1.0, 0.1, 0.1, 0.2);
				//ctx.Rectangle (5.5, 5.5, 400, 250);
				//ctx.FillPreserve ();
				//ctx.Flush ();
				ctx.SetSource (0.8, 0.8, 0.8);
				//ctx.Stroke ();

				ctx.FontFace = "mono";
				ctx.FontSize = 8;
				int x = 10;
				int y = 10, dy = 14;
				for (int j = 0; j < 10; j++) {

					for (int i = 0; i < 50; i++) {
						ctx.Save ();
						string text = string.Format ($"fps: {fps,5}");
						vkvg.TextExtents te = ctx.TextExtents (text);
						vkvg.FontExtents fe = ctx.FontExtents;

						ctx.Rectangle (x, y, te.XAdvance + 1.0f, fe.Height);
						clearAndClip (ctx);
						ctx.SetSource (0.1, 0.2, 0.8);
						ctx.Fill ();
						ctx.SetSource (0.8, 0.8, 0.8);
						ctx.MoveTo (x, (float)y+fe.Ascent);
						ctx.ShowText (text);
						y += dy;
						ctx.Restore ();
					}
					ctx.Flush ();
					x += 100;
					y = 10;

				}

			}
		}
		#endregion

		#endregion

		bool recreateSurfaceStatus = true;

		void recreateSurface () {
			vkvgImage?.Dispose ();
			vkvgSurf?.Dispose ();
			vkvgSurf = new vkvg.Surface (vkvgDev, (int)swapChain.Width, (int)swapChain.Height);
			vkvgSurf.Clear ();
			vkvgImage = new Image (dev, new VkImage ((ulong)vkvgSurf.VkImage.ToInt64 ()), VkFormat.B8g8r8a8Unorm,
				VkImageUsageFlags.ColorAttachment, (uint)vkvgSurf.Width, (uint)vkvgSurf.Height);
			vkvgImage.CreateView (VkImageViewType.ImageView2D, VkImageAspectFlags.Color);
			vkvgImage.CreateSampler (VkFilter.Nearest, VkFilter.Nearest, VkSamplerMipmapMode.Nearest, VkSamplerAddressMode.ClampToBorder);
			recreateSurfaceStatus = false;
		}

		void uiThreadFunc () {
			vkvgDev = new vkvg.Device (instance.Handle, phy.Handle, dev.Handle, presentQueue.qFamIndex,
				vkvg.SampleCount.Sample_8, presentQueue.index);

			while (true) {
				if (recreateSurfaceStatus == true)
					recreateSurface ();
				lock (Qmutex) {
					vkvgDraw ();
				}
				Thread.Sleep (10);
			}
		}

		Program () : base () {
			Thread uiThread = new Thread (uiThreadFunc);
			uiThread.IsBackground = true;
			uiThread.Start ();
					
			init ();

			UpdateFrequency = 5;
		}
		object Qmutex = new object ();

		protected override void render () {
			int idx = swapChain.GetNextImage ();
			if (idx < 0) {
				OnResize ();
				return;
			}

			lock (Qmutex) {
				presentQueue.Submit (cmds[idx], swapChain.presentComplete, drawComplete[idx]);
				presentQueue.Present (swapChain, drawComplete[idx]);

				presentQueue.WaitIdle ();
			}
		}

		void init (VkSampleCountFlags samples = VkSampleCountFlags.SampleCount4) { 
			descriptorPool = new DescriptorPool (dev, 2,
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler)
			);

			descLayout = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler)
			);				

			GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, samples);

			cfg.Layout = new PipelineLayout (dev, descLayout);
			cfg.RenderPass = new RenderPass (dev, swapChain.ColorFormat, dev.GetSuitableDepthFormat (), samples);

			cfg.ResetShadersAndVerticesInfos ();
			cfg.AddShader (VkShaderStageFlags.Vertex, "#vkvg_tests.FullScreenQuad.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "#vkvg_tests.simpletexture.frag.spv");

			cfg.blendAttachments[0] = new VkPipelineColorBlendAttachmentState (true);

			uiPipeline = new GraphicPipeline (cfg);

			dsVkvg = descriptorPool.Allocate (descLayout);
		}

		void buildCommandBuffers () {
			for (int i = 0; i < swapChain.ImageCount; ++i) { 								
                cmds[i]?.Free ();

				cmds[i] = cmdPool.AllocateCommandBuffer ();
				cmds[i].Start ();

				recordDraw (cmds[i], frameBuffers[i]);
				cmds[i].End ();
			}
		} 
		void recordDraw (CommandBuffer cmd, Framebuffer fb) {

			uiPipeline.RenderPass.Begin (cmd, fb);

			cmd.SetViewport (fb.Width, fb.Height);
			cmd.SetScissor (fb.Width, fb.Height);

			uiPipeline.Bind (cmd);
			cmd.BindDescriptorSet (uiPipeline.Layout, dsVkvg);

			vkvgImage.SetLayout (cmd, VkImageAspectFlags.Color, VkImageLayout.ColorAttachmentOptimal, VkImageLayout.ShaderReadOnlyOptimal,
				VkPipelineStageFlags.ColorAttachmentOutput, VkPipelineStageFlags.FragmentShader);

			cmd.Draw (3, 1, 0, 0);

			vkvgImage.SetLayout (cmd, VkImageAspectFlags.Color, VkImageLayout.ShaderReadOnlyOptimal, VkImageLayout.ColorAttachmentOptimal,
				VkPipelineStageFlags.FragmentShader, VkPipelineStageFlags.BottomOfPipe);


			uiPipeline.RenderPass.End (cmd);
		}

		public override void Update () {
		
		}
		protected override void OnResize () {
			dev.WaitIdle ();
			recreateSurfaceStatus = true;
			while (recreateSurfaceStatus)
				Thread.Sleep (1);

			vkvgImage.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (dsVkvg, descLayout);				
			uboUpdate.Write (dev, vkvgImage.Descriptor);

			if (frameBuffers!=null)
				for (int i = 0; i < swapChain.ImageCount; ++i)
					frameBuffers[i]?.Dispose ();

			frameBuffers = new Framebuffer[swapChain.ImageCount];

			for (int i = 0; i < swapChain.ImageCount; ++i) {
				frameBuffers[i] = new Framebuffer (uiPipeline.RenderPass, swapChain.Width, swapChain.Height,
					(uiPipeline.Samples == VkSampleCountFlags.SampleCount1) ? new Image[] {
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

					uiPipeline.Dispose ();

					descLayout.Dispose ();
					descriptorPool.Dispose ();
					vkvgImage?.Dispose ();
					vkvgSurf?.Dispose ();
					vkvgDev.Dispose ();
				}
			}

			base.Dispose (disposing);
		}
	}
}
