using System;
using System.IO;
using System.Runtime.InteropServices;
using VK;

using static VK.Vk;

namespace VKE {
    public abstract class Resource : Activable {
        protected VkDeviceMemory vkMemory;
        protected UInt64 deviceMemSize;
        protected IntPtr mappedData;

        public readonly VkMemoryPropertyFlags MemoryFlags;
        public IntPtr MappedData => mappedData;
		public UInt64 AllocatedDeviceMemorySize => deviceMemSize;

        protected Resource (Device device, VkMemoryPropertyFlags memoryFlags) : base (device) {            
            MemoryFlags = memoryFlags;
        }

        protected abstract VkMemoryRequirements getMemoryRequirements ();
        protected abstract void bindMemory (ulong offset);

        protected void allocateMemory () {
            VkMemoryRequirements memReqs = getMemoryRequirements ();
            VkMemoryAllocateInfo memInfo = VkMemoryAllocateInfo.New();
            memInfo.allocationSize = memReqs.size;
            memInfo.memoryTypeIndex = dev.GetMemoryTypeIndex (memReqs.memoryTypeBits, MemoryFlags);

            Utils.CheckResult (vkAllocateMemory (dev.VkDev, ref memInfo, IntPtr.Zero, out vkMemory));

            deviceMemSize = memInfo.allocationSize;
        }

        public void Map (ulong size = WholeSize, ulong offset = 0) {
            Utils.CheckResult (vkMapMemory (dev.VkDev, vkMemory, offset, size, 0, ref mappedData));
        }
        public void Unmap () {
            vkUnmapMemory (dev.VkDev, vkMemory);
            mappedData = IntPtr.Zero;
        }
        public void Update (object data, ulong size, ulong offset = 0) {
            GCHandle ptr = GCHandle.Alloc (data, GCHandleType.Pinned);
            unsafe {
                System.Buffer.MemoryCopy (ptr.AddrOfPinnedObject ().ToPointer (), (mappedData + (int)offset).ToPointer (), size, size);
                Flush ();
            }
            ptr.Free ();
        }
        public void Flush (ulong size = WholeSize, ulong offset = 0) {
            VkMappedMemoryRange range = new VkMappedMemoryRange {
                sType = VkStructureType.MappedMemoryRange,
                memory = vkMemory,
                offset = offset,
                size = size,
            };
            vkFlushMappedMemoryRanges (dev.VkDev, 1, ref range);
        }

        #region IDisposable Support        
        protected override void Dispose (bool disposing) {
			if (!disposing)
				System.Diagnostics.Debug.WriteLine ("VKE Activable object disposed by finalizer");
			if (state == ActivableState.Activated) {
				if (mappedData != IntPtr.Zero)
					Unmap ();
				vkFreeMemory (dev.VkDev, vkMemory, IntPtr.Zero);
			}
			base.Dispose (disposing);
        }
        #endregion
    }
}