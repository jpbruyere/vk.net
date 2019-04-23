//
// AttachmentReference.cs
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
    public class MarshaledObject<T> : IDisposable where T : struct {
        GCHandle handle;

        public IntPtr Pointer {
            get {
                if (!handle.IsAllocated)
                    throw new InvalidOperationException ("Unalocated MarshaledObject");
                return handle.AddrOfPinnedObject ();
            }
        }

        public MarshaledObject (T mobj) {
            handle = GCHandle.Alloc (mobj, GCHandleType.Pinned);
        }

        void freeHandle () {
            if (!disposed) 
                handle.Free ();
        }

        #region IDisposable Support
        private bool disposed;

        ~MarshaledObject() {
            freeHandle ();
        }

        public void Dispose () {
            freeHandle ();
            GC.SuppressFinalize(this);
        }
        #endregion
    }

	public class SubPass : IDisposable {
        NativeList<VkAttachmentReference> colorRefs;
        NativeList<VkAttachmentReference> inputRefs;
        MarshaledObject<VkAttachmentReference> depthRef;
        NativeList<VkAttachmentReference> resolveRefs;
        NativeList<uint> preservedRefs;

        public SubPass () {
        }
		public SubPass (params VkImageLayout[] layouts) {
			for (uint i = 0; i < layouts.Length; i++) 
				AddColorReference (i, layouts[i]);
		}


		public void AddColorReference (uint attachment, VkImageLayout layout = VkImageLayout.DepthStencilAttachmentOptimal) {
            AddColorReference (new VkAttachmentReference { attachment = attachment, layout = layout });
        }
        public void AddColorReference (params VkAttachmentReference[] refs) {
            if (colorRefs == null)
                colorRefs = new NativeList<VkAttachmentReference> ((uint)refs.Length);
            for (int i = 0; i < refs.Length; i++)
                colorRefs.Add (refs[i]);
        }
        public void AddInputReference (params VkAttachmentReference[] refs) {
            if (inputRefs == null)
                inputRefs = new NativeList<VkAttachmentReference> ((uint)refs.Length);
            for (int i = 0; i < refs.Length; i++)
                inputRefs.Add (refs[i]);
        }
        public void AddPreservedReference (params uint[] refs) {
            if (preservedRefs == null)
                preservedRefs = new NativeList<uint> ((uint)refs.Length);
            for (int i = 0; i < refs.Length; i++)
                preservedRefs.Add (refs[i]);
        }
        public void SetDepthReference (uint attachment, VkImageLayout layout = VkImageLayout.DepthStencilAttachmentOptimal) {
            DepthReference = new VkAttachmentReference { attachment = attachment, layout = layout };
        }
		public void AddResolveReference (params VkAttachmentReference[] refs) {
			if (resolveRefs == null)
				resolveRefs = new NativeList<VkAttachmentReference> ((uint)refs.Length);
			for (int i = 0; i < refs.Length; i++)
				resolveRefs.Add (refs[i]);
		}
		public void AddResolveReference (uint attachment, VkImageLayout layout = VkImageLayout.ColorAttachmentOptimal) {
			AddResolveReference (new VkAttachmentReference { attachment = attachment, layout = layout });
		}
		public VkAttachmentReference DepthReference {
            set {
                if (depthRef != null)
                    depthRef.Dispose ();
                depthRef = new MarshaledObject<VkAttachmentReference> (value);
            }
        }

        public VkSubpassDescription SubpassDescription {
            get {
                VkSubpassDescription subpassDescription = new VkSubpassDescription ();
                subpassDescription.pipelineBindPoint = VkPipelineBindPoint.Graphics;
                if (colorRefs?.Count > 0) {
                    subpassDescription.colorAttachmentCount = colorRefs.Count;
                    subpassDescription.pColorAttachments = colorRefs.Data; ; 
                }
                if (inputRefs?.Count > 0) {
                    subpassDescription.inputAttachmentCount = inputRefs.Count;
                    subpassDescription.pInputAttachments = inputRefs.Data; ;
                }
                if (preservedRefs?.Count > 0) {
                    subpassDescription.preserveAttachmentCount = preservedRefs.Count;
                    subpassDescription.pPreserveAttachments = preservedRefs.Data; ;
                }
				if (resolveRefs?.Count > 0)
					subpassDescription.pResolveAttachments = resolveRefs.Data;

				if (depthRef != null)
                    subpassDescription.pDepthStencilAttachment = depthRef.Pointer;

                return subpassDescription;
            }        
        }

		/*public void GetAttachmentUsage (uint frameBufferAttachmentIndex, ref VkImageUsageFlags usage, ref VkImageAspectFlags aspect) {
			for (int i = 0; i < colorRefs.Count; i++) {
				if (colorRefs[i].attachment == frameBufferAttachmentIndex)
					Utils.QueryLayoutRequirements (colorRefs[i].layout, ref usage, ref aspect);
			}
			for (int i = 0; i < inputRefs.Count; i++) {
				if (colorRefs[i].attachment == frameBufferAttachmentIndex)
					Utils.QueryLayoutRequirements (colorRefs[i].layout, ref usage, ref aspect);
			}
		}*/

		#region IDisposable Support
		protected virtual void Dispose (bool disposing) {
            if (disposing) {
                colorRefs?.Dispose ();
                inputRefs?.Dispose ();
                preservedRefs?.Dispose ();
                depthRef?.Dispose ();
                resolveRefs?.Dispose ();
            }
        }
        public void Dispose () {
            Dispose (true);
            GC.SuppressFinalize (this);
        }
        #endregion
    }
}
