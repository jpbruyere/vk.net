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
using VK;
using static VK.Vk;

namespace VKE {
	public sealed class PipelineLayout : Activable {
        internal VkPipelineLayout handle;

		public List<DescriptorSetLayout> DescriptorSetLayouts = new List<DescriptorSetLayout> ();
		public List<VkPushConstantRange> PushConstantRanges = new List<VkPushConstantRange> ();

#if DEBUG && DEBUG_MARKER
		protected override VkDebugMarkerObjectNameInfoEXT DebugMarkerInfo
			=> new VkDebugMarkerObjectNameInfoEXT(VkDebugReportObjectTypeEXT.PipelineLayoutEXT, handle.Handle);
#endif

#region CTORS
		public PipelineLayout (Device device) : base (device) {	}
		public PipelineLayout (Device device, VkPushConstantRange pushConstantRange, params DescriptorSetLayout[] descriptorSetLayouts) 
		: this (device, descriptorSetLayouts) {
			PushConstantRanges.Add (pushConstantRange);
		}
		public PipelineLayout (Device device, VkPushConstantRange[] pushConstantRanges, params DescriptorSetLayout[] descriptorSetLayouts)
		: this (device, descriptorSetLayouts) {
			foreach (VkPushConstantRange pcr in pushConstantRanges)
				PushConstantRanges.Add (pcr);
		}
		public PipelineLayout (Device device, params DescriptorSetLayout[] descriptorSetLayouts)
			:this (device) {
            
			if (descriptorSetLayouts.Length > 0)
				DescriptorSetLayouts.AddRange (descriptorSetLayouts);
        }
#endregion

		public void AddPushConstants (params VkPushConstantRange[] pushConstantRanges) { 
			foreach (VkPushConstantRange pcr in pushConstantRanges)
				PushConstantRanges.Add (pcr);
		}

		public override void Activate () {
			if (state != ActivableState.Activated) {
				VkPipelineLayoutCreateInfo info = VkPipelineLayoutCreateInfo.New ();
				VkDescriptorSetLayout[] dsls = DescriptorSetLayouts.Select (dsl => dsl.handle).ToArray ();

				if (dsls.Length > 0) {
					info.setLayoutCount = (uint)dsls.Length;
					info.pSetLayouts = dsls.Pin ();
				}
				if (PushConstantRanges.Count > 0) {
					info.pushConstantRangeCount = (uint)PushConstantRanges.Count;
					info.pPushConstantRanges = PushConstantRanges.Pin();
				}
				Utils.CheckResult (vkCreatePipelineLayout (dev.VkDev, ref info, IntPtr.Zero, out handle));
				dsls.Unpin ();
				PushConstantRanges.Unpin ();
			}
			base.Activate ();
		}

		public override string ToString () {
			return string.Format ($"{base.ToString ()}[0x{handle.Handle.ToString("x")}]");
		}

#region IDisposable Support
		protected override void Dispose (bool disposing) {
			if (!disposing)
				System.Diagnostics.Debug.WriteLine ("VKE Activable PipelineLayout disposed by finalizer");
			if (state == ActivableState.Activated)
				vkDestroyPipelineLayout (dev.VkDev, handle, IntPtr.Zero);
			base.Dispose (disposing);
		}
#endregion
	}
}
