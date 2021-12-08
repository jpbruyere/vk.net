// Copyright (c) 2019-2021  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

using static Vulkan.Vk;
using static Vulkan.Utils;

namespace Vulkan {
	public static class VulkanExtensionMethods {
		#region instance
		public static bool TryGetPhysicalDevice (this VkInstance inst, VkPhysicalDeviceType deviceType, out VkPhysicalDevice phy) {
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
		#endregion
		#region Physical device
		public static VkQueueFamilyProperties[] GetQueueFamilyProperties (this VkPhysicalDevice phy) {
			vkGetPhysicalDeviceQueueFamilyProperties (phy, out uint queueFamilyCount, IntPtr.Zero);
			VkQueueFamilyProperties[] qFamProps = new VkQueueFamilyProperties[queueFamilyCount];
			vkGetPhysicalDeviceQueueFamilyProperties (phy, out queueFamilyCount, qFamProps.Pin());
			qFamProps.Unpin();
			return qFamProps;
		}
		public static VkSurfaceFormatKHR [] GetSurfaceFormats (this VkPhysicalDevice phy, VkSurfaceKHR surf)
		{
			vkGetPhysicalDeviceSurfaceFormatsKHR (phy, surf, out uint count, IntPtr.Zero);
			VkSurfaceFormatKHR [] formats = new VkSurfaceFormatKHR [count];
			vkGetPhysicalDeviceSurfaceFormatsKHR (phy, surf, out count, formats.Pin ());
			formats.Unpin ();
			return formats;
		}
		public static VkPresentModeKHR[] GetSurfacePresentModes (this VkPhysicalDevice phy, VkSurfaceKHR surf) {
			vkGetPhysicalDeviceSurfacePresentModesKHR (phy, surf, out uint count, IntPtr.Zero);
			VkPresentModeKHR[] modes = new VkPresentModeKHR[count];
			vkGetPhysicalDeviceSurfacePresentModesKHR (phy, surf, out count, modes.Pin ());
			modes.Unpin ();
			return modes;

		}
		#endregion
	}
}
