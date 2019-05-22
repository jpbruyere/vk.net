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
using System.Collections.Generic;
using System.Numerics;
using VK;



namespace CVKL {

	public enum AlphaMode : UInt32 {
		Opaque,
		Mask,
		Blend
	};
	//TODO:stride in buffer views?
	public class Model {
		public struct Vertex {
			[VertexAttribute (VertexAttributeType.Position, VkFormat.R32g32b32Sfloat)]
			public Vector3 pos;
			[VertexAttribute (VertexAttributeType.Normal, VkFormat.R32g32b32Sfloat)]
			public Vector3 normal;
			[VertexAttribute (VertexAttributeType.UVs, VkFormat.R32g32Sfloat)]
			public Vector2 uv;
			public override string ToString () {
				return pos.ToString () + ";" + normal.ToString () + ";" + uv.ToString ();
			}
		};

		public struct Dimensions {
			public Vector3 min;
			public Vector3 max;

			public Dimensions (Vector3 _min, Vector3 _max) {
				min = _min;
				max = _max;
			}
		}
        
        public class InstancedCmd
        {
            public int meshIdx;
            public uint count;
        }
        
        public class Primitive {
			public string name;
			public UInt32 indexBase;
			public Int32 vertexBase;
			public UInt32 vertexCount;
			public UInt32 indexCount;
			public UInt32 material;
			public BoundingBox bb;
		}

		public class Mesh {
			public string Name;
			public List<Primitive> Primitives = new List<Primitive> ();
			public BoundingBox bb;

			/// <summary>
			/// add primitive and update mesh bounding box
			/// </summary>
			public void AddPrimitive (Primitive p) {
				if (Primitives.Count == 0)
					bb = p.bb;
				else
					bb += p.bb;
				Primitives.Add (p);
			}
		}

		public class Scene {
			public string Name;
			public Node Root;
			public List<Node> GetNodes () => Root?.Children;
            public Node FindNode(string name) => Root == null ? null : Root.FindNode (name);

            public BoundingBox AABB => Root.GetAABB (Matrix4x4.Identity);
		}

		public class Node {
            public string Name;
			public Node Parent;
			public List<Node> Children;
			public Matrix4x4 localMatrix;
			public Mesh Mesh;

            public Node FindNode(string name)
            {
                if (Name == name)
                    return this;
                if (Children == null)
                    return null;
                foreach (Node child in Children) {
                    Node n = child.FindNode(name);
                    if (n != null)
                        return n;
                }
                return null;
            }

            public Matrix4x4 Matrix {
				get { return Parent == null ? localMatrix : Parent.Matrix * localMatrix; }
			}

			public BoundingBox GetAABB (Matrix4x4 currentTransform) {
				Matrix4x4 curTransform = localMatrix * currentTransform;
				BoundingBox aabb = new BoundingBox();

				if (Mesh != null)
					aabb = Mesh.bb.getAABB (curTransform);

				if (Children != null) {
					for (int i = 0; i < Children.Count; i++) 
						aabb += Children[i].GetAABB (curTransform);
				}
				return aabb;
			}
		}

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
		protected Device dev;

		public VkIndexType IndexBufferType;

		protected int defaultSceneIndex;
		public Scene DefaultScene => Scenes[defaultSceneIndex];
		public List<Mesh> Meshes;
		public List<Scene> Scenes;

        public Node FindNode(string name) {
            foreach (Scene scene in Scenes)
            {
                Node n = scene.FindNode(name);
                if (n != null)
                    return n;
            }
            return null;
        }

        public Dimensions dimensions = new Dimensions (new Vector3 (float.MaxValue), new Vector3 (float.MinValue));
	}
}
