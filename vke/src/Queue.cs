//
// Queue.cs
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
using System.Collections;
using VK;

using static VK.Vk;

namespace VKE {

    public class PresentQueue : Queue {
        public readonly VkSurfaceKHR Surface;

        public PresentQueue (Device _dev, VkQueueFlags requestedFlags, VkSurfaceKHR _surface, float _priority = 0.0f) {        
            dev = _dev;
            priority = _priority;
            Surface = _surface;

            qFamIndex = searchQFamily (requestedFlags);
            dev.queues.Add (this);
        }
        
        uint searchQFamily (VkQueueFlags requestedFlags) {
            //search for dedicated Q
            for (uint i = 0; i < dev.phy.QueueFamilies.Length; i++) {
                if (dev.phy.QueueFamilies[i].queueFlags == requestedFlags && dev.phy.GetPresentIsSupported (i, Surface)) 
                    return i;
            }
            //search Q having flags
            for (uint i = 0; i < dev.phy.QueueFamilies.Length; i++) {
                if ((dev.phy.QueueFamilies[i].queueFlags & requestedFlags) == requestedFlags && dev.phy.GetPresentIsSupported (i, Surface)) 
                    return i;
            }

            throw new Exception (string.Format ("No Queue with flags {0} found", requestedFlags));
        }

        public void Present (VkPresentInfoKHR present) {
            Utils.CheckResult (vkQueuePresentKHR (handle, ref present));
        }
        public void Present (SwapChain swapChain, VkSemaphore wait) {
            unsafe {
                VkPresentInfoKHR present = VkPresentInfoKHR.New();

                uint idx = swapChain.currentImageIndex;
                VkSwapchainKHR sc = swapChain.handle;
                present.swapchainCount = 1;
                present.pSwapchains = sc.Pin();
                present.waitSemaphoreCount = 1;
                present.pWaitSemaphores = wait.Pin();
                present.pImageIndices = idx.Pin();

                Utils.CheckResult (vkQueuePresentKHR (handle, ref present));

				sc.Unpin ();
				wait.Unpin ();
				idx.Unpin ();
            }
        }
    }

    public class Queue {

        internal VkQueue handle;
        internal Device dev;
		public Device Dev => dev;

        VkQueueFlags flags => dev.phy.QueueFamilies[qFamIndex].queueFlags;
        public uint qFamIndex;
        public uint index;//index in queue family
        public float priority;

        protected Queue () { }
        public Queue (Device _dev, VkQueueFlags requestedFlags, float _priority = 0.0f) {
            dev = _dev;
            priority = _priority;

            qFamIndex = searchQFamily (requestedFlags);
            dev.queues.Add (this);
        }

        public void Submit (CommandBuffer cmd, VkSemaphore wait = default(VkSemaphore), VkSemaphore signal = default (VkSemaphore), VkFence fence = default (VkFence)) {
            cmd.Submit (handle, wait, signal, fence);
        }
        public void WaitIdle () {
            Utils.CheckResult (vkQueueWaitIdle (handle));
        }

        uint searchQFamily (VkQueueFlags requestedFlags) {
            //search for dedicated Q
            for (uint i = 0; i < dev.phy.QueueFamilies.Length; i++) {
                if (dev.phy.QueueFamilies[i].queueFlags == requestedFlags)
                    return i;
            }
            //search Q having flags
            for (uint i = 0; i < dev.phy.QueueFamilies.Length; i++) {
                if ((dev.phy.QueueFamilies[i].queueFlags & requestedFlags) == requestedFlags)
                    return i;
            }

            throw new Exception (string.Format ("No Queue with flags {0} found", requestedFlags));
        }

        internal void updateHandle () {
            vkGetDeviceQueue (dev.VkDev, qFamIndex, index, out handle);
        }
    }
}
