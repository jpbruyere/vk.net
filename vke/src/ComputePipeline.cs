//
// ComputePipeline.cs
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
using VK;
using static VK.Vk;

namespace VKE {
    public class ComputePipeline : Pipeline {

		public string SpirVPath;
    
		#region CTORS
		protected ComputePipeline (Device dev, string name = "compute pipeline") : base (dev, name) { 
		}
		/// <summary>
		/// Create a new Pipeline with supplied RenderPass
		/// </summary>
		public ComputePipeline (PipelineLayout layout, string spirvPath, string name = "pipeline") : base(layout.dev, name)
		{
			SpirVPath = spirvPath;
			this.layout = layout;

			init ();
		}
		#endregion

		void init () {
			layout.Activate ();

			using (ShaderInfo shader = new ShaderInfo (VkShaderStageFlags.Compute, SpirVPath)) {
				VkComputePipelineCreateInfo info = VkComputePipelineCreateInfo.New ();
				info.layout = layout.Handle;
				info.stage = shader.GetStageCreateInfo (dev);
				info.basePipelineHandle = 0;
				info.basePipelineIndex = 0;

				Utils.CheckResult (Vk.vkCreateComputePipelines (dev.VkDev, VkPipelineCache.Null, 1, ref info, IntPtr.Zero, out handle));
			}
		}

		public override void Bind (CommandBuffer cmd) {
            vkCmdBindPipeline (cmd.Handle, VkPipelineBindPoint.Compute, handle);
        }
		public override void BindDescriptorSet (CommandBuffer cmd, DescriptorSet dset, uint firstSet = 0) {
			cmd.BindDescriptorSet (VkPipelineBindPoint.Compute, layout, dset, firstSet);
		}
		public void BindAndDispatch (CommandBuffer cmd, uint groupCountX, uint groupCountY = 1, uint groupCountZ = 1) {
			vkCmdBindPipeline (cmd.Handle, VkPipelineBindPoint.Compute, handle);
			vkCmdDispatch (cmd.Handle, groupCountX, groupCountY, groupCountZ);
		}
	}
}
