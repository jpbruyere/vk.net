﻿//
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
using Vulkan;

using static Vulkan.VulkanNative;

namespace VKE {

    public class Framebuffer : IDisposable {
        internal VkFramebuffer handle;
        RenderPass renderPass;
        public List<Image> attachments = new List<Image> ();
        VkFramebufferCreateInfo createInfo = VkFramebufferCreateInfo.New ();
        public uint Width => createInfo.width;
        public uint Height => createInfo.height;
        public uint Layers => createInfo.layers;


        public Framebuffer (RenderPass _renderPass, uint _width, uint _height, uint _layers = 1) {
            renderPass = _renderPass;
            createInfo.width = _width;
            createInfo.height = _height;
            createInfo.layers = _layers;
            createInfo.renderPass = renderPass.handle;
        }

		void checkLayoutRequirements (VkImageLayout layout, ref VkImageUsageFlags usage, ref VkImageAspectFlags aspectFlags) {
			switch (layout) {
				case VkImageLayout.ColorAttachmentOptimal:
				case VkImageLayout.PresentSrcKHR:
				case VkImageLayout.SharedPresentKHR:
					aspectFlags |= VkImageAspectFlags.Color;
					usage |= VkImageUsageFlags.ColorAttachment;
					break;
				case VkImageLayout.DepthStencilAttachmentOptimal:
					aspectFlags |= VkImageAspectFlags.Depth | VkImageAspectFlags.Stencil;
					usage |= VkImageUsageFlags.DepthStencilAttachment;
					break;
				case VkImageLayout.DepthStencilReadOnlyOptimal:
					aspectFlags |= VkImageAspectFlags.Depth | VkImageAspectFlags.Stencil;
					usage |= VkImageUsageFlags.Sampled;
					break;
				case VkImageLayout.ShaderReadOnlyOptimal:
					aspectFlags |= VkImageAspectFlags.Color;
					usage |= VkImageUsageFlags.Sampled;
					break;
				case VkImageLayout.TransferSrcOptimal:
					usage |= VkImageUsageFlags.TransferSrc;
					break;
				case VkImageLayout.TransferDstOptimal:
					usage |= VkImageUsageFlags.TransferDst;
					break;
				case VkImageLayout.DepthReadOnlyStencilAttachmentOptimalKHR:
				case VkImageLayout.DepthAttachmentStencilReadOnlyOptimalKHR:
					aspectFlags |= VkImageAspectFlags.Depth | VkImageAspectFlags.Stencil;
					usage |= VkImageUsageFlags.Sampled | VkImageUsageFlags.DepthStencilAttachment;
					break;
			}
		}


		public Framebuffer (RenderPass _renderPass, uint _width, uint _height, params Image[] views)
        : this (_renderPass, _width, _height) {
			for (int i = 0; i < views.Length; i++) {
				Image v = views[i];
				if (v == null){ 
					//automatically create attachment if not in unused state in the renderpass
					VkAttachmentDescription ad = renderPass.attachments[i];
					VkImageUsageFlags usage = VkImageUsageFlags.None;
					VkImageAspectFlags aspectFlags = VkImageAspectFlags.None;

					checkLayoutRequirements (ad.initialLayout, ref usage, ref aspectFlags);
					checkLayoutRequirements (ad.finalLayout, ref usage, ref aspectFlags);
					foreach (SubPass sp in renderPass.subpasses) {
						//TODO:check subpass usage
					}

					v = new Image (renderPass.dev, ad.format, usage, VkMemoryPropertyFlags.DeviceLocal,
						_width, _height, VkImageType.Image2D, ad.samples, VkImageTiling.Optimal, 1, createInfo.layers);
					v.CreateView (VkImageViewType.Image2D, aspectFlags);
				}
				v.references++;
                attachments.Add (v);
			}
            Activate ();
		}

		public void Activate () {
			if (isDisposed) {
				GC.ReRegisterForFinalize (this);
				isDisposed = false;
			}
			VkImageView[] views;
			views = attachments.Select(a=>a.Descriptor.imageView).ToArray();

			createInfo.attachmentCount = (uint)views.Length;
            createInfo.pAttachments = views.Pin();

            Utils.CheckResult (vkCreateFramebuffer (renderPass.dev.VkDev, ref createInfo, IntPtr.Zero, out handle));

			views.Unpin();
        }

		#region IDisposable Support
		private bool isDisposed = false; // Pour détecter les appels redondants

		protected virtual void Dispose (bool disposing) {
			if (!isDisposed) {
				renderPass.dev.DestroyFramebuffer (handle);
				if (disposing) {
					foreach (Image img in attachments) 
						img.Dispose();
				} else
					System.Diagnostics.Debug.WriteLine ("A FrameBuffer has not been disposed.");

				isDisposed = true;
			}
		}

		~Framebuffer () {
			Dispose (false);
		}
		public void Dispose () {
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		#endregion

	}
}
