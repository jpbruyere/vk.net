using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;
using VKE;
using Vulkan;
using Buffer = VKE.Buffer;

namespace VKE {
	public class Skybox {
		//struct Matrices {
		//	public Matrix4x4 projection;
		//	public Matrix4x4 view;
		//}

		//Matrices matrices;

		//HostBuffer 			uboMats;
		//GPUBuffer<float> 	vbo;
		//DescriptorPool 		descriptorPool;
		//DescriptorSetLayout dsLayout;
		//DescriptorSet 		descriptorSet;
		//Pipeline 			pipeline;
		//Framebuffer[] 		frameBuffers;

		//Image texture;

		static float[] g_vertex_buffer_data = {
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

		//string[] imgPathes = {
		//	"../data/textures/papermill.ktx",
		//	"../data/textures/cubemap_yokohama_bc3_unorm.ktx",
		//	"../data/textures/gcanyon_cube.ktx",
		//	"../data/textures/pisa_cube.ktx",
		//	"../data/textures/uffizi_cube.ktx",
		//};


		//public Skybox () {
		//	vbo = new GPUBuffer<float> (presentQueue, cmdPool, VkBufferUsageFlags.VertexBuffer, g_vertex_buffer_data);

		//	texture = KTX.KTX.Load (presentQueue, cmdPool, imgPathes[0],
		//		VkImageUsageFlags.Sampled, VkMemoryPropertyFlags.DeviceLocal, true);
		//	texture.CreateView (VkImageViewType.ImageCube,VkImageAspectFlags.Color,6);
		//	texture.CreateSampler ();

		//	descriptorPool = new DescriptorPool (dev, 1,
		//		new VkDescriptorPoolSize (VkDescriptorType.UniformBuffer),
		//		new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler)
		//	);

		//	dsLayout = new DescriptorSetLayout (dev, VkDescriptorSetLayoutCreateFlags.None,
		//		new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Vertex, VkDescriptorType.UniformBuffer),
		//		new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler));

		//	descriptorSet = descriptorPool.Allocate (dsLayout);

		//	pipeline = new Pipeline (dev,
		//		swapChain.ColorFormat,
		//		dev.GetSuitableDepthFormat (),
		//		VkPrimitiveTopology.TriangleList, VkSampleCountFlags.Count1);

		//	pipeline.Layout = new PipelineLayout (dev, dsLayout);
		//	pipeline.AddVertexBinding (0, 5 * sizeof(float));
		//	pipeline.SetVertexAttributes (0, VkFormat.R32g32b32Sfloat, VkFormat.R32g32Sfloat);

		//	pipeline.AddShader (VkShaderStageFlags.Vertex, "shaders/skybox.vert.spv");
		//	pipeline.AddShader (VkShaderStageFlags.Fragment, "shaders/skybox.frag.spv");

		//	pipeline.Activate ();

		//	uboMats = new HostBuffer (dev, VkBufferUsageFlags.UniformBuffer, matrices);
		//	uboMats.Map ();//permanent map

		//	texture.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
		//	using (DescriptorSetWrites uboUpdate = new DescriptorSetWrites (dev)) {
		//		uboUpdate.AddWriteInfo (descriptorSet, dsLayout.Bindings[0], uboMats.Descriptor);
		//		uboUpdate.AddWriteInfo (descriptorSet, dsLayout.Bindings[1], texture.Descriptor);
		//		uboUpdate.Update ();
		//	}
		//}

		//void buildCommandBuffers () {
		//	for (int i = 0; i < swapChain.ImageCount; ++i) { 								
  //                	cmds[i]?.Free ();
		//		cmds[i] = cmdPool.AllocateCommandBuffer ();
		//		cmds[i].Start ();

		//		recordDraw (cmds[i], frameBuffers[i]);

		//		cmds[i].End ();				 
		//	}
		//} 
		//void recordDraw (CommandBuffer cmd, Framebuffer fb) { 
		//	pipeline.RenderPass.Begin (cmd, fb);

		//	cmd.SetViewport (fb.Width, fb.Height);
		//	cmd.SetScissor (fb.Width, fb.Height);
		//	cmd.BindDescriptorSet (pipeline.Layout, descriptorSet);

		//	pipeline.Bind (cmd);

		//	cmd.BindVertexBuffer (vbo, 0);
		//	cmd.Draw (36);

		//	pipeline.RenderPass.End (cmd);
		//}			
			
		//void updateMatrices () { 
		//	matrices.projection = Matrix4x4.CreatePerspectiveFieldOfView (Utils.DegreesToRadians (60f), (float)swapChain.Width / (float)swapChain.Height, 0.1f, 256.0f);
		//	matrices.view =
		//		Matrix4x4.CreateFromAxisAngle (Vector3.UnitZ, rotZ) *
		//		Matrix4x4.CreateFromAxisAngle (Vector3.UnitY, rotY) *
		//		Matrix4x4.CreateFromAxisAngle (Vector3.UnitX, rotX);

		//	uboMats.Update (matrices, (uint)Marshal.SizeOf<Matrices> ());
		//}
	}
}
