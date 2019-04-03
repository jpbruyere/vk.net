using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;
using VKE;
using Vulkan;
using Buffer = VKE.Buffer;

namespace TextureSample {
	class Program : VkWindow {
		static void Main (string[] args) {
			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}

		struct Matrices {
			public Matrix4x4 projection;
			public Matrix4x4 view;
		}

		Matrices matrices;

		HostBuffer vbo;
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

		float[] vertices = {
			-1.0f,  1.0f, -1.0f,
		    -1.0f, -1.0f, -1.0f,
		     1.0f, -1.0f, -1.0f,
		     1.0f, -1.0f, -1.0f,
		     1.0f,  1.0f, -1.0f,
		    -1.0f,  1.0f, -1.0f,

		    -1.0f, -1.0f,  1.0f,
		    -1.0f, -1.0f, -1.0f,
		    -1.0f,  1.0f, -1.0f,
		    -1.0f,  1.0f, -1.0f,
		    -1.0f,  1.0f,  1.0f,
		    -1.0f, -1.0f,  1.0f,

		     1.0f, -1.0f, -1.0f,
		     1.0f, -1.0f,  1.0f,
		     1.0f,  1.0f,  1.0f,
		     1.0f,  1.0f,  1.0f,
		     1.0f,  1.0f, -1.0f,
		     1.0f, -1.0f, -1.0f,

		    -1.0f, -1.0f,  1.0f,
		    -1.0f,  1.0f,  1.0f,
		     1.0f,  1.0f,  1.0f,
		     1.0f,  1.0f,  1.0f,
		     1.0f, -1.0f,  1.0f,
		    -1.0f, -1.0f,  1.0f,

		    -1.0f,  1.0f, -1.0f,
		     1.0f,  1.0f, -1.0f,
		     1.0f,  1.0f,  1.0f,
		     1.0f,  1.0f,  1.0f,
		    -1.0f,  1.0f,  1.0f,
		    -1.0f,  1.0f, -1.0f,

		    -1.0f, -1.0f, -1.0f,
		    -1.0f, -1.0f,  1.0f,
		     1.0f, -1.0f, -1.0f,
		     1.0f, -1.0f, -1.0f,
		    -1.0f, -1.0f,  1.0f,
		     1.0f, -1.0f,  1.0f
		};

		float rotSpeed = 0.01f, zoomSpeed = 0.01f;
		double lastX, lastY;
		float rotX, rotY, rotZ = 0f, zoom = 1f;

		Image texture;

		Program () : base () {
			//texture = Image.Load (dev, presentQueue, cmdPool, "data/texture.jpg");

			string[] ktxPathes = {
				"/mnt/devel/vulkan/ktx20/testimages/rgba-reference.ktx",
				"/mnt/devel/vulkan/vulkanExUpstream/data/textures/font_bitmap_rgba.ktx",
				"/mnt/devel/vulkan/vulkanExUpstream/data/textures/ground_dry_bc3_unorm.ktx",
				"/mnt/devel/vulkan/vulkanExUpstream/data/textures/texturearray_rocks_bc3_unorm.ktx",
				"/mnt/devel/vulkan/vulkanExUpstream/data/textures/texturearray_plants_bc3_unorm.ktx",//first slice seems corrupted
				"/mnt/devel/vulkan/vulkanExUpstream/data/textures/skysphere_bc3_unorm.ktx"
			};


			texture = KTX.KTX.Load (presentQueue, cmdPool, "data/papermill.ktx",
				VkImageUsageFlags.Sampled, VkMemoryPropertyFlags.DeviceLocal, true);

			//texture = Image.Load (dev, "data/texture.jpg");
			texture.CreateView ();
			texture.CreateSampler ();

			descriptorPool = new DescriptorPool (dev, 1,
				new VkDescriptorPoolSize (VkDescriptorType.UniformBuffer),
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler)
			);

			matricesBinding = new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Vertex, VkDescriptorType.UniformBuffer);
			textureBinding = new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler);

			dsLayout = new DescriptorSetLayout (dev, matricesBinding, textureBinding);
			descriptorSet = descriptorPool.Allocate (dsLayout);

			pipelineLayout = new PipelineLayout (dev, dsLayout);
			pipelineLayout.Activate ();

			vbo = new HostBuffer<float> (dev, VkBufferUsageFlags.VertexBuffer, vertices);
			uboMats = new HostBuffer (dev, VkBufferUsageFlags.UniformBuffer, matrices);

			depthFormat = dev.GetSuitableDepthFormat ();

			renderPass = new RenderPass (dev, swapChain.ColorFormat, depthFormat);

			frameBuffers = new Framebuffer[swapChain.ImageCount];

			pipeline = new Pipeline (pipelineLayout, renderPass);

			pipeline.vertexBindings.Add (new VkVertexInputBindingDescription (0, (uint)Marshal.SizeOf<float> ()));

			pipeline.vertexAttributes.Add (new VkVertexInputAttributeDescription (0, VkFormat.R32g32b32Sfloat));

			pipeline.shaders.Add (new ShaderInfo (VkShaderStageFlags.Vertex, "shaders/skybox.vert.spv"));
			pipeline.shaders.Add (new ShaderInfo (VkShaderStageFlags.Fragment, "shaders/skybox.frag.spv"));

			pipeline.Activate ();

			texture.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
			using (DescriptorSetWrites uboUpdate = new DescriptorSetWrites (dev)) {
				uboUpdate.AddWriteInfo (descriptorSet, matricesBinding, uboMats.Descriptor);
				uboUpdate.AddWriteInfo (descriptorSet, textureBinding, texture.Descriptor);
				uboUpdate.Update ();
			}

			uboMats.Map ();//permanent map
			updateMatrices ();
		}

		protected override void configureEnabledFeatures (ref VkPhysicalDeviceFeatures features) {
			base.configureEnabledFeatures (ref features);
			features.textureCompressionBC = true;
		}

		public override void Update () {
			updateMatrices ();
			updateRequested = false;
		}

		void updateMatrices () {
			matrices.projection = Matrix4x4.CreatePerspectiveFieldOfView (Utils.DegreesToRadians (60f), (float)swapChain.Width / (float)swapChain.Height, 0.1f, 256.0f);
			matrices.view = Matrix4x4.CreateTranslation (0, 0, 0);

			uboMats.Update (matrices, (uint)Marshal.SizeOf<Matrices> ());
		}

		protected override void onMouseMove (double xPos, double yPos) {
			double diffX = lastX - xPos;
			double diffY = lastY - yPos;
			if (MouseButton[0]) {
				rotY -= rotSpeed * (float)diffX;
				rotX += rotSpeed * (float)diffY;
			} else if (MouseButton[1]) {
				zoom += zoomSpeed * (float)diffY;
			}
			lastX = xPos;
			lastY = yPos;

			updateRequested = true;
		}

		protected override void OnResize () {

			depthTexture?.Dispose ();

			depthTexture = new Image (dev, depthFormat, VkImageUsageFlags.DepthStencilAttachment,
				VkMemoryPropertyFlags.DeviceLocal, swapChain.Width, swapChain.Height);
			depthTexture.CreateView (VkImageViewType.Image2D, VkImageAspectFlags.Depth);

			for (int i = 0; i < swapChain.ImageCount; ++i)
				frameBuffers[i]?.Dispose ();

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

				cmds[i].BindVertexBuffer (vbo);
				cmds[i].Draw ((uint)vertices.Length);

				renderPass.End (cmds[i]);

				cmds[i].End ();
			}
		}
		protected override void Dispose (bool disposing) {
			if (disposing) {
				if (!isDisposed) {
					pipeline.Dispose ();
					pipelineLayout.Dispose ();
					dsLayout.Dispose ();
					for (int i = 0; i < swapChain.ImageCount; i++)
						frameBuffers[i].Dispose ();
					descriptorPool.Dispose ();
					renderPass.Dispose ();
					texture.Dispose ();
					depthTexture.Dispose ();
					vbo.Dispose ();
					uboMats.Dispose ();
				}
			}

			base.Dispose (disposing);
		}
	}
}
