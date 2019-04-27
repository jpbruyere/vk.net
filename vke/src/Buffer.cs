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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VK;
using static VK.Vk;

namespace CVKL {

	#region GPUBuffer
	/// <summary>
	/// Device local Buffer
	/// </summary>
	public class GPUBuffer : Buffer {
        public GPUBuffer (Device device, VkBufferUsageFlags usage, UInt64 size) 
        : base (device, usage, VkMemoryPropertyFlags.DeviceLocal, size){ 
        }
    }
	/// <summary>
	/// Device local Buffer
	/// </summary>
	public class GPUBuffer<T> : GPUBuffer {
        public GPUBuffer (Device device, VkBufferUsageFlags usage, int elementCount)
            : base (device, usage, (ulong)(Marshal.SizeOf<T> () * elementCount)) {
        }
        public GPUBuffer (Queue staggingQ, CommandPool staggingCmdPool, VkBufferUsageFlags usage, T[] elements)
            : base (staggingQ.Dev, usage | VkBufferUsageFlags.TransferDst, (ulong)(Marshal.SizeOf<T> () * elements.Length)) {
			using (HostBuffer<T> stagging = new HostBuffer<T> (Dev, VkBufferUsageFlags.TransferSrc, elements)) { 
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
	#endregion

	#region HostBuffer
	public class HostBuffer<T> : HostBuffer {
		public HostBuffer (Device device, VkBufferUsageFlags usage, uint arrayElementCount)
			: base (device, usage, (ulong)(Marshal.SizeOf<T> () * arrayElementCount)) {
		}
		public HostBuffer (Device device, VkBufferUsageFlags usage, IList<T> data, bool keepMapped = false)
            : base (device, usage, (ulong)(Marshal.SizeOf<T> () * data.Count)) {
            Map ();
            Update (data, createInfo.size);
			if (!keepMapped)
            	Unmap ();
        }
        public HostBuffer (Device device, VkBufferUsageFlags usage, T[] data, bool keepMapped = false)
            : base (device, usage, (ulong)(Marshal.SizeOf<T> () * data.Length)) {
            Map ();
            Update (data, createInfo.size);
			if (!keepMapped)
            	Unmap ();
        }
		public void Update (T[] data) {
			Update (data, (ulong)(Marshal.SizeOf<T> () * data.Length));
		}
	}
	/// <summary>
	/// Mappable Buffer with HostVisble and HostCoherent memory flags
	/// </summary>
	public class HostBuffer : Buffer {
        public HostBuffer (Device device, VkBufferUsageFlags usage, UInt64 size)
                    : base (device, usage, VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent, size) {
        }
        public HostBuffer (Device device, VkBufferUsageFlags usage, object data, bool keepMapped = false)
            : base (device, usage, VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent, (ulong)Marshal.SizeOf(data)) {
            Map ();
            Update (data, createInfo.size);
			if (!keepMapped)
            	Unmap ();
        }
        public HostBuffer (Device device, VkBufferUsageFlags usage, UInt64 size, IntPtr data, bool keepMapped = false)
            : base (device, usage, VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent, size) {
            Map ();
            unsafe {
                System.Buffer.MemoryCopy (data.ToPointer (), mappedData.ToPointer (), size, size);
            }
			if (!keepMapped)
            	Unmap ();
        }
    }
	#endregion

	/// <summary>
	/// Base class for HostBuffer and GPUBuffer
	/// </summary>
	public class Buffer : Resource {
        internal VkBuffer handle;
        public VkDescriptorBufferInfo Descriptor;
		public VkBuffer Handle => handle;
        protected VkBufferCreateInfo createInfo = VkBufferCreateInfo.New();
        
		protected override VkDebugMarkerObjectNameInfoEXT DebugMarkerInfo
			=> new VkDebugMarkerObjectNameInfoEXT(VkDebugReportObjectTypeEXT.BufferEXT, handle.Handle);

		#region CTORS
		public Buffer (Device device, VkBufferUsageFlags usage, VkMemoryPropertyFlags _memoryPropertyFlags, UInt64 size)
        : base (device, _memoryPropertyFlags) {

            createInfo.size = size;
            createInfo.usage = usage;
            createInfo.sharingMode = VkSharingMode.Exclusive;

            Activate ();//DONT OVERRIDE Activate in derived classes!!!!
        }
		#endregion

        public override void Activate () {
			if (state != ActivableState.Activated) {
				Utils.CheckResult (vkCreateBuffer (Dev.VkDev, ref createInfo, IntPtr.Zero, out handle));
				Dev.resourceManager.Add (this);
				SetupDescriptor ();
			}
			base.Activate ();
        }

		internal override void updateMemoryRequirements () {
			vkGetBufferMemoryRequirements (Dev.VkDev, handle, out memReqs);
		}

		internal override void bindMemory () {
			Utils.CheckResult (vkBindBufferMemory (Dev.VkDev, handle, memoryPool.vkMemory, poolOffset));
		}

		public void SetupDescriptor (ulong size = WholeSize, ulong offset = 0) {
            Descriptor.buffer = handle;
            Descriptor.range = size;
            Descriptor.offset = offset;
        }

        public void CopyTo (CommandBuffer cmd, Image img, VkImageLayout finalLayout = VkImageLayout.ShaderReadOnlyOptimal) {
            img.SetLayout (cmd, VkImageAspectFlags.Color,
                VkImageLayout.Undefined, VkImageLayout.TransferDstOptimal,
                VkPipelineStageFlags.AllCommands, VkPipelineStageFlags.Transfer);

            VkBufferImageCopy bufferCopyRegion = new VkBufferImageCopy {
                imageExtent = img.CreateInfo.extent,
                imageSubresource = new VkImageSubresourceLayers(VkImageAspectFlags.Color)
            };

            vkCmdCopyBufferToImage (cmd.Handle, handle, img.handle, VkImageLayout.TransferDstOptimal, 1, ref bufferCopyRegion);

            img.SetLayout (cmd, VkImageAspectFlags.Color,
                VkImageLayout.TransferDstOptimal, finalLayout,
                VkPipelineStageFlags.Transfer, VkPipelineStageFlags.AllGraphics);
        }
        public void CopyTo (CommandBuffer cmd, Buffer buff, ulong size = 0, ulong srcOffset = 0, ulong dstOffset = 0) {
            VkBufferCopy bufferCopy = new VkBufferCopy {
                size = (size == 0) ? AllocatedDeviceMemorySize : size,
                srcOffset = srcOffset,
                dstOffset = dstOffset
            };
            vkCmdCopyBuffer (cmd.Handle, handle, buff.handle, 1, ref bufferCopy);
        }
		public void Fill (CommandBuffer cmd, uint data, ulong size = 0, ulong offset = 0) {
			vkCmdFillBuffer (cmd.Handle, handle, offset, (size == 0) ? AllocatedDeviceMemorySize : size, data);
		}

		public override string ToString () {
			return string.Format ($"{base.ToString ()}[0x{handle.Handle.ToString("x")}]");
		}

		#region IDisposable Support
		protected override void Dispose (bool disposing) {
			if (state == ActivableState.Activated) {
				base.Dispose (disposing);
				vkDestroyBuffer (Dev.VkDev, handle, IntPtr.Zero);
			}
			state = ActivableState.Disposed;
        }
		#endregion
	}
}
