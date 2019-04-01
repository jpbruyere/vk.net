//
// OveridedStructs.cs
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

namespace Vulkan
{
    [StructLayout (LayoutKind.Explicit)]
    public unsafe struct VkClearColorValue
    {
        [FieldOffset (0)]
        public fixed float float32[4];
        [FieldOffset (0)]
        public fixed int int32[4];
        [FieldOffset (0)]
        public fixed uint uint32[4];

        public VkClearColorValue (float r, float g, float b, float a = 1.0f) : this ()
        {
            fixed (float* tmp = float32) {
                tmp[0] = r;
                tmp[1] = g;
                tmp[2] = b;
                tmp[3] = a;
            }
        }

        public VkClearColorValue (int r, int g, int b, int a = 255) : this ()
        {
            fixed (int* tmp = int32)
            {
                tmp[0] = r;
                tmp[1] = g;
                tmp[2] = b;
                tmp[3] = a;
            }
        }

        public VkClearColorValue (uint r, uint g, uint b, uint a = 255) : this ()
        {
            fixed (uint* tmp = uint32)
            {
                tmp[0] = r;
                tmp[1] = g;
                tmp[2] = b;
                tmp[3] = a;
            }
        }
    }

    [StructLayout (LayoutKind.Explicit)]
    public partial struct VkClearValue
    {
        [FieldOffset (0)]
        public VkClearColorValue color;
        [FieldOffset (0)]
        public VkClearDepthStencilValue depthStencil;
    }

    [StructLayout (LayoutKind.Sequential)]
    public partial struct VkMemoryType
    {
        public VkMemoryPropertyFlags propertyFlags;
        public uint heapIndex;
    }

    [StructLayout (LayoutKind.Sequential)]
    public partial struct VkMemoryHeap
    {
        public ulong size;
        public VkMemoryHeapFlags flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPhysicalDeviceMemoryProperties
    {
        public uint memoryTypeCount;
        [MarshalAs (UnmanagedType.ByValArray, SizeConst = 32)]
        public VkMemoryType[] memoryTypes;
        public uint memoryHeapCount;
        [MarshalAs (UnmanagedType.ByValArray, SizeConst = 16)]
        public VkMemoryHeap[] memoryHeaps;
    }

    public static unsafe partial class VulkanNative
    {
		[Generator.CalliRewrite]
		public static unsafe void vkGetPhysicalDeviceMemoryProperties (VkPhysicalDevice physicalDevice, IntPtr pMemoryProperties) {
			throw new NotImplementedException ();
		}
		[Generator.CalliRewrite]
		public static unsafe void vkGetPhysicalDeviceMemoryProperties (VkPhysicalDevice physicalDevice, out VkPhysicalDeviceMemoryProperties pMemoryProperties) {
			throw new NotImplementedException ();
		}
		[Generator.CalliRewrite]
        public static unsafe VkResult vkMapMemory (VkDevice device, VkDeviceMemory memory, ulong offset, ulong size, uint flags, ref IntPtr ppData)
        {
            throw new NotImplementedException ();
        }
    }
}
