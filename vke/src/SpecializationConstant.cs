//
// SpecializationConstant.cs
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

namespace CVKL {
	/// <summary>
	/// Hold shader specialization constant value and type
	/// </summary>
	public class SpecializationConstant<T> : SpecializationConstant {
		T val;
		IntPtr ptr = IntPtr.Zero;

        public T Value {
			get { return val; }
			set {
				val = value;
				if (ptr != IntPtr.Zero)
					WriteTo (ptr);
			}
        }
        public SpecializationConstant(uint id, T value) : base(id) {            
            val = value;
        }
		public override uint Size => (uint)Marshal.SizeOf<T> ();
		public unsafe override void WriteTo (IntPtr ptr) {
			this.ptr = ptr;

			if (typeof (T) == typeof (float)) {
				float v = Convert.ToSingle (Value);
				System.Buffer.MemoryCopy (&v, ptr.ToPointer (), 4, 4);
			} else if (typeof (T) == typeof (int) || typeof (T) == typeof (uint)) {
				Marshal.WriteInt32 (ptr, Convert.ToInt32 (val));
			} else if (typeof (T) == typeof (long) || typeof (T) == typeof (ulong)) {
				Marshal.WriteInt64 (ptr, Convert.ToInt64 (val));
			} else if (typeof (T) == typeof (byte)) {
				Marshal.WriteByte (ptr, Convert.ToByte (val));
			}
		}
	}
	public abstract class SpecializationConstant {
		public uint id;
		public SpecializationConstant (uint id) {
			this.id = id;
		}
		public abstract uint Size { get; }
		public abstract void WriteTo (IntPtr ptr);
	}

	/// <summary>
	/// Specialization constant infos, must be disposed after pipeline creation
	/// </summary>
	public class SpecializationInfo : IDisposable {
		IntPtr pData;
		VkSpecializationMapEntry[] entries;

		VkSpecializationInfo infos;

		public IntPtr InfosPtr { get; private set; }

		public SpecializationInfo (params SpecializationConstant[] constants) {
			uint offset = 0;
			entries = new VkSpecializationMapEntry[constants.Length];
			for (int i = 0; i < constants.Length; i++) {
				entries[i] = new VkSpecializationMapEntry { constantID = constants[i].id, offset = offset, size = (UIntPtr)constants[i].Size };
				offset += constants[i].Size;
			}
			int totSize = (int)offset;
			offset = 0;
			pData = Marshal.AllocHGlobal (totSize);
			IntPtr curPtr = pData;
			foreach (SpecializationConstant sc in constants) {
				sc.WriteTo (curPtr);
				curPtr += (int)sc.Size;
			}

			infos = new VkSpecializationInfo {
				mapEntryCount = (uint)constants.Length,
				pMapEntries = entries.Pin (),
				pData = pData,
				dataSize = (UIntPtr)totSize
			};
			InfosPtr = infos.Pin ();
		}

		public void Dispose () {
			infos.Unpin ();
			Marshal.FreeHGlobal (pData);
			entries.Unpin ();
		}
	}
}
