////
//// Model.cs
////
//// Author:
////       Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
////
//// Copyright (c) 2019 jp
////
//// Permission is hereby granted, free of charge, to any person obtaining a copy
//// of this software and associated documentation files (the "Software"), to deal
//// in the Software without restriction, including without limitation the rights
//// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//// copies of the Software, and to permit persons to whom the Software is
//// furnished to do so, subject to the following conditions:
////
//// The above copyright notice and this permission notice shall be included in
//// all copies or substantial portions of the Software.
////
//// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//// THE SOFTWARE.
//using System;
//using System.Diagnostics;
//using System.Numerics;
//using System.Runtime.InteropServices;

//using VK;
//using System.Collections.Generic;

//namespace CVKL {

//	public class PbrModel2 : Model {
//        public Image textures;

//		DescriptorPool descriptorPool;
//        public GPUBuffer vbo;
//        public GPUBuffer ibo;

//		/// <summary>
//		/// Pbr data structure suitable for push constant, containing
//		/// availablility of attached textures and the coef of pbr inputs
//		/// </summary>
//		public struct PbrPushConstStruct {
//			public Vector4 baseColorFactor;
//			public Vector4 emissiveFactor;
//			public VK.AttachmentType availableAttachments;
//			public AlphaMode alphaMode;
//			public float alphaCutoff;
//			public float metallicFactor;
//			public float roughnessFactor;

//			public PbrPushConstStruct (float metallicFactor = 1.0f, float roughnessFactor = 1.0f,
//				AlphaMode alphaMode = AlphaMode.Opaque, float alphaCutoff = 0.5f,
//				VK.AttachmentType availableAttachments = VK.AttachmentType.None)
//			{
//				this.baseColorFactor = Vector4.One;
//				this.emissiveFactor = Vector4.One;
//				this.availableAttachments = availableAttachments;
//				this.alphaMode = alphaMode;
//				this.alphaCutoff = alphaCutoff;
//				this.metallicFactor = metallicFactor;
//				this.roughnessFactor = roughnessFactor;
//			}
//			public PbrPushConstStruct (Vector4 baseColorFactor,
//				Vector4 emissiveFactor,
//				float metallicFactor = 1.0f, float roughnessFactor = 1.0f,
//				AlphaMode alphaMode = AlphaMode.Opaque, float alphaCutoff = 0.5f,
//				VK.AttachmentType availableAttachments = VK.AttachmentType.None)
//			{
//				this.baseColorFactor = baseColorFactor;
//				this.emissiveFactor = emissiveFactor;
//				this.availableAttachments = availableAttachments;
//				this.alphaMode = alphaMode;
//				this.alphaCutoff = alphaCutoff;
//				this.metallicFactor = metallicFactor;
//				this.roughnessFactor = roughnessFactor;			
//			}
//		}

//		/// <summary>
//		/// Material class with textures indices and a descriptorSet for those textures
//		/// </summary>
//		public class Material2 : Model.Material{
//			public PbrPushConstStruct pbrDatas;
//			public DescriptorSet descriptorSet;

//            public Material2 (UInt32 _baseColorTexture = 0, UInt32 _metallicRoughnessTexture = 0,
//            	UInt32 _normalTexture = 0, UInt32 _occlusionTexture = 0) : base(_baseColorTexture, _metallicRoughnessTexture, _normalTexture, _occlusionTexture) {
//            }            
//        }        


//		public PbrModel2 (Queue transferQ, string path) {
//            dev = transferQ.dev;
//			using (CommandPool cmdPool = new CommandPool (dev, transferQ.index)) {
//				using (glTFLoader ctx = new glTFLoader (path, transferQ, cmdPool)) {
//					ulong vertexCount, indexCount;

//					ctx.GetVertexCount (out vertexCount, out indexCount);
//					ulong vertSize = vertexCount * (ulong)Marshal.SizeOf<Vertex> ();
//					ulong idxSize = indexCount * (IndexBufferType == VkIndexType.Uint16 ? 2ul : 4ul);
//					ulong size = vertSize + idxSize;

//					vbo = new GPUBuffer (dev, VkBufferUsageFlags.VertexBuffer | VkBufferUsageFlags.TransferDst, vertSize);
//					ibo = new GPUBuffer (dev, VkBufferUsageFlags.IndexBuffer | VkBufferUsageFlags.TransferDst, idxSize);

//					vbo.SetName ("vbo gltf");
//					ibo.SetName ("ibo gltf");

//					Meshes = new List<Mesh> (ctx.LoadMeshes<Vertex> (VkIndexType.Uint16, vbo, 0, ibo, 0));
//					textures = new List<Image> (ctx.LoadImages ());
//					materials = new List<Material> (ctx.LoadMaterial ());
//					Scenes = new List<Scene> (ctx.LoadScenes (out DefaultScene));
//				}
//			}
//		}

//		/// <summary>
//		/// Setup descriptorSets for all materials include in model
//		/// Depending on pipeline configuration, one or more attachment may be defined
//		/// </summary>
//		public void WriteMaterialsDescriptorSets (DescriptorSetLayout layout, params VK.AttachmentType[] attachments) {
//			if (attachments.Length == 0)
//				throw new InvalidOperationException ("At least one attachment is required for Model.WriteMaterialDescriptor");

//			descriptorPool = new DescriptorPool (dev, (uint)materials.Count,
//				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler, (uint)(attachments.Length * materials.Count))
//			);
//			descriptorPool.SetName ("descPool gltfTextures");
//			foreach (Material2 mat in materials) 
//				WriteMaterialDescriptorSet (mat, layout, attachments);
//		}

//		/// <summary>
//		/// Setup one descriptorSet for a single material
//		/// </summary>
//		/// <param name="mat">Material</param>
//		/// <param name="layout">Descriptor Layout for texture</param>
//		/// <param name="attachments">Layout Attachments meaning</param>
//		public void WriteMaterialDescriptorSet (Material2 mat, DescriptorSetLayout layout, params VK.AttachmentType[] attachments) {
//			mat.descriptorSet = descriptorPool.Allocate (layout);
//			mat.descriptorSet.handle.SetDebugMarkerName (dev, "descSet " + mat.Name);
//			WriteMaterialDescriptorSet (mat, attachments);
//		}

//		/// <summary>
//		/// Update Writes already allocated material descriptor set.
//		/// </summary>
//		/// <param name="mat">Material</param>
//		/// <param name="attachments">Layout Attachments meaning</param>
//		public void WriteMaterialDescriptorSet (Material2 mat, params VK.AttachmentType[] attachments) {
//			VkDescriptorSetLayoutBinding dslb =
//				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler);

//			using (DescriptorSetWrites2 uboUpdate = new DescriptorSetWrites2 (dev)) {
//				for (uint i = 0; i < attachments.Length; i++) {
//					dslb.binding = i;
//					switch (attachments[i]) {
//						case VK.AttachmentType.None:
//							break;
//						case VK.AttachmentType.Color:
//							if (mat.pbrDatas.availableAttachments.HasFlag (VK.AttachmentType.Color))
//								uboUpdate.AddWriteInfo (mat.descriptorSet, dslb, textures[(int)mat.baseColorTexture].Descriptor);
//							break;
//						case VK.AttachmentType.Normal:
//							if (mat.pbrDatas.availableAttachments.HasFlag (VK.AttachmentType.Normal))
//								uboUpdate.AddWriteInfo (mat.descriptorSet, dslb, textures[(int)mat.normalTexture].Descriptor);
//							break;
//						case VK.AttachmentType.AmbientOcclusion:
//							if (mat.pbrDatas.availableAttachments.HasFlag (VK.AttachmentType.AmbientOcclusion))
//								uboUpdate.AddWriteInfo (mat.descriptorSet, dslb, textures[(int)mat.occlusionTexture].Descriptor);
//							break;
//						case VK.AttachmentType.MetalRoughness:
//							if (mat.pbrDatas.availableAttachments.HasFlag (VK.AttachmentType.MetalRoughness))
//								uboUpdate.AddWriteInfo (mat.descriptorSet, dslb, textures[(int)mat.metallicRoughnessTexture].Descriptor);
//							break;
//						case VK.AttachmentType.Metal:
//							break;
//						case VK.AttachmentType.Roughness:
//							break;
//						case VK.AttachmentType.Emissive:
//							if (mat.pbrDatas.availableAttachments.HasFlag (VK.AttachmentType.Emissive))
//								uboUpdate.AddWriteInfo (mat.descriptorSet, dslb, textures[(int)mat.emissiveTexture].Descriptor);
//							break;
//					}
//				}
//				uboUpdate.Update ();
//			}
//		}

//		/// <summary> bind vertex and index buffers </summary>
//		public void Bind (CommandBuffer cmd) {
//			cmd.BindVertexBuffer (vbo);
//			cmd.BindIndexBuffer (ibo, IndexBufferType);
//		}

//		//TODO:destset for binding must be variable
//		//TODO: ADD REFAULT MAT IF NO MAT DEFINED
//		public void RenderNode (CommandBuffer cmd, PipelineLayout pipelineLayout, Node node, Matrix4x4 currentTransform) {
//			Matrix4x4 localMat = currentTransform * node.matrix;

//			cmd.PushConstant (pipelineLayout, VkShaderStageFlags.Vertex, localMat);

//			if (node.Mesh != null) {
//				foreach (Primitive p in node.Mesh.Primitives) {
//					Material2 mat = materials[(int)p.material];
//					cmd.PushConstant (pipelineLayout, VkShaderStageFlags.Fragment, mat.pbrDatas, (uint)Marshal.SizeOf<Matrix4x4>());
//					if (mat.descriptorSet != null)
//						cmd.BindDescriptorSet (pipelineLayout, mat.descriptorSet, 1);
//					cmd.DrawIndexed (p.indexCount, 1, p.indexBase, p.vertexBase, 0);
//				}
//			}
//			if (node.Children == null)
//				return;
//			foreach (Node child in node.Children) 
//				RenderNode (cmd, pipelineLayout, child, localMat);
//		}

//		public void DrawAll (CommandBuffer cmd, PipelineLayout pipelineLayout) {
//			foreach (Scene sc in Scenes) {
//				foreach (Node node in sc.Root.Children) {
//					RenderNode (cmd, pipelineLayout, node, sc.Root.matrix);
//				}
//			}
//		}

//		#region IDisposable Support
//		private bool isDisposed = false; // Pour détecter les appels redondants

//		protected virtual void Dispose (bool disposing) {
//			if (!isDisposed) {
//				if (disposing) {
//					ibo?.Dispose ();
//					vbo?.Dispose ();
//					descriptorPool?.Dispose ();
//					foreach (Image txt in textures) {
//						txt.Dispose ();
//					}
//					//uboMaterials?.Dispose ();
//				} else
//					Debug.WriteLine ("model was not disposed");
//				isDisposed = true;
//			}
//		}
//		public void Dispose () {
//			Dispose (true);
//		}
//		#endregion
//	}
//}
