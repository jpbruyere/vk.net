// Copyright (c) 2017 Eric Mellino
// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Runtime.InteropServices;

namespace Vulkan {

	public partial struct VkClearValue {
		public VkClearValue (float r, float g, float b) {
			depthStencil = default (VkClearDepthStencilValue);
			color = new VkClearColorValue (r, g, b);
		}
	}

	[StructLayout(LayoutKind.Explicit, Pack = 1, CharSet = CharSet.Ansi, Size = 16)]
    public struct VkClearColorValue {
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct _Float32 {
			public float r;
			public float g;
			public float b;
			public float a;
		}
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct _Int32 {
			public int r;
			public int g;
			public int b;
			public int a;
		}
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct _UInt32 {
			public uint r;
			public uint g;
			public uint b;
			public uint a;
		}
        [FieldOffset(0)]
		public _Float32 float32;
        [FieldOffset(0)]
		public _Int32 int32;
        [FieldOffset(0)]
		public _UInt32 uint32;
		public VkClearColorValue (float r, float g, float b, float a = 1.0f) : this () {
			float32.r = r;
			float32.g = g;
			float32.b = b;
			float32.a = a;
		}

		public VkClearColorValue (int r, int g, int b, int a = 255) : this () {
			int32.r = r;
			int32.g = g;
			int32.b = b;
			int32.a = a;
		}

		public VkClearColorValue (uint r, uint g, uint b, uint a = 255) : this () {
			uint32.r = r;
			uint32.g = g;
			uint32.b = b;
			uint32.a = a;
		}
    }

	/*[StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct VkClearColorValue {
        [FieldOffset(0)]public float float32_r;
        [FieldOffset(1)]public float float32_g;
        [FieldOffset(2)]public float float32_b;
        [FieldOffset(3)]public float float32_a;
        [FieldOffset(0)]public int int32_r;
        [FieldOffset(1)]public int int32_g;
        [FieldOffset(2)]public int int32_b;
        [FieldOffset(3)]public int int32_a;
        [FieldOffset(0)]public uint uint32_r;
        [FieldOffset(1)]public uint uint32_g;
        [FieldOffset(2)]public uint uint32_b;
        [FieldOffset(3)]public uint uint32_a;
		public float[] ToFloats => new float[] {float32_r, float32_g, float32_b, float32_a};
		public float[] ToInts => new float[] {int32_r, int32_g, int32_b, int32_a};
		public float[] ToUInts => new float[] {uint32_r, uint32_g, uint32_b, uint32_a};
		public VkClearColorValue (float r, float g, float b, float a = 1.0f) : this () {
			float32_r = r;
			float32_g = g;
			float32_b = b;
			float32_a = a;
		}

		public VkClearColorValue (int r, int g, int b, int a = 255) : this () {
			int32_r = r;
			int32_g = g;
			int32_b = b;
			int32_a = a;
		}

		public VkClearColorValue (uint r, uint g, uint b, uint a = 255) : this () {
			uint32_r = r;
			uint32_g = g;
			uint32_b = b;
			uint32_a = a;
		}
    }*/
    public partial struct VkClearDepthStencilValue
    {
        public VkClearDepthStencilValue(float depth, uint stencil)
        {
            this.depth = depth;
            this.stencil = stencil;
        }
    }
	public partial struct VkOffset3D {
		public VkOffset3D (int _x = 0, int _y = 0, int _z = 0) {
			x = _x; y = _y; z = _z;
		}
	}
	public partial struct VkImageSubresourceLayers {
		public VkImageSubresourceLayers (VkImageAspectFlags _aspectMask = VkImageAspectFlags.Color,
			uint _layerCount = 1, uint _mipLevel = 0, uint _baseArrayLayer = 0) {
			aspectMask = _aspectMask;
			layerCount = _layerCount;
			mipLevel = _mipLevel;
			baseArrayLayer = _baseArrayLayer;
		}
	}
	public partial struct VkImageSubresourceRange {
		public VkImageSubresourceRange (
			VkImageAspectFlags aspectMask,
			uint baseMipLevel = 0, uint levelCount = 1,
			uint baseArrayLayer = 0, uint layerCount = 1) {
			this.aspectMask = aspectMask;
			this.baseMipLevel = baseMipLevel;
			this.levelCount = levelCount;
			this.baseArrayLayer = baseArrayLayer;
			this.layerCount = layerCount;
		}
	}
	public partial struct VkDescriptorSetLayoutBinding {
		public VkDescriptorSetLayoutBinding (uint _binding, VkShaderStageFlags _stageFlags, VkDescriptorType _descriptorType, uint _descriptorCount = 1) {
			binding = _binding;
			this._descriptorType = (int)_descriptorType;
			descriptorCount = _descriptorCount;
			stageFlags = _stageFlags;
			pImmutableSamplers = (IntPtr)0;
		}
	}
	public partial struct VkDescriptorSetLayoutCreateInfo {
		public VkDescriptorSetLayoutCreateInfo (VkDescriptorSetLayoutCreateFlags flags, uint bindingCount, IntPtr pBindings) {
			this._sType = (int)VkStructureType.DescriptorSetLayoutCreateInfo;
			pNext = IntPtr.Zero;
			this.flags = flags;
			this.bindingCount = bindingCount;
			this.pBindings = pBindings;
		}
	}
	public partial struct VkVertexInputBindingDescription {
		public VkVertexInputBindingDescription (uint _binding, uint _stride, VkVertexInputRate _inputRate = VkVertexInputRate.Vertex) {
			binding = _binding;
			stride = _stride;
			this._inputRate = (int)_inputRate;
		}
	}

	public partial struct VkVertexInputAttributeDescription {
		public VkVertexInputAttributeDescription (uint _binding, uint _location, VkFormat _format, uint _offset = 0) {
			location = _location;
			binding = _binding;
			this._format = (int)_format;
			offset = _offset;
		}
		public VkVertexInputAttributeDescription (uint _location, VkFormat _format, uint _offset = 0) {
			location = _location;
			binding = 0;
			this._format = (int)_format;
			offset = _offset;
		}
	}
	public partial struct VkPipelineColorBlendAttachmentState {
		public VkPipelineColorBlendAttachmentState (VkBool32 blendEnable,
			VkBlendFactor srcColorBlendFactor = VkBlendFactor.SrcAlpha,
			VkBlendFactor dstColorBlendFactor = VkBlendFactor.OneMinusSrcAlpha,
			VkBlendOp colorBlendOp = VkBlendOp.Add,
			VkBlendFactor srcAlphaBlendFactor = VkBlendFactor.OneMinusSrcAlpha,
			VkBlendFactor dstAlphaBlendFactor = VkBlendFactor.Zero,
			VkBlendOp alphaBlendOp = VkBlendOp.Add,
			VkColorComponentFlags colorWriteMask = VkColorComponentFlags.R | VkColorComponentFlags.G | VkColorComponentFlags.B | VkColorComponentFlags.A) {
			this.blendEnable = blendEnable;
			this._srcColorBlendFactor = (int)srcColorBlendFactor;
			this._dstColorBlendFactor = (int)dstColorBlendFactor;
			this._colorBlendOp = (int)colorBlendOp;
			this._srcAlphaBlendFactor = (int)srcAlphaBlendFactor;
			this._dstAlphaBlendFactor = (int)dstAlphaBlendFactor;
			this._alphaBlendOp = (int)alphaBlendOp;
			this.colorWriteMask = colorWriteMask;
		}
	}
	public partial struct VkPushConstantRange {
		public VkPushConstantRange (VkShaderStageFlags stageFlags, uint size, uint offset = 0) {
			this.stageFlags = stageFlags;
			this.size = size;
			this.offset = offset;
		}
	}
	public partial struct VkCommandBufferBeginInfo {
		public VkCommandBufferBeginInfo (VkCommandBufferUsageFlags usage = (VkCommandBufferUsageFlags)0) {
			this._sType = (int)VkStructureType.CommandBufferBeginInfo;
			pNext = pInheritanceInfo = IntPtr.Zero;
			flags = usage;
		}
	}
	public partial struct VkQueryPoolCreateInfo {
		public static VkQueryPoolCreateInfo New (VkQueryType queryType,
			VkQueryPipelineStatisticFlags statisticFlags = (VkQueryPipelineStatisticFlags)0, uint count = 1) {
			VkQueryPoolCreateInfo ret = new VkQueryPoolCreateInfo ();
			ret.sType = VkStructureType.QueryPoolCreateInfo;
			ret.pipelineStatistics = statisticFlags;
			ret.queryType = queryType;
			ret.queryCount = count;
			return ret;
		}
	}
	public partial struct VkDebugMarkerObjectNameInfoEXT {
		public VkDebugMarkerObjectNameInfoEXT (VkDebugReportObjectTypeEXT objectType, ulong handle) {
			this._sType = (int)VkStructureType.DebugMarkerObjectNameInfoEXT;
			pNext = IntPtr.Zero;
			this._objectType = (int)objectType;
			_object = handle;
			pObjectName = IntPtr.Zero;
		}
	}
	public partial struct VkDebugUtilsObjectNameInfoEXT {
		public VkDebugUtilsObjectNameInfoEXT(VkObjectType objectType, ulong handle)
		{
			this._sType = (int)VkStructureType.DebugUtilsObjectNameInfoEXT;
			pNext = IntPtr.Zero;
			this._objectType = (int)objectType;
			objectHandle = handle;
			pObjectName = IntPtr.Zero;
		}
	}
	public partial struct VkDescriptorPoolSize {
		public VkDescriptorPoolSize (VkDescriptorType descriptorType, uint count = 1) {
			this._type = (int)descriptorType;
			descriptorCount = count;
		}
	}
    public unsafe partial struct VkAttachmentReference {
		public VkAttachmentReference (uint attachment, VkImageLayout layout) {
			this.attachment = attachment;
			this._layout = (int)layout;
		}
    }
	/// <summary>
	/// 3x4 row-major affine transformation matrix.
	/// </summary>
	public unsafe partial struct VkTransformMatrixKHR
	{
		public float this [int row, int column] {
			get {
				fixed (float* tmp = _matrix)
					return tmp[row * 3 + column];
            }
			set {
				fixed (float* tmp = _matrix)
					tmp[row * 3 + column] = value;
			}
		}
	}
}

