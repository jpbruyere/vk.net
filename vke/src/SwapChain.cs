//
// SwapChain.cs
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

namespace CVKL {
    public class SwapChain : Activable {
		public static VkImageUsageFlags IMAGES_USAGE = VkImageUsageFlags.ColorAttachment;

		internal VkSwapchainKHR handle;

		internal uint currentImageIndex;
		VkSwapchainCreateInfoKHR createInfos;
		PresentQueue presentQueue;

		public VkSemaphore presentComplete;
        public Image[] images;

		protected override VkDebugMarkerObjectNameInfoEXT DebugMarkerInfo
			=> new VkDebugMarkerObjectNameInfoEXT (VkDebugReportObjectTypeEXT.SwapchainKhrEXT, handle.Handle);


		public uint ImageCount => (uint)images?.Length;
        public uint Width => createInfos.imageExtent.width;
        public uint Height => createInfos.imageExtent.height;
        public VkFormat ColorFormat => createInfos.imageFormat;
        public VkImageUsageFlags ImageUsage => createInfos.imageUsage;

        public SwapChain (PresentQueue _presentableQueue, uint width = 800, uint height = 600, VkFormat format = VkFormat.B8g8r8a8Unorm,
        	VkPresentModeKHR presentMode = VkPresentModeKHR.FifoKHR)
        : base (_presentableQueue.dev){

            presentQueue = _presentableQueue;            
            createInfos = VkSwapchainCreateInfoKHR.New();

            VkSurfaceFormatKHR[] formats = Dev.phy.GetSurfaceFormats (presentQueue.Surface);
            for (int i = 0; i < formats.Length; i++) {
                if (formats[i].format == format) {
                    createInfos.imageFormat = format;
                    createInfos.imageColorSpace = formats[i].colorSpace;
                    break;
                }
            }
            if (createInfos.imageFormat == VkFormat.Undefined) 
                throw new Exception ("Invalid format for swapchain: " + format);

            VkPresentModeKHR[] presentModes = Dev.phy.GetSurfacePresentModes (presentQueue.Surface);
            for (int i = 0; i < presentModes.Length; i++) {
                if (presentModes[i] == presentMode) {
                    createInfos.presentMode = presentMode;
                    break;
                }
            }
            if (createInfos.presentMode != presentMode)
                throw new Exception ("Invalid presentMode for swapchain: " + presentMode);

            createInfos.surface = presentQueue.Surface;
            createInfos.imageExtent = new VkExtent2D (width, height);
            createInfos.imageArrayLayers = 1;
            createInfos.imageUsage = IMAGES_USAGE;
            createInfos.imageSharingMode = VkSharingMode.Exclusive;
            createInfos.compositeAlpha = VkCompositeAlphaFlagsKHR.OpaqueKHR;
            createInfos.presentMode = presentMode;
            createInfos.clipped = 1;            
        }
		public override void Activate () {
			if (state != ActivableState.Activated) {
				presentComplete = Dev.CreateSemaphore ();
				presentComplete.SetDebugMarkerName (Dev, "Semaphore PresentComplete");
			}
			base.Activate ();
		}

		public void Create () {
			if (state != ActivableState.Activated)
				Activate ();

			Dev.WaitIdle ();

            VkSurfaceCapabilitiesKHR capabilities = Dev.phy.GetSurfaceCapabilities (presentQueue.Surface);

            createInfos.minImageCount = capabilities.minImageCount;
            createInfos.preTransform = capabilities.currentTransform;
            createInfos.oldSwapchain = handle;

            if (capabilities.currentExtent.width == 0xFFFFFFFF) {
                if (createInfos.imageExtent.width < capabilities.minImageExtent.width)
                    createInfos.imageExtent.width = capabilities.minImageExtent.width;
                else if (createInfos.imageExtent.width > capabilities.maxImageExtent.width)
                    createInfos.imageExtent.width = capabilities.maxImageExtent.width;

                if (createInfos.imageExtent.height < capabilities.minImageExtent.height)
                    createInfos.imageExtent.height = capabilities.minImageExtent.height;
                else if (createInfos.imageExtent.height > capabilities.maxImageExtent.height)
                    createInfos.imageExtent.height = capabilities.maxImageExtent.height;
            } else 
                createInfos.imageExtent = capabilities.currentExtent;

            VkSwapchainKHR newSwapChain = Dev.CreateSwapChain (createInfos);
            if (handle.Handle != 0)
                _destroy ();
            handle = newSwapChain;

            VkImage[] tmp = Dev.GetSwapChainImages (handle);
            images = new Image[tmp.Length];
            for (int i = 0; i < tmp.Length; i++) {
                images[i] = new Image (Dev, tmp[i], ColorFormat, ImageUsage, Width, Height);
                images[i].CreateView ();
				images[i].SetName ("SwapChain Img" + i);
				images[i].Descriptor.imageView.SetDebugMarkerName (Dev, "SwapChain Img" + i + " view");
            }
        }

        public int GetNextImage () {
            VkResult res = vkAcquireNextImageKHR (Dev.VkDev, handle, UInt64.MaxValue, presentComplete, VkFence.Null, out currentImageIndex);
            if (res == VkResult.ErrorOutOfDateKHR || res == VkResult.SuboptimalKHR) {
                Create ();
                return -1;
            }
            Utils.CheckResult (res);
            return (int)currentImageIndex;
        }

        void _destroy () {
            for (int i = 0; i < ImageCount; i++) 
                images[i].Dispose ();

            Dev.DestroySwapChain (handle);
        }

		public override string ToString () {
			return string.Format ($"{base.ToString ()}[0x{handle.Handle.ToString ("x")}]");
		}

		#region IDisposable Support
		protected override void Dispose (bool disposing) {
			if (state == ActivableState.Activated) {
				if (disposing) {
				} else
					System.Diagnostics.Debug.WriteLine ("VKE Swapchain disposed by finalizer");

				Dev.DestroySemaphore (presentComplete);
				_destroy ();

			} else if (disposing)
				System.Diagnostics.Debug.WriteLine ("Calling dispose on unactive Swapchain");

			base.Dispose (disposing);
		}
		#endregion
	}
}
