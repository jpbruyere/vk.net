// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using VK;

namespace CVKL {
	public interface IModelLoader {
		void GetVertexCount (out ulong vertexCount, out ulong indexCount, out VkIndexType largestIndexType);
		Model.Mesh[] LoadMeshes<TVertex> (VkIndexType indexType, Buffer vbo, ulong vboOffset, Buffer ibo, ulong iboOffset);
		Model.Scene[] LoadScenes (out int defaultScene);
		Image[] LoadImages ();
		//Material[] LoadMaterial ();
	}
}
