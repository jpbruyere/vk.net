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

		struct Matrices {
			public Matrix4x4 projection;
			public Matrix4x4 view;
			public Matrix4x4 model;
		}

		Matrices matrices;

		HostBuffer uboMats;

		VkDescriptorSetLayoutBinding matricesBinding;
		VkDescriptorSetLayoutBinding textureBinding;
		DescriptorSetLayout dsLayout;
		DescriptorPool descriptorPool;
		DescriptorSet descriptorSet;

		RenderPass renderPass;
		Framebuffer[] frameBuffers;

		PipelineLayout pipelineLayout;
		Pipeline pipeline;

		VkFormat depthFormat;
		Image depthTexture;


		float rotSpeed = 0.01f;
		double lastMouseX, lastMouseY;
		float rotX = -1.5f, rotY = 2.7f, rotZ = 0f;

		Model helmet;

		Program () : base () {
		
			descriptorPool = new DescriptorPool (dev, 2,
				new VkDescriptorPoolSize (VkDescriptorType.UniformBuffer),
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler)
			);

			matricesBinding = new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Vertex, VkDescriptorType.UniformBuffer);
			textureBinding = new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler);

			dsLayout = new DescriptorSetLayout (dev, matricesBinding, textureBinding);
			descriptorSet = descriptorPool.Allocate (dsLayout);

			pipelineLayout = new PipelineLayout (dev, dsLayout);

			loadAssets ();

			uboMats = new HostBuffer (dev, VkBufferUsageFlags.UniformBuffer, matrices);

			depthFormat = dev.GetSuitableDepthFormat ();

			renderPass = new RenderPass (dev, swapChain.ColorFormat, depthFormat);

			frameBuffers = new Framebuffer[swapChain.ImageCount];

			pipeline = new Pipeline (pipelineLayout, renderPass);

			pipeline.vertexBindings.Add (new VkVertexInputBindingDescription (0, (uint)Marshal.SizeOf<Model.Vertex> ()));

			pipeline.vertexAttributes.Add (new VkVertexInputAttributeDescription (0, VkFormat.R32g32b32Sfloat));
			pipeline.vertexAttributes.Add (new VkVertexInputAttributeDescription (1, VkFormat.R32g32b32Sfloat, 3 * sizeof (float)));
			pipeline.vertexAttributes.Add (new VkVertexInputAttributeDescription (2, VkFormat.R32g32Sfloat, 6 * sizeof (float)));

			pipeline.shaders.Add (new ShaderInfo (VkShaderStageFlags.Vertex, "shaders/model.vert.spv"));
			pipeline.shaders.Add (new ShaderInfo (VkShaderStageFlags.Fragment, "shaders/model.frag.spv"));

			pipeline.Activate ();

			helmet.textures[0].Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
			using (DescriptorSetWrites uboUpdate = new DescriptorSetWrites (dev)) {
				uboUpdate.AddWriteInfo (descriptorSet, matricesBinding, uboMats.Descriptor);
				uboUpdate.AddWriteInfo (descriptorSet, textureBinding, helmet.textures[0].Descriptor);
				uboUpdate.Update ();
			}

			uboMats.Map ();//permanent map
			updateMatrices ();
		}

		void loadAssets () {
			helmet = new Model (dev, presentQueue, cmdPool, "data/DamagedHelmet.gltf");
		}
		public override void Update () {
			updateMatrices ();
			updateRequested = false;
		}

		void updateMatrices () {
			matrices.projection = Matrix4x4.CreatePerspectiveFieldOfView (Utils.DegreesToRadians (60f), (float)swapChain.Width / (float)swapChain.Height, 0.1f, 256.0f);
			matrices.view = Matrix4x4.CreateTranslation (0, 0, -2.5f);
			matrices.model =
					Matrix4x4.CreateFromAxisAngle (Vector3.UnitX, rotX) *
					Matrix4x4.CreateFromAxisAngle (Vector3.UnitY, rotY) *
					Matrix4x4.CreateFromAxisAngle (Vector3.UnitZ, rotZ);

			uboMats.Update (matrices, (uint)Marshal.SizeOf<Matrices> ());
		}

		protected override void onMouseMove (double xPos, double yPos) {
			double diffX = lastMouseX - xPos;
			double diffY = lastMouseY - yPos;
			if (MouseButton[0]) {
				rotY -= rotSpeed * (float)diffX;
				rotX -= rotSpeed * (float)diffY;
			}
			lastMouseX = xPos;
			lastMouseY = yPos;

			updateRequested = true;
		}

		protected override void Prepare () {

			if (depthTexture != null)
				depthTexture.Dispose ();

			depthTexture = new Image (dev, depthFormat, VkImageUsageFlags.DepthStencilAttachment,
					VkMemoryPropertyFlags.DeviceLocal, swapChain.Width, swapChain.Height);
			depthTexture.CreateView (VkImageViewType.Image2D, VkImageAspectFlags.Depth);

			for (int i = 0; i < swapChain.ImageCount; ++i) {
				if (frameBuffers[i] != null)
					frameBuffers[i].Destroy ();
			}

			for (int i = 0; i < swapChain.ImageCount; ++i) {
				frameBuffers[i] = new Framebuffer (renderPass, swapChain.Width, swapChain.Height,
						new VkImageView[] { swapChain.images[i].Descriptor.imageView, depthTexture.Descriptor.imageView });

				cmds[i] = cmdPool.AllocateCommandBuffer ();
				cmds[i].Start ();

				renderPass.Begin (cmds[i], frameBuffers[i]);

				cmds[i].SetViewport (swapChain.Width, swapChain.Height);
				cmds[i].SetScissor (swapChain.Width, swapChain.Height);

				cmds[i].BindDescriptorSet (pipelineLayout, descriptorSet);

				cmds[i].BindPipeline (pipeline);

				helmet.DrawAll (cmds[i]);

				renderPass.End (cmds[i]);

				cmds[i].End ();
			}
		}

		protected override void Dispose (bool disposing) {
			pipeline.Destroy ();
			pipelineLayout.Destroy ();
			dsLayout.Destroy ();
			for (int i = 0; i < swapChain.ImageCount; i++)
				frameBuffers[i].Destroy ();
			descriptorPool.Destroy ();
			renderPass.Destroy ();

			if (disposing) {
				helmet.Dispose ();
				depthTexture.Dispose ();
				uboMats.Dispose ();
			}

			base.Dispose (disposing);
		}
	}
}
