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

namespace VKE {
    public static class Stb {
        public const string stblib = "stb";

        [DllImport (stblib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load")]
        public static extern IntPtr Load ([MarshalAs (UnmanagedType.LPStr)] string filename, out int x, out int y, out int channels_in_file, int desired_channels);

        [DllImport (stblib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_image_free")]
        public static extern void FreeImage (IntPtr img);
    }

    public class Model {
        Device dev;
        Queue transferQ;
        CommandPool cmdPool;

        UInt32 textureSize = 1024; //texture array size w/h


        public List<VKE.Image> textures = new List<Image>();

        public struct Vertex {
            public Vector3 pos;
            public Vector3 normal;
            public Vector3 uv;
        };

        [StructLayout (LayoutKind.Explicit, Size = 80)]
        public struct Material {
            [FieldOffset (0)] Vector4 baseColorFactor;
            [FieldOffset (16)] AlphaMode alphaMode;
            [FieldOffset (20)] float alphaCutoff;
            [FieldOffset (24)] float metallicFactor;
            [FieldOffset (28)] float roughnessFactor;

            [FieldOffset (32)] UInt32 baseColorTexture;
            [FieldOffset (36)] UInt32 metallicRoughnessTexture;
            [FieldOffset (40)] UInt32 normalTexture;
            [FieldOffset (44)] UInt32 occlusionTexture;

            [FieldOffset (48)] UInt32 emissiveTexture;

            [FieldOffset (64)] Vector4 emissiveFactor;

            public Material (
                UInt32 _baseColorTexture = 0, UInt32 _metallicRoughnessTexture = 0, UInt32 _normalTexture = 0, UInt32 _occlusionTexture = 0) {
                baseColorFactor = new Vector4 (1f);
                alphaMode = AlphaMode.ALPHAMODE_OPAQUE;
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
            ALPHAMODE_OPAQUE,
            ALPHAMODE_MASK,
            ALPHAMODE_BLEND
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

        struct Primitive {
            public string name;
            public UInt32 indexBase;
            public UInt32 indexCount;
            public UInt32 vertexBase;
            public UInt32 vertexCount;
            public UInt32 material;
            public Dimensions dims;
        }

        struct InstanceData {
            UInt32 materialIndex;
            Matrix4x4 modelMat;
            Vector4 color;
        };

        string baseDirectory;

        public Model (Device device, Queue _transferQ, CommandPool _cmdPool, string path) {
            dev = device;
            transferQ = _transferQ;
            cmdPool = _cmdPool;
            baseDirectory = System.IO.Path.GetDirectoryName (path);
            Load (path);
        }

        void Load (string path) {

            GL.Gltf gltf = Interface.LoadModel (path);
            loadImages (gltf);
            loadMaterial (gltf);

            foreach (GL.Buffer buf in gltf.Buffers) {
                Console.WriteLine ("buffer: {0}", buf.Uri);

            }

        }
        void loadImages (GL.Gltf gltf) {        
            HostBuffer stagging;

            foreach (GL.Image img in gltf.Images) {
                Console.WriteLine ("loading image {0} : {1} : {2}", img.Name, img.MimeType, img.Uri);

                int width, height, channels;
                IntPtr imgPtr = Stb.Load (System.IO.Path.Combine (baseDirectory, img.Uri), out width, out height, out channels, 4);
                long size = width * height * 4;

                stagging = new HostBuffer (dev, VkBufferUsageFlags.TransferSrc, (UInt64)size);

                stagging.Map ((ulong)size);
                unsafe {
                    System.Buffer.MemoryCopy (imgPtr.ToPointer(), stagging.MappedData.ToPointer (), size, size);
                }
                stagging.Unmap ();

                Stb.FreeImage (imgPtr);

                VKE.Image vkimg = new Image (dev, VkFormat.R8g8b8a8Unorm, VkImageUsageFlags.TransferDst | VkImageUsageFlags.Sampled,
                    VkMemoryPropertyFlags.DeviceLocal, (uint) width, (uint)height);

                CommandBuffer cmd = cmdPool.AllocateCommandBuffer ();
                cmd.Start ();

                stagging.CopyToImage (cmd, vkimg);

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

        void loadMaterial (GL.Gltf gltf) {
            foreach (GL.Material mat in gltf.Materials) {
                Console.WriteLine ("loading material: " + mat.Name);

            }
        }

        public void Destroy () {
            foreach (Image txt in textures) {
                txt.Dispose ();
            }
        }
    }
}
