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

using VK;
using System.Collections.Generic;

namespace CVKL {
	using static VK.Utils;
    
	//TODO:stride in buffer views?
	public class PbrModel : Model {        
		/// <summary>
		/// Pbr data structure suitable for push constant, containing
		/// availablility of attached textures and the coef of pbr inputs
		/// </summary>
		public struct PbrMaterial {
			public Vector4 baseColorFactor;
			public Vector4 emissiveFactor;
			public AttachmentType availableAttachments;
			public AlphaMode alphaMode;
			public float alphaCutoff;
			public float metallicFactor;
			public float roughnessFactor;

			public PbrMaterial (float metallicFactor = 1.0f, float roughnessFactor = 1.0f,
				AlphaMode alphaMode = AlphaMode.Opaque, float alphaCutoff = 0.5f,
				AttachmentType availableAttachments = AttachmentType.None)
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
				AlphaMode alphaMode = AlphaMode.Opaque, float alphaCutoff = 0.5f,
				AttachmentType availableAttachments = AttachmentType.None)
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

		Image[] textures;
		PbrMaterial[] materials;
		DescriptorSet[] descriptorSets;

		DescriptorPool descriptorPool;
		public GPUBuffer vbo;
		public GPUBuffer ibo;


		public PbrModel (Queue transferQ, string path, DescriptorSetLayout layout, params AttachmentType[] attachments) {
            dev = transferQ.dev;
			using (CommandPool cmdPool = new CommandPool (dev, transferQ.index)) {
				using (glTFLoader ctx = new glTFLoader (path, transferQ, cmdPool)) {
					ulong vertexCount, indexCount;

					ctx.GetVertexCount (out vertexCount, out indexCount);
					ulong vertSize = vertexCount * (ulong)Marshal.SizeOf<Vertex> ();
					ulong idxSize = indexCount * (IndexBufferType == VkIndexType.Uint16 ? 2ul : 4ul);
					ulong size = vertSize + idxSize;

					vbo = new GPUBuffer (dev, VkBufferUsageFlags.VertexBuffer | VkBufferUsageFlags.TransferDst, vertSize);
					ibo = new GPUBuffer (dev, VkBufferUsageFlags.IndexBuffer | VkBufferUsageFlags.TransferDst, idxSize);

					vbo.SetName ("vbo gltf");
					ibo.SetName ("ibo gltf");

					Meshes = new List<Mesh> (ctx.LoadMeshes<Vertex> (VkIndexType.Uint16, vbo, 0, ibo, 0));
					textures = ctx.LoadImages ();

					loadMaterials (ctx, layout, attachments);

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
				materials[i] = new PbrMaterial (mats[i].baseColorFactor, mats[i].emissiveFactor, mats[i].metallicFactor, mats[i].roughnessFactor,
					mats[i].alphaMode, mats[i].alphaCutoff, mats[i].availableAttachments);
				descriptorSets[i] = descriptorPool.Allocate (layout);
				descriptorSets[i].handle.SetDebugMarkerName (dev, "descSet " + mats[i].Name);

				/*materials[i].availableAttachments &= ~AttachmentType.Emissive;
				materials[i].availableAttachments &= ~AttachmentType.AmbientOcclusion;
				materials[i].availableAttachments &= ~AttachmentType.Normal;*/

				//materials[i].metallicFactor = 0.9f;
				//materials[i].roughnessFactor = 0.2f;

				VkDescriptorSetLayoutBinding dslb =
					new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler);

				using (DescriptorSetWrites2 uboUpdate = new DescriptorSetWrites2 (dev)) {
					for (uint a = 0; a < attachments.Length; a++) {
						dslb.binding = a;
						switch (attachments[a]) {
							case AttachmentType.None:
								break;
							case AttachmentType.Color:
								if (materials[i].availableAttachments.HasFlag (AttachmentType.Color))
									uboUpdate.AddWriteInfo (descriptorSets[i], dslb, textures[(int)mats[i].baseColorTexture].Descriptor);
								break;
							case AttachmentType.Normal:
								if (materials[i].availableAttachments.HasFlag (AttachmentType.Normal))
									uboUpdate.AddWriteInfo (descriptorSets[i], dslb, textures[(int)mats[i].normalTexture].Descriptor);
								break;
							case AttachmentType.AmbientOcclusion:
								if (materials[i].availableAttachments.HasFlag (AttachmentType.AmbientOcclusion))
									uboUpdate.AddWriteInfo (descriptorSets[i], dslb, textures[(int)mats[i].occlusionTexture].Descriptor);
								break;
							case AttachmentType.MetalRoughness:
								if (materials[i].availableAttachments.HasFlag (AttachmentType.MetalRoughness))
									uboUpdate.AddWriteInfo (descriptorSets[i], dslb, textures[(int)mats[i].metallicRoughnessTexture].Descriptor);
								break;
							case AttachmentType.Metal:
								break;
							case AttachmentType.Roughness:
								break;
							case AttachmentType.Emissive:
								if (materials[i].availableAttachments.HasFlag (AttachmentType.Emissive))
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
					cmd.PushConstant (pipelineLayout, VkShaderStageFlags.Fragment, materials[(int)p.material], (uint)Marshal.SizeOf<Matrix4x4>());
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
