using System;
using System.Runtime.InteropServices;

namespace Glfw
{
    /// <summary>
    /// Wraps a pointer to a native, null-terminated ANSI string.
    /// </summary>
    public struct NativeString
    {
        internal NativeString(System.IntPtr pointer)
        {
            this.pointer = pointer;
        }

        private System.IntPtr pointer;

        /// <summary>
        /// Gets the marshalled string value for this native string.
        /// </summary>
        public string Value
        {
            get
            {
                if (this.IsNull)
                {
                    throw new NullReferenceException();
                }

                return Marshal.PtrToStringAnsi(this.pointer);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the pointer wrapped by this
        /// instance is null.
        /// </summary>
        public bool IsNull => this.pointer == System.IntPtr.Zero;

        /// <summary>
        /// The underlying pointer wrapped by this instance.
        /// </summary>
        public IntPtr RawPointer => this.pointer;

        public override string ToString()
        {
            return this.Value;
        }
    }
}
