//
// FrameBuffer.cs
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

namespace CVKL {

    public class Framebuffer : Activable {
        internal VkFramebuffer handle;
        RenderPass renderPass;
        
		public List<Image> attachments = new List<Image> ();
        VkFramebufferCreateInfo createInfo = VkFramebufferCreateInfo.New();

        public uint Width => createInfo.width;
        public uint Height => createInfo.height;
        public uint Layers => createInfo.layers;

		protected override VkDebugMarkerObjectNameInfoEXT DebugMarkerInfo 
			=> new VkDebugMarkerObjectNameInfoEXT(VkDebugReportObjectTypeEXT.FramebufferEXT, handle.Handle);

		#region CTORS
		public Framebuffer (RenderPass _renderPass, uint _width, uint _height, uint _layers = 1) : base(_renderPass.Dev) {
            renderPass = _renderPass;
            createInfo.width = _width;
            createInfo.height = _height;
            createInfo.layers = _layers;
            createInfo.renderPass = renderPass.handle;
        }

		public Framebuffer (RenderPass _renderPass, uint _width, uint _height, params Image[] views)
        : this (_renderPass, _width, _height) {
			for (int i = 0; i < views.Length; i++) {
				Image v = views[i];
				if (v == null) {
					//automatically create attachment if not in unused state in the renderpass
					VkAttachmentDescription ad = renderPass.attachments[i];
					VkImageUsageFlags usage = 0;
					VkImageAspectFlags aspectFlags = 0;

					Utils.QueryLayoutRequirements (ad.initialLayout, ref usage, ref aspectFlags);
					Utils.QueryLayoutRequirements (ad.finalLayout, ref usage, ref aspectFlags);
					foreach (SubPass sp in renderPass.subpasses) {
						//TODO:check subpass usage
					}

					v = new Image (renderPass.Dev, ad.format, usage, VkMemoryPropertyFlags.DeviceLocal,
						_width, _height, VkImageType.Image2D, ad.samples, VkImageTiling.Optimal, 1, createInfo.layers);
					v.SetName ($"fbImg{i}");
					v.CreateView (VkImageViewType.ImageView2D, aspectFlags);
				} else
					v.Activate ();//increase ref and create handle if not already activated

                attachments.Add (v);
			}
            Activate ();
		}
		#endregion

		public override void Activate () {
			if (state != ActivableState.Activated) {
				VkImageView[] views = attachments.Select (a => a.Descriptor.imageView).ToArray ();
				createInfo.attachmentCount = (uint)views.Length;
				createInfo.pAttachments = views.Pin ();

				Utils.CheckResult (vkCreateFramebuffer (renderPass.Dev.VkDev, ref createInfo, IntPtr.Zero, out handle));

				views.Unpin ();
			}
			base.Activate ();
        }


		public override string ToString () {
			return string.Format ($"{base.ToString ()}[0x{handle.Handle.ToString("x")}]");
		}

#region IDisposable Support
		protected override void Dispose (bool disposing) {
			if (state == ActivableState.Activated)
				Dev.DestroyFramebuffer (handle);
			if (disposing) {
				foreach (Image img in attachments) 
					img.Dispose();
			}else
				System.Diagnostics.Debug.WriteLine ("VKE Activable object disposed by finalizer");
				
			base.Dispose (disposing);
		}
#endregion

	}
}
