//
// PbrModel.cs
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

	/// <summary>
	/// Indexed pbr model whith one descriptorSet per material with separate textures attachments
	/// </summary>
	public class PbrModelSeparatedTextures : PbrModel {
		/// <summary>
		/// Pbr data structure suitable for push constant or ubo, containing
		/// availablility of attached textures and the coef of pbr inputs
		/// </summary>
		public new struct Material {
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
		public Material[] materials;
		/// <summary>
		/// one descriptor per material containing textures
		/// </summary>
		DescriptorSet[] descriptorSets;

		public PbrModelSeparatedTextures (Queue transferQ, string path, DescriptorSetLayout layout, params AttachmentType[] attachments) {
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

					materialUBO = new HostBuffer<Material> (dev, VkBufferUsageFlags.UniformBuffer, materials);

					Scenes = new List<Scene> (ctx.LoadScenes (out defaultSceneIndex));
				}
			}
		}

		void loadMaterials (glTFLoader ctx, DescriptorSetLayout layout, params AttachmentType[] attachments) {
			Model.Material[] mats = ctx.LoadMaterial ();
			materials = new Material[mats.Length];
			descriptorSets = new DescriptorSet[mats.Length];

			if (attachments.Length == 0)
				throw new InvalidOperationException ("At least one attachment is required for Model.WriteMaterialDescriptor");

			descriptorPool = new DescriptorPool (dev, (uint)materials.Length,
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler, (uint)(attachments.Length * materials.Length))
			);
			descriptorPool.SetName ("descPool gltfTextures");

			for (int i = 0; i < mats.Length; i++) {
				materials[i] = new Material {
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

		public override void RenderNode (CommandBuffer cmd, PipelineLayout pipelineLayout, Node node, Matrix4x4 currentTransform, bool shadowPass = false) {
			Matrix4x4 localMat = node.localMatrix * currentTransform;
			VkShaderStageFlags matStage = shadowPass ? VkShaderStageFlags.Geometry : VkShaderStageFlags.Vertex;
			cmd.PushConstant (pipelineLayout, matStage, localMat);

			if (node.Mesh != null) {
				foreach (Primitive p in node.Mesh.Primitives) {
					if (!shadowPass) {
						cmd.PushConstant (pipelineLayout, VkShaderStageFlags.Fragment, (int)p.material, (uint)Marshal.SizeOf<Matrix4x4> ());
						if (descriptorSets[p.material] != null)
							cmd.BindDescriptorSet (pipelineLayout, descriptorSets[p.material], 2);
					}
					cmd.DrawIndexed (p.indexCount, 1, p.indexBase, p.vertexBase, 0);
				}
			}
			if (node.Children == null)
				return;
			foreach (Node child in node.Children)
				RenderNode (cmd, pipelineLayout, child, localMat, shadowPass);
		}

		protected override void Dispose (bool disposing) {
			if (!isDisposed) {
				if (disposing) {
					foreach (Image txt in textures) 
						txt.Dispose ();
				} else
					Debug.WriteLine ("model was not disposed");
			}
			base.Dispose (disposing);
		}
	}
}
