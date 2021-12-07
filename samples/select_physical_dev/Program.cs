using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Vulkan;
using static Vulkan.Vk;
using static samples.Utils;
using Version = Vulkan.Version;

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

			CheckResult (vkEnumeratePhysicalDevices (inst, out uint phyCount, IntPtr.Zero));

			VkPhysicalDevice[] phys = new VkPhysicalDevice[phyCount];

			CheckResult (vkEnumeratePhysicalDevices (inst, out phyCount, phys.Pin()));

			for (int i = 0; i < phys.Length; i++)
			{
				vkGetPhysicalDeviceProperties (phys[i], out VkPhysicalDeviceProperties props);
				Console.WriteLine ($"{props.deviceName}");
				Console.WriteLine ($"\tdeviceType:   {props.deviceType,20}");
				Console.WriteLine ($"\tapiVersion:   {(Version)props.apiVersion,20}");
				Console.WriteLine ($"\tdriverVersion:{props.driverVersion,20}");
				Console.WriteLine ($"\tvendorID:     {props.vendorID,20}");
			}

			vkDestroyInstance (inst, IntPtr.Zero);
		}
	}
}
