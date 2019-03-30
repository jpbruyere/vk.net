using System;
using System.Runtime.InteropServices;

namespace Glfw
{
    /// <summary>
    /// Wraps a pointer to a VideoMode struct.
    /// </summary>
    public struct VideoModePointer
    {
        internal VideoModePointer(System.IntPtr pointer)
        {
            this.pointer = pointer;
        }

        private System.IntPtr pointer;
        
        /// <summary>
        /// Gets the VideoMode value at the referenced memory location.
        /// </summary>
        public VideoMode Value => this.IsNull ? throw new NullReferenceException() : Marshal.PtrToStructure<VideoMode>(this.pointer);

        /// <summary>
        /// Gets a value indicating whether the pointer wrapped by this
        /// instance is null.
        /// </summary>
        public bool IsNull => this.pointer == System.IntPtr.Zero;

        /// <summary>
        /// The underlying pointer wrapped by this instance.
        /// </summary>
        public IntPtr RawPointer => this.pointer;
    }
}
