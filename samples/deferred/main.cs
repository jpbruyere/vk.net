using System;
using System.Numerics;
using CVKL;
using Glfw;
using VK;

namespace deferred {
	class Deferred : VkWindow {
		static void Main (string[] args) {

			Instance.VALIDATION = true;
			Instance.DEBUG_UTILS = true;
			Instance.RENDER_DOC_CAPTURE = true;

			DeferredPbrRenderer.TEXTURE_ARRAY = true;
			DeferredPbrRenderer.NUM_SAMPLES = VkSampleCountFlags.SampleCount2;
			DeferredPbrRenderer.HDR_FORMAT = VkFormat.R16g16b16a16Sfloat;
			DeferredPbrRenderer.MRT_FORMAT = VkFormat.R32g32b32a32Sfloat;

			PbrModelTexArray.TEXTURE_DIM = 1024;

			using (Deferred vke = new Deferred ()) {
				vke.Run ();
			}
		}

		public override string[] EnabledDeviceExtensions => new string[] {
			Ext.D.VK_KHR_swapchain,
			Ext.D.VK_EXT_debug_marker
		};

		protected override void configureEnabledFeatures (VkPhysicalDeviceFeatures available_features, ref VkPhysicalDeviceFeatures enabled_features) {
			base.configureEnabledFeatures (available_features, ref enabled_features);

			enabled_features.samplerAnisotropy = available_features.samplerAnisotropy;
			enabled_features.sampleRateShading = available_features.sampleRateShading;
			enabled_features.geometryShader = available_features.geometryShader;

			enabled_features.textureCompressionBC = available_features.textureCompressionBC;
		}

		protected override void createQueues () {
			base.createQueues ();
			transferQ = new Queue (dev, VkQueueFlags.Transfer);
			computeQ = new Queue (dev, VkQueueFlags.Compute);
		}
		string[] cubemapPathes = {
			"data/textures/papermill.ktx",
			"data/textures/cubemap_yokohama_bc3_unorm.ktx",
			"data/textures/gcanyon_cube.ktx",
			"data/textures/pisa_cube.ktx",
			"data/textures/uffizi_cube.ktx",
		};
		string[] modelPathes = {
				//"/mnt/devel/gts/vkChess.net/data/models/chess.glb",
				"data/models/DamagedHelmet/glTF/DamagedHelmet.gltf",
				"data/models/shadow.glb",
				"data/models/Hubble.glb",
				"data/models/MER_static.glb",
				"data/models/ISS_stationary.glb",
			};

		int curModelIndex = 0;
		bool reloadModel;

		Queue transferQ, computeQ;
		DeferredPbrRenderer renderer;


		GraphicPipeline plToneMap;
		Framebuffer[] frameBuffers;
		DescriptorPool descriptorPool;
		DescriptorSetLayout dsLayout;
		DescriptorSet descriptorSet;



		Deferred () : base("deferred", 1024, 1024) {		
			camera = new Camera (Utils.DegreesToRadians (45f), 1f, 0.1f, 16f);
			camera.SetPosition (0, 0, 2);

			renderer = new DeferredPbrRenderer (presentQueue, cubemapPathes[2], swapChain.Width, swapChain.Height, camera.NearPlane, camera.FarPlane);
			renderer.LoadModel (transferQ, modelPathes[curModelIndex]);
			camera.Model = Matrix4x4.CreateScale (1f / Math.Max (Math.Max (renderer.modelAABB.Width, renderer.modelAABB.Height), renderer.modelAABB.Depth));

			init_final_pl ();
		}

		void init_final_pl() {
			descriptorPool = new DescriptorPool (dev, 1,
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler, 1)
			);

			GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, DeferredPbrRenderer.NUM_SAMPLES);

			cfg.Layout = new PipelineLayout (dev,
				new VkPushConstantRange (VkShaderStageFlags.Fragment, 2 * sizeof (float)),
				new DescriptorSetLayout (dev, 0,
					new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler)
					//new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler)
					));

			cfg.RenderPass = new RenderPass (dev, swapChain.ColorFormat, DeferredPbrRenderer.NUM_SAMPLES);

			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/FullScreenQuad.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/tone_mapping.frag.spv");

			plToneMap = new GraphicPipeline (cfg);

			descriptorSet = descriptorPool.Allocate (cfg.Layout.DescriptorSetLayouts[0]);
		}

		CommandBuffer cmdPbr;

		void buildCommandBuffers () {
			cmdPbr?.Free ();
			cmdPbr = cmdPool.AllocateAndStart ();
			renderer.buildCommandBuffers (cmdPbr);
			cmdPbr.End ();

			for (int i = 0; i < swapChain.ImageCount; ++i) {
				cmds[i]?.Free ();
				cmds[i] = cmdPool.AllocateAndStart ();

				renderer.hdrImgResolved.SetLayout (cmds[i], VkImageAspectFlags.Color, VkImageLayout.ColorAttachmentOptimal, VkImageLayout.ShaderReadOnlyOptimal,
					VkPipelineStageFlags.ColorAttachmentOutput, VkPipelineStageFlags.FragmentShader);

				plToneMap.RenderPass.Begin (cmds[i], frameBuffers[i]);

				cmds[i].SetViewport (frameBuffers[i].Width, frameBuffers[i].Height);
				cmds[i].SetScissor (frameBuffers[i].Width, frameBuffers[i].Height);

				plToneMap.Bind (cmds[i]);
				plToneMap.BindDescriptorSet (cmds[i], descriptorSet);

				cmds[i].PushConstant (plToneMap.Layout, VkShaderStageFlags.Fragment, 2 * sizeof(float), new float[] { renderer.exposure, renderer.gamma }, 0);

				cmds[i].Draw (3, 1, 0, 0);

				plToneMap.RenderPass.End (cmds[i]);

				renderer.hdrImgResolved.SetLayout (cmds[i], VkImageAspectFlags.Color,
					VkAccessFlags.ShaderRead, VkAccessFlags.ColorAttachmentWrite,
					VkImageLayout.ShaderReadOnlyOptimal, VkImageLayout.ColorAttachmentOptimal,
					VkPipelineStageFlags.FragmentShader, VkPipelineStageFlags.ColorAttachmentOutput);

				cmds[i].End ();
			}
		}

		public override void UpdateView () {
			renderer.UpdateView (camera);
			updateViewRequested = false;
#if WITH_SHADOWS
			if (renderer.shadowMapRenderer.updateShadowMap)
				renderer.shadowMapRenderer.update_shadow_map (cmdPool);
#endif
		}

		public override void Update () {
			if (reloadModel) {
				renderer.LoadModel (transferQ, modelPathes[curModelIndex]);
				reloadModel = false;
				camera.Model = Matrix4x4.CreateScale (1f / Math.Max (Math.Max (renderer.modelAABB.Width, renderer.modelAABB.Height), renderer.modelAABB.Depth));
				updateViewRequested = true;
				rebuildBuffers = true;
#if WITH_SHADOWS
				renderer.shadowMapRenderer.updateShadowMap = true;
#endif
			}

			if (rebuildBuffers) {
				buildCommandBuffers ();
				rebuildBuffers = false;
			}

		}
		protected override void render () {
			int idx = swapChain.GetNextImage ();
			if (idx < 0) {
				OnResize ();
				return;
			}

			if (cmds[idx] == null)
				return;

			presentQueue.Submit (cmdPbr, swapChain.presentComplete, renderer.DrawComplete);

			presentQueue.Submit (cmds[idx], renderer.DrawComplete, drawComplete[idx]);
			presentQueue.Present (swapChain, drawComplete[idx]);

			presentQueue.WaitIdle ();
		}
		protected override void OnResize () {
			dev.WaitIdle ();

			UpdateView ();
			renderer.Resize (swapChain.Width, swapChain.Height);

			if (frameBuffers != null)
				for (int i = 0; i < swapChain.ImageCount; ++i)
					frameBuffers[i]?.Dispose ();

			frameBuffers = new Framebuffer[swapChain.ImageCount];

			for (int i = 0; i < swapChain.ImageCount; ++i) {
				frameBuffers[i] = new Framebuffer (plToneMap.RenderPass, swapChain.Width, swapChain.Height,
					(plToneMap.Samples == VkSampleCountFlags.SampleCount1) ? new Image[] {
						swapChain.images[i],
					} : new Image[] {
						null,
						swapChain.images[i]
					});
			}

			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (plToneMap.Layout.DescriptorSetLayouts[0]);
			uboUpdate.Write (dev, descriptorSet, renderer.hdrImgResolved.Descriptor);

			buildCommandBuffers ();

			dev.WaitIdle ();
		}

		#region Mouse and keyboard
		protected override void onScroll (double xOffset, double yOffset) {
		}
		protected override void onMouseMove (double xPos, double yPos) {
			double diffX = lastMouseX - xPos;
			double diffY = lastMouseY - yPos;
			if (MouseButton[0]) {
				camera.Rotate ((float)-diffX, (float)-diffY);
			} else if (MouseButton[1]) {
				camera.SetZoom ((float)diffY);
			} else
				return;

			updateViewRequested = true;
		}
		protected override void onKeyDown (Key key, int scanCode, Modifier modifiers) {
			switch (key) {
				case Key.F:
					if (modifiers.HasFlag (Modifier.Shift)) {
						renderer.debugFace--;
						if (renderer.debugFace < 0)
							renderer.debugFace = 5;
					} else {
						renderer.debugFace++;
						if (renderer.debugFace >= 5)
							renderer.debugFace = 0;
					}
					rebuildBuffers = true;
					break;
				case Key.M:
					if (modifiers.HasFlag (Modifier.Shift)) {
						renderer.debugMip--;
						if (renderer.debugMip < 0)
							renderer.debugMip = (int)renderer.envCube.prefilterCube.CreateInfo.mipLevels - 1;
					} else {
						renderer.debugMip++;
						if (renderer.debugMip >= renderer.envCube.prefilterCube.CreateInfo.mipLevels)
							renderer.debugMip = 0;
					}
					rebuildBuffers = true;
					break;
				case Key.L:
					if (modifiers.HasFlag (Modifier.Shift)) {
						renderer.lightNumDebug--;
						if (renderer.lightNumDebug < 0)
							renderer.lightNumDebug = (int)renderer.lights.Length - 1;
					} else {
						renderer.lightNumDebug++;
						if (renderer.lightNumDebug >= renderer.lights.Length)
							renderer.lightNumDebug = 0;
					}
					rebuildBuffers = true;
					break;
				case Key.Keypad0:
				case Key.Keypad1:
				case Key.Keypad2:
				case Key.Keypad3:
				case Key.Keypad4:
				case Key.Keypad5:
				case Key.Keypad6:
				case Key.Keypad7:
				case Key.Keypad8:
				case Key.Keypad9:
					renderer.currentDebugView = (DeferredPbrRenderer.DebugView)(int)key-320;
					rebuildBuffers = true;
					break;
				case Key.KeypadDivide:
					renderer.currentDebugView = DeferredPbrRenderer.DebugView.irradiance;
					rebuildBuffers = true;
					break;
				case Key.S:
					if (modifiers.HasFlag (Modifier.Control)) {
						renderer.pipelineCache.Save ();
						Console.WriteLine ($"Pipeline Cache saved.");
					} else {
						renderer.currentDebugView = DeferredPbrRenderer.DebugView.shadowMap;
						rebuildBuffers = true; 
					}
					break;
				case Key.Up:
					if (modifiers.HasFlag (Modifier.Shift))
						renderer.MoveLight(-Vector4.UnitZ);
					else
						camera.Move (0, 0, 1);
					break;
				case Key.Down:
					if (modifiers.HasFlag (Modifier.Shift))
						renderer.MoveLight (Vector4.UnitZ);
					else
						camera.Move (0, 0, -1);
					break;
				case Key.Left:
					if (modifiers.HasFlag (Modifier.Shift))
						renderer.MoveLight (-Vector4.UnitX);
					else
						camera.Move (1, 0, 0);
					break;
				case Key.Right:
					if (modifiers.HasFlag (Modifier.Shift))
						renderer.MoveLight (Vector4.UnitX);
					else
						camera.Move (-1, 0, 0);
					break;
				case Key.PageUp:
					if (modifiers.HasFlag (Modifier.Shift))
						renderer.MoveLight (Vector4.UnitY);
					else
						camera.Move (0, 1, 0);
					break;
				case Key.PageDown:
					if (modifiers.HasFlag (Modifier.Shift))
						renderer.MoveLight (-Vector4.UnitY);
					else
						camera.Move (0, -1, 0);
					break;
				case Key.F2:
					if (modifiers.HasFlag (Modifier.Shift))
						renderer.exposure -= 0.3f;
					else
						renderer.exposure += 0.3f;
					break;
				case Key.F3:
					if (modifiers.HasFlag (Modifier.Shift))
						renderer.gamma -= 0.1f;
					else
						renderer.gamma += 0.1f;
					break;
				case Key.F4:
					if (camera.Type == Camera.CamType.FirstPerson)
						camera.Type = Camera.CamType.LookAt;
					else
						camera.Type = Camera.CamType.FirstPerson;
					Console.WriteLine ($"camera type = {camera.Type}");
					break;
				case Key.KeypadAdd:
					curModelIndex++;
					if (curModelIndex >= modelPathes.Length)
						curModelIndex = 0;
					reloadModel = true;
					break;
				case Key.KeypadSubtract:
					curModelIndex--;
					if (curModelIndex < 0)
						curModelIndex = modelPathes.Length -1;
					reloadModel = true;
					break;
				default:
					base.onKeyDown (key, scanCode, modifiers);
					return;
			}
			updateViewRequested = true;
		}
		#endregion

		protected override void Dispose (bool disposing) {
			if (disposing) {
				if (!isDisposed) {
					renderer.Dispose ();
				}
			}
			base.Dispose (disposing);
		}
	}
}
