using System;
using System.Runtime.InteropServices;

// Copyright (c) 2019 Andrew Armstrong/FacticiusVir
// Copyright (c) 2019 Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
namespace Glfw
{
    /// <summary>
    /// Wraps a pointer to a native, null-terminated ANSI string.
    /// </summary>
    public struct NativeString
    {
        internal NativeString(IntPtr pointer)
        {
            this.pointer = pointer;
        }

        readonly IntPtr pointer;

		/// <summary>
		/// Gets the marshalled string value for this native string.
		/// </summary>
		public string Value => IsNull ? throw new NullReferenceException () : Marshal.PtrToStringAnsi (this.pointer);
        /// <summary>
        /// Gets a value indicating whether the pointer wrapped by this
        /// instance is null.
        /// </summary>
        public bool IsNull => pointer == IntPtr.Zero;

        /// <summary>
        /// The underlying pointer wrapped by this instance.
        /// </summary>
        public IntPtr RawPointer => pointer;

        public override string ToString()
        {
            return Value;
        }
    }
}
