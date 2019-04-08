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
using System.IO;
using System.Numerics;

namespace VK {
    public static partial class Utils {
        public static void CheckResult (VkResult result, string errorString = "Call failed") {
            if (result != VkResult.Success)
                throw new InvalidOperationException (errorString + ": " + result.ToString ());
        }
        public static float DegreesToRadians (float degrees) {
            return degrees * (float)Math.PI / 180f;
        }

		#region Extensions methods
		public static void FromFloatArray (ref Vector3 v, float[] floats) {
			if (floats.Length > 0)
				v.X = floats[0];
			if (floats.Length > 1)
				v.Y = floats[1];
			if (floats.Length > 2)
				v.Z = floats[2];
		}
		public static void FromFloatArray (ref Vector4 v, float[] floats) {
			if (floats.Length > 0)
				v.X = floats[0];
			if (floats.Length > 1)
				v.Y = floats[1];
			if (floats.Length > 2)
				v.Z = floats[2];
			if (floats.Length > 3)
				v.W = floats[3];            
        }
        public static void FromFloatArray (ref Quaternion v, float[] floats) {
			if (floats.Length > 0)
				v.X = floats[0];
			if (floats.Length > 1)
				v.Y = floats[1];
			if (floats.Length > 2)
				v.Z = floats[2];
			if (floats.Length > 3)
				v.W = floats[3];
		}
		public static void FromByteArray (ref Vector2 v, byte[] byteArray, int offset) {
            v.X = BitConverter.ToSingle (byteArray, offset);
            v.Y = BitConverter.ToSingle (byteArray, offset + 4);
        }
        public static void FromByteArray (ref Vector3 v, byte[] byteArray, int offset) {
            v.X = BitConverter.ToSingle (byteArray, offset);
            v.Y = BitConverter.ToSingle (byteArray, offset + 4);
            v.Z = BitConverter.ToSingle (byteArray, offset + 8);
        }
        public static void FromByteArray (ref Vector4 v, byte[] byteArray, int offset) {
            v.X = BitConverter.ToSingle (byteArray, offset);
            v.Y = BitConverter.ToSingle (byteArray, offset + 4);
            v.Z = BitConverter.ToSingle (byteArray, offset + 8);
            v.W = BitConverter.ToSingle (byteArray, offset + 12);
        }
        public static void FromByteArray (ref Quaternion v, byte[] byteArray, int offset) {
            v.X = BitConverter.ToSingle (byteArray, offset);
            v.Y = BitConverter.ToSingle (byteArray, offset + 4);
            v.Z = BitConverter.ToSingle (byteArray, offset + 8);
            v.W = BitConverter.ToSingle (byteArray, offset + 12);
        }
		#endregion        

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

        public static void setImageLayout (
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
            imageMemoryBarrier.srcQueueFamilyIndex = Vk.QueueFamilyIgnored;
            imageMemoryBarrier.dstQueueFamilyIndex = Vk.QueueFamilyIgnored;
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
            Vk.vkCmdPipelineBarrier (
                cmdbuffer,
                srcStageMask,
                dstStageMask,
                0,
                0, IntPtr.Zero,
                0, IntPtr.Zero,
                1, ref imageMemoryBarrier);
        }
    }
}
