//
// DescriptorSet.cs
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
    public class DescriptorSet {
        internal VkDescriptorSet handle;
        DescriptorPool pool;
        internal NativeList<VkDescriptorSetLayout> descriptorSetLayouts = new NativeList<VkDescriptorSetLayout> ();

        public DescriptorSet (DescriptorPool descriptorPool) {
            pool = descriptorPool;
        }
        public DescriptorSet (DescriptorPool descriptorPool, params DescriptorSetLayout[] layouts) 
            : this (descriptorPool) {

            foreach (DescriptorSetLayout layout in layouts)
                descriptorSetLayouts.Add (layout.handle);
        }

        public NativeList<VkWriteDescriptorSet> CreateWriteDescritprSet (params DescriptorSetLayout[] dsLayouts) {
            NativeList<VkWriteDescriptorSet> wdss = new NativeList<VkWriteDescriptorSet> ();
            foreach (DescriptorSetLayout dsl in dsLayouts) {
                foreach (VkDescriptorSetLayoutBinding binding in dsl.Bindings) {
                    VkWriteDescriptorSet wds = VkWriteDescriptorSet.New ();
                    wds.descriptorType = binding.descriptorType;
                    wds.descriptorCount = binding.descriptorCount;
                    wds.dstBinding = binding.binding;
                    wds.dstSet = handle;
                    wdss.Add (wds);
                }
            }
            return wdss;
        }

		public void Free () {
			pool.FreeDescriptorSet (this);
		}
    }
}
