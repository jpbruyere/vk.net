//
// Pipeline.cs
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

namespace CVKL {
	public abstract class Pipeline : Activable {
        protected VkPipeline handle;
		protected PipelineLayout layout;

		public VkPipeline Handle => handle;
		public PipelineLayout Layout => layout;

		protected readonly VkPipelineBindPoint bindPoint;

		#region CTORS
		protected Pipeline (Device dev, string name = "custom pipeline") : base(dev, name) {
		}
		#endregion

		protected override VkDebugMarkerObjectNameInfoEXT DebugMarkerInfo
			=> new VkDebugMarkerObjectNameInfoEXT (VkDebugReportObjectTypeEXT.PipelineEXT, handle.Handle);

		public abstract void Bind (CommandBuffer cmd);
		public abstract void BindDescriptorSet (CommandBuffer cmd, DescriptorSet dset, uint firstSet = 0);

		protected override void Dispose (bool disposing) {
			if (state == ActivableState.Activated) {
				if (disposing) {
					layout.Dispose ();
				} else
					System.Diagnostics.Debug.WriteLine ($"Pipeline '{name}' disposed by finalizer");

				vkDestroyPipeline (Dev.VkDev, handle, IntPtr.Zero);
			} else if (disposing)
				System.Diagnostics.Debug.WriteLine ($"Calling dispose on unactive Pipeline: {name}");

			base.Dispose (disposing);
		}
	}
}
