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
using System.Runtime.InteropServices;
using Vulkan;

using static Vulkan.VulkanNative;

namespace VKE {


    public class Image {
        VkImage handle;
        Device dev;
        public readonly VkImageCreateInfo CreateInfo;
        VkDeviceMemory devMem;
        UInt64 memSize;
        public VkDescriptorImageInfo Descriptor;
        bool imported = false;

        //public Image (Device device, VkBufferUsageFlags usage, VkMemoryPropertyFlags _memoryPropertyFlags, UInt64 size, byte[] data)
        //    : this (device, usage, _memoryPropertyFlags, size) {
        //    Map ();
        //    unsafe {
        //        Marshal.Copy (data, 0, mappedData, data.Length);
        //    }
        //    Unmap ();
        //}

        //public Image (Device device, VkBufferUsageFlags usage, VkMemoryPropertyFlags _memoryPropertyFlags, UInt64 size, IntPtr data)
        //    : this (device, usage, _memoryPropertyFlags, size) {
        //    Map ();
        //    unsafe {
        //        System.Buffer.MemoryCopy (data.ToPointer (), mappedData.ToPointer (), size, size);
        //    }
        //    Unmap ();
        //}
        /// <summary>
        /// Import vkImage handle into a new Image class, handle will be preserve on destruction.
        /// </summary>
        public Image (Device device, VkImage vkHandle, VkFormat format, VkImageUsageFlags usage, uint width, uint height) {
            dev = device;

            CreateInfo.imageType = VkImageType.Image2D;
            CreateInfo.format = format;
            CreateInfo.extent.width = width;
            CreateInfo.extent.height = height;
            CreateInfo.extent.depth = 1;
            CreateInfo.mipLevels = 1;
            CreateInfo.arrayLayers = 1;
            CreateInfo.samples = VkSampleCountFlags.Count1;
            CreateInfo.tiling = VkImageTiling.Optimal;
            CreateInfo.usage = usage;

            handle = vkHandle;
            imported = true;
        }
        public Image (Device device, VkFormat format, VkImageUsageFlags usage, VkMemoryPropertyFlags _memoryPropertyFlags,
            uint width, uint height,
            VkImageType type = VkImageType.Image2D, VkSampleCountFlags samples = VkSampleCountFlags.Count1,
            VkImageTiling tiling = VkImageTiling.Optimal, uint mipsLevels = 1, uint layers = 1) {

            dev = device;

            VkImageCreateInfo info = VkImageCreateInfo.New ();
                    
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

            unsafe {
                Utils.CheckResult (vkCreateImage (dev.VkDev, ref info, null, out handle));


                VkMemoryRequirements memReqs;
                vkGetImageMemoryRequirements (dev.VkDev, handle, out memReqs);

                VkMemoryAllocateInfo memInfo = VkMemoryAllocateInfo.New ();
                memInfo.allocationSize = memReqs.size;
                memInfo.memoryTypeIndex = dev.GetMemoryTypeIndex (memReqs.memoryTypeBits, _memoryPropertyFlags);

                Utils.CheckResult (vkAllocateMemory (device.VkDev, ref memInfo, IntPtr.Zero, out devMem));

                memSize = memInfo.allocationSize;

                Utils.CheckResult(vkBindImageMemory (dev.VkDev, handle, devMem, 0));
            }

            CreateInfo = info;
        }

        IntPtr mappedData;
        public IntPtr MappedData => mappedData;

        public void Map (ulong size = WholeSize, ulong offset = 0) {
            Utils.CheckResult (vkMapMemory (dev.VkDev, devMem, offset, size, 0, ref mappedData));
        }

        public void CreateView (VkImageViewType type = VkImageViewType.Image2D, VkImageAspectFlags aspectFlags = VkImageAspectFlags.Color,
            uint baseMipLevel = 0, uint levelCount = 1, uint baseArrayLayer = 0, uint layerCount = 1) {

            VkImageView view = default(VkImageView);
            VkImageViewCreateInfo info = VkImageViewCreateInfo.New ();
            info.image = handle;
            info.viewType = VkImageViewType.Image2D;
            info.format = CreateInfo.format;
            info.components.r = VkComponentSwizzle.R;
            info.components.g = VkComponentSwizzle.G;
            info.components.b = VkComponentSwizzle.B;
            info.components.a = VkComponentSwizzle.A;
            info.subresourceRange.aspectMask = aspectFlags;
            info.subresourceRange.baseMipLevel = baseMipLevel;
            info.subresourceRange.levelCount = levelCount;
            info.subresourceRange.baseArrayLayer = baseArrayLayer;
            info.subresourceRange.layerCount = layerCount;

            Utils.CheckResult (vkCreateImageView (dev.VkDev, ref info, IntPtr.Zero, out view));

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

        public void SetupDescriptor () {
            //descriptor.imageView;
            //descriptor.imageLayout;
        }

        public void Unmap () {
            if (mappedData == IntPtr.Zero)
                return;
            vkUnmapMemory (dev.VkDev, devMem);
            mappedData = IntPtr.Zero;
        }

        public void Destroy () {
            Unmap ();
            if (Descriptor.imageView.Handle != 0)
                dev.DestroyImageView (Descriptor.imageView);
            if (imported)
                return;
            vkFreeMemory (dev.VkDev, devMem, IntPtr.Zero);
            dev.DestroyImage (handle);
        }
    }
}
