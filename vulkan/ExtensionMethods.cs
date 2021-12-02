// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

using static Vulkan.Vk;

namespace Vulkan {
	public static class ExtensionMethods {
		#region pinning
		/// <summary>
		/// list of pinned GCHandles used to pass value from managed to unmanaged code.
		/// </summary>
		public static Dictionary<object, GCHandle> handles = new Dictionary<object, GCHandle>();

		/// <summary>
		/// Unpin the specified object and free the GCHandle associated.
		/// </summary>
		public static void Unpin (this object obj) {
			if (!handles.ContainsKey (obj)) {
				Debug.WriteLine ("Trying to unpin unpinned object: {0}.", obj);
				return;
			}
			handles[obj].Free ();
			handles.Remove (obj);
		}

		/// <summary>
		/// Pin the specified object and return a pointer. MUST be Unpined as soon as possible.
		/// </summary>
		public static IntPtr Pin (this object obj) {
			if (handles.ContainsKey (obj)) {
				Debug.WriteLine ("Trying to pin already pinned object: {0}", obj);
				return handles[obj].AddrOfPinnedObject ();
			}

            GCHandle hnd = GCHandle.Alloc (obj, GCHandleType.Pinned);
            handles.Add (obj, hnd);
            return hnd.AddrOfPinnedObject ();
        }
        public static IntPtr Pin<T> (this List<T> obj) {
            if (handles.ContainsKey (obj))
                Debug.WriteLine ("Pinning already pinned object: {0}", obj);

            GCHandle hnd = GCHandle.Alloc (obj.ToArray(), GCHandleType.Pinned);
            handles.Add (obj, hnd);
            return hnd.AddrOfPinnedObject ();
        }
		public static IntPtr Pin<T> (this T[] obj) {
			if (handles.ContainsKey (obj))
				Debug.WriteLine ("Pinning already pinned object: {0}", obj);

			GCHandle hnd = GCHandle.Alloc (obj, GCHandleType.Pinned);
			handles.Add (obj, hnd);
			return hnd.AddrOfPinnedObject ();
		}
		public static IntPtr Pin (this string obj) {
			if (handles.ContainsKey (obj)) {
				Debug.WriteLine ("Trying to pin already pinned object: {0}", obj);
				return handles[obj].AddrOfPinnedObject ();
			}
            byte[] n = System.Text.Encoding.UTF8.GetBytes(obj +'\0');
			GCHandle hnd = GCHandle.Alloc (n, GCHandleType.Pinned);
            handles.Add (obj, hnd);
            return hnd.AddrOfPinnedObject ();
        }

		//pin with pinning context
		public static IntPtr Pin (this object obj, PinnedObjects ctx) {
			GCHandle hnd = GCHandle.Alloc (obj, GCHandleType.Pinned);
			ctx.Handles.Add (hnd);
			return hnd.AddrOfPinnedObject ();
		}
		public static IntPtr Pin<T> (this IList<T> obj, PinnedObjects ctx) {
			GCHandle hnd = GCHandle.Alloc (obj.ToArray (), GCHandleType.Pinned);
			ctx.Handles.Add (hnd);
			return hnd.AddrOfPinnedObject ();
		}
		public static IntPtr Pin<T> (this T[] obj, PinnedObjects ctx) {
			GCHandle hnd = GCHandle.Alloc (obj, GCHandleType.Pinned);
			ctx.Handles.Add (hnd);
			return hnd.AddrOfPinnedObject ();
		}
		public static IntPtr Pin (this string obj, PinnedObjects ctx) {
			byte[] n = System.Text.Encoding.UTF8.GetBytes (obj + '\0');
			GCHandle hnd = GCHandle.Alloc (n, GCHandleType.Pinned);
			ctx.Handles.Add (hnd);
			return hnd.AddrOfPinnedObject ();
		}

		#endregion

	}
}
