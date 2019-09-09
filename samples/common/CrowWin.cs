using System;
using Glfw;
using VK;
using System.Threading;

namespace Crow {
	public class CrowWin : CVKL.VkWindow, IValueChange {
		#region IValueChange implementation
		public event EventHandler<ValueChangeEventArgs> ValueChanged;
		public virtual void NotifyValueChanged (string MemberName, object _value) {
			if (ValueChanged != null)
				ValueChanged.Invoke (this, new ValueChangeEventArgs (MemberName, _value));
		}
		#endregion

		CVKL.DescriptorPool descriptorPool;
		CVKL.DescriptorSetLayout descLayout;
		CVKL.DescriptorSet dsCrow;

		CVKL.GraphicPipeline uiPipeline;
		CVKL.Framebuffer[] uiFrameBuffers;

		protected Interface crow;
		protected vkvg.Device vkvgDev;
		protected CVKL.Image uiImage;
		protected bool isRunning, rebuildBuffers;

		protected CrowWin (string name = "CrowWin", uint _width = 1024, uint _height = 768, bool vSync = false) :
			base (name, _width, _height, vSync) {

			Thread crowThread = new Thread (crow_thread_func);
			crowThread.IsBackground = true;
			crowThread.Start ();

			while (crow == null)
				Thread.Sleep (2);

			initUISurface ();

			initUIPipeline ();
		}

		protected override void render () {
			int idx = swapChain.GetNextImage ();

			if (idx < 0) {
				OnResize ();
				return;
			}

			lock (crow.RenderMutex) {
				presentQueue.Submit (cmds[idx], swapChain.presentComplete, drawComplete[idx]);
				presentQueue.Present (swapChain, drawComplete[idx]);
				presentQueue.WaitIdle ();
			}
			Thread.Sleep (1);
		}

		void initUIPipeline (VkSampleCountFlags samples = VkSampleCountFlags.SampleCount1) {
			descriptorPool = new CVKL.DescriptorPool (dev, 1, new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler));
			descLayout = new CVKL.DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler)
			);

			CVKL.GraphicPipelineConfig cfg = CVKL.GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, samples, false);
			cfg.Layout = new CVKL.PipelineLayout (dev, descLayout);
			cfg.RenderPass = new CVKL.RenderPass (dev, swapChain.ColorFormat, samples);
			cfg.AddShader (VkShaderStageFlags.Vertex, "#deferred.FullScreenQuad.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "#deferred.simpletexture.frag.spv");

			cfg.blendAttachments[0] = new VkPipelineColorBlendAttachmentState (true);

			uiPipeline = new CVKL.GraphicPipeline (cfg);

			dsCrow = descriptorPool.Allocate (descLayout);
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
			vkvgDev = new vkvg.Device (instance.Handle, phy.Handle, dev.Handle, presentQueue.qFamIndex,
	   			vkvg.SampleCount.Sample_4, presentQueue.index);

			crow = new Interface (vkvgDev, (int)swapChain.Width, (int)swapChain.Height);

			isRunning = true;	
			while (isRunning) {
				crow.Update ();
				Thread.Sleep (2);
			}

			dev.WaitIdle ();
			crow.Dispose ();
			vkvgDev.Dispose ();
			crow = null;
		}

		protected void loadWindow (string path, object dataSource = null) {
			try {
				Widget w = crow.FindByName (path);
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
		protected virtual void recordDraw (CVKL.CommandBuffer cmd, int imageIndex) { }

		void buildCommandBuffers () {
			for (int i = 0; i < swapChain.ImageCount; ++i) {
				cmds[i]?.Free ();
				cmds[i] = cmdPool.AllocateAndStart ();

				CVKL.CommandBuffer cmd = cmds[i];

				recordDraw (cmd, i);

				uiPipeline.RenderPass.Begin (cmd, uiFrameBuffers[i]);

				uiPipeline.Bind (cmd);
				cmd.BindDescriptorSet (uiPipeline.Layout, dsCrow);

				uiImage.SetLayout (cmd, VkImageAspectFlags.Color, VkImageLayout.ColorAttachmentOptimal, VkImageLayout.ShaderReadOnlyOptimal,
					VkPipelineStageFlags.ColorAttachmentOutput, VkPipelineStageFlags.FragmentShader);

				cmd.Draw (3, 1, 0, 0);

				uiImage.SetLayout (cmd, VkImageAspectFlags.Color, VkImageLayout.ShaderReadOnlyOptimal, VkImageLayout.ColorAttachmentOptimal,
					VkPipelineStageFlags.FragmentShader, VkPipelineStageFlags.BottomOfPipe);
					
				uiPipeline.RenderPass.End (cmd);

				cmds[i].End ();
			}
		}

		/// <summary>
		/// rebuild command buffers if needed
		/// </summary>
		public override void Update () {
			if (rebuildBuffers) {
				buildCommandBuffers ();
				rebuildBuffers = false;
			}
		}

		protected override void OnResize () {
			dev.WaitIdle ();

			crow.ProcessResize (new Rectangle (0, 0, (int)swapChain.Width, (int)swapChain.Height));

			initUISurface ();

			CVKL.DescriptorSetWrites uboUpdate = new CVKL.DescriptorSetWrites (dsCrow, descLayout);
			uboUpdate.Write (dev, uiImage.Descriptor);

			if (uiFrameBuffers != null)
				for (int i = 0; i < swapChain.ImageCount; ++i)
					uiFrameBuffers[i]?.Dispose ();

			uiFrameBuffers = new CVKL.Framebuffer[swapChain.ImageCount];

			for (int i = 0; i < swapChain.ImageCount; ++i) {
				uiFrameBuffers[i] = new CVKL.Framebuffer (uiPipeline.RenderPass, swapChain.Width, swapChain.Height,
					(uiPipeline.Samples == VkSampleCountFlags.SampleCount1) ? new CVKL.Image[] {
						swapChain.images[i],
					} : new CVKL.Image[] {
						null,
						swapChain.images[i]
					});
				uiFrameBuffers[i].SetName ("ui FB " + i);
			}

			buildCommandBuffers ();
			dev.WaitIdle ();
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
			base.onMouseMove (xPos, yPos);
		}
		protected override void onMouseButtonDown (Glfw.MouseButton button) {
			if (crow.ProcessMouseButtonDown ((MouseButton)button))
				return;
			base.onMouseButtonDown (button);
		}
		protected override void onMouseButtonUp (Glfw.MouseButton button) {
			if (crow.ProcessMouseButtonUp ((MouseButton)button))
				return;
			base.onMouseButtonUp (button);
		}
		protected override void onKeyDown (Glfw.Key key, int scanCode, Modifier modifiers) {
			if (crow.ProcessKeyDown ((Key)key))
				return;
			base.onKeyDown (key, scanCode, modifiers);
		}
		protected override void onKeyUp (Glfw.Key key, int scanCode, Modifier modifiers) {
			if (crow.ProcessKeyUp ((Key)key))
				return;
		}
		protected override void onChar (CodePoint cp) {
			if (crow.ProcessKeyPress (cp.ToChar ()))
				return;
		}
		#endregion

		#region dispose
		protected override void Dispose (bool disposing) {
			if (disposing) {
				if (!isDisposed) {
					dev.WaitIdle ();
					isRunning = false;

					for (int i = 0; i < swapChain.ImageCount; ++i)
						uiFrameBuffers[i]?.Dispose ();

					uiPipeline.Dispose ();
					descLayout.Dispose ();
					descriptorPool.Dispose ();

					uiImage?.Dispose ();
					while (crow != null)
						Thread.Sleep (1);
				}
			}

			base.Dispose (disposing);
		}
		#endregion
	}
}
