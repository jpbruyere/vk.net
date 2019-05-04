using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;
using VK;
using CVKL;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace deferred {
	class Program : VkWindow, Crow.IValueChange {
		static void Main (string[] args) {
			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}
		bool isRunning;

		protected override void render () {
			int idx = swapChain.GetNextImage ();

			lock (crow.RenderMutex) {
				if (idx < 0) {
					OnResize ();
					return;
				}

				presentQueue.Submit (cmds[idx], swapChain.presentComplete, drawComplete[idx]);
				presentQueue.Present (swapChain, drawComplete[idx]);
				presentQueue.WaitIdle ();
			}
		}

		#region crow
		vkvg.Device vkvgDev;
		Image uiImage;

		public Crow.Command CMDViewScenes, CMDViewEditor, CMDViewDebug, CMDViewMaterials;
		void init_crow_commands () {
			CMDViewScenes = new Crow.Command (new Action (() => loadWindow ("#deferred.main.crow", this))) { Caption = "Lighting", Icon = new Crow.SvgPicture ("#deferred.crow.svg"), CanExecute = true };
			CMDViewEditor = new Crow.Command (new Action (() => loadWindow ("#deferred.scenes.crow", this))) { Caption = "Scenes", Icon = new Crow.SvgPicture ("#deferred.crow.svg"), CanExecute = true };
			CMDViewDebug = new Crow.Command (new Action (() => loadWindow ("#deferred.debug.crow", this))) { Caption = "Debug", Icon = new Crow.SvgPicture ("#deferred.crow.svg"), CanExecute = true };
			CMDViewMaterials = new Crow.Command (new Action (() => loadWindow ("#deferred.materials.crow", this))) { Caption = "Materials", Icon = new Crow.SvgPicture ("#deferred.crow.svg"), CanExecute = true };
		}

		void onApplyMaterialChanges (object sender, Crow.MouseButtonEventArgs e) {
			Crow.ListBox lb = ((sender as Crow.Widget).Parent as Crow.Group).Children[0] as Crow.ListBox;
			renderer.model.materialUBO.Update (renderer.model.materials);
		}


		void initUISurface () {
			lock (crow.UpdateMutex) {
				uiImage?.Dispose ();
				uiImage = new CVKL.Image (dev, new VkImage ((ulong)crow.surf.VkImage.ToInt64 ()), VkFormat.B8g8r8a8Unorm,
					VkImageUsageFlags.Sampled, swapChain.Width, swapChain.Height);
				uiImage.SetName ("uiImage");
				uiImage.CreateView (VkImageViewType.ImageView2D, VkImageAspectFlags.Color);
				uiImage.CreateSampler (VkFilter.Nearest, VkFilter.Nearest, VkSamplerMipmapMode.Nearest, VkSamplerAddressMode.ClampToBorder);
				uiImage.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
			}
		}


		void crow_thread_func () {
			vkvgDev = new vkvg.Device (instance.Handle, phy.Handle, dev.VkDev.Handle, presentQueue.qFamIndex,
	   			vkvg.SampleCount.Sample_4, presentQueue.index);

			crow = new Crow.Interface (vkvgDev, 800, 600);

			isRunning = true;

			while (isRunning) {
				crow.Update ();
				Thread.Sleep (2);
			}

			dev.WaitIdle ();
			crow.Dispose ();
			vkvgDev.Dispose ();
		}

		#region IValueChange implementation
		public event EventHandler<Crow.ValueChangeEventArgs> ValueChanged;
		public virtual void NotifyValueChanged (string MemberName, object _value) {
			if (ValueChanged != null)
				ValueChanged.Invoke (this, new Crow.ValueChangeEventArgs (MemberName, _value));
		}
		#endregion

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
		public PbrModel.PbrMaterial[] Materials => renderer.model.materials;
		Crow.Interface crow;
		#endregion

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
				"../../../samples/data/models/DamagedHelmet/glTF/DamagedHelmet.gltf",
				"../../../samples/data/models/shadow.glb",
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
				"/mnt/devel/vulkan/glTF-Sample-Models-master/2.0/Sponza/glTF/Sponza.gltf",
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

			Thread crowThread = new Thread (crow_thread_func);
			crowThread.IsBackground = true;
			crowThread.Start ();

			while (crow == null)
				Thread.Sleep (5);

			initUISurface ();

			renderer = new DeferredPbrRenderer (dev, swapChain, presentQueue, camera.NearPlane, camera.FarPlane);
			renderer.LoadModel (transferQ, modelPathes[curModelIndex]);
			camera.Model = Matrix4x4.CreateScale (1f / Math.Max (Math.Max (renderer.modelAABB.Width, renderer.modelAABB.Height), renderer.modelAABB.Depth));

			init_crow_commands ();

			crow.Load ("#deferred.menu.crow").DataSource = this;
			//crow.Load ("#deferred.testImage.crow").DataSource = this;

			//crow.LoadIMLFragment (@"<Image Width='32' Height='32' Margin='2' HorizontalAlignment='Left' VerticalAlignment='Top' Path='/mnt/devel/gts/vkvg/build/data/tiger.svg' Background='Grey' MouseEnter='{Background=Blue}' MouseLeave='{Background=White}'/>");
		}

		void loadWindow (string path, object dataSource = null) {
			try {
				Crow.Widget w = crow.FindByName (path);
				if (w != null) {
					crow.PutOnTop (w);
					return;
				}
				w = crow.Load (path);
				w.Name = path;
				w.DataSource = dataSource;

			} catch (Exception ex) {
				System.Diagnostics.Debug.WriteLine (ex.ToString ());
			}
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

			if (rebuildBuffers) {
				buildCommandBuffers ();
				rebuildBuffers = false;
			}

			if (++frameCount > 20) {
				NotifyValueChanged ("fps", fps);
				frameCount = 0;
			}
		}
		protected override void OnResize () {
			crow.ProcessResize (new Crow.Rectangle (0, 0, (int)swapChain.Width, (int)swapChain.Height));

			initUISurface ();
			renderer.WriteUiImgDesciptor (uiImage);

			UpdateView ();
			renderer.Resize ();
			buildCommandBuffers ();
		}

		#region Mouse and keyboard
		protected override void onScroll (double xOffset, double yOffset) {
			if (KeyModifiers.HasFlag (Modifier.Shift))
				crow.ProcessMouseWheelChanged ((float)xOffset);
			else
				crow.ProcessMouseWheelChanged ((float)yOffset);
		}
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
					isRunning = false;
					Thread.Sleep (2);

					renderer.Dispose ();

					uiImage?.Dispose ();
				}
			}

			base.Dispose (disposing);
		}
	}
}
