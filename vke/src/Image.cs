//
// Buffer.cs
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
    

    public class Image : Resource {
        internal VkImage handle;
        VkImageCreateInfo info = VkImageCreateInfo.New();

        bool imported = false;

        public VkDescriptorImageInfo Descriptor;
        public VkImageCreateInfo CreateInfo => info;
        public VkExtent3D Extent => info.extent;
        public VkFormat Format => info.format;

        /// <summary>
        /// Import vkImage handle into a new Image class, handle will be preserve on destruction.
        /// </summary>
        public Image (Device device, VkImage vkHandle, VkFormat format, VkImageUsageFlags usage, uint width, uint height)
        : base (device, VkMemoryPropertyFlags.DeviceLocal) {
            dev = device;

            info.imageType = VkImageType.Image2D;
            info.format = format;
            info.extent.width = width;
            info.extent.height = height;
            info.extent.depth = 1;
            info.mipLevels = 1;
            info.arrayLayers = 1;
            info.samples = VkSampleCountFlags.Count1;
            info.tiling = VkImageTiling.Optimal;
            info.usage = usage;

            handle = vkHandle;
            imported = true;
        }
        public Image (Device device, VkFormat format, VkImageUsageFlags usage, VkMemoryPropertyFlags _memoryPropertyFlags,
            uint width, uint height,
            VkImageType type = VkImageType.Image2D, VkSampleCountFlags samples = VkSampleCountFlags.Count1,
            VkImageTiling tiling = VkImageTiling.Optimal, uint mipsLevels = 1, uint layers = 1)
            : base (device, _memoryPropertyFlags) {

            info.imageType = type;
            info.format = format;
            info.extent.width = width;
            info.extent.height = height;
            info.extent.depth = 1;
            info.mipLevels = mipsLevels;
            info.arrayLayers = layers;
            info.samples = samples;
            info.tiling = tiling;
            info.usage = usage;
            info.initialLayout = (tiling == VkImageTiling.Optimal) ? VkImageLayout.Undefined : VkImageLayout.Preinitialized;
            info.sharingMode = VkSharingMode.Exclusive;

            Activate ();//DONT OVERRIDE Activate in derived classes!!!!
        }

        public static Image Load (Device dev, string path) {
            int width, height, channels;
            IntPtr imgPtr = Stb.Load (path, out width, out height, out channels, 4);
            long size = width * height * 4;

            Image img = new Image(dev, VkFormat.R8g8b8a8Unorm, VkImageUsageFlags.Sampled,
                VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent, (uint)width, (uint)height, VkImageType.Image2D,
                VkSampleCountFlags.Count1, VkImageTiling.Linear);

            img.Map ();
            unsafe {
                System.Buffer.MemoryCopy (imgPtr.ToPointer (), img.MappedData.ToPointer (), size, size);
            }
            img.Unmap ();

            return img;
        }

        //public Image (Device dev, string path) {


        //    stagging = new HostBuffer (dev, VkBufferUsageFlags.TransferSrc, (UInt64)size);

        //    stagging.Map ((ulong)size);
        //    unsafe {
        //        System.Buffer.MemoryCopy (imgPtr.ToPointer (), stagging.MappedData.ToPointer (), size, size);
        //    }
        //    stagging.Unmap ();

        //    Stb.FreeImage (imgPtr);

        //    VKE.Image vkimg = new Image (dev, VkFormat.R8g8b8a8Unorm, VkImageUsageFlags.TransferDst | VkImageUsageFlags.Sampled,
        //        VkMemoryPropertyFlags.DeviceLocal, (uint)width, (uint)height);

        //    CommandBuffer cmd = cmdPool.AllocateCommandBuffer ();
        //    cmd.Start ();

        //    stagging.CopyTo (cmd, vkimg);

        //    cmd.End ();

        //    transferQ.Submit (cmd);

        //    dev.WaitIdle ();
        //    cmd.Destroy ();

        //    stagging.Dispose ();
        //}


        protected override VkMemoryRequirements getMemoryRequirements () {
            VkMemoryRequirements memReqs;
            vkGetImageMemoryRequirements (dev.VkDev, handle, out memReqs);
            return memReqs;
        }
        protected override void bindMemory (ulong offset) {
            Utils.CheckResult (vkBindImageMemory (dev.VkDev, handle, devMem, offset));
        }
        public override void Activate () {
            Utils.CheckResult (vkCreateImage (dev.VkDev, ref info, IntPtr.Zero, out handle));
            allocateMemory ();
            bindMemory (0);
            base.Activate ();
        }

        public void CreateView (VkImageViewType type = VkImageViewType.Image2D, VkImageAspectFlags aspectFlags = VkImageAspectFlags.Color,
            uint baseMipLevel = 0, uint levelCount = 1, uint baseArrayLayer = 0, uint layerCount = 1) {

            VkImageView view = default(VkImageView);
            VkImageViewCreateInfo viewInfo = VkImageViewCreateInfo.New ();
            viewInfo.image = handle;
            viewInfo.viewType = VkImageViewType.Image2D;
            viewInfo.format = Format;
            viewInfo.components.r = VkComponentSwizzle.R;
            viewInfo.components.g = VkComponentSwizzle.G;
            viewInfo.components.b = VkComponentSwizzle.B;
            viewInfo.components.a = VkComponentSwizzle.A;
            viewInfo.subresourceRange.aspectMask = aspectFlags;
            viewInfo.subresourceRange.baseMipLevel = baseMipLevel;
            viewInfo.subresourceRange.levelCount = levelCount;
            viewInfo.subresourceRange.baseArrayLayer = baseArrayLayer;
            viewInfo.subresourceRange.layerCount = layerCount;

            Utils.CheckResult (vkCreateImageView (dev.VkDev, ref viewInfo, IntPtr.Zero, out view));

            if (Descriptor.imageView.Handle != 0)
                dev.DestroyImageView (Descriptor.imageView);
            Descriptor.imageView = view;
        }

        public void CreateSampler (VkFilter minFilter = VkFilter.Linear, VkFilter magFilter = VkFilter.Linear,
                               VkSamplerMipmapMode mipmapMode = VkSamplerMipmapMode.Linear, VkSamplerAddressMode addressMode = VkSamplerAddressMode.Repeat,
            float maxAnisotropy = 1.0f, float minLod = 1.0f, float maxLod = 1.0f) {
            VkSampler sampler;
            VkSamplerCreateInfo info = VkSamplerCreateInfo.New ();
            info.maxAnisotropy = maxAnisotropy;
            info.addressModeU = addressMode;
            info.addressModeV = addressMode;
            info.addressModeW = addressMode;
            info.magFilter = magFilter;
            info.minFilter = minFilter;
            info.mipmapMode = mipmapMode;
            info.minLod = minLod;
            info.maxLod = maxLod;

            Utils.CheckResult (vkCreateSampler (dev.VkDev, ref info, IntPtr.Zero, out sampler));

            if (Descriptor.sampler.Handle != 0)
                dev.DestroySampler (Descriptor.sampler);
            Descriptor.sampler = sampler;
        }

        public void SetLayout (
            CommandBuffer cmdbuffer,
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
            SetLayout (cmdbuffer, aspectMask, oldImageLayout, newImageLayout, subresourceRange);
        }

        // Create an image memory barrier for changing the layout of
        // an image and put it into an active command buffer
        // See chapter 11.4 "Image Layout" for details
        public void SetLayout (
            CommandBuffer cmdbuffer,
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
            imageMemoryBarrier.image = handle;
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
                cmdbuffer.Handle,
                srcStageMask,
                dstStageMask,
                0,
                0,IntPtr.Zero,
                0, IntPtr.Zero,
                1, ref imageMemoryBarrier);
        }

        protected override void Dispose (bool disposing) {
            if (!isDisposed) {
                if (!disposing) 
                    System.Diagnostics.Debug.WriteLine ($"An Image Ressource was not properly disposed.");

                if (Descriptor.sampler.Handle != 0)
                    dev.DestroySampler (Descriptor.sampler);
                if (Descriptor.imageView.Handle != 0)
                    dev.DestroyImageView (Descriptor.imageView);
                if (imported)
                    return;

                if (!imported)
                    base.Dispose (disposing);

                dev.DestroyImage (handle);

                isDisposed = true;
            }
        }
    }
}
