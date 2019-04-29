using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;
using VK;
using CVKL;

namespace deferred {
	class Program : VkWindow{	
		static void Main (string[] args) {
			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}

		protected override void configureEnabledFeatures (VkPhysicalDeviceFeatures available_features, ref VkPhysicalDeviceFeatures features) {
			base.configureEnabledFeatures (available_features, ref features);

			features.samplerAnisotropy = available_features.samplerAnisotropy;
			features.sampleRateShading = available_features.sampleRateShading;
			features.geometryShader = available_features.geometryShader;

			if (available_features.textureCompressionBC) { 
			}
		}

		protected override void createQueues () {
			base.createQueues ();
			transferQ = new Queue (dev, VkQueueFlags.Transfer);
		}

		string[] modelPathes = {
				"../data/models/DamagedHelmet/glTF/DamagedHelmet.gltf",
				"../data/models/shadow.glb",
				//"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/Avocado/glTF/Avocado.gltf",
				//"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/BarramundiFish/glTF/BarramundiFish.gltf",
				//"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/BoomBoxWithAxes/glTF/BoomBoxWithAxes.gltf",
				//"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/Box/glTF/Box.gltf",
				//"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/EnvironmentTest/glTF/EnvironmentTest.gltf",
				//"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/MetalRoughSpheres/glTF/MetalRoughSpheres.gltf",
				//"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/OrientationTest/glTF/OrientationTest.gltf",
				//"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/Buggy/glTF/Buggy.gltf",
				//"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/2CylinderEngine/glTF-Embedded/2CylinderEngine.gltf",
				//"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/FlightHelmet/glTF/FlightHelmet.gltf",
				//"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/GearboxAssy/glTF/GearboxAssy.gltf",
				//"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/Lantern/glTF/Lantern.gltf",
				//"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/SciFiHelmet/glTF/SciFiHelmet.gltf",
				//"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/Sponza/glTF/Sponza.gltf",
				//"/mnt/devel/vkChess/data/chess.gltf",
				//"/home/jp/gltf/camaro/scene.gltf",
				//"/home/jp/gltf/chessold/scene.gltf",
				//"../data/models/Hubble.glb",
				//"../data/models/MER_static.glb",
				//"../data/models/ISS_stationary.glb",
			};

		int curModelIndex = 0;
		bool reloadModel;

		Queue transferQ;
		DeferredPbrRenderer renderer;

		#if WITH_VKVG
		vkvg.Device vkvgDev;
		vkvg.Surface vkvgSurf;
		Image uiImage;

		void initUISurface () {
			uiImage?.Dispose ();
			vkvgSurf?.Dispose ();
			vkvgSurf = new vkvg.Surface (vkvgDev, (int)swapChain.Width, (int)swapChain.Height);
			uiImage = new Image (dev, new VkImage ((ulong)vkvgSurf.VkImage.ToInt64 ()), VkFormat.B8g8r8a8Unorm,
				VkImageUsageFlags.ColorAttachment, (uint)vkvgSurf.Width, (uint)vkvgSurf.Height);
			uiImage.CreateView (VkImageViewType.ImageView2D, VkImageAspectFlags.Color);
			uiImage.CreateSampler (VkFilter.Nearest, VkFilter.Nearest, VkSamplerMipmapMode.Nearest, VkSamplerAddressMode.ClampToBorder);
			uiImage.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
			uiImage.SetName ("uiImage");
		}

		void vkvgDraw () {
			using (vkvg.Context ctx = new vkvg.Context (vkvgSurf)) {
				ctx.Operator = vkvg.Operator.Clear;
				ctx.Paint ();
				ctx.Operator = vkvg.Operator.Over;

				ctx.LineWidth = 1;
				ctx.SetSource (0.1, 0.1, 0.1, 0.8);
				ctx.Rectangle (5.5, 5.5, 320, 300);
				ctx.FillPreserve ();
				ctx.Flush ();
				ctx.SetSource (0.8, 0.8, 0.8);
				ctx.Stroke ();

				ctx.FontFace = "mono";
				ctx.FontSize = 8;
				int x = 16;
				int y = 40, dy = 16;
				ctx.MoveTo (x, y);
				ctx.ShowText (string.Format ($"fps:     {fps,5} "));
				ctx.MoveTo (x + 200, y - 0.5);
				ctx.Rectangle (x + 200, y - 8.5, 0.1 * fps, 10);
				ctx.SetSource (0.1, 0.9, 0.1);
				ctx.Fill ();
				ctx.SetSource (0.8, 0.8, 0.8);
				y += dy;
				ctx.MoveTo (x, y);
				ctx.ShowText (string.Format ($"Exposure:{renderer.matrices.exposure,5} "));
				y += dy;
				ctx.MoveTo (x, y);
				ctx.ShowText (string.Format ($"Gamma:   {renderer.matrices.gamma,5} "));
				y += dy;
				ctx.MoveTo (x, y);
				ctx.ShowText (string.Format ($"Light pos:   {renderer.lights[0].position.ToString ()} "));
				y += dy;
				ctx.MoveTo (x, y);
				ctx.ShowText (string.Format ($"{"Debug draw (numpad 0->9)",-30} : {renderer.currentDebugView.ToString ()} "));
				y += dy;
				ctx.MoveTo (x, y);
				ctx.ShowText (string.Format ($"{"Debug Prefil Face: (f)",-30} : {renderer.envCube.debugFace.ToString ()} "));
				y += dy;
				ctx.MoveTo (x, y);
				ctx.ShowText (string.Format ($"{"Debug Prefil Mip: (m)",-30} : {renderer.envCube.debugMip.ToString ()} "));

				//drawResources (ctx);
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

				Resource r = mp.First;
				while (r != null) {
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
					ctx.Rectangle (0.5f+ x + byteWidth * r.poolOffset, 0.5f+ y, width, memPoolHeight);
					ctx.FillPreserve ();
					ctx.SetSource (0.01f, 0.01f, 0.01f);
					ctx.Stroke ();
					r = r.next;
				}
				y += memPoolHeight;
			}
		}
		#endif

		Program () : base(true) {
			camera = new Camera (Utils.DegreesToRadians (45f), 1f, 0.1f, 16f);
			camera.SetPosition (0, 0, 2);

			#if WITH_VKVG
			vkvgDev = new vkvg.Device (instance.Handle, phy.Handle, dev.VkDev.Handle, presentQueue.qFamIndex,
				vkvg.SampleCount.Sample_4, presentQueue.index);

			initUISurface ();
			#endif

			renderer = new DeferredPbrRenderer (dev, swapChain, presentQueue, camera.NearPlane, camera.FarPlane);
			renderer.LoadModel (transferQ, modelPathes[curModelIndex]);
			camera.Model = Matrix4x4.CreateScale (1f / Math.Max (Math.Max (renderer.modelAABB.Width, renderer.modelAABB.Height), renderer.modelAABB.Depth));
		}

		void buildCommandBuffers () {
			cmdPool.FreeCommandBuffers (cmds);
			cmds = cmdPool.AllocateCommandBuffer (swapChain.ImageCount);
			renderer.buildCommandBuffers (cmds);
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
#if WITH_VKVG
			vkvgDraw ();
#endif
		}
		protected override void OnResize () {
#if WITH_VKVG
			initUISurface ();
			renderer.WriteUiImgDesciptor (uiImage);
#endif

			UpdateView ();
			renderer.Resize ();
			buildCommandBuffers ();
		}

#region Mouse and keyboard
		protected override void onKeyDown (Key key, int scanCode, Modifier modifiers) {
			switch (key) {
				case Key.F:
					if (modifiers.HasFlag (Modifier.Shift)) {
						renderer.envCube.debugFace--;
						if (renderer.envCube.debugFace < 0)
							renderer.envCube.debugFace = 5;
					} else {
						renderer.envCube.debugFace++;
						if (renderer.envCube.debugFace >= 5)
							renderer.envCube.debugFace = 0;
					}
					rebuildBuffers = true;
					break;
				case Key.M:
					if (modifiers.HasFlag (Modifier.Shift)) {
						renderer.envCube.debugMip--;
						if (renderer.envCube.debugMip < 0)
							renderer.envCube.debugMip = (int)renderer.envCube.prefilterCube.CreateInfo.mipLevels - 1;
					} else {
						renderer.envCube.debugMip++;
						if (renderer.envCube.debugMip >= renderer.envCube.prefilterCube.CreateInfo.mipLevels)
							renderer.envCube.debugMip = 0;
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
#if WITH_VKVG
					uiImage?.Dispose ();
					vkvgSurf?.Dispose ();
					vkvgDev.Dispose ();
#endif
				}
			}

			base.Dispose (disposing);
		}
	}
}
