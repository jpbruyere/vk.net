// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

using VK;
using static VK.Vk;

namespace CVKL {
	public static class ExtensionMethods {
		/// <summary>
		/// Extensions method to check byte array equality.
		/// </summary>
		public static bool AreEquals (this byte[] b, byte[] other) {
			if (b.Length != other.Length)
				return false;
			for (int i = 0; i < b.Length; i++) {
				if (b[i] != other[i])
					return false;
			}
			return true;
		}

		#region pinning
		/// <summary>
		/// list of pinned GCHandles used to pass value from managed to unmanaged code.
		/// </summary>
		public static Dictionary<object, GCHandle> handles = new Dictionary<object, GCHandle>();

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
		#endregion

		#region DebugMarkers
		public static void SetDebugMarkerName (this VkCommandBuffer obj, Device dev, string name) {
			if (!dev.debugMarkersEnabled)
				return;
			VkDebugMarkerObjectNameInfoEXT dmo = new VkDebugMarkerObjectNameInfoEXT (VkDebugReportObjectTypeEXT.CommandBufferEXT,
				(ulong)obj.Handle.ToInt64 ()) { pObjectName = name.Pin () };
			Utils.CheckResult (vkDebugMarkerSetObjectNameEXT (dev.VkDev, ref dmo));
			name.Unpin ();
		}
		public static void SetDebugMarkerName (this VkImageView obj, Device dev, string name) {
			if (!dev.debugMarkersEnabled)
				return;
			VkDebugMarkerObjectNameInfoEXT dmo = new VkDebugMarkerObjectNameInfoEXT (VkDebugReportObjectTypeEXT.ImageViewEXT,
				obj.Handle) { pObjectName = name.Pin () };
			Utils.CheckResult (vkDebugMarkerSetObjectNameEXT (dev.VkDev, ref dmo));
			name.Unpin ();
		}
		public static void SetDebugMarkerName (this VkSampler obj, Device dev, string name) {
			if (!dev.debugMarkersEnabled)
				return;
			VkDebugMarkerObjectNameInfoEXT dmo = new VkDebugMarkerObjectNameInfoEXT (VkDebugReportObjectTypeEXT.SamplerEXT,
				obj.Handle) { pObjectName = name.Pin () };
			Utils.CheckResult (vkDebugMarkerSetObjectNameEXT (dev.VkDev, ref dmo));
			name.Unpin ();
		}
		public static void SetDebugMarkerName (this VkPipeline obj, Device dev, string name) {
			if (!dev.debugMarkersEnabled)
				return;
			VkDebugMarkerObjectNameInfoEXT dmo = new VkDebugMarkerObjectNameInfoEXT (VkDebugReportObjectTypeEXT.PipelineEXT,
				obj.Handle) { pObjectName = name.Pin () };
			Utils.CheckResult (vkDebugMarkerSetObjectNameEXT (dev.VkDev, ref dmo));
			name.Unpin ();
		}
		public static void SetDebugMarkerName (this VkDescriptorSet obj, Device dev, string name) {
			if (!dev.debugMarkersEnabled)
				return;
			VkDebugMarkerObjectNameInfoEXT dmo = new VkDebugMarkerObjectNameInfoEXT (VkDebugReportObjectTypeEXT.DescriptorSetEXT,
				obj.Handle) { pObjectName = name.Pin () };
			Utils.CheckResult (vkDebugMarkerSetObjectNameEXT (dev.VkDev, ref dmo));
			name.Unpin ();
		}
		public static void SetDebugMarkerName (this VkSemaphore obj, Device dev, string name) {
			if (!dev.debugMarkersEnabled)
				return;
			VkDebugMarkerObjectNameInfoEXT dmo = new VkDebugMarkerObjectNameInfoEXT (VkDebugReportObjectTypeEXT.SemaphoreEXT,
				obj.Handle) { pObjectName = name.Pin () };
			Utils.CheckResult (vkDebugMarkerSetObjectNameEXT (dev.VkDev, ref dmo));
			name.Unpin ();
		}
		public static void SetDebugMarkerName (this VkFence obj, Device dev, string name) {
			if (!dev.debugMarkersEnabled)
				return;
			VkDebugMarkerObjectNameInfoEXT dmo = new VkDebugMarkerObjectNameInfoEXT (VkDebugReportObjectTypeEXT.FenceEXT,
				obj.Handle) { pObjectName = name.Pin () };
			Utils.CheckResult (vkDebugMarkerSetObjectNameEXT (dev.VkDev, ref dmo));
			name.Unpin ();
		}
		#endregion
	}
}
