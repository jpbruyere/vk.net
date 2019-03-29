//
// DebugReport.cs
//
// Author:
//       Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// Copyright (c) 2019 jp
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Runtime.InteropServices;
using Vulkan;
using static Vulkan.VulkanNative;

namespace VKE {
    public static class DebugReport {
        public delegate VkResult PFN_vkCreateDebugReportCallbackEXT (VkInstance instance, ref VkDebugReportCallbackCreateInfoEXT pCreateInfo, IntPtr pAllocator, out VkDebugReportCallbackEXT pCallback);
        public delegate void PFN_vkDestroyDebugReportCallbackEXT (VkInstance instance, VkDebugReportCallbackEXT callback, IntPtr pAllocator);

        public static PFN_vkCreateDebugReportCallbackEXT CreateDebugReportCallback;
        public static PFN_vkDestroyDebugReportCallbackEXT DestroyDebugReportCallback;


        static VkDebugReportCallbackEXT dbgCallbackHandle;
        static PFN_vkDebugReportCallbackEXT debugCallbackDelegate = new PFN_vkDebugReportCallbackEXT (debugCallback);
        static VkBool32 debugCallback (VkDebugReportFlagsEXT flags, VkDebugReportObjectTypeEXT objectType, ulong obj,
            UIntPtr location, int messageCode, IntPtr pLayerPrefix, IntPtr pMessage, IntPtr pUserData) {
            string prefix = "";
            switch (flags) {
                case VkDebugReportFlagsEXT.None:
                    prefix = "?";
                    break;
                case VkDebugReportFlagsEXT.InformationEXT:
                    prefix = "INFO";
                    break;
                case VkDebugReportFlagsEXT.WarningEXT:
                    prefix = "WARN";
                    break;
                case VkDebugReportFlagsEXT.PerformanceWarningEXT:
                    prefix = "PERF";
                    break;
                case VkDebugReportFlagsEXT.ErrorEXT:
                    prefix = "EROR";
                    break;
                case VkDebugReportFlagsEXT.DebugEXT:
                    prefix = "DBUG";
                    break;
            }
            Console.WriteLine ("{0} {1}: {2}",prefix, messageCode, Marshal.PtrToStringAnsi(pMessage));
            return VkBool32.False;
        }

        public static void InitDebug (Instance inst, VkDebugReportFlagsEXT flags = VkDebugReportFlagsEXT.ErrorEXT | VkDebugReportFlagsEXT.WarningEXT) {
            inst.GetDelegate ("vkCreateDebugReportCallbackEXT", out CreateDebugReportCallback);
            inst.GetDelegate ("vkDestroyDebugReportCallbackEXT", out DestroyDebugReportCallback);

            VkDebugReportCallbackCreateInfoEXT dbgInfo = new VkDebugReportCallbackCreateInfoEXT {
                sType = VkStructureType.DebugReportCallbackCreateInfoEXT,
                flags = flags,
                pfnCallback = Marshal.GetFunctionPointerForDelegate (debugCallbackDelegate)
            };

            Utils.CheckResult (CreateDebugReportCallback (inst.Handle, ref dbgInfo, IntPtr.Zero, out dbgCallbackHandle));
        }

        public static void Clean (Instance inst) {
            DestroyDebugReportCallback (inst.Handle, dbgCallbackHandle, IntPtr.Zero);
        }
    }
}
