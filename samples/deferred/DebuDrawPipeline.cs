using System;
using System.Numerics;
using System.Runtime.InteropServices;
using VK;

namespace CVKL {
	public class DebugDrawPipeline : GraphicPipeline {
		public HostBuffer Vertices;
		uint vertexCount;
		uint vboLength = 100 * 6 * sizeof (float);

		public DebugDrawPipeline (Device dev, DescriptorSetLayout dsLayout, VkFormat colorFormat, VkSampleCountFlags samples = VkSampleCountFlags.SampleCount1) :
			base (new RenderPass (dev, colorFormat), "Debug draw pipeline") {

			GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.LineList, samples);
			cfg.rasterizationState.lineWidth = 1.0f;
			cfg.RenderPass = RenderPass;
			cfg.Layout = new PipelineLayout(dev, dsLayout);
			cfg.Layout.AddPushConstants (
				new VkPushConstantRange (VkShaderStageFlags.Vertex, (uint)Marshal.SizeOf<Matrix4x4> () * 2)			
			);
			cfg.AddVertexBinding (0, 6 * sizeof(float));
			cfg.SetVertexAttributes (0, VkFormat.R32g32b32Sfloat, VkFormat.R32g32b32Sfloat);

			cfg.blendAttachments[0] = new VkPipelineColorBlendAttachmentState (true);

			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/debug.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/debug.frag.spv");

			layout = cfg.Layout;

			init (cfg);

			Vertices = new HostBuffer (dev, VkBufferUsageFlags.VertexBuffer, vboLength);
			Vertices.Map ();
		}

		public void AddLine (Vector3 start, Vector3 end, float r, float g, float b) {
			float[] data = {
				start.X, start.Y, start.Z,
				r, g, b,
				end.X, end.Y, end.Z,
				r, g, b
			};
			Vertices.Update (data, 12 * sizeof (float), vertexCount * 6 * sizeof (float));
			vertexCount+=2;
		}

		public void RecordDraw (CommandBuffer cmd, Framebuffer fb, Camera camera) {
			RenderPass.Begin (cmd, fb);
			const int ratio = 8;
			cmd.SetViewport (fb.Width/ratio, fb.Height/ratio, (ratio-1) * (int)fb.Width / ratio, (ratio-1) * (int)fb.Height / ratio);
			//cmd.SetViewport (200, 200,100,100,-10,10);//, 4 * (int)fb.Width / 5, 4 * (int)fb.Height / 5);
			cmd.SetScissor (fb.Width / ratio, fb.Height / ratio, (ratio-1) * (int)fb.Width / ratio, (ratio-1) * (int)fb.Height / ratio);
			//cmd.SetScissor (200, 200,100,100);

			Matrix4x4 ortho = Matrix4x4.CreateOrthographic (4, 4.0f / camera.AspectRatio,-1,1);

			cmd.PushConstant (layout, VkShaderStageFlags.Vertex, ortho);

			Bind (cmd);

			cmd.BindVertexBuffer (Vertices);
			cmd.Draw (vertexCount);
			RenderPass.End (cmd);
		}

		protected override void Dispose (bool disposing) {
			if (disposing) {
				Vertices.Unmap ();
				Vertices.Dispose ();
			}

			base.Dispose (disposing);
		}
	}

}
