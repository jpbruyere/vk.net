// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

using VK;
using System.Collections.Generic;

namespace CVKL.glTF {
	//TODO:stride in buffer views?
	public abstract class PbrModel : Model {        
		//public new struct Vertex {
		//	[VertexAttribute (VertexAttributeType.Position, VkFormat.R32g32b32Sfloat)]
		//	public Vector3 pos;
		//	[VertexAttribute (VertexAttributeType.Normal, VkFormat.R32g32b32Sfloat)]
		//	public Vector3 normal;
		//	[VertexAttribute (VertexAttributeType.UVs, VkFormat.R32g32Sfloat)]
		//	public Vector2 uv0;
		//	[VertexAttribute (VertexAttributeType.UVs, VkFormat.R32g32Sfloat)]
		//	public Vector2 uv1;
		//	public override string ToString () {
		//		return pos.ToString () + ";" + normal.ToString () + ";" + uv0.ToString () + ";" + uv1.ToString ();
		//	}
		//};

		protected DescriptorPool descriptorPool;
		public GPUBuffer vbo;
		public GPUBuffer ibo;
		public Buffer materialUBO;

		protected PbrModel () { }
		public PbrModel (Queue transferQ, string path) {
			dev = transferQ.Dev;
			using (CommandPool cmdPool = new CommandPool (dev, transferQ.index)) {
				using (glTFLoader ctx = new glTFLoader (path, transferQ, cmdPool)) {
					loadSolids (ctx);
				}
			}
		}

		protected void loadSolids (glTFLoader ctx) {
			loadSolids<Vertex> (ctx);
		}

		protected void loadSolids<VX> (glTFLoader ctx) {
			ulong vertexCount, indexCount;

			ctx.GetVertexCount (out vertexCount, out indexCount, out IndexBufferType);

			ulong vertSize = vertexCount * (ulong)Marshal.SizeOf<VX> ();
			ulong idxSize = indexCount * (IndexBufferType == VkIndexType.Uint16 ? 2ul : 4ul);
			ulong size = vertSize + idxSize;

			vbo = new GPUBuffer (dev, VkBufferUsageFlags.VertexBuffer | VkBufferUsageFlags.TransferDst, vertSize);
			ibo = new GPUBuffer (dev, VkBufferUsageFlags.IndexBuffer | VkBufferUsageFlags.TransferDst, idxSize);

			vbo.SetName ("vbo gltf");
			ibo.SetName ("ibo gltf");

			Meshes = new List<Mesh> (ctx.LoadMeshes<VX> (IndexBufferType, vbo, 0, ibo, 0));
			Scenes = new List<Scene> (ctx.LoadScenes (out defaultSceneIndex));
		}

		/// <summary> bind vertex and index buffers </summary>
		public virtual void Bind (CommandBuffer cmd) {
			cmd.BindVertexBuffer (vbo);
			cmd.BindIndexBuffer (ibo, IndexBufferType);
		}

		//TODO:destset for binding must be variable
		//TODO: ADD REFAULT MAT IF NO MAT DEFINED
		public abstract void RenderNode (CommandBuffer cmd, PipelineLayout pipelineLayout, Node node, Matrix4x4 currentTransform, bool shadowPass = false);

		public void DrawAll (CommandBuffer cmd, PipelineLayout pipelineLayout, bool shadowPass = false) {
			foreach (Scene sc in Scenes) {
				foreach (Node node in sc.Root.Children)
					RenderNode (cmd, pipelineLayout, node, sc.Root.localMatrix, shadowPass);				
			}
		}
        public void Draw(CommandBuffer cmd, PipelineLayout pipelineLayout, Scene scene, bool shadowPass = false)
        {
            if (scene.Root == null)
                return;
            RenderNode(cmd, pipelineLayout, scene.Root, Matrix4x4.Identity, shadowPass);
        }

        public void Draw (CommandBuffer cmd, PipelineLayout pipelineLayout, Buffer instanceBuf, bool shadowPass = false, params InstancedCmd[] instances) {
            cmd.BindVertexBuffer(instanceBuf, 1);
            uint firstInstance = 0;
            for (int i = 0; i < instances.Length; i++)
            {
                foreach (Primitive p in Meshes[instances[i].meshIdx].Primitives)
                {
                    if (!shadowPass)
                        cmd.PushConstant(pipelineLayout, VkShaderStageFlags.Fragment, (int)p.material, (uint)Marshal.SizeOf<Matrix4x4>());
                    cmd.DrawIndexed(p.indexCount, instances[i].count, p.indexBase, p.vertexBase, firstInstance);
                }
                firstInstance += instances[i].count;
            }
        }

        #region IDisposable Support
        protected bool isDisposed;

		protected virtual void Dispose (bool disposing) {
			if (!isDisposed) {
				if (disposing) {
					ibo?.Dispose ();
					vbo?.Dispose ();
					materialUBO?.Dispose ();
					descriptorPool?.Dispose ();
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
