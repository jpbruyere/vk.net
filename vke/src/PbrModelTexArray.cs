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
	public class PbrModelTexArray : PbrModel {
		public static uint TEXTURE_DIM = 512;
		/// <summary>
		/// Pbr data structure suitable for push constant or ubo, containing
		/// availablility of attached textures and the coef of pbr inputs
		/// </summary>
		public struct Material {
			public Vector4 baseColorFactor;
			public Vector4 emissiveFactor;
			public Vector4 diffuseFactor;
			public Vector4 specularFactor;

			public float workflow;
			public AttachmentType TexCoord0;
			public AttachmentType TexCoord1;
			public int baseColorTextureSet;

			public int physicalDescriptorTextureSet;
			public int normalTextureSet;
			public int occlusionTextureSet;
			public int emissiveTextureSet;

			public float metallicFactor;
			public float roughnessFactor;
			public float alphaMask;
			public float alphaMaskCutoff;
		}

		public Image texArray;
		public Material[] materials;

		public PbrModelTexArray (Queue transferQ, string path) {
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

					if (ctx.ImageCount > 0) {
						texArray = new Image (dev, Image.DefaultTextureFormat, VkImageUsageFlags.Sampled | VkImageUsageFlags.TransferDst | VkImageUsageFlags.TransferSrc,
							VkMemoryPropertyFlags.DeviceLocal, TEXTURE_DIM, TEXTURE_DIM, VkImageType.Image2D,
							VkSampleCountFlags.SampleCount1, VkImageTiling.Optimal, Image.ComputeMipLevels (TEXTURE_DIM), ctx.ImageCount);

						ctx.BuildTexArray (ref texArray, 0);

						texArray.CreateView (VkImageViewType.ImageView2DArray, VkImageAspectFlags.Color, texArray.CreateInfo.arrayLayers);
						texArray.CreateSampler ();
						texArray.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
						texArray.SetName ("model texArray");
					}

					loadMaterials (ctx);

					materialUBO = new HostBuffer<Material> (dev, VkBufferUsageFlags.UniformBuffer, materials);

					Scenes = new List<Scene> (ctx.LoadScenes (out defaultSceneIndex));
				}
			}
		}

		void loadMaterials (glTFLoader ctx) {
			Model.Material[] mats = ctx.LoadMaterial ();
			materials = new Material[mats.Length];

			for (int i = 0; i < mats.Length; i++) {
				materials[i] = new Material {
					workflow = (float)mats[i].workflow,
					baseColorFactor = mats[i].baseColorFactor,
					emissiveFactor = mats[i].emissiveFactor,
					metallicFactor = mats[i].metallicFactor,
					roughnessFactor = mats[i].roughnessFactor,

					baseColorTextureSet = mats[i].baseColorTexture,
					physicalDescriptorTextureSet = mats[i].metallicRoughnessTexture,
					normalTextureSet = mats[i].normalTexture,
					occlusionTextureSet = mats[i].occlusionTexture,
					emissiveTextureSet = mats[i].emissiveTexture,

					TexCoord0 = mats[i].availableAttachments,
					TexCoord1 = mats[i].availableAttachments1,

					alphaMask = 0f,
					alphaMaskCutoff = 0.0f,
					diffuseFactor = new Vector4 (0),
					specularFactor = new Vector4 (0)
				};
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
					texArray?.Dispose ();						
				} else
					Debug.WriteLine ("model was not disposed");
			}
			base.Dispose (disposing);
		}
	}
}
