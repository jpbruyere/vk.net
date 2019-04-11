//
// PipelineConfig.cs
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
using VK;
using static VK.Vk;

namespace VKE {
    public class PipelineConfig {
        public PipelineLayout Layout;
		public RenderPass RenderPass;

		public VkPipelineBindPoint bindPoint = VkPipelineBindPoint.Graphics;
        public VkPipelineInputAssemblyStateCreateInfo inputAssemblyState = VkPipelineInputAssemblyStateCreateInfo.New();
        public VkPipelineRasterizationStateCreateInfo rasterizationState = VkPipelineRasterizationStateCreateInfo.New();
        public VkPipelineViewportStateCreateInfo viewportState = VkPipelineViewportStateCreateInfo.New();
        public VkPipelineDepthStencilStateCreateInfo depthStencilState = VkPipelineDepthStencilStateCreateInfo.New();
        public VkPipelineMultisampleStateCreateInfo multisampleState = VkPipelineMultisampleStateCreateInfo.New();
        public List<VkPipelineColorBlendAttachmentState> blendAttachments = new List<VkPipelineColorBlendAttachmentState>();
        public List<VkDynamicState> dynamicStates = new List<VkDynamicState> ();
        public List<VkVertexInputBindingDescription> vertexBindings = new List<VkVertexInputBindingDescription> ();
        public List<VkVertexInputAttributeDescription> vertexAttributes = new List<VkVertexInputAttributeDescription> ();
        public readonly List<ShaderInfo> shaders = new List<ShaderInfo>();
        
		public VkSampleCountFlags Samples {
			get { return multisampleState.rasterizationSamples; }
		}

		public PipelineConfig () {

		}

		public static PipelineConfig CreateDefault (VkPrimitiveTopology topology = VkPrimitiveTopology.TriangleList,
			VkSampleCountFlags samples = VkSampleCountFlags.SampleCount1)
		{
			PipelineConfig cfg = new PipelineConfig ();

			cfg.inputAssemblyState.topology = topology;
            cfg.multisampleState.rasterizationSamples = samples;

            cfg.rasterizationState.polygonMode = VkPolygonMode.Fill;
            cfg.rasterizationState.cullMode = (uint)VkCullModeFlags.None;
            cfg.rasterizationState.frontFace = VkFrontFace.CounterClockwise;
            cfg.rasterizationState.depthClampEnable = False;
            cfg.rasterizationState.rasterizerDiscardEnable = False;
            cfg.rasterizationState.depthBiasEnable = False;
            cfg.rasterizationState.lineWidth = 1.0f;

            cfg.viewportState.viewportCount = 1;
            cfg.viewportState.scissorCount = 1;

            cfg.blendAttachments.Add (new VkPipelineColorBlendAttachmentState (false));

            cfg.dynamicStates.Add (VkDynamicState.Viewport);
            cfg.dynamicStates.Add (VkDynamicState.Scissor);

            cfg.depthStencilState.depthTestEnable = True;
            cfg.depthStencilState.depthWriteEnable = True;
            cfg.depthStencilState.depthCompareOp = VkCompareOp.LessOrEqual;
            cfg.depthStencilState.depthBoundsTestEnable = False;
            cfg.depthStencilState.back.failOp = VkStencilOp.Keep;
            cfg.depthStencilState.back.passOp = VkStencilOp.Keep;
            cfg.depthStencilState.back.compareOp = VkCompareOp.Always;
            cfg.depthStencilState.stencilTestEnable = False;
            cfg.depthStencilState.front = cfg.depthStencilState.back;

			return cfg;
		}

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

		public void ResetShadersAndVerticesInfos () {
			foreach (ShaderInfo shader in shaders) 
				shader.Dispose ();
			vertexBindings.Clear ();
			vertexAttributes.Clear ();
			shaders.Clear ();
		}
	}
}
