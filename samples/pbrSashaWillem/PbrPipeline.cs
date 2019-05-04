using System;
using System.Numerics;
using System.Runtime.InteropServices;
using VK;

namespace CVKL {
	class PBRPipeline : GraphicPipeline {

		public struct Matrices {
			public Matrix4x4 projection;
			public Matrix4x4 model;
			public Matrix4x4 view;
			public Vector4 camPos;
			public Vector4 lightDir;
			public float exposure;
			public float gamma;
			public float prefilteredCubeMipLevels;
			public float scaleIBLAmbient;
			public float debugViewInputs;
			public float debugViewEquation;
		}

		public Matrices matrices = new Matrices {
			lightDir = Vector4.Normalize (new Vector4 (0.7f, 0.6f, 0.2f, 0.0f)),
			gamma = 2.2f,
			exposure = 4.5f,
			scaleIBLAmbient = 1f,
			debugViewInputs = 0,
			debugViewEquation = 0
		};

		public HostBuffer uboMats;

		DescriptorPool descriptorPool;

		DescriptorSetLayout descLayoutMain;
		DescriptorSetLayout descLayoutTextures;
		public DescriptorSet dsMain;

		public PbrModel2 model;
		public EnvironmentCube envCube;

		public PBRPipeline (Queue staggingQ, RenderPass renderPass, Image uiImage, PipelineCache pipelineCache = null) :
			base (renderPass, pipelineCache, "pbr pipeline") {

			descriptorPool = new DescriptorPool (Dev, 2,
				new VkDescriptorPoolSize (VkDescriptorType.UniformBuffer, 2),
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler, 9)
			);

			descLayoutMain = new DescriptorSetLayout (Dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment, VkDescriptorType.UniformBuffer),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (2, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (3, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (4, VkShaderStageFlags.Fragment, VkDescriptorType.UniformBuffer),
				new VkDescriptorSetLayoutBinding (5, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler));//ui image

			descLayoutTextures = new DescriptorSetLayout (Dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (2, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (3, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (4, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler)
			);
				
			GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, renderPass.Samples);
			cfg.Layout = new PipelineLayout (Dev, descLayoutMain, descLayoutTextures);
			cfg.Layout.AddPushConstants (
				new VkPushConstantRange (VkShaderStageFlags.Vertex, (uint)Marshal.SizeOf<Matrix4x4> ()),
				new VkPushConstantRange (VkShaderStageFlags.Fragment, sizeof(int), 64)
			);
			cfg.RenderPass = renderPass;
			cfg.AddVertexBinding<PbrModel2.Vertex> (0);
			cfg.SetVertexAttributes (0, VkFormat.R32g32b32Sfloat, VkFormat.R32g32b32Sfloat, VkFormat.R32g32Sfloat, VkFormat.R32g32Sfloat);

			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/pbr.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/pbr_khr.frag.spv");

			layout = cfg.Layout;

			init (cfg);

			dsMain = descriptorPool.Allocate (descLayoutMain);

			envCube = new EnvironmentCube (dsMain, layout, staggingQ, RenderPass);

			matrices.prefilteredCubeMipLevels = envCube.prefilterCube.CreateInfo.mipLevels;
			uboMats = new HostBuffer (Dev, VkBufferUsageFlags.UniformBuffer, matrices, true);

			string[] modelPathes = {
				"../../../samples/data/models/DamagedHelmet/glTF/DamagedHelmet.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/Avocado/glTF/Avocado.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/BarramundiFish/glTF/BarramundiFish.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/BoomBoxWithAxes/glTF/BoomBoxWithAxes.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/Box/glTF/Box.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/EnvironmentTest/glTF/EnvironmentTest.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/MetalRoughSpheres/glTF/MetalRoughSpheres.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/OrientationTest/glTF/OrientationTest.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/Buggy/glTF/Buggy.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/2CylinderEngine/glTF-Embedded/2CylinderEngine.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/FlightHelmet/glTF/FlightHelmet.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/GearboxAssy/glTF/GearboxAssy.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/Lantern/glTF/Lantern.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/SciFiHelmet/glTF/SciFiHelmet.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/Sponza/glTF/Sponza.gltf",
				"/mnt/devel/vkChess/data/chess.gltf"
			};


			model = new PbrModel2 (staggingQ, modelPathes[0], descLayoutTextures,
				AttachmentType.Color,
				AttachmentType.PhysicalProps,
				AttachmentType.Normal,
				AttachmentType.AmbientOcclusion,
				AttachmentType.Emissive);
			//model = new Model (Dev, presentQueue, "../data/models/icosphere.gltf");
			//model = new Model (Dev, presentQueue, cmdPool, "../data/models/cube.gltf");
			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (descLayoutMain);
			uboUpdate.Write (Dev, dsMain,
				uboMats.Descriptor,
				envCube.irradianceCube.Descriptor,
				envCube.prefilterCube.Descriptor,
				envCube.lutBrdf.Descriptor,
				model.materialUBO.Descriptor,
				uiImage.Descriptor);

		}

		public void RecordDraw (CommandBuffer cmd) {
			cmd.BindDescriptorSet (Layout, dsMain);
			envCube.RecordDraw (cmd);
			drawModel (cmd);
		}
		void drawModel (CommandBuffer cmd) {
			Bind (cmd);
			model.Bind (cmd);
			model.DrawAll (cmd, Layout);

		}

		protected override void Dispose (bool disposing) {
			model.Dispose ();
			envCube.Dispose ();

			descLayoutMain.Dispose ();
			descLayoutTextures.Dispose ();
			descriptorPool.Dispose ();

			uboMats.Dispose ();

			base.Dispose (disposing);
		}
	}

}
