//
// Instance.cs
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
	/// <summary>
	/// Vulkan Instance disposable class
	/// </summary>
    public class Instance : IDisposable {
		/// <summary>If true, the VK_LAYER_KHRONOS_validation layer is loaded at startup; </summary>
		public static bool VALIDATION;
		/// <summary>If true, the VK_EXT_debug_utils and VK_EXT_debug_report device extensions are enabled</summary>
		public static bool DEBUG_UTILS;
		/// <summary>If true, the VK_LAYER_RENDERDOC_Capture layer is loaded at startup; </summary>
		public static bool RENDER_DOC_CAPTURE;

        VkInstance inst;

		public IntPtr Handle => inst.Handle;


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

        public Instance () {
            init ();
        }


		void init () {
			List<IntPtr> instanceExtensions = new List<IntPtr> ();
			List<IntPtr> enabledLayerNames = new List<IntPtr> ();

			instanceExtensions.Add (Strings.VK_KHR_SURFACE_EXTENSION_NAME);
			if (RuntimeInformation.IsOSPlatform (OSPlatform.Windows)) {
				instanceExtensions.Add (Strings.VK_KHR_WIN32_SURFACE_EXTENSION_NAME);
			} else if (RuntimeInformation.IsOSPlatform (OSPlatform.Linux)) {
				instanceExtensions.Add (Strings.VK_KHR_XCB_SURFACE_EXTENSION_NAME);
			} else {
				throw new PlatformNotSupportedException ();
			}

			if (VALIDATION)
				enabledLayerNames.Add (Strings.LayerValidation);
			if (RENDER_DOC_CAPTURE)
				enabledLayerNames.Add (Strings.RenderdocCaptureLayerName);

			if (DEBUG_UTILS) {
				instanceExtensions.Add (Strings.VK_EXT_DEBUG_UTILS_EXTENSION_NAME);
				instanceExtensions.Add (Strings.VK_EXT_DEBUG_REPORT_EXTENSION_NAME);
			}

			VkApplicationInfo appInfo = new VkApplicationInfo () {
				sType = VkStructureType.ApplicationInfo,
				apiVersion = new Vulkan.Version (1, 0, 0),
				pApplicationName = Strings.Name,
				pEngineName = Strings.Name,
			};

			VkInstanceCreateInfo instanceCreateInfo = VkInstanceCreateInfo.New ();
			instanceCreateInfo.pApplicationInfo = appInfo.Pin ();

			if (instanceExtensions.Count > 0) {
				instanceCreateInfo.enabledExtensionCount = (uint)instanceExtensions.Count;
				instanceCreateInfo.ppEnabledExtensionNames = instanceExtensions.Pin ();
			}
			if (enabledLayerNames.Count > 0) {
				instanceCreateInfo.enabledLayerCount = (uint)enabledLayerNames.Count;
				instanceCreateInfo.ppEnabledLayerNames = enabledLayerNames.Pin ();
			}

			VkResult result = vkCreateInstance (ref instanceCreateInfo, IntPtr.Zero, out inst);
			if (result != VkResult.Success)
				throw new InvalidOperationException ("Could not create Vulkan instance. Error: " + result);

			Vk.LoadInstanceFunctionPointers (inst);

			appInfo.Unpin ();

			if (instanceExtensions.Count > 0)
				instanceExtensions.Unpin ();
			if (enabledLayerNames.Count > 0)
				enabledLayerNames.Unpin ();
		}

		public PhysicalDeviceCollection GetAvailablePhysicalDevice () => new PhysicalDeviceCollection (inst);
		/// <summary>
		/// Create a new vulkan surface from native window pointer
		/// </summary>
		public VkSurfaceKHR CreateSurface (IntPtr hWindow) {
			ulong surf;
			Utils.CheckResult ((VkResult)Glfw.Glfw3.CreateWindowSurface (inst.Handle, hWindow, IntPtr.Zero, out surf), "Create Surface Failed.");
			return surf;
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
				} else
					System.Diagnostics.Debug.WriteLine ("Instance disposed by Finalizer");
                
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
