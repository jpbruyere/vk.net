﻿//
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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VK;
using static VK.Vk;

namespace CVKL {
    public class Instance : IDisposable {
        VkInstance inst;

        static class Strings {
            public static FixedUtf8String Name = "VKENGINE";
            public static FixedUtf8String VK_KHR_SURFACE_EXTENSION_NAME = "VK_KHR_surface";
            public static FixedUtf8String VK_KHR_WIN32_SURFACE_EXTENSION_NAME = "VK_KHR_win32_surface";
            public static FixedUtf8String VK_KHR_XCB_SURFACE_EXTENSION_NAME = "VK_KHR_xcb_surface";
            public static FixedUtf8String VK_KHR_XLIB_SURFACE_EXTENSION_NAME = "VK_KHR_xlib_surface";
            public static FixedUtf8String VK_KHR_SWAPCHAIN_EXTENSION_NAME = "VK_KHR_swapchain";
            public static FixedUtf8String VK_EXT_DEBUG_REPORT_EXTENSION_NAME = "VK_EXT_debug_report";
			public static FixedUtf8String VK_EXT_DEBUG_UTILS_EXTENSION_NAME = "VK_EXT_debug_utils";
			public static FixedUtf8String LayerValidation = "VK_LAYER_KHRONOS_validation"; 
			public static FixedUtf8String LayerMonitor = "VK_LAYER_LUNARG_monitor";
			public static FixedUtf8String VkTraceLayeName = "VK_LAYER_LUNARG_vktrace";
            public static FixedUtf8String RenderdocCaptureLayerName = "VK_LAYER_RENDERDOC_Capture";
            public static FixedUtf8String main = "main";
        }

        public PhysicalDeviceCollection GetAvailablePhysicalDevice () {
            return new PhysicalDeviceCollection (inst);
        }

        public VkSurfaceKHR CreateSurface (IntPtr hWindow) {
            ulong surf;
            Utils.CheckResult ((VkResult)Glfw.Glfw3.CreateWindowSurface (inst.Handle, hWindow, IntPtr.Zero, out surf), "Create Surface Failed.");
            return surf;
        }

        public Instance () {
            init ();
        }

        public IntPtr Handle {
            get { return inst.Handle; }
        }

        void init () {        
            NativeList<IntPtr> instanceExtensions = new NativeList<IntPtr> ();
			NativeList<IntPtr> enabledLayerNames = new NativeList<IntPtr> ();

			instanceExtensions.Add (Strings.VK_KHR_SURFACE_EXTENSION_NAME);
            if (RuntimeInformation.IsOSPlatform (OSPlatform.Windows)) {
                instanceExtensions.Add (Strings.VK_KHR_WIN32_SURFACE_EXTENSION_NAME);
            } else if (RuntimeInformation.IsOSPlatform (OSPlatform.Linux)) {
                instanceExtensions.Add (Strings.VK_KHR_XCB_SURFACE_EXTENSION_NAME);
            } else {
                throw new PlatformNotSupportedException ();
            }

#if DEBUG
			instanceExtensions.Add (Strings.VK_EXT_DEBUG_REPORT_EXTENSION_NAME);
			instanceExtensions.Add (Strings.VK_EXT_DEBUG_UTILS_EXTENSION_NAME);
			enabledLayerNames.Add (Strings.LayerMonitor);
			enabledLayerNames.Add (Strings.LayerValidation);
#if RENDERDOC
			enabledLayerNames.Add (Strings.RenderdocCaptureLayerName);
#endif
#endif
			VkApplicationInfo appInfo = new VkApplicationInfo () {
				sType = VkStructureType.ApplicationInfo,
				apiVersion = new Vulkan.Version (1, 0, 0),
				pApplicationName = Strings.Name,
				pEngineName = Strings.Name,
			};

			VkInstanceCreateInfo instanceCreateInfo = VkInstanceCreateInfo.New();
	         instanceCreateInfo.pApplicationInfo = appInfo.Pin();

			if (instanceExtensions.Count > 0) {
			    instanceCreateInfo.enabledExtensionCount = instanceExtensions.Count;
			    instanceCreateInfo.ppEnabledExtensionNames = instanceExtensions.Data;
			}
			if (enabledLayerNames.Count > 0) {
				instanceCreateInfo.enabledLayerCount = enabledLayerNames.Count;
				instanceCreateInfo.ppEnabledLayerNames = enabledLayerNames.Data;
			}

			VkResult result = vkCreateInstance (ref instanceCreateInfo, IntPtr.Zero, out inst);
			if (result != VkResult.Success) 
			    throw new InvalidOperationException ("Could not create Vulkan instance. Error: " + result);

			Vk.LoadInstanceFunctionPointers (inst);

			appInfo.Unpin ();
			instanceExtensions.Dispose ();
			enabledLayerNames.Dispose ();            
        }


        public void GetDelegate<T> (string name, out T del) {
            using (FixedUtf8String n = new FixedUtf8String (name)) {
                del = Marshal.GetDelegateForFunctionPointer<T> (vkGetInstanceProcAddr (Handle, (IntPtr)n));
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose (bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: supprimer l'état managé (objets managés).
                }
                
                vkDestroyInstance (inst, IntPtr.Zero);

                disposedValue = true;
            }
        }

        ~Instance () {
           Dispose(false);
        }

        public void Dispose () {
            Dispose (true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
