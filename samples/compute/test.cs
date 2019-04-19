using System;
using System.Runtime.InteropServices;
using VKE;
using VK;
using System.Linq;

namespace test {
	class Program : IDisposable {	
		VkPhysicalDeviceFeatures enabledFeatures = default (VkPhysicalDeviceFeatures);
		string[] enabledExtensions = { Ext.D.VK_KHR_swapchain };

		Instance instance;
		PhysicalDevice phy;
		Device dev;
		Queue computeQ;

		HostBuffer inBuff, outBuff;
		DescriptorPool dsPool;
		DescriptorSetLayout dsLayoutCompute;
		DescriptorSet dsetPing, dsetPong;

		ComputePipeline plCompute;

		DebugReport dbgReport;

		const uint imgDim = 256;

		uint data_size => imgDim * imgDim * 4;

		float[] datas;

		public Program () {
			instance = new Instance ();

#if DEBUG
			dbgReport = new DebugReport (instance,
				VkDebugReportFlagsEXT.ErrorEXT
				| VkDebugReportFlagsEXT.DebugEXT
				| VkDebugReportFlagsEXT.WarningEXT
				| VkDebugReportFlagsEXT.PerformanceWarningEXT
			
			);
#endif

			phy = instance.GetAvailablePhysicalDevice ().FirstOrDefault ();
			dev = new Device (phy);
			computeQ = new Queue (dev, VkQueueFlags.Compute);
			dev.Activate (enabledFeatures, enabledExtensions);

			datas = new float[data_size];
			Random rnd = new Random ();
			for (uint i = 0; i < data_size; i++) {
				datas[i] = (float)rnd.NextDouble ();
			}

			inBuff = new HostBuffer<float> (dev, VkBufferUsageFlags.StorageBuffer, datas);
			outBuff = new HostBuffer<float> (dev, VkBufferUsageFlags.StorageBuffer, data_size);

			dsPool = new DescriptorPool (dev, 2, new VkDescriptorPoolSize (VkDescriptorType.StorageBuffer, 4));
			dsLayoutCompute = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Compute, VkDescriptorType.StorageBuffer),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Compute, VkDescriptorType.StorageBuffer)
			);

			dsetPing = dsPool.Allocate (dsLayoutCompute);
			dsetPong = dsPool.Allocate (dsLayoutCompute);
			DescriptorSetWrites dsUpdate = new DescriptorSetWrites (dsetPing, dsLayoutCompute);
			dsUpdate.Write (dev, inBuff.Descriptor, outBuff.Descriptor);

			dsUpdate.Write (dev, dsetPong, outBuff.Descriptor, inBuff.Descriptor);

			plCompute = new ComputePipeline (
				new PipelineLayout (dev, new VkPushConstantRange (VkShaderStageFlags.Compute, sizeof (int)), dsLayoutCompute),
				"shaders/computeTest.comp.spv" );
		}



		public void Run () {
			using (CommandPool cmdPool = new CommandPool (dev, computeQ.qFamIndex)) {

				CommandBuffer cmd = cmdPool.AllocateAndStart (VkCommandBufferUsageFlags.OneTimeSubmit);

				bool pong = false;

				plCompute.Bind (cmd);
				cmd.PushConstant (plCompute.Layout, VkShaderStageFlags.Compute, imgDim);

				for (int i = 0; i < 4; i++) {
					if (pong)
						plCompute.BindDescriptorSet (cmd, dsetPong);
					else
						plCompute.BindDescriptorSet (cmd, dsetPing);

					cmd.Dispatch (imgDim, imgDim);
				}

				cmd.End ();

				computeQ.Submit (cmd);
				computeQ.WaitIdle ();
			}

		}


		public void Dispose () {
			dev.WaitIdle ();

			plCompute.Dispose ();
			dsLayoutCompute.Dispose ();
			dsPool.Dispose ();

			inBuff.Dispose ();
			outBuff.Dispose ();

			dev.Dispose ();

#if DEBUG
			dbgReport.Dispose ();
#endif
			instance.Dispose ();
		}

		static void Main (string[] args) {
			using (Program vke = new Program ())
				vke.Run ();
		}
	}
}
