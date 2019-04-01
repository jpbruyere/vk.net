//
// DescriptorPool.cs
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
    public class DescriptorPool : IDisposable {
        internal VkDescriptorPool handle;
        internal Device dev;
        public uint MaxSets;

        public NativeList<VkDescriptorPoolSize> PoolSizes = new NativeList<VkDescriptorPoolSize> ();

        public DescriptorPool (Device device, uint maxSets = 1) {
            dev = device;
            MaxSets = maxSets;
        }
        public DescriptorPool (Device device, uint maxSets = 1, params VkDescriptorPoolSize[] poolSizes)
            : this (device, maxSets) {

            foreach (VkDescriptorPoolSize poolSize in poolSizes)
                PoolSizes.Add (poolSize);
            Activate ();            
        }
        public void Activate () {
			if (isDisposed) {
				GC.ReRegisterForFinalize (this);
				isDisposed = false;
			}
			VkDescriptorPoolCreateInfo info = VkDescriptorPoolCreateInfo.New ();
            info.poolSizeCount = PoolSizes.Count;
            info.pPoolSizes = PoolSizes.Data;
            info.maxSets = MaxSets;

            Utils.CheckResult (vkCreateDescriptorPool (dev.VkDev, ref info, IntPtr.Zero, out handle));
        }
        /// <summary>
        /// Create and allocate a new DescriptorSet
        /// </summary>
        public DescriptorSet Allocate (params DescriptorSetLayout[] layouts) {
            DescriptorSet ds = new DescriptorSet (this, layouts);
            Allocate (ds);
            return ds;
        }
        public unsafe void Allocate (DescriptorSet descriptorSet) {
            VkDescriptorSetAllocateInfo allocInfo = VkDescriptorSetAllocateInfo.New ();
            allocInfo.descriptorPool = handle;
            allocInfo.descriptorSetCount = descriptorSet.descriptorSetLayouts.Count;
            allocInfo.pSetLayouts = (VkDescriptorSetLayout*)descriptorSet.descriptorSetLayouts.Data.ToPointer ();

            Utils.CheckResult (vkAllocateDescriptorSets (dev.VkDev, ref allocInfo, out descriptorSet.handle));
        }
        public void FreeDescriptorSet (params DescriptorSet[] descriptorSets) {
            if (descriptorSets.Length == 1) {
                Utils.CheckResult (vkFreeDescriptorSets (dev.VkDev, handle, 1, ref descriptorSets[0].handle));
                return;
            }
            using (NativeList<VkDescriptorSet> dSets = new NativeList<VkDescriptorSet> ((uint)descriptorSets.Length)) {
                foreach (DescriptorSet ds in descriptorSets)
                    dSets.Add (ds.handle);
                Utils.CheckResult (vkFreeDescriptorSets (dev.VkDev, handle, dSets.Count, dSets.Data));
            }
        }
        public void Reset () {
            Utils.CheckResult (vkResetDescriptorPool (dev.VkDev, handle, 0));
        }

		#region IDisposable Support
		private bool isDisposed = false; // Pour détecter les appels redondants

		protected virtual void Dispose (bool disposing) {
			if (!isDisposed) {
				if (disposing) {
					PoolSizes.Dispose ();
				} else
					System.Diagnostics.Debug.WriteLine ("A descriptorPool has not been disposed.");
				dev.DestroyDescriptorPool (handle); 
				isDisposed = true;
			}
		}

		~DescriptorPool () {
			Dispose (false);
		}
		public void Dispose () {
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		#endregion

	}
}
