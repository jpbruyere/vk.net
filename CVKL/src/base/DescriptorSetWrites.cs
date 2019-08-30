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
using VK;
using static VK.Vk;

namespace CVKL {
	/// <summary>
	/// Descriptor set writes is defined once, then update affect descriptors to write array
	/// </summary>
	public class DescriptorSetWrites {
		VkDescriptorSet? dstSetOverride = null;//when set, override target descriptors to update in each write
		public List<VkWriteDescriptorSet> WriteDescriptorSets = new List<VkWriteDescriptorSet> ();

		#region CTORS
		public DescriptorSetWrites () { }
		public DescriptorSetWrites (VkDescriptorSetLayoutBinding binding) {
			AddWriteInfo (binding);
		}
		public DescriptorSetWrites (DescriptorSet destSet, VkDescriptorSetLayoutBinding binding) {
			AddWriteInfo (destSet, binding);
		}
		public DescriptorSetWrites (params VkDescriptorSetLayoutBinding[] bindings) {
			AddWriteInfo (bindings);
		}
		public DescriptorSetWrites (DescriptorSet destSet, params VkDescriptorSetLayoutBinding[] bindings) {
			AddWriteInfo (destSet, bindings);
		}
		/// <summary>
		/// Configure the Write to update the full layout at once
		/// </summary>
		public DescriptorSetWrites (DescriptorSet destSet, DescriptorSetLayout layout) {
			foreach (VkDescriptorSetLayoutBinding binding in layout.Bindings) {
				AddWriteInfo (destSet, binding);
			}
		}
		/// <summary>
		/// Configure the Write to update the full layout at once
		/// </summary>
		public DescriptorSetWrites (DescriptorSetLayout layout) {
			foreach (VkDescriptorSetLayoutBinding binding in layout.Bindings) {
				AddWriteInfo (binding);
			}
		}
		#endregion

		/// <summary>
		/// Adds write info with a destination descriptor set, it could be overriden by calling Write
		/// with another descriptorSet in parametters
		/// </summary>
	 	public void AddWriteInfo (DescriptorSet destSet, params VkDescriptorSetLayoutBinding[] bindings) {
			foreach (VkDescriptorSetLayoutBinding binding in bindings)
				AddWriteInfo (destSet, binding);
        }
		/// <summary>
		/// Adds write info with a destination descriptor set, it could be overriden by calling Write
		/// with another descriptorSet in parametters
		/// </summary>
	 	public void AddWriteInfo (DescriptorSet destSet, VkDescriptorSetLayoutBinding binding) {
			VkWriteDescriptorSet wds = VkWriteDescriptorSet.New();
			wds.descriptorType = binding.descriptorType;
			wds.descriptorCount = binding.descriptorCount;
			wds.dstBinding = binding.binding;
			wds.dstSet = destSet.handle;
			WriteDescriptorSets.Add (wds);
        }
		/// <summary>
		/// Adds write info without specifying a destination descriptor set, this imply that on calling Write, you MUST
		/// provide a desDescriptor!
		/// </summary>
		public void AddWriteInfo (VkDescriptorSetLayoutBinding[] bindings) {
			foreach (VkDescriptorSetLayoutBinding binding in bindings)
				AddWriteInfo (binding);
		}
		/// <summary>
		/// Adds write info without specifying a destination descriptor set, this imply that on calling Write, you MUST
		/// provide a desDescriptor!
		/// </summary>
		public void AddWriteInfo (VkDescriptorSetLayoutBinding binding) {
            VkWriteDescriptorSet wds = VkWriteDescriptorSet.New();
            wds.descriptorType = binding.descriptorType;
            wds.descriptorCount = binding.descriptorCount;
            wds.dstBinding = binding.binding;
            WriteDescriptorSets.Add (wds); 
		}

		/// <summary>
		/// execute the descriptors writes providing a target descriptorSet
		/// </summary>
		public void Write (Device dev, DescriptorSet set, params object[] descriptors) {
			dstSetOverride = set.handle;
			Write (dev, descriptors);
		}

		/// <summary>
		/// execute the descriptors writes targeting descriptorSets setted on AddWriteInfo call
		/// </summary>
		public void Write (Device dev, params object[] descriptors) {
			//if (descriptors.Length != WriteDescriptorSets.Count)
			//	throw new Exception ("descriptors count must equal the WriteInfo count.");
			List<object> descriptorsLists = new List<object> ();//strore temp arrays of pDesc for unpinning
																//if descriptorCount>1
			int i = 0;
			int wdsPtr = 0;
			while (i < descriptors.Length) {
				int firstDescriptor = i;
				VkWriteDescriptorSet wds = WriteDescriptorSets[wdsPtr];
				if (dstSetOverride != null)
					wds.dstSet = dstSetOverride.Value.Handle;
				IntPtr pDescriptors = IntPtr.Zero;

				if (wds.descriptorCount > 1) {
					List<IntPtr> descPtrArray = new List<IntPtr> ();
					for (int d = 0; d < wds.descriptorCount; d++) {
						descPtrArray.Add (descriptors[i].Pin ());
						i++;
					}
					descriptorsLists.Add (descPtrArray);
					pDescriptors = descPtrArray.Pin ();
				} else {
					pDescriptors = descriptors[i].Pin ();
					i++;
				}
				if (descriptors[firstDescriptor] is VkDescriptorBufferInfo)
					wds.pBufferInfo = pDescriptors;
				else if (descriptors[firstDescriptor] is VkDescriptorImageInfo)
					wds.pImageInfo = pDescriptors;

				WriteDescriptorSets[wdsPtr] = wds;
				wdsPtr++;
			}
			vkUpdateDescriptorSets (dev.VkDev, (uint)WriteDescriptorSets.Count, WriteDescriptorSets.Pin (), 0, IntPtr.Zero);
			WriteDescriptorSets.Unpin ();
			foreach (object descArray in descriptorsLists) 
				descArray.Unpin ();
			for (i = 0; i < descriptors.Length; i++) 
				descriptors[i].Unpin ();			
		}
	}
	/// <summary>
	/// Descriptor set writes include descriptor in write addition with IDisposable model
	/// </summary>
	[Obsolete]
    public class DescriptorSetWrites2 : IDisposable {
        Device dev;
        List<VkWriteDescriptorSet> WriteDescriptorSets = new List<VkWriteDescriptorSet> ();
		List<object> descriptors = new List<object> ();
        
		public DescriptorSetWrites2 (Device device) {
            dev = device;
        }
        public void AddWriteInfo (DescriptorSet destSet, VkDescriptorSetLayoutBinding binding, VkDescriptorBufferInfo descriptor) {
			if (!descriptors.Contains (descriptor)) 
				descriptors.Add (descriptor);
            VkWriteDescriptorSet wds = VkWriteDescriptorSet.New();
            wds.descriptorType = binding.descriptorType;
            wds.descriptorCount = binding.descriptorCount;
            wds.dstBinding = binding.binding;
            wds.dstSet = destSet.handle;            
            wds.pBufferInfo = descriptor.Pin ();            
            
			WriteDescriptorSets.Add (wds);            
        }
        public void AddWriteInfo (DescriptorSet destSet, VkDescriptorSetLayoutBinding binding, VkDescriptorImageInfo descriptor) {
			if (!descriptors.Contains (descriptor)) 
				descriptors.Add (descriptor);
            VkWriteDescriptorSet wds = VkWriteDescriptorSet.New();
            wds.descriptorType = binding.descriptorType;
            wds.descriptorCount = binding.descriptorCount;
            wds.dstBinding = binding.binding;
            wds.dstSet = destSet.handle;            
            wds.pImageInfo = descriptor.Pin ();
            
            WriteDescriptorSets.Add (wds);            
        }

        public void Update () {
            vkUpdateDescriptorSets (dev.VkDev, (uint)WriteDescriptorSets.Count, WriteDescriptorSets.Pin (), 0, IntPtr.Zero);
			WriteDescriptorSets.Unpin ();
        }

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        protected virtual void Dispose (bool disposing) {
            if (!disposedValue) {
                foreach (object descriptor in descriptors)
                    descriptor.Unpin ();
                disposedValue = true;
            }
        }
        ~DescriptorSetWrites2() {
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
