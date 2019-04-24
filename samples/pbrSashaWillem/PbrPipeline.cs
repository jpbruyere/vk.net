using System;
using System.Numerics;
using System.Runtime.InteropServices;
using VK;

namespace CVKL {
	class PBRPipeline : GraphicPipeline {
		[StructLayout(LayoutKind.Sequential)]
		public struct Matrices {
			public Matrix4x4 projection;
			public Matrix4x4 model;
			public Matrix4x4 view;
			public Vector3 camPos;
		}
		[StructLayout (LayoutKind.Sequential)]
		public struct Params {
			public Vector4 lightDir;
			public float exposure;
			public float gamma;
			public float prefilteredCubeMipLevels;
			public float scaleIBLAmbient;
			public float debugViewInputs;
			public float debugViewEquation;
		}

		public Params parametters = new Params {
			lightDir = Vector4.Normalize(new Vector4 (0.7f, 0.6f, 0.2f, 0.0f)),
			gamma = 2.2f,
			exposure = 4.5f,
			scaleIBLAmbient = 1f,
			debugViewInputs = 0,
			debugViewEquation = 0
		};


		public Matrices matrices = new Matrices {
		};

		public HostBuffer uboMats, uboSkybox;
		public HostBuffer uboParams;

		DescriptorPool descriptorPool;

		DescriptorSetLayout descLayoutMain;
		DescriptorSetLayout descLayoutTextures;
		DescriptorSet dsMain, dsSkybox;

		public PbrModel2 model;
		public EnvironmentCube envCube;

		public PBRPipeline (Queue staggingQ, RenderPass renderPass) :
			base (renderPass, "pbr pipeline") {

			descriptorPool = new DescriptorPool (Dev, 3,
				new VkDescriptorPoolSize (VkDescriptorType.UniformBuffer, 6),
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler, 11)
			);

			descLayoutMain = new DescriptorSetLayout (Dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment, VkDescriptorType.UniformBuffer),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.UniformBuffer),
				new VkDescriptorSetLayoutBinding (2, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (3, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (4, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (5, VkShaderStageFlags.Fragment, VkDescriptorType.UniformBuffer));

			descLayoutTextures = new DescriptorSetLayout (Dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (2, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (3, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (4, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler)
			);

			dsMain = descriptorPool.Allocate (descLayoutMain);
			dsSkybox = descriptorPool.Allocate (descLayoutMain);

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

			envCube = new EnvironmentCube (dsSkybox, layout, staggingQ, RenderPass);

			parametters.prefilteredCubeMipLevels = envCube.prefilterCube.CreateInfo.mipLevels;

			uboMats = new HostBuffer (Dev, VkBufferUsageFlags.UniformBuffer, (ulong)Marshal.SizeOf<Matrices> ());
			uboSkybox = new HostBuffer (Dev, VkBufferUsageFlags.UniformBuffer, (ulong)Marshal.SizeOf<Matrices> ());
			uboParams = new HostBuffer (Dev, VkBufferUsageFlags.UniformBuffer, (ulong)Marshal.SizeOf<Params> ());

			uboMats.Map ();//permanent map
			uboSkybox.Map ();
			uboParams.Map ();

			string[] modelPathes = {
				"../data/models/DamagedHelmet/glTF/DamagedHelmet.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/Avocado/glTF/Avocado.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/BarramundiFish/glTF/BarramundiFish.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/BoomBoxWithAxes/glTF/BoomBoxWithAxes.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/Box/glTF/Box.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/EnvironmentTest/glTF/EnvironmentTest.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/MetalRoughSpheres/glTF/MetalRoughSpheres.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/OrientationTest/glTF/OrientationTest.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/Buggy/glTF/Buggy.gltf",
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/2CylinderEngine/glTF-Embedded/2CylinderEngine.gltf"
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
			uboUpdate.Write (Dev, dsSkybox,
				uboSkybox.Descriptor,
				uboParams.Descriptor,
				envCube.cubemap.Descriptor,
				envCube.prefilterCube.Descriptor,
				envCube.lutBrdf.Descriptor,
				model.materialUBO.Descriptor);
			uboUpdate.Write (Dev, dsMain,
				uboMats.Descriptor,
				uboParams.Descriptor,
				envCube.irradianceCube.Descriptor,
				envCube.prefilterCube.Descriptor,
				envCube.lutBrdf.Descriptor,
				model.materialUBO.Descriptor);

		}

		public void RecordDraw (CommandBuffer cmd) {		
			envCube.RecordDraw (cmd);
			drawModel (cmd);
		}
		void drawModel (CommandBuffer cmd) {
			Bind (cmd);
			cmd.BindDescriptorSet (Layout, dsMain);
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
			uboSkybox.Dispose ();
			uboParams.Dispose ();

			base.Dispose (disposing);
		}
	}

}
