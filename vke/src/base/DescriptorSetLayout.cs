// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Collections.Generic;
using VK;
using static VK.Vk;

namespace CVKL {
    public sealed class DescriptorSetLayout : Activable {
        internal VkDescriptorSetLayout handle;
        
		public VkDescriptorSetLayoutCreateFlags Flags { get; private set; } = 0;
        public List<VkDescriptorSetLayoutBinding> Bindings { get; private set; } = new List<VkDescriptorSetLayoutBinding> ();
        
		protected override VkDebugMarkerObjectNameInfoEXT DebugMarkerInfo 
			=> new VkDebugMarkerObjectNameInfoEXT(VkDebugReportObjectTypeEXT.DescriptorSetLayoutEXT, handle.Handle);

		#region CTORS
		public DescriptorSetLayout (Device device, VkDescriptorSetLayoutCreateFlags flags) : base (device) {            
			Flags = flags;
        }
		public DescriptorSetLayout (Device device, params VkDescriptorSetLayoutBinding[] bindings)
        : this (device, 0, bindings) {
        }
        public DescriptorSetLayout (Device device, VkDescriptorSetLayoutCreateFlags flags, params VkDescriptorSetLayoutBinding[] bindings)
        : this (device, flags) {
            foreach (VkDescriptorSetLayoutBinding b in bindings) 
                Bindings.Add (b);            
        }
		#endregion

		public override void Activate () {
			if (state != ActivableState.Activated) {
				VkDescriptorSetLayoutCreateInfo info = new VkDescriptorSetLayoutCreateInfo (Flags, (uint)Bindings.Count, Bindings.Pin());
	            Utils.CheckResult (vkCreateDescriptorSetLayout (Dev.VkDev, ref info, IntPtr.Zero, out handle));
				Bindings.Unpin ();
			}
			base.Activate ();
		}

		public override string ToString () {
			return string.Format ($"{base.ToString ()}[0x{handle.Handle.ToString("x")}]");
		}

		#region IDisposable Support
		protected override void Dispose (bool disposing) {
			if (!disposing)
				System.Diagnostics.Debug.WriteLine ("VKE DescriptorSetLayout disposed by finalizer");
			if (state == ActivableState.Activated)
				vkDestroyDescriptorSetLayout (Dev.VkDev, handle, IntPtr.Zero);
			base.Dispose (disposing);
		}
		#endregion
	}
}
