using System;
using System.Numerics;
using System.Runtime.InteropServices;
using VK;

namespace CVKL {
	class PBRPipeline : GraphicPipeline {	
		public struct Matrices {
			public Matrix4x4 projection;
			public Matrix4x4 view;
			public Matrix4x4 model;
			public Vector4 lightPos;
			public float gamma;
			public float exposure;
		}

		public Matrices matrices = new Matrices {
			lightPos = new Vector4 (1.0f, 0.0f, 0.0f, 1.0f),
			gamma = 1.0f,
			exposure = 2.0f,
		};

		public HostBuffer uboMats;

		DescriptorPool descriptorPool;
		DescriptorSetLayout descLayoutMain;
		DescriptorSetLayout descLayoutTextures;
		DescriptorSet dsMain;

		Model model;
		EnvironmentCube envCube;

		public PBRPipeline (Queue staggingQ, RenderPass renderPass) :
			base (renderPass, "pbr pipeline") {

			descriptorPool = new DescriptorPool (Dev, 2,
				new VkDescriptorPoolSize (VkDescriptorType.UniformBuffer, 2),
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler, 8)
			);

			descLayoutMain = new DescriptorSetLayout (Dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment, VkDescriptorType.UniformBuffer),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (2, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (3, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler));

			descLayoutTextures = new DescriptorSetLayout (Dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (2, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (3, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (4, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler)
			);

			dsMain = descriptorPool.Allocate (descLayoutMain);

			GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, renderPass.Samples);

			cfg.Layout = new PipelineLayout (Dev, descLayoutMain, descLayoutTextures);
			cfg.Layout.AddPushConstants (
				new VkPushConstantRange (VkShaderStageFlags.Vertex, (uint)Marshal.SizeOf<Matrix4x4> ()),
				new VkPushConstantRange (VkShaderStageFlags.Fragment, (uint)Marshal.SizeOf<Model.PbrMaterial> (), 64)
			);
			cfg.RenderPass = renderPass;
			cfg.AddVertexBinding<Model.Vertex> (0);
			cfg.SetVertexAttributes (0, VkFormat.R32g32b32Sfloat, VkFormat.R32g32b32Sfloat, VkFormat.R32g32Sfloat);

			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/pbrtest.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/pbrtest.frag.spv");

			layout = cfg.Layout;

			init (cfg);

			envCube = new EnvironmentCube (staggingQ, RenderPass);

			uboMats = new HostBuffer (Dev, VkBufferUsageFlags.UniformBuffer, (ulong)Marshal.SizeOf<Matrices> () * 2);
			uboMats.Map ();//permanent map

			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (descLayoutMain);
			uboUpdate.Write (Dev, dsMain, uboMats.Descriptor,
				envCube.lutBrdf.Descriptor,
				envCube.irradianceCube.Descriptor,
				envCube.prefilterCube.Descriptor);
			uboMats.Descriptor.offset = (ulong)Marshal.SizeOf<Matrices> ();
			envCube.WriteDesc (uboMats.Descriptor);

			model = new Model (staggingQ, "../data/models/DamagedHelmet/glTF/DamagedHelmet.gltf");
			//model = new Model (Dev, presentQueue, "../data/models/icosphere.gltf");
			//model = new Model (Dev, presentQueue, cmdPool, "../data/models/cube.gltf");
			model.WriteMaterialsDescriptorSets (descLayoutTextures,
				VK.AttachmentType.Color,
				VK.AttachmentType.Normal,
				VK.AttachmentType.AmbientOcclusion,
				VK.AttachmentType.MetalRoughness,
				VK.AttachmentType.Emissive);
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
		void drawShadedModelArray (CommandBuffer cmd) {
			Bind (cmd);
			cmd.BindDescriptorSet (Layout, dsMain);
			model.Bind (cmd);
			model.DrawAll (cmd, Layout);


			Matrix4x4 modelMat = Matrix4x4.Identity;
			Model.PbrMaterial material = new Model.PbrMaterial (1, 0, Model.AlphaMode.Opaque, 0.5f, VK.AttachmentType.None);
			Model.Primitive p = model.Scenes[0].Root.Children[0].Mesh.Primitives[0];

			for (int metalFact = 0; metalFact < 10; metalFact++) {

				material.metallicFactor = (float)metalFact / 10;

				for (int roughFact = 0; roughFact < 10; roughFact++) {

					material.roughnessFactor = (float)roughFact / 10;

					modelMat = Matrix4x4.CreateTranslation (-roughFact, metalFact, 0);
				

					cmd.PushConstant (Layout, VkShaderStageFlags.Vertex, modelMat);
					cmd.PushConstant (Layout, VkShaderStageFlags.Fragment, material, (uint)Marshal.SizeOf<Matrix4x4> ());

					cmd.DrawIndexed (p.indexCount, 1, p.indexBase, p.vertexBase, 0);
				}
			}


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
