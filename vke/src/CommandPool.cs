//
// CommandPool.cs
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
using Vulkan;
using static Vulkan.VulkanNative;

namespace VKE {
    public sealed class CommandPool : Activable {
        public readonly uint QFamIndex;
        VkCommandPool handle;

		#region CTORS
		public CommandPool (Device device, uint qFamIdx) : base(device)
        {            
            QFamIndex = qFamIdx;

			Activate ();
        }
		#endregion

#if DEBUG && DEBUG_MARKER
		protected override VkDebugMarkerObjectNameInfoEXT DebugMarkerInfo
			=> new VkDebugMarkerObjectNameInfoEXT(VkDebugReportObjectTypeEXT.CommandPoolEXT, handle.Handle);
#endif

		public override void Activate () {
			if (state != ActivableState.Activated) {            
        	    VkCommandPoolCreateInfo infos = VkCommandPoolCreateInfo.New ();
    	        infos.queueFamilyIndex = QFamIndex;
	            Utils.CheckResult (vkCreateCommandPool (dev.VkDev, ref infos, IntPtr.Zero, out handle));
			}
			base.Activate ();
		}

        public CommandBuffer AllocateCommandBuffer (VkCommandBufferLevel level = VkCommandBufferLevel.Primary) {
            VkCommandBuffer buff;
            VkCommandBufferAllocateInfo infos = VkCommandBufferAllocateInfo.New ();
            infos.commandPool = handle;
            infos.level = level;
            infos.commandBufferCount = 1;

            Utils.CheckResult (vkAllocateCommandBuffers (dev.VkDev, ref infos, out buff));

            return new CommandBuffer (dev.VkDev, this, buff);
        }
        public void FreeCommandBuffers (params CommandBuffer[] cmds) {
            if (cmds.Length == 1) {
                VkCommandBuffer hnd = cmds[0].Handle;
                vkFreeCommandBuffers (dev.VkDev, handle, 1, ref hnd);
                return;
            }
            using (NativeList<VkCommandBuffer> nlCmds = new NativeList<VkCommandBuffer> ((uint)cmds.Length, (uint)cmds.Length)) {
                for (int i = 0; i < cmds.Length; i++) 
                    nlCmds[i] = cmds[i].Handle;
                vkFreeCommandBuffers (dev.VkDev, handle, (uint)cmds.Length, nlCmds.Data);
            }
        }

		public override string ToString () {
			return string.Format ($"{base.ToString ()}[0x{handle.Handle.ToString("x")}]");
		}

		#region IDisposable Support
		protected override void Dispose (bool disposing) {
			if (!disposing)
				System.Diagnostics.Debug.WriteLine ("VKE CommandPool disposed by finalizer");
			if (state == ActivableState.Activated)
				vkDestroyCommandPool (dev.VkDev, handle, IntPtr.Zero);
			base.Dispose (disposing);
		}
		#endregion

	}
}
