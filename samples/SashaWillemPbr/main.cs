using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;
using VK;
using VKE;

namespace PbrSample {
	class Program : VkWindow, Crow.IValueChange {	
		static void Main (string[] args) {
			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}

		VkSampleCountFlags samples = VkSampleCountFlags.SampleCount4;

		Camera Camera = new Camera (Utils.DegreesToRadians (60f), 1f);

		Framebuffer[] frameBuffers;

		PBRPipeline pbrPipeline;

		#region crow

		#region IValueChange implementation
		public event EventHandler<Crow.ValueChangeEventArgs> ValueChanged;
		public virtual void NotifyValueChanged(string MemberName, object _value)
		{
			if (ValueChanged != null)
				ValueChanged.Invoke(this, new Crow.ValueChangeEventArgs(MemberName, _value));
		}
		#endregion

		public float Gamma {
			get { return pbrPipeline.matrices.gamma; }
			set {
				if (value == pbrPipeline.matrices.gamma)
					return;
				pbrPipeline.matrices.gamma = value;
				NotifyValueChanged ("Gamma", value);
				updateViewRequested = true;
			}
		}
		public float Exposure {
			get { return pbrPipeline.matrices.exposure; }
			set {
				if (value == pbrPipeline.matrices.exposure)
					return;
				pbrPipeline.matrices.exposure = value;
				NotifyValueChanged ("Exposure", value);
				updateViewRequested = true;
			}
		}
		Crow.Interface crow;
		Pipeline uiPipeline;
		Image uiImage;
		vkvg.Device vkvgDev;

		void initCrow () { 
			vkvgDev = new vkvg.Device (instance.Handle, phy.Handle, dev.VkDev.Handle, presentQueue.qFamIndex,
				vkvg.SampleCount.Sample_1, presentQueue.index);

			crow = new Crow.Interface(vkvgDev, 800,600);
		}
		void createUIImage () { 
			uiImage?.Dispose ();
			uiImage = new Image (dev, new VkImage ((ulong)crow.surf.VkImage.ToInt64 ()), VkFormat.B8g8r8a8Unorm,
				VkImageUsageFlags.Sampled, swapChain.Width, swapChain.Height);
			uiImage.SetName ("uiImage");
			uiImage.CreateView ();
			uiImage.CreateSampler ();
			uiImage.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
		}
		#endregion

		Program () {
			//UpdateFrequency = 20;
			Camera.Model = Matrix4x4.CreateRotationX (Utils.DegreesToRadians (-90));

			initCrow ();

			init ();

			crow.Load ("ui/fps.crow").DataSource = this;
		}

		DescriptorPool descriptorPool;
		DescriptorSetLayout descLayoutMain;
		DescriptorSet dsMain;

		void init() {

			pbrPipeline = new PBRPipeline(presentQueue,
				new RenderPass (dev, swapChain.ColorFormat, dev.GetSuitableDepthFormat (), samples));

			descriptorPool = new DescriptorPool (dev, 1, new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler));
			descLayoutMain = new DescriptorSetLayout (dev,			
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler));
			dsMain = descriptorPool.Allocate (descLayoutMain);

			PipelineConfig cfg = PipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, samples);
			cfg.RenderPass = pbrPipeline.RenderPass;
			cfg.Layout = new PipelineLayout (dev, descLayoutMain);

			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/FullScreenQuad.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/simpletexture.frag.spv");
			cfg.blendAttachments[0] = new VkPipelineColorBlendAttachmentState (true);

			uiPipeline = new Pipeline (cfg);

			createUIImage ();
		}



		#region command buffers
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

			uiPipeline.Bind (cmd);
			cmd.BindDescriptorSet (uiPipeline.Layout, dsMain);
			cmd.Draw (3, 1, 0, 0);

			pbrPipeline.RenderPass.End (cmd);
		}

		#endregion

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
			NotifyValueChanged ("fps", fps);
			crow.Update ();
		}
		#endregion

		protected override void OnResize () {
			crow.ProcessResize (new Crow.Rectangle (0,0,(int)swapChain.Width, (int)swapChain.Height));

			createUIImage ();

			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (dsMain, descLayoutMain.Bindings[0]);
			uboUpdate.Write (dev, uiImage.Descriptor);

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
			if (crow.ProcessMouseMove ((int)xPos, (int)yPos))
				return;

			double diffX = lastMouseX - xPos;
			double diffY = lastMouseY - yPos;
			if (MouseButton[0]) {
				Camera.Rotate ((float)-diffX, (float)-diffY);
			} else if (MouseButton[1]) {
				Camera.Zoom ((float)diffY);
			}

			updateViewRequested = true;
		}
		protected override void onMouseButtonDown (MouseButton button) {
			if (crow.ProcessMouseButtonDown ((Crow.MouseButton)button))
				return;
			base.onMouseButtonDown (button);
		}
		protected override void onMouseButtonUp (MouseButton button) {
			if (crow.ProcessMouseButtonUp ((Crow.MouseButton)button))
				return;
			base.onMouseButtonUp (button);
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

					uiPipeline.Dispose ();
					descLayoutMain.Dispose ();
					descriptorPool.Dispose ();

					uiImage.Dispose ();

					crow.Dispose ();

					vkvgDev.Dispose ();


				}
			}

			base.Dispose (disposing);
		}

		
	}
}
