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
using Vulkan;

using static Vulkan.VulkanNative;

namespace VKE {

    public class Framebuffer : IDisposable {
        internal VkFramebuffer handle;
        RenderPass renderPass;
        NativeList<VkImageView> attachments = new NativeList<VkImageView> ();
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

        public Framebuffer (RenderPass _renderPass, uint _width, uint _height, VkImageView[] views, uint _layers = 1)
        : this (_renderPass, _width, _height, _layers) {
            foreach (VkImageView v in views)
                attachments.Add (v);
            Activate ();
        }

        public unsafe void Activate () {
			if (isDisposed) {
				GC.ReRegisterForFinalize (this);
				isDisposed = false;
			}
			createInfo.attachmentCount = attachments.Count;
            createInfo.pAttachments = (VkImageView*)attachments.Data.ToPointer ();

            Utils.CheckResult (vkCreateFramebuffer (renderPass.dev.VkDev, ref createInfo, IntPtr.Zero, out handle));
        }

        public void Destroy () {
            attachments.Dispose ();
            renderPass.dev.DestroyFramebuffer (handle);
        }

		#region IDisposable Support
		private bool isDisposed = false; // Pour détecter les appels redondants

		protected virtual void Dispose (bool disposing) {
			if (!isDisposed) {
				if (disposing) {
					attachments.Dispose ();
				} else
					System.Diagnostics.Debug.WriteLine ("A FrameBuffer has not been disposed.");
				renderPass.dev.DestroyFramebuffer (handle);
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
