using System;
using System.IO;
using System.Runtime.InteropServices;
using VK;

using static VK.Vk;

namespace CVKL {
	public enum MemoryPoolType {
		Random,
		Linear 
	}
	public class MemoryPool : IDisposable {
		Device dev;
		internal VkDeviceMemory vkMemory;
		VkMemoryAllocateInfo memInfo = VkMemoryAllocateInfo.New ();

		Resource firstResource;
		Resource lastResource;

		//Resource mappedFrom;
		//Resource mappedTo;

		ulong freeMemPointer;
		IntPtr mappedPointer;

		public ulong Size => memInfo.allocationSize;
		public bool IsMapped => mappedPointer != IntPtr.Zero;
		public IntPtr MappedData => mappedPointer;
		public Resource First => firstResource;

		public MemoryPool (Device dev, uint memoryTypeIndex, UInt64 size) {
			this.dev = dev;
			memInfo.allocationSize = size;
			memInfo.memoryTypeIndex = memoryTypeIndex;
			Utils.CheckResult (vkAllocateMemory (dev.VkDev, ref memInfo, IntPtr.Zero, out vkMemory));
		}

		public void Add (Resource resource) {
			resource.memoryPool = this;

			if (lastResource == null) {
				firstResource = lastResource = resource;
				resource.poolOffset = 0;
			} else {

				resource.poolOffset = lastResource.poolOffset + lastResource.AllocatedDeviceMemorySize;
				resource.poolOffset += resource.MemoryAlignment - (resource.poolOffset % resource.MemoryAlignment);

				lastResource.next = resource;
				resource.previous = lastResource;
				lastResource = resource;
			}
			resource.bindMemory ();
		}

		public void Defrag () { 

		}

		public void Remove (Resource resource) {
			if (resource == firstResource)
				firstResource = resource.next;
			if (resource == lastResource)
				lastResource = resource.previous;
			if (resource.previous != null)
				resource.previous.next = resource.next;
			if (resource.next != null)
				resource.next.previous = resource.previous;

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
}