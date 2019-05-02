using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;
using VK;
using CVKL;

namespace triangulation {
	class Program : VkWindow {
		static void Main (string[] args) {
			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}

		Framebuffer[] frameBuffers;
		GraphicPipeline grPipeline, trianglesPipeline;

		Image imgResult;

		Queue computeQ, transferQ;

		GPUBuffer inBuff, outBuff;
		HostBuffer<Vector2> staggingVBO;
		DescriptorPool dsPool;
		DescriptorSetLayout dslCompute, dslImage, dslVAO;
		DescriptorSet dsPing, dsPong, dsImage, dsVAO;

		ComputePipeline plCompute, plNormalize, plInit;

		DebugReport dbgReport;


		GPUBuffer<Vector2> vbo;
		GPUBuffer<uint> ibo;

		uint zoom = 2;

		const int MAX_VERTICES = 128;
		const uint IMG_DIM = 256;

		int invocationCount = 8;

		uint data_size => IMG_DIM * IMG_DIM * 4;

		Vector2[] points = new Vector2[MAX_VERTICES];
		uint pointCount;
		bool clear = true;//if true, inBuff will be fill with zeros

		bool pong;//ping-pong between buffers

		void addPoint (uint x, uint y) {
			points[pointCount] = new Vector2 (x, y);
			pointCount++;
			staggingVBO.Update (points, pointCount * (ulong)Marshal.SizeOf<Vector2> ());


		}
		void clearPoints () {
			pointCount = 0;
			clear = true;
		}

		public Program () : base () {
#if DEBUG
			dbgReport = new DebugReport (instance,
				VkDebugReportFlagsEXT.ErrorEXT
				| VkDebugReportFlagsEXT.DebugEXT
				| VkDebugReportFlagsEXT.WarningEXT
				| VkDebugReportFlagsEXT.PerformanceWarningEXT
			
			);
#endif
			imgResult = new Image (dev, VkFormat.R32g32b32a32Sfloat, VkImageUsageFlags.TransferDst | VkImageUsageFlags.Sampled, VkMemoryPropertyFlags.DeviceLocal,
				IMG_DIM, IMG_DIM);
			imgResult.CreateView ();
			imgResult.CreateSampler (VkFilter.Nearest, VkFilter.Nearest, VkSamplerMipmapMode.Nearest, VkSamplerAddressMode.ClampToBorder);
			imgResult.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;


			staggingVBO = new HostBuffer<Vector2> (dev, VkBufferUsageFlags.TransferSrc, MAX_VERTICES);
			staggingVBO.Map ();

			vbo = new GPUBuffer<Vector2> (dev, VkBufferUsageFlags.VertexBuffer | VkBufferUsageFlags.StorageBuffer | VkBufferUsageFlags.TransferDst, MAX_VERTICES);
			ibo = new GPUBuffer<uint> (dev, VkBufferUsageFlags.IndexBuffer | VkBufferUsageFlags.StorageBuffer, MAX_VERTICES * 3);

			inBuff = new GPUBuffer<float> (dev, VkBufferUsageFlags.StorageBuffer | VkBufferUsageFlags.TransferSrc | VkBufferUsageFlags.TransferDst, (int)data_size);
			outBuff = new GPUBuffer<float> (dev, VkBufferUsageFlags.StorageBuffer | VkBufferUsageFlags.TransferSrc | VkBufferUsageFlags.TransferDst, (int)data_size);

			dsPool = new DescriptorPool (dev, 4,
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler),
				new VkDescriptorPoolSize (VkDescriptorType.StorageBuffer, 6));
			dslImage = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler)
			);
			dslCompute = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Compute, VkDescriptorType.StorageBuffer),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Compute, VkDescriptorType.StorageBuffer)
			);
			dslVAO = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Compute, VkDescriptorType.StorageBuffer),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Compute, VkDescriptorType.StorageBuffer)
			);

			plInit = new ComputePipeline (
				new PipelineLayout (dev, new VkPushConstantRange (VkShaderStageFlags.Compute, 3 * sizeof (int)), dslCompute, dslVAO),
				"shaders/init.comp.spv");
			plCompute = new ComputePipeline (plInit.Layout, "shaders/computeTest.comp.spv");
			plNormalize = new ComputePipeline (plInit.Layout, "shaders/normalize.comp.spv");

			GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, VkSampleCountFlags.SampleCount1);

			cfg.Layout = new PipelineLayout (dev, dslImage);
			cfg.RenderPass = new RenderPass (dev, swapChain.ColorFormat, dev.GetSuitableDepthFormat (), VkSampleCountFlags.SampleCount1);
			cfg.RenderPass.ClearValues[0] = new VkClearValue { color = new VkClearColorValue (0.1f, 0.1f, 0.1f) };
			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/FullScreenQuad.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/simpletexture.frag.spv");

			cfg.blendAttachments[0] = new VkPipelineColorBlendAttachmentState (true);

			grPipeline = new GraphicPipeline (cfg);

			cfg.ResetShadersAndVerticesInfos ();
			cfg.Layout = new PipelineLayout (dev, new VkPushConstantRange (VkShaderStageFlags.Vertex, 4 * sizeof (int)));
			cfg.inputAssemblyState.topology = VkPrimitiveTopology.LineStrip;
			cfg.AddVertexBinding<Vector2> (0);
			cfg.SetVertexAttributes (0, VkFormat.R32g32Sfloat);
			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/triangle.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/triangle.frag.spv");

			trianglesPipeline = new GraphicPipeline (cfg);

			dsImage = dsPool.Allocate (dslImage);
			dsPing = dsPool.Allocate (dslCompute);
			dsPong = dsPool.Allocate (dslCompute);
			dsVAO = dsPool.Allocate (dslCompute);


			DescriptorSetWrites dsUpdate = new DescriptorSetWrites (dsPing, dslCompute);
			dsUpdate.Write (dev, inBuff.Descriptor, outBuff.Descriptor);
			dsUpdate.Write (dev, dsPong, outBuff.Descriptor, inBuff.Descriptor);
			dsUpdate = new DescriptorSetWrites (dsImage, dslImage);
			dsUpdate.Write (dev, imgResult.Descriptor);
			dsUpdate = new DescriptorSetWrites (dsVAO, dslVAO);
			dsUpdate.Write (dev, vbo.Descriptor, ibo.Descriptor);

			UpdateFrequency = 5;

			addPoint (IMG_DIM / 2 - 1, IMG_DIM / 2 - 1);
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

				int xPad = (int)swapChain.Width / 2 - (int)IMG_DIM * (int)zoom / 2;
				int yPad = (int)swapChain.Height / 2- (int)IMG_DIM * (int)zoom / 2;

				cmds[i].SetViewport (IMG_DIM * zoom, IMG_DIM * zoom, xPad, yPad);
				cmds[i].SetScissor (IMG_DIM * zoom, IMG_DIM * zoom, Math.Max (0, xPad), Math.Max (0, yPad));

				cmds[i].BindDescriptorSet (grPipeline.Layout, dsImage);
				cmds[i].BindPipeline (grPipeline);
				cmds[i].Draw (3, 1, 0, 0);

				trianglesPipeline.Bind (cmds[i]);
				cmds[i].PushConstant (trianglesPipeline.Layout, VkShaderStageFlags.Vertex, IMG_DIM);
				cmds[i].PushConstant (trianglesPipeline.Layout, VkShaderStageFlags.Vertex, xPad, sizeof(int));
				cmds[i].PushConstant (trianglesPipeline.Layout, VkShaderStageFlags.Vertex, yPad, 2 * sizeof (int));
				cmds[i].PushConstant (trianglesPipeline.Layout, VkShaderStageFlags.Vertex, zoom, 3 * sizeof (int));

				cmds[i].BindVertexBuffer (vbo);
				cmds[i].BindIndexBuffer (ibo, VkIndexType.Uint32);
				cmds[i].DrawIndexed (pointCount*3);

				grPipeline.RenderPass.End (cmds[i]);

				cmds[i].End ();
			}
		}

		public override void Update () {
			initGpuBuffers ();

			using (CommandPool cmdPoolCompute = new CommandPool (dev, computeQ.qFamIndex)) {

				CommandBuffer cmd = cmdPoolCompute.AllocateAndStart (VkCommandBufferUsageFlags.OneTimeSubmit);

				plInit.BindDescriptorSet (cmd, dsVAO, 1);
				cmd.PushConstant (plCompute.Layout, VkShaderStageFlags.Compute, IMG_DIM, sizeof (int));
				cmd.PushConstant (plCompute.Layout, VkShaderStageFlags.Compute, pointCount, 2 * sizeof (int));

				if (!pong)
					plInit.BindDescriptorSet (cmd, dsPong);
				else
					plInit.BindDescriptorSet (cmd, dsPing);

				plInit.Bind (cmd);
				cmd.Dispatch (pointCount);

				VkMemoryBarrier memBar = VkMemoryBarrier.New ();
				memBar.srcAccessMask = VkAccessFlags.ShaderWrite;
				memBar.dstAccessMask = VkAccessFlags.ShaderRead;
				Vk.vkCmdPipelineBarrier (cmd.Handle, VkPipelineStageFlags.ComputeShader, VkPipelineStageFlags.ComputeShader, VkDependencyFlags.ByRegion,
					1, ref memBar, 0, IntPtr.Zero, 0, IntPtr.Zero);

				pong = false;
				uint stepSize = IMG_DIM / 2;

				plCompute.Bind (cmd);


				int pass = 0;
				while (stepSize > 0 && pass < invocationCount) {
					cmd.PushConstant (plCompute.Layout, VkShaderStageFlags.Compute, stepSize);

					if (pong)
						plCompute.BindDescriptorSet (cmd, dsPong);
					else
						plCompute.BindDescriptorSet (cmd, dsPing);

					cmd.Dispatch (IMG_DIM, IMG_DIM);

					Vk.vkCmdPipelineBarrier (cmd.Handle, VkPipelineStageFlags.ComputeShader, VkPipelineStageFlags.ComputeShader, VkDependencyFlags.ByRegion,
						1, ref memBar, 0, IntPtr.Zero, 0, IntPtr.Zero);

					pong = !pong;
					stepSize /= 2;
					pass++;
				}

				plNormalize.Bind (cmd);
				if (pong)
					plNormalize.BindDescriptorSet (cmd, dsPong);
				else
					plNormalize.BindDescriptorSet (cmd, dsPing);
				cmd.Dispatch (IMG_DIM, IMG_DIM);
				pong = !pong;

				cmd.End ();

				computeQ.Submit (cmd);
				computeQ.WaitIdle ();
			}

			printResults ();
		}

		protected override void onMouseButtonDown (MouseButton button) {
			int xPad = (int)swapChain.Width / 2 - (int)IMG_DIM * (int)zoom / 2;
			int yPad = (int)swapChain.Height / 2 - (int)IMG_DIM * (int)zoom / 2;

			int localX = (int)((lastMouseX - xPad) / zoom);
			int localY = (int)((lastMouseY - yPad) / zoom);

			if (localX < 0 || localY < 0 || localX >= IMG_DIM || localY >= IMG_DIM)
				base.onMouseButtonDown (button);
			else {
				addPoint ((uint)localX, (uint)localY);
			}
		}
		protected override void onKeyDown (Key key, int scanCode, Modifier modifiers) {
			switch (key) {
				case Key.Delete:
					clearPoints ();
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

				if (clear) {
					if (pong)
						inBuff.Fill (cmd, 0);
					else
						outBuff.Fill (cmd, 0);
				}

				staggingVBO.CopyTo (cmd, vbo, pointCount * (ulong)Marshal.SizeOf<Vector2>());

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
					trianglesPipeline.Dispose ();

					plInit.Dispose ();
					plCompute.Dispose ();
					plNormalize.Dispose ();

					dslCompute.Dispose ();
					dslImage.Dispose ();

					dsPool.Dispose ();

					inBuff.Dispose ();
					outBuff.Dispose ();
					staggingVBO.Dispose ();
					vbo.Dispose ();
					ibo.Dispose ();

					imgResult.Dispose ();

#if DEBUG
					dbgReport.Dispose ();
#endif
				}
			}

			base.Dispose (disposing);
		}


	}
}
