using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Vulkan;
using static Vulkan.Vk;
using static samples.Utils;
using Version = Vulkan.Version;
using System.Linq;

namespace tests
{
	class Program
	{

		static bool TryGetPhysicalDevice (VkInstance inst, VkPhysicalDeviceType deviceType, out VkPhysicalDevice phy) {
			CheckResult (vkEnumeratePhysicalDevices (inst, out uint phyCount, IntPtr.Zero));

			VkPhysicalDevice[] phys = new VkPhysicalDevice[phyCount];

			CheckResult (vkEnumeratePhysicalDevices (inst, out phyCount, phys.Pin()));

			for (int i = 0; i < phys.Length; i++)
			{
				phy = phys[i];
				vkGetPhysicalDeviceProperties (phy, out VkPhysicalDeviceProperties props);
				if (props.deviceType == deviceType)
					return true;
			}
			phy = default;
			return false;
		}
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
