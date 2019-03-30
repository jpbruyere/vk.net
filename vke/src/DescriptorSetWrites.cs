//
// DescriptorSetWrites.cs
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
using Vulkan;
using static Vulkan.VulkanNative;

namespace VKE {
    public class DescriptorSetWrites : IDisposable {
        Device dev;
        bool disposed;
        List<GCHandle> handles = new List<GCHandle> ();
        NativeList<VkWriteDescriptorSet> WriteDescriptorSets = new NativeList<VkWriteDescriptorSet> ();
        public DescriptorSetWrites (Device device) {
            dev = device;
        }
        public void AddWriteInfo (DescriptorSet destSet, VkDescriptorSetLayoutBinding binding, VkDescriptorBufferInfo descriptor) {
            GCHandle handle = GCHandle.Alloc (descriptor, GCHandleType.Pinned);
            VkWriteDescriptorSet wds = VkWriteDescriptorSet.New ();
            wds.descriptorType = binding.descriptorType;
            wds.descriptorCount = binding.descriptorCount;
            wds.dstBinding = binding.binding;
            wds.dstSet = destSet.handle;
            unsafe {
                wds.pBufferInfo = (VkDescriptorBufferInfo*)handle.AddrOfPinnedObject ().ToPointer ();
            }
            WriteDescriptorSets.Add (wds);
            handles.Add (handle);
        }
        public void AddWriteInfo (DescriptorSet destSet, VkDescriptorSetLayoutBinding binding, VkDescriptorImageInfo descriptor) {
            GCHandle handle = GCHandle.Alloc (descriptor, GCHandleType.Pinned);
            VkWriteDescriptorSet wds = VkWriteDescriptorSet.New ();
            wds.descriptorType = binding.descriptorType;
            wds.descriptorCount = binding.descriptorCount;
            wds.dstBinding = binding.binding;
            wds.dstSet = destSet.handle;
            unsafe {
                wds.pImageInfo = (VkDescriptorImageInfo*)handle.AddrOfPinnedObject ().ToPointer ();
            }
            WriteDescriptorSets.Add (wds);
            handles.Add (handle);
        }

        public void Update () {
            vkUpdateDescriptorSets (dev.VkDev, WriteDescriptorSets.Count, WriteDescriptorSets.Data, 0, IntPtr.Zero);
        }

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        protected virtual void Dispose (bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    WriteDescriptorSets.Dispose ();
                }

                foreach (GCHandle hnd in handles)
                    hnd.Free ();

                disposedValue = true;
            }
        }

        ~DescriptorSetWrites() {
            Dispose(false);
        }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        public void Dispose () {
            Dispose (true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
