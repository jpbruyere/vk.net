// Copyright (c) 2022  Bruyère Jean-Philippe jp_bruyere@hotmail.com
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)

using System;

namespace Vulkan
{
	public struct Utf8StringPointer : IDisposable
	{
		IntPtr handle;
		Utf8StringPointer (IntPtr ptr) {
			handle = ptr;
		}
		Utf8StringPointer (string str) {
			handle = str.PinPointer();
		}
		public static implicit operator string (Utf8StringPointer pt)
			=> pt.ToString();
		public static implicit operator IntPtr (Utf8StringPointer pt)
			=> pt.handle;
		public static implicit operator Utf8StringPointer (IntPtr ptr)
			=> new Utf8StringPointer (ptr);
		public static implicit operator Utf8StringPointer (string s)
			=> new Utf8StringPointer (s);
		public override string ToString()
			=> handle == IntPtr.Zero ? "" : System.Runtime.InteropServices.Marshal.PtrToStringUTF8 (handle);
		public void Dispose()
		{
			if (handle != IntPtr.Zero) {
				handle.Unpin();
				handle = IntPtr.Zero;
			}
		}
	}
}