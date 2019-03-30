//
// Device.cs
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vulkan;
using static Vulkan.VulkanNative;

namespace VKE {
    public class Device : IDisposable {
        public readonly PhysicalDevice phy;
        VkDevice dev;
        public VkDevice VkDev => dev;

        internal List<Queue> queues = new List<Queue> ();

        public Device (PhysicalDevice _phy) {
            phy = _phy;
        }

        unsafe public void Activate (VkPhysicalDeviceFeatures enabledFeatures, params string[] extensions) {

            NativeList<VkDeviceQueueCreateInfo> qInfos = new NativeList<VkDeviceQueueCreateInfo> ();
            NativeList<float> priorities = null;

            foreach (IGrouping<uint, Queue> qfams in queues.GroupBy (q => q.qFamIndex)) {
                int qTot = qfams.Count ();
                uint qIndex = 0;
                priorities = new NativeList<float> ();
                bool qCountReached = false;//true when queue count of that family is reached

                foreach (Queue q in qfams) {
                    if (!qCountReached)
                        priorities.Add (q.priority);
                    q.index = qIndex++;
                    if (qIndex == phy.QueueFamilies[qfams.Key].queueCount) {
                        qIndex = 0;
                        qCountReached = true;
                    }
                }

                qInfos.Add (new VkDeviceQueueCreateInfo {
                    sType = VkStructureType.DeviceQueueCreateInfo,
                    queueCount = qCountReached ? phy.QueueFamilies[qfams.Key].queueCount : qIndex,
                    queueFamilyIndex = qfams.Key,
                    pQueuePriorities = (float*)priorities.Data.ToPointer ()
                });
            }
            float tmp = priorities[0];

            NativeList<IntPtr> deviceExtensions = new NativeList<IntPtr> ();
            for (int i = 0; i < extensions.Length; i++) {
                deviceExtensions.Add (new FixedUtf8String(extensions[i]));
            }

            VkPhysicalDeviceFeatures features = enabledFeatures;
            VkDeviceCreateInfo deviceCreateInfo = VkDeviceCreateInfo.New ();

            deviceCreateInfo.queueCreateInfoCount = qInfos.Count;
            deviceCreateInfo.pQueueCreateInfos = (VkDeviceQueueCreateInfo*)qInfos.Data.ToPointer();
            deviceCreateInfo.pEnabledFeatures = &features;

            if (deviceExtensions.Count > 0) {
                deviceCreateInfo.enabledExtensionCount = deviceExtensions.Count;
                deviceCreateInfo.ppEnabledExtensionNames = deviceExtensions.Data;
            }

            Utils.CheckResult (vkCreateDevice (phy.Handle, &deviceCreateInfo, null, out dev));

            qInfos.Dispose ();
            priorities.Dispose ();
            deviceExtensions.Dispose ();

            foreach (Queue q in queues)
                q.updateHandle ();
        }

        unsafe public VkSemaphore CreateSemaphore () {
            VkSemaphore tmp;
            VkSemaphoreCreateInfo info = VkSemaphoreCreateInfo.New ();
            Utils.CheckResult (vkCreateSemaphore (dev, &info, null, out tmp));
            return tmp;
        }
        public void DestroySemaphore (VkSemaphore semaphore) {
            vkDestroySemaphore (dev, semaphore, IntPtr.Zero);
        }
        unsafe public VkFence CreateFence (bool signaled = false) {
            VkFence tmp;
            VkFenceCreateInfo info = VkFenceCreateInfo.New ();
            info.flags = signaled ? VkFenceCreateFlags.Signaled : VkFenceCreateFlags.None;
            Utils.CheckResult (vkCreateFence (dev, &info, null, out tmp));
            return tmp;
        }
        public void DestroyFence (VkFence fence) {
            vkDestroyFence (dev, fence, IntPtr.Zero);
        }
        public void WaitForFence (VkFence fence, ulong timeOut = UInt64.MaxValue) {
            vkWaitForFences (dev, 1, ref fence, true, timeOut);
        }
        public void ResetFence (VkFence fence) {
            vkResetFences (dev, 1, ref fence);
        }
        public void WaitForFences (NativeList<VkFence> fences, ulong timeOut = UInt64.MaxValue) {
            vkWaitForFences (dev, fences.Count, fences.Data, true, timeOut);
        }
        public void ResetFences (NativeList<VkFence> fences) {
            vkResetFences (dev, fences.Count, fences.Data);
        }

        public void DestroyShaderModule (VkShaderModule module) {
            vkDestroyShaderModule (VkDev, module, IntPtr.Zero);
        }
        public void DestroyDescriptorPool (VkDescriptorPool pool) {
            vkDestroyDescriptorPool (VkDev, pool, IntPtr.Zero);
        }
        public void WaitIdle () {
            Utils.CheckResult(vkDeviceWaitIdle (dev));
        }
        public VkRenderPass CreateRenderPass (VkRenderPassCreateInfo info) {
            VkRenderPass renderPass;
            Utils.CheckResult (vkCreateRenderPass (dev, ref info, IntPtr.Zero, out renderPass));
            return renderPass;
        }
        internal VkSwapchainKHR CreateSwapChain (VkSwapchainCreateInfoKHR infos) {
            VkSwapchainKHR newSwapChain;
            Utils.CheckResult (vkCreateSwapchainKHR (dev, ref infos, IntPtr.Zero, out newSwapChain));
            return newSwapChain;
        }
        internal void DestroySwapChain (VkSwapchainKHR swapChain) {
            vkDestroySwapchainKHR (dev, swapChain, IntPtr.Zero);
        }
        unsafe public VkImage[] GetSwapChainImages (VkSwapchainKHR swapchain) {
            uint imageCount = 0;
            Utils.CheckResult (vkGetSwapchainImagesKHR (dev, swapchain, ref imageCount, null));
            if (imageCount == 0)
                throw new Exception ("Swapchain image count is 0.");
            VkImage[] imgs = new VkImage[imageCount];
            fixed (VkImage* ptr = imgs) {
                Utils.CheckResult (vkGetSwapchainImagesKHR (dev, swapchain, ref imageCount, ptr));
            }
            return imgs;
        }
        unsafe public VkImageView CreateImageView (VkImage image, VkFormat format, VkImageViewType viewType = VkImageViewType.Image2D, VkImageAspectFlags aspectFlags = VkImageAspectFlags.Color) {
            VkImageView view;
            VkImageViewCreateInfo infos = VkImageViewCreateInfo.New ();
            infos.image = image;
            infos.viewType = viewType;
            infos.format = format;
            infos.components = new VkComponentMapping { r = VkComponentSwizzle.R, g = VkComponentSwizzle.G, b = VkComponentSwizzle.B, a = VkComponentSwizzle.A };
            infos.subresourceRange = new VkImageSubresourceRange (aspectFlags);
                    
            Utils.CheckResult (vkCreateImageView (dev, &infos, IntPtr.Zero, out view));
            return view;
        }
        public void DestroyImageView (VkImageView view) {
            vkDestroyImageView (dev, view, IntPtr.Zero);
        }
        public void DestroySampler (VkSampler sampler) {
            vkDestroySampler (dev, sampler, IntPtr.Zero);
        }
        public void DestroyImage (VkImage img) {
            vkDestroyImage (dev, img, IntPtr.Zero);
        }
        public void DestroyFramebuffer (VkFramebuffer fb) {
            vkDestroyFramebuffer (dev, fb, IntPtr.Zero);
        }
        public void DestroyRenderPass (VkRenderPass rp) {
            vkDestroyRenderPass (dev, rp, IntPtr.Zero);
        }

        unsafe public CommandPool CreateCommandPool (uint qFamIdx) {
            VkCommandPool pool;
            VkCommandPoolCreateInfo infos = VkCommandPoolCreateInfo.New ();
            infos.queueFamilyIndex = qFamIdx;
            Utils.CheckResult (vkCreateCommandPool (dev, &infos, null, out pool));
            return new CommandPool (this, qFamIdx, pool);
        }
        // This function is used to request a Device memory type that supports all the property flags we request (e.g. Device local, host visibile)
        // Upon success it will return the index of the memory type that fits our requestes memory properties
        // This is necessary as implementations can offer an arbitrary number of memory types with different
        // memory properties. 
        // You can check http://vulkan.gpuinfo.org/ for details on different memory configurations
        internal unsafe  uint GetMemoryTypeIndex (uint typeBits, VkMemoryPropertyFlags properties) {
            // Iterate over all memory types available for the Device used in this example
            for (uint i = 0; i < phy.memoryProperties.memoryTypeCount; i++) {
                if ((typeBits & 1) == 1) {
                    if ((phy.memoryProperties.memoryTypes[i].propertyFlags & properties) == properties) {
                        return i;
                    }
                }
                typeBits >>= 1;
            }

            throw new InvalidOperationException ("Could not find a suitable memory type!");
        }
        public VkFormat GetSuitableDepthFormat () {
            VkFormat[] formats = new VkFormat[] { VkFormat.D32SfloatS8Uint, VkFormat.D32Sfloat, VkFormat.D24UnormS8Uint, VkFormat.D16UnormS8Uint, VkFormat.D16Unorm };
            foreach (VkFormat f in formats) {
                if (phy.GetFormatProperties (f).optimalTilingFeatures.HasFlag(VkFormatFeatureFlags.DepthStencilAttachment))
                    return f;
            }
            throw new InvalidOperationException ("No suitable depth format found.");
        }

        public VkShaderModule LoadSPIRVShader (string filename) {
            byte[] shaderCode = File.ReadAllBytes (filename);
            ulong shaderSize = (ulong)shaderCode.Length;
            unsafe {
                fixed (byte* scPtr = shaderCode) {
                    // Create a new shader module that will be used for Pipeline creation
                    VkShaderModuleCreateInfo moduleCreateInfo = VkShaderModuleCreateInfo.New ();
                    moduleCreateInfo.codeSize = new UIntPtr (shaderSize);
                    moduleCreateInfo.pCode = (uint*)scPtr;

                    Utils.CheckResult (vkCreateShaderModule (VkDev, ref moduleCreateInfo, IntPtr.Zero, out VkShaderModule shaderModule));

                    return shaderModule;
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        protected virtual void Dispose (bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: supprimer l'état managé (objets managés).
                }

                vkDestroyDevice (dev, IntPtr.Zero);

                disposedValue = true;
            }
        }

        // TODO: remplacer un finaliseur seulement si la fonction Dispose(bool disposing) ci-dessus a du code pour libérer les ressources non managées.
        ~Device() {
           // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
           Dispose(false);
        }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        public void Dispose () {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose (true);
            // TODO: supprimer les marques de commentaire pour la ligne suivante si le finaliseur est remplacé ci-dessus.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
