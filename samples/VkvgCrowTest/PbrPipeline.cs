using System;
using System.Numerics;
using System.Runtime.InteropServices;
using VK;
using System.Diagnostics;
using System.Collections.Generic;

namespace CVKL {
	using static VK.Utils;


	//TODO:stride in buffer views?
	public class PbrModel2 : Model {
		public struct Vertex {
			[VertexAttribute (VertexAttributeType.Position, VkFormat.R32g32b32Sfloat)]
			public Vector3 pos;
			[VertexAttribute (VertexAttributeType.Normal, VkFormat.R32g32b32Sfloat)]
			public Vector3 normal;
			[VertexAttribute (VertexAttributeType.UVs, VkFormat.R32g32Sfloat)]
			public Vector2 uv0;
			[VertexAttribute (VertexAttributeType.UVs, VkFormat.R32g32Sfloat)]
			public Vector2 uv1;
			public override string ToString () {
				return pos.ToString () + ";" + normal.ToString () + ";" + uv0.ToString () + ";" + uv1.ToString ();
			}
		};
		/// <summary>
		/// Pbr data structure suitable for push constant or ubo, containing
		/// availablility of attached textures and the coef of pbr inputs
		/// </summary>
		public struct PbrMaterial {
			public Vector4 baseColorFactor;
			public Vector4 emissiveFactor;
			public Vector4 diffuseFactor;
			public Vector4 specularFactor;
			public float workflow;
			public AttachmentType TexCoord0;
			public AttachmentType TexCoord1;
			public float metallicFactor;
			public float roughnessFactor;
			public float alphaMask;
			public float alphaMaskCutoff;
			int pad0;//see std420
		}

		Image[] textures;
		PbrMaterial[] materials;
		DescriptorSet[] descriptorSets;

		DescriptorPool descriptorPool;
		public GPUBuffer vbo;
		public GPUBuffer ibo;
		public GPUBuffer<PbrMaterial> materialUBO;

		public PbrModel2 (Queue transferQ, string path) {
			dev = transferQ.Dev;
			using (CommandPool cmdPool = new CommandPool (dev, transferQ.index)) {
				using (glTFLoader ctx = new glTFLoader (path, transferQ, cmdPool)) {
					ulong vertexCount, indexCount;

					ctx.GetVertexCount (out vertexCount, out indexCount, out IndexBufferType);

					ulong vertSize = vertexCount * (ulong)Marshal.SizeOf<Vertex> ();
					ulong idxSize = indexCount * (IndexBufferType == VkIndexType.Uint16 ? 2ul : 4ul);
					ulong size = vertSize + idxSize;

					vbo = new GPUBuffer (dev, VkBufferUsageFlags.VertexBuffer | VkBufferUsageFlags.TransferDst, vertSize);
					ibo = new GPUBuffer (dev, VkBufferUsageFlags.IndexBuffer | VkBufferUsageFlags.TransferDst, idxSize);

					vbo.SetName ("vbo gltf");
					ibo.SetName ("ibo gltf");

					Meshes = new List<Mesh> (ctx.LoadMeshes<Vertex> (IndexBufferType, vbo, 0, ibo, 0));
					Scenes = new List<Scene> (ctx.LoadScenes (out defaultSceneIndex));
				}
			}
		}
		public PbrModel2 (Queue transferQ, string path, DescriptorSetLayout layout, params AttachmentType[] attachments) {
			dev = transferQ.Dev;
			using (CommandPool cmdPool = new CommandPool (dev, transferQ.index)) {
				using (glTFLoader ctx = new glTFLoader (path, transferQ, cmdPool)) {
					ulong vertexCount, indexCount;

					ctx.GetVertexCount (out vertexCount, out indexCount, out IndexBufferType);
					ulong vertSize = vertexCount * (ulong)Marshal.SizeOf<Vertex> ();
					ulong idxSize = indexCount * (IndexBufferType == VkIndexType.Uint16 ? 2ul : 4ul);
					ulong size = vertSize + idxSize;

					vbo = new GPUBuffer (dev, VkBufferUsageFlags.VertexBuffer | VkBufferUsageFlags.TransferDst, vertSize);
					ibo = new GPUBuffer (dev, VkBufferUsageFlags.IndexBuffer | VkBufferUsageFlags.TransferDst, idxSize);

					vbo.SetName ("vbo gltf");
					ibo.SetName ("ibo gltf");

					Meshes = new List<Mesh> (ctx.LoadMeshes<Vertex> (IndexBufferType, vbo, 0, ibo, 0));
					textures = ctx.LoadImages ();

					loadMaterials (ctx, layout, attachments);

					materialUBO = new GPUBuffer<PbrMaterial> (transferQ, cmdPool, VkBufferUsageFlags.UniformBuffer, materials);

					Scenes = new List<Scene> (ctx.LoadScenes (out defaultSceneIndex));
				}
			}
		}

		void loadMaterials (glTFLoader ctx, DescriptorSetLayout layout, params AttachmentType[] attachments) {
			Model.Material[] mats = ctx.LoadMaterial ();
			materials = new PbrMaterial[mats.Length];
			descriptorSets = new DescriptorSet[mats.Length];

			if (attachments.Length == 0)
				throw new InvalidOperationException ("At least one attachment is required for Model.WriteMaterialDescriptor");

			descriptorPool = new DescriptorPool (dev, (uint)materials.Length,
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler, (uint)(attachments.Length * materials.Length))
			);
			descriptorPool.SetName ("descPool gltfTextures");

			for (int i = 0; i < mats.Length; i++) {
				materials[i] = new PbrMaterial {
					workflow = (float)mats[i].workflow,
					baseColorFactor = mats[i].baseColorFactor,
					emissiveFactor = mats[i].emissiveFactor,
					metallicFactor = mats[i].metallicFactor,
					roughnessFactor = mats[i].roughnessFactor,
					TexCoord0 = mats[i].availableAttachments,
					TexCoord1 = mats[i].availableAttachments1,
					alphaMask = 0f,
					alphaMaskCutoff = 0.0f,
					diffuseFactor = new Vector4 (0),
					specularFactor = new Vector4 (0)
				};

				descriptorSets[i] = descriptorPool.Allocate (layout);
				descriptorSets[i].Handle.SetDebugMarkerName (dev, "descSet " + mats[i].Name);

				VkDescriptorSetLayoutBinding dslb =
					new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler);

				using (DescriptorSetWrites2 uboUpdate = new DescriptorSetWrites2 (dev)) {
					for (uint a = 0; a < attachments.Length; a++) {
						dslb.binding = a;
						switch (attachments[a]) {
							case AttachmentType.None:
								break;
							case AttachmentType.Color:
								if (mats[i].availableAttachments.HasFlag (AttachmentType.Color))
									uboUpdate.AddWriteInfo (descriptorSets[i], dslb, textures[(int)mats[i].baseColorTexture].Descriptor);
								break;
							case AttachmentType.Normal:
								if (mats[i].availableAttachments.HasFlag (AttachmentType.Normal))
									uboUpdate.AddWriteInfo (descriptorSets[i], dslb, textures[(int)mats[i].normalTexture].Descriptor);
								break;
							case AttachmentType.AmbientOcclusion:
								if (mats[i].availableAttachments.HasFlag (AttachmentType.AmbientOcclusion))
									uboUpdate.AddWriteInfo (descriptorSets[i], dslb, textures[(int)mats[i].occlusionTexture].Descriptor);
								break;
							case AttachmentType.PhysicalProps:
								if (mats[i].availableAttachments.HasFlag (AttachmentType.PhysicalProps))
									uboUpdate.AddWriteInfo (descriptorSets[i], dslb, textures[(int)mats[i].metallicRoughnessTexture].Descriptor);
								break;
							case AttachmentType.Metal:
								break;
							case AttachmentType.Roughness:
								break;
							case AttachmentType.Emissive:
								if (mats[i].availableAttachments.HasFlag (AttachmentType.Emissive))
									uboUpdate.AddWriteInfo (descriptorSets[i], dslb, textures[(int)mats[i].emissiveTexture].Descriptor);
								break;
						}
					}
					uboUpdate.Update ();
				}
			}
		}


		/// <summary> bind vertex and index buffers </summary>
		public void Bind (CommandBuffer cmd) {
			cmd.BindVertexBuffer (vbo);
			cmd.BindIndexBuffer (ibo, IndexBufferType);
		}

		//TODO:destset for binding must be variable
		//TODO: ADD REFAULT MAT IF NO MAT DEFINED
		public void RenderNode (CommandBuffer cmd, PipelineLayout pipelineLayout, Node node, Matrix4x4 currentTransform) {
			Matrix4x4 localMat = node.localMatrix * currentTransform;

			cmd.PushConstant (pipelineLayout, VkShaderStageFlags.Vertex, localMat);

			if (node.Mesh != null) {
				foreach (Primitive p in node.Mesh.Primitives) {
					cmd.PushConstant (pipelineLayout, VkShaderStageFlags.Fragment, (int)p.material, (uint)Marshal.SizeOf<Matrix4x4> ());
					if (descriptorSets[p.material] != null)
						cmd.BindDescriptorSet (pipelineLayout, descriptorSets[p.material], 1);
					cmd.DrawIndexed (p.indexCount, 1, p.indexBase, p.vertexBase, 0);
				}
			}
			if (node.Children == null)
				return;
			foreach (Node child in node.Children)
				RenderNode (cmd, pipelineLayout, child, localMat);
		}

		public void DrawAll (CommandBuffer cmd, PipelineLayout pipelineLayout) {
			foreach (Scene sc in Scenes) {
				foreach (Node node in sc.Root.Children) {
					RenderNode (cmd, pipelineLayout, node, sc.Root.localMatrix);
				}
			}
		}

		#region IDisposable Support
		private bool isDisposed = false; // Pour détecter les appels redondants

		protected virtual void Dispose (bool disposing) {
			if (!isDisposed) {
				if (disposing) {
					ibo?.Dispose ();
					vbo?.Dispose ();
					materialUBO?.Dispose ();
					descriptorPool?.Dispose ();
					foreach (Image txt in textures) {
						txt.Dispose ();
					}
				} else
					Debug.WriteLine ("model was not disposed");
				isDisposed = true;
			}
		}
		public void Dispose () {
			Dispose (true);
		}
		#endregion
	}
}

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
				new VkPushConstantRange (VkShaderStageFlags.Fragment, sizeof (int), 64)
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
				"../data/models/DamagedHelmet/glTF/DamagedHelmet.gltf",
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
