using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using VK;

using static VK.Vk;

namespace CVKL {
	[DebuggerDisplay ("{previous.name} <- {name} -> {next.name}")]
	public abstract class Resource : Activable {
		protected VkMemoryRequirements memReqs;
#if MEMORY_POOLS
		internal MemoryPool memoryPool;
		public ulong poolOffset;
#else
		protected VkDeviceMemory vkMemory;
#endif

		/// <summary> double linked list in memory pool </summary>
		internal Resource previous;
		public Resource next;

		public ulong AllocatedDeviceMemorySize => memReqs.size;
		public uint TypeBits => memReqs.memoryTypeBits;
		public ulong MemoryAlignment => memReqs.alignment;

		protected IntPtr mappedData;
		public IntPtr MappedData => mappedData;

		public readonly VkMemoryPropertyFlags MemoryFlags;

        protected Resource (Device device, VkMemoryPropertyFlags memoryFlags) : base (device) {            
            MemoryFlags = memoryFlags;
        }

        internal abstract void updateMemoryRequirements ();

		internal abstract void bindMemory ();

		internal VkMappedMemoryRange MapRange => new VkMappedMemoryRange {
				sType = VkStructureType.MappedMemoryRange,
#if MEMORY_POOLS
				memory = memoryPool.vkMemory,
				offset = poolOffset,
#else
				memory = vkMemory,
				offset = 0,
#endif
				size = AllocatedDeviceMemorySize
			};
#if !MEMORY_POOLS
		protected void allocateMemory () {
			VkMemoryAllocateInfo memInfo = VkMemoryAllocateInfo.New ();
			memInfo.allocationSize = memReqs.size;
			memInfo.memoryTypeIndex = Dev.GetMemoryTypeIndex (memReqs.memoryTypeBits, MemoryFlags);
			Utils.CheckResult (vkAllocateMemory (Dev.VkDev, ref memInfo, IntPtr.Zero, out vkMemory));
		}
#endif


		public void Map (ulong offset = 0) {
#if MEMORY_POOLS
			if (!memoryPool.IsMapped)
				memoryPool.Map ();
			mappedData = new IntPtr(memoryPool.MappedData.ToInt64() + (long)(poolOffset + offset));
#else
			Utils.CheckResult (vkMapMemory (Dev.VkDev, vkMemory, offset, AllocatedDeviceMemorySize, 0, ref mappedData));
#endif
		}
		public void Unmap () {
#if MEMORY_POOLS
#else
			vkUnmapMemory (Dev.VkDev, vkMemory);
			mappedData = IntPtr.Zero;
#endif
		}
		public void Update (object data, ulong size, ulong offset = 0) {
            GCHandle ptr = GCHandle.Alloc (data, GCHandleType.Pinned);
            unsafe {
                System.Buffer.MemoryCopy (ptr.AddrOfPinnedObject ().ToPointer (), (mappedData + (int)offset).ToPointer (), size, size);
            }
            ptr.Free ();
        }
        public void Flush () {
			VkMappedMemoryRange range = MapRange;
            vkFlushMappedMemoryRanges (Dev.VkDev, 1, ref range);
        }

#region IDisposable Support        
        protected override void Dispose (bool disposing) {
			if (!disposing)
				System.Diagnostics.Debug.WriteLine ("VKE Activable object disposed by finalizer");
			if (state == ActivableState.Activated) {
				if (mappedData != IntPtr.Zero)
					Unmap ();
#if MEMORY_POOLS
				memoryPool.Remove (this);
#else
				vkFreeMemory (Dev.VkDev, vkMemory, IntPtr.Zero);
#endif

			}
			base.Dispose (disposing);
        }
#endregion
	}
}