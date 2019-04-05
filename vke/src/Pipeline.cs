//
// FrameBuffer.cs
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
using System.Runtime.InteropServices;
using Vulkan;
using static Vulkan.VulkanNative;

namespace VKE {
    public class Pipeline : Activable {
        internal VkPipeline handle;        
		VkPipelineBindPoint bindPoint = VkPipelineBindPoint.Graphics;

        VkPipelineInputAssemblyStateCreateInfo inputAssemblyState = VkPipelineInputAssemblyStateCreateInfo.New ();
        VkPipelineRasterizationStateCreateInfo rasterizationState = VkPipelineRasterizationStateCreateInfo.New ();
        VkPipelineViewportStateCreateInfo viewportState = VkPipelineViewportStateCreateInfo.New ();
        public VkPipelineDepthStencilStateCreateInfo depthStencilState = VkPipelineDepthStencilStateCreateInfo.New ();
        public VkPipelineMultisampleStateCreateInfo multisampleState = VkPipelineMultisampleStateCreateInfo.New ();

        List<VkPipelineColorBlendAttachmentState> blendAttachments = new List<VkPipelineColorBlendAttachmentState>();
        List<VkDynamicState> dynamicStates = new List<VkDynamicState> ();
        public List<VkVertexInputBindingDescription> vertexBindings = new List<VkVertexInputBindingDescription> ();
        public List<VkVertexInputAttributeDescription> vertexAttributes = new List<VkVertexInputAttributeDescription> ();
        public List<ShaderInfo> shaders = new List<ShaderInfo>();

        public PipelineLayout Layout;
        
		public VkSampleCountFlags Samples {
			get { return multisampleState.rasterizationSamples; }
		}

		public RenderPass RenderPass { get; private set; }

		public void SetVertexAttributes (uint binding, params VkFormat[] attribsDesc) {
			uint offset = 0;

			for (uint i = 0; i < attribsDesc.Length; i++) {
				vertexAttributes.Add (new VkVertexInputAttributeDescription (binding, i, attribsDesc[i], offset));
				VkFormatSize fs;
				Utils.vkGetFormatSize (attribsDesc[i], out fs);
				offset += fs.blockSizeInBits/8;
			}
		}
		public void AddVertexBinding (uint binding, uint stride, VkVertexInputRate inputRate = VkVertexInputRate.Vertex) { 
			vertexBindings.Add (new VkVertexInputBindingDescription (binding, stride, inputRate));
		}
		public void AddVertexBinding<T> (uint binding, VkVertexInputRate inputRate = VkVertexInputRate.Vertex) { 
			vertexBindings.Add (new VkVertexInputBindingDescription (binding, (uint)Marshal.SizeOf<T> (), inputRate));
		}
		public void AddShader (VkShaderStageFlags _stageFlags, string _spirvPath, string _entryPoint = "main") {
			shaders.Add (new ShaderInfo (_stageFlags, _spirvPath, _entryPoint));
		}

		#region CTORS
		/// <summary>
		/// Create a new Pipeline with supplied RenderPass
		/// </summary>
		public Pipeline (Device dev, RenderPass renderPass, VkPrimitiveTopology topology = VkPrimitiveTopology.TriangleList,
			VkSampleCountFlags samples = VkSampleCountFlags.Count1) : base (dev)
		{

			RenderPass = renderPass;

			inputAssemblyState.topology = topology;

            rasterizationState.polygonMode = VkPolygonMode.Fill;
            rasterizationState.cullMode = (uint)VkCullModeFlags.None;
            rasterizationState.frontFace = VkFrontFace.CounterClockwise;
            rasterizationState.depthClampEnable = False;
            rasterizationState.rasterizerDiscardEnable = False;
            rasterizationState.depthBiasEnable = False;
            rasterizationState.lineWidth = 1.0f;

            viewportState.viewportCount = 1;
            viewportState.scissorCount = 1;

            blendAttachments.Add (new VkPipelineColorBlendAttachmentState (false));

            dynamicStates.Add (VkDynamicState.Viewport);
            dynamicStates.Add (VkDynamicState.Scissor);

            depthStencilState.depthTestEnable = True;
            depthStencilState.depthWriteEnable = True;
            depthStencilState.depthCompareOp = VkCompareOp.LessOrEqual;
            depthStencilState.depthBoundsTestEnable = False;
            depthStencilState.back.failOp = VkStencilOp.Keep;
            depthStencilState.back.passOp = VkStencilOp.Keep;
            depthStencilState.back.compareOp = VkCompareOp.Always;
            depthStencilState.stencilTestEnable = False;
            depthStencilState.front = depthStencilState.back;

            multisampleState.rasterizationSamples = samples;
		}

		/// <summary>
		/// Create a new pipeline and the default renderpass for it
		/// </summary>
		public Pipeline (Device dev, VkFormat colorFormat, VkFormat depthFormat, 
			VkPrimitiveTopology topology = VkPrimitiveTopology.TriangleList,
			VkSampleCountFlags samples = VkSampleCountFlags.Count1)
			: this (dev, new RenderPass (dev, colorFormat, depthFormat, samples), topology,samples)
		{
        }
		#endregion

        public override void Activate () {
			if (state != ActivableState.Activated) {
				Layout.Activate ();
				RenderPass.Activate ();

				List<VkPipelineShaderStageCreateInfo> shaderStages = new List<VkPipelineShaderStageCreateInfo> ();
				foreach (ShaderInfo shader in shaders)
					shaderStages.Add (shader.GetStageCreateInfo(dev));

				VkPipelineColorBlendStateCreateInfo colorBlendInfo = VkPipelineColorBlendStateCreateInfo.New ();
				colorBlendInfo.attachmentCount = (uint)blendAttachments.Count;
				colorBlendInfo.pAttachments = blendAttachments.Pin ();

				VkPipelineDynamicStateCreateInfo dynStatesInfo = VkPipelineDynamicStateCreateInfo.New ();
				dynStatesInfo.dynamicStateCount = (uint)dynamicStates.Count;
				dynStatesInfo.pDynamicStates = dynamicStates.Pin ();

				VkPipelineVertexInputStateCreateInfo vertInputInfo = VkPipelineVertexInputStateCreateInfo.New ();
				vertInputInfo.vertexBindingDescriptionCount = (uint)vertexBindings.Count;
				vertInputInfo.pVertexBindingDescriptions = vertexBindings.Pin ();
				vertInputInfo.vertexAttributeDescriptionCount = (uint)vertexAttributes.Count;
				vertInputInfo.pVertexAttributeDescriptions = vertexAttributes.Pin ();

				VkGraphicsPipelineCreateInfo info = VkGraphicsPipelineCreateInfo.New ();
				info.renderPass 			= RenderPass.handle;
				info.layout					= Layout.handle;
				info.pVertexInputState 		= vertInputInfo.Pin ();
				info.pInputAssemblyState 	= inputAssemblyState.Pin ();
				info.pRasterizationState 	= rasterizationState.Pin ();
				info.pColorBlendState 		= colorBlendInfo.Pin ();
				info.pMultisampleState 		= multisampleState.Pin ();
				info.pViewportState 		= viewportState.Pin ();
				info.pDepthStencilState 	= depthStencilState.Pin ();
				info.pDynamicState 			= dynStatesInfo.Pin ();
				info.stageCount 			= (uint)shaders.Count;
				info.pStages 				= shaderStages.Pin ();

				Utils.CheckResult (vkCreateGraphicsPipelines (dev.VkDev, VkPipelineCache.Null, 1, ref info, IntPtr.Zero, out handle));

				for (int i = 0; i < shaders.Count; i++)
					dev.DestroyShaderModule (shaderStages[i].module);

				vertInputInfo.Unpin ();
				inputAssemblyState.Unpin ();
				rasterizationState.Unpin ();
				colorBlendInfo.Unpin ();
				multisampleState.Unpin ();
				viewportState.Unpin ();
				depthStencilState.Unpin ();
				dynStatesInfo.Unpin ();
				shaderStages.Unpin ();

				vertexAttributes.Unpin ();
				vertexBindings.Unpin ();
				dynamicStates.Unpin ();
				blendAttachments.Unpin ();
			}
			base.Activate ();
        }
        
		public void Bind (CommandBuffer cmd) {
            vkCmdBindPipeline (cmd.Handle, bindPoint, handle);
        }

		#region IDisposable Support
		protected override void Dispose (bool disposing) {
			if (disposing) { 
				RenderPass.Dispose ();
				Layout.Dispose ();
			}else
				System.Diagnostics.Debug.WriteLine ("VKE Activable object disposed by finalizer");
			if (state == ActivableState.Activated)
				VulkanNative.vkDestroyPipeline (dev.VkDev, handle, IntPtr.Zero);
			base.Dispose (disposing);
		}
		#endregion
	}
}
