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
using VK;
using static VK.Vk;

namespace VKE {
    public class Pipeline : IDisposable {
		internal Device dev;
        internal VkPipeline handle;

		public readonly RenderPass RenderPass;
		public readonly PipelineLayout Layout;
		public readonly VkPipelineBindPoint BindPoint;
		public readonly VkSampleCountFlags Samples;

		#region CTORS
		/// <summary>
		/// Create a new Pipeline with supplied RenderPass
		/// </summary>
		public Pipeline (PipelineConfig cfg, string name = "pipeline")
		{
			BindPoint = cfg.bindPoint;
			RenderPass = cfg.RenderPass;
			Layout = cfg.Layout;
			Samples = cfg.Samples;

			dev = RenderPass.dev;

#if DEBUG && DEBUG_MARKER
			handle.SetDebugMarkerName (dev, name);
#endif

			init (cfg);
		}

		/// <summary>
		/// Create a new pipeline and the default renderpass for it
		/// </summary>
		//public Pipeline (Device dev, VkFormat colorFormat, VkFormat depthFormat, 
		//	VkPrimitiveTopology topology = VkPrimitiveTopology.TriangleList,
		//	VkSampleCountFlags samples = VkSampleCountFlags.SampleCount1)
		//	: this (dev, , topology,samples)
		//{
        //}
		#endregion

        void init (PipelineConfig cfg) {
			Layout.Activate ();
			RenderPass.Activate ();

			List<VkPipelineShaderStageCreateInfo> shaderStages = new List<VkPipelineShaderStageCreateInfo> ();
			foreach (ShaderInfo shader in cfg.shaders)
				shaderStages.Add (shader.GetStageCreateInfo(dev));

			VkPipelineColorBlendStateCreateInfo colorBlendInfo = VkPipelineColorBlendStateCreateInfo.New;
			colorBlendInfo.attachmentCount = (uint)cfg.blendAttachments.Count;
			colorBlendInfo.pAttachments = cfg.blendAttachments.Pin ();

			VkPipelineDynamicStateCreateInfo dynStatesInfo = VkPipelineDynamicStateCreateInfo.New;
			dynStatesInfo.dynamicStateCount = (uint)cfg.dynamicStates.Count;
			dynStatesInfo.pDynamicStates = cfg.dynamicStates.Pin ();

			VkPipelineVertexInputStateCreateInfo vertInputInfo = VkPipelineVertexInputStateCreateInfo.New;
			vertInputInfo.vertexBindingDescriptionCount = (uint)cfg.vertexBindings.Count;
			vertInputInfo.pVertexBindingDescriptions = cfg.vertexBindings.Pin ();
			vertInputInfo.vertexAttributeDescriptionCount = (uint)cfg.vertexAttributes.Count;
			vertInputInfo.pVertexAttributeDescriptions = cfg.vertexAttributes.Pin ();

			VkGraphicsPipelineCreateInfo info = VkGraphicsPipelineCreateInfo.New;
			info.renderPass 			= RenderPass.handle;
			info.layout					= Layout.handle;
			info.pVertexInputState 		= vertInputInfo.Pin ();
			info.pInputAssemblyState 	= cfg.inputAssemblyState.Pin ();
			info.pRasterizationState 	= cfg.rasterizationState.Pin ();
			info.pColorBlendState 		= colorBlendInfo.Pin ();
			info.pMultisampleState 		= cfg.multisampleState.Pin ();
			info.pViewportState 		= cfg.viewportState.Pin ();
			info.pDepthStencilState 	= cfg.depthStencilState.Pin ();
			info.pDynamicState 			= dynStatesInfo.Pin ();
			info.stageCount 			= (uint)cfg.shaders.Count;
			info.pStages 				= shaderStages.Pin ();

			Utils.CheckResult (vkCreateGraphicsPipelines (dev.VkDev, VkPipelineCache.Null, 1, ref info, IntPtr.Zero, out handle));

			for (int i = 0; i < cfg.shaders.Count; i++)
				dev.DestroyShaderModule (shaderStages[i].module);

			vertInputInfo.Unpin ();
			cfg.inputAssemblyState.Unpin ();
			cfg.rasterizationState.Unpin ();
			colorBlendInfo.Unpin ();
			cfg.multisampleState.Unpin ();
			cfg.viewportState.Unpin ();
			cfg.depthStencilState.Unpin ();
			dynStatesInfo.Unpin ();
			shaderStages.Unpin ();

			cfg.vertexAttributes.Unpin ();
			cfg.vertexBindings.Unpin ();
			cfg.dynamicStates.Unpin ();
			cfg.blendAttachments.Unpin ();
		}

		public void Bind (CommandBuffer cmd) {
            vkCmdBindPipeline (cmd.Handle, BindPoint, handle);
        }

		#region IDisposable Support
		private bool isDisposed = false; // Pour détecter les appels redondants

		protected virtual void Dispose (bool disposing) {
			if (!isDisposed) {
				if (disposing) {
					RenderPass.Dispose ();
					Layout.Dispose ();
				}else
					System.Diagnostics.Debug.WriteLine ("Pipeline disposed by finalizer.");

				vkDestroyPipeline (dev.VkDev, handle, IntPtr.Zero);
				isDisposed = true;
			}
		}

		~Pipeline() {
			Dispose(false);
		}
		public void Dispose () {
			Dispose (true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
