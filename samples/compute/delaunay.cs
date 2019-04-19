using System;
using Glfw;
using VK;
using VKE;

namespace delaunay {
	class Program : VkWindow {
		static void Main (string[] args) {
			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}
		VkSampleCountFlags samples = VkSampleCountFlags.SampleCount1;

		DescriptorPool descriptorPool;
		DescriptorSetLayout descLayout;
		DescriptorSet dsImgResult;
		Image imgResult;

		GraphicPipeline uiPipeline;
		Framebuffer[] frameBuffers;

		ComputePipeline delaunayPL, normalizePL;
		DescriptorSetLayout dslCompute;
		DescriptorSet dsDelaunayBufsPing, dsDelaunayBufsPong;
		HostBuffer<float> delaunayBufIn;
		GPUBuffer<float> delaunayBufOut;
		Queue computeQ;
		CommandPool cmdPoolCompute;

		DebugReport dbgReport;

		protected override void createQueues () {

			computeQ = new Queue (dev, VkQueueFlags.Compute);

			base.createQueues ();
		}


		Program () : base (true, "delaunay", 600,600) {
#if DEBUG
			dbgReport = new DebugReport (instance,
				VkDebugReportFlagsEXT.ErrorEXT
				| VkDebugReportFlagsEXT.DebugEXT
				| VkDebugReportFlagsEXT.WarningEXT
				| VkDebugReportFlagsEXT.PerformanceWarningEXT

			);
#endif

			UpdateFrequency = 1;
			descriptorPool = new DescriptorPool (dev, 3,
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler),
				new VkDescriptorPoolSize (VkDescriptorType.StorageBuffer,4)
			);

			descLayout = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler)
			);

			dsImgResult = descriptorPool.Allocate (descLayout);

			GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, samples);

			cfg.Layout = new PipelineLayout (dev, descLayout);
			cfg.RenderPass = new RenderPass (dev, swapChain.ColorFormat, dev.GetSuitableDepthFormat (), samples);

			cfg.ResetShadersAndVerticesInfos ();
			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/FullScreenQuad.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/simpletexture.frag.spv");

			cfg.blendAttachments[0] = new VkPipelineColorBlendAttachmentState (true);

			uiPipeline = new GraphicPipeline (cfg);

			addSeed (7, 7);
			addSeed (11, 9);

			initComputePipeline ();

			imgResult = new Image (dev, VkFormat.R32g32b32a32Sfloat, VkImageUsageFlags.TransferDst | VkImageUsageFlags.Sampled, VkMemoryPropertyFlags.DeviceLocal, imgDim, imgDim);				
			imgResult.CreateView ();
			imgResult.CreateSampler (VkFilter.Nearest, VkFilter.Nearest, VkSamplerMipmapMode.Nearest, VkSamplerAddressMode.ClampToBorder);
			imgResult.SetName ("imgResult");
			imgResult.Descriptor.imageView.SetDebugMarkerName (dev, "imgViewResult");
			imgResult.Descriptor.sampler.SetDebugMarkerName (dev, "samplerResult");

			imgResult.Descriptor.imageLayout = VkImageLayout.General;
			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (dsImgResult, descLayout);
			uboUpdate.Write (dev, imgResult.Descriptor);

			performCompute ();
		}
		void initComputePipeline () {
			delaunayBufIn = new HostBuffer<float> (dev, VkBufferUsageFlags.TransferSrc | VkBufferUsageFlags.TransferDst | VkBufferUsageFlags.StorageBuffer, imgData);
			delaunayBufIn.SetName ("buffIn");
			delaunayBufIn.Map ();

			delaunayBufOut = new GPUBuffer<float> (dev, VkBufferUsageFlags.StorageBuffer | VkBufferUsageFlags.TransferSrc, (int)(imgDim * imgDim * 4));
			delaunayBufIn.SetName ("buffOut");

			cmdPoolCompute = new CommandPool (dev, computeQ.qFamIndex);
			dslCompute = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Compute, VkDescriptorType.StorageBuffer),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Compute, VkDescriptorType.StorageBuffer));

			dsDelaunayBufsPing = descriptorPool.Allocate (dslCompute);
			dsDelaunayBufsPong = descriptorPool.Allocate (dslCompute);

			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (dsDelaunayBufsPing, dslCompute);
			uboUpdate.Write (dev, delaunayBufIn.Descriptor, delaunayBufOut.Descriptor);
			uboUpdate.Write (dev, dsDelaunayBufsPong, delaunayBufOut.Descriptor, delaunayBufIn.Descriptor);

			delaunayPL = new ComputePipeline (
				new PipelineLayout (dev, new VkPushConstantRange (VkShaderStageFlags.Compute, 2 * sizeof (int)), dslCompute),
				"shaders/test.comp.spv");
			delaunayPL.Handle.SetDebugMarkerName (dev, "delaunayPL");
			normalizePL = new ComputePipeline (
				delaunayPL.Layout,
				"shaders/normalize.comp.spv");
			normalizePL.Handle.SetDebugMarkerName (dev, "normalizePL");
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

			const int zoom = 6;
			const int shift = 200;

			cmd.SetViewport (imgDim * zoom + shift, imgDim * zoom + shift);
			cmd.SetScissor (imgDim * zoom + shift, imgDim * zoom + shift);

			uiPipeline.Bind (cmd);
			cmd.BindDescriptorSet (uiPipeline.Layout, dsImgResult);

			cmd.Draw (3, 1, 0, 0);

			uiPipeline.RenderPass.End (cmd);
		}

		const uint imgDim = 16;
		float[] imgData = new float[imgDim * imgDim * 4];
		uint seedCount;

		void addSeed (uint x, uint y) {
			uint ptr = (y * imgDim + x) * 4;
			seedCount++;
			imgData[ptr] = (float)seedCount;//seedId
			imgData[ptr+1] = (float)x;
			imgData[ptr+2] = (float)y;
			imgData[ptr+3] = 1;

		}
		void setPixel (uint x, uint y, float r, float g, float b, float a) {
			uint ptr = (y * imgDim + x) * 4;
			imgData[ptr] = r;
			imgData[ptr + 1] = g;
			imgData[ptr + 2] = b;
			imgData[ptr + 3] = a;
			seedCount++;
		}

		public override void Update () {
			delaunayBufIn.Update (imgData);
			dev.WaitIdle ();

			performCompute ();

			copyResultToImage ();
		}

		void copyResultToImage () {
			CommandBuffer cmd = cmdPool.AllocateAndStart (VkCommandBufferUsageFlags.OneTimeSubmit);
			if (odd)
				delaunayBufOut.CopyTo (cmd, imgResult);
			else
				delaunayBufIn.CopyTo (cmd, imgResult);

			cmd.End ();
			presentQueue.Submit (cmd);
			presentQueue.WaitIdle ();
		}

		int passBreak = 1;
		bool odd = false;
		void performCompute () {		

			CommandBuffer cmd = cmdPoolCompute.AllocateAndStart (VkCommandBufferUsageFlags.OneTimeSubmit);
			delaunayPL.Bind (cmd);

			uint stepSize = imgDim / 2;
			cmd.PushConstant (delaunayPL.Layout, VkShaderStageFlags.Compute, imgDim, sizeof(int));
			int pass = 0;
			odd = false;
			while (stepSize > 0 && ++pass<passBreak) {
				cmd.PushConstant (delaunayPL.Layout, VkShaderStageFlags.Compute, stepSize);

				if (odd)
					delaunayPL.BindDescriptorSet (cmd, dsDelaunayBufsPong);
				else
					delaunayPL.BindDescriptorSet (cmd, dsDelaunayBufsPing);
					
				cmd.Dispatch (imgDim, imgDim);

				stepSize /= 2;
				odd = !odd;

				VkMemoryBarrier memBar = VkMemoryBarrier.New ();
				memBar.srcAccessMask = VkAccessFlags.ShaderWrite;
				memBar.dstAccessMask = VkAccessFlags.ShaderRead;
				Vk.vkCmdPipelineBarrier (cmd.Handle, VkPipelineStageFlags.ComputeShader, VkPipelineStageFlags.ComputeShader, VkDependencyFlags.ByRegion,
					1, ref memBar, 0, IntPtr.Zero, 0, IntPtr.Zero);
			}


			if (odd)
				delaunayPL.BindDescriptorSet (cmd, dsDelaunayBufsPong);
			else
				delaunayPL.BindDescriptorSet (cmd, dsDelaunayBufsPing);

			normalizePL.Bind (cmd);
			cmd.Dispatch (imgDim, imgDim);

			cmd.End ();

			computeQ.Submit (cmd);
			computeQ.WaitIdle ();

		}

		protected override void onKeyDown (Key key, int scanCode, Modifier modifiers) {
			switch (key) {
				case Key.KeypadAdd:
					passBreak++;
					break;
				case Key.KeypadSubtract:
					passBreak--;
					break;
				default:
					base.onKeyDown (key, scanCode, modifiers);
					break;
			}
			Console.WriteLine ($"break after {passBreak} step");
		}

		protected override void OnResize () {


			if (frameBuffers != null)
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

					delaunayPL.Dispose ();
					normalizePL.Dispose ();
					dslCompute.Dispose ();

					uiPipeline.Dispose ();

					descLayout.Dispose ();
					descriptorPool.Dispose ();

					delaunayBufIn.Dispose ();
					delaunayBufOut.Dispose ();
					imgResult.Dispose ();

					cmdPoolCompute.Dispose ();

					dbgReport.Dispose ();
				}
			}

			base.Dispose (disposing);
		}
	}
}
