using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using VK;

using static VK.Vk;

namespace CVKL {
#if MEMORY_POOLS
	public class ResourceManager : IDisposable {
		VkPhysicalDeviceMemoryProperties memoryProperties;
		public MemoryPool[] memoryPools;
		ulong[] reservedHeapMemory;

		VkMemoryHeap getHeapFromMemoryIndex (uint i) => memoryProperties.memoryHeaps[memoryProperties.memoryTypes[i].heapIndex];


		public ResourceManager (Device dev, ulong defaultPoolsBlockDivisor = 16) {
			memoryProperties = dev.phy.memoryProperties;
			memoryPools = new MemoryPool[memoryProperties.memoryTypeCount];
			reservedHeapMemory = new ulong[memoryProperties.memoryHeapCount];

			for (uint i = 0; i < memoryPools.Length; i++) {
				ulong size = getHeapFromMemoryIndex (i).size / defaultPoolsBlockDivisor;
				memoryPools[i] = new MemoryPool (dev, i, size);
				reservedHeapMemory[memoryProperties.memoryTypes[i].heapIndex] += size;
			}
		}

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

		public void Dispose () {
			for (uint i = 0; i < memoryPools.Length; i++)
				memoryPools[i].Dispose ();
		}
	}
#endif
}