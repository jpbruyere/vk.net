using System;
using System.Numerics;
using System.Runtime.InteropServices;
using VK;

namespace CVKL {
	public class DebugDrawPipeline : GraphicPipeline {
		public HostBuffer Vertices;
		uint vertexCount;
        uint vboLength;

		public DebugDrawPipeline (RenderPass renderPass, uint maxVertices = 100,
            VkSampleCountFlags samples = VkSampleCountFlags.SampleCount1, PipelineCache pipelineCache = null) :
			base (renderPass, pipelineCache, "Debug draw pipeline") {

            vboLength = 100 * 6 * sizeof(float);

            GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.LineList, samples);
			cfg.rasterizationState.lineWidth = 1.0f;
			cfg.RenderPass = RenderPass;
			cfg.Layout = new PipelineLayout(Dev, new VkPushConstantRange(VkShaderStageFlags.Vertex, (uint)Marshal.SizeOf<Matrix4x4>() * 2));
			cfg.AddVertexBinding (0, 6 * sizeof(float));
			cfg.AddVertexAttributes (0, VkFormat.R32g32b32Sfloat, VkFormat.R32g32b32Sfloat);
			cfg.blendAttachments[0] = new VkPipelineColorBlendAttachmentState (true);

			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/debug.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/debug.frag.spv");

			layout = cfg.Layout;

			init (cfg);

			Vertices = new HostBuffer (Dev, VkBufferUsageFlags.VertexBuffer, vboLength);
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
		public void UpdateLine (uint lineNum, Vector3 start, Vector3 end, float r, float g, float b) {
			float[] data = {
				start.X, start.Y, start.Z,
				r, g, b,
				end.X, end.Y, end.Z,
				r, g, b
			};
			Vertices.Update (data, 12 * sizeof (float), (lineNum-1) * 2 * 6 * sizeof (float));
		}

		public void RecordDraw (CommandBuffer cmd, Framebuffer fb, Matrix4x4 projection, Matrix4x4 view) {

            //cmd.SetViewport (fb.Width/ratio, fb.Height/ratio, (ratio-1) * (int)fb.Width / ratio, (ratio-1) * (int)fb.Height / ratio);
            //cmd.SetViewport (200, 200,100,100,-10,10);//, 4 * (int)fb.Width / 5, 4 * (int)fb.Height / 5);
            //cmd.SetScissor (fb.Width / ratio, fb.Height / ratio, (ratio-1) * (int)fb.Width / ratio, (ratio-1) * (int)fb.Height / ratio);
            //cmd.SetScissor (200, 200,100,100);

            Bind(cmd);

            cmd.PushConstant (layout, VkShaderStageFlags.Vertex, projection);
            cmd.PushConstant (layout, VkShaderStageFlags.Vertex, view, (uint)Marshal.SizeOf<Matrix4x4>());

			cmd.BindVertexBuffer (Vertices);
			cmd.Draw (vertexCount);
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
