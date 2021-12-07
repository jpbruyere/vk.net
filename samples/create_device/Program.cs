using System;
using Vulkan;
using static Vulkan.Vk;
using static samples.Utils;
using System.Linq;

namespace tests
{
	class Program
	{
		static void Main(string[] args)
		{
			VkInstance inst;

			using (VkApplicationInfo ai = new VkApplicationInfo ()) {
				using (VkInstanceCreateInfo ci = new VkInstanceCreateInfo {pApplicationInfo = ai}){
					CheckResult (vkCreateInstance (ci, IntPtr.Zero, out inst));
				}
			}

			Vk.LoadInstanceFunctionPointers (inst);

			if (!TryGetPhysicalDevice (inst, VkPhysicalDeviceType.DiscreteGpu, out VkPhysicalDevice phy))
				if (!TryGetPhysicalDevice (inst, VkPhysicalDeviceType.IntegratedGpu, out phy))
					if (!TryGetPhysicalDevice (inst, VkPhysicalDeviceType.Cpu, out phy))
						throw new Exception ("no suitable physical device found");

			foreach (VkPhysicalDeviceToolPropertiesEXT toolProp in GetToolProperties(phy)) {
				Console.ForegroundColor = ConsoleColor.DarkYellow;
				Console.WriteLine ($"Enabled Tool: {toolProp.name}({toolProp.version})");
				Console.ResetColor ();
			}

			vkGetPhysicalDeviceQueueFamilyProperties (phy, out uint queueFamilyCount, IntPtr.Zero);
			VkQueueFamilyProperties[] phys = new VkQueueFamilyProperties[queueFamilyCount];
			vkGetPhysicalDeviceQueueFamilyProperties (phy, out queueFamilyCount, phys.Pin());
			phys.Unpin();

			uint qFamIndex = (uint)phys.Select((phy, index) => (phy, index))
				.First (qfp=>qfp.phy.queueFlags.HasFlag (VkQueueFlags.Graphics)).index;

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
