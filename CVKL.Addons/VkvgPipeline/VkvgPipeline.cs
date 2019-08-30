// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Numerics;
using CVKL;
using VK;

namespace VkvgPipeline {

	public class VkvgPipeline : GraphicPipeline {
		vkvg.Device vkvgDev;
		vkvg.Surface vkvgSurf;
		Image uiImage;

		public vkvg.Context CreateContext () => new vkvg.Context (vkvgSurf);

		public VkvgPipeline (Instance instance, Device dev, Queue queue, GraphicPipeline pipeline, PipelineCache pipelineCache = null)
		: base (pipeline.RenderPass, pipelineCache, "vkvg pipeline") {

			GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, this.RenderPass.Samples, false);
			cfg.RenderPass = RenderPass;
			cfg.Layout = pipeline.Layout;
			cfg.AddShader (VkShaderStageFlags.Vertex, "#CVKL.FullScreenQuad.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "#CVKL.simpletexture.frag.spv");
			cfg.multisampleState.rasterizationSamples = Samples;

			cfg.blendAttachments[0] = new VkPipelineColorBlendAttachmentState (true);

			layout = cfg.Layout;

			init (cfg);

			vkvgDev = new vkvg.Device (instance.Handle, dev.phy.Handle, dev.VkDev.Handle, queue.qFamIndex,
				vkvg.SampleCount.Sample_4, queue.index);				
		}

		void initUISurface (int width, int height) {
			uiImage?.Dispose ();
			vkvgSurf?.Dispose ();
			vkvgSurf = new vkvg.Surface (vkvgDev, width, height);
			uiImage = new Image (Dev, new VkImage ((ulong)vkvgSurf.VkImage.ToInt64 ()), VkFormat.B8g8r8a8Unorm,
				VkImageUsageFlags.ColorAttachment, (uint)vkvgSurf.Width, (uint)vkvgSurf.Height);
			uiImage.CreateView (VkImageViewType.ImageView2D, VkImageAspectFlags.Color);
			uiImage.CreateSampler (VkFilter.Nearest, VkFilter.Nearest, VkSamplerMipmapMode.Nearest, VkSamplerAddressMode.ClampToBorder);
			uiImage.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
		}


		public void Resize (int width, int height, DescriptorSetWrites dsUpdate) {
			initUISurface (width, height);
			dsUpdate.Write (Dev, uiImage.Descriptor);
		}
		public void RecordDraw (CommandBuffer cmd) {
			Bind (cmd);

			uiImage.SetLayout (cmd, VkImageAspectFlags.Color, VkImageLayout.ColorAttachmentOptimal, VkImageLayout.ShaderReadOnlyOptimal,
				VkPipelineStageFlags.ColorAttachmentOutput, VkPipelineStageFlags.FragmentShader);

			cmd.Draw (3, 1, 0, 0);

			uiImage.SetLayout (cmd, VkImageAspectFlags.Color, VkImageLayout.ShaderReadOnlyOptimal, VkImageLayout.ColorAttachmentOptimal,
				VkPipelineStageFlags.FragmentShader, VkPipelineStageFlags.BottomOfPipe);
		}
		public void DrawResources (vkvg.Context ctx, int width, int height) {
			ResourceManager rm = Dev.resourceManager;

			int margin = 5, memPoolHeight = 15;
			int drawingWidth = width - 4 * margin;
			int drawingHeight = (memPoolHeight + margin) * (rm.memoryPools.Length) + margin;
			int y = height - drawingHeight - margin;
			int x = 2 * margin;
			ctx.LineWidth = 1;
			ctx.SetSource (0.1, 0.1, 0.1, 0.8);
			ctx.Rectangle (0.5 + margin, 0.5 + y, width - 2 * margin, drawingHeight);
			ctx.FillPreserve ();
			ctx.Flush ();
			ctx.SetSource (0.8, 0.8, 0.8);
			ctx.Stroke ();

			foreach (MemoryPool mp in rm.memoryPools) {
				float byteWidth = (float)drawingWidth / mp.Size;

				y += margin;
				ctx.Rectangle (x, y, drawingWidth, memPoolHeight);
				ctx.SetSource (0.3, 0.3, 0.3, 0.4);
				ctx.Fill ();

				if (mp.Last == null)
					return;

				Resource r = mp.Last;
				do {
					float w = Math.Max (1f, byteWidth * r.AllocatedDeviceMemorySize);

					Vector3 c = new Vector3 (0);
					Image img = r as Image;
					if (img != null) {
						c.Z = 1f;
						if (img.CreateInfo.usage.HasFlag (VkImageUsageFlags.InputAttachment))
							c.Y += 0.3f;
						if (img.CreateInfo.usage.HasFlag (VkImageUsageFlags.ColorAttachment))
							c.Y += 0.1f;
						if (img.CreateInfo.usage.HasFlag (VkImageUsageFlags.Sampled))
							c.X += 0.3f;
					} else {
						CVKL.Buffer buff = r as CVKL.Buffer;
						c.X = 1f;
						if (buff.Infos.usage.HasFlag (VkBufferUsageFlags.IndexBuffer))
							c.Y += 0.2f;
						if (buff.Infos.usage.HasFlag (VkBufferUsageFlags.VertexBuffer))
							c.Y += 0.4f;
						if (buff.Infos.usage.HasFlag (VkBufferUsageFlags.UniformBuffer))
							c.Z += 0.3f;
					}
					ctx.SetSource (c.X, c.Y, c.Z, 0.5);
					ctx.Rectangle (0.5f + x + byteWidth * r.poolOffset, 0.5f + y, w, memPoolHeight);
					ctx.FillPreserve ();
					ctx.SetSource (0.01f, 0.01f, 0.01f);
					ctx.Stroke ();
					r = r.next;
				} while (r != mp.Last && r != null);
				y += memPoolHeight;
			}
		}
		protected override void Dispose (bool disposing) {
			uiImage?.Dispose ();
			vkvgSurf?.Dispose ();
			vkvgDev.Dispose ();

			base.Dispose (disposing);
		}
	}

}
