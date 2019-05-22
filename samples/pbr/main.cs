/* Forward pbr sample inspire from https://github.com/SaschaWillems/Vulkan-glTF-PBR
 *
 * Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
 *
 * This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
 */

using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;
using VK;
using CVKL;

namespace pbrSample {
	class Program : VkWindow{	
		static void Main (string[] args) {
			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}
		protected override void configureEnabledFeatures (VkPhysicalDeviceFeatures available_features, ref VkPhysicalDeviceFeatures features) {
			base.configureEnabledFeatures (available_features, ref features);
#if PIPELINE_STATS
			features.pipelineStatisticsQuery = true;
#endif
			features.samplerAnisotropy = true;
		}

		VkSampleCountFlags samples = VkSampleCountFlags.SampleCount4;

		Framebuffer[] frameBuffers;
		PBRPipeline pbrPipeline;

		enum DebugView {
			none,
			color,
			normal,
			occlusion,
			emissive,
			metallic,
			roughness
		}

		DebugView currentDebugView = DebugView.none;

#if PIPELINE_STATS
		PipelineStatisticsQueryPool statPool;
		TimestampQueryPool timestampQPool;
		ulong[] results;
#endif


		bool queryUpdatePrefilCube, showDebugImg, showUI;


		Image uiImage;

#region ui
		//DescriptorSet dsDebugImg;
		//void initDebugImg () {
		//	dsDebugImg = descriptorPool.Allocate (descLayoutMain);
		//	pbrPipeline.envCube.debugImg.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
		//	DescriptorSetWrites uboUpdate = new DescriptorSetWrites (dsDebugImg, descLayoutMain);
		//	uboUpdate.Write (dev, pbrPipeline.envCube.debugImg.Descriptor);
		//}

		vkvg.Device vkvgDev;
        vkvg.Surface vkvgSurf;
        
		Pipeline uiPipeline;

		void initUIPipeline () {

			GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, samples, false);
			cfg.RenderPass = pbrPipeline.RenderPass;
			cfg.Layout = pbrPipeline.Layout;
			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/FullScreenQuad.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/simpletexture.frag.spv");
			cfg.blendAttachments[0] = new VkPipelineColorBlendAttachmentState (true);

			uiPipeline = new GraphicPipeline (cfg);

		}
		void initUISurface () {
			uiImage?.Dispose ();
			vkvgSurf?.Dispose ();
			vkvgSurf = new vkvg.Surface (vkvgDev, (int)swapChain.Width, (int)swapChain.Height);
			uiImage = new Image (dev, new VkImage ((ulong)vkvgSurf.VkImage.ToInt64 ()), VkFormat.B8g8r8a8Unorm,
				VkImageUsageFlags.ColorAttachment, (uint)vkvgSurf.Width, (uint)vkvgSurf.Height);
			uiImage.CreateView (VkImageViewType.ImageView2D, VkImageAspectFlags.Color);
			uiImage.CreateSampler (VkFilter.Nearest, VkFilter.Nearest, VkSamplerMipmapMode.Nearest, VkSamplerAddressMode.ClampToBorder);
			uiImage.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
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
				ctx.ShowText (string.Format ($"Exposure:{pbrPipeline.matrices.exposure,5} "));
				y += dy;
				ctx.MoveTo (x, y);
				ctx.ShowText (string.Format ($"Gamma:   {pbrPipeline.matrices.gamma,5} "));
				y += dy;
				ctx.MoveTo (x, y);
				ctx.ShowText (string.Format ($"Light pos:   {lightPos.ToString()} "));

#if PIPELINE_STATS
				if (results == null)
					return;

				y += dy*2;
				ctx.MoveTo (x, y);
				ctx.ShowText ("Pipeline Statistics");
				ctx.MoveTo (x-2, 4.5+y);
				ctx.LineTo (x+160, 4.5+y);
				ctx.Stroke ();
				y += 4;
				x += 20;

				for (int i = 0; i < statPool.RequestedStats.Length; i++) {
					y += dy;
					ctx.MoveTo (x, y);
					ctx.ShowText (string.Format ($"{statPool.RequestedStats[i].ToString(),-30} :{results[i],12:0,0} "));
				}
				/*y += dy;
				ctx.MoveTo (x, y);
				ctx.ShowText (string.Format ($"{"Elapsed microsecond",-20} :{timestampQPool.ElapsedMiliseconds:0.0000} "));*/
#endif
				y += dy;
				ctx.MoveTo (x, y);
				ctx.ShowText (string.Format ($"{"Debug draw (numpad 0->6)",-30} : {currentDebugView.ToString ()} "));
				y += dy;
				ctx.MoveTo (x, y);
				ctx.ShowText (string.Format ($"{"Debug Prefil Face: (f)",-30} : {pbrPipeline.envCube.debugFace.ToString ()} "));
				y += dy;
				ctx.MoveTo (x, y);
				ctx.ShowText (string.Format ($"{"Debug Prefil Mip: (m)",-30} : {pbrPipeline.envCube.debugMip.ToString ()} "));
			}
		}
		void recordDrawOverlay (CommandBuffer cmd) {
			uiPipeline.Bind (cmd);

			uiImage.SetLayout (cmd, VkImageAspectFlags.Color, VkImageLayout.ColorAttachmentOptimal, VkImageLayout.ShaderReadOnlyOptimal,
				VkPipelineStageFlags.ColorAttachmentOutput, VkPipelineStageFlags.FragmentShader);

			cmd.Draw (3, 1, 0, 0);

			uiImage.SetLayout (cmd, VkImageAspectFlags.Color, VkImageLayout.ShaderReadOnlyOptimal, VkImageLayout.ColorAttachmentOptimal,
				VkPipelineStageFlags.FragmentShader, VkPipelineStageFlags.BottomOfPipe);

			//if (!showDebugImg)
			//	return;
			//const uint debugImgSize = 256;
			//const uint debugImgMargin = 10;

			//cmd.BindDescriptorSet (uiPipeline.Layout, dsDebugImg);

			//cmd.SetViewport (debugImgSize, debugImgSize, debugImgMargin, swapChain.Height - debugImgSize - debugImgMargin);
			//cmd.SetScissor (debugImgSize, debugImgSize, (int)debugImgMargin, (int)(swapChain.Height - debugImgSize - debugImgMargin));

			//cmd.Draw (3, 1, 0, 0);
		}
#endregion


		Vector4 lightPos = new Vector4 (1, 0, 0, 0);
		BoundingBox modelAABB;

		Program () {

			//UpdateFrequency = 20;

			camera.SetPosition (0, 0, 5);

			vkvgDev = new vkvg.Device (instance.Handle, phy.Handle, dev.VkDev.Handle, presentQueue.qFamIndex,
				vkvg.SampleCount.Sample_4, presentQueue.index);

			initUISurface ();

			pbrPipeline = new PBRPipeline(presentQueue,
				new RenderPass (dev, swapChain.ColorFormat, dev.GetSuitableDepthFormat (), samples), uiImage);

			initUIPipeline ();

			modelAABB = pbrPipeline.model.DefaultScene.AABB;

			//camera.Model = Matrix4x4.CreateScale (1f/ Math.Max (Math.Max (modelAABB.max.X, modelAABB.max.Y), modelAABB.max.Z));

#if PIPELINE_STATS
			statPool = new PipelineStatisticsQueryPool (dev,
				VkQueryPipelineStatisticFlags.InputAssemblyVertices |
				VkQueryPipelineStatisticFlags.InputAssemblyPrimitives |
				VkQueryPipelineStatisticFlags.ClippingInvocations |
				VkQueryPipelineStatisticFlags.ClippingPrimitives |
				VkQueryPipelineStatisticFlags.FragmentShaderInvocations);

			timestampQPool = new TimestampQueryPool (dev);
#endif
		}

		void buildCommandBuffers () {
			for (int i = 0; i < swapChain.ImageCount; ++i) {
				cmds[i]?.Free ();
				cmds[i] = cmdPool.AllocateAndStart ();
#if PIPELINE_STATS
				statPool.Begin (cmds[i]);
				recordDraw (cmds[i], frameBuffers[i]);
				statPool.End (cmds[i]);
#else
				recordDraw (cmds[i], frameBuffers[i]);
#endif

				cmds[i].End ();
			}
		}
		void recordDraw (CommandBuffer cmd, Framebuffer fb) {
			pbrPipeline.RenderPass.Begin (cmd, fb);

			cmd.SetViewport (fb.Width, fb.Height);
			cmd.SetScissor (fb.Width, fb.Height);

			pbrPipeline.RecordDraw (cmd);

			if (showUI)
				recordDrawOverlay (cmd);

			pbrPipeline.RenderPass.End (cmd);
		}


#region update
		public override void UpdateView () {
			camera.AspectRatio = (float)swapChain.Width / swapChain.Height;

			pbrPipeline.matrices.lightDir = lightPos;
			pbrPipeline.matrices.projection = camera.Projection;
			pbrPipeline.matrices.view = camera.View;
			pbrPipeline.matrices.model = camera.Model;


			pbrPipeline.matrices.camPos = new Vector4 (
				-camera.Position.Z * (float)Math.Sin (camera.Rotation.Y) * (float)Math.Cos (camera.Rotation.X),
				 camera.Position.Z * (float)Math.Sin (camera.Rotation.X),
				 camera.Position.Z * (float)Math.Cos (camera.Rotation.Y) * (float)Math.Cos (camera.Rotation.X),
				 0
			);
			pbrPipeline.matrices.debugViewInputs = (float)currentDebugView;

			pbrPipeline.uboMats.Update (pbrPipeline.matrices, (uint)Marshal.SizeOf<PBRPipeline.Matrices> ());

			updateViewRequested = false;
		}

		public override void Update () {
#if PIPELINE_STATS
			results = statPool.GetResults ();
#endif
			if (rebuildBuffers) {
				buildCommandBuffers ();
				rebuildBuffers = false;
			}
			if (showUI)
				vkvgDraw ();
		}
#endregion

		 
		protected override void OnResize () {
			initUISurface ();

			uiImage.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (pbrPipeline.dsMain, pbrPipeline.Layout.DescriptorSetLayouts[0].Bindings[5]);
			uboUpdate.Write (dev, uiImage.Descriptor);

			UpdateView ();

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
						pbrPipeline.envCube.debugFace --;
						if (pbrPipeline.envCube.debugFace < 0)
							pbrPipeline.envCube.debugFace = 5;
					} else {
						pbrPipeline.envCube.debugFace ++;
						if (pbrPipeline.envCube.debugFace > 5)
							pbrPipeline.envCube.debugFace = 0;
					}
					queryUpdatePrefilCube = updateViewRequested = true;
					break;
				case Key.M:
					if (modifiers.HasFlag (Modifier.Shift)) {
						pbrPipeline.envCube.debugMip --;
						if (pbrPipeline.envCube.debugMip < 0)
							pbrPipeline.envCube.debugMip = (int)pbrPipeline.envCube.prefilterCube.CreateInfo.mipLevels - 1;
					} else {
						pbrPipeline.envCube.debugMip ++;
						if (pbrPipeline.envCube.debugMip > pbrPipeline.envCube.prefilterCube.CreateInfo.mipLevels)
							pbrPipeline.envCube.debugMip = 0;
					}
					queryUpdatePrefilCube = updateViewRequested = true;
					break;
				case Key.P:
					showDebugImg = !showDebugImg;
					queryUpdatePrefilCube = updateViewRequested = true;
					break;
				case Key.Keypad0:
					currentDebugView = DebugView.none;
					break;
				case Key.Keypad1:
					currentDebugView = DebugView.color;
					break;
				case Key.Keypad2:
					currentDebugView = DebugView.normal;
					break;
				case Key.Keypad3:
					currentDebugView = DebugView.occlusion;
					break;
				case Key.Keypad4:
					currentDebugView = DebugView.emissive;
					break;
				case Key.Keypad5:
					currentDebugView = DebugView.metallic;
					break;
				case Key.Keypad6:
					currentDebugView = DebugView.roughness;
					break;
				case Key.Up:
					if (modifiers.HasFlag (Modifier.Shift))
						lightPos -= Vector4.UnitZ;
					else
						camera.Move (0, 0, 1);
					break;
				case Key.Down:
					if (modifiers.HasFlag (Modifier.Shift))
						lightPos += Vector4.UnitZ;
					else
						camera.Move (0, 0, -1);
					break;
				case Key.Left:
					if (modifiers.HasFlag (Modifier.Shift))
						lightPos -= Vector4.UnitX;
					else
						camera.Move (1, 0, 0);
					break;
				case Key.Right:
					if (modifiers.HasFlag (Modifier.Shift))
						lightPos += Vector4.UnitX;
					else
						camera.Move (-1, 0, 0);
					break;
				case Key.PageUp:
					if (modifiers.HasFlag (Modifier.Shift))
						lightPos += Vector4.UnitY;
					else
						camera.Move (0, 1, 0);
					break;
				case Key.PageDown:
					if (modifiers.HasFlag (Modifier.Shift))
						lightPos -= Vector4.UnitY;
					else
						camera.Move (0, -1, 0);
					break;
				case Key.F1:
					showUI = !showUI;
					rebuildBuffers = true;
					break;
				case Key.F2:
					if (modifiers.HasFlag (Modifier.Shift))
						pbrPipeline.matrices.exposure -= 0.3f;
					else
						pbrPipeline.matrices.exposure += 0.3f;
					break;
				case Key.F3:
					if (modifiers.HasFlag (Modifier.Shift))
						pbrPipeline.matrices.gamma -= 0.1f;
					else
						pbrPipeline.matrices.gamma += 0.1f;
					break;
				case Key.F4:
					if (camera.Type == Camera.CamType.FirstPerson)
						camera.Type = Camera.CamType.LookAt;
					else
						camera.Type = Camera.CamType.FirstPerson;
					Console.WriteLine ($"camera type = {camera.Type}");
					break;
				default:
					base.onKeyDown (key, scanCode, modifiers);
					return;
			}
			updateViewRequested = true;
		}
#endregion

#region dispose
		protected override void Dispose (bool disposing) {
			if (disposing) {
				if (!isDisposed) {
					dev.WaitIdle ();
					for (int i = 0; i < swapChain.ImageCount; ++i)
						frameBuffers[i]?.Dispose ();

					pbrPipeline.Dispose ();

					uiPipeline.Dispose ();

					uiImage?.Dispose ();
					vkvgSurf?.Dispose ();
					vkvgDev.Dispose ();

#if PIPELINE_STATS
					timestampQPool?.Dispose ();
					statPool?.Dispose ();
#endif
				}
			}

			base.Dispose (disposing);
		}
#endregion
	}
}
