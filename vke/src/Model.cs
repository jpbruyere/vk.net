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

using Vulkan;
using System.Collections.Generic;
using System.IO;



namespace VKE {
    using static Utils;

    public class Model : IDisposable {
        Device dev;
        Queue transferQ;
        CommandPool cmdPool;

        UInt32 textureSize = 1024; //texture array size w/h


        public List<VKE.Image> textures = new List<Image> ();
		public List<Material> materials = new List<Material> ();
		public List<Mesh> Meshes = new List<Mesh> ();
		public List<Scene> Scenes;
		public int DefaultScene;

		public GPUBuffer uboMaterials;
        public GPUBuffer vbo;
        public GPUBuffer ibo;

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

		
        public class Material {
            public Vector4 baseColorFactor;
            public AlphaMode alphaMode;
            public float alphaCutoff;
            public float metallicFactor;
            public float roughnessFactor;

            public UInt32 baseColorTexture;
            public UInt32 metallicRoughnessTexture;
            public UInt32 normalTexture;
            public UInt32 occlusionTexture;

            public UInt32 emissiveTexture;

            public Vector4 emissiveFactor;

            public Material (
                UInt32 _baseColorTexture = 0, UInt32 _metallicRoughnessTexture = 0, UInt32 _normalTexture = 0, UInt32 _occlusionTexture = 0) {
                baseColorFactor = new Vector4 (1f);
                alphaMode = AlphaMode.Opaque;
                alphaCutoff = 1f;
                metallicFactor = 1f;
                roughnessFactor = 1f;

                baseColorTexture = _baseColorTexture;
                metallicRoughnessTexture = _metallicRoughnessTexture;
                normalTexture = _normalTexture;
                occlusionTexture = _occlusionTexture;

                emissiveTexture = 0;
                emissiveFactor = new Vector4 (0.0f);
            }

			public DescriptorSet descriptorSet;

        }

        public enum AlphaMode : UInt32 {
            Opaque,
            Mask,
            Blend
        };
        public struct Vertex {
            public Vector3 pos;
            public Vector3 normal;
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

		public Model (Device device, Queue _transferQ, CommandPool _cmdPool, string path) {
            dev = device;
            transferQ = _transferQ;
            cmdPool = _cmdPool;

			using (LoadingContext<UInt16> ctx = new LoadingContext<UInt16> (path)) {

				loadImages (ctx);
				loadMaterial (ctx);
				loadMeshes (ctx);
				loadScenes (ctx);
			}
        }

        /// <summary>
        /// Loading context with I as the vertex index type (uint16,uint32)
        /// </summary>
        class LoadingContext<I> : IDisposable {
            public GL.Gltf gltf;
            public string baseDirectory;
            
            public VkIndexType IndexType;
            
            public byte[][] loadedBuffers;
            public GCHandle[] bufferHandles;

            public int VertexCount;
            public int IndexCount;

            public LoadingContext (string path) {
                baseDirectory = System.IO.Path.GetDirectoryName (path);
                gltf = Interface.LoadModel (path); ;
                loadedBuffers = new byte[gltf.Buffers.Length][];
                bufferHandles = new GCHandle[gltf.Buffers.Length];

                if (typeof (I) == typeof (uint))
                    IndexType = VkIndexType.Uint32;
                else if (typeof (I) == typeof (ushort))
                    IndexType = VkIndexType.Uint16;
                else throw new Exception ("unsupported vertex index type : " + typeof (I).ToString ());
            }

			public void EnsureBufferIsLoaded (int bufferIdx) {
				if (loadedBuffers[bufferIdx] == null) {
					//load full buffer
					string uri = gltf.Buffers[bufferIdx].Uri;
					if (uri.StartsWith ("data", StringComparison.Ordinal)) {
						//embedded
						//System.Buffers.Text.Base64.EncodeToUtf8InPlace (tmp,tmp.Length, out int bytewriten);
						//string txt = System.Text.Encoding.UTF8.GetString (tmp);
						//string txt = File.ReadAllText (Path.Combine (baseDirectory, gltf.Buffers[bv.Buffer].Uri));
						//byte[] b64 = System.Convert.FromBase64String (tmp);
						//                                 = Convert.FromBase64 (tmp);
						throw new NotImplementedException ();
					} else
						loadedBuffers[bufferIdx] = File.ReadAllBytes (Path.Combine (baseDirectory, gltf.Buffers[bufferIdx].Uri));
					bufferHandles[bufferIdx] = GCHandle.Alloc (loadedBuffers[bufferIdx], GCHandleType.Pinned);
				}
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

			 ~LoadingContext() {
				Dispose(false);
			 }
			public void Dispose () {
				Dispose (true);
				GC.SuppressFinalize(this);
			}
			#endregion
		}

		void loadMeshes<I> (LoadingContext<I> ctx) {
			//compute size of stagging buf
			foreach (GL.Mesh mesh in ctx.gltf.Meshes) {
				foreach (GL.MeshPrimitive p in mesh.Primitives) {
					int accessorIdx;
					if (p.Attributes.TryGetValue ("POSITION", out accessorIdx))
						ctx.VertexCount += ctx.gltf.Accessors[accessorIdx].Count;
					if (p.Indices != null)
						ctx.IndexCount += ctx.gltf.Accessors[(int)p.Indices].Count;
				}
			}

			ulong vertSize = (ulong)(ctx.VertexCount * Marshal.SizeOf<Vertex> ());
			ulong idxSize = (ulong)(ctx.IndexCount * (ctx.IndexType == VkIndexType.Uint16 ? 2 : 4));
			ulong size = vertSize + idxSize;

			HostBuffer stagging = new HostBuffer (dev, VkBufferUsageFlags.TransferSrc, size);
			stagging.Map ();

			int vertexCount = 0, indexCount = 0;
			int autoNamedMesh = 1;

			unsafe {
				byte* stagVertPtr = (byte*)stagging.MappedData.ToPointer ();
				byte* stagIdxPtr = (byte*)(stagging.MappedData.ToPointer ()) + vertSize;

				foreach (GL.Mesh mesh in ctx.gltf.Meshes) {
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
							AccPos = ctx.gltf.Accessors[accessorIdx];
							ctx.EnsureBufferIsLoaded (ctx.gltf.BufferViews[(int)AccPos.BufferView].Buffer);
						}
						if (p.Attributes.TryGetValue ("NORMAL", out accessorIdx)) {
							AccNorm = ctx.gltf.Accessors[accessorIdx];
							ctx.EnsureBufferIsLoaded (ctx.gltf.BufferViews[(int)AccNorm.BufferView].Buffer);
						}
						if (p.Attributes.TryGetValue ("TEXCOORD_0", out accessorIdx)) {
							AccUv = ctx.gltf.Accessors[accessorIdx];
							ctx.EnsureBufferIsLoaded (ctx.gltf.BufferViews[(int)AccUv.BufferView].Buffer);
						}

						Primitive prim = new Primitive {
							indexBase = (uint)indexCount,
							vertexBase = vertexCount,
							vertexCount = (uint)AccPos.Count,
							material = (uint)(p.Material ?? 0)
						};
						//Interleaving vertices
						byte* inPosPtr = null, inNormPtr = null, inUvPtr = null;

						GL.BufferView bv = ctx.gltf.BufferViews[(int)AccPos.BufferView];
						inPosPtr = (byte*)ctx.bufferHandles[bv.Buffer].AddrOfPinnedObject ().ToPointer ();
						inPosPtr += AccPos.ByteOffset + bv.ByteOffset;

						if (AccNorm != null) {
							bv = ctx.gltf.BufferViews[(int)AccNorm.BufferView];
							inNormPtr = (byte*)ctx.bufferHandles[bv.Buffer].AddrOfPinnedObject ().ToPointer ();
							inNormPtr += AccNorm.ByteOffset + bv.ByteOffset;
						}
						if (AccUv != null) {
							bv = ctx.gltf.BufferViews[(int)AccUv.BufferView];
							inUvPtr = (byte*)ctx.bufferHandles[bv.Buffer].AddrOfPinnedObject ().ToPointer ();
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
							GL.Accessor acc = ctx.gltf.Accessors[(int)p.Indices];
							bv = ctx.gltf.BufferViews[(int)acc.BufferView];

							#region check index type
							VkIndexType idxType;
							if (acc.ComponentType == GL.Accessor.ComponentTypeEnum.UNSIGNED_SHORT)
								idxType = VkIndexType.Uint16;
							else if (acc.ComponentType == GL.Accessor.ComponentTypeEnum.UNSIGNED_INT)
								idxType = VkIndexType.Uint32;
							else
								throw new NotImplementedException ();

							if (idxType != ctx.IndexType)
								throw new Exception ("primitive index type different from loadingContext index type");
							#endregion

							byte* inIdxPtr = (byte*)ctx.bufferHandles[bv.Buffer].AddrOfPinnedObject ().ToPointer ();
							inIdxPtr += acc.ByteOffset + bv.ByteOffset;

							System.Buffer.MemoryCopy (inIdxPtr, stagIdxPtr, bv.ByteLength, bv.ByteLength);

							prim.indexCount = (uint)acc.Count;

							stagIdxPtr += bv.ByteLength;
							indexCount += acc.Count;
						}

						m.Primitives.Add (prim);

						vertexCount += AccPos.Count;
					}
					Meshes.Add (m);
				}
			}

			stagging.Unmap ();

			vbo = new GPUBuffer (dev, VkBufferUsageFlags.VertexBuffer | VkBufferUsageFlags.TransferDst, vertSize);
			ibo = new GPUBuffer (dev, VkBufferUsageFlags.IndexBuffer | VkBufferUsageFlags.TransferDst, idxSize);

			CommandBuffer cmd = cmdPool.AllocateCommandBuffer ();
			cmd.Start ();

			stagging.CopyTo (cmd, vbo, vertSize, 0, 0);
			stagging.CopyTo (cmd, ibo, idxSize, vertSize, 0);

			cmd.End ();

			transferQ.Submit (cmd);

			dev.WaitIdle ();
			cmd.Destroy ();

			stagging.Dispose ();
		}

		void loadScenes<I> (LoadingContext<I> ctx) {
			if (ctx.gltf.Scene == null)
				return;

			Scenes = new List<Scene> ();
			DefaultScene = (int)ctx.gltf.Scene;
			for (int i = 0; i < ctx.gltf.Scenes.Length; i++) {
				GL.Scene scene = ctx.gltf.Scenes[i];
				Debug.WriteLine ("Loading Scene {0}", scene.Name);

				Scenes.Add (new Scene {
					Name = scene.Name,
				});

				if (scene.Nodes.Length == 0)
					continue;

				Scenes[i].Root = new Node {
					matrix = Matrix4x4.Identity,
					Children = new List<Node> ()
				};

				foreach (int nodeIdx in scene.Nodes)
					loadNode (ctx, Scenes[i].Root, ctx.gltf.Nodes[nodeIdx]);					
			}

		}

		void loadNode<I> (LoadingContext<I> ctx, Node parentNode, GL.Node gltfNode) {
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
					loadNode (ctx, node, ctx.gltf.Nodes[gltfNode.Children[i]]);
			}
		        
		    if (gltfNode.Mesh != null)
				node.Mesh = Meshes[(int)gltfNode.Mesh];		    
		}

		void loadImages<I> (LoadingContext<I> ctx) {
			HostBuffer stagging;
			if (ctx.gltf.Images == null)
				return;
			foreach (GL.Image img in ctx.gltf.Images) {
				Debug.WriteLine ("loading image {0} : {1} : {2}", img.Name, img.MimeType, img.Uri);

				int width, height, channels;
				IntPtr imgPtr = Stb.Load (Path.Combine (ctx.baseDirectory, img.Uri), out width, out height, out channels, 4);
				long size = width * height * 4;

				stagging = new HostBuffer (dev, VkBufferUsageFlags.TransferSrc, (UInt64)size);

				stagging.Map ((ulong)size);
				unsafe {
					System.Buffer.MemoryCopy (imgPtr.ToPointer (), stagging.MappedData.ToPointer (), size, size);
				}
				stagging.Unmap ();

				Stb.FreeImage (imgPtr);

				VKE.Image vkimg = new Image (dev, VkFormat.R8g8b8a8Unorm, VkImageUsageFlags.TransferDst | VkImageUsageFlags.Sampled,
					VkMemoryPropertyFlags.DeviceLocal, (uint)width, (uint)height);

				CommandBuffer cmd = cmdPool.AllocateCommandBuffer ();
				cmd.Start ();

				stagging.CopyTo (cmd, vkimg);

				cmd.End ();

				transferQ.Submit (cmd);

				dev.WaitIdle ();
				cmd.Destroy ();

				stagging.Dispose ();

				vkimg.CreateView ();
				vkimg.CreateSampler ();
				vkimg.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;

				textures.Add (vkimg);
			}
		}

		void loadMaterial<I> (LoadingContext<I> ctx) {
			if (ctx.gltf.Materials == null)
				return;

			foreach (GL.Material mat in ctx.gltf.Materials) {
				Debug.WriteLine ("loading material: " + mat.Name);
				Material pbr = new Material ();

				pbr.alphaCutoff = mat.AlphaCutoff;
				pbr.alphaMode = (AlphaMode)mat.AlphaMode;
				FromFloatArray (ref pbr.emissiveFactor, mat.EmissiveFactor);
				if (mat.EmissiveTexture != null)
					pbr.emissiveTexture = (uint)mat.EmissiveTexture.Index;
				if (mat.NormalTexture != null)
					pbr.normalTexture = (uint)mat.NormalTexture.Index;
				if (mat.OcclusionTexture != null)
					pbr.occlusionTexture = (uint)mat.OcclusionTexture.Index;

				if (mat.PbrMetallicRoughness != null) {
					if (mat.PbrMetallicRoughness.BaseColorTexture != null)
						pbr.baseColorTexture = (uint)mat.PbrMetallicRoughness.BaseColorTexture.Index;
					FromFloatArray (ref pbr.baseColorFactor, mat.PbrMetallicRoughness.BaseColorFactor);
					if (mat.PbrMetallicRoughness.MetallicRoughnessTexture != null)
						pbr.metallicRoughnessTexture = (uint)mat.PbrMetallicRoughness.MetallicRoughnessTexture.Index;
					pbr.metallicFactor = mat.PbrMetallicRoughness.MetallicFactor;
					pbr.roughnessFactor = mat.PbrMetallicRoughness.RoughnessFactor;
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
		}


		public void Bind (CommandBuffer cmd) {
			cmd.BindVertexBuffer (vbo);
			cmd.BindIndexBuffer (ibo, VkIndexType.Uint16);
		}

		public PipelineLayout PipelineLayout;

		public void RenderNode (CommandBuffer cmd, Node node, Matrix4x4 currentTransform) {
			Matrix4x4 localMat = currentTransform * node.matrix;

			cmd.PushConstant (PipelineLayout, VkShaderStageFlags.Vertex, localMat);

			if (node.Mesh != null) {
				foreach (Primitive p in node.Mesh.Primitives) {
					cmd.BindDescriptorSet (PipelineLayout, materials[(int)p.material].descriptorSet, 1);
					cmd.DrawIndexed (p.indexCount, 1, p.indexBase, p.vertexBase, 0);
				}
			}
			if (node.Children == null)
				return;
			foreach (Node child in node.Children) 
				RenderNode (cmd, child, localMat);
		}

		public void DrawAll (CommandBuffer cmd, PipelineLayout pipelineLayout) {

			foreach (Scene sc in Scenes) {
				foreach (Node node in sc.Root.Children) {
					RenderNode (cmd, node, Matrix4x4.Identity);
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
					foreach (Image txt in textures) {
						txt.Dispose ();
					}
					uboMaterials?.Dispose ();
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
