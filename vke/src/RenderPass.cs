//
// RenderPass.cs
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
    public class RenderPass : Activable {
        internal VkRenderPass handle;        

		public readonly VkSampleCountFlags Samples;

        internal List<VkAttachmentDescription> attachments = new List<VkAttachmentDescription> ();
        public List<VkClearValue> ClearValues = new List<VkClearValue> ();
        internal List<SubPass> subpasses = new List<SubPass> ();
        List<VkSubpassDependency> dependencies = new List<VkSubpassDependency> ();

		protected override VkDebugMarkerObjectNameInfoEXT DebugMarkerInfo
			=> new VkDebugMarkerObjectNameInfoEXT(VkDebugReportObjectTypeEXT.RenderPassEXT, handle.Handle);

		#region CTORS
		public RenderPass (Device device, VkSampleCountFlags samples = VkSampleCountFlags.SampleCount1) : base(device) {
			Samples = samples;
		}

		/// <summary>
		/// Create renderpass with a single color attachment and a resolve one if needed
		/// </summary>
		public RenderPass (Device device, VkFormat colorFormat, VkSampleCountFlags samples = VkSampleCountFlags.SampleCount1)
			: this (device) { 
			Samples = samples;

			AddAttachment (colorFormat, (samples == VkSampleCountFlags.SampleCount1) ? VkImageLayout.PresentSrcKHR : VkImageLayout.ColorAttachmentOptimal, samples,
				VkAttachmentLoadOp.Load, VkAttachmentStoreOp.Store, VkImageLayout.ColorAttachmentOptimal);
            ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0.0f, 0.0f, 0.0f) });

			SubPass subpass0 = new SubPass ();
			subpass0.AddColorReference (0, VkImageLayout.ColorAttachmentOptimal);

			if (samples != VkSampleCountFlags.SampleCount1) {
				AddAttachment (colorFormat, VkImageLayout.PresentSrcKHR, VkSampleCountFlags.SampleCount1);
				ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0.0f, 0.0f, 0.0f) });
				subpass0.AddResolveReference (1, VkImageLayout.ColorAttachmentOptimal);
			}

            AddSubpass (subpass0);

            AddDependency (Vk.SubpassExternal, 0,
                VkPipelineStageFlags.BottomOfPipe, VkPipelineStageFlags.ColorAttachmentOutput,
                VkAccessFlags.MemoryRead, VkAccessFlags.ColorAttachmentRead | VkAccessFlags.ColorAttachmentWrite);
            AddDependency (0, Vk.SubpassExternal,
                VkPipelineStageFlags.ColorAttachmentOutput, VkPipelineStageFlags.BottomOfPipe,
                VkAccessFlags.ColorAttachmentRead | VkAccessFlags.ColorAttachmentWrite, VkAccessFlags.MemoryRead);

		}
        /// <summary>
        /// Create default renderpass with one color and one depth attachments
        /// </summary>
        public RenderPass (Device device, VkFormat colorFormat, VkFormat depthFormat, VkSampleCountFlags samples = VkSampleCountFlags.SampleCount1)
            : this (device){

			Samples = samples;

			AddAttachment (colorFormat, (samples == VkSampleCountFlags.SampleCount1) ? VkImageLayout.PresentSrcKHR : VkImageLayout.ColorAttachmentOptimal, samples);
			AddAttachment (depthFormat, VkImageLayout.DepthStencilAttachmentOptimal, samples);

            ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0.0f, 0.0f, 0.0f) });
            ClearValues.Add (new VkClearValue { depthStencil = new VkClearDepthStencilValue (1.0f, 0) });

			SubPass subpass0 = new SubPass ();

			subpass0.AddColorReference (0, VkImageLayout.ColorAttachmentOptimal);
			subpass0.SetDepthReference (1, VkImageLayout.DepthStencilAttachmentOptimal);

			if (samples != VkSampleCountFlags.SampleCount1) {
				AddAttachment (colorFormat, VkImageLayout.PresentSrcKHR, VkSampleCountFlags.SampleCount1);
				ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0.0f, 0.0f, 0.2f) });
				subpass0.AddResolveReference (2, VkImageLayout.ColorAttachmentOptimal);
			}

            AddSubpass (subpass0);

            AddDependency (Vk.SubpassExternal, 0,
                VkPipelineStageFlags.BottomOfPipe, VkPipelineStageFlags.ColorAttachmentOutput,
                VkAccessFlags.MemoryRead, VkAccessFlags.ColorAttachmentRead | VkAccessFlags.ColorAttachmentWrite);
            AddDependency (0, Vk.SubpassExternal,
                VkPipelineStageFlags.ColorAttachmentOutput, VkPipelineStageFlags.BottomOfPipe,
                VkAccessFlags.ColorAttachmentRead | VkAccessFlags.ColorAttachmentWrite, VkAccessFlags.MemoryRead);
        }
		#endregion

		public override void Activate () {
			if (state != ActivableState.Activated) {
				List<VkSubpassDescription> spDescs = new List<VkSubpassDescription> ();
				foreach (SubPass sp in subpasses)
					spDescs.Add (sp.SubpassDescription);

				VkRenderPassCreateInfo renderPassInfo = VkRenderPassCreateInfo.New();
				renderPassInfo.attachmentCount = (uint)attachments.Count;
				renderPassInfo.pAttachments = attachments.Pin ();
				renderPassInfo.subpassCount = (uint)spDescs.Count;
				renderPassInfo.pSubpasses = spDescs.Pin ();
				renderPassInfo.dependencyCount = (uint)dependencies.Count;
				renderPassInfo.pDependencies = dependencies.Pin ();

				handle = Dev.CreateRenderPass (renderPassInfo);

				foreach (SubPass sp in subpasses)
					sp.UnpinLists ();

				attachments.Unpin ();
				spDescs.Unpin ();
				dependencies.Unpin ();
			}
			base.Activate ();
        }


        public void AddAttachment (VkFormat format,
            VkImageLayout finalLayout, VkSampleCountFlags samples = VkSampleCountFlags.SampleCount1,
            VkAttachmentLoadOp loadOp = VkAttachmentLoadOp.Clear,
            VkAttachmentStoreOp storeOp = VkAttachmentStoreOp.Store,
            VkImageLayout initialLayout = VkImageLayout.Undefined) {
            attachments.Add (new VkAttachmentDescription {
                format = format,
                samples = samples,
                loadOp = loadOp,
                storeOp = storeOp,
                stencilLoadOp = VkAttachmentLoadOp.DontCare,
                stencilStoreOp = VkAttachmentStoreOp.DontCare,
                initialLayout = initialLayout,
                finalLayout = finalLayout,
            }); 
        }
        public void AddAttachment (VkFormat format, VkImageLayout finalLayout,
            VkAttachmentLoadOp stencilLoadOp,
            VkAttachmentStoreOp stencilStoreOp,
            VkAttachmentLoadOp loadOp = VkAttachmentLoadOp.DontCare,
            VkAttachmentStoreOp storeOp = VkAttachmentStoreOp.DontCare,
            VkSampleCountFlags samples = VkSampleCountFlags.SampleCount1,
            VkImageLayout initialLayout = VkImageLayout.Undefined) {
            attachments.Add (new VkAttachmentDescription {
                format = format,
                samples = samples,
                loadOp = loadOp,
                storeOp = storeOp,
                stencilLoadOp = stencilLoadOp,
                stencilStoreOp = stencilStoreOp,
                initialLayout = initialLayout,
                finalLayout = finalLayout,
            });
        }

        public void AddDependency (uint srcSubpass, uint dstSubpass,
            VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask,
            VkAccessFlags srcAccessMask, VkAccessFlags dstAccessMask,
            VkDependencyFlags dependencyFlags = VkDependencyFlags.ByRegion) {
            dependencies.Add (new VkSubpassDependency {
                srcSubpass = srcSubpass,
                dstSubpass = dstSubpass,
                srcStageMask = srcStageMask,
                dstStageMask = dstStageMask,
                srcAccessMask = srcAccessMask,
                dstAccessMask = dstAccessMask,
                dependencyFlags = dependencyFlags
            });
        }
        public void AddSubpass (params SubPass[] subPass) {
            subpasses.AddRange (subPass);
        }
        /// <summary>
        /// Begin Render pass with framebuffer extent dimensions
        /// </summary>
        public void Begin (CommandBuffer cmd, Framebuffer frameBuffer) {
            Begin (cmd, frameBuffer, frameBuffer.Width, frameBuffer.Height);
        }
        /// <summary>
        /// Begin Render pass with custom render area
        /// </summary>
        public void Begin (CommandBuffer cmd, Framebuffer frameBuffer, uint width, uint height) {

            VkRenderPassBeginInfo info = VkRenderPassBeginInfo.New();
            info.renderPass = handle;
            info.renderArea.extent.width = width;
            info.renderArea.extent.height = height;
            info.clearValueCount = (uint)ClearValues.Count;
            info.pClearValues = ClearValues.Pin ();
            info.framebuffer = frameBuffer.handle;

			vkCmdBeginRenderPass (cmd.Handle, ref info, VkSubpassContents.Inline);

			ClearValues.Unpin ();
        }
		/// <summary>
		/// Switch to next subpass
		/// </summary>
		public void BeginSubPass (CommandBuffer cmd, VkSubpassContents subpassContents = VkSubpassContents.Inline) {
			vkCmdNextSubpass (cmd.Handle, subpassContents);
		}
        public void End (CommandBuffer cmd) {
            vkCmdEndRenderPass (cmd.Handle);
        }

		public override string ToString () {
			return string.Format ($"{base.ToString ()}[0x{handle.Handle.ToString("x")}]");
		}
        
		#region IDisposable Support
		protected override void Dispose (bool disposing) {
			if (state == ActivableState.Activated) {
				if (disposing) {
				} else
					System.Diagnostics.Debug.WriteLine ("VKE Activable RenderPass disposed by finalizer");

				Dev.DestroyRenderPass (handle);
			}else if (disposing)
				System.Diagnostics.Debug.WriteLine ("Calling dispose on unactive RenderPass");

			base.Dispose (disposing);
		}
		#endregion
	}
}
