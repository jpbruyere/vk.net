using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;
using VK;
using VKE;
using Vulkan;
using Buffer = VKE.Buffer;

namespace VKE {
	public class Skybox : IDisposable {
		static float[] box_vertices = {
			-1.0f,-1.0f,-1.0f,    0.0f, 1.0f,  // -X side
			-1.0f,-1.0f, 1.0f,    1.0f, 1.0f,
			-1.0f, 1.0f, 1.0f,    1.0f, 0.0f,
			-1.0f, 1.0f, 1.0f,    1.0f, 0.0f,
			-1.0f, 1.0f,-1.0f,    0.0f, 0.0f,
			-1.0f,-1.0f,-1.0f,    0.0f, 1.0f,

			-1.0f,-1.0f,-1.0f,    1.0f, 1.0f,  // -Z side
			 1.0f, 1.0f,-1.0f,    0.0f, 0.0f,
			 1.0f,-1.0f,-1.0f,    0.0f, 1.0f,
			-1.0f,-1.0f,-1.0f,    1.0f, 1.0f,
			-1.0f, 1.0f,-1.0f,    1.0f, 0.0f,
			 1.0f, 1.0f,-1.0f,    0.0f, 0.0f,

			-1.0f,-1.0f,-1.0f,    1.0f, 0.0f,  // -Y side
			 1.0f,-1.0f,-1.0f,    1.0f, 1.0f,
			 1.0f,-1.0f, 1.0f,    0.0f, 1.0f,
			-1.0f,-1.0f,-1.0f,    1.0f, 0.0f,
			 1.0f,-1.0f, 1.0f,    0.0f, 1.0f,
			-1.0f,-1.0f, 1.0f,    0.0f, 0.0f,

			-1.0f, 1.0f,-1.0f,    1.0f, 0.0f,  // +Y side
			-1.0f, 1.0f, 1.0f,    0.0f, 0.0f,
			 1.0f, 1.0f, 1.0f,    0.0f, 1.0f,
			-1.0f, 1.0f,-1.0f,    1.0f, 0.0f,
			 1.0f, 1.0f, 1.0f,    0.0f, 1.0f,
			 1.0f, 1.0f,-1.0f,    1.0f, 1.0f,

			 1.0f, 1.0f,-1.0f,    1.0f, 0.0f,  // +X side
			 1.0f, 1.0f, 1.0f,    0.0f, 0.0f,
			 1.0f,-1.0f, 1.0f,    0.0f, 1.0f,
			 1.0f,-1.0f, 1.0f,    0.0f, 1.0f,
			 1.0f,-1.0f,-1.0f,    1.0f, 1.0f,
			 1.0f, 1.0f,-1.0f,    1.0f, 0.0f,

			-1.0f, 1.0f, 1.0f,    0.0f, 0.0f,  // +Z side
			-1.0f,-1.0f, 1.0f,    0.0f, 1.0f,
			 1.0f, 1.0f, 1.0f,    1.0f, 0.0f,
			-1.0f,-1.0f, 1.0f,    0.0f, 1.0f,
			 1.0f,-1.0f, 1.0f,    1.0f, 1.0f,
			 1.0f, 1.0f, 1.0f,    1.0f, 0.0f,
		};

		GPUBuffer vboSkybox;
		DescriptorSet dsSkybox;
		DescriptorSetLayout dsLayoutMain;

		public Image cubemap;
		string[] cubemapPathes = {
			"../data/textures/papermill.ktx",
			"../data/textures/cubemap_yokohama_bc3_unorm.ktx",
			"../data/textures/gcanyon_cube.ktx",
			"../data/textures/pisa_cube.ktx",
			"../data/textures/uffizi_cube.ktx",
		};

		//string[] imgPathes = {
		//	"../data/textures/papermill.ktx",
		//	"../data/textures/cubemap_yokohama_bc3_unorm.ktx",
		//	"../data/textures/gcanyon_cube.ktx",
		//	"../data/textures/pisa_cube.ktx",
		//	"../data/textures/uffizi_cube.ktx",
		//};


		public Skybox (Queue presentQueue, CommandPool cmdPool , DescriptorSetLayout dsLayout) {
			dsLayoutMain = dsLayout;

			vboSkybox = new GPUBuffer<float> (presentQueue, cmdPool, VkBufferUsageFlags.VertexBuffer, box_vertices);

			cubemap = KTX.KTX.Load (presentQueue, cmdPool, cubemapPathes[0],
				VkImageUsageFlags.Sampled, VkMemoryPropertyFlags.DeviceLocal, true);
			cubemap.CreateView (VkImageViewType.Cube, VkImageAspectFlags.Color, 6);
			cubemap.CreateSampler ();
		}

		public void Dispose () {
			throw new NotImplementedException ();
		}
	}
}
