using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;
using CVKL;
using VK;
using Buffer = CVKL.Buffer;

namespace TextureCube {
	class Program : VkWindow {
		static void Main (string[] args) {
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

		struct Matrices {
			public Matrix4x4 projection;
			public Matrix4x4 view;
		}

		Matrices matrices;

		HostBuffer 			uboMats;
		GPUBuffer<float> 	vbo;
		DescriptorPool 		descriptorPool;
		DescriptorSetLayout dsLayout;
		DescriptorSet 		descriptorSet;
		GraphicPipeline 	pipeline;
		Framebuffer[] 		frameBuffers;

		Image texture;
		Image nextTexture;

		static float[] g_vertex_buffer_data = {
			-1.0f,-1.0f,-1.0f,    0.0f, 1.0f,  // -X side
			-1.0f,-1.0f, 1.0f,    1.0f, 1.0f,
			-1.0f, 1.0f, 1.0f,    1.0f, 0.0f,
			-1.0f, 1.0f, 1.0f,    1.0f, 0.0f,
			-1.0f, 1.0f,-1.0f,    0.0f, 0.0f,
			-1.0f,-1.0f,-1.0f,    0.0f, 1.0f,
			                      
			-1.0f,-1.0f,-1.0f,    1.0f, 1.0f,  // -Z side
			 1.0f, 1.0f,-1.0f,    0.0f, 0.0f,
			 1.0f,-1.0f,-1.0f,    0.0f, 1.0f,
			-1.0f,-1.0f,-1.0f,    1.0f, 1.0f,
			-1.0f, 1.0f,-1.0f,    1.0f, 0.0f,
			 1.0f, 1.0f,-1.0f,    0.0f, 0.0f,
			                      
			-1.0f,-1.0f,-1.0f,    1.0f, 0.0f,  // -Y side
			 1.0f,-1.0f,-1.0f,    1.0f, 1.0f,
			 1.0f,-1.0f, 1.0f,    0.0f, 1.0f,
			-1.0f,-1.0f,-1.0f,    1.0f, 0.0f,
			 1.0f,-1.0f, 1.0f,    0.0f, 1.0f,
			-1.0f,-1.0f, 1.0f,    0.0f, 0.0f,
			                      
			-1.0f, 1.0f,-1.0f,    1.0f, 0.0f,  // +Y side
			-1.0f, 1.0f, 1.0f,    0.0f, 0.0f,
			 1.0f, 1.0f, 1.0f,    0.0f, 1.0f,
			-1.0f, 1.0f,-1.0f,    1.0f, 0.0f,
			 1.0f, 1.0f, 1.0f,    0.0f, 1.0f,
			 1.0f, 1.0f,-1.0f,    1.0f, 1.0f,
			                      
			 1.0f, 1.0f,-1.0f,    1.0f, 0.0f,  // +X side
			 1.0f, 1.0f, 1.0f,    0.0f, 0.0f,
			 1.0f,-1.0f, 1.0f,    0.0f, 1.0f,
			 1.0f,-1.0f, 1.0f,    0.0f, 1.0f,
			 1.0f,-1.0f,-1.0f,    1.0f, 1.0f,
			 1.0f, 1.0f,-1.0f,    1.0f, 0.0f,
			                      
			-1.0f, 1.0f, 1.0f,    0.0f, 0.0f,  // +Z side
			-1.0f,-1.0f, 1.0f,    0.0f, 1.0f,
			 1.0f, 1.0f, 1.0f,    1.0f, 0.0f,
			-1.0f,-1.0f, 1.0f,    0.0f, 1.0f,
			 1.0f,-1.0f, 1.0f,    1.0f, 1.0f,
			 1.0f, 1.0f, 1.0f,    1.0f, 0.0f,
		};
		int currentImgIndex = 4;
		string[] imgPathes = {
			"../data/textures/uffizi_cube.ktx",
			"../data/textures/papermill.ktx",
			"../data/textures/cubemap_yokohama_bc3_unorm.ktx",
			"../data/textures/gcanyon_cube.ktx",
			"../data/textures/pisa_cube.ktx",
		};

		VkSampleCountFlags samples = VkSampleCountFlags.SampleCount1;

		vkvg.Device vkvgDev;
		vkvg.Surface vkvgSurf;
		Image uiImage;
		GraphicPipeline uiPipeline;
		DescriptorSet dsVKVG;

		void initUISurface () {
			uiImage?.Dispose ();
			vkvgSurf?.Dispose ();
			vkvgSurf = new vkvg.Surface (vkvgDev, (int)swapChain.Width, (int)swapChain.Height);
			uiImage = new Image (dev, new VkImage ((ulong)vkvgSurf.VkImage.ToInt64 ()), VkFormat.B8g8r8a8Unorm,
				VkImageUsageFlags.ColorAttachment, (uint)vkvgSurf.Width, (uint)vkvgSurf.Height);
			uiImage.CreateView (VkImageViewType.ImageView2D, VkImageAspectFlags.Color);
			uiImage.CreateSampler (VkFilter.Nearest, VkFilter.Nearest, VkSamplerMipmapMode.Nearest, VkSamplerAddressMode.ClampToBorder);
			uiImage.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;

			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (dsVKVG, dsLayout.Bindings[1]);
			uboUpdate.Write (dev, uiImage.Descriptor);
		}

		void vkvgDraw () {
			using (vkvg.Context ctx = new vkvg.Context (vkvgSurf)) {
				ctx.Operator = vkvg.Operator.Clear;
				ctx.Paint ();
				ctx.Operator = vkvg.Operator.Over;

				drawResources (ctx);
			}
		}

		void drawResources (vkvg.Context ctx) {
			ResourceManager rm = dev.resourceManager;

			int margin = 5, memPoolHeight=15;
			int drawingWidth = (int)swapChain.Width - 4 * margin;
			int drawingHeight = (memPoolHeight + margin) * (rm.memoryPools.Length) + margin;
			int y = (int)swapChain.Height - drawingHeight - margin;
			int x = 2 * margin;
			ctx.LineWidth = 1;
			ctx.SetSource (0.1, 0.1, 0.1, 0.8);
			ctx.Rectangle (0.5+margin, 0.5+y, swapChain.Width - 2*margin, drawingHeight);
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
					float width = Math.Max (1f, byteWidth * r.AllocatedDeviceMemorySize);

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
					ctx.Rectangle (0.5f + x + byteWidth * r.poolOffset, 0.5f + y, width, memPoolHeight);
					ctx.FillPreserve ();
					ctx.SetSource (0.01f, 0.01f, 0.01f);
					ctx.Stroke ();
					r = r.next;
				} while (r != mp.Last && r != null);
				y += memPoolHeight;
			}
		}

		Program () : base () {
			vkvgDev = new vkvg.Device (instance.Handle, phy.Handle, dev.VkDev.Handle, presentQueue.qFamIndex,
				vkvg.SampleCount.Sample_4, presentQueue.index);
				
			vbo = new GPUBuffer<float> (presentQueue, cmdPool, VkBufferUsageFlags.VertexBuffer, g_vertex_buffer_data);

			descriptorPool = new DescriptorPool (dev, 2,
				new VkDescriptorPoolSize (VkDescriptorType.UniformBuffer,2),
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler,2)
			);

			dsLayout = new DescriptorSetLayout (dev, 0,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Vertex, VkDescriptorType.UniformBuffer),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler));
				
			GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, samples);

			cfg.Layout = new PipelineLayout (dev, dsLayout);
			cfg.RenderPass = new RenderPass (dev, swapChain.ColorFormat, dev.GetSuitableDepthFormat (), cfg.Samples);

			cfg.AddVertexBinding (0, 5 * sizeof(float));
			cfg.AddVertexAttributes (0, VkFormat.R32g32b32Sfloat, VkFormat.R32g32Sfloat);

			cfg.AddShader (VkShaderStageFlags.Vertex, "#TexturedCube.skybox.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "#TexturedCube.skybox.frag.spv");

			pipeline = new GraphicPipeline (cfg);

			cfg.ResetShadersAndVerticesInfos ();
			cfg.AddShader (VkShaderStageFlags.Vertex, "#TexturedCube.FullScreenQuad.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "#TexturedCube.simpletexture.frag.spv");

			cfg.blendAttachments[0] = new VkPipelineColorBlendAttachmentState (true);

			uiPipeline = new GraphicPipeline (cfg);


			uboMats = new HostBuffer (dev, VkBufferUsageFlags.UniformBuffer, matrices);
			uboMats.Map ();//permanent map

			descriptorSet = descriptorPool.Allocate (dsLayout);
			dsVKVG = descriptorPool.Allocate (dsLayout);

			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (descriptorSet, dsLayout.Bindings[0]);				
			uboUpdate.Write (dev, uboMats.Descriptor);

			loadTexture (imgPathes[currentImgIndex]);
			if (nextTexture != null)
				updateTextureSet ();

			initUISurface ();
		}
		public override void Update () {
			vkvgDraw ();
		}
		void buildCommandBuffers () {
			for (int i = 0; i < swapChain.ImageCount; ++i) { 								
                  	cmds[i]?.Free ();
				cmds[i] = cmdPool.AllocateCommandBuffer ();
				cmds[i].Start ();

				recordDraw (cmds[i], frameBuffers[i]);

				cmds[i].End ();				 
			}
		} 
		void recordDraw (CommandBuffer cmd, Framebuffer fb) { 
			pipeline.RenderPass.Begin (cmd, fb);

			cmd.SetViewport (fb.Width, fb.Height);
			cmd.SetScissor (fb.Width, fb.Height);
			cmd.BindDescriptorSet (pipeline.Layout, descriptorSet);

			pipeline.Bind (cmd);

			cmd.BindVertexBuffer (vbo, 0);
			cmd.Draw (36);

			uiPipeline.Bind (cmd);
			cmd.BindDescriptorSet (pipeline.Layout, dsVKVG);

			uiImage.SetLayout (cmd, VkImageAspectFlags.Color, VkImageLayout.ColorAttachmentOptimal, VkImageLayout.ShaderReadOnlyOptimal,
				VkPipelineStageFlags.ColorAttachmentOutput, VkPipelineStageFlags.FragmentShader);

			cmd.Draw (3, 1, 0, 0);

			uiImage.SetLayout (cmd, VkImageAspectFlags.Color, VkImageLayout.ShaderReadOnlyOptimal, VkImageLayout.ColorAttachmentOptimal,
				VkPipelineStageFlags.FragmentShader, VkPipelineStageFlags.BottomOfPipe);

			pipeline.RenderPass.End (cmd);
		}

		//in the thread of the keyboard
		void loadTexture (string path) {
			try {
				if (path.EndsWith ("ktx", StringComparison.OrdinalIgnoreCase))
					nextTexture = KTX.KTX.Load (presentQueue, cmdPool, path,
						VkImageUsageFlags.Sampled, VkMemoryPropertyFlags.DeviceLocal, true);
				else
					nextTexture = Image.Load (dev, path);
				updateViewRequested = true;
			} catch (Exception ex) {
				Console.WriteLine (ex);
				nextTexture = null;
			}
		}

		//in the main vulkan thread
		void updateTextureSet (){ 
			nextTexture.CreateView (VkImageViewType.Cube,VkImageAspectFlags.Color,6);
			nextTexture.CreateSampler ();

			nextTexture.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (descriptorSet, dsLayout.Bindings[1]);				
			uboUpdate.Write (dev, nextTexture.Descriptor);

			texture?.Dispose ();
			texture = nextTexture;
			nextTexture = null;
		}
		void updateMatrices () { 
			matrices.projection = Matrix4x4.CreatePerspectiveFieldOfView (Utils.DegreesToRadians (60f), (float)swapChain.Width / (float)swapChain.Height, 0.1f, 5.0f);
			matrices.view =
				Matrix4x4.CreateFromAxisAngle (Vector3.UnitZ, rotZ) *
				Matrix4x4.CreateFromAxisAngle (Vector3.UnitY, rotY) *
				Matrix4x4.CreateFromAxisAngle (Vector3.UnitX, rotX);

			uboMats.Update (matrices, (uint)Marshal.SizeOf<Matrices> ());
		}

		protected override void configureEnabledFeatures (VkPhysicalDeviceFeatures available_features, ref VkPhysicalDeviceFeatures enabled_features) {
			base.configureEnabledFeatures (available_features, ref enabled_features);
			enabled_features.textureCompressionBC = available_features.textureCompressionBC;
		}

		public override void UpdateView () {
			if (nextTexture != null) {
				dev.WaitIdle ();
				updateTextureSet ();
				buildCommandBuffers ();
			}else 
				updateMatrices ();

			updateViewRequested = false;
			dev.WaitIdle ();
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
					currentImgIndex++;
					if (currentImgIndex == imgPathes.Length)
						currentImgIndex = 0;
					loadTexture (imgPathes[currentImgIndex]);
					break;
				default:
					base.onKeyDown (key, scanCode, modifiers);
					break;
			}
		}

		protected override void OnResize () {
			dev.WaitIdle ();

			initUISurface ();

			updateMatrices ();

			if (frameBuffers!=null)
				for (int i = 0; i < swapChain.ImageCount; ++i)
					frameBuffers[i]?.Dispose ();

			frameBuffers = new Framebuffer[swapChain.ImageCount];

			for (int i = 0; i < swapChain.ImageCount; ++i) {
				frameBuffers[i] = new Framebuffer (pipeline.RenderPass, swapChain.Width, swapChain.Height,
					(pipeline.Samples == VkSampleCountFlags.SampleCount1) ? new Image[] {
						swapChain.images[i],
						null
					} : new Image[] {
						null,
						null,
						swapChain.images[i]
					});	
			}

			buildCommandBuffers ();

			dev.WaitIdle ();
		}
			
		protected override void Dispose (bool disposing) {
			if (disposing) {
				if (!isDisposed) {
					dev.WaitIdle ();
					pipeline.Dispose ();
					uiPipeline.Dispose ();
					dsLayout.Dispose ();
					for (int i = 0; i < swapChain.ImageCount; i++)
						frameBuffers[i].Dispose ();
					descriptorPool.Dispose ();
					texture.Dispose ();
					uboMats.Dispose ();

					vbo.Dispose ();

					uiImage?.Dispose ();
					vkvgSurf?.Dispose ();
					vkvgDev.Dispose ();
				}
			}

			base.Dispose (disposing);
		}
	}
}
