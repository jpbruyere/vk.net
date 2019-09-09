using System;
using VK;

namespace CVKL {
#if MEMORY_POOLS
	/// <summary>
	/// Resource manager is responsible for the memory allocations. It holds one pool for each memory type
	/// </summary>
	public class ResourceManager : IDisposable {
		VkPhysicalDeviceMemoryProperties memoryProperties;
		public MemoryPool[] memoryPools;
		ulong[] reservedHeapMemory;

		VkMemoryHeap getHeapFromMemoryIndex (uint i) => memoryProperties.memoryHeaps[memoryProperties.memoryTypes[i].heapIndex];
		/// <summary>
		/// Create a new resource manager that will create one memory pool for each kind of memory available on the device
		/// </summary>
		/// <param name="dev">Device</param>
		/// <param name="defaultPoolsBlockDivisor">part of the whole available memory size to reserve by pools</param>
		public ResourceManager (Device dev, ulong defaultPoolsBlockDivisor = 4) {
			memoryProperties = dev.phy.memoryProperties;
			memoryPools = new MemoryPool[memoryProperties.memoryTypeCount];
			reservedHeapMemory = new ulong[memoryProperties.memoryHeapCount];

			for (uint i = 0; i < memoryPools.Length; i++) {
				ulong size = getHeapFromMemoryIndex (i).size / defaultPoolsBlockDivisor;
				memoryPools[i] = new MemoryPool (dev, i, size);
				reservedHeapMemory[memoryProperties.memoryTypes[i].heapIndex] += size;
			}
		}
		/// <summary>
		/// Add one or more resources to the manager, pool will be choosen depending on the resource memory flags
		/// </summary>
		/// <param name="resources">Resource(s).</param>
		public void Add (params Resource[] resources) {
			foreach (Resource res in resources) {
				res.updateMemoryRequirements ();
				GetMemoryPool (res.TypeBits, res.MemoryFlags).Add (res);
			}
		}

		MemoryPool GetMemoryPool (uint typeBits, VkMemoryPropertyFlags properties) {
			// Iterate over all memory types available for the Device used in this example
			for (uint i = 0; i < memoryProperties.memoryTypeCount; i++) {
				if ((typeBits & 1) == 1) {
					if ((memoryProperties.memoryTypes[i].propertyFlags & properties) == properties) 
						return memoryPools[i];
				}
				typeBits >>= 1;
			}
			throw new InvalidOperationException ("Could not find a suitable memory type!");
		}
		/// <summary>
		/// Dispose all memory pools held by this resource manager.
		/// </summary>
		public void Dispose () {
			for (uint i = 0; i < memoryPools.Length; i++)
				memoryPools[i].Dispose ();
		}
	}
#endif
}