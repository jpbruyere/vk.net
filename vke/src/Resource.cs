using System;
using System.IO;
using System.Runtime.InteropServices;
using VK;

using static VK.Vk;

namespace CVKL {
    public abstract class Resource : Activable {
		protected VkMemoryRequirements memReqs;
		internal MemoryPool memoryPool;
		public ulong poolOffset;
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
				memory = memoryPool.vkMemory,
				offset = poolOffset,
				size = AllocatedDeviceMemorySize
			};

		public void Map (ulong offset = 0) {
			if (!memoryPool.IsMapped)
				memoryPool.Map ();
			mappedData = new IntPtr(memoryPool.MappedData.ToInt64() + (long)(poolOffset + offset));
        }
        public void Unmap () {
            
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
				memoryPool.Remove (this);
			}
			base.Dispose (disposing);
        }
        #endregion
    }
}