//
// Model.cs
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
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using glTFLoader;
using GL = glTFLoader.Schema;

using Vulkan;
using System.Collections.Generic;
using System.IO;



namespace VKE {
    using static Utils;
    	

	public class Renderer : IDisposable {
        Device dev;
        
        public struct Matrices {
			public Matrix4x4 projection;
			public Matrix4x4 view;
			public Matrix4x4 model;
			public Vector4 lightPos;
			public float gamma;
			public float exposure;
		}

		public Matrices matrices;

		HostBuffer uboMats;
		DescriptorSetLayout descLayoutMatrix;
		DescriptorSetLayout descLayoutTextures;

		DescriptorPool descriptorPool;
		DescriptorSet dsMats;
		DescriptorSet dsSkybox;

		public RenderPass renderPass { get; private set; }

		PipelineLayout pipelineLayout;
		Pipeline pipeline;

		public Camera Camera { get; private set; }

		Model model;

		VkFormat depthFormat, colorFormat;
		VkSampleCountFlags samples;

		       
        public Renderer (Device device, VkFormat depthFormat, VkFormat colorFormat, VkSampleCountFlags samples = VkSampleCountFlags.Count4) {
            dev = device;
			this.depthFormat = depthFormat;
			this.colorFormat = colorFormat;
			this.samples = samples;

			Camera = new Camera (Utils.DegreesToRadians (60f), 1f);

			init ();
        }

		void init () { 
			descriptorPool = new DescriptorPool (dev, 2,
				new VkDescriptorPoolSize (VkDescriptorType.UniformBuffer)
			);				

			descLayoutMatrix = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Vertex|VkShaderStageFlags.Fragment, VkDescriptorType.UniformBuffer));

			descLayoutTextures = new DescriptorSetLayout (dev, 
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (2, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (3, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (4, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler)
			);

			dsMats = descriptorPool.Allocate (descLayoutMatrix);

			VkPushConstantRange[] pushConstantRanges =
			{ new VkPushConstantRange {
				stageFlags = VkShaderStageFlags.Vertex,
				size = (uint)Marshal.SizeOf<Matrix4x4>(),
				offset = 0
			}, new VkPushConstantRange {
				stageFlags = VkShaderStageFlags.Fragment,
				size = (uint)Marshal.SizeOf<Model.PbrMaterial>(),
				offset = 64
			}};

			pipelineLayout = new PipelineLayout (dev, pushConstantRanges, descLayoutMatrix, descLayoutTextures).Activate ();

			renderPass = new RenderPass (dev, colorFormat, depthFormat, samples);

			pipeline = new Pipeline (pipelineLayout, renderPass);
			pipeline.multisampleState.rasterizationSamples = samples;
			pipeline.vertexBindings.Add (new VkVertexInputBindingDescription (0, (uint)Marshal.SizeOf<Model.Vertex> ()));
			pipeline.vertexAttributes.Add (new VkVertexInputAttributeDescription (0, VkFormat.R32g32b32Sfloat));
			pipeline.vertexAttributes.Add (new VkVertexInputAttributeDescription (1, VkFormat.R32g32b32Sfloat, 3 * sizeof (float)));
			pipeline.vertexAttributes.Add (new VkVertexInputAttributeDescription (2, VkFormat.R32g32Sfloat, 6 * sizeof (float)));
			pipeline.shaders.Add (new ShaderInfo (VkShaderStageFlags.Vertex, "shaders/pbrtest.vert.spv"));
			pipeline.shaders.Add (new ShaderInfo (VkShaderStageFlags.Fragment, "shaders/pbrtest.frag.spv"));
			pipeline.Activate ();

			uboMats = new HostBuffer (dev, VkBufferUsageFlags.UniformBuffer, (ulong)Marshal.SizeOf<Matrices>());

			using (DescriptorSetWrites uboUpdate = new DescriptorSetWrites (dev)) {
				uboUpdate.AddWriteInfo (dsMats, descLayoutMatrix.Bindings[0], uboMats.Descriptor);
				uboUpdate.Update ();
			}

			matrices.lightPos = new Vector4 (0.0f, 0.0f, -2.0f, 1.0f);
			matrices.gamma = 1.0f;
			matrices.exposure = 2.0f;

			uboMats.Map ();//permanent map

			Update ();
		}

		public void LoadModel (Queue transferQ, CommandPool cmdPool, string path) { 
			model = new Model (dev, transferQ, cmdPool, path);
			model.WriteMaterialsDescriptorSets (descLayoutTextures,
				ShaderBinding.Color,
				ShaderBinding.Normal,
				ShaderBinding.AmbientOcclusion,
				ShaderBinding.MetalRoughness,
				ShaderBinding.Emissive);
		}

		public void RecordCmd (CommandBuffer cmd, Framebuffer framebuffer) {
			renderPass.Begin (cmd, framebuffer);

			cmd.SetViewport (framebuffer.Width, framebuffer.Height);
			cmd.SetScissor (framebuffer.Width, framebuffer.Height);

			cmd.BindDescriptorSet (pipelineLayout, dsMats);

			cmd.BindPipeline (pipeline);

			model.Bind (cmd);
			model.DrawAll (cmd, pipelineLayout);

			renderPass.End (cmd);		 
		}

		public void Update () {
			Camera.Update ();
			matrices.projection = Camera.Projection;
			matrices.view = Camera.View;
			matrices.model = Camera.Model;
			uboMats.Update (matrices, (uint)Marshal.SizeOf<Matrices> ());
		}

		#region IDisposable Support
		private bool isDisposed = false; // Pour détecter les appels redondants

		protected virtual void Dispose (bool disposing) {
			if (!isDisposed) {
				if (disposing) {
					model.Dispose ();
					pipeline.Dispose ();
					pipelineLayout.Dispose ();
					descLayoutMatrix.Dispose ();
					descLayoutTextures.Dispose ();

					descriptorPool.Dispose ();
					renderPass.Dispose ();

					uboMats.Dispose ();
				} else
					Debug.WriteLine ("renderer was not disposed");
				isDisposed = true;
			}
		}
		public void Dispose () {
			Dispose (true);
		}
		#endregion
	}
}
