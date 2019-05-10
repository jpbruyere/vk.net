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
using VK;
using static VK.Vk;

namespace CVKL {

    /// <summary>
    /// Base class for HostBuffer and GPUBuffer
    /// </summary>
    public class Buffer : Resource {
        internal VkBuffer handle;
		protected VkBufferCreateInfo createInfo = VkBufferCreateInfo.New ();

		public VkDescriptorBufferInfo Descriptor;
		public VkBuffer Handle => handle;
		public VkBufferCreateInfo Infos => createInfo;
        
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
