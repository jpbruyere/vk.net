using System;
using System.Numerics;
using System.Runtime.InteropServices;
using VK;

namespace VKE {
	public class DebugDrawPipeline : Pipeline {

		public HostBuffer Vertices;
		uint vertexCount;
		uint vboLength = 100 * 6 * sizeof (float);

		public DebugDrawPipeline (Device dev, VkFormat colorFormat, VkSampleCountFlags samples = VkSampleCountFlags.SampleCount1) :
			base (new RenderPass (dev, colorFormat), "Debug draw pipeline") {

			PipelineConfig cfg = PipelineConfig.CreateDefault (VkPrimitiveTopology.LineList, samples);
			cfg.RenderPass = RenderPass;
			cfg.Layout = new PipelineLayout(dev);
			cfg.AddVertexBinding (0, 6 * sizeof(float));
			cfg.SetVertexAttributes (0, VkFormat.R32g32b32Sfloat, VkFormat.R32g32b32Sfloat);

			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/pbrtest.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/pbrtest.frag.spv");

			layout = cfg.Layout;

			init (cfg);

			Vertices = new HostBuffer (dev, VkBufferUsageFlags.VertexBuffer, vboLength);
			Vertices.Map ();
		}

		public void AddLine (Vector3 start, Vector3 end, float r, float g, float b) {
			float[] data = { };
			Vertices.Update (data, 12 * sizeof (float), vertexCount * 6 * sizeof (float));
			vertexCount++;
		}

		protected override void Dispose (bool disposing) {
			Vertices.Unmap ();
			Vertices.Dispose ();

			base.Dispose (disposing);
		}
	}

}
