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
	public struct BoundingBox {
		public Vector3 min;
		public Vector3 max;
		public bool isValid;
		public BoundingBox (Vector3 min, Vector3 max, bool isValid = false) {
			this.min = min;
			this.max = max;
			this.isValid = isValid;
		}
		public BoundingBox getAABB (Matrix4x4 m) {
			if (!isValid)
				return default (BoundingBox);
			Vector3 mini = new Vector3 (m.M41, m.M42, m.M43);
			Vector3 maxi = mini;
			Vector3 v0, v1;

			Vector3 right = new Vector3 (m.M11, m.M12, m.M13);
			v0 = right * this.min.X;
			v1 = right * this.max.X;
			mini += Vector3.Min (v0, v1);
			maxi += Vector3.Max (v0, v1);

			Vector3 up = new Vector3 (m.M21, m.M22, m.M23);
			v0 = up * this.min.Y;
			v1 = up * this.max.Y;
			mini += Vector3.Min (v0, v1);
			maxi += Vector3.Max (v0, v1);

			Vector3 back = new Vector3 (m.M31, m.M32, m.M33);
			v0 = back * this.min.Z;
			v1 = back * this.max.Z;
			mini += Vector3.Min (v0, v1);
			maxi += Vector3.Max (v0, v1);

			return new BoundingBox (mini, maxi, true);
		}

		public float Width => max.X - min.X;
		public float Height => max.Y - min.Y;
		public float Depth => max.Z - min.Z;

		public Vector3 Center => new Vector3 (Width / 2f + min.X, Height / 2f + min.Y, Depth / 2f + min.Z);

		public static BoundingBox operator +(BoundingBox bb1, BoundingBox bb2) {
			return bb1.isValid ? bb2.isValid ? new BoundingBox (Vector3.Min (bb1.min, bb2.min), Vector3.Min (bb1.max, bb2.max),true) : bb1 : bb2.isValid ? bb2 : default(BoundingBox);
		}
		public override string ToString () => isValid ? string.Format ($" {min}->{max}") : "Invalid";
	}


	public enum AlphaMode : UInt32 {
		Opaque,
		Mask,
		Blend
	};
	//TODO:stride in buffer views?
	public class Model {
		public struct Dimensions {
			public Vector3 min;
			public Vector3 max;

			public Dimensions (Vector3 _min, Vector3 _max) {
				min = _min;
				max = _max;
			}
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

		public struct InstanceData {
			public UInt32 materialIndex;
			public Matrix4x4 modelMat;
			public Vector4 color;
		};

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
		//public struct MaterialStruct {
		//	public UInt32 baseColorTexture;
		//	public UInt32 metallicRoughnessTexture;
		//	public UInt32 normalTexture;
		//	public UInt32 occlusionTexture;
		//	public UInt32 emissiveTexture;
		//}


		/// <summary>
		/// Material class with textures indices and a descriptorSet for those textures
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

			//struct Extension {
			//	vkglTF::Texture* specularGlossinessTexture;
			//	vkglTF::Texture* diffuseTexture;
			//	glm::vec4 diffuseFactor = glm::vec4 (1.0f);
			//	glm::vec3 specularFactor = glm::vec3 (0.0f);
			//}
			//extension;

			public Material (Int32 _baseColorTexture = -1, Int32 _metallicRoughnessTexture = -1,
            	Int32 _normalTexture = -1, Int32 _occlusionTexture = -1)
			{
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

		public struct Vertex {
			[VertexAttribute(VertexAttributeType.Position, VkFormat.R32g32b32Sfloat)]
			public Vector3 pos;
			[VertexAttribute(VertexAttributeType.Normal, VkFormat.R32g32b32Sfloat)]
            public Vector3 normal;
			[VertexAttribute(VertexAttributeType.UVs, VkFormat.R32g32Sfloat)]
            public Vector2 uv;
            public override string ToString () {
                return pos.ToString () + ";" + normal.ToString () + ";" + uv.ToString ();
            }
        };

		//void getSceneDimensions () {
		//	// Calculate binary volume hierarchy for all nodes in the scene
		//	for (auto node : linearNodes) {
		//		calculateBoundingBox (node, nullptr);
		//	}

		//	dimensions.min = glm::vec3 (FLT_MAX);
		//	dimensions.max = glm::vec3 (-FLT_MAX);

		//	for (auto node : linearNodes) {
		//		if (node->bvh.valid) {
		//			dimensions.min = glm::min (dimensions.min, node->bvh.min);
		//			dimensions.max = glm::max (dimensions.max, node->bvh.max);
		//		}
		//	}

		//	// Calculate scene aabb
		//	aabb = glm::scale (glm::mat4 (1.0f), glm::vec3 (dimensions.max[0] - dimensions.min[0], dimensions.max[1] - dimensions.min[1], dimensions.max[2] - dimensions.min[2]));
		//	aabb[3][0] = dimensions.min[0];
		//	aabb[3][1] = dimensions.min[1];
		//	aabb[3][2] = dimensions.min[2];
		//}

		//public void calculateBoundingBox (Node node, Node parent) {
		//	BoundingBox parentBvh = (parent != null) ? parent.bvh : new BoundingBox (dimensions.min, dimensions.max);

		//	if (node.Mesh != null) {
		//		if (node.Mesh.bb.valid) {
		//			node.aabb = node.Mesh.bb.getAABB (node.Matrix);
		//			if (node.Children == null) {
		//				node.bvh.min = node.aabb.min;
		//				node.bvh.max = node.aabb.max;
		//				node.bvh.valid = true;
		//			}
		//		}
		//	}

		//	parentBvh.min = Vector3.Min (parentBvh.min, node.bvh.min);
		//	parentBvh.max = Vector3.Min (parentBvh.max, node.bvh.max);

		//	if (node.Children == null)
		//		return;

		//	foreach (Node child in node.Children)
		//		calculateBoundingBox (child, node);
		//}
	}
}
