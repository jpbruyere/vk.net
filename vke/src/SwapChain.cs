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

namespace VKE {
    public class SwapChain {
        Device dev;
        PresentQueue presentQueue;
        internal VkSwapchainKHR handle;

        public VkSemaphore presentComplete;

        public Image[] images;

        internal uint currentImageIndex;

        VkSwapchainCreateInfoKHR createInfos;

        public uint ImageCount => (uint)images?.Length;
        public uint Width => createInfos.imageExtent.width;
        public uint Height => createInfos.imageExtent.height;
        public VkFormat ColorFormat => createInfos.imageFormat;
        public VkImageUsageFlags ImageUsage => createInfos.imageUsage;

        public SwapChain (PresentQueue _presentableQueue, uint width = 800, uint height = 600, VkFormat format = VkFormat.B8g8r8a8Unorm, VkPresentModeKHR presentMode = VkPresentModeKHR.FifoKHR) {
            presentQueue = _presentableQueue;
            dev = presentQueue.dev;

            createInfos = VkSwapchainCreateInfoKHR.New ();

            VkSurfaceFormatKHR[] formats = dev.phy.GetSurfaceFormats (presentQueue.Surface);
            for (int i = 0; i < formats.Length; i++) {
                if (formats[i].format == format) {
                    createInfos.imageFormat = format;
                    createInfos.imageColorSpace = formats[i].colorSpace;
                    break;
                }
            }
            if (createInfos.imageFormat == VkFormat.Undefined) 
                throw new Exception ("Invalid format for swapchain: " + format);

            VkPresentModeKHR[] presentModes = dev.phy.GetSurfacePresentModes (presentQueue.Surface);
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
            createInfos.imageUsage = VkImageUsageFlags.ColorAttachment | VkImageUsageFlags.TransferDst;
            createInfos.imageSharingMode = VkSharingMode.Exclusive;
            createInfos.compositeAlpha = VkCompositeAlphaFlagsKHR.OpaqueKHR;
            createInfos.presentMode = presentMode;
            createInfos.clipped = true;

            presentComplete = dev.CreateSemaphore ();
#if DEBUG && DEBUG_MARKER
			presentComplete.SetDebugMarkerName (dev, "Semaphore PresentComplete");
#endif

            Create ();
        }

        unsafe public void Create () {
            dev.WaitIdle ();

            VkSurfaceCapabilitiesKHR capabilities = dev.phy.GetSurfaceCapabilities (presentQueue.Surface);

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

            VkSwapchainKHR newSwapChain = dev.CreateSwapChain (createInfos);
            if (handle.Handle != 0)
                _destroy ();
            handle = newSwapChain;

            VkImage[] tmp = dev.GetSwapChainImages (handle);
            images = new Image[tmp.Length];
            for (int i = 0; i < tmp.Length; i++) {
                images[i] = new Image (dev, tmp[i], ColorFormat, ImageUsage, Width, Height);
                images[i].CreateView ();
#if DEBUG && DEBUG_MARKER
				images[i].SetName ("SwapChain Img" + i);
				images[i].Descriptor.imageView.SetDebugMarkerName (dev, "SwapChain Img" + i + " view");
#endif
            }
        }

        public int GetNextImage () {
            VkResult res = vkAcquireNextImageKHR (dev.VkDev, handle, UInt64.MaxValue, presentComplete, VkFence.Null, ref currentImageIndex);
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

            dev.DestroySwapChain (handle);
        }

        public void Destroy () {
            _destroy ();

            dev.DestroySemaphore (presentComplete);
        }
    }
}
