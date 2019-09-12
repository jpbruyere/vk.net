// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Runtime.InteropServices;

namespace VK {
	internal static class LoadingUtils {
		public static IntPtr GetDelegate (VkInstance inst, string name) {
			byte[] n = System.Text.Encoding.UTF8.GetBytes (name + '\0');
			GCHandle hnd = GCHandle.Alloc (n, GCHandleType.Pinned);
			IntPtr del = Vk.vkGetInstanceProcAddr (inst, hnd.AddrOfPinnedObject ());
			if (del == IntPtr.Zero)
				System.Diagnostics.Debug.WriteLine ("instance function pointer not found for " + name);
			hnd.Free ();
			return del;
		}
		/// <summary>
		/// try to get device function handle if available
		/// </summary>
		public static void GetDelegate (VkDevice dev, string name, ref IntPtr ptr) {
			byte[] n = System.Text.Encoding.UTF8.GetBytes (name + '\0');
			GCHandle hnd = GCHandle.Alloc (n, GCHandleType.Pinned);
			IntPtr del = Vk.vkGetDeviceProcAddr (dev, hnd.AddrOfPinnedObject ());
			if (del == IntPtr.Zero)
				System.Diagnostics.Debug.WriteLine ("device function pointer not found for " + name);
			else
				ptr = del;
			hnd.Free ();			
		} 
	} 
}
