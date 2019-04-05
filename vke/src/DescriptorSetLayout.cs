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
using System.Collections.Generic;
using Vulkan;
using static Vulkan.VulkanNative;

namespace VKE {
    public sealed class DescriptorSetLayout : Activable {
        internal VkDescriptorSetLayout handle;
        
		public VkDescriptorSetLayoutCreateFlags Flags { get; private set; } = VkDescriptorSetLayoutCreateFlags.None;
        public List<VkDescriptorSetLayoutBinding> Bindings { get; private set; } = new List<VkDescriptorSetLayoutBinding> ();

		#region CTORS
		public DescriptorSetLayout (Device device, VkDescriptorSetLayoutCreateFlags flags) : base (device) {            
			Flags = flags;
        }
		public DescriptorSetLayout (Device device, params VkDescriptorSetLayoutBinding[] bindings)
        : this (device, VkDescriptorSetLayoutCreateFlags.None, bindings) {
        }
        public DescriptorSetLayout (Device device, VkDescriptorSetLayoutCreateFlags flags, params VkDescriptorSetLayoutBinding[] bindings)
        : this (device, flags) {
            foreach (VkDescriptorSetLayoutBinding b in bindings) 
                Bindings.Add (b);
            Activate ();
        }
		#endregion

		public override void Activate () {
			if (state != ActivableState.Activated) {
				VkDescriptorSetLayoutCreateInfo info = new VkDescriptorSetLayoutCreateInfo (Flags, (uint)Bindings.Count, Bindings.Pin());
	            Utils.CheckResult (vkCreateDescriptorSetLayout (dev.VkDev, ref info, IntPtr.Zero, out handle));
				Bindings.Unpin ();
			}
			base.Activate ();
		}

		public override string ToString () {
			return string.Format ($"{base.ToString ()}[0x{handle.Handle.ToString("x")}]");
		}

		#region IDisposable Support
		protected override void Dispose (bool disposing) {
			if (!disposing)
				System.Diagnostics.Debug.WriteLine ("VKE DescriptorSetLayout disposed by finalizer");
			if (state == ActivableState.Activated)
				vkDestroyDescriptorSetLayout (dev.VkDev, handle, IntPtr.Zero);
			base.Dispose (disposing);
		}
		#endregion
	}
}
