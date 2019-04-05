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
	public class ShaderInfo : IDisposable {
		public VkShaderStageFlags StageFlags;
		public string SpirvPath;
		public FixedUtf8String EntryPoint;

		public ShaderInfo (VkShaderStageFlags _stageFlags, string _spirvPath, string _entryPoint = "main") {
			StageFlags = _stageFlags;
			SpirvPath = _spirvPath;
			EntryPoint = new FixedUtf8String (_entryPoint);
		}

		#region IDisposable Support
		private bool disposedValue = false; // Pour détecter les appels redondants

		protected virtual void Dispose (bool disposing) {
			if (!disposedValue) {
				if (disposing)
					EntryPoint.Dispose ();

				disposedValue = true;
			}
		}
		public void Dispose () {
			Dispose (true);
		}
		#endregion
	}

    public class Pipeline : IDisposable {
        internal VkPipeline handle;
        Device dev;
		VkPipelineBindPoint bindPoint = VkPipelineBindPoint.Graphics;

        VkPipelineInputAssemblyStateCreateInfo inputAssemblyState = VkPipelineInputAssemblyStateCreateInfo.New ();
        VkPipelineRasterizationStateCreateInfo rasterizationState = VkPipelineRasterizationStateCreateInfo.New ();
        VkPipelineViewportStateCreateInfo viewportState = VkPipelineViewportStateCreateInfo.New ();
        public VkPipelineDepthStencilStateCreateInfo depthStencilState = VkPipelineDepthStencilStateCreateInfo.New ();
        public VkPipelineMultisampleStateCreateInfo multisampleState = VkPipelineMultisampleStateCreateInfo.New ();

        NativeList<VkPipelineColorBlendAttachmentState> blendAttachments = new NativeList<VkPipelineColorBlendAttachmentState>();
        NativeList<VkDynamicState> dynamicStates = new NativeList<VkDynamicState> ();
        public NativeList<VkVertexInputBindingDescription> vertexBindings = new NativeList<VkVertexInputBindingDescription> ();
        public NativeList<VkVertexInputAttributeDescription> vertexAttributes = new NativeList<VkVertexInputAttributeDescription> ();
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
		/// <summary>
		/// Create a new Pipeline with supplied RenderPass
		/// </summary>
		public Pipeline (Device dev, RenderPass renderPass, VkPrimitiveTopology topology = VkPrimitiveTopology.TriangleList,
			VkSampleCountFlags samples = VkSampleCountFlags.Count1)
		{ 
            this.dev = dev;            

			RenderPass = renderPass;
			RenderPass.references++;

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

            blendAttachments.Add (new VkPipelineColorBlendAttachmentState {
                colorWriteMask = VkColorComponentFlags.R | VkColorComponentFlags.G | VkColorComponentFlags.B | VkColorComponentFlags.A,
                blendEnable = False
            });

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

        public unsafe void Activate () {
			if (isDisposed) {
				GC.ReRegisterForFinalize (this);
				isDisposed = false;
			}

			Layout.Activate ();

			VkGraphicsPipelineCreateInfo info = VkGraphicsPipelineCreateInfo.New ();
            info.renderPass = RenderPass.handle;
            info.layout = Layout.handle;

            NativeList<VkPipelineShaderStageCreateInfo> shaderStages = new NativeList<VkPipelineShaderStageCreateInfo> ();
            foreach (ShaderInfo shader in shaders) {
                shaderStages.Add (new VkPipelineShaderStageCreateInfo {
                    sType = VkStructureType.PipelineShaderStageCreateInfo,
                    stage = shader.StageFlags,
                    pName = shader.EntryPoint.StringPtr,
                    module = dev.LoadSPIRVShader (shader.SpirvPath),
                });
            }

            VkPipelineColorBlendStateCreateInfo colorBlendInfo = VkPipelineColorBlendStateCreateInfo.New ();
            colorBlendInfo.attachmentCount = blendAttachments.Count;
            colorBlendInfo.pAttachments = (VkPipelineColorBlendAttachmentState*) blendAttachments.Data.ToPointer ();

            VkPipelineDynamicStateCreateInfo dynStatesInfo = VkPipelineDynamicStateCreateInfo.New ();
            dynStatesInfo.dynamicStateCount = dynamicStates.Count;
            dynStatesInfo.pDynamicStates = (VkDynamicState*)dynamicStates.Data.ToPointer ();

            VkPipelineVertexInputStateCreateInfo vertInputInfo = VkPipelineVertexInputStateCreateInfo.New ();
            vertInputInfo.vertexBindingDescriptionCount = vertexBindings.Count;
            vertInputInfo.pVertexBindingDescriptions = (VkVertexInputBindingDescription*)vertexBindings.Data.ToPointer ();
            vertInputInfo.vertexAttributeDescriptionCount = vertexAttributes.Count;
            vertInputInfo.pVertexAttributeDescriptions = (VkVertexInputAttributeDescription*)vertexAttributes.Data.ToPointer ();

            //local copy of class fiels whose addresses are requested
            VkPipelineInputAssemblyStateCreateInfo sInputAssembly = inputAssemblyState;
            VkPipelineRasterizationStateCreateInfo sRasterization = rasterizationState;
            VkPipelineMultisampleStateCreateInfo sMultisample = multisampleState;
            VkPipelineViewportStateCreateInfo sViewport = viewportState;
            VkPipelineDepthStencilStateCreateInfo sDepthStencil = depthStencilState;

            info.pVertexInputState = &vertInputInfo;
            info.pInputAssemblyState = &sInputAssembly;
            info.pRasterizationState = &sRasterization;
            info.pColorBlendState = &colorBlendInfo;
            info.pMultisampleState = &sMultisample;
            info.pViewportState = &sViewport;
            info.pDepthStencilState = &sDepthStencil;
            info.pDynamicState = &dynStatesInfo;
            info.stageCount = (uint)shaders.Count;
            info.pStages = (VkPipelineShaderStageCreateInfo*)shaderStages.Data.ToPointer();

            Utils.CheckResult (vkCreateGraphicsPipelines (dev.VkDev, VkPipelineCache.Null, 1, ref info, IntPtr.Zero, out handle));

            for (int i = 0; i < shaders.Count; i++) 
                dev.DestroyShaderModule (shaderStages[i].module);

            shaderStages.Dispose ();
        }
        
		public void Bind (CommandBuffer cmd) {
            vkCmdBindPipeline (cmd.Handle, bindPoint, handle);
        }
		#region IDisposable Support
		private bool isDisposed = false; // Pour détecter les appels redondants

		protected virtual void Dispose (bool disposing) {
			if (!isDisposed) {
				if (disposing) {
					blendAttachments.Dispose ();
					dynamicStates.Dispose ();
					vertexBindings.Dispose ();
					vertexAttributes.Dispose ();
					RenderPass.Dispose ();
					Layout.Dispose ();
				} else
					System.Diagnostics.Debug.WriteLine ("A Pipeline has not been disposed.");
				VulkanNative.vkDestroyPipeline (dev.VkDev, handle, IntPtr.Zero);
				isDisposed = true;
			}
		}

		~Pipeline () {
			Dispose (false);
		}
		public void Dispose () {
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		#endregion
	}
}
