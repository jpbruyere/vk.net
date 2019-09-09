// Copyright (c) 2019 Andrew Armstrong/FacticiusVir
// Copyright (c) 2019 Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;

namespace Glfw
{
    /// <summary>
    /// Opaque monitor handle.
    /// </summary>
    public struct MonitorHandle
    {
		readonly IntPtr handle;

		internal MonitorHandle(IntPtr handle)
        {
            this.handle = handle;
        }
		/// <summary>
		/// Gets the underlying native pointer to the monitor object.
		/// </summary>
		public IntPtr RawHandle => handle;
        /// <summary>
        /// A read-only field that represents a MonitorHandle that has been
        /// inititalised to zero.
        /// </summary>
        public static readonly MonitorHandle Zero = new MonitorHandle (IntPtr.Zero);
    }
}
