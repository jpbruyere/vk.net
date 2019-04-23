using System;
using System.Runtime.InteropServices;

namespace CVKL {
    public static class Stb {
        public const string stblib = "stb";

        [DllImport (stblib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load")]
        public static extern IntPtr Load ([MarshalAs (UnmanagedType.LPStr)] string filename, out int x, out int y, out int channels_in_file, int desired_channels);

		[DllImport (stblib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load_from_memory")]
        public static extern IntPtr Load (IntPtr bitmap, int byteCount, out int x, out int y, out int channels_in_file, int desired_channels);        

        [DllImport (stblib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_image_free")]
        public static extern void FreeImage (IntPtr img);
    }
}
