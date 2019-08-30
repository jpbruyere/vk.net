// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;

namespace VK {
	public enum VertexAttributeType {
		Position,
		Normal,
		UVs,
		Tangent,
	}
	[Flags]
	public enum AttachmentType : UInt32 {
		None,
		Color = 0x1,
		Normal = 0x2,
		AmbientOcclusion = 0x4,
		Metal = 0x8,
		Roughness = 0x10,
		PhysicalProps = 0x20,
		Emissive = 0x40,
	};

	public class VertexAttributeAttribute : Attribute {
		public VkFormat Format;
		public VertexAttributeType Type;

		public VertexAttributeAttribute (VertexAttributeType type, VkFormat format)  {
			Format = format;
			Type = type;
		}
	}
	public class AttachmentAttribute : Attribute {
		public AttachmentType AttachmentType;
		public Type DataType;
		public AttachmentAttribute (AttachmentType attachmentType, Type dataType) {
			AttachmentType = attachmentType;
			DataType = dataType;
		}
	}
}
