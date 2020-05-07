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

	public unsafe partial struct VkClearColorValue {
		public VkClearColorValue (float r, float g, float b, float a = 1.0f) : this () {
			fixed (float* tmp = float32) {
				tmp[0] = r;
				tmp[1] = g;
				tmp[2] = b;
				tmp[3] = a;
			}
		}

		public VkClearColorValue (int r, int g, int b, int a = 255) : this () {
			fixed (int* tmp = int32) {
				tmp[0] = r;
				tmp[1] = g;
				tmp[2] = b;
				tmp[3] = a;
			}
		}

		public VkClearColorValue (uint r, uint g, uint b, uint a = 255) : this () {
			fixed (uint* tmp = uint32) {
				tmp[0] = r;
				tmp[1] = g;
				tmp[2] = b;
				tmp[3] = a;
			}
		}
	}
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
	/// 3X4 ROW Major affine transformation matrix
	/// </summary>
	[StructLayout (LayoutKind.Sequential)]
	public struct VkTransformMatrixKHR {
		[MarshalAs (UnmanagedType.ByValArray, SizeConst = 48)] float[] values;
		public float this[int row, int col] {
			get => values[row * 3 + col];
			set => values[row * 3 + col] = value;
		}
	}
}

