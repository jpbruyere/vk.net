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
	using static VKE.Model;

	/// <summary>
	/// Loading context with I as the vertex index type (uint16,uint32)
	/// </summary>
	public class glTFLoader : IDisposable {
		public Queue transferQ;
		public CommandPool cmdPool;
		Device dev => transferQ.dev;

		public GL.Gltf gltf;
		public string baseDirectory;

		public byte[][] loadedBuffers;
		public GCHandle[] bufferHandles;

		List<Mesh> meshes;

		public glTFLoader (string path, Queue _transferQ, CommandPool _cmdPool) {
			transferQ = _transferQ;
			cmdPool = _cmdPool;
			baseDirectory = System.IO.Path.GetDirectoryName (path);
			gltf = Interface.LoadModel (path); ;
			loadedBuffers = new byte[gltf.Buffers.Length][];
			bufferHandles = new GCHandle[gltf.Buffers.Length];
		}

		public static byte[] loadDataUri (GL.Image img) {
			int idxComa = img.Uri.IndexOf (",", 5, StringComparison.Ordinal);
			return Convert.FromBase64String (img.Uri.Substring (idxComa + 1));
		}
		public static byte[] loadDataUri (GL.Buffer buff) {
			int idxComa = buff.Uri.IndexOf (",", 5, StringComparison.Ordinal);
			return Convert.FromBase64String (buff.Uri.Substring (idxComa + 1));
		}

		public void EnsureBufferIsLoaded (int bufferIdx) {
			if (loadedBuffers[bufferIdx] == null) {
				//load full buffer
				string uri = gltf.Buffers[bufferIdx].Uri;
				if (uri.StartsWith ("data", StringComparison.Ordinal))
					loadedBuffers[bufferIdx] = loadDataUri (gltf.Buffers[bufferIdx]);//TODO:check this func=>System.Buffers.Text.Base64.EncodeToUtf8InPlace
				else
					loadedBuffers[bufferIdx] = File.ReadAllBytes (Path.Combine (baseDirectory, gltf.Buffers[bufferIdx].Uri));
				bufferHandles[bufferIdx] = GCHandle.Alloc (loadedBuffers[bufferIdx], GCHandleType.Pinned);
			}
		}

		public void GetVertexCount (out ulong vertexCount, out ulong indexCount) {
			vertexCount = 0;
			indexCount = 0; 
			//compute size of stagging buf
			foreach (GL.Mesh mesh in gltf.Meshes) {
				foreach (GL.MeshPrimitive p in mesh.Primitives) {
					int accessorIdx;
					if (p.Attributes.TryGetValue ("POSITION", out accessorIdx))
						vertexCount += (ulong)gltf.Accessors[accessorIdx].Count;
					if (p.Indices != null)
						indexCount += (ulong)gltf.Accessors[(int)p.Indices].Count;
				}
			}
		}

		//TODO: some buffer data are reused between primitives, and I duplicate the datas
		//buffers must be constructed without duplications
		public Mesh[] LoadMeshes<TVertex> (VkIndexType indexType, Buffer vbo, ulong vboOffset, Buffer ibo, ulong iboOffset) {
			ulong vCount, iCount;
			GetVertexCount (out vCount, out iCount);
			ulong vertSize = vCount * (ulong)Marshal.SizeOf<TVertex> ();
			ulong idxSize = iCount * (indexType == VkIndexType.Uint16 ? 2ul : 4ul);
			ulong size = vertSize + idxSize;

			int vertexCount = 0, indexCount = 0;
			int autoNamedMesh = 1;

			meshes = new List<Mesh> ();

			using (HostBuffer stagging = new HostBuffer (dev, VkBufferUsageFlags.TransferSrc, size)) {
				stagging.Map ();

				unsafe {
					byte* stagVertPtr = (byte*)stagging.MappedData.ToPointer ();
					byte* stagIdxPtr = (byte*)(stagging.MappedData.ToPointer ()) + vertSize;

					foreach (GL.Mesh mesh in gltf.Meshes) {
						string meshName = mesh.Name;
						if (string.IsNullOrEmpty (meshName)) {
							meshName = "mesh_" + autoNamedMesh.ToString ();
							autoNamedMesh++;
						}
						Mesh m = new Mesh { Name = meshName };

						foreach (GL.MeshPrimitive p in mesh.Primitives) {
							GL.Accessor AccPos = null, AccNorm = null, AccUv = null;

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

							Primitive prim = new Primitive {
								indexBase = (uint)indexCount,
								vertexBase = vertexCount,
								vertexCount = (uint)AccPos.Count,
								material = (uint)(p.Material ?? 0)
							};
							//Interleaving vertices
							byte* inPosPtr = null, inNormPtr = null, inUvPtr = null;

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
								stagVertPtr += 32;
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
										System.Buffer.MemoryCopy (inIdxPtr, stagIdxPtr, bv.ByteLength, bv.ByteLength);
										stagIdxPtr += bv.ByteLength;
									} else { 
										for (int i = 0; i < bv.ByteLength; i++) {
											uint* usPtr = (uint*)stagIdxPtr;
											usPtr[0] = (uint)inIdxPtr[i];
										}
										stagIdxPtr += bv.ByteLength * 2;
									}
								} else if (acc.ComponentType == GL.Accessor.ComponentTypeEnum.UNSIGNED_INT) {
									if (indexType == VkIndexType.Uint32) {
										System.Buffer.MemoryCopy (inIdxPtr, stagIdxPtr, bv.ByteLength, bv.ByteLength);
										stagIdxPtr += bv.ByteLength;
									} else { 
										for (int i = 0; i < bv.ByteLength; i++) {
											ushort* usPtr = (ushort*)stagIdxPtr;
											usPtr[0] = (ushort)inIdxPtr[i];
										}
										stagIdxPtr += bv.ByteLength / 2;
									}
								} else if (acc.ComponentType == GL.Accessor.ComponentTypeEnum.UNSIGNED_BYTE) {
									//convert to Uint16
									if (indexType == VkIndexType.Uint16) {
										for (int i = 0; i < bv.ByteLength; i++) {
											ushort* usPtr = (ushort*)stagIdxPtr;
											usPtr[0] = (ushort)inIdxPtr[i];
										}
										stagIdxPtr += bv.ByteLength * 2;
									}
								} else
									throw new NotImplementedException ();

								prim.indexCount = (uint)acc.Count;
								indexCount += acc.Count;
							}

							m.Primitives.Add (prim);

							vertexCount += AccPos.Count;
						}
						meshes.Add (m);
					}
				}

				stagging.Unmap ();

				CommandBuffer cmd = cmdPool.AllocateCommandBuffer ();
				cmd.Start (VkCommandBufferUsageFlags.OneTimeSubmit);

				stagging.CopyTo (cmd, vbo, vertSize, 0, vboOffset);
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
					matrix = Matrix4x4.Identity,
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
			Quaternion rotation = new Quaternion ();
			Vector3 scale = new Vector3 ();
			Matrix4x4 localTransform = Matrix4x4.Identity;

			if (gltfNode.Matrix == null) {
				FromFloatArray (ref translation, gltfNode.Translation);
				FromFloatArray (ref rotation, gltfNode.Rotation);
				FromFloatArray (ref scale, gltfNode.Scale);

				localTransform =
					Matrix4x4.CreateTranslation (translation) *
					Matrix4x4.CreateFromQuaternion (rotation) *
					Matrix4x4.CreateScale (scale);

			} else {
				unsafe {
					long size = (long)Marshal.SizeOf<Matrix4x4> ();
					GCHandle ptr = GCHandle.Alloc (localTransform, GCHandleType.Pinned);
					fixed (float* m = gltfNode.Matrix) {
						System.Buffer.MemoryCopy (m, ptr.AddrOfPinnedObject ().ToPointer (), size, size);
					}
					ptr.Free ();
				}
			}

			Node node = new Node {
				matrix = localTransform,
				Parent = parentNode
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

				pbr.pbrDatas.alphaCutoff = mat.AlphaCutoff;
				pbr.pbrDatas.alphaMode = (AlphaMode)mat.AlphaMode;
				FromFloatArray (ref pbr.pbrDatas.emissiveFactor, mat.EmissiveFactor);
				if (mat.EmissiveTexture != null) {
					pbr.emissiveTexture = (uint)mat.EmissiveTexture.Index;
					pbr.pbrDatas.availableAttachments |= ShaderBinding.Emissive;
				}
				if (mat.NormalTexture != null) {
					pbr.normalTexture = (uint)mat.NormalTexture.Index;
					pbr.pbrDatas.availableAttachments |= ShaderBinding.Normal;
				}
				if (mat.OcclusionTexture != null) {
					pbr.occlusionTexture = (uint)mat.OcclusionTexture.Index;
					pbr.pbrDatas.availableAttachments |= ShaderBinding.AmbientOcclusion;
				}

				if (mat.PbrMetallicRoughness != null) {
					if (mat.PbrMetallicRoughness.BaseColorTexture != null) {
						pbr.baseColorTexture = (uint)mat.PbrMetallicRoughness.BaseColorTexture.Index;
						pbr.pbrDatas.availableAttachments |= ShaderBinding.Color;
					}
					FromFloatArray (ref pbr.pbrDatas.baseColorFactor, mat.PbrMetallicRoughness.BaseColorFactor);
					if (mat.PbrMetallicRoughness.MetallicRoughnessTexture != null) {
						pbr.metallicRoughnessTexture = (uint)mat.PbrMetallicRoughness.MetallicRoughnessTexture.Index;
						pbr.pbrDatas.availableAttachments |= ShaderBinding.MetalRoughness;
					}
					pbr.pbrDatas.metallicFactor = mat.PbrMetallicRoughness.MetallicFactor;
					pbr.pbrDatas.roughnessFactor = mat.PbrMetallicRoughness.RoughnessFactor;
				}
				materials.Add (pbr);
				/*
				ulong size = (ulong)(materials.Count * Marshal.SizeOf<Material> ());

				uboMaterials = new GPUBuffer (dev, VkBufferUsageFlags.UniformBuffer | VkBufferUsageFlags.TransferDst, size);

				using (HostBuffer stagging = new HostBuffer (dev, VkBufferUsageFlags.TransferSrc, size, materials.Data)) {
					CommandBuffer cmd = cmdPool.AllocateCommandBuffer ();

					cmd.Start ();
					stagging.CopyTo (cmd, uboMaterials);
					cmd.End ();

					transferQ.Submit (cmd);

					dev.WaitIdle ();
					cmd.Destroy ();
				}
				*/
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