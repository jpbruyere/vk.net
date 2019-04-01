//
// PipelineLayout.cs
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
using System.Linq;
using Vulkan;
using static Vulkan.VulkanNative;

namespace VKE {

	public class PipelineLayout : IDisposable {
        internal VkPipelineLayout handle;
        internal Device dev;

		public List<DescriptorSetLayout> DescriptorSetLayouts = new List<DescriptorSetLayout> ();
		public NativeList<VkPushConstantRange> PushConstantRanges = new NativeList<VkPushConstantRange> ();

		public PipelineLayout (Device device) {
			dev = device;
		}
		public PipelineLayout (Device device, VkPushConstantRange pushConstantRange, params DescriptorSetLayout[] descriptorSetLayouts) 
		: this (device, descriptorSetLayouts) {
			PushConstantRanges.Add (pushConstantRange);
		}
		public PipelineLayout (Device device, params DescriptorSetLayout[] descriptorSetLayouts)
			:this (device) {
            
			if (descriptorSetLayouts.Length > 0)
				DescriptorSetLayouts.AddRange (descriptorSetLayouts);
        }

		public PipelineLayout Activate () {
			if (isDisposed) {
				isDisposed = false;
				GC.ReRegisterForFinalize (this);
				PushConstantRanges = new NativeList<VkPushConstantRange> ();
			}
			VkPipelineLayoutCreateInfo info = VkPipelineLayoutCreateInfo.New ();
			VkDescriptorSetLayout[] dsls = DescriptorSetLayouts.Select (dsl => dsl.handle).ToArray ();
			if (dsls.Length > 0) {
				info.setLayoutCount = (uint)dsls.Length;
				info.pSetLayouts = dsls.Pin ();
			}
			if (PushConstantRanges.Count > 0) {
				info.pushConstantRangeCount = (uint)PushConstantRanges.Count;
				info.pPushConstantRanges = PushConstantRanges.Data;
			}
			Utils.CheckResult (vkCreatePipelineLayout (dev.VkDev, ref info, IntPtr.Zero, out handle));
			dsls.Unpin ();
			return this;
		}

		#region IDisposable Support
		private bool isDisposed = false; // Pour détecter les appels redondants

		protected virtual void Dispose (bool disposing) {
			if (!isDisposed) {
				if (disposing) {
					PushConstantRanges.Dispose ();
				} else
					System.Diagnostics.Debug.WriteLine ("One Pipeline layout was not disposed");

				vkDestroyPipelineLayout (dev.VkDev, handle, IntPtr.Zero);
				isDisposed = true;
			}
		}

		~PipelineLayout() {
			Dispose(false);
		}
		public void Dispose () {
			Dispose (true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
