using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;
using VKE;
using Vulkan;
using Buffer = VKE.Buffer;

namespace ModelSample {
	class Program : VkWindow {
		static void Main (string[] args) {
			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}

		double lastMouseX, lastMouseY;

		VkSampleCountFlags samples = VkSampleCountFlags.Count8;
		VkFormat depthFormat;

		Image depthTexture, colorTexture;
		Framebuffer[] frameBuffers;

		Renderer pbrRenderer;

		Program () : base () {
		
			depthFormat = dev.GetSuitableDepthFormat ();
			frameBuffers = new Framebuffer[swapChain.ImageCount];

			pbrRenderer = new Renderer (dev, depthFormat, swapChain.ColorFormat);
			pbrRenderer.LoadModel (presentQueue, cmdPool, "data/DamagedHelmet.gltf");
		}

		protected override void OnResize () {
			pbrRenderer.Camera.AspectRatio = (float)swapChain.Width / (float)swapChain.Height;

			depthTexture?.Dispose ();
			colorTexture?.Dispose ();

			depthTexture = new Image (dev, depthFormat, VkImageUsageFlags.DepthStencilAttachment,
					VkMemoryPropertyFlags.DeviceLocal, swapChain.Width, swapChain.Height, VkImageType.Image2D, samples);
			depthTexture.CreateView (VkImageViewType.Image2D, VkImageAspectFlags.Depth);
			if (samples != VkSampleCountFlags.Count1) {
				colorTexture = new Image (dev, swapChain.ColorFormat, VkImageUsageFlags.ColorAttachment,
						VkMemoryPropertyFlags.DeviceLocal, swapChain.Width, swapChain.Height, VkImageType.Image2D, samples);
				colorTexture.CreateView ();
			}

			for (int i = 0; i < swapChain.ImageCount; ++i)
				frameBuffers[i]?.Dispose ();

			for (int i = 0; i < swapChain.ImageCount; ++i) {

				frameBuffers[i] = new Framebuffer (pbrRenderer.renderPass, swapChain.Width, swapChain.Height,
					(samples == VkSampleCountFlags.Count1) ? new VkImageView[] {
						swapChain.images[i].Descriptor.imageView,
						depthTexture.Descriptor.imageView
					} : new VkImageView[] {
						colorTexture.Descriptor.imageView,
						depthTexture.Descriptor.imageView,
						swapChain.images[i].Descriptor.imageView
					});					

				cmds[i] = cmdPool.AllocateCommandBuffer ();
				cmds[i].Start ();

				pbrRenderer.RecordCmd (cmds[i], frameBuffers[i]);

				cmds[i].End ();
			}
		}

		public override void Update () {
			pbrRenderer.Update ();
			updateRequested = false;
		}

		protected override void onMouseMove (double xPos, double yPos) {
			double diffX = lastMouseX - xPos;
			double diffY = lastMouseY - yPos;
			if (MouseButton[0]) {
				pbrRenderer.Camera.Rotate ((float)-diffX,(float)-diffY);
			} else if (MouseButton[1]) {
				pbrRenderer.Camera.Zoom ((float)diffY);
			}
			lastMouseX = xPos;
			lastMouseY = yPos;

			updateRequested = true;
		}
		protected override void onKeyDown (Key key, int scanCode, Modifier modifiers) {
			switch (key) {
				case Key.Up:
					pbrRenderer.Camera.Rotate (0, 0, 1);
					break;
				case Key.Down:
					pbrRenderer.Camera.Move (0, 0, -1);
					break;
				case Key.Left:
					pbrRenderer.Camera.Move (1, 0, 0);
					break;
				case Key.Right:
					pbrRenderer.Camera.Move (-1, 0, 0);
					break;
				case Key.PageUp:
					pbrRenderer.Camera.Move (0, 1, 0);
					break;
				case Key.PageDown:
					pbrRenderer.Camera.Move (0, -1, 0);
					break;
				case Key.F1:
					if (modifiers.HasFlag (Modifier.Shift))
						pbrRenderer.matrices.exposure -= 0.3f;
					else
						pbrRenderer.matrices.exposure += 0.3f;
					break;
				case Key.F2:
					if (modifiers.HasFlag (Modifier.Shift))
						pbrRenderer.matrices.gamma -= 0.1f;
					else
						pbrRenderer.matrices.gamma += 0.1f;
					break;
				default:
					base.onKeyDown (key, scanCode, modifiers);
					return;
			}
			updateRequested = true;
		}

		protected override void Dispose (bool disposing) {
			if (disposing) {
				if (!isDisposed) {
					for (int i = 0; i < swapChain.ImageCount; i++)
						frameBuffers[i]?.Dispose ();
					colorTexture?.Dispose ();
					depthTexture.Dispose ();
				}
			}

			base.Dispose (disposing);
		}
	}
}
