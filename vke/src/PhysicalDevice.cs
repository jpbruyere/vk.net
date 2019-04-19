//
// PhysicalDevice.cs
//
// Author:
//       Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// Copyright (c) 2019 jp
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using VK;
using static VK.Vk;
using static VK.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VKE {
    public class PhysicalDeviceCollection : IEnumerable<PhysicalDevice> {
        VkInstance inst;
        PhysicalDevice[] phys;

        public PhysicalDeviceCollection (VkInstance instance) {
            inst = instance;
            init ();
        }

        public PhysicalDevice this[int i] {
            get {
                return phys[i];
            }
        }

        public IEnumerator<PhysicalDevice> GetEnumerator () {
            return ((IEnumerable<PhysicalDevice>)phys).GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator () {
            return ((IEnumerable<PhysicalDevice>)phys).GetEnumerator ();
        }

        unsafe void init () {
            uint gpuCount = 0;
            CheckResult (vkEnumeratePhysicalDevices (inst, out gpuCount, IntPtr.Zero));
            if (gpuCount <= 0)
                throw new Exception ("No GPU found");
            NativeList<VkPhysicalDevice> gpus = new NativeList<VkPhysicalDevice> (gpuCount, gpuCount);
            //fixed (IntPtr* physicalDevices = gpus) {
            CheckResult (vkEnumeratePhysicalDevices (inst, out gpuCount, gpus.Data),
                    "Could not enumerate physical devices.");
            //}
            phys = new PhysicalDevice[gpuCount];
            Console.WriteLine ("gpu count " + gpuCount);

            for (int i = 0; i < gpuCount; i++)
                phys[i] = new PhysicalDevice (gpus[i].Handle);

            gpus.Dispose ();
        }
    }
    public class PhysicalDevice {
        IntPtr phy;

        public VkPhysicalDeviceProperties deviceProperties { get; private set; }
        public VkPhysicalDeviceFeatures deviceFeatures { get; private set; }
        public VkPhysicalDeviceMemoryProperties memoryProperties { get; private set; }
        public VkQueueFamilyProperties[] QueueFamilies { get; private set; }

		public VkPhysicalDeviceLimits Limits => deviceProperties.limits;

        public bool HasSwapChainSupport { get; private set; }
        public IntPtr Handle => phy;

        public PhysicalDevice (IntPtr vkPhy) {
            phy = vkPhy;
            init ();
        }

        unsafe void init () {
            VkPhysicalDeviceProperties pdp;
            vkGetPhysicalDeviceProperties (phy, out pdp);
            deviceProperties = pdp;

            VkPhysicalDeviceFeatures df;
            vkGetPhysicalDeviceFeatures (phy, out df);
            deviceFeatures = df;

            // Gather physical Device memory properties
            IntPtr tmp = Marshal.AllocHGlobal (Marshal.SizeOf<VkPhysicalDeviceMemoryProperties>());
            vkGetPhysicalDeviceMemoryProperties (phy, tmp);
            memoryProperties = Marshal.PtrToStructure<VkPhysicalDeviceMemoryProperties> (tmp);

            uint queueFamilyCount = 0;
            vkGetPhysicalDeviceQueueFamilyProperties (phy, out queueFamilyCount, IntPtr.Zero);
            QueueFamilies = new VkQueueFamilyProperties[queueFamilyCount];

            if (queueFamilyCount <= 0)
                throw new Exception ("No queues found for physical device");

			vkGetPhysicalDeviceQueueFamilyProperties (phy, out queueFamilyCount, QueueFamilies.Pin ());
			QueueFamilies.Unpin ();

            uint propCount = 0;

            vkEnumerateDeviceExtensionProperties (phy, IntPtr.Zero, out propCount, IntPtr.Zero);

            VkExtensionProperties[] extProps = new VkExtensionProperties[propCount];

			vkEnumerateDeviceExtensionProperties (phy, IntPtr.Zero, out propCount, extProps.Pin ());
			extProps.Unpin ();

            for (int i = 0; i < extProps.Length; i++)
            {
                fixed (VkExtensionProperties* ep = extProps) {
                    IntPtr n = (IntPtr)ep[i].extensionName;
                    switch (Marshal.PtrToStringAnsi(n))
                    {
                        case "VK_KHR_swapchain":
                            HasSwapChainSupport = true;
                            break;
                    }
                }
			}		
        }

		public unsafe bool GetDeviceExtensionSupported (string extName) {
			uint propCount = 0;

			vkEnumerateDeviceExtensionProperties (phy, IntPtr.Zero, out propCount, IntPtr.Zero);

			VkExtensionProperties[] extProps = new VkExtensionProperties[propCount];

			vkEnumerateDeviceExtensionProperties (phy, IntPtr.Zero, out propCount, extProps.Pin ());
			extProps.Unpin ();

			for (int i = 0; i < extProps.Length; i++) {
				fixed (VkExtensionProperties* ep = extProps) {
					IntPtr n = (IntPtr)ep[i].extensionName;
					if (Marshal.PtrToStringAnsi (n) == extName)
							return true;
				}
			}
			Console.WriteLine ($"INFO: unsuported device extension: {extName}");
			return false;
		}


        public bool GetPresentIsSupported (uint qFamilyIndex, VkSurfaceKHR surf) {
            VkBool32 isSupported = false;
            vkGetPhysicalDeviceSurfaceSupportKHR (phy, qFamilyIndex, surf, out isSupported);
            return isSupported;
        }

        public VkSurfaceCapabilitiesKHR GetSurfaceCapabilities (VkSurfaceKHR surf) {
            VkSurfaceCapabilitiesKHR caps;
            vkGetPhysicalDeviceSurfaceCapabilitiesKHR (phy, surf, out caps);
            return caps;
        }

        unsafe public VkSurfaceFormatKHR[] GetSurfaceFormats (VkSurfaceKHR surf) {
            uint count = 0;
            vkGetPhysicalDeviceSurfaceFormatsKHR (phy, surf, out count, IntPtr.Zero);
            VkSurfaceFormatKHR[] formats = new VkSurfaceFormatKHR[count];
            
            vkGetPhysicalDeviceSurfaceFormatsKHR (phy, surf, out count, formats.Pin());
			formats.Unpin ();
            
            return formats;
        }
        unsafe public VkPresentModeKHR[] GetSurfacePresentModes (VkSurfaceKHR surf) {
            uint count = 0;
            vkGetPhysicalDeviceSurfacePresentModesKHR (phy, surf, out count, IntPtr.Zero);
            VkPresentModeKHR[] modes = new VkPresentModeKHR[count];
            
            vkGetPhysicalDeviceSurfacePresentModesKHR (phy, surf, out count, modes.Pin());
			modes.Unpin ();
            
            return modes;
        }
        public VkFormatProperties GetFormatProperties (VkFormat format) {
            VkFormatProperties properties;
            vkGetPhysicalDeviceFormatProperties (phy, format, out properties);
            return properties;
        }
    }
}
