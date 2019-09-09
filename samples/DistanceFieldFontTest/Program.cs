using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;

using VK;
using CVKL;
using CVKL.DistanceFieldFont;

namespace DistanceFieldFontTest {



	class Program : VkWindow {
		static void Main (string[] args) {
			SwapChain.PREFERED_FORMAT = VkFormat.B8g8r8a8Unorm;
#if DEBUG
			Instance.VALIDATION = true;
			Instance.DEBUG_UTILS = true;
			Instance.RENDER_DOC_CAPTURE = false;
#endif
			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}

		float rotSpeed = 0.01f, zoomSpeed = 0.01f;
		float rotX, rotY, rotZ = 0f, zoom = 1f;

		float rotAnim = 1f;

		struct Matrices {
			public Matrix4x4 projection;
			public Matrix4x4 view;
			public Matrix4x4 model;
		}

		Matrices matrices;

		HostBuffer uboMats;
		GPUBuffer<float> vbo;
		GPUBuffer<ushort> ibo;

		DescriptorPool descriptorPool;
		DescriptorSetLayout dsLayout;
		DescriptorSet descriptorSet;

		GraphicPipeline pipeline;
		Framebuffer[] frameBuffers;

		Image fontTexture;

		struct Vertex {
			public Vector3 pos;
			public Vector2 uv;
			public Vertex (float x, float y, float z, float u, float v) {
				pos.X = x; pos.Y = y; pos.Z = z;
				uv.X = u; uv.Y = v;
			}
		}

		BMFont font;
		Vector4 textColor = new Vector4 (1.0f, 1.0f, 0.0f, 1.0f);//alpha => 0:disabled 1:enabled
		Vector4 outlineColor = new Vector4 (1.0f, 0.0f, 0.0f, 0.6f);//alpha => 0:disabled 1:enabled

		Program () : base () {

			font = new BMFont ("../data/font.fnt");

			vbo = new GPUBuffer<float> (dev, VkBufferUsageFlags.VertexBuffer, 1024);
			ibo = new GPUBuffer<ushort> (dev, VkBufferUsageFlags.IndexBuffer, 2048);

			descriptorPool = new DescriptorPool (dev, 1,
				new VkDescriptorPoolSize (VkDescriptorType.UniformBuffer),
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler)
			);

			dsLayout = new DescriptorSetLayout (dev, 0,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Vertex, VkDescriptorType.UniformBuffer),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler));

			GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, VkSampleCountFlags.SampleCount4, false);

			cfg.Layout = new PipelineLayout (dev, dsLayout);
			cfg.Layout.AddPushConstants (new VkPushConstantRange (VkShaderStageFlags.Fragment, (uint)Marshal.SizeOf<Vector4> () * 2));

			cfg.RenderPass = new RenderPass (dev, swapChain.ColorFormat, cfg.Samples);

			cfg.blendAttachments[0] = new VkPipelineColorBlendAttachmentState (
				true, VkBlendFactor.One, VkBlendFactor.OneMinusSrcAlpha, VkBlendOp.Add, VkBlendFactor.One, VkBlendFactor.Zero);

			cfg.AddVertexBinding (0, 5 * sizeof (float));
			cfg.AddVertexAttributes (0, VkFormat.R32g32b32Sfloat, VkFormat.R32g32Sfloat);

			cfg.AddShader (VkShaderStageFlags.Vertex, "#DistanceFieldFontTest.main.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "#DistanceFieldFontTest.main.frag.spv");

			pipeline = new GraphicPipeline (cfg);

			uboMats = new HostBuffer (dev, VkBufferUsageFlags.UniformBuffer, matrices);
			uboMats.Map ();//permanent map

			descriptorSet = descriptorPool.Allocate (dsLayout);

			fontTexture = font.GetPageTexture (0, presentQueue, cmdPool);
			fontTexture.CreateView ();
			fontTexture.CreateSampler ();
			fontTexture.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;

			DescriptorSetWrites dsUpdate = new DescriptorSetWrites (descriptorSet, dsLayout);
			dsUpdate.Write (dev, uboMats.Descriptor, fontTexture.Descriptor);

			generateText ("Vulkan", out HostBuffer<Vertex> staggingVbo, out HostBuffer<ushort> staggingIbo);

			CommandBuffer cmd = cmdPool.AllocateAndStart (VkCommandBufferUsageFlags.OneTimeSubmit);

			staggingVbo.CopyTo (cmd, vbo);
			staggingIbo.CopyTo (cmd, ibo);

			presentQueue.EndSubmitAndWait (cmd);

			staggingVbo.Dispose ();
			staggingIbo.Dispose ();

			UpdateFrequency = 10;
		}


		// Creates a vertex buffer containing quads for the passed text
		void generateText (string text, out HostBuffer<Vertex> svbo, out HostBuffer<ushort> sibo) {
			List<Vertex> vertices = new List<Vertex> ();
			List<ushort> indices = new List<ushort> ();
			ushort indexOffset = 0;

			float w = fontTexture.Width;

			float posx = 0.0f;
			float posy = 0.0f;

			for (int i = 0; i < text.Length; i++) {
				BMChar charInfo = font.CharMap[text[i]];


				if (charInfo.width == 0)
					charInfo.width = 36;

				float charw = ((float)(charInfo.width) / 36.0f);
				float dimx = 1.0f * charw;
				float charh = ((float)(charInfo.height) / 36.0f);
				float dimy = 1.0f * charh;
				posy = 1.0f - charh;

				float us = charInfo.x / w;
				float ue = (charInfo.x + charInfo.width) / w;
				float ts = charInfo.y / w;
				float te = (charInfo.y + charInfo.height) / w;

				float xo = charInfo.xoffset / 36.0f;
				float yo = charInfo.yoffset / 36.0f;

				vertices.Add (new Vertex (posx + dimx + xo, posy + dimy, 0.0f, ue, te));
				vertices.Add (new Vertex (posx + xo, posy + dimy, 0.0f, us, te));
				vertices.Add (new Vertex (posx + xo, posy, 0.0f, us, ts));
				vertices.Add (new Vertex (posx + dimx + xo, posy, 0.0f, ue, ts));

				indices.AddRange (new ushort[] { indexOffset, (ushort)(indexOffset + 1), (ushort)(indexOffset + 2), (ushort)(indexOffset + 2), (ushort)(indexOffset + 3), indexOffset });
				indexOffset += 4;

				float advance = charInfo.xadvance / 36.0f;
				posx += advance;
			}

			Vertex[] vx = vertices.ToArray ();

			// Center
			for (int i = 0; i < vx.Length; i++) {
				vx[i].pos.X -= posx / 2.0f;
				vx[i].pos.Y -= 0.5f;
			}

			svbo = new HostBuffer<Vertex> (dev, VkBufferUsageFlags.TransferSrc, vx);
			sibo = new HostBuffer<ushort> (dev, VkBufferUsageFlags.TransferSrc, indices.ToArray());
		}

		bool rebuildBuffers;

		public override void Update () {
			if (rebuildBuffers) {
				buildCommandBuffers ();
				rebuildBuffers = false;
			}
			if (rotAnim < 0.000001f)
				return;
			rotY += rotAnim;
			rotAnim *= 0.95f;
			updateViewRequested = true;
		}

		void buildCommandBuffers () {
			for (int i = 0; i < swapChain.ImageCount; ++i) {
				cmds[i]?.Free ();
				cmds[i] = cmdPool.AllocateAndStart ();

				recordDraw (cmds[i], frameBuffers[i]);

				cmds[i].End ();
			}
		}

		void recordDraw (CommandBuffer cmd, Framebuffer fb) {
			pipeline.RenderPass.Begin (cmd, fb);

			cmd.SetViewport (fb.Width, fb.Height);
			cmd.SetScissor (fb.Width, fb.Height);

			cmd.BindPipeline (pipeline, descriptorSet);

			cmd.PushConstant (pipeline, textColor);
			cmd.PushConstant (pipeline, outlineColor, 0, 16);

			cmd.BindVertexBuffer (vbo, 0);
			cmd.BindIndexBuffer (ibo, VkIndexType.Uint16);
			cmd.DrawIndexed ((uint)ibo.ElementCount,1,0,0,0);

			pipeline.RenderPass.End (cmd);
		}


		public override void UpdateView () {
			matrices.projection = Matrix4x4.CreatePerspectiveFieldOfView (Utils.DegreesToRadians (60f), (float)swapChain.Width / (float)swapChain.Height, 0.1f, 256.0f);
			matrices.view = Matrix4x4.CreateTranslation (0, 0, -2.5f * zoom);
			matrices.model =
				Matrix4x4.CreateFromAxisAngle (Vector3.UnitZ, rotZ) *
				Matrix4x4.CreateFromAxisAngle (Vector3.UnitY, rotY) *
				Matrix4x4.CreateFromAxisAngle (Vector3.UnitX, rotX);

			uboMats.Update (matrices, (uint)Marshal.SizeOf<Matrices> ());

			updateViewRequested = false;
		}

		protected override void onMouseMove (double xPos, double yPos) {
			double diffX = lastMouseX - xPos;
			double diffY = lastMouseY - yPos;
			if (MouseButton[0]) {
				rotY -= rotSpeed * (float)diffX;
				rotX += rotSpeed * (float)diffY;
			} else if (MouseButton[1]) {
				zoom += zoomSpeed * (float)diffY;
			}

			updateViewRequested = true;
		}

		protected override void onKeyDown (Key key, int scanCode, Modifier modifiers) {
			switch (key) {
				case Key.Space:
					rotAnim += 0.1f;
					break;
				case Key.F2:
					if (modifiers.HasFlag (Modifier.Shift))
						outlineColor.W -= 0.01f;
					else
						outlineColor.W += 0.01f;
					rebuildBuffers = true;
					break;
				default:
					base.onKeyDown (key, scanCode, modifiers);
					break;
			}					
		}
		protected override void OnResize () {
			base.OnResize ();

			updateViewRequested = true;

			if (frameBuffers != null)
				for (int i = 0; i < swapChain.ImageCount; ++i)
					frameBuffers[i]?.Dispose ();

			frameBuffers = new Framebuffer[swapChain.ImageCount];

			for (int i = 0; i < swapChain.ImageCount; ++i) {
				frameBuffers[i] = new Framebuffer (pipeline.RenderPass, swapChain.Width, swapChain.Height,
					(pipeline.Samples == VkSampleCountFlags.SampleCount1) ? new Image[] {
						swapChain.images[i],
					} : new Image[] {
						null,
						swapChain.images[i]
					});
			}

			buildCommandBuffers ();
		}

		protected override void Dispose (bool disposing) {
			if (disposing) {
				if (!isDisposed) {
					dev.WaitIdle ();
					pipeline.Dispose ();
					dsLayout.Dispose ();
					for (int i = 0; i < swapChain.ImageCount; i++)
						frameBuffers[i].Dispose ();
					descriptorPool.Dispose ();
					fontTexture?.Dispose ();
					vbo.Dispose ();
					ibo.Dispose ();
					uboMats.Dispose ();
				}
			}
			base.Dispose (disposing);
		}
	}
}
