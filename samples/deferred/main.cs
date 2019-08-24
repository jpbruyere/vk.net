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
			//Instance.RENDER_DOC_CAPTURE = true;

			DeferredPbrRenderer.TEXTURE_ARRAY = true;
			DeferredPbrRenderer.NUM_SAMPLES = VkSampleCountFlags.SampleCount1;

			PbrModelTexArray.TEXTURE_DIM = 1024;

			using (Deferred vke = new Deferred ()) {
				vke.Run ();
			}
		}

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

		Queue transferQ;
		DeferredPbrRenderer renderer;

		CVKL.DebugUtils.Messenger dbgmsg;

		Deferred () : base() {

			if (Instance.DEBUG_UTILS)
				dbgmsg = new CVKL.DebugUtils.Messenger (instance);

			camera = new Camera (Utils.DegreesToRadians (45f), 1f, 0.1f, 16f);
			camera.SetPosition (0, 0, 2);

			renderer = new DeferredPbrRenderer (dev, swapChain, presentQueue, cubemapPathes[0], camera.NearPlane, camera.FarPlane);
			renderer.LoadModel (transferQ, modelPathes[curModelIndex]);
			camera.Model = Matrix4x4.CreateScale (1f / Math.Max (Math.Max (renderer.modelAABB.Width, renderer.modelAABB.Height), renderer.modelAABB.Depth));

		}

		void buildCommandBuffers () {
			for (int i = 0; i < swapChain.ImageCount; ++i) {
				cmds[i]?.Free ();
				cmds[i] = cmdPool.AllocateAndStart ();
				renderer.buildCommandBuffers (cmds[i], i);
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
		protected override void OnResize () {
			UpdateView ();
			renderer.Resize ();
			buildCommandBuffers ();
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
		#endregion

		protected override void Dispose (bool disposing) {
			if (disposing) {
				if (!isDisposed) {
					renderer.Dispose ();
					dbgmsg?.Dispose ();
				}
			}
			base.Dispose (disposing);
		}
	}
}
