// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
//
// Copyright (c) 2022 JP Bruyère
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Vulkan
{
    /// <summary>
    /// Helper class to construct pnext chain
    /// </summary>
	public class PNext<T> : IDisposable where T : struct
	{
		IntPtr handle;
		/// <summary>
		/// Allocate on HGlobal a new structure of type T, ensure instance is Disposed.
		/// </summary>
		public PNext () {
			init();
		}
		/// <summary>
		/// Allocate on HGlobal a new structure of type T and set the pNext field,
		/// ensure instance is Disposed.
		/// </summary>
		public PNext (IntPtr pNext) {
			init();
			Marshal.WriteIntPtr(handle + 4, pNext);
		}
		void init() {
			handle = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
			Type t = typeof(T);
			StructureTypeAttribute st = (StructureTypeAttribute)t.GetCustomAttributes(typeof(StructureTypeAttribute), false).FirstOrDefault();
			Marshal.WriteInt32(handle, st.StructureType);
		}
		/// <summary>
		/// Get value of T
		/// </summary>
		public T Val => Marshal.PtrToStructure<T> (handle);
		public static implicit operator IntPtr(PNext<T> pn) => pn.handle;
		public void Dispose()
		{
			Marshal.FreeHGlobal(handle);
		}
	}
}
