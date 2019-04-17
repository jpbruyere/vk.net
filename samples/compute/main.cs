using System;
using System.Runtime.InteropServices;
using VKE;
using VK;
using System.Linq;

namespace SimpleCompute {
	class Program : IDisposable {	
		VkPhysicalDeviceFeatures enabledFeatures = default (VkPhysicalDeviceFeatures);
		string[] enabledExtensions = { Ext.D.VK_KHR_swapchain };

		Instance instance;
		PhysicalDevice phy;
		Device dev;
		Queue computeQ;

		HostBuffer inBuff, outBuff;
		DescriptorPool dsPool;
		DescriptorSetLayout dsLayout;
		DescriptorSet dset;

		ComputePipeline plCompute;

		DebugReport dbgReport;

		const uint data_size = 256;
		int[] datas;

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

			datas = new int[data_size];
			Random rnd = new Random ();
			for (uint i = 0; i < data_size; i++) {
				datas[i] = rnd.Next ();
			}

			inBuff = new HostBuffer<int> (dev, VkBufferUsageFlags.StorageBuffer, datas);
			outBuff = new HostBuffer<int> (dev, VkBufferUsageFlags.StorageBuffer, data_size);

			dsPool = new DescriptorPool (dev, 1, new VkDescriptorPoolSize (VkDescriptorType.StorageBuffer, 2));
			dsLayout = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Compute, VkDescriptorType.StorageBuffer),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Compute, VkDescriptorType.StorageBuffer)
			);
			dset = dsPool.Allocate (dsLayout);
			DescriptorSetWrites dsUpdate = new DescriptorSetWrites (dset, dsLayout);
			dsUpdate.Write (dev, inBuff.Descriptor, outBuff.Descriptor);

			plCompute = new ComputePipeline (new PipelineLayout (dev, dsLayout), "shaders/compute.comp.spv" );
		}



		public void Run () {
			using (CommandPool cmdPool = new CommandPool (dev, computeQ.qFamIndex)) {

				CommandBuffer cmd = cmdPool.AllocateAndStart (VkCommandBufferUsageFlags.OneTimeSubmit);

				plCompute.Bind (cmd);
				plCompute.BindDescriptorSet (cmd, dset);

				cmd.Dispatch (data_size * sizeof (int));

				cmd.End ();

				computeQ.Submit (cmd);
				computeQ.WaitIdle ();
			}

			printResults ();
		}

		void printResults () {
			int[] results = new int[data_size];

			outBuff.Map ();
			Marshal.Copy (outBuff.MappedData, results, 0, results.Length);

			Console.ForegroundColor = ConsoleColor.DarkBlue;
			Console.Write ("IN :");
			for (int i = 0; i < data_size; i++) {
				Console.Write ($" {datas[i]} ");
			}
			Console.WriteLine ();
			Console.Write ("OUT:");
			for (int i = 0; i < data_size; i++) {
				Console.Write ($" {results[i]} ");
			}
			Console.WriteLine ();
			outBuff.Unmap ();
		}

		public void Dispose () {
			dev.WaitIdle ();

			plCompute.Dispose ();
			dsLayout.Dispose ();
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
