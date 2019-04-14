//
// Model.cs
//
// Author:
//       Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// Copyright (c) 2019 jp
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using glTFLoader;
using GL = glTFLoader.Schema;

using VK;
using System.Collections.Generic;
using System.IO;



namespace VKE {
    using static VK.Utils;

	[Flags]
	public enum ShaderBinding : UInt32 {
		None,
		Color = 0x1,
		Normal = 0x2,
		AmbientOcclusion = 0x4,
		Metal = 0x8,
		Roughness = 0x16,
		MetalRoughness = 0x32,
		Emissive = 0x64,
	};
	//TODO:stride in buffer views?
	public class Model : IDisposable {
        Device dev;        

		public int DefaultScene;
        public List<Image>		textures = new List<Image> ();
		public List<Material>	materials = new List<Material> ();
		public List<Mesh>		Meshes = new List<Mesh> ();
		public List<Scene>		Scenes;

		DescriptorPool descriptorPool;
        public GPUBuffer vbo;
        public GPUBuffer ibo;
		public VkIndexType IndexBufferType { get; private set; } = VkIndexType.Uint16;

		//public GPUBuffer uboMaterials;
		//[StructLayout (LayoutKind.Explicit, Size = 80)]
		//public struct Material {
		//	[FieldOffset (0)] public Vector4 baseColorFactor;
		//	[FieldOffset (16)] public AlphaMode alphaMode;
		//	[FieldOffset (20)] public float alphaCutoff;
		//	[FieldOffset (24)] public float metallicFactor;
		//	[FieldOffset (28)] public float roughnessFactor;

		//	[FieldOffset (32)] public UInt32 baseColorTexture;
		//	[FieldOffset (36)] public UInt32 metallicRoughnessTexture;
		//	[FieldOffset (40)] public UInt32 normalTexture;
		//	[FieldOffset (44)] public UInt32 occlusionTexture;

		//	[FieldOffset (48)] public UInt32 emissiveTexture;

		//	[FieldOffset (64)] public Vector4 emissiveFactor;

		//	public Material (
		//		UInt32 _baseColorTexture = 0, UInt32 _metallicRoughnessTexture = 0, UInt32 _normalTexture = 0, UInt32 _occlusionTexture = 0) {
		//		baseColorFactor = new Vector4 (1f);
		//		alphaMode = AlphaMode.Opaque;
		//		alphaCutoff = 1f;
		//		metallicFactor = 1f;
		//		roughnessFactor = 1f;

		//		baseColorTexture = _baseColorTexture;
		//		metallicRoughnessTexture = _metallicRoughnessTexture;
		//		normalTexture = _normalTexture;
		//		occlusionTexture = _occlusionTexture;

		//		emissiveTexture = 0;
		//		emissiveFactor = new Vector4 (0.0f);
		//	}
		//}

		/// <summary>
		/// Pbr data structure suitable for push constant, containing
		/// availablility of attached textures and the coef of pbr inputs
		/// </summary>
		public struct PbrMaterial {
			public Vector4 baseColorFactor;
			public Vector4 emissiveFactor;
			public ShaderBinding availableAttachments;
			public Model.AlphaMode alphaMode;
			public float alphaCutoff;
			public float metallicFactor;
			public float roughnessFactor;

			public PbrMaterial (float metallicFactor = 1.0f, float roughnessFactor = 1.0f,
				Model.AlphaMode alphaMode = AlphaMode.Opaque, float alphaCutoff = 0.5f,
				ShaderBinding availableAttachments = ShaderBinding.None)
			{
				this.baseColorFactor = Vector4.One;
				this.emissiveFactor = Vector4.One;
				this.availableAttachments = availableAttachments;
				this.alphaMode = alphaMode;
				this.alphaCutoff = alphaCutoff;
				this.metallicFactor = metallicFactor;
				this.roughnessFactor = roughnessFactor;
			}
			public PbrMaterial (Vector4 baseColorFactor,
				Vector4 emissiveFactor,
				float metallicFactor = 1.0f, float roughnessFactor = 1.0f,
				Model.AlphaMode alphaMode = AlphaMode.Opaque, float alphaCutoff = 0.5f,
				ShaderBinding availableAttachments = ShaderBinding.None)
			{
				this.baseColorFactor = baseColorFactor;
				this.emissiveFactor = emissiveFactor;
				this.availableAttachments = availableAttachments;
				this.alphaMode = alphaMode;
				this.alphaCutoff = alphaCutoff;
				this.metallicFactor = metallicFactor;
				this.roughnessFactor = roughnessFactor;			
			}
		}

		/// <summary>
		/// Material class with textures indices and a descriptorSet for those textures
		/// </summary>
		public class Material {
			public string Name;
			public PbrMaterial pbrDatas;

			public DescriptorSet descriptorSet;

			public UInt32 baseColorTexture;
            public UInt32 metallicRoughnessTexture;
            public UInt32 normalTexture;
            public UInt32 occlusionTexture;
            public UInt32 emissiveTexture;


            public Material (UInt32 _baseColorTexture = 0, UInt32 _metallicRoughnessTexture = 0,
            	UInt32 _normalTexture = 0, UInt32 _occlusionTexture = 0)
			{
                baseColorTexture = _baseColorTexture;
                metallicRoughnessTexture = _metallicRoughnessTexture;
                normalTexture = _normalTexture;
                occlusionTexture = _occlusionTexture;
                emissiveTexture = 0;
            }            
        }        

        public enum AlphaMode : UInt32 {
            Opaque,
            Mask,
            Blend
        };
        
        public struct Vertex {
			[VertexAttribute(VertexAttributeType.Position, VkFormat.R32g32b32Sfloat)]
			public Vector3 pos;
			[VertexAttribute(VertexAttributeType.Normal, VkFormat.R32g32b32Sfloat)]
            public Vector3 normal;
			[VertexAttribute(VertexAttributeType.UVs, VkFormat.R32g32Sfloat)]
            public Vector2 uv;
            public override string ToString () {
                return pos.ToString () + ";" + normal.ToString () + ";" + uv.ToString ();
            }
        };

        public struct Dimensions {
            public Vector3 min;
            public Vector3 max;
            public Vector3 size;

            public Dimensions (Vector3 _min, Vector3 _max, Vector3 _size) {
                min = _min;
                max = _max;
                size = _size;
            }
        }

        public class Primitive {
            public string name;
            public UInt32 indexBase;
            public Int32 vertexBase;
            public UInt32 vertexCount;
            public UInt32 indexCount;
            public UInt32 material;
            public Dimensions dims;
        }

        public struct InstanceData {
            public UInt32 materialIndex;
            public Matrix4x4 modelMat;
            public Vector4 color;
        };

		public class Mesh {
			public string Name;
			public List<Primitive> Primitives = new List<Primitive>();
		}

		public class Scene {
			public string Name;
			public Node Root;
		}

		public class Node {
			public Node Parent;
			public List<Node> Children;
			public Matrix4x4 matrix;
			public Mesh Mesh;
		}

		VkIndexType indexType;

		public Model (Device device, Queue transferQ, string path) {
            dev = device;
			using (CommandPool cmdPool = new CommandPool (device, transferQ.index)) {
				using (glTFLoader ctx = new glTFLoader (path, transferQ, cmdPool)) {
					ulong vertexCount, indexCount;

					ctx.GetVertexCount (out vertexCount, out indexCount);
					ulong vertSize = vertexCount * (ulong)Marshal.SizeOf<Vertex> ();
					ulong idxSize = indexCount * (indexType == VkIndexType.Uint16 ? 2ul : 4ul);
					ulong size = vertSize + idxSize;

					vbo = new GPUBuffer (dev, VkBufferUsageFlags.VertexBuffer | VkBufferUsageFlags.TransferDst, vertSize);
					ibo = new GPUBuffer (dev, VkBufferUsageFlags.IndexBuffer | VkBufferUsageFlags.TransferDst, idxSize);

					#if DEBUG && DEBUG_MARKER
					vbo.SetName ("vbo gltf");
					ibo.SetName ("ibo gltf");
					#endif

					Meshes = new List<Mesh> (ctx.LoadMeshes<Vertex> (VkIndexType.Uint16, vbo, 0, ibo, 0));
					textures = new List<Image> (ctx.LoadImages ());
					materials = new List<Material> (ctx.LoadMaterial ());
					Scenes = new List<Scene> (ctx.LoadScenes (out DefaultScene));
				}
			}
		}

		/// <summary>
		/// Setup descriptorSets for all materials include in model
		/// Depending on pipeline configuration, one or more attachment may be defined
		/// </summary>
		public void WriteMaterialsDescriptorSets (DescriptorSetLayout layout, params ShaderBinding[] attachments) {
			if (attachments.Length == 0)
				throw new InvalidOperationException ("At least one attachment is required for Model.WriteMaterialDescriptor");

			descriptorPool = new DescriptorPool (dev, (uint)materials.Count,
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler, (uint)(attachments.Length * materials.Count))
			);
#if DEBUG && DEBUG_MARKER
			descriptorPool.SetName ("descPool gltfTextures");
#endif
			foreach (Material mat in materials) 
				WriteMaterialDescriptorSet (mat, layout, attachments);
		}

		/// <summary>
		/// Setup one descriptorSet for a single material
		/// </summary>
		/// <param name="mat">Material</param>
		/// <param name="layout">Descriptor Layout for texture</param>
		/// <param name="attachments">Layout Attachments meaning</param>
		public void WriteMaterialDescriptorSet (Material mat, DescriptorSetLayout layout, params ShaderBinding[] attachments) {
			mat.descriptorSet = descriptorPool.Allocate (layout);
#if DEBUG && DEBUG_MARKER
			mat.descriptorSet.handle.SetDebugMarkerName (dev, "descSet " + mat.Name);
#endif
			WriteMaterialDescriptorSet (mat, attachments);
		}

		/// <summary>
		/// Update Writes already allocated material descriptor set.
		/// </summary>
		/// <param name="mat">Material</param>
		/// <param name="attachments">Layout Attachments meaning</param>
		public void WriteMaterialDescriptorSet (Material mat, params ShaderBinding[] attachments) {
			VkDescriptorSetLayoutBinding dslb =
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler);

			using (DescriptorSetWrites2 uboUpdate = new DescriptorSetWrites2 (dev)) {
				for (uint i = 0; i < attachments.Length; i++) {
					dslb.binding = i;
					switch (attachments[i]) {
						case ShaderBinding.None:
							break;
						case ShaderBinding.Color:
							if (mat.pbrDatas.availableAttachments.HasFlag (ShaderBinding.Color))
								uboUpdate.AddWriteInfo (mat.descriptorSet, dslb, textures[(int)mat.baseColorTexture].Descriptor);
							break;
						case ShaderBinding.Normal:
							if (mat.pbrDatas.availableAttachments.HasFlag (ShaderBinding.Normal))
								uboUpdate.AddWriteInfo (mat.descriptorSet, dslb, textures[(int)mat.normalTexture].Descriptor);
							break;
						case ShaderBinding.AmbientOcclusion:
							if (mat.pbrDatas.availableAttachments.HasFlag (ShaderBinding.AmbientOcclusion))
								uboUpdate.AddWriteInfo (mat.descriptorSet, dslb, textures[(int)mat.occlusionTexture].Descriptor);
							break;
						case ShaderBinding.MetalRoughness:
							if (mat.pbrDatas.availableAttachments.HasFlag (ShaderBinding.MetalRoughness))
								uboUpdate.AddWriteInfo (mat.descriptorSet, dslb, textures[(int)mat.metallicRoughnessTexture].Descriptor);
							break;
						case ShaderBinding.Metal:
							break;
						case ShaderBinding.Roughness:
							break;
						case ShaderBinding.Emissive:
							if (mat.pbrDatas.availableAttachments.HasFlag (ShaderBinding.Emissive))
								uboUpdate.AddWriteInfo (mat.descriptorSet, dslb, textures[(int)mat.emissiveTexture].Descriptor);
							break;
					}
				}
				uboUpdate.Update ();
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
			Matrix4x4 localMat = currentTransform * node.matrix;

			cmd.PushConstant (pipelineLayout, VkShaderStageFlags.Vertex, localMat);

			if (node.Mesh != null) {
				foreach (Primitive p in node.Mesh.Primitives) {
					Material mat = materials[(int)p.material];
					cmd.PushConstant (pipelineLayout, VkShaderStageFlags.Fragment, mat.pbrDatas, (uint)Marshal.SizeOf<Matrix4x4>());
					if (mat.descriptorSet != null)
						cmd.BindDescriptorSet (pipelineLayout, mat.descriptorSet, 1);
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
					RenderNode (cmd, pipelineLayout, node, sc.Root.matrix);
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
					descriptorPool?.Dispose ();
					foreach (Image txt in textures) {
						txt.Dispose ();
					}
					//uboMaterials?.Dispose ();
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
