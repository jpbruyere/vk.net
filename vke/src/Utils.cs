//
// Utils.cs
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
namespace Vulkan {
    public static class Utils {
        public static void CheckResult (VkResult result, string errorString = "Call failed") {
            if (result != VkResult.Success)
                throw new InvalidOperationException (errorString + ": " + result.ToString ());
        }
        public static float DegreesToRadians (float degrees) {
            return degrees * (float)Math.PI / 180f;
        }
        // Fixed sub resource on first mip level and layer
        public static void setImageLayout (
            VkCommandBuffer cmdbuffer,
            VkImage image,
            VkImageAspectFlags aspectMask,
            VkImageLayout oldImageLayout,
            VkImageLayout newImageLayout,
            VkPipelineStageFlags srcStageMask = VkPipelineStageFlags.AllCommands,
            VkPipelineStageFlags dstStageMask = VkPipelineStageFlags.AllCommands) {
            VkImageSubresourceRange subresourceRange = new VkImageSubresourceRange {
                aspectMask = aspectMask,
                baseMipLevel = 0,
                levelCount = 1,
                layerCount = 1,
            };
            setImageLayout (cmdbuffer, image, aspectMask, oldImageLayout, newImageLayout, subresourceRange);
        }

        // Create an image memory barrier for changing the layout of
        // an image and put it into an active command buffer
        // See chapter 11.4 "Image Layout" for details

        unsafe public static void setImageLayout (
            VkCommandBuffer cmdbuffer,
            VkImage image,
            VkImageAspectFlags aspectMask,
            VkImageLayout oldImageLayout,
            VkImageLayout newImageLayout,
            VkImageSubresourceRange subresourceRange,
            VkPipelineStageFlags srcStageMask = VkPipelineStageFlags.AllCommands,
            VkPipelineStageFlags dstStageMask = VkPipelineStageFlags.AllCommands) {
            // Create an image barrier object
            VkImageMemoryBarrier imageMemoryBarrier = VkImageMemoryBarrier.New ();
            imageMemoryBarrier.srcQueueFamilyIndex = VulkanNative.QueueFamilyIgnored;
            imageMemoryBarrier.dstQueueFamilyIndex = VulkanNative.QueueFamilyIgnored;
            imageMemoryBarrier.oldLayout = oldImageLayout;
            imageMemoryBarrier.newLayout = newImageLayout;
            imageMemoryBarrier.image = image;
            imageMemoryBarrier.subresourceRange = subresourceRange;

            // Source layouts (old)
            // Source access mask controls actions that have to be finished on the old layout
            // before it will be transitioned to the new layout
            switch (oldImageLayout) {
                case VkImageLayout.Undefined:
                    // Image layout is undefined (or does not matter)
                    // Only valid as initial layout
                    // No flags required, listed only for completeness
                    imageMemoryBarrier.srcAccessMask = 0;
                    break;

                case VkImageLayout.Preinitialized:
                    // Image is preinitialized
                    // Only valid as initial layout for linear images, preserves memory contents
                    // Make sure host writes have been finished
                    imageMemoryBarrier.srcAccessMask = VkAccessFlags.HostWrite;
                    break;

                case VkImageLayout.ColorAttachmentOptimal:
                    // Image is a color attachment
                    // Make sure any writes to the color buffer have been finished
                    imageMemoryBarrier.srcAccessMask = VkAccessFlags.ColorAttachmentWrite;
                    break;

                case VkImageLayout.DepthStencilAttachmentOptimal:
                    // Image is a depth/stencil attachment
                    // Make sure any writes to the depth/stencil buffer have been finished
                    imageMemoryBarrier.srcAccessMask = VkAccessFlags.DepthStencilAttachmentWrite;
                    break;

                case VkImageLayout.TransferSrcOptimal:
                    // Image is a transfer source 
                    // Make sure any reads from the image have been finished
                    imageMemoryBarrier.srcAccessMask = VkAccessFlags.TransferRead;
                    break;

                case VkImageLayout.TransferDstOptimal:
                    // Image is a transfer destination
                    // Make sure any writes to the image have been finished
                    imageMemoryBarrier.srcAccessMask = VkAccessFlags.TransferWrite;
                    break;

                case VkImageLayout.ShaderReadOnlyOptimal:
                    // Image is read by a shader
                    // Make sure any shader reads from the image have been finished
                    imageMemoryBarrier.srcAccessMask = VkAccessFlags.ShaderRead;
                    break;
            }

            // Target layouts (new)
            // Destination access mask controls the dependency for the new image layout
            switch (newImageLayout) {
                case VkImageLayout.TransferDstOptimal:
                    // Image will be used as a transfer destination
                    // Make sure any writes to the image have been finished
                    imageMemoryBarrier.dstAccessMask = VkAccessFlags.TransferWrite;
                    break;

                case VkImageLayout.TransferSrcOptimal:
                    // Image will be used as a transfer source
                    // Make sure any reads from and writes to the image have been finished
                    imageMemoryBarrier.srcAccessMask = imageMemoryBarrier.srcAccessMask | VkAccessFlags.TransferRead;
                    imageMemoryBarrier.dstAccessMask = VkAccessFlags.TransferRead;
                    break;

                case VkImageLayout.ColorAttachmentOptimal:
                    // Image will be used as a color attachment
                    // Make sure any writes to the color buffer have been finished
                    imageMemoryBarrier.srcAccessMask = VkAccessFlags.TransferRead;
                    imageMemoryBarrier.dstAccessMask = VkAccessFlags.ColorAttachmentWrite;
                    break;

                case VkImageLayout.DepthStencilAttachmentOptimal:
                    // Image layout will be used as a depth/stencil attachment
                    // Make sure any writes to depth/stencil buffer have been finished
                    imageMemoryBarrier.dstAccessMask = imageMemoryBarrier.dstAccessMask | VkAccessFlags.DepthStencilAttachmentWrite;
                    break;

                case VkImageLayout.ShaderReadOnlyOptimal:
                    // Image will be read in a shader (sampler, input attachment)
                    // Make sure any writes to the image have been finished
                    if (imageMemoryBarrier.srcAccessMask == 0) {
                        imageMemoryBarrier.srcAccessMask = VkAccessFlags.HostWrite | VkAccessFlags.TransferWrite;
                    }
                    imageMemoryBarrier.dstAccessMask = VkAccessFlags.ShaderRead;
                    break;
            }

            // Put barrier inside setup command buffer
            VulkanNative.vkCmdPipelineBarrier (
                cmdbuffer,
                srcStageMask,
                dstStageMask,
                0,
                0, null,
                0, null,
                1, ref imageMemoryBarrier);
        }
    }
}
