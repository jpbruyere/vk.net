using System;
using System.IO;
using System.Runtime.InteropServices;
using VK;

using static VK.Vk;

namespace CVKL {
#if MEMORY_POOLS
	public enum MemoryPoolType {
		Random,
		Linear 
	}
	public class MemoryPool : IDisposable {
		Device dev;
		internal VkDeviceMemory vkMemory;
		VkMemoryAllocateInfo memInfo = VkMemoryAllocateInfo.New ();

		//Resource firstResource;
		Resource lastResource;

		//Resource mappedFrom;
		//Resource mappedTo;

		//ulong freeMemPointer;
		IntPtr mappedPointer;

		public ulong Size => memInfo.allocationSize;
		public bool IsMapped => mappedPointer != IntPtr.Zero;
		public IntPtr MappedData => mappedPointer;
		public Resource Last => lastResource;

		public MemoryPool (Device dev, uint memoryTypeIndex, UInt64 size) {
			this.dev = dev;
			memInfo.allocationSize = size;
			memInfo.memoryTypeIndex = memoryTypeIndex;
			Utils.CheckResult (vkAllocateMemory (dev.VkDev, ref memInfo, IntPtr.Zero, out vkMemory));
		}

		public void Add (Resource resource) {
			resource.memoryPool = this;

			ulong limit = Size;
			ulong offset = 0;
			Resource previous = lastResource;

			if (previous != null) {
				do {
					offset = previous.poolOffset + previous.AllocatedDeviceMemorySize;
					offset += resource.MemoryAlignment - (offset % resource.MemoryAlignment);

					if (previous.next == null) {
						if (offset + resource.AllocatedDeviceMemorySize >= limit) {
							offset = 0;
							limit = previous.poolOffset;
						}
						break;
					}

					if (previous.next.poolOffset < previous.poolOffset) {
						limit = Size;
						if (offset + resource.AllocatedDeviceMemorySize < Size) 
							break;
						offset = 0;
						limit = previous.next.poolOffset;
					}else
						limit = previous.next.poolOffset;

					if (offset + resource.AllocatedDeviceMemorySize < limit)
						break;

					previous = previous.next;

				} while (previous != lastResource);

			}

			if (offset + resource.AllocatedDeviceMemorySize >= limit)
				throw new Exception ($"Out of Memory pool: {memInfo.memoryTypeIndex}");

			resource.poolOffset = offset;
			resource.previous = previous;
			if (previous != null) {
				if (previous.next == null) {
					resource.next = resource.previous = previous;
					previous.next = previous.previous = resource;
				} else {
					resource.next = previous.next;
					previous.next = resource;
				}
			}
			lastResource = resource;

			resource.bindMemory ();
		}



		public void Defrag () { 

		}

		public void Remove (Resource resource) {
			if (resource == lastResource)
				lastResource = resource.previous;
			if (lastResource != null) {
				if (resource.previous == resource.next)//only 1 resources remaining
					lastResource.next = lastResource.previous = null;
				else {
					resource.previous.next = resource.next;
					resource.next.previous = resource.previous;
				}
			}
			resource.next = resource.previous = null;
		}

		public void Map (ulong size = Vk.WholeSize, ulong offset = 0) {
			Utils.CheckResult (vkMapMemory (dev.VkDev, vkMemory, offset, size, 0, ref mappedPointer));
		}
		public void Unmap () {
			vkUnmapMemory (dev.VkDev, vkMemory);
			mappedPointer = IntPtr.Zero;
		}

#region IDisposable Support
		private bool disposedValue;

		protected virtual void Dispose (bool disposing) {
			if (!disposedValue) {
				if (!disposing)
					System.Diagnostics.Debug.WriteLine ("MemoryPool disposed by Finalizer.");
				vkFreeMemory (dev.VkDev, vkMemory, IntPtr.Zero);
				disposedValue = true;
			}
		}

		~MemoryPool() {	
			Dispose(false);
		}
		public void Dispose () {
			Dispose (true);
			GC.SuppressFinalize(this);
		}
#endregion
	}
#endif
}