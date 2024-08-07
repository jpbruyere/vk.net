// Copyright (c) 2024  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using Vulkan;
using static Vulkan.Vk;
using static Vulkan.Utils;
using Version = Vulkan.Version;

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
				new Vulkan.Version(1,3,0))) {
				using (VkInstanceCreateInfo ci = new VkInstanceCreateInfo {pApplicationInfo = ai}){
					CheckResult (vkCreateInstance (ci, IntPtr.Zero, out inst));
				}
			}

			Vk.LoadInstanceFunctionPointers (inst);

			CheckResult (vkEnumeratePhysicalDevices (inst, out uint phyCount, IntPtr.Zero));

			VkPhysicalDevice[] phys = new VkPhysicalDevice[phyCount];

			CheckResult (vkEnumeratePhysicalDevices (inst, out phyCount, phys.Pin()));
			phys.Unpin();

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
