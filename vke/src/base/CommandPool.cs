// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)

using System;
using System.Runtime.InteropServices;
using VK;
using static VK.Vk;

namespace CVKL {
	/// <summary>
	/// Command pools are opaque objects that command buffer memory is allocated from, and which allow the implementation
	/// to amortize the cost of resource creation across multiple command buffers.
	/// </summary>
	public sealed class CommandPool : Activable {
        public readonly uint QFamIndex;
        VkCommandPool handle;

		#region CTORS
		/// <summary>
		/// Create and activate a new Command Pool.
		/// </summary>
		/// <param name="device">Vulkan Device.</param>
		/// <param name="qFamIdx">Queue family index.</param>
		public CommandPool (Device device, uint qFamIdx) : base(device)
        {            
            QFamIndex = qFamIdx;

			Activate ();
        }
		/// <summary>
		/// Initializes a new instance of the [[CommandPool]]"/> class.
		/// </summary>
		/// <param name="queue">Device Queue of the queue family to create the pool for.</param>
		public CommandPool (Queue queue) : this(queue.dev, queue.qFamIndex) {}
		#endregion

		protected override VkDebugMarkerObjectNameInfoEXT DebugMarkerInfo
			=> new VkDebugMarkerObjectNameInfoEXT(VkDebugReportObjectTypeEXT.CommandPoolEXT, handle.Handle);

		public override void Activate () {
			if (state != ActivableState.Activated) {            
        	    VkCommandPoolCreateInfo infos = VkCommandPoolCreateInfo.New();
    	        infos.queueFamilyIndex = QFamIndex;
	            Utils.CheckResult (vkCreateCommandPool (Dev.VkDev, ref infos, IntPtr.Zero, out handle));
			}
			base.Activate ();
		}
		/// <summary>
		/// Allocates single command buffer.
		/// When command buffers are first allocated, they are in the initial state.
		/// </summary>
		/// <returns>The command buffer in the Init state.</returns>
		/// <param name="level">Command Buffer Level.</param>
		public CommandBuffer AllocateCommandBuffer (VkCommandBufferLevel level = VkCommandBufferLevel.Primary) {
            VkCommandBuffer buff;
            VkCommandBufferAllocateInfo infos = VkCommandBufferAllocateInfo.New();
            infos.commandPool = handle;
            infos.level = level;
            infos.commandBufferCount = 1;

            Utils.CheckResult (vkAllocateCommandBuffers (Dev.VkDev, ref infos, out buff));

            return new CommandBuffer (Dev.VkDev, this, buff);
        }
		/// <summary>
		/// Allocates multiple command buffer.
		/// </summary>
		/// <returns>An array of command buffers alloocated from this pool.</returns>
		/// <param name="count">Buffer count to create.</param>
		/// <param name="level">Command Buffer Level.</param>
		public CommandBuffer[] AllocateCommandBuffer (uint count, VkCommandBufferLevel level = VkCommandBufferLevel.Primary) {
			VkCommandBufferAllocateInfo infos = VkCommandBufferAllocateInfo.New ();
			infos.commandPool = handle;
			infos.level = level;
			infos.commandBufferCount = count;
			VkCommandBuffer[] buffs = new VkCommandBuffer[count];
			Utils.CheckResult (vkAllocateCommandBuffers (Dev.VkDev, ref infos, buffs.Pin()));
			buffs.Unpin ();
			CommandBuffer[] cmds = new CommandBuffer[count];
			for (int i = 0; i < count; i++) 
				cmds[i] = new CommandBuffer (Dev.VkDev, this, buffs[i]);

			return cmds;
		}
		/// <summary>
		/// Resetting a command pool recycles all of the resources from all of the command buffers allocated from the command
		/// pool back to the command pool. All command buffers that have been allocated from the command pool are put in the initial state. 
		/// Any primary command buffer allocated from another VkCommandPool that is in the recording or executable state and has a secondary
		/// command buffer allocated from commandPool recorded into it, becomes invalid.
		/// </summary>
		/// <param name="flags">Set `ReleaseResources` flag to recycles all of the resources from the command pool back to the system.</param>
		public void Reset (VkCommandPoolResetFlags flags = 0) {
			Vk.vkResetCommandPool (Dev.VkDev, handle, flags);
		}
		/// <summary>
		/// Allocates a new command buffer and automatically start it.
		/// </summary>
		/// <returns>New command buffer in the recording state.</returns>
		/// <param name="usage">Usage.</param>
		/// <param name="level">Command buffer level, primary or secondary.</param>
		public CommandBuffer AllocateAndStart (VkCommandBufferUsageFlags usage = 0, VkCommandBufferLevel level = VkCommandBufferLevel.Primary) {
			CommandBuffer cmd = AllocateCommandBuffer (level);
			cmd.Start (usage);
			return cmd;
		}
		/// <summary>
		/// Any primary command buffer that is in the recording or executable state and has any element of the command buffer list recorded into it, becomes invalid.
		/// </summary>
		/// <param name="cmds">Command buffer list to free.</param>
		public void FreeCommandBuffers (params CommandBuffer[] cmds) {
            if (cmds.Length == 1) {
                VkCommandBuffer hnd = cmds[0].Handle;
                vkFreeCommandBuffers (Dev.VkDev, handle, 1, ref hnd);
                return;
            }
			int sizeElt = Marshal.SizeOf<IntPtr> ();
			IntPtr cmdsPtr = Marshal.AllocHGlobal (cmds.Length * sizeElt);
			int count = 0;
			for (int i = 0; i < cmds.Length; i++) {
				if (cmds[i] == null)
					continue;
				Marshal.WriteIntPtr (cmdsPtr + count * sizeElt, cmds[i].Handle.Handle);
				count++;
			}
			if (count > 0)
				vkFreeCommandBuffers (Dev.VkDev, handle, (uint)count, cmdsPtr);

			Marshal.FreeHGlobal (cmdsPtr);
        }

		public override string ToString () {
			return string.Format ($"{base.ToString ()}[0x{handle.Handle.ToString("x")}]");
		}

		#region IDisposable Support
		protected override void Dispose (bool disposing) {
			if (!disposing)
				System.Diagnostics.Debug.WriteLine ("VKE CommandPool disposed by finalizer");
			if (state == ActivableState.Activated)
				vkDestroyCommandPool (Dev.VkDev, handle, IntPtr.Zero);
			base.Dispose (disposing);
		}
		#endregion

	}
}
