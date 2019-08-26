using System;
using System.Numerics;
using Glfw;
using VK;
using CVKL;
using System.Collections.Generic;
using System.Linq;

namespace deferred {
	class Program : Crow.CrowWin {
		static void Main (string[] args) {
			/*Instance.DEBUG_UTILS = true;
			Instance.VALIDATION = true;
			Instance.RENDER_DOC_CAPTURE = true;*/
			DeferredPbrRenderer.TEXTURE_ARRAY = true;
			DeferredPbrRenderer.NUM_SAMPLES = VkSampleCountFlags.SampleCount1;
			DeferredPbrRenderer.HDR_FORMAT = VkFormat.R32g32b32a32Sfloat;
			DeferredPbrRenderer.MRT_FORMAT = VkFormat.R16g16b16a16Sfloat;

			PbrModelTexArray.TEXTURE_DIM = 1024;

			using (Program vke = new Program ()) {
				vke.Run ();			
			}
		}

		#region crow ui
		public Crow.Command CMDViewScenes, CMDViewEditor, CMDViewDebug, CMDViewMaterials;
		void init_crow_commands () {
			CMDViewScenes = new Crow.Command (new Action (() => loadWindow ("#deferred.main.crow", this))) { Caption = "Lighting", Icon = new Crow.SvgPicture ("#deferred.crow.svg"), CanExecute = true };
			CMDViewEditor = new Crow.Command (new Action (() => loadWindow ("#deferred.scenes.crow", this))) { Caption = "Scenes", Icon = new Crow.SvgPicture ("#deferred.crow.svg"), CanExecute = true };
			CMDViewDebug = new Crow.Command (new Action (() => loadWindow ("#deferred.debug.crow", this))) { Caption = "Debug", Icon = new Crow.SvgPicture ("#deferred.crow.svg"), CanExecute = true };
			CMDViewMaterials = new Crow.Command (new Action (() => loadWindow ("#deferred.materials.crow", this))) { Caption = "Materials", Icon = new Crow.SvgPicture ("#deferred.crow.svg"), CanExecute = true };
		}

		public DeferredPbrRenderer.DebugView CurrentDebugView {
			get { return renderer.currentDebugView; }
			set {
				if (value == renderer.currentDebugView)
					return;
				lock(crow.UpdateMutex)
					renderer.currentDebugView = value;
				rebuildBuffers = true;
				NotifyValueChanged ("CurrentDebugView", renderer.currentDebugView);
			}
		}

		public float Gamma {
			get { return renderer.matrices.gamma; }
			set {
				if (value == renderer.matrices.gamma)
					return;
				renderer.matrices.gamma = value;
				NotifyValueChanged ("Gamma", value);
				updateViewRequested = true;
			}
		}
		public float Exposure {
			get { return renderer.matrices.exposure; }
			set {
				if (value == renderer.matrices.exposure)
					return;
				renderer.matrices.exposure = value;
				NotifyValueChanged ("Exposure", value);
				updateViewRequested = true;
			}
		}
		public float LightStrength {
			get { return renderer.lights[renderer.lightNumDebug].color.X; }
			set {
				if (value == renderer.lights[renderer.lightNumDebug].color.X)
					return;
				renderer.lights[renderer.lightNumDebug].color = new Vector4(value);
				NotifyValueChanged ("LightStrength", value);
				renderer.uboLights.Update (renderer.lights);
			}
		}
		public List<DeferredPbrRenderer.Light> Lights => renderer.lights.ToList ();
		public List<Model.Scene> Scenes => renderer.model.Scenes;
		#endregion

		public override string[] EnabledDeviceExtensions => new string[] {
			Ext.D.VK_KHR_swapchain,
			Ext.D.VK_EXT_debug_marker
		};

		protected override void configureEnabledFeatures (VkPhysicalDeviceFeatures available_features, ref VkPhysicalDeviceFeatures features) {
			base.configureEnabledFeatures (available_features, ref features);

			features.samplerAnisotropy = available_features.samplerAnisotropy;
			features.sampleRateShading = available_features.sampleRateShading;
			features.geometryShader = available_features.geometryShader;
			features.pipelineStatisticsQuery = true;

			if (available_features.textureCompressionETC2) {
				features.textureCompressionETC2 = true;
				Image.DefaultTextureFormat = VkFormat.Etc2R8g8b8a8UnormBlock;
			}else if (available_features.textureCompressionBC) {
				//features.textureCompressionBC = true;
				//Image.DefaultTextureFormat = VkFormat.Bc3UnormBlock;
			}


		}

		protected override void createQueues () {
			base.createQueues ();
			transferQ = new Queue (dev, VkQueueFlags.Transfer);
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
				//"/home/jp/gltf/jaguar/scene.gltf",
				"data/models/DamagedHelmet/glTF/DamagedHelmet.gltf",
				"data/models/shadow.glb",
				"data/models/Hubble.glb",
				"data/models/MER_static.glb",
				"data/models/ISS_stationary.glb",
			};

		int curModelIndex = 0;
		bool reloadModel;

		Queue transferQ;
		DeferredPbrRenderer renderer;
		PipelineStatisticsQueryPool statPool;
		TimestampQueryPool timestampQPool;
		ulong[] results;

		DebugReport dbgRepport;

		Program () : base() {

			if (Instance.DEBUG_UTILS)
				dbgRepport = new DebugReport (instance,
					VkDebugReportFlagsEXT.ErrorEXT
					| VkDebugReportFlagsEXT.DebugEXT
					| VkDebugReportFlagsEXT.WarningEXT
					| VkDebugReportFlagsEXT.PerformanceWarningEXT
				);

			camera = new Camera (Utils.DegreesToRadians (45f), 1f, 0.1f, 16f);
			camera.SetPosition (0, 0, 2);

			renderer = new DeferredPbrRenderer (dev, swapChain, presentQueue, cubemapPathes[2], camera.NearPlane, camera.FarPlane);
			renderer.LoadModel (transferQ, modelPathes[curModelIndex]);
			camera.Model = Matrix4x4.CreateScale (1f / Math.Max (Math.Max (renderer.modelAABB.Width, renderer.modelAABB.Height), renderer.modelAABB.Depth));

			statPool = new PipelineStatisticsQueryPool (dev,
				VkQueryPipelineStatisticFlags.InputAssemblyVertices |
				VkQueryPipelineStatisticFlags.InputAssemblyPrimitives |
				VkQueryPipelineStatisticFlags.ClippingInvocations |
				VkQueryPipelineStatisticFlags.ClippingPrimitives |
				VkQueryPipelineStatisticFlags.FragmentShaderInvocations);

			timestampQPool = new TimestampQueryPool (dev);

			init_crow_commands ();

			crow.Load ("#deferred.menu.crow").DataSource = this;

		}

		protected override void recordDraw (CommandBuffer cmd, int imageIndex) {
			statPool.Begin (cmd);
			renderer.buildCommandBuffers (cmd, imageIndex);
			statPool.End (cmd);
		}

		public override void UpdateView () {
			renderer.UpdateView (camera);
			updateViewRequested = false;
#if WITH_SHADOWS
			if (renderer.shadowMapRenderer.updateShadowMap)
				renderer.shadowMapRenderer.update_shadow_map (cmdPool);
#endif
		}

		int frameCount = 0;
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

			base.Update ();

			if (++frameCount > 20) {
				NotifyValueChanged ("fps", fps);
				frameCount = 0;
			}

			results = statPool.GetResults ();
		}
		protected override void OnResize () {		
			renderer.Resize ();
			base.OnResize ();
		}

		#region Mouse and keyboard
		protected override void onMouseMove (double xPos, double yPos) {
			if (crow.ProcessMouseMove ((int)xPos, (int)yPos))
				return;

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
		protected override void onMouseButtonDown (Glfw.MouseButton button) {
			if (crow.ProcessMouseButtonDown ((Crow.MouseButton)button))
				return;
			base.onMouseButtonDown (button);
		}
		protected override void onMouseButtonUp (Glfw.MouseButton button) {
			if (crow.ProcessMouseButtonUp ((Crow.MouseButton)button))
				return;
			base.onMouseButtonUp (button);
		}
		protected override void onKeyDown (Key key, int scanCode, Modifier modifiers) {
			if (crow.ProcessKeyDown ((Crow.Key)key))
				return;
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
						renderer.matrices.exposure -= 0.3f;
					else
						renderer.matrices.exposure += 0.3f;
					break;
				case Key.F3:
					if (modifiers.HasFlag (Modifier.Shift))
						renderer.matrices.gamma -= 0.1f;
					else
						renderer.matrices.gamma += 0.1f;
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
		protected override void onKeyUp (Key key, int scanCode, Modifier modifiers) {
			if (crow.ProcessKeyUp ((Crow.Key)key))
				return;
		}
		protected override void onChar (CodePoint cp) {
			if (crow.ProcessKeyPress (cp.ToChar ()))
				return;
		}
		#endregion

		protected override void Dispose (bool disposing) {
			if (disposing) {
				if (!isDisposed) {
					renderer.Dispose ();
					statPool.Dispose ();
					timestampQPool.Dispose ();
					dbgRepport?.Dispose ();
				}
			}

			base.Dispose (disposing);
		}
	}
}
