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
using VK;
using static VK.Vk;

namespace CVKL {

    public class DebugReport : IDisposable {        
        VkDebugReportCallbackEXT handle;
		Instance inst;

        PFN_vkDebugReportCallbackEXT debugCallbackDelegate = new PFN_vkDebugReportCallbackEXT (debugCallback);

        static VkBool32 debugCallback (VkDebugReportFlagsEXT flags, VkDebugReportObjectTypeEXT objectType, ulong obj,
            UIntPtr location, int messageCode, IntPtr pLayerPrefix, IntPtr pMessage, IntPtr pUserData) {
            string prefix = "";
            switch (flags) {
                case 0:
                    prefix = "?";
                    break;
                case VkDebugReportFlagsEXT.InformationEXT:
					Console.ForegroundColor = ConsoleColor.Gray;
					prefix = "INFO";
                    break;
                case VkDebugReportFlagsEXT.WarningEXT:
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					prefix = "WARN";
                    break;
                case VkDebugReportFlagsEXT.PerformanceWarningEXT:
					Console.ForegroundColor = ConsoleColor.Yellow;
					prefix = "PERF";
                    break;
                case VkDebugReportFlagsEXT.ErrorEXT:
					Console.ForegroundColor = ConsoleColor.DarkRed;
					prefix = "EROR";
					break;
                case VkDebugReportFlagsEXT.DebugEXT:
					Console.ForegroundColor = ConsoleColor.Red;
					prefix = "DBUG";
                    break;
            }

            Console.WriteLine ("{0} {1}: {2}",prefix, messageCode, Marshal.PtrToStringAnsi(pMessage));
			Console.ForegroundColor = ConsoleColor.White;
            return VkBool32.False;
        }
        
        public DebugReport (Instance instance, VkDebugReportFlagsEXT flags = VkDebugReportFlagsEXT.ErrorEXT | VkDebugReportFlagsEXT.WarningEXT) {
			inst = instance;

            VkDebugReportCallbackCreateInfoEXT dbgInfo = new VkDebugReportCallbackCreateInfoEXT {
                sType = VkStructureType.DebugReportCallbackCreateInfoEXT,
                flags = flags,
                pfnCallback = Marshal.GetFunctionPointerForDelegate (debugCallbackDelegate)
            };

            Utils.CheckResult (vkCreateDebugReportCallbackEXT (inst.Handle, ref dbgInfo, IntPtr.Zero, out handle));
        }

		#region IDisposable Support
		private bool disposedValue = false;

		protected virtual void Dispose (bool disposing) {
			if (!disposedValue) {
				if (disposing) {
					// TODO: supprimer l'état managé (objets managés).
				}

				vkDestroyDebugReportCallbackEXT (inst.Handle, handle, IntPtr.Zero);

				disposedValue = true;
			}
		}

		~DebugReport () {
			Dispose (false);
		}
		public void Dispose () {
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		#endregion
	}
}
