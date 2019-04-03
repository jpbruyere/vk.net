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

	public class Model : IDisposable {
        Device dev;        

        UInt32 textureSize = 1024; //texture array size w/h
		DescriptorPool descriptorPool;


        public List<VKE.Image> textures = new List<Image> ();
		public List<Material> materials = new List<Material> ();
		public List<Mesh> Meshes = new List<Mesh> ();
		public List<Scene> Scenes;
		public int DefaultScene;

        public GPUBuffer vbo;
        public GPUBuffer ibo;
		public VkIndexType IndexBufferType { get; private set; } = VkIndexType.Uint16;

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


		public class Material {
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

		public Model (Device device, Queue transferQ, CommandPool cmdPool, string path) {
            dev = device;

			using (LoadingContext<UInt16> ctx = new LoadingContext<UInt16> (path, transferQ, cmdPool)) {
				IndexBufferType = ctx.IndexType;

				loadImages (ctx);
				loadMaterial (ctx);
				loadMeshes (ctx);
				loadScenes (ctx);
			}
		}

		public void WriteMaterialsDescriptorSets (DescriptorSetLayout layout, params ShaderBinding[] attachments) {
			if (attachments.Length == 0)
				throw new InvalidOperationException ("At least one attachment is required for Model.WriteMaterialDescriptor");

			descriptorPool = new DescriptorPool (dev, (uint)materials.Count,
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler, (uint)(attachments.Length * materials.Count))
			);

			foreach (Model.Material mat in materials) 
				WriteMaterialDescriptorSet (mat, layout, attachments);
		}
		/// <summary>
		/// Allocate and Write descriptorSet for material
		/// </summary>
		/// <param name="mat">Material</param>
		/// <param name="layout">Descriptor Layout for texture</param>
		/// <param name="attachments">Layout Attachments meaning</param>
		public void WriteMaterialDescriptorSet (Material mat, DescriptorSetLayout layout, params ShaderBinding[] attachments) {
			mat.descriptorSet = descriptorPool.Allocate (layout);
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

			using (DescriptorSetWrites uboUpdate = new DescriptorSetWrites (dev)) {
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

		/// <summary>
		/// Loading context with I as the vertex index type (uint16,uint32)
		/// </summary>
		class LoadingContext<I> : IDisposable {
			public Queue transferQ;
			public CommandPool cmdPool;

			public GL.Gltf gltf;
            public string baseDirectory;
            
            public VkIndexType IndexType;
            
            public byte[][] loadedBuffers;
            public GCHandle[] bufferHandles;

            public int VertexCount;
            public int IndexCount;

            public LoadingContext (string path, Queue _transferQ, CommandPool _cmdPool) {
				transferQ = _transferQ;
				cmdPool = _cmdPool;
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
		//TODO: some buffer data are reused between primitives, and I duplicate the datas
		//buffers must be constructed without duplications
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

			CommandBuffer cmd = ctx.cmdPool.AllocateCommandBuffer ();
			cmd.Start ();

			stagging.CopyTo (cmd, vbo, vertSize, 0, 0);
			stagging.CopyTo (cmd, ibo, idxSize, vertSize, 0);

			cmd.End ();

			ctx.transferQ.Submit (cmd);

			dev.WaitIdle ();
			cmd.Free ();

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
			if (ctx.gltf.Images == null)
				return;
			foreach (GL.Image img in ctx.gltf.Images) {
				VKE.Image vkimg = null;

				if (img.Uri.StartsWith ("data:", StringComparison.Ordinal)) {
					Debug.WriteLine ("loading embedded image {0} : {1}", img.Name, img.MimeType);
					vkimg = Image.Load (dev, ctx.transferQ, ctx.cmdPool, LoadingContext<I>.loadDataUri(img));
				} else {
					Debug.WriteLine ("loading image {0} : {1} : {2}", img.Name, img.MimeType, img.Uri);
					vkimg = Image.Load (dev, ctx.transferQ, ctx.cmdPool, Path.Combine (ctx.baseDirectory, img.Uri));
				}

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
		}


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
