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
using System.Runtime.CompilerServices;

using static Vulkan.VulkanNative;
using System.Collections.Generic;

namespace VKE {
    public class GPUBuffer : Buffer {
        public GPUBuffer (Device device, VkBufferUsageFlags usage, UInt64 size) 
        : base (device, usage, VkMemoryPropertyFlags.DeviceLocal, size){ 
        }
    }
    public class GPUBuffer<T> : GPUBuffer {
        public GPUBuffer (Device device, VkBufferUsageFlags usage, int elementCount)
            : base (device, usage, (ulong)(Marshal.SizeOf<T> () * elementCount)) {
        }
        public GPUBuffer (Queue staggingQ, CommandPool staggingCmdPool, VkBufferUsageFlags usage, T[] elements)
            : base (staggingQ.Dev, usage | VkBufferUsageFlags.TransferDst, (ulong)(Marshal.SizeOf<T> () * elements.Length)) {
			using (HostBuffer<T> stagging = new HostBuffer<T> (dev, VkBufferUsageFlags.TransferSrc, elements)) { 
				CommandBuffer cmd = staggingCmdPool.AllocateCommandBuffer ();
				cmd.Start (VkCommandBufferUsageFlags.OneTimeSubmit);

				stagging.CopyTo (cmd, this);

				cmd.End ();

				staggingQ.Submit (cmd);
				staggingQ.WaitIdle ();

				cmd.Free ();
			}
        }
    }
    public class HostBuffer<T> : HostBuffer {
        public HostBuffer (Device device, VkBufferUsageFlags usage, IList<T> data)
            : base (device, usage, (ulong)(Marshal.SizeOf<T> () * data.Count)) {
            Map ();
            Update (data, createInfo.size);
            Unmap ();
        }
        public HostBuffer (Device device, VkBufferUsageFlags usage, T[] data)
            : base (device, usage, (ulong)(Marshal.SizeOf<T> () * data.Length)) {
            Map ();
            Update (data, createInfo.size);
            Unmap ();
        }
    }
    public class HostBuffer : Buffer {
        public HostBuffer (Device device, VkBufferUsageFlags usage, UInt64 size)
                    : base (device, usage, VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent, size) {
        }
        public HostBuffer (Device device, VkBufferUsageFlags usage, object data)
            : base (device, usage, VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent, (ulong)Marshal.SizeOf(data)) {
            Map ();
            Update (data, createInfo.size);
            Unmap ();
        }
        public HostBuffer (Device device, VkBufferUsageFlags usage, UInt64 size, IntPtr data)
            : base (device, usage, VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent, size) {
            Map ();
            unsafe {
                System.Buffer.MemoryCopy (data.ToPointer (), mappedData.ToPointer (), size, size);
            }
            Unmap ();
        }
    }

    public class Buffer : Resource {
        internal VkBuffer handle;
        public VkDescriptorBufferInfo Descriptor;
        protected VkBufferCreateInfo createInfo = VkBufferCreateInfo.New ();


        public Buffer (Device device, VkBufferUsageFlags usage, VkMemoryPropertyFlags _memoryPropertyFlags, UInt64 size)
        : base (device, _memoryPropertyFlags) {

            createInfo.size = size;
            createInfo.usage = usage;
            createInfo.sharingMode = VkSharingMode.Exclusive;

            Utils.CheckResult (vkCreateBuffer (dev.VkDev, ref createInfo, IntPtr.Zero, out handle));

            Activate ();//DONT OVERRIDE Activate in derived classes!!!!
        }

        public override void Activate () {
            allocateMemory ();
            bindMemory (0);
            SetupDescriptor ();
        }


        public void SetupDescriptor (ulong size = WholeSize, ulong offset = 0) {
            Descriptor.buffer = handle;
            Descriptor.range = size;
            Descriptor.offset = offset;
        }

        public void CopyTo (CommandBuffer cmd, Image img) {
            img.SetLayout (cmd, VkImageAspectFlags.Color,
                VkImageLayout.Undefined, VkImageLayout.TransferDstOptimal,
                VkPipelineStageFlags.AllCommands, VkPipelineStageFlags.Transfer);

            VkBufferImageCopy bufferCopyRegion = new VkBufferImageCopy {
                imageExtent = img.CreateInfo.extent,
                imageSubresource = new VkImageSubresourceLayers(VkImageAspectFlags.Color)
            };

            vkCmdCopyBufferToImage (cmd.Handle, handle, img.handle, VkImageLayout.TransferDstOptimal, 1, ref bufferCopyRegion);

            img.SetLayout (cmd, VkImageAspectFlags.Color,
                VkImageLayout.TransferDstOptimal, VkImageLayout.ShaderReadOnlyOptimal,
                VkPipelineStageFlags.Transfer, VkPipelineStageFlags.AllGraphics);
        }
        public void CopyTo (CommandBuffer cmd, Buffer buff, ulong size = 0, ulong srcOffset = 0, ulong dstOffset = 0) {
            VkBufferCopy bufferCopy = new VkBufferCopy {
                size = (size == 0) ? deviceMemSize : size,
                srcOffset = srcOffset,
                dstOffset = dstOffset
            };
            vkCmdCopyBuffer (cmd.Handle, handle, buff.handle, 1, ref bufferCopy);
        }

        protected override VkMemoryRequirements getMemoryRequirements () {
            VkMemoryRequirements memReqs;
            vkGetBufferMemoryRequirements (dev.VkDev, handle, out memReqs);
            return memReqs;
        }

        protected override void bindMemory (ulong offset) {
            Utils.CheckResult (vkBindBufferMemory (dev.VkDev, handle, vkMemory, offset));
        }
        protected override void Dispose (bool disposing) {
			if (references>0)
				return;
            if (!isDisposed) {
                base.Dispose (disposing);
                if (!disposing)
                    System.Diagnostics.Debug.WriteLine ($"A Buffer Ressource was not properly disposed.");

                        
                vkDestroyBuffer (dev.VkDev, handle, IntPtr.Zero);

                isDisposed = true;
            }
        }
    }
}
