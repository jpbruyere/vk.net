// Copyright (c) 2019 Andrew Armstrong/FacticiusVir
// Copyright (c) 2019 Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Runtime.InteropServices;

namespace Glfw
{
    /// <summary>
    /// Wraps a pointer to a VideoMode struct.
    /// </summary>
    public struct VideoModePointer
    {
		private readonly IntPtr pointer;
		internal VideoModePointer (IntPtr pointer)
        {
            this.pointer = pointer;
        }
        /// <summary>
        /// Gets the VideoMode value at the referenced memory location.
        /// </summary>
        public VideoMode Value => IsNull ? throw new NullReferenceException() : Marshal.PtrToStructure<VideoMode>(pointer);
        /// <summary>
        /// Gets a value indicating whether the pointer wrapped by this
        /// instance is null.
        /// </summary>
        public bool IsNull => pointer == IntPtr.Zero;
        /// <summary>
        /// The underlying pointer wrapped by this instance.
        /// </summary>
        public IntPtr RawPointer => pointer;
    }
}
