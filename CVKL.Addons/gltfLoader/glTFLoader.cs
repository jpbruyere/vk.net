// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using glTFLoader;
using GL = glTFLoader.Schema;

using VK;
using System.Collections.Generic;
using System.IO;



namespace CVKL.glTF {
	using static VK.Utils;
	using static CVKL.Model;

	/// <summary>
	/// Loading context with I as the vertex index type (uint16,uint32)
	/// </summary>
	public class glTFLoader : IDisposable {
		/// <summary>
		/// Material class used by the gltfLoader to fetch values.
		/// </summary>
		public class Material {
			public enum Workflow { PhysicalyBaseRendering = 1, SpecularGlossinnes };
			public string Name;
			public Workflow workflow;
			public Int32 baseColorTexture;
			public Int32 metallicRoughnessTexture;
			public Int32 normalTexture;
			public Int32 occlusionTexture;
			public Int32 emissiveTexture;

			public Vector4 baseColorFactor;
			public Vector4 emissiveFactor;
			public AttachmentType availableAttachments;
			public AttachmentType availableAttachments1;

			public AlphaMode alphaMode;
			public float alphaCutoff;
			public float metallicFactor;
			public float roughnessFactor;

			public bool metallicRoughness = true;
			public bool specularGlossiness = false;

			public Material (Int32 _baseColorTexture = -1, Int32 _metallicRoughnessTexture = -1,
				Int32 _normalTexture = -1, Int32 _occlusionTexture = -1) {
				workflow = Workflow.PhysicalyBaseRendering;
				baseColorTexture = _baseColorTexture;
				metallicRoughnessTexture = _metallicRoughnessTexture;
				normalTexture = _normalTexture;
				occlusionTexture = _occlusionTexture;
				emissiveTexture = -1;

				alphaMode = AlphaMode.Opaque;
				alphaCutoff = 1f;
				metallicFactor = 1f;
				roughnessFactor = 1;
				baseColorFactor = new Vector4 (1);
				emissiveFactor = new Vector4 (1);

				metallicRoughness = true;
				specularGlossiness = false;

			}
		}

		public Queue transferQ;
		public CommandPool cmdPool;
		Device dev => transferQ.Dev;

		public GL.Gltf gltf;
		public string baseDirectory;

		public byte[][] loadedBuffers;
		public GCHandle[] bufferHandles;

		List<Mesh> meshes;
		string path;

		public glTFLoader (string path, Queue _transferQ, CommandPool _cmdPool) {
			this.path = path;
			transferQ = _transferQ;
			cmdPool = _cmdPool;
			baseDirectory = System.IO.Path.GetDirectoryName (path);
			gltf = Interface.LoadModel (path); ;
			loadedBuffers = new byte[gltf.Buffers.Length][];
			bufferHandles = new GCHandle[gltf.Buffers.Length];
		}

		static byte[] loadDataUri (GL.Image img) {
			int idxComa = img.Uri.IndexOf (",", 5, StringComparison.Ordinal);
			return Convert.FromBase64String (img.Uri.Substring (idxComa + 1));
		}
		static byte[] loadDataUri (GL.Buffer buff) {
			int idxComa = buff.Uri.IndexOf (",", 5, StringComparison.Ordinal);
			return Convert.FromBase64String (buff.Uri.Substring (idxComa + 1));
		}

		void EnsureBufferIsLoaded (int bufferIdx) {
			if (loadedBuffers[bufferIdx] == null) {
				//load full buffer
				string uri = gltf.Buffers[bufferIdx].Uri;
				if (string.IsNullOrEmpty(uri))//glb
					loadedBuffers[bufferIdx] = gltf.LoadBinaryBuffer (bufferIdx, path);
				else if (uri.StartsWith ("data", StringComparison.Ordinal))
					loadedBuffers[bufferIdx] = loadDataUri (gltf.Buffers[bufferIdx]);//TODO:check this func=>System.Buffers.Text.Base64.EncodeToUtf8InPlace
				else
					loadedBuffers[bufferIdx] = File.ReadAllBytes (Path.Combine (baseDirectory, gltf.Buffers[bufferIdx].Uri));
				bufferHandles[bufferIdx] = GCHandle.Alloc (loadedBuffers[bufferIdx], GCHandleType.Pinned);
			}
		}

		public void GetVertexCount (out ulong vertexCount, out ulong indexCount, out VkIndexType largestIndexType) {
			vertexCount = 0;
			indexCount = 0;
			largestIndexType = VkIndexType.Uint16;
			//compute size of stagging buf
			foreach (GL.Mesh mesh in gltf.Meshes) {
				foreach (GL.MeshPrimitive p in mesh.Primitives) {
					int accessorIdx;
					if (p.Attributes.TryGetValue ("POSITION", out accessorIdx))
						vertexCount += (ulong)gltf.Accessors[accessorIdx].Count;
					if (p.Indices != null) {
						indexCount += (ulong)gltf.Accessors[(int)p.Indices].Count;
						if (gltf.Accessors[(int)p.Indices].ComponentType == GL.Accessor.ComponentTypeEnum.UNSIGNED_INT)
							largestIndexType = VkIndexType.Uint32;
					}
				}
			}
		}

		public uint ImageCount => gltf.Images == null ? 0 : (uint)gltf.Images.Length;
		

		//TODO: some buffer data are reused between primitives, and I duplicate the datas
		//buffers must be constructed without duplications
		public Mesh[] LoadMeshes<TVertex> (VkIndexType indexType, Buffer vbo, ulong vboOffset, Buffer ibo, ulong iboOffset) {
			ulong vCount, iCount;
			VkIndexType idxType;
			GetVertexCount (out vCount, out iCount, out idxType);

			int vertexByteSize = Marshal.SizeOf<TVertex> ();
			ulong vertSize = vCount * (ulong)vertexByteSize;
			ulong idxSize = iCount * (indexType == VkIndexType.Uint16 ? 2ul : 4ul);
			ulong size = vertSize + idxSize;

			int vertexCount = 0, indexCount = 0;
			int autoNamedMesh = 1;

			meshes = new List<Mesh> ();

			using (HostBuffer stagging = new HostBuffer (dev, VkBufferUsageFlags.TransferSrc, size)) {
				stagging.Map ();

				unsafe {
					byte* stagVertPtrInit = (byte*)stagging.MappedData.ToPointer ();
					byte* stagIdxPtrInit = (byte*)(stagging.MappedData.ToPointer ()) + vertSize;
					byte* stagVertPtr = stagVertPtrInit;
					byte* stagIdxPtr = stagIdxPtrInit;

					foreach (GL.Mesh mesh in gltf.Meshes) {

						string meshName = mesh.Name;
						if (string.IsNullOrEmpty (meshName)) {
							meshName = "mesh_" + autoNamedMesh.ToString ();
							autoNamedMesh++;
						}
						Mesh m = new Mesh { Name = meshName };

						foreach (GL.MeshPrimitive p in mesh.Primitives) {
							GL.Accessor AccPos = null, AccNorm = null, AccUv = null, AccUv1 = null;

							int accessorIdx;
							if (p.Attributes.TryGetValue ("POSITION", out accessorIdx)) {
								AccPos = gltf.Accessors[accessorIdx];
								EnsureBufferIsLoaded (gltf.BufferViews[(int)AccPos.BufferView].Buffer);
							}
							if (p.Attributes.TryGetValue ("NORMAL", out accessorIdx)) {
								AccNorm = gltf.Accessors[accessorIdx];
								EnsureBufferIsLoaded (gltf.BufferViews[(int)AccNorm.BufferView].Buffer);
							}
							if (p.Attributes.TryGetValue ("TEXCOORD_0", out accessorIdx)) {
								AccUv = gltf.Accessors[accessorIdx];
								EnsureBufferIsLoaded (gltf.BufferViews[(int)AccUv.BufferView].Buffer);
							}
							if (p.Attributes.TryGetValue ("TEXCOORD_1", out accessorIdx)) {
								AccUv1 = gltf.Accessors[accessorIdx];
								EnsureBufferIsLoaded (gltf.BufferViews[(int)AccUv1.BufferView].Buffer);
							}

							Primitive prim = new Primitive {
								indexBase = (uint)indexCount,
								vertexBase = vertexCount,
								vertexCount = (uint)AccPos.Count,
								material = (uint)(p.Material ?? 0)
							};

							prim.bb.min.ImportFloatArray (AccPos.Min);
							prim.bb.max.ImportFloatArray (AccPos.Max);
							prim.bb.isValid = true;

							//Interleaving vertices
							byte * inPosPtr = null, inNormPtr = null, inUvPtr = null, inUv1Ptr = null;

							GL.BufferView bv = gltf.BufferViews[(int)AccPos.BufferView];
							inPosPtr = (byte*)bufferHandles[bv.Buffer].AddrOfPinnedObject ().ToPointer ();
							inPosPtr += AccPos.ByteOffset + bv.ByteOffset;

							if (AccNorm != null) {
								bv = gltf.BufferViews[(int)AccNorm.BufferView];
								inNormPtr = (byte*)bufferHandles[bv.Buffer].AddrOfPinnedObject ().ToPointer ();
								inNormPtr += AccNorm.ByteOffset + bv.ByteOffset;
							}
							if (AccUv != null) {
								bv = gltf.BufferViews[(int)AccUv.BufferView];
								inUvPtr = (byte*)bufferHandles[bv.Buffer].AddrOfPinnedObject ().ToPointer ();
								inUvPtr += AccUv.ByteOffset + bv.ByteOffset;
							}
							if (AccUv1 != null) {
								bv = gltf.BufferViews[(int)AccUv1.BufferView];
								inUv1Ptr = (byte*)bufferHandles[bv.Buffer].AddrOfPinnedObject ().ToPointer ();
								inUv1Ptr += AccUv1.ByteOffset + bv.ByteOffset;
							}


							for (int j = 0; j < prim.vertexCount; j++) {
								System.Buffer.MemoryCopy (inPosPtr, stagVertPtr, 12, 12);
								inPosPtr += 12;
								if (inNormPtr != null) {
									System.Buffer.MemoryCopy (inNormPtr, stagVertPtr + 12, 12, 12);
									inNormPtr += 12;
								}
								if (inUvPtr != null) {
									System.Buffer.MemoryCopy (inUvPtr, stagVertPtr + 24, 8, 8);
									inUvPtr += 8;
								}
								if (inUv1Ptr != null) {
									System.Buffer.MemoryCopy (inUv1Ptr, stagVertPtr + 32, 8, 8);
									inUv1Ptr += 8;
								}
								stagVertPtr += vertexByteSize;
							}

							//indices loading
							if (p.Indices != null) {
								GL.Accessor acc = gltf.Accessors[(int)p.Indices];
								bv = gltf.BufferViews[(int)acc.BufferView];

								byte* inIdxPtr = (byte*)bufferHandles[bv.Buffer].AddrOfPinnedObject ().ToPointer ();
								inIdxPtr += acc.ByteOffset + bv.ByteOffset;

								//TODO:double check this, I dont seems to increment stag pointer
								if (acc.ComponentType == GL.Accessor.ComponentTypeEnum.UNSIGNED_SHORT) {
									if (indexType == VkIndexType.Uint16) {
										System.Buffer.MemoryCopy (inIdxPtr, stagIdxPtr, (long)acc.Count * 2, (long)acc.Count * 2);
										stagIdxPtr += (long)acc.Count * 2;
									} else {
										uint* usPtr = (uint*)stagIdxPtr;
										ushort* inPtr = (ushort*)inIdxPtr;
										for (int i = 0; i < acc.Count; i++) 
											usPtr[i] = inPtr[i];										
										stagIdxPtr += (long)acc.Count * 4;
									}
								} else if (acc.ComponentType == GL.Accessor.ComponentTypeEnum.UNSIGNED_INT) {
									if (indexType == VkIndexType.Uint32) {
										System.Buffer.MemoryCopy (inIdxPtr, stagIdxPtr, (long)acc.Count * 4, (long)acc.Count * 4);
										stagIdxPtr += (long)acc.Count * 4;
									} else {
										ushort* usPtr = (ushort*)stagIdxPtr;
										uint* inPtr = (uint*)inIdxPtr;
										for (int i = 0; i < acc.Count; i++) 
											usPtr[i] = (ushort)inPtr[i];
										stagIdxPtr += (long)acc.Count * 2;
									}
								} else if (acc.ComponentType == GL.Accessor.ComponentTypeEnum.UNSIGNED_BYTE) {
									//convert
									if (indexType == VkIndexType.Uint16) {
										ushort* usPtr = (ushort*)stagIdxPtr;
										for (int i = 0; i < acc.Count; i++)
											usPtr[i] = (ushort)inIdxPtr[i];
										stagIdxPtr += (long)acc.Count * 2;
									} else {
										uint* usPtr = (uint*)stagIdxPtr;
										for (int i = 0; i < acc.Count; i++)
											usPtr[i] = (uint)inIdxPtr[i];
										stagIdxPtr += (long)acc.Count * 4;
									}
								} else
									throw new NotImplementedException ();

								prim.indexCount = (uint)acc.Count;
								indexCount += acc.Count;
							}

							m.AddPrimitive (prim);

							vertexCount += AccPos.Count;
						}
						meshes.Add (m);
					}
				}

				stagging.Unmap ();

				CommandBuffer cmd = cmdPool.AllocateCommandBuffer ();
				cmd.Start (VkCommandBufferUsageFlags.OneTimeSubmit);

				stagging.CopyTo (cmd, vbo, vertSize, 0, vboOffset);
				if (iCount>0)
					stagging.CopyTo (cmd, ibo, idxSize, vertSize, iboOffset);

				cmd.End ();

				transferQ.Submit (cmd);

				dev.WaitIdle ();
				cmd.Free ();

			}

			return meshes.ToArray ();
		}

		public Scene[] LoadScenes (out int defaultScene) {
			defaultScene = -1;
			if (gltf.Scene == null)
				return new Scene[] {};

			List<Scene> scenes = new List<Scene> ();
			defaultScene = (int)gltf.Scene;			
			
			for (int i = 0; i < gltf.Scenes.Length; i++) {
				GL.Scene scene = gltf.Scenes[i];
				Debug.WriteLine ("Loading Scene {0}", scene.Name);

				scenes.Add (new Scene {
					Name = scene.Name,
				});

				if (scene.Nodes.Length == 0)
					continue;

				scenes[i].Root = new Node {
					localMatrix = Matrix4x4.Identity,
					Children = new List<Node> ()
				};

				foreach (int nodeIdx in scene.Nodes)
					loadNode (scenes[i].Root, gltf.Nodes[nodeIdx]);
			}
			return scenes.ToArray ();
		}

		void loadNode (Node parentNode, GL.Node gltfNode) {
			Debug.WriteLine ("Loading node {0}", gltfNode.Name);

			Vector3 translation = new Vector3 ();
			Quaternion rotation = Quaternion.Identity;
			Vector3 scale = new Vector3 (1);
			Matrix4x4 localTransform = Matrix4x4.Identity;

			if (gltfNode.Matrix != null) {
				float[] M = gltfNode.Matrix;
				localTransform = new Matrix4x4 (
					M[0], M[1], M[2], M[3],
					M[4], M[5], M[6], M[7],
					M[8], M[9],M[10],M[11],
				   M[12],M[13],M[14],M[15]);
			}

			if (gltfNode.Translation != null) 
				FromFloatArray (ref translation, gltfNode.Translation);
			if (gltfNode.Translation != null) 
				FromFloatArray (ref rotation, gltfNode.Rotation);			
			if (gltfNode.Translation != null) 
				FromFloatArray (ref scale, gltfNode.Scale);

			localTransform *=
				Matrix4x4.CreateScale (scale) *
				Matrix4x4.CreateFromQuaternion (rotation) *
				Matrix4x4.CreateTranslation (translation);

			//localTransform = Matrix4x4.Identity;

			Node node = new Node {
				localMatrix = localTransform,
				Parent = parentNode,
                Name = gltfNode.Name
			};
			parentNode.Children.Add (node);

			if (gltfNode.Children != null) {
				node.Children = new List<Node> ();
				for (int i = 0; i < gltfNode.Children.Length; i++)
					loadNode (node, gltf.Nodes[gltfNode.Children[i]]);
			}

			if (gltfNode.Mesh != null)
				node.Mesh = meshes[(int)gltfNode.Mesh];
		}

		///// <summary>
		///// build texture array
		///// </summary>
		///// <returns>The images.</returns>
		///// <param name="textureSize">Uniformized Texture size for all images</param>
		public void BuildTexArray (ref Image texArray, uint firstImg = 0) {
			int texDim = (int)texArray.CreateInfo.extent.width;

			CommandBuffer cmd = cmdPool.AllocateAndStart (VkCommandBufferUsageFlags.OneTimeSubmit);
			texArray.SetLayout (cmd, VkImageAspectFlags.Color, VkImageLayout.Undefined, VkImageLayout.TransferDstOptimal, 
						VkPipelineStageFlags.BottomOfPipe, VkPipelineStageFlags.Transfer);
			cmd.End ();
			transferQ.Submit (cmd);
			transferQ.WaitIdle ();
			cmd.Free ();

			VkImageBlit imageBlit = new VkImageBlit {
				srcSubresource = new VkImageSubresourceLayers (VkImageAspectFlags.Color, 1, 0),
				dstOffsets_1 = new VkOffset3D (texDim, texDim, 1)
			};

			for (int l = 0; l < gltf.Images.Length; l++) {
				GL.Image img = gltf.Images[l];
				Image vkimg = null;

				if (img.BufferView != null) {//load image from gltf buffer view
					GL.BufferView bv = gltf.BufferViews[(int)img.BufferView];
					EnsureBufferIsLoaded (bv.Buffer);
					vkimg = Image.Load (dev, bufferHandles[bv.Buffer].AddrOfPinnedObject () + bv.ByteOffset, (ulong)bv.ByteLength, VkImageUsageFlags.TransferSrc);
				} else if (img.Uri.StartsWith ("data:", StringComparison.Ordinal)) {//load base64 encoded image
					Debug.WriteLine ("loading embedded image {0} : {1}", img.Name, img.MimeType);
					vkimg = Image.Load (dev, glTFLoader.loadDataUri (img), VkImageUsageFlags.TransferSrc);
				} else {
					Debug.WriteLine ("loading image {0} : {1} : {2}", img.Name, img.MimeType, img.Uri);//load image from file path in uri
					vkimg = Image.Load (dev, Path.Combine (baseDirectory, img.Uri), VkImageUsageFlags.TransferSrc);
				}

				imageBlit.srcOffsets_1 = new VkOffset3D ((int)vkimg.CreateInfo.extent.width, (int)vkimg.CreateInfo.extent.height, 1);
				imageBlit.dstSubresource = new VkImageSubresourceLayers (VkImageAspectFlags.Color, 1, 0, (uint)l + firstImg);

				cmd = cmdPool.AllocateAndStart (VkCommandBufferUsageFlags.OneTimeSubmit);

				vkimg.SetLayout (cmd, VkImageAspectFlags.Color,
						VkAccessFlags.HostWrite, VkAccessFlags.TransferRead,
						VkImageLayout.Undefined, VkImageLayout.TransferSrcOptimal,
						VkPipelineStageFlags.Host, VkPipelineStageFlags.Transfer);

				Vk.vkCmdBlitImage (cmd.Handle, vkimg.Handle, VkImageLayout.TransferSrcOptimal,
					texArray.Handle, VkImageLayout.TransferDstOptimal, 1, ref imageBlit, VkFilter.Linear);

				cmd.End ();
				transferQ.Submit (cmd);
				transferQ.WaitIdle ();
				cmd.Free ();

				vkimg.Dispose ();
			}

			cmd = cmdPool.AllocateAndStart (VkCommandBufferUsageFlags.OneTimeSubmit);

			uint imgCount = (uint)gltf.Images.Length;
			VkImageSubresourceRange mipSubRange = new VkImageSubresourceRange (VkImageAspectFlags.Color, 0, 1, firstImg, imgCount);

			for (int i = 1; i < texArray.CreateInfo.mipLevels; i++) {
				imageBlit = new VkImageBlit {
					srcSubresource = new VkImageSubresourceLayers (VkImageAspectFlags.Color, imgCount, (uint)i - 1, firstImg),
					srcOffsets_1 = new VkOffset3D ((int)texDim >> (i - 1), (int)texDim >> (i - 1), 1),
					dstSubresource = new VkImageSubresourceLayers (VkImageAspectFlags.Color, imgCount, (uint)i, firstImg),
					dstOffsets_1 = new VkOffset3D ((int)texDim >> i, (int)texDim >> i, 1)
				};

				texArray.SetLayout (cmd,
					VkAccessFlags.TransferWrite, VkAccessFlags.TransferRead,
					VkImageLayout.TransferDstOptimal, VkImageLayout.TransferSrcOptimal, mipSubRange,
					VkPipelineStageFlags.Transfer, VkPipelineStageFlags.Transfer);

				Vk.vkCmdBlitImage (cmd.Handle, texArray.Handle, VkImageLayout.TransferSrcOptimal,
					texArray.Handle, VkImageLayout.TransferDstOptimal, 1, ref imageBlit, VkFilter.Linear);
				texArray.SetLayout (cmd, VkImageLayout.TransferSrcOptimal, VkImageLayout.ShaderReadOnlyOptimal, mipSubRange,
					VkPipelineStageFlags.Transfer, VkPipelineStageFlags.FragmentShader);

				mipSubRange.baseMipLevel = (uint)i;
			}
			mipSubRange.baseMipLevel = texArray.CreateInfo.mipLevels - 1;
			texArray.SetLayout (cmd, VkImageLayout.TransferDstOptimal, VkImageLayout.ShaderReadOnlyOptimal, mipSubRange,
				VkPipelineStageFlags.Transfer, VkPipelineStageFlags.FragmentShader);

			cmd.End ();
			transferQ.Submit (cmd);
			transferQ.WaitIdle ();
			cmd.Free ();
		}
		/// <summary>
		/// Load model images as separate texture in a c# array
		/// </summary>
		/// <returns>The images.</returns>
		public Image[] LoadImages () {
			if (gltf.Images == null)
				return new Image[] {};

			List<Image>	textures = new List<Image> ();

			foreach (GL.Image img in gltf.Images) {
				Image vkimg = null;

				string imgName = img.Name;

				if (img.BufferView != null) {//load image from gltf buffer view
					GL.BufferView bv = gltf.BufferViews[(int)img.BufferView];
					EnsureBufferIsLoaded (bv.Buffer);
					vkimg = Image.Load (dev, transferQ, cmdPool, bufferHandles[bv.Buffer].AddrOfPinnedObject () + bv.ByteOffset, (ulong)bv.ByteLength);
				} else if (img.Uri.StartsWith ("data:", StringComparison.Ordinal)) {//load base64 encoded image
					Debug.WriteLine ("loading embedded image {0} : {1}", img.Name, img.MimeType);
					vkimg = Image.Load (dev, transferQ, cmdPool, glTFLoader.loadDataUri (img));
				} else {
					Debug.WriteLine ("loading image {0} : {1} : {2}", img.Name, img.MimeType, img.Uri);//load image from file path in uri
					vkimg = Image.Load (dev, transferQ, cmdPool, Path.Combine (baseDirectory, img.Uri));
					imgName += ";" + img.Uri;
				}

				vkimg.CreateView ();
				vkimg.CreateSampler ();
				vkimg.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;

				vkimg.SetName (imgName);
				vkimg.Descriptor.imageView.SetDebugMarkerName (dev, "imgView " + imgName);
				vkimg.Descriptor.sampler.SetDebugMarkerName (dev, "sampler " + imgName);

				textures.Add (vkimg);
			}
			return textures.ToArray ();
		}

		public Material[] LoadMaterial () {
			if (gltf.Materials == null)
				return new Material[] {};

			List<Material>	materials = new List<Material> ();

			foreach (GL.Material mat in gltf.Materials) {
				Debug.WriteLine ("loading material: " + mat.Name);
				Material pbr = new Material ();
				pbr.Name = mat.Name;

				pbr.alphaCutoff = mat.AlphaCutoff;
				pbr.alphaMode = (AlphaMode)mat.AlphaMode;

				FromFloatArray (ref pbr.emissiveFactor, mat.EmissiveFactor);

				if (mat.EmissiveTexture != null) {
					pbr.emissiveTexture = mat.EmissiveTexture.Index;
					if (mat.EmissiveTexture.TexCoord == 1)
						pbr.availableAttachments1 |= AttachmentType.Emissive;
					else
						pbr.availableAttachments |= AttachmentType.Emissive;
				}
				if (mat.NormalTexture != null) {
					pbr.normalTexture = mat.NormalTexture.Index;
					if (mat.NormalTexture.TexCoord == 1)
						pbr.availableAttachments1 |= AttachmentType.Normal;
					else
						pbr.availableAttachments |= AttachmentType.Normal;
				}
				if (mat.OcclusionTexture != null) {
					pbr.occlusionTexture = mat.OcclusionTexture.Index;
					if (mat.OcclusionTexture.TexCoord == 1)
						pbr.availableAttachments1 |= AttachmentType.AmbientOcclusion;
					else
						pbr.availableAttachments |= AttachmentType.AmbientOcclusion;
				}

				if (mat.PbrMetallicRoughness != null) {
					if (mat.PbrMetallicRoughness.BaseColorTexture != null) {
						pbr.baseColorTexture = mat.PbrMetallicRoughness.BaseColorTexture.Index;
						if (mat.PbrMetallicRoughness.BaseColorTexture.TexCoord == 1)
							pbr.availableAttachments1 |= AttachmentType.Color;
						else
							pbr.availableAttachments |= AttachmentType.Color;
					}

					FromFloatArray (ref pbr.baseColorFactor, mat.PbrMetallicRoughness.BaseColorFactor);

					if (mat.PbrMetallicRoughness.MetallicRoughnessTexture != null) {
						pbr.metallicRoughnessTexture = mat.PbrMetallicRoughness.MetallicRoughnessTexture.Index;
						if (mat.PbrMetallicRoughness.MetallicRoughnessTexture.TexCoord == 1)
							pbr.availableAttachments1 |= AttachmentType.PhysicalProps;
						else
							pbr.availableAttachments |= AttachmentType.PhysicalProps;
					}
					pbr.metallicFactor = mat.PbrMetallicRoughness.MetallicFactor;
					pbr.roughnessFactor = mat.PbrMetallicRoughness.RoughnessFactor;

					pbr.workflow = Material.Workflow.PhysicalyBaseRendering;
				}
				materials.Add (pbr);
			}
			return materials.ToArray ();
		}


		#region IDisposable Support
		private bool isDisposed = false; // Pour détecter les appels redondants

		protected virtual void Dispose (bool disposing) {
			if (!isDisposed) {
				if (disposing) {
					// TODO: supprimer l'état managé (objets managés).
				}

				for (int i = 0; i < gltf.Buffers.Length; i++) {
					if (bufferHandles[i].IsAllocated)
						bufferHandles[i].Free ();
				}

				isDisposed = true;
			}
		}

		~glTFLoader () {
			Dispose (false);
		}
		public void Dispose () {
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		#endregion
	}
}