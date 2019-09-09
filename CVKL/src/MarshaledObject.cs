// Copyright (c) 2019 Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Runtime.InteropServices;

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
			disposed = true;
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
}
