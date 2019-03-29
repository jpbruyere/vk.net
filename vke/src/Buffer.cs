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

namespace VKE {


    public class Buffer {
        internal VkBuffer handle;
        Device dev;
        VkDeviceMemory devMem;
        UInt64 memSize;
        public VkDescriptorBufferInfo Descriptor;


        public void Update (object data, uint size) {
            Map ();
            GCHandle ptr = GCHandle.Alloc (data, GCHandleType.Pinned);
            unsafe {
                System.Buffer.MemoryCopy (ptr.AddrOfPinnedObject ().ToPointer (), mappedData.ToPointer (), size, size); 
                Unmap ();
            }
            ptr.Free ();
        }

        public Buffer (Device device, VkBufferUsageFlags usage, VkMemoryPropertyFlags _memoryPropertyFlags, UInt64 size, object data)
            : this (device, usage, _memoryPropertyFlags, size) {
            Map ();
            Update (data, (uint)size);
        }
        public Buffer (Device device, VkBufferUsageFlags usage, VkMemoryPropertyFlags _memoryPropertyFlags, UInt64 size, int[] data)
            : this (device, usage, _memoryPropertyFlags, size) {
            Map ();
            unsafe {
                Marshal.Copy (data, 0, mappedData, data.Length);
            }
            Unmap ();
        }
        public Buffer (Device device, VkBufferUsageFlags usage, VkMemoryPropertyFlags _memoryPropertyFlags, UInt64 size, uint[] data)
            : this (device, usage, _memoryPropertyFlags, size) {
            Map ();
            unsafe {
                fixed (uint* ptr = data) {
                    System.Buffer.MemoryCopy (ptr, mappedData.ToPointer (), size, size);
                    Unmap ();
                }
            }
        }
        public Buffer (Device device, VkBufferUsageFlags usage, VkMemoryPropertyFlags _memoryPropertyFlags, UInt64 size, float[] data)
            : this (device, usage, _memoryPropertyFlags, size) {
            Map ();
            unsafe {
                Marshal.Copy (data, 0, mappedData, data.Length);
            }
            Unmap ();
        }

        public Buffer (Device device, VkBufferUsageFlags usage, VkMemoryPropertyFlags _memoryPropertyFlags, UInt64 size, IntPtr data)
            : this (device, usage, _memoryPropertyFlags, size) {
            Map ();
            unsafe {
                System.Buffer.MemoryCopy (data.ToPointer (), mappedData.ToPointer (), size, size);
            }
            Unmap ();
        }
        public Buffer (Device device, VkBufferUsageFlags usage, VkMemoryPropertyFlags _memoryPropertyFlags, UInt64 size) {
            dev = device;

            VkBufferCreateInfo info = VkBufferCreateInfo.New ();
            info.size = size;
            info.usage = usage;
            info.sharingMode = VkSharingMode.Exclusive;
            unsafe {
                Utils.CheckResult (vkCreateBuffer (dev.VkDev, ref info, null, out handle));


                VkMemoryRequirements memReqs;
                vkGetBufferMemoryRequirements (dev.VkDev, handle, out memReqs);

                VkMemoryAllocateInfo memInfo = VkMemoryAllocateInfo.New ();
                memInfo.allocationSize = memReqs.size;
                memInfo.memoryTypeIndex = dev.GetMemoryTypeIndex (memReqs.memoryTypeBits, _memoryPropertyFlags);

                Utils.CheckResult (vkAllocateMemory (device.VkDev, ref memInfo, IntPtr.Zero, out devMem));

                memSize = memInfo.allocationSize;

                Utils.CheckResult(vkBindBufferMemory (dev.VkDev, handle, devMem, 0));
            }
            SetupDescriptor ();
        }

        IntPtr mappedData;
        public IntPtr MappedData => mappedData;

        public void Map (ulong size = WholeSize, ulong offset = 0) {
            Utils.CheckResult (vkMapMemory (dev.VkDev, devMem, offset, size, 0, ref mappedData));
        }

        public void SetupDescriptor (ulong size = WholeSize, ulong offset = 0) {
            Descriptor.buffer = handle;
            Descriptor.range = size;
            Descriptor.offset = offset;
        }

        public void Unmap () {
            if (mappedData == IntPtr.Zero)
                return;
            vkUnmapMemory (dev.VkDev, devMem);
            mappedData = IntPtr.Zero;
        }

        public void Destroy () {
            Unmap ();
            vkFreeMemory (dev.VkDev, devMem, IntPtr.Zero);
            vkDestroyBuffer (dev.VkDev, handle, IntPtr.Zero);
        }
    }
}
