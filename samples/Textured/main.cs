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
            public Matrix4x4 model;
        }

        Matrices matrices;

        HostBuffer ibo;
        HostBuffer vbo;
        HostBuffer uboMats;

        GPUBuffer gpuBuff;

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

        struct Vertex {
            Vector3 position;
            Vector2 uv;

            public Vertex (float x, float y, float z, float u, float v) {
                position = new Vector3 (x, y, z);
                uv = new Vector2 (u, v);
            }
        }

        Vertex[] vertices = new Vertex[] {
            new Vertex ( 1.0f,  1.0f, 0.0f ,  1.0f, 0.0f),
            new Vertex (-1.0f,  1.0f, 0.0f ,  0.0f, 0.0f),
            new Vertex (-1.0f, -1.0f, 0.0f ,  0.0f, 1.0f),
            new Vertex ( 1.0f, -1.0f, 0.0f ,  1.0f, 1.0f),
        };

        ushort[] indices = new ushort[] { 0, 1, 2, 2, 0, 3 };

        float rotX, rotY, rotZ;

        Image texture;

        Program () : base () {
            texture = Image.Load (dev, "data/texture.jpg");
            texture.CreateView ();
            texture.CreateSampler ();

            descriptorPool = new DescriptorPool (dev, 1,
                new VkDescriptorPoolSize (VkDescriptorType.UniformBuffer),
                new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler)
            );

            matricesBinding = new VkDescriptorSetLayoutBinding(0, VkShaderStageFlags.Vertex, VkDescriptorType.UniformBuffer);
            textureBinding = new VkDescriptorSetLayoutBinding  (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler);

            dsLayout = new DescriptorSetLayout (dev, matricesBinding, textureBinding);
            descriptorSet = descriptorPool.Allocate (dsLayout);

            pipelineLayout = new PipelineLayout (dev, dsLayout);
			pipelineLayout.Activate ();

            vbo = new HostBuffer<Vertex> (dev, VkBufferUsageFlags.VertexBuffer, vertices);
            ibo = new HostBuffer<ushort> (dev, VkBufferUsageFlags.IndexBuffer, indices);
            uboMats = new HostBuffer (dev, VkBufferUsageFlags.UniformBuffer, matrices);

            depthFormat = dev.GetSuitableDepthFormat ();

            renderPass = new RenderPass (dev, swapChain.ColorFormat, depthFormat);

            frameBuffers = new Framebuffer[swapChain.ImageCount];

            pipeline = new Pipeline (pipelineLayout, renderPass);

            pipeline.vertexBindings.Add (new VkVertexInputBindingDescription (0, (uint)Marshal.SizeOf<Vertex> ()));

            pipeline.vertexAttributes.Add (new VkVertexInputAttributeDescription (0, VkFormat.R32g32b32Sfloat));
            pipeline.vertexAttributes.Add (new VkVertexInputAttributeDescription (1, VkFormat.R32g32Sfloat, 3 * sizeof (float)));

            pipeline.shaders.Add (new ShaderInfo (VkShaderStageFlags.Vertex, "shaders/main.vert.spv"));
            pipeline.shaders.Add (new ShaderInfo (VkShaderStageFlags.Fragment, "shaders/main.frag.spv"));

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

        public override void Update () {
            updateMatrices ();
            updateRequested = false;
        }

        void updateMatrices () {
            matrices.projection = Matrix4x4.CreatePerspectiveFieldOfView (Utils.DegreesToRadians (60f), (float)swapChain.Width / (float)swapChain.Height, 0.1f, 256.0f);
            matrices.view = Matrix4x4.CreateTranslation (0, 0, -2.5f);
            matrices.model =
                Matrix4x4.CreateFromAxisAngle (Vector3.UnitZ, rotZ) *
                Matrix4x4.CreateFromAxisAngle (Vector3.UnitY, rotY) *
                Matrix4x4.CreateFromAxisAngle (Vector3.UnitX, rotX);

            uboMats.Update (matrices, (uint)Marshal.SizeOf<Matrices> ());
        }

        float rotSpeed = 0.01f;
        double lastX, lastY;

        protected override void onMouseMove (double xPos, double yPos) {
            double diffX = lastX - xPos;
            double diffY = lastY - yPos;
            if (MouseButton[0]) {
                rotY -= rotSpeed * (float)diffX;
                rotX += rotSpeed * (float)diffY;
            }
            lastX = xPos;
            lastY = yPos;

            updateRequested = true;
        }

        protected override void Prepare () {
        
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
                cmds[i].BindIndexBuffer (ibo, VkIndexType.Uint16);
                cmds[i].DrawIndexed ((uint)indices.Length);

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
					ibo.Dispose ();
					uboMats.Dispose ();
				}
			}

			base.Dispose (disposing);
		}
    }
}
