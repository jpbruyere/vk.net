// Copyright (c) 2017 Eric Mellino
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Runtime.InteropServices;

namespace Vulkan
{
    internal static class Libdl
    {
        [DllImport("dlib")]
        public static extern IntPtr dlopen(string fileName, int flags);

        [DllImport("dlib")]
        public static extern IntPtr dlsym(IntPtr handle, string name);

        [DllImport("dlib")]
        public static extern int dlclose(IntPtr handle);

        [DllImport("dlib")]
        public static extern string dlerror();

        public const int RTLD_NOW = 0x002;
    }
}
