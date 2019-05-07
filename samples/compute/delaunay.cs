using System;
using Glfw;
using VK;
using CVKL;

namespace delaunay {
	class Program : VkWindow {
		static void Main (string[] args) {
			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}

		Framebuffer[] frameBuffers;
		GraphicPipeline grPipeline;

		Image imgResult;

		Queue computeQ, transferQ;

		GPUBuffer inBuff, outBuff;
		HostBuffer<float> stagingDataBuff;
		DescriptorPool dsPool;
		DescriptorSetLayout dslCompute, dslImage;
		DescriptorSet dsetPing, dsetPong, dsImage;

		ComputePipeline plCompute, plNormalize;

		DebugReport dbgReport;

		const uint imgDim = 256;
		uint zoom = 2;
		int invocationCount = 8;

		uint data_size => imgDim * imgDim * 4;

		float[] datas;

		uint seedCount;

		void addSeed (uint x, uint y) {
			uint ptr = (y * imgDim + x) * 4;
			datas[ptr] = ++seedCount;//seedId
			datas[ptr + 1] = x;
			datas[ptr + 2] = y;
			datas[ptr + 3] = 1;

		}


		public Program () : base () {
			if (Instance.DebugUtils)
				dbgReport = new DebugReport (instance,
					VkDebugReportFlagsEXT.ErrorEXT
					| VkDebugReportFlagsEXT.DebugEXT
					| VkDebugReportFlagsEXT.WarningEXT
					| VkDebugReportFlagsEXT.PerformanceWarningEXT
				
				);
			imgResult = new Image (dev, VkFormat.R32g32b32a32Sfloat, VkImageUsageFlags.TransferDst | VkImageUsageFlags.Sampled, VkMemoryPropertyFlags.DeviceLocal,
				imgDim, imgDim);
			imgResult.CreateView ();
			imgResult.CreateSampler (VkFilter.Nearest, VkFilter.Nearest, VkSamplerMipmapMode.Nearest, VkSamplerAddressMode.ClampToBorder);
			imgResult.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;

			datas = new float[data_size];

			addSeed (imgDim / 2 - 1, imgDim / 2 - 1);


			stagingDataBuff = new HostBuffer<float> (dev, VkBufferUsageFlags.TransferSrc, datas);
			stagingDataBuff.Map ();

			inBuff = new GPUBuffer<float> (dev, VkBufferUsageFlags.StorageBuffer | VkBufferUsageFlags.TransferSrc | VkBufferUsageFlags.TransferDst, (int)data_size);
			outBuff = new GPUBuffer<float> (dev, VkBufferUsageFlags.StorageBuffer | VkBufferUsageFlags.TransferSrc, (int)data_size);

			dsPool = new DescriptorPool (dev, 3,
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler),
				new VkDescriptorPoolSize (VkDescriptorType.StorageBuffer, 4));
			dslImage = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler)
			);
			dslCompute = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Compute, VkDescriptorType.StorageBuffer),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Compute, VkDescriptorType.StorageBuffer)
			);

			GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, VkSampleCountFlags.SampleCount1);

			cfg.Layout = new PipelineLayout (dev, dslImage);
			cfg.RenderPass = new RenderPass (dev, swapChain.ColorFormat, dev.GetSuitableDepthFormat (), VkSampleCountFlags.SampleCount1);
			cfg.RenderPass.ClearValues[0] = new VkClearValue { color = new VkClearColorValue (0.0f, 0.1f, 0.0f) };

			cfg.ResetShadersAndVerticesInfos ();
			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/FullScreenQuad.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/simpletexture.frag.spv");

			cfg.blendAttachments[0] = new VkPipelineColorBlendAttachmentState (true);

			grPipeline = new GraphicPipeline (cfg);

			plCompute = new ComputePipeline (
				new PipelineLayout (dev, new VkPushConstantRange (VkShaderStageFlags.Compute, 2 * sizeof (int)), dslCompute),
				"shaders/computeTest.comp.spv");
			plNormalize = new ComputePipeline (
				plCompute.Layout,
				"shaders/normalize.comp.spv");

			dsImage = dsPool.Allocate (dslImage);
			dsetPing = dsPool.Allocate (dslCompute);
			dsetPong = dsPool.Allocate (dslCompute);

			DescriptorSetWrites dsUpdate = new DescriptorSetWrites (dsetPing, dslCompute);
			dsUpdate.Write (dev, inBuff.Descriptor, outBuff.Descriptor);
			dsUpdate.Write (dev, dsetPong, outBuff.Descriptor, inBuff.Descriptor);
			dsUpdate = new DescriptorSetWrites (dsImage, dslImage);
			dsUpdate.Write (dev, imgResult.Descriptor);

			UpdateFrequency = 5;
		}

		protected override void createQueues () {
			computeQ = new Queue (dev, VkQueueFlags.Compute);
			transferQ = new Queue (dev, VkQueueFlags.Transfer);

			base.createQueues ();
		}

		protected override void OnResize () {

			if (frameBuffers != null)
				for (int i = 0; i < swapChain.ImageCount; ++i)
					frameBuffers[i]?.Dispose ();
			frameBuffers = new Framebuffer[swapChain.ImageCount];

			for (int i = 0; i < swapChain.ImageCount; ++i) {
				frameBuffers[i] = new Framebuffer (grPipeline.RenderPass, swapChain.Width, swapChain.Height,
					(grPipeline.Samples == VkSampleCountFlags.SampleCount1) ? new Image[] {
						swapChain.images[i],
						null
					} : new Image[] {
						null,
						null,
						swapChain.images[i]
					});

				cmds[i] = cmdPool.AllocateCommandBuffer ();
				cmds[i].Start ();

				imgResult.SetLayout (cmds[i], VkImageAspectFlags.Color,
					VkImageLayout.Undefined, VkImageLayout.ShaderReadOnlyOptimal,
					VkPipelineStageFlags.AllCommands, VkPipelineStageFlags.FragmentShader);

				grPipeline.RenderPass.Begin (cmds[i], frameBuffers[i]);

				int xPad = (int)swapChain.Width / 2 - (int)imgDim * (int)zoom / 2;
				int yPad = (int)swapChain.Height / 2- (int)imgDim * (int)zoom / 2;

				cmds[i].SetViewport (imgDim * zoom, imgDim * zoom, xPad, yPad);
				cmds[i].SetScissor (imgDim * zoom, imgDim * zoom, Math.Max (0, xPad), Math.Max (0, yPad));

				cmds[i].BindDescriptorSet (grPipeline.Layout, dsImage);
				cmds[i].BindPipeline (grPipeline);
				cmds[i].Draw (3, 1, 0, 0);

				grPipeline.RenderPass.End (cmds[i]);

				cmds[i].End ();
			}
		}
		bool pong;

		public override void Update () {
			initGpuBuffers ();

			using (CommandPool cmdPoolCompute = new CommandPool (dev, computeQ.qFamIndex)) {

				CommandBuffer cmd = cmdPoolCompute.AllocateAndStart (VkCommandBufferUsageFlags.OneTimeSubmit);

				pong = false;
				uint stepSize = imgDim / 2;

				plCompute.Bind (cmd);
				cmd.PushConstant (plCompute.Layout, VkShaderStageFlags.Compute, imgDim, sizeof(int));

				int pass = 0;
				while (stepSize > 0 && pass < invocationCount) {
					cmd.PushConstant (plCompute.Layout, VkShaderStageFlags.Compute, stepSize);

					if (pong)
						plCompute.BindDescriptorSet (cmd, dsetPong);
					else
						plCompute.BindDescriptorSet (cmd, dsetPing);

					cmd.Dispatch (imgDim, imgDim);

					VkMemoryBarrier memBar = VkMemoryBarrier.New ();
					memBar.srcAccessMask = VkAccessFlags.ShaderWrite;
					memBar.dstAccessMask = VkAccessFlags.ShaderRead;
					Vk.vkCmdPipelineBarrier (cmd.Handle, VkPipelineStageFlags.ComputeShader, VkPipelineStageFlags.ComputeShader, VkDependencyFlags.ByRegion,
						1, ref memBar, 0, IntPtr.Zero, 0, IntPtr.Zero);

					pong = !pong;
					stepSize /= 2;
					pass++;
				}

				plNormalize.Bind (cmd);
				if (pong)
					plNormalize.BindDescriptorSet (cmd, dsetPong);
				else
					plNormalize.BindDescriptorSet (cmd, dsetPing);
				cmd.Dispatch (imgDim, imgDim);
				pong = !pong;

				cmd.End ();

				computeQ.Submit (cmd);
				computeQ.WaitIdle ();
			}

			printResults ();
		}

		protected override void onMouseButtonDown (MouseButton button) {
			int xPad = (int)swapChain.Width / 2 - (int)imgDim * (int)zoom / 2;
			int yPad = (int)swapChain.Height / 2 - (int)imgDim * (int)zoom / 2;

			int localX = (int)((lastMouseX - xPad) / zoom);
			int localY = (int)((lastMouseY - yPad) / zoom);

			if (localX < 0 || localY < 0 || localX >= imgDim || localY >= imgDim)
				base.onMouseButtonDown (button);
			else {
				addSeed ((uint)localX, (uint)localY);
				stagingDataBuff.Update (datas);
			}
		}
		protected override void onKeyDown (Key key, int scanCode, Modifier modifiers) {
			switch (key) {
				case Key.Delete:
					datas = new float[data_size];
					stagingDataBuff.Update (datas);
					seedCount = 0;
					break;
				case Key.KeypadAdd:
					invocationCount++;
					break;
				case Key.KeypadSubtract:
					if (invocationCount>0)
						invocationCount--;
					break;
				default:
					base.onKeyDown (key, scanCode, modifiers);
					break;
			}
			Console.WriteLine ($"break after {invocationCount} step");
		}

		void printResults () {
			dev.WaitIdle ();
			using (CommandPool cmdPoolTransfer = new CommandPool (dev, transferQ.qFamIndex)) {

				CommandBuffer cmd = cmdPoolTransfer.AllocateAndStart (VkCommandBufferUsageFlags.OneTimeSubmit);

				imgResult.SetLayout (cmd, VkImageAspectFlags.Color,
					VkImageLayout.ShaderReadOnlyOptimal, VkImageLayout.TransferDstOptimal,
					VkPipelineStageFlags.FragmentShader, VkPipelineStageFlags.Transfer);

				if (pong)
					outBuff.CopyTo (cmd, imgResult, VkImageLayout.ShaderReadOnlyOptimal);
				else
					inBuff.CopyTo (cmd, imgResult, VkImageLayout.ShaderReadOnlyOptimal);

				cmd.End ();

				transferQ.Submit (cmd);
				transferQ.WaitIdle ();
			}
		}

		void initGpuBuffers () {
			using (CommandPool staggingCmdPool = new CommandPool (dev, transferQ.qFamIndex)) {
				CommandBuffer cmd = staggingCmdPool.AllocateAndStart (VkCommandBufferUsageFlags.OneTimeSubmit);

				stagingDataBuff.CopyTo (cmd, inBuff);

				transferQ.EndSubmitAndWait (cmd);
			}
		}

		protected override void Dispose (bool disposing) {
			if (disposing) {
				if (!isDisposed) {
					dev.WaitIdle ();

					for (int i = 0; i < swapChain.ImageCount; ++i)
						frameBuffers[i]?.Dispose ();

					grPipeline.Dispose ();
					plCompute.Dispose ();
					plNormalize.Dispose ();

					dslCompute.Dispose ();
					dslImage.Dispose ();

					dsPool.Dispose ();

					inBuff.Dispose ();
					outBuff.Dispose ();
					stagingDataBuff.Dispose ();

					imgResult.Dispose ();

					dbgReport?.Dispose ();
				}
			}

			base.Dispose (disposing);
		}


	}
}
