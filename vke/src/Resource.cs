using System;
using System.IO;
using System.Runtime.InteropServices;
using Vulkan;

using static Vulkan.VulkanNative;

namespace VKE {
    public abstract class Resource : IDisposable {
		internal uint references;
        protected Device dev;
        protected VkDeviceMemory vkMemory;
        protected UInt64 deviceMemSize;
        protected IntPtr mappedData;

        public readonly VkMemoryPropertyFlags MemoryFlags;
        public IntPtr MappedData => mappedData;
		public UInt64 AllocatedDeviceMemorySize => deviceMemSize;

        protected Resource (Device device, VkMemoryPropertyFlags memoryFlags) {
            dev = device;
            MemoryFlags = memoryFlags;
        }

        protected abstract VkMemoryRequirements getMemoryRequirements ();
        protected abstract void bindMemory (ulong offset);

        //memory is not allocated and bindind in here because Activate may be called in ctor of
        //derrived classes, and appropriate virtual allocateMemory and bindMemory must be called
        public virtual void Activate () {
            if (isDisposed)
                GC.ReRegisterForFinalize (this);
        }

        protected void allocateMemory () {
            VkMemoryRequirements memReqs = getMemoryRequirements ();
            VkMemoryAllocateInfo memInfo = VkMemoryAllocateInfo.New ();
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
        public void Update (object data, ulong size) {
            GCHandle ptr = GCHandle.Alloc (data, GCHandleType.Pinned);
            unsafe {
                System.Buffer.MemoryCopy (ptr.AddrOfPinnedObject ().ToPointer (), mappedData.ToPointer (), size, size);
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
        protected bool isDisposed;

        protected virtual void Dispose (bool disposing) {
            if (!isDisposed) {
                if (disposing) {
                    // TODO: supprimer l'état managé (objets managés).
                }

                if (mappedData != IntPtr.Zero)
                    Unmap ();
                vkFreeMemory (dev.VkDev, vkMemory, IntPtr.Zero);

                isDisposed = true;
            }
        }

        ~Resource () {
            Dispose (false);
        }

        public void Dispose () {
			if (references>0)
				references--;
			if (references>0)
				return;
            Dispose (true);
            GC.SuppressFinalize (this);
        }
        #endregion
    }
}