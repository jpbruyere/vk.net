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
using Vulkan;

using static Vulkan.VulkanNative;

namespace VKE {
    public class RenderPass {
        internal VkRenderPass handle;
        internal Device dev;

        NativeList<VkAttachmentDescription> attachments = new NativeList<VkAttachmentDescription> ();
        public NativeList<VkClearValue> ClearValues = new NativeList<VkClearValue> ();

        List<SubPass> subpasses = new List<SubPass> ();
        NativeList<VkSubpassDependency> dependencies = new NativeList<VkSubpassDependency> ();

        public void AddAttachment (VkFormat format,
            VkImageLayout finalLayout, VkSampleCountFlags samples = VkSampleCountFlags.Count1,
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
            VkSampleCountFlags samples = VkSampleCountFlags.Count1,
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
            VkSubpassDependency dep = new VkSubpassDependency {
                srcSubpass = srcSubpass,
                dstSubpass = dstSubpass,
                srcStageMask = srcStageMask,
                dstStageMask = dstStageMask,
                srcAccessMask = srcAccessMask,
                dstAccessMask = dstAccessMask,
                dependencyFlags = dependencyFlags
            };
        }
        public void AddSubpass (SubPass subPass) {
            subpasses.Add (subPass);
        }
        public RenderPass (Device device) {
            dev = device;
        }
        /// <summary>
        /// Create default renderpass with one color and one depth attachments
        /// </summary>
        public RenderPass (Device device, VkFormat colorFormat, VkFormat depthFormat)
            : this (device){

            AddAttachment (colorFormat, VkImageLayout.PresentSrcKHR);
            AddAttachment (depthFormat, VkImageLayout.DepthStencilAttachmentOptimal);

            ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0.0f, 0.0f, 0.2f) });
            ClearValues.Add (new VkClearValue { depthStencil = new VkClearDepthStencilValue (1.0f, 0) });

            SubPass subpass0 = new SubPass ();

            subpass0.AddColorReference (0, VkImageLayout.ColorAttachmentOptimal);
            subpass0.SetDepthReference (1, VkImageLayout.DepthStencilAttachmentOptimal);

            AddSubpass (subpass0);

            AddDependency (VulkanNative.SubpassExternal, 0,
                VkPipelineStageFlags.BottomOfPipe, VkPipelineStageFlags.ColorAttachmentOutput,
                VkAccessFlags.MemoryRead, VkAccessFlags.ColorAttachmentRead | VkAccessFlags.ColorAttachmentWrite);
            AddDependency (0, 1,
                VkPipelineStageFlags.ColorAttachmentOutput, VkPipelineStageFlags.BottomOfPipe,
                VkAccessFlags.ColorAttachmentRead | VkAccessFlags.ColorAttachmentWrite, VkAccessFlags.MemoryRead);

            Activate ();
        }
        public void Activate () {
            using (NativeList<VkSubpassDescription> spDescs = new NativeList<VkSubpassDescription> ((uint)subpasses.Count)) {
                foreach (SubPass sp in subpasses)
                    spDescs.Add (sp.SubpassDescription);

                unsafe {

                    VkRenderPassCreateInfo renderPassInfo = VkRenderPassCreateInfo.New ();
                    renderPassInfo.attachmentCount = attachments.Count;
                    renderPassInfo.pAttachments = (VkAttachmentDescription*)attachments.Data.ToPointer ();
                    renderPassInfo.subpassCount = spDescs.Count;
                    renderPassInfo.pSubpasses = (VkSubpassDescription*)spDescs.Data.ToPointer ();
                    renderPassInfo.dependencyCount = dependencies.Count;
                    renderPassInfo.pDependencies = (VkSubpassDependency*)dependencies.Data.ToPointer ();

                    handle = dev.CreateRenderPass (renderPassInfo);
                }
            }
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
        public unsafe void Begin (CommandBuffer cmd, Framebuffer frameBuffer, uint width, uint height) {

            VkRenderPassBeginInfo info = VkRenderPassBeginInfo.New ();
            info.renderPass = handle;
            info.renderArea.extent.width = width;
            info.renderArea.extent.height = height;
            info.clearValueCount = ClearValues.Count;
            info.pClearValues = (VkClearValue*) ClearValues.Data.ToPointer();
            info.framebuffer = frameBuffer.handle;

            vkCmdBeginRenderPass (cmd.Handle, ref info, VkSubpassContents.Inline);
        }
        public void End (CommandBuffer cmd) {
            vkCmdEndRenderPass (cmd.Handle);
        }

        public void Destroy () {
            dev.DestroyRenderPass (handle);
        }
    }
}
