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

		protected override void configureEnabledFeatures (ref VkPhysicalDeviceFeatures features) {
			base.configureEnabledFeatures (ref features);
			features.samplerAnisotropy = true;
			features.sampleRateShading = true;
		}
		VkSampleCountFlags samples = VkSampleCountFlags.SampleCount4;
		VkFormat hdrFormat = VkFormat.R32g32b32a32Sfloat;

		DeferredPbrRenderer renderer;

		Program () {
			camera = new Camera (Utils.DegreesToRadians (45f), 1f, 0.1f, 16f);
			camera.SetPosition (0, 0, 3);

			renderer = new DeferredPbrRenderer (dev, swapChain, presentQueue, camera.NearPlane, camera.FarPlane);

			camera.Model = Matrix4x4.CreateScale (1f / Math.Max (Math.Max (renderer.modelAABB.Width, renderer.modelAABB.Height), renderer.modelAABB.Depth));
		}



		void buildCommandBuffers () {
			cmdPool.FreeCommandBuffers (cmds);
			cmds = cmdPool.AllocateCommandBuffer (swapChain.ImageCount);
			renderer.buildCommandBuffers (cmds);
		}



		#region update
		public override void UpdateView () {
			renderer.UpdateView (camera);
			updateViewRequested = false;
		}

		#endregion

		protected override void OnResize () {
			UpdateView ();
			renderer.Resize ();
			buildCommandBuffers ();
		}


		public override void Update () {
			if (!rebuildBuffers)
				return;
			buildCommandBuffers ();
			rebuildBuffers = false;
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
						if (renderer.envCube.debugFace > 5)
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
						if (renderer.envCube.debugMip > renderer.envCube.prefilterCube.CreateInfo.mipLevels)
							renderer.envCube.debugMip = 0;
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
				case Key.Up:
					if (modifiers.HasFlag (Modifier.Shift))
						renderer.lightPos -= Vector4.UnitZ;
					else
						camera.Move (0, 0, 1);
					break;
				case Key.Down:
					if (modifiers.HasFlag (Modifier.Shift))
						renderer.lightPos += Vector4.UnitZ;
					else
						camera.Move (0, 0, -1);
					break;
				case Key.Left:
					if (modifiers.HasFlag (Modifier.Shift))
						renderer.lightPos -= Vector4.UnitX;
					else
						camera.Move (1, 0, 0);
					break;
				case Key.Right:
					if (modifiers.HasFlag (Modifier.Shift))
						renderer.lightPos += Vector4.UnitX;
					else
						camera.Move (-1, 0, 0);
					break;
				case Key.PageUp:
					if (modifiers.HasFlag (Modifier.Shift))
						renderer.lightPos += Vector4.UnitY;
					else
						camera.Move (0, 1, 0);
					break;
				case Key.PageDown:
					if (modifiers.HasFlag (Modifier.Shift))
						renderer.lightPos -= Vector4.UnitY;
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
				case Key.S:
					renderer.pipelineCache.Save ();
					Console.WriteLine ($"Pipeline Cache saved.");
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
