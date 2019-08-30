// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System.Numerics;
using System.Runtime.InteropServices;

using VK;

namespace CVKL {
	using CVKL.glTF;


	//TODO:stride in buffer views?
	public class PbrModel2 : PbrModelSeparatedTextures {
		public PbrModel2 (Queue transferQ, string path, DescriptorSetLayout layout, params AttachmentType[] attachments)
		: base (transferQ, path, layout, attachments) {}

		//TODO:destset for binding must be variable
		//TODO: ADD REFAULT MAT IF NO MAT DEFINED
		public override void RenderNode (CommandBuffer cmd, PipelineLayout pipelineLayout, Node node, Matrix4x4 currentTransform, bool shadowPass = false) {
			Matrix4x4 localMat = node.localMatrix * currentTransform;

			cmd.PushConstant (pipelineLayout, VkShaderStageFlags.Vertex, localMat);

			if (node.Mesh != null) {
				foreach (Primitive p in node.Mesh.Primitives) {
					cmd.PushConstant (pipelineLayout, VkShaderStageFlags.Fragment, (int)p.material, (uint)Marshal.SizeOf<Matrix4x4> ());
					if (descriptorSets[p.material] != null)
						cmd.BindDescriptorSet (pipelineLayout, descriptorSets[p.material], 1);
					cmd.DrawIndexed (p.indexCount, 1, p.indexBase, p.vertexBase, 0);
				}
			}
			if (node.Children == null)
				return;
			foreach (Node child in node.Children)
				RenderNode (cmd, pipelineLayout, child, localMat);
		}
	}
}
