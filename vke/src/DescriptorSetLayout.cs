//
// DescriptorSetLayout.cs
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
using Vulkan;
using static Vulkan.VulkanNative;

namespace VKE {
    public class DescriptorSetLayout {
        internal VkDescriptorSetLayout handle;
        internal Device dev;

        internal NativeList<VkDescriptorSetLayoutBinding> layoutBindings = new NativeList<VkDescriptorSetLayoutBinding> ();

        public DescriptorSetLayout (Device device) {
            dev = device;
        }
        public DescriptorSetLayout (Device device, params VkDescriptorSetLayoutBinding[] bindings)
        : this (device) {
            foreach (VkDescriptorSetLayoutBinding b in bindings) 
                layoutBindings.Add (b);
            Activate ();
        }
        public void Activate () {
            VkDescriptorSetLayoutCreateInfo info = VkDescriptorSetLayoutCreateInfo.New ();
            unsafe {
                info.bindingCount = layoutBindings.Count;
                info.pBindings = (VkDescriptorSetLayoutBinding*)layoutBindings.Data.ToPointer ();
                Utils.CheckResult (vkCreateDescriptorSetLayout (dev.VkDev, ref info, IntPtr.Zero, out handle));
            }
        }
        public void Destroy () {
            vkDestroyDescriptorSetLayout (dev.VkDev, handle, IntPtr.Zero);
        }
    }
}
