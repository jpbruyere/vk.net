//
// GPUBuffer.cs
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
using VK;

namespace CVKL {

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

		public int ElementCount { get; private set; }

		public GPUBuffer (Device device, VkBufferUsageFlags usage, int elementCount)
            : base (device, usage, (ulong)(Marshal.SizeOf<T> () * elementCount)) {
			ElementCount = elementCount;
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
}
