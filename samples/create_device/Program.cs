using System;
using Vulkan;
using static Vulkan.Vk;
using static Vulkan.Utils;
using System.Linq;
using System.Runtime.InteropServices;

namespace tests
{	
	class Program
	{
		static void Main(string[] args)
		{
			VkInstance inst;

			using (VkApplicationInfo ai = new VkApplicationInfo (
				new Vulkan.Version(1,2,0),
				new Vulkan.Version(1,2,0),
				new Vulkan.Version(1,3,0)
			)) {
				
			using (PinnedObjects po = new PinnedObjects()) {
				IntPtr[] instanceExts = {Ext.I.VK_KHR_get_physical_device_properties2.Pin(po)};
				IntPtr[] layers = {"VK_LAYER_KHRONOS_validation".Pin(po)};
				using (VkInstanceCreateInfo ci = new VkInstanceCreateInfo {
					pApplicationInfo = ai,
					enabledExtensionCount = 1,
					enabledLayerCount = 1,
					ppEnabledExtensionNames = instanceExts.Pin(po),
					ppEnabledLayerNames = layers.Pin(po)}){
						CheckResult (vkCreateInstance (ci, IntPtr.Zero, out inst));
					}
				}
			}

			Vk.LoadInstanceFunctionPointers (inst);

			CheckResult (vkEnumerateInstanceVersion(out uint apiVersion));

			Console.WriteLine ($"Vulkan api version: {(Vulkan.Version)apiVersion})");
		

			if (!TryGetPhysicalDevice (inst, VkPhysicalDeviceType.DiscreteGpu, out VkPhysicalDevice phy))
				if (!TryGetPhysicalDevice (inst, VkPhysicalDeviceType.IntegratedGpu, out phy))
					if (!TryGetPhysicalDevice (inst, VkPhysicalDeviceType.Cpu, out phy))
						throw new Exception ("no suitable physical device found");

			foreach (VkPhysicalDeviceToolProperties toolProp in GetToolProperties(phy)) {
				Console.ForegroundColor = ConsoleColor.DarkYellow;
				Console.WriteLine ($"Enabled Tool: {toolProp.name}({toolProp.version})");
				Console.ResetColor ();
			}
			
			vkGetPhysicalDeviceQueueFamilyProperties (phy, out uint queueFamilyCount, IntPtr.Zero);
			VkQueueFamilyProperties[] qFamProps = new VkQueueFamilyProperties[queueFamilyCount];
			vkGetPhysicalDeviceQueueFamilyProperties (phy, out queueFamilyCount, qFamProps.Pin());
			qFamProps.Unpin();
			Console.WriteLine ($"Queues:");
			for (int i = 0; i < queueFamilyCount; i++)
				Console.WriteLine ($"\t{i}: {qFamProps[i].queueFlags,-60} ({qFamProps[i].queueCount})");

			uint qFamIndex = (uint)qFamProps.Select((qFam, index) => (qFam, index))
				.First (qfp=>qfp.qFam.queueFlags.HasFlag (VkQueueFlags.Graphics)).index;

						
			VkPhysicalDeviceProperties2 phyProps2 = VkPhysicalDeviceProperties2.New;
			using (var vk11 = new PNext<VkPhysicalDeviceVulkan11Properties>()) {
				phyProps2.pNext = vk11;
				vkGetPhysicalDeviceProperties2(phy, ref phyProps2);				
				Console.WriteLine($"SupportedStages: {vk11.Val.subgroupSupportedStages}" );
			}

			VkDevice dev = default;
			float[] priorities = {0};
			using (VkDeviceQueueCreateInfo qInfo = new VkDeviceQueueCreateInfo (qFamIndex,1,priorities)) {
				using (VkDeviceCreateInfo deviceCreateInfo = new VkDeviceCreateInfo () {
							pQueueCreateInfos = qInfo
						}) {
					CheckResult (vkCreateDevice (phy, deviceCreateInfo, IntPtr.Zero, out dev));
				}
			}
			
			vkGetDeviceQueue (dev, qFamIndex, 0, out VkQueue gQ);

			vkDestroyDevice (dev, IntPtr.Zero);

			vkDestroyInstance (inst, IntPtr.Zero);
		}
	}
}
