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

    public class Model {
        Device dev;
        Queue transferQ;
        CommandPool cmdPool;

        UInt32 textureSize = 1024; //texture array size w/h


        public List<VKE.Image> textures = new List<Image> ();
        public Dictionary<string, List<Primitive>> primitives = new Dictionary<string, List<Primitive>> ();

        public GPUBuffer uboMaterials;
        public GPUBuffer vbo;
        public GPUBuffer ibo;


        [StructLayout (LayoutKind.Explicit, Size = 80)]
        public struct Material {
            [FieldOffset (0)] public Vector4 baseColorFactor;
            [FieldOffset (16)] public AlphaMode alphaMode;
            [FieldOffset (20)] public float alphaCutoff;
            [FieldOffset (24)] public float metallicFactor;
            [FieldOffset (28)] public float roughnessFactor;

            [FieldOffset (32)] public UInt32 baseColorTexture;
            [FieldOffset (36)] public UInt32 metallicRoughnessTexture;
            [FieldOffset (40)] public UInt32 normalTexture;
            [FieldOffset (44)] public UInt32 occlusionTexture;

            [FieldOffset (48)] public UInt32 emissiveTexture;

            [FieldOffset (64)] public Vector4 emissiveFactor;

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

        public struct Primitive {
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

        public Model (Device device, Queue _transferQ, CommandPool _cmdPool, string path) {
            dev = device;
            transferQ = _transferQ;
            cmdPool = _cmdPool;

            LoadingContext<UInt16> ctx = new LoadingContext<UInt16> (path);

            loadImages (ctx);
            loadMaterial (ctx);
            loadMeshes (ctx);
            //loadScenes (ctx);

            releaseBufferHandles (ctx);
        }
        /// <summary>
        /// Loading context with I as the vertex index type (uint16,uint32)
        /// </summary>
        class LoadingContext<I> {
            public GL.Gltf gltf;
            public string baseDirectory;
            public List<Vertex> vertices = new List<Vertex>();
            public VkIndexType IndexType;
            public List<I> indices = new List<I>();
            public byte[][] loadedBuffers;
            public GCHandle[] bufferHandles;

            public int VertexCount;
            public int IndexCount;

            public uint staggingIndex;
            public HostBuffer stagging;

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
        }

        void ensureBufferIsLoaded<I> (LoadingContext<I> ctx, int bufferIdx) {
            if (ctx.loadedBuffers[bufferIdx] == null) {
                //load full buffer
                string uri = ctx.gltf.Buffers[bufferIdx].Uri;
                if (uri.StartsWith ("data", StringComparison.Ordinal)) {
                    //embedded
                    //System.Buffers.Text.Base64.EncodeToUtf8InPlace (tmp,tmp.Length, out int bytewriten);
                    //string txt = System.Text.Encoding.UTF8.GetString (tmp);
                    //string txt = File.ReadAllText (Path.Combine (baseDirectory, gltf.Buffers[bv.Buffer].Uri));
                    //byte[] b64 = System.Convert.FromBase64String (tmp);
                    //                                 = Convert.FromBase64 (tmp);
                    throw new NotImplementedException ();
                } else
                    ctx.loadedBuffers[bufferIdx] = File.ReadAllBytes (Path.Combine (ctx.baseDirectory, ctx.gltf.Buffers[bufferIdx].Uri));
                ctx.bufferHandles[bufferIdx] = GCHandle.Alloc (ctx.loadedBuffers[bufferIdx], GCHandleType.Pinned);
            }
        }

        void releaseBufferHandles<I> (LoadingContext<I> ctx) {
            for (int i = 0; i < ctx.gltf.Buffers.Length; i++) {
                if (ctx.bufferHandles[i].IsAllocated)
                    ctx.bufferHandles[i].Free ();
            }
        }

        //void loadNode2<I> (LoadingContext<I> ctx, GL.Node node, Matrix4x4 parentTransform) {

        //    #region Transformations 
        //    Vector3 translation = new Vector3 ();
        //    Quaternion rotation = new Quaternion ();
        //    Vector3 scale = new Vector3 ();
        //    Matrix4x4 localTransform = Matrix4x4.Identity;


        //    if (node.Matrix == null) {
        //        FromFloatArray (ref translation, node.Translation);
        //        FromFloatArray (ref rotation, node.Rotation);
        //        FromFloatArray (ref scale, node.Scale);

        //        localTransform =
        //            Matrix4x4.CreateTranslation (translation) *
        //            Matrix4x4.CreateFromQuaternion (rotation) *
        //            Matrix4x4.CreateScale (scale);

        //    } else {
        //        unsafe {
        //            long size = (long)Marshal.SizeOf<Matrix4x4> ();
        //            GCHandle ptr = GCHandle.Alloc (localTransform, GCHandleType.Pinned);
        //            fixed (float* m = node.Matrix) {
        //                System.Buffer.MemoryCopy (m, ptr.AddrOfPinnedObject ().ToPointer (), size, size);
        //            }
        //            ptr.Free ();
        //        }
        //    }

        //    localTransform = parentTransform * localTransform;
        //    #endregion

        //    for (int i = 0; i < node.Children?.Length; i++)
        //        loadNode2 (ctx, ctx.gltf.Nodes[node.Children[i]], localTransform);

        //    if (node.Mesh != null) {
        //        GL.Mesh mesh = ctx.gltf.Meshes[(int)node.Mesh];

        //        List<Primitive> primitives = new List<Primitive> ();

        //        for (int i = 0; i < mesh.Primitives.Length; i++) {
        //            GL.MeshPrimitive p = mesh.Primitives[i];

        //            Primitive prim = new Primitive {
        //                indexBase = (uint)ctx.IndexCount,
        //                vertexBase = (uint)ctx.VertexCount,
        //                material = (uint)p.Material,
        //                attributs = new List<Primitive.Attribut> ()
        //            };

        //            int accessorIdx;
        //            if (p.Attributes.TryGetValue ("POSITION", out accessorIdx))
        //                prim.AccPos = ctx.gltf.Accessors[accessorIdx];
        //            if (p.Attributes.TryGetValue ("NORMAL", out accessorIdx)) 
        //                prim.AccNorm = ctx.gltf.Accessors[accessorIdx];
        //            if (p.Attributes.TryGetValue ("TEXCOORD_0", out accessorIdx)) 
        //                prim.AccUv = ctx.gltf.Accessors[accessorIdx];                        
        //            if (p.Indices != null)
        //                prim.AccIdx = ctx.gltf.Accessors[(int)p.Indices];

        //            ctx.VertexCount += prim.AccPos.Count;
        //            ctx.IndexCount += prim.AccIdx.Count;

        //            primitives.Add (prim);
        //        }

        //        ctx.primitives[mesh.Name] = primitives;
        //    }
        //}

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

                    primitives.Add (meshName, new List<Primitive> ());

                    foreach (GL.MeshPrimitive p in mesh.Primitives) {
                        GL.Accessor AccPos = null, AccNorm = null, AccUv = null;

                        int accessorIdx;
                        if (p.Attributes.TryGetValue ("POSITION", out accessorIdx)) {
                            AccPos = ctx.gltf.Accessors[accessorIdx];
                            ensureBufferIsLoaded (ctx, ctx.gltf.BufferViews[(int)AccPos.BufferView].Buffer);
                        }
                        if (p.Attributes.TryGetValue ("NORMAL", out accessorIdx)) {
                            AccNorm = ctx.gltf.Accessors[accessorIdx];
                            ensureBufferIsLoaded (ctx, ctx.gltf.BufferViews[(int)AccNorm.BufferView].Buffer);
                        }
                        if (p.Attributes.TryGetValue ("TEXCOORD_0", out accessorIdx)) {
                            AccUv = ctx.gltf.Accessors[accessorIdx];
                            ensureBufferIsLoaded (ctx, ctx.gltf.BufferViews[(int)AccUv.BufferView].Buffer);
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

                        Console.WriteLine ("orig pos");
                        for (int i = 0; i < bv.ByteLength; i += 12) {
                            Console.WriteLine ("{0};{1};{2}",
                                BitConverter.ToSingle (ctx.loadedBuffers[bv.Buffer], i + AccPos.ByteOffset + bv.ByteOffset),
                                BitConverter.ToSingle (ctx.loadedBuffers[bv.Buffer], i + 4 + AccPos.ByteOffset + bv.ByteOffset),
                                BitConverter.ToSingle (ctx.loadedBuffers[bv.Buffer], i + 8 + AccPos.ByteOffset + bv.ByteOffset));
                        }


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
                                throw new NotImplementedException();

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

                        primitives[meshName].Add (prim);
                    
                        vertexCount += AccPos.Count;
                    }
                    //int byteSize = vertexCount * Marshal.SizeOf<Vertex> ();
                    //byte[] arr = new byte[byteSize];
                    //Marshal.Copy (stagging.MappedData, arr, 0, byteSize);

                    //for (int i = 0; i < byteSize; i += Marshal.SizeOf<Vertex> ()) {
                    //    Console.WriteLine ("{0};{1};{2}",
                    //        BitConverter.ToSingle (arr, i),
                    //        BitConverter.ToSingle (arr, i + 4),
                    //        BitConverter.ToSingle (arr, i + 8));
                    //}
                    //int byteSize = indexCount * 2;
                    //byte[] arr = new byte[byteSize];
                    //IntPtr ptrIndices = new IntPtr (stagging.MappedData.ToInt64 () + vertexCount * Marshal.SizeOf<Vertex> ());
                    //Marshal.Copy (ptrIndices, arr, 0, byteSize);

                    //for (int i = 0; i < byteSize; i += 2) {
                    //    Console.WriteLine ("{0}",
                    //        BitConverter.ToInt16 (arr, i));
                    //}
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

        //void loadNode<I> (LoadingContext<I> ctx, GL.Node node, Matrix4x4 parentTransform) {

        //    Vector3 translation = new Vector3 ();
        //    Quaternion rotation = new Quaternion ();
        //    Vector3 scale = new Vector3 ();
        //    Matrix4x4 localTransform = Matrix4x4.Identity;


        //    if (node.Matrix == null) {
        //        FromFloatArray (ref translation, node.Translation);
        //        FromFloatArray (ref rotation, node.Rotation);
        //        FromFloatArray (ref scale, node.Scale);

        //        localTransform =
        //            Matrix4x4.CreateTranslation (translation) *
        //            Matrix4x4.CreateFromQuaternion (rotation) *
        //            Matrix4x4.CreateScale (scale);

        //    } else {
        //        unsafe {
        //            long size = (long)Marshal.SizeOf<Matrix4x4> ();
        //            GCHandle ptr = GCHandle.Alloc (localTransform, GCHandleType.Pinned);
        //            fixed (float* m = node.Matrix) {
        //                System.Buffer.MemoryCopy (m, ptr.AddrOfPinnedObject ().ToPointer (), size, size);
        //            }
        //            ptr.Free ();
        //        }
        //    }

        //    localTransform = parentTransform * localTransform;

        //    for (int i = 0; i < node.Children?.Length; i++)
        //        loadNode (ctx, ctx.gltf.Nodes[node.Children[i]], localTransform);

        //    if (node.Mesh != null) {
        //        GL.Mesh mesh = ctx.gltf.Meshes[(int)node.Mesh];

        //        List<Primitive> primitives = new List<Primitive> ();

        //        for (int i = 0; i < mesh.Primitives.Length; i++) {
        //            GL.MeshPrimitive p = mesh.Primitives[i];

        //            Primitive prim = new Primitive {
        //                indexBase = (uint)ctx.indices.Count,
        //                vertexBase = (uint)ctx.vertices.Count,
        //                material = (uint)p.Material,
        //                attributs = new List<Primitive.Attribut> ()
        //            };

        //            #region Vertices loading

        //            int accessorIdx;
        //            if (p.Attributes.TryGetValue ("POSITION", out accessorIdx)) {
        //                GL.Accessor acc = ctx.gltf.Accessors[accessorIdx];
        //                GL.BufferView bv = ctx.gltf.BufferViews[(int)acc.BufferView];

        //                ensureBufferIsLoaded (ctx, bv.Buffer);

        //                prim.vertexCount = (uint)acc.Count;

        //                prim.attributs.Add (new Primitive.Attribut { 
        //                    type = Primitive.AttributType.Position,
        //                    idx = bv.Buffer,
        //                    offset = acc.ByteOffset + bv.ByteOffset
        //                });
        //            }
        //            if (p.Attributes.TryGetValue ("NORMAL", out accessorIdx)) {
        //                GL.Accessor acc = ctx.gltf.Accessors[accessorIdx];
        //                GL.BufferView bv = ctx.gltf.BufferViews[(int)acc.BufferView];

        //                ensureBufferIsLoaded (ctx, bv.Buffer);

        //                prim.vertexCount = (uint)acc.Count;

        //                prim.attributs.Add (new Primitive.Attribut {
        //                    type = Primitive.AttributType.Position,
        //                    idx = bv.Buffer,
        //                    offset = acc.ByteOffset + bv.ByteOffset
        //                });
        //            }
        //            if (p.Attributes.TryGetValue ("TEXCOORD_0", out accessorIdx)) {
        //                GL.Accessor acc = ctx.gltf.Accessors[accessorIdx];
        //                GL.BufferView bv = ctx.gltf.BufferViews[(int)acc.BufferView];

        //                ensureBufferIsLoaded (ctx, bv.Buffer);

        //                prim.vertexCount = (uint)acc.Count;

        //                prim.attributs.Add (new Primitive.Attribut {
        //                    type = Primitive.AttributType.Position,
        //                    idx = bv.Buffer,
        //                    offset = acc.ByteOffset + bv.ByteOffset
        //                });
        //            }


        //            for (int j = 0; j < prim.vertexCount; j++) {
        //                Vertex v = new Vertex ();
        //                foreach (Primitive.Attribut att in prim.attributs) {
        //                    switch (att.type) {
        //                        case Primitive.AttributType.Position:
        //                            FromByteArray (ref v.pos, ctx.loadedBuffers[att.idx], att.offset);
        //                            v.pos = v.pos.Transform(ref localTransform, true);
        //                            break;
        //                        case Primitive.AttributType.Normal:
        //                            FromByteArray (ref v.normal, ctx.loadedBuffers[att.idx], att.offset);
        //                            v.normal = v.normal.Transform (ref localTransform, false);
        //                            break;
        //                        case Primitive.AttributType.uv:
        //                            FromByteArray (ref v.uv, ctx.loadedBuffers[att.idx], att.offset);
        //                            break;                                
        //                    }
        //                }

        //                ctx.vertices.Add (v);
        //            }
        //            #endregion

        //            #region Indices loading
        //            if (p.Indices != null) {
        //                GL.Accessor acc = ctx.gltf.Accessors[(int)p.Indices];
        //                GL.BufferView bv = ctx.gltf.BufferViews[(int)acc.BufferView];

        //                ensureBufferIsLoaded (ctx, bv.Buffer);

        //                prim.indexAcc = acc;
        //                prim.indexView = bv;

        //                #region check index type
        //                VkIndexType idxType;
        //                if (acc.ComponentType == GL.Accessor.ComponentTypeEnum.UNSIGNED_SHORT)
        //                    idxType = VkIndexType.Uint16;
        //                else if (acc.ComponentType == GL.Accessor.ComponentTypeEnum.UNSIGNED_INT)
        //                    idxType = VkIndexType.Uint32;
        //                else 
        //                    throw new NotImplementedException();

        //                if (idxType != ctx.IndexType)
        //                    throw new Exception ("primitive index type different from loadingContext index type");

        //                #endregion
        //            }

        //            #endregion

        //            primitives.Add (prim);
        //        }

        //        ctx.primitives[mesh.Name] = primitives;
        //    }


        //}

        //void loadScenes<I> (LoadingContext<I> ctx) {
        //    if (ctx.gltf.Scene == null)
        //        return;
        //    GL.Scene scene = ctx.gltf.Scenes[(int)ctx.gltf.Scene];

        //    foreach (int nodeIdx in scene.Nodes) 
        //        loadNode (ctx, ctx.gltf.Nodes[nodeIdx], Matrix4x4.Identity);

        //}
        void loadImages<I> (LoadingContext<I> ctx) {
            HostBuffer stagging;
            if (ctx.gltf.Images == null)
                return;
            foreach (GL.Image img in ctx.gltf.Images) {
                Console.WriteLine ("loading image {0} : {1} : {2}", img.Name, img.MimeType, img.Uri);

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

                textures.Add (vkimg);
            }
        }

        void loadMaterial<I> (LoadingContext<I> ctx) {
            if (ctx.gltf.Materials == null)
                return;
            using (NativeList<Material> materials = new NativeList<Material> ()) {

                foreach (GL.Material mat in ctx.gltf.Materials) {
                    Debug.WriteLine ("loading material: " + mat.Name);
                    Material pbr = new Material ();

                    pbr.alphaCutoff = mat.AlphaCutoff;
                    pbr.alphaMode = (AlphaMode)mat.AlphaMode;
                    FromFloatArray(ref pbr.emissiveFactor, mat.EmissiveFactor);
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
                }

                ulong size = (ulong)(materials.Count * Marshal.SizeOf<Material> ());

                uboMaterials = new GPUBuffer (dev, VkBufferUsageFlags.UniformBuffer|VkBufferUsageFlags.TransferDst, size);

                using (HostBuffer stagging = new HostBuffer (dev, VkBufferUsageFlags.TransferSrc, size, materials.Data)) {
                    CommandBuffer cmd = cmdPool.AllocateCommandBuffer ();

                    cmd.Start ();
                    stagging.CopyTo (cmd, uboMaterials);
                    cmd.End ();

                    transferQ.Submit (cmd);

                    dev.WaitIdle ();
                    cmd.Destroy ();
                }
            }
        }


        public void DrawAll (CommandBuffer cmd) {
            cmd.BindVertexBuffer (vbo);
            cmd.BindIndexBuffer (ibo, VkIndexType.Uint16);

            foreach (string mesh in primitives.Keys) {
                Console.WriteLine ("drawing " + mesh);
                foreach (Primitive p in primitives[mesh]) {
                    cmd.DrawIndexed (p.indexCount, 1, p.indexBase, p.vertexBase, 0);
                }
            }
        }

        public void Destroy () {
            foreach (Image txt in textures) {
                txt.Dispose ();
            }
            uboMaterials?.Dispose ();
        }
    }
}
