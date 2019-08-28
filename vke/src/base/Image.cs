﻿//
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
using System.Diagnostics;
using VK;

using static VK.Vk;

namespace CVKL {
	/// <summary>
	/// Combined Image/Descriptor class. Optional Sampler and View are disposed with the vkImage. If multiple view/sampler have to be
	/// created for the same vkImage, you may call the constructor accepting a vkImage as parameter to import an existing one. vkImage handle of
	/// such imported image will not be disposed with the sampler and the view.
	/// </summary>
	public class Image : Resource {
		public static VkFormat DefaultTextureFormat = VkFormat.R8g8b8a8Unorm;

		internal VkImage handle; 
        VkImageCreateInfo info = VkImageCreateInfo.New();

		/// <summary>
		/// if true, the vkImage handle will not be destroyed on dispose, useful to create image for swapchain
		/// </summary>
        bool imported;

        public VkDescriptorImageInfo Descriptor;
        public VkImageCreateInfo CreateInfo => info;
        public VkExtent3D Extent => info.extent;
        public VkFormat Format => info.format;
		public VkImage Handle => handle;
		public uint Width => CreateInfo.extent.width;
		public uint Height => CreateInfo.extent.height;

		protected override VkDebugMarkerObjectNameInfoEXT DebugMarkerInfo
			=> new VkDebugMarkerObjectNameInfoEXT(VkDebugReportObjectTypeEXT.ImageEXT, handle.Handle);

		#region CTORS
        public Image (Device device, VkFormat format, VkImageUsageFlags usage, VkMemoryPropertyFlags _memoryPropertyFlags,
            uint width, uint height,
            VkImageType type = VkImageType.Image2D, VkSampleCountFlags samples = VkSampleCountFlags.SampleCount1,
            VkImageTiling tiling = VkImageTiling.Optimal, uint mipsLevels = 1, uint layers = 1, uint depth = 1,
			VkImageCreateFlags createFlags = 0)
            : base (device, _memoryPropertyFlags) {

            info.imageType = type;
            info.format = format;
            info.extent.width = width;
            info.extent.height = height;
            info.extent.depth = depth;
            info.mipLevels = mipsLevels;
            info.arrayLayers = layers;
            info.samples = samples;
            info.tiling = tiling;
            info.usage = usage;
            info.initialLayout = (tiling == VkImageTiling.Optimal) ? VkImageLayout.Undefined : VkImageLayout.Preinitialized;
            info.sharingMode = VkSharingMode.Exclusive;
			info.flags = createFlags;

            Activate ();//DONT OVERRIDE Activate in derived classes!!!!
        }

		/// <summary>
		/// Import vkImage handle into a new Image class, handle will be preserve on destruction.
		/// </summary>
		public Image (Device device, VkImage vkHandle, VkFormat format, VkImageUsageFlags usage, uint width, uint height)
		: base (device, VkMemoryPropertyFlags.DeviceLocal) {
			info.imageType = VkImageType.Image2D;
			info.format = format;
			info.extent.width = width;
			info.extent.height = height;
			info.extent.depth = 1;
			info.mipLevels = 1;
			info.arrayLayers = 1;
			info.samples = VkSampleCountFlags.SampleCount1;
			info.tiling = VkImageTiling.Optimal;
			info.usage = usage;

			handle = vkHandle;
			imported = true;

			state = ActivableState.Activated;
			references++;//increment ref because it is bound to swapchain
		}
		#endregion

		public static uint ComputeMipLevels(uint size) => (uint)Math.Floor (Math.Log (size)) + 1;
		public static uint ComputeMipLevels (int width, int height) =>	(uint)Math.Floor (Math.Log (Math.Max (width, height))) + 1;

		public static bool CheckFormatIsSupported (VkImageUsageFlags usage, VkFormatFeatureFlags phyFormatSupport) {
			if (usage.HasFlag (VkImageUsageFlags.TransferSrc) & !phyFormatSupport.HasFlag (VkFormatFeatureFlags.TransferSrc))
				return false;
			if (usage.HasFlag (VkImageUsageFlags.TransferDst) & !phyFormatSupport.HasFlag (VkFormatFeatureFlags.TransferSrc))
				return false;
			if (usage.HasFlag (VkImageUsageFlags.Sampled) & !phyFormatSupport.HasFlag (VkFormatFeatureFlags.SampledImage))
				return false;
			if (usage.HasFlag (VkImageUsageFlags.Storage) & !phyFormatSupport.HasFlag (VkFormatFeatureFlags.StorageImage))
				return false;
			if (usage.HasFlag (VkImageUsageFlags.ColorAttachment) & !phyFormatSupport.HasFlag (VkFormatFeatureFlags.ColorAttachment))
				return false;
			if (usage.HasFlag (VkImageUsageFlags.DepthStencilAttachment) & !phyFormatSupport.HasFlag (VkFormatFeatureFlags.DepthStencilAttachment))
				return false;
			/*if (usage.HasFlag (VkImageUsageFlags.TransientAttachment) ^ phyFormatSupport.HasFlag (VkFormatFeatureFlags.))
				return false;*/
			if (usage.HasFlag (VkImageUsageFlags.InputAttachment) & !phyFormatSupport.HasFlag (VkFormatFeatureFlags.SampledImage))
				return false;
			/*if (usage.HasFlag (VkImageUsageFlags.ShadingRateImageNV) ^ phyFormatSupport.HasFlag (VkFormatFeatureFlags.TransferSrc))
				return false;
			if (usage.HasFlag (VkImageUsageFlags.FragmentDensityMapEXT) ^ phyFormatSupport.HasFlag (VkFormatFeatureFlags.TransferSrc))
				return false;*/
			return true;
		}
		/// <summary>
		/// Load image from byte array containing full image file (jpg, png,...)
		/// </summary>
		public static Image Load (Device dev, Queue staggingQ, CommandPool staggingCmdPool,
			byte[] bitmap, VkFormat format = VkFormat.Undefined,
			VkMemoryPropertyFlags memoryProps = VkMemoryPropertyFlags.DeviceLocal,
			VkImageTiling tiling = VkImageTiling.Optimal, bool generateMipmaps = true,
			VkImageType imageType = VkImageType.Image2D,
			VkImageUsageFlags usage = VkImageUsageFlags.Sampled | VkImageUsageFlags.TransferSrc | VkImageUsageFlags.TransferDst) {

			Image img = Load (dev, staggingQ, staggingCmdPool, bitmap.Pin (), (ulong)bitmap.Length, format, memoryProps, tiling, generateMipmaps,
				imageType, usage);
			bitmap.Unpin ();
			return img;
		}

		#region bitmap loading
		/// <summary>
		/// Load image from data pointed by IntPtr pointer containing full image file (jpg, png,...)
		/// </summary>
		public static Image Load (Device dev, Queue staggingQ, CommandPool staggingCmdPool,
			IntPtr bitmap, ulong bitmapByteCount, VkFormat format = VkFormat.Undefined,
			VkMemoryPropertyFlags memoryProps = VkMemoryPropertyFlags.DeviceLocal,
			VkImageTiling tiling = VkImageTiling.Optimal, bool generateMipmaps = true,
			VkImageType imageType = VkImageType.Image2D,
			VkImageUsageFlags usage = VkImageUsageFlags.Sampled | VkImageUsageFlags.TransferSrc | VkImageUsageFlags.TransferDst) {

			if (format == VkFormat.Undefined)
				format = DefaultTextureFormat;
			if (tiling == VkImageTiling.Optimal)
				usage |= VkImageUsageFlags.TransferDst;
			if (generateMipmaps)
				usage |= (VkImageUsageFlags.TransferSrc | VkImageUsageFlags.TransferDst);

			using (StbImage stbi = new StbImage (bitmap, bitmapByteCount)) {
				uint mipLevels = generateMipmaps ? ComputeMipLevels (stbi.Width, stbi.Height) : 1;

				Image img = new Image (dev, format, usage, memoryProps, (uint)stbi.Width, (uint)stbi.Height, imageType,
					VkSampleCountFlags.SampleCount1, tiling, mipLevels);

				img.load (staggingQ, staggingCmdPool, stbi.Handle, generateMipmaps);

				return img;
			}
		}

		/// <summary>
		/// Load bitmap into Image with stagging and mipmap generation if necessary
		/// and usage.
		/// </summary>
		public static Image Load (Device dev, Queue staggingQ, CommandPool staggingCmdPool,
			string path, VkFormat format = VkFormat.Undefined,
			VkMemoryPropertyFlags memoryProps = VkMemoryPropertyFlags.DeviceLocal,
			VkImageTiling tiling = VkImageTiling.Optimal, bool generateMipmaps = true,
			VkImageType imageType = VkImageType.Image2D,
			VkImageUsageFlags usage = VkImageUsageFlags.Sampled | VkImageUsageFlags.TransferSrc | VkImageUsageFlags.TransferDst) {

			if (format == VkFormat.Undefined)
				format = DefaultTextureFormat;
			if (tiling == VkImageTiling.Optimal)
				usage |= VkImageUsageFlags.TransferDst;
			if (generateMipmaps)
				usage |= (VkImageUsageFlags.TransferSrc | VkImageUsageFlags.TransferDst);

			using (StbImage stbi = new StbImage (path)) {
				uint mipLevels = generateMipmaps ? ComputeMipLevels (stbi.Width, stbi.Height) : 1;

				Image img = new Image (dev, format, usage, memoryProps, (uint)stbi.Width, (uint)stbi.Height, imageType,
					VkSampleCountFlags.SampleCount1, tiling, mipLevels);

				img.load (staggingQ, staggingCmdPool, stbi.Handle, generateMipmaps);

				return img;
			}
		}

		/// <summary>
		/// create host visible linear image without command from path
		/// </summary>
		public static Image Load (Device dev,
			string path, VkImageUsageFlags usage = VkImageUsageFlags.Sampled, bool reserveSpaceForMipmaps = true, VkFormat format = VkFormat.Undefined,
			VkMemoryPropertyFlags memoryProps = VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent,
			VkImageTiling tiling = VkImageTiling.Linear, VkImageType imageType = VkImageType.Image2D) {

			if (format == VkFormat.Undefined)
				format = DefaultTextureFormat;

			using (StbImage stbi = new StbImage (path)) {
				uint mipLevels = reserveSpaceForMipmaps ? ComputeMipLevels (stbi.Width, stbi.Height) : 1;

				Image img = new Image (dev, format, usage, memoryProps, (uint)stbi.Width, (uint)stbi.Height, imageType,
					VkSampleCountFlags.SampleCount1, tiling, mipLevels);

				img.Map ();
				stbi.CoptyTo (img.MappedData);
				img.Unmap ();

				return img;
			}
        }
		/// <summary>
		/// create host visible linear image without command from byte array
		/// </summary>
		public static Image Load (Device dev,
			byte[] bitmap, VkImageUsageFlags usage = VkImageUsageFlags.TransferSrc,
			VkFormat format = VkFormat.Undefined,
			VkMemoryPropertyFlags memoryProps = VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent,
			VkImageTiling tiling = VkImageTiling.Linear, bool generateMipmaps = false,
			VkImageType imageType = VkImageType.Image2D) {

			Image img = Load (dev, bitmap.Pin (), (ulong)bitmap.Length, usage, format, memoryProps, tiling, generateMipmaps,
				imageType);
			bitmap.Unpin ();
			return img;
		}
		/// <summary>
		/// create host visible linear image without command from data pointed by IntPtr pointer containing full image file (jpg, png,...)
		/// </summary>
		public static Image Load (Device dev,
			IntPtr bitmap, ulong bitmapByteCount, VkImageUsageFlags usage = VkImageUsageFlags.TransferSrc,
			VkFormat format = VkFormat.Undefined,
			VkMemoryPropertyFlags memoryProps = VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent,
			VkImageTiling tiling = VkImageTiling.Linear, bool generateMipmaps = false,
			VkImageType imageType = VkImageType.Image2D) {

			if (format == VkFormat.Undefined)
				format = DefaultTextureFormat;
			if (generateMipmaps)
				usage |= (VkImageUsageFlags.TransferSrc | VkImageUsageFlags.TransferDst);

			using (StbImage stbi = new StbImage (bitmap, bitmapByteCount)) {
				uint mipLevels = generateMipmaps ? ComputeMipLevels (stbi.Width, stbi.Height) : 1;

				Image img = new Image (dev, format, usage, memoryProps, (uint)stbi.Width, (uint)stbi.Height, imageType,
					VkSampleCountFlags.SampleCount1, tiling, mipLevels);

				img.Map ();
				stbi.CoptyTo (img.MappedData);
				img.Unmap ();

				return img;
			}
		}

		/// <summary>
		/// load bitmap from pointer
		/// </summary>
		void load (Queue staggingQ, CommandPool staggingCmdPool, IntPtr bitmap, bool generateMipmaps = true) {
			long size = info.extent.width * info.extent.height * 4 * info.extent.depth;

			if (MemoryFlags.HasFlag (VkMemoryPropertyFlags.HostVisible)) {
				Map ();
				unsafe {
					System.Buffer.MemoryCopy (bitmap.ToPointer (), MappedData.ToPointer (), size, size);
				}
				Unmap ();

				if (generateMipmaps)
					BuildMipmaps (staggingQ, staggingCmdPool);
			} else {
				using (HostBuffer stagging = new HostBuffer (Dev, VkBufferUsageFlags.TransferSrc, (UInt64)size, bitmap)) {

					CommandBuffer cmd = staggingCmdPool.AllocateAndStart (VkCommandBufferUsageFlags.OneTimeSubmit);

					stagging.CopyTo (cmd, this);
					if (generateMipmaps)
						BuildMipmaps (cmd);

					cmd.End ();
					staggingQ.Submit (cmd);
					staggingQ.WaitIdle ();
					cmd.Free ();
				}
			}
		}

		#endregion

		internal override void updateMemoryRequirements () {            
            vkGetImageMemoryRequirements (Dev.VkDev, handle, out memReqs);            
        }
		internal override void bindMemory () {
#if MEMORY_POOLS
			Utils.CheckResult (vkBindImageMemory (Dev.VkDev, handle, memoryPool.vkMemory, poolOffset));
#else
			Utils.CheckResult (vkBindImageMemory (Dev.VkDev, handle, vkMemory, 0));
#endif
		}
        public override void Activate () {
			if (state != ActivableState.Activated) {
				Utils.CheckResult (vkCreateImage (Dev.VkDev, ref info, IntPtr.Zero, out handle));
#if MEMORY_POOLS
				Dev.resourceManager.Add (this);
#else
				updateMemoryRequirements ();
				allocateMemory ();
				bindMemory ();
#endif

			}
            base.Activate ();
        }

		public void CreateView (VkImageViewType type = VkImageViewType.ImageView2D, VkImageAspectFlags aspectFlags = VkImageAspectFlags.Color,
			uint layerCount = 1,
            uint baseMipLevel = 0, int levelCount = -1, uint baseArrayLayer = 0,
			VkComponentSwizzle r = VkComponentSwizzle.R,
			VkComponentSwizzle g = VkComponentSwizzle.G,
			VkComponentSwizzle b = VkComponentSwizzle.B,
			VkComponentSwizzle a = VkComponentSwizzle.A) {

            VkImageView view = default(VkImageView);
            VkImageViewCreateInfo viewInfo = VkImageViewCreateInfo.New();
            viewInfo.image = handle;
            viewInfo.viewType = type;
            viewInfo.format = Format;
            viewInfo.components.r = r;
            viewInfo.components.g = g;
            viewInfo.components.b = b;
            viewInfo.components.a = a;
            viewInfo.subresourceRange.aspectMask = aspectFlags;
			viewInfo.subresourceRange.baseMipLevel = baseMipLevel;
			viewInfo.subresourceRange.levelCount = levelCount < 0 ? info.mipLevels : (uint)levelCount;
			viewInfo.subresourceRange.baseArrayLayer = baseArrayLayer;
			viewInfo.subresourceRange.layerCount = layerCount;

            Utils.CheckResult (vkCreateImageView (Dev.VkDev, ref viewInfo, IntPtr.Zero, out view));

            if (Descriptor.imageView.Handle != 0)
                Dev.DestroyImageView (Descriptor.imageView);
            Descriptor.imageView = view;
        }

		public void CreateSampler (VkSamplerAddressMode addressMode, VkFilter minFilter = VkFilter.Linear,
				VkFilter magFilter = VkFilter.Linear, VkSamplerMipmapMode mipmapMode = VkSamplerMipmapMode.Linear,
				float maxAnisotropy = 1.0f, float minLod = 0.0f, float maxLod = -1f) {
			CreateSampler (minFilter, magFilter, mipmapMode, addressMode, maxAnisotropy, minLod, maxLod);
		}

		public void CreateSampler (VkFilter minFilter = VkFilter.Linear, VkFilter magFilter = VkFilter.Linear,
                               VkSamplerMipmapMode mipmapMode = VkSamplerMipmapMode.Linear, VkSamplerAddressMode addressMode = VkSamplerAddressMode.Repeat,
            float maxAnisotropy = 1.0f, float minLod = 0.0f, float maxLod = -1f) {
            VkSampler sampler;
            VkSamplerCreateInfo sampInfo = VkSamplerCreateInfo.New();
            sampInfo.maxAnisotropy = maxAnisotropy;
			sampInfo.maxAnisotropy = 1.0f;// device->enabledFeatures.samplerAnisotropy ? device->properties.limits.maxSamplerAnisotropy : 1.0f;
			//samplerInfo.anisotropyEnable = device->enabledFeatures.samplerAnisotropy;
			sampInfo.addressModeU = addressMode;
            sampInfo.addressModeV = addressMode;
            sampInfo.addressModeW = addressMode;
            sampInfo.magFilter = magFilter;
            sampInfo.minFilter = minFilter;
            sampInfo.mipmapMode = mipmapMode;
            sampInfo.minLod = minLod;
            sampInfo.maxLod = maxLod < 0f ? info.mipLevels > 1 ? info.mipLevels : 0 : maxLod;
			sampInfo.compareOp = VkCompareOp.Never;
			sampInfo.borderColor = VkBorderColor.FloatOpaqueWhite;

            Utils.CheckResult (vkCreateSampler (Dev.VkDev, ref sampInfo, IntPtr.Zero, out sampler));

            if (Descriptor.sampler.Handle != 0)
                Dev.DestroySampler (Descriptor.sampler);
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
                levelCount = CreateInfo.mipLevels,
                layerCount = CreateInfo.arrayLayers,
            };
            SetLayout (cmdbuffer, oldImageLayout, newImageLayout, subresourceRange, srcStageMask, dstStageMask);
        }
		public void SetLayout (
			CommandBuffer cmdbuffer,
			VkImageAspectFlags aspectMask,
			VkAccessFlags srcAccessMask,
			VkAccessFlags dstAccessMask,
			VkImageLayout oldImageLayout,
			VkImageLayout newImageLayout,
			VkPipelineStageFlags srcStageMask = VkPipelineStageFlags.AllCommands,
			VkPipelineStageFlags dstStageMask = VkPipelineStageFlags.AllCommands) {
			VkImageSubresourceRange subresourceRange = new VkImageSubresourceRange {
				aspectMask = aspectMask,
				baseMipLevel = 0,
				levelCount = CreateInfo.mipLevels,
				layerCount = CreateInfo.arrayLayers,
			};
			SetLayout (cmdbuffer, srcAccessMask, dstAccessMask, oldImageLayout, newImageLayout, subresourceRange, srcStageMask, dstStageMask);
		}
		public void SetLayout (
			CommandBuffer cmdbuffer,
			VkAccessFlags srcAccessMask,
			VkAccessFlags dstAccessMask,
			VkImageLayout oldImageLayout,
			VkImageLayout newImageLayout,
			VkImageSubresourceRange subresourceRange,
			VkPipelineStageFlags srcStageMask = VkPipelineStageFlags.AllCommands,
			VkPipelineStageFlags dstStageMask = VkPipelineStageFlags.AllCommands) {

			VkImageMemoryBarrier imageMemoryBarrier = VkImageMemoryBarrier.New ();
			imageMemoryBarrier.srcQueueFamilyIndex = Vk.QueueFamilyIgnored;
			imageMemoryBarrier.dstQueueFamilyIndex = Vk.QueueFamilyIgnored;
			imageMemoryBarrier.oldLayout = oldImageLayout;
			imageMemoryBarrier.newLayout = newImageLayout;
			imageMemoryBarrier.image = handle;
			imageMemoryBarrier.subresourceRange = subresourceRange;
			imageMemoryBarrier.srcAccessMask = srcAccessMask;
			imageMemoryBarrier.dstAccessMask = dstAccessMask;

			Vk.vkCmdPipelineBarrier (
				cmdbuffer.Handle,
				srcStageMask,
				dstStageMask,
				0,
				0, IntPtr.Zero,
				0, IntPtr.Zero,
				1, ref imageMemoryBarrier);

		}
		// Create an image memory barrier for changing the layout of
		// an image and put it into an active command buffer
		// See chapter 11.4 "Image Layout" for details
		public void SetLayout (
            CommandBuffer cmdbuffer,            
            VkImageLayout oldImageLayout,
            VkImageLayout newImageLayout,
            VkImageSubresourceRange subresourceRange,
            VkPipelineStageFlags srcStageMask = VkPipelineStageFlags.AllCommands,
            VkPipelineStageFlags dstStageMask = VkPipelineStageFlags.AllCommands) {
            // Create an image barrier object
            VkImageMemoryBarrier imageMemoryBarrier = VkImageMemoryBarrier.New();
            imageMemoryBarrier.srcQueueFamilyIndex = Vk.QueueFamilyIgnored;
            imageMemoryBarrier.dstQueueFamilyIndex = Vk.QueueFamilyIgnored;
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
            Vk.vkCmdPipelineBarrier (
                cmdbuffer.Handle,
                srcStageMask,
                dstStageMask,
                0,
                0,IntPtr.Zero,
                0, IntPtr.Zero,
                1, ref imageMemoryBarrier);
        }

		public void BuildMipmaps (Queue copyQ, CommandPool copyCmdPool) {
			if (info.mipLevels == 1) {
				Debug.WriteLine ("Invoking BuildMipmaps on image that has only one mipLevel");
				return;
			}
			CommandBuffer cmd = copyCmdPool.AllocateCommandBuffer ();

			cmd.Start (VkCommandBufferUsageFlags.OneTimeSubmit);
			BuildMipmaps (cmd);
			cmd.End ();

			copyQ.Submit (cmd);
			copyQ.WaitIdle ();

			cmd.Free ();
		}
		public void BuildMipmaps (CommandBuffer cmd) {
			VkImageSubresourceRange mipSubRange = new VkImageSubresourceRange (VkImageAspectFlags.Color, 0, 1, 0, info.arrayLayers);

			for (int i = 1; i < info.mipLevels; i++) {
				VkImageBlit imageBlit = new VkImageBlit {
					srcSubresource = new VkImageSubresourceLayers(VkImageAspectFlags.Color, info.arrayLayers, (uint)i - 1),
					srcOffsets_1 = new VkOffset3D ((int)info.extent.width >> (i - 1), (int)info.extent.height >> (i - 1), 1),
					dstSubresource = new VkImageSubresourceLayers (VkImageAspectFlags.Color, info.arrayLayers, (uint)i),
					dstOffsets_1 = new VkOffset3D ((int)info.extent.width >> i, (int)info.extent.height >> i, 1)
				};

				SetLayout (cmd, VkImageLayout.TransferDstOptimal, VkImageLayout.TransferSrcOptimal, mipSubRange,
					VkPipelineStageFlags.Transfer, VkPipelineStageFlags.Transfer);
				vkCmdBlitImage (cmd.Handle, handle, VkImageLayout.TransferSrcOptimal, handle, VkImageLayout.TransferDstOptimal, 1, ref imageBlit, VkFilter.Linear);
				mipSubRange.baseMipLevel = (uint)i;
			}
			SetLayout (cmd, VkImageAspectFlags.Color, VkImageLayout.TransferSrcOptimal, VkImageLayout.ShaderReadOnlyOptimal,
					VkPipelineStageFlags.Transfer, VkPipelineStageFlags.FragmentShader);
		}
		public void BlitTo (CommandBuffer cmd, Image dest) {
			VkImageBlit imageBlit = new VkImageBlit {
				srcSubresource = new VkImageSubresourceLayers (VkImageAspectFlags.Color, info.arrayLayers, 0),
				srcOffsets_1 = new VkOffset3D ((int)info.extent.width, (int)info.extent.height, 1),
				dstSubresource = new VkImageSubresourceLayers (VkImageAspectFlags.Color, info.arrayLayers, 0),
				dstOffsets_1 = new VkOffset3D ((int)dest.info.extent.width, (int)dest.info.extent.height, 1)
			};
			vkCmdBlitImage (cmd.Handle, handle, VkImageLayout.TransferSrcOptimal, dest.handle, VkImageLayout.TransferDstOptimal, 1, ref imageBlit, VkFilter.Linear);
		}
		public override string ToString () {
			return string.Format ($"{base.ToString ()}[0x{handle.Handle.ToString("x")}]");
		}
		#region IDisposable Support
        protected override void Dispose (bool disposing) {
			if (state == ActivableState.Activated) {
				if (Descriptor.sampler.Handle != 0)
                    Dev.DestroySampler (Descriptor.sampler);
                if (Descriptor.imageView.Handle != 0)
                    Dev.DestroyImageView (Descriptor.imageView);
				if (!imported) {
					base.Dispose (disposing);
					Dev.DestroyImage (handle);
				}
			}
			state = ActivableState.Disposed;
        }
		#endregion
	}
}
