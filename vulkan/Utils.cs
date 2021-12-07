// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Vulkan;
using static Vulkan.Vk;

namespace Vulkan {
	public static class Utils {
		public static void CheckResult (VkResult result, string msg = "Call failed",
			[CallerMemberName] string caller = null,
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0) {
			if (result != VkResult.Success)
				throw new InvalidOperationException ($"[{sourceFilePath}:{sourceLineNumber}->{caller}]{msg}: {result}");
		}
		public static bool TryGetPhysicalDevice (VkInstance inst, VkPhysicalDeviceType deviceType, out VkPhysicalDevice phy) {
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
		public static VkPhysicalDeviceToolPropertiesEXT[] GetToolProperties (VkPhysicalDevice phy) {
			CheckResult (vkGetPhysicalDeviceToolPropertiesEXT (phy , out uint count, IntPtr.Zero));
			int sizeStruct = Marshal.SizeOf<VkPhysicalDeviceToolPropertiesEXT> ();
			IntPtr ptrTools = Marshal.AllocHGlobal (sizeStruct * (int)count);
			CheckResult (vkGetPhysicalDeviceToolPropertiesEXT (phy , out count, ptrTools));

			VkPhysicalDeviceToolPropertiesEXT[] result = new VkPhysicalDeviceToolPropertiesEXT[count];
			IntPtr tmp = ptrTools;
			for (int i = 0; i < count; i++) {
				result[i] = Marshal.PtrToStructure<VkPhysicalDeviceToolPropertiesEXT> (tmp);
				tmp += sizeStruct;
			}

			Marshal.FreeHGlobal (ptrTools);
			return result;
		}
	}
}
