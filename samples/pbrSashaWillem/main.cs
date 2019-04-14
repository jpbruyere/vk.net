using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;
using VK;
using VKE;

namespace PbrSample {
	class Program : VkWindow{	
		static void Main (string[] args) {
			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}

		VkSampleCountFlags samples = VkSampleCountFlags.SampleCount4;

		Camera Camera = new Camera (Utils.DegreesToRadians (60f), 1f);

		Framebuffer[] frameBuffers;

		PBRPipeline pbrPipeline;

		Program () {
			//UpdateFrequency = 20;
			Camera.Model = Matrix4x4.CreateRotationX (Utils.DegreesToRadians (-90)) * Matrix4x4.CreateRotationY (Utils.DegreesToRadians (180));
			Camera.SetRotation (-0.1f,-0.4f);
			Camera.SetPosition (0, 0, -3);

			pbrPipeline = new PBRPipeline(presentQueue,
				new RenderPass (dev, swapChain.ColorFormat, dev.GetSuitableDepthFormat (), samples));
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
			pbrPipeline.RenderPass.Begin (cmd, fb);

			cmd.SetViewport (fb.Width, fb.Height);
			cmd.SetScissor (fb.Width, fb.Height);

			pbrPipeline.RecordDraw (cmd);

			pbrPipeline.RenderPass.End (cmd);
		}

		#region update
		void updateMatrices () {
			Camera.AspectRatio = (float)swapChain.Width / swapChain.Height;

			pbrPipeline.matrices.projection = Camera.Projection;
			pbrPipeline.matrices.view = Camera.View;
			pbrPipeline.matrices.model = Camera.Model;
			pbrPipeline.uboMats.Update (pbrPipeline.matrices, (uint)Marshal.SizeOf<PBRPipeline.Matrices> ());
			pbrPipeline.matrices.view *= Matrix4x4.CreateTranslation (-pbrPipeline.matrices.view.Translation);
			pbrPipeline.matrices.model = Matrix4x4.Identity;
			pbrPipeline.uboMats.Update (pbrPipeline.matrices, (uint)Marshal.SizeOf<PBRPipeline.Matrices> (), (uint)Marshal.SizeOf<PBRPipeline.Matrices> ());
		}

		public override void UpdateView () {
			updateMatrices ();
			updateViewRequested = false;
		}
		public override void Update () {
		}
		#endregion

		protected override void OnResize () {
			updateMatrices ();

			if (frameBuffers != null)
				for (int i = 0; i < swapChain.ImageCount; ++i)
					frameBuffers[i]?.Dispose ();

			frameBuffers = new Framebuffer[swapChain.ImageCount];

			for (int i = 0; i < swapChain.ImageCount; ++i) {
				frameBuffers[i] = new Framebuffer (pbrPipeline.RenderPass, swapChain.Width, swapChain.Height,
					(pbrPipeline.RenderPass.Samples == VkSampleCountFlags.SampleCount1) ? new Image[] {
						swapChain.images[i],
						null
					} : new Image[] {
						null,
						null,
						swapChain.images[i]
					});
			}

			buildCommandBuffers ();
		}


		#region Mouse and keyboard
		protected override void onMouseMove (double xPos, double yPos) {
			double diffX = lastMouseX - xPos;
			double diffY = lastMouseY - yPos;
			if (MouseButton[0]) {
				Camera.Rotate ((float)-diffX, (float)-diffY);
			} else if (MouseButton[1]) {
				Camera.Zoom ((float)diffY);
			}

			updateViewRequested = true;
		}

		protected override void onKeyDown (Key key, int scanCode, Modifier modifiers) {
			switch (key) {
				case Key.Up:
					if (modifiers.HasFlag (Modifier.Shift))
						pbrPipeline.matrices.lightPos += new Vector4 (0, 0, 1, 0);
					else
						Camera.Move (0, 0, 1);
					break;
				case Key.Down:
					if (modifiers.HasFlag (Modifier.Shift))
						pbrPipeline.matrices.lightPos += new Vector4 (0, 0, -1, 0);
					else
						Camera.Move (0, 0, -1);
					break;
				case Key.Left:
					if (modifiers.HasFlag (Modifier.Shift))
						pbrPipeline.matrices.lightPos += new Vector4 (1, 0, 0, 0);
					else
						Camera.Move (1, 0, 0);
					break;
				case Key.Right:
					if (modifiers.HasFlag (Modifier.Shift))
						pbrPipeline.matrices.lightPos += new Vector4 (-1, 0, 0, 0);
					else
						Camera.Move (-1, 0, 0);
					break;
				case Key.PageUp:
					if (modifiers.HasFlag (Modifier.Shift))
						pbrPipeline.matrices.lightPos += new Vector4 (0, 1, 0, 0);
					else
						Camera.Move (0, 1, 0);
					break;
				case Key.PageDown:
					if (modifiers.HasFlag (Modifier.Shift))
						pbrPipeline.matrices.lightPos += new Vector4 (0, -1, 0, 0);
					else
						Camera.Move (0, -1, 0);
					break;
				case Key.F1:
					if (modifiers.HasFlag (Modifier.Shift))
						pbrPipeline.matrices.exposure -= 0.3f;
					else
						pbrPipeline.matrices.exposure += 0.3f;
					break;
				case Key.F2:
					if (modifiers.HasFlag (Modifier.Shift))
						pbrPipeline.matrices.gamma -= 0.1f;
					else
						pbrPipeline.matrices.gamma += 0.1f;
					break;
				case Key.F3:
					if (Camera.Type == Camera.CamType.FirstPerson)
						Camera.Type = Camera.CamType.LookAt;
					else
						Camera.Type = Camera.CamType.FirstPerson;
					Console.WriteLine ($"camera type = {Camera.Type}");
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
					dev.WaitIdle ();
					for (int i = 0; i < swapChain.ImageCount; ++i)
						frameBuffers[i]?.Dispose ();

					pbrPipeline.Dispose ();
				}
			}

			base.Dispose (disposing);
		}
	}
}
