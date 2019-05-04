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
using System.Collections.Generic;
using VK;
using static VK.Vk;

namespace CVKL {
    public sealed class DescriptorPool : Activable {
        internal VkDescriptorPool handle;        
        public readonly uint MaxSets;

        public List<VkDescriptorPoolSize> PoolSizes = new List<VkDescriptorPoolSize> ();
        
		protected override VkDebugMarkerObjectNameInfoEXT DebugMarkerInfo
			=> new VkDebugMarkerObjectNameInfoEXT(VkDebugReportObjectTypeEXT.DescriptorPoolEXT, handle.Handle);

		#region CTORS
		public DescriptorPool (Device device, uint maxSets = 1) : base (device) {            
            MaxSets = maxSets;
        }
        public DescriptorPool (Device device, uint maxSets = 1, params VkDescriptorPoolSize[] poolSizes)
            : this (device, maxSets) {

			PoolSizes.AddRange (poolSizes);

            Activate ();            
        }
		#endregion

		public override void Activate () {
			if (state != ActivableState.Activated) {            
				VkDescriptorPoolCreateInfo info = VkDescriptorPoolCreateInfo.New();
	            info.poolSizeCount = (uint)PoolSizes.Count;
	            info.pPoolSizes = PoolSizes.Pin ();
	            info.maxSets = MaxSets;

	            Utils.CheckResult (vkCreateDescriptorPool (Dev.VkDev, ref info, IntPtr.Zero, out handle));
				PoolSizes.Unpin ();
			}
			base.Activate ();
		}

        /// <summary>
        /// Create and allocate a new DescriptorSet
        /// </summary>
        public DescriptorSet Allocate (params DescriptorSetLayout[] layouts) {
            DescriptorSet ds = new DescriptorSet (this, layouts);
            Allocate (ds);
            return ds;
        }
        public void Allocate (DescriptorSet descriptorSet) {
            VkDescriptorSetAllocateInfo allocInfo = VkDescriptorSetAllocateInfo.New();
            allocInfo.descriptorPool = handle;
            allocInfo.descriptorSetCount = (uint)descriptorSet.descriptorSetLayouts.Count;
            allocInfo.pSetLayouts = descriptorSet.descriptorSetLayouts.Pin();

            Utils.CheckResult (vkAllocateDescriptorSets (Dev.VkDev, ref allocInfo, out descriptorSet.handle));

			descriptorSet.descriptorSetLayouts.Unpin ();
        }
        public void FreeDescriptorSet (params DescriptorSet[] descriptorSets) {
            if (descriptorSets.Length == 1) {
                Utils.CheckResult (vkFreeDescriptorSets (Dev.VkDev, handle, 1, ref descriptorSets[0].handle));
                return;
            }
            Utils.CheckResult (vkFreeDescriptorSets (Dev.VkDev, handle, (uint)descriptorSets.Length, descriptorSets.Pin()));
			descriptorSets.Unpin ();
        }
        public void Reset () {
            Utils.CheckResult (vkResetDescriptorPool (Dev.VkDev, handle, 0));
        }

		public override string ToString () {
			return string.Format ($"{base.ToString ()}[0x{handle.Handle.ToString("x")}]");
		}

		#region IDisposable Support
		protected override void Dispose (bool disposing) {
			if (!disposing)
				System.Diagnostics.Debug.WriteLine ($"CVKL DescriptorPool '{name}' disposed by finalizer");
			if (state == ActivableState.Activated)
				vkDestroyDescriptorPool (Dev.VkDev, handle, IntPtr.Zero);
			base.Dispose (disposing);
		}
		#endregion
	}
}
