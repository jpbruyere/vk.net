﻿// Copyright (c) 2017 Eric Mellino
// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Vulkan {
	public static class BindingsHelpers
    {
        public static unsafe StringHandle StringToHGlobalUtf8(string s)
        {
            Debug.Assert(s != null);
            int byteCount = Encoding.UTF8.GetByteCount(s);
            IntPtr retPtr = Marshal.AllocHGlobal(byteCount);
            fixed (char* stringPtr = s)
            {
                Encoding.UTF8.GetBytes(stringPtr, s.Length, (byte*)retPtr.ToPointer(), byteCount);
            }

            return new StringHandle() { Handle = retPtr };
        }

        public static void FreeHGlobal(StringHandle ptr)
        {
            Marshal.FreeHGlobal(ptr.Handle);
        }
    }

    public struct StringHandle
    {
        public IntPtr Handle;
    }
}
