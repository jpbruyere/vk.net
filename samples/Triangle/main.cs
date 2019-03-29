using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;
using VKE;
using Vulkan;
using Buffer = VKE.Buffer;

namespace Triangle {
    class Program : VkWindow {
    
        struct Matrices{
            public Matrix4x4 projection;
            public Matrix4x4 view;
            public Matrix4x4 model;
        }

        Matrices matrices;

        Buffer ibo;
        Buffer vbo;
        Buffer uboMats;

        VkDescriptorSetLayoutBinding dsBinding;
        DescriptorSetLayout dsLayout;
        DescriptorPool descriptorPool;
        DescriptorSet descriptorSet;

        RenderPass renderPass;
        Framebuffer[] frameBuffers;

        PipelineLayout pipelineLayout;
        Pipeline pipeline;

        Image depthTexture;

        struct Vertex {
            Vector3 position;
            Vector3 color;

            public Vertex (float x, float y, float z, float r, float g, float b) {
                position = new Vector3 (x, y, z);
                color = new Vector3 (r, g, b);
            }
        }

        Vertex[] vertices = new Vertex[] {
            new Vertex ( 1.0f,  1.0f, 0.0f ,  1.0f, 0.0f, 0.0f),
            new Vertex (-1.0f,  1.0f, 0.0f ,  0.0f, 1.0f, 0.0f),
            new Vertex ( 0.0f, -1.0f, 0.0f ,  0.0f, 0.0f, 1.0f),
        };

        uint[] indices = new uint[] { 0, 1, 2 };

        float rotX, rotY, rotZ;

        Program () : base () {
            descriptorPool = new DescriptorPool (dev, 1, new VkDescriptorPoolSize {
                type = VkDescriptorType.UniformBuffer, descriptorCount = 1
            });
            dsBinding = new VkDescriptorSetLayoutBinding {
                binding = 0, descriptorType = VkDescriptorType.UniformBuffer, stageFlags = VkShaderStageFlags.Vertex, descriptorCount = 1
            };

            dsLayout = new DescriptorSetLayout (dev, dsBinding);
            descriptorSet = descriptorPool.Allocate (dsLayout);

            pipelineLayout = new PipelineLayout (dev, dsLayout);

            vbo = new Buffer (dev, VkBufferUsageFlags.VertexBuffer,
                VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent, (ulong)(Marshal.SizeOf<Vertex>() * vertices.Length), vertices);
            ibo = new Buffer (dev, VkBufferUsageFlags.IndexBuffer,
                VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent, (ulong)indices.Length * sizeof (uint), indices);
            uboMats = new Buffer (dev, VkBufferUsageFlags.UniformBuffer,
                VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent, (ulong)Marshal.SizeOf<Matrices>(), matrices);
                
            depthTexture = new Image (dev, dev.GetSuitableDepthFormat (), VkImageUsageFlags.DepthStencilAttachment,
                VkMemoryPropertyFlags.DeviceLocal, swapChain.Width, swapChain.Height);
            depthTexture.CreateView (VkImageViewType.Image2D, VkImageAspectFlags.Depth);

            renderPass = new RenderPass (dev, swapChain.ColorFormat, depthTexture.CreateInfo.format);

            frameBuffers = new Framebuffer[swapChain.ImageCount];
            for (int i = 0; i < swapChain.ImageCount; i++) 
                frameBuffers[i] = new Framebuffer (renderPass, swapChain.Width, swapChain.Height,
                    new VkImageView[] {swapChain.images[i].Descriptor.imageView, depthTexture.Descriptor.imageView });

            pipeline = new Pipeline (pipelineLayout, renderPass);
            pipeline.Activate ();

            DescriptorSetWrites uboUpdate = new DescriptorSetWrites (dev);
            uboUpdate.AddWriteInfo (descriptorSet, dsBinding, uboMats.Descriptor);
            uboUpdate.Update ();

            updateMatrices ();
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

        protected override void HandleCursorPosDelegate (IntPtr window, double xPosition, double yPosition) {
            double diffX = lastX - xPosition;
            double diffY = lastY - yPosition;
            if (buttons[0]) {
                rotY -= rotSpeed * (float)diffX;
                rotX += rotSpeed * (float)diffY;
            }
            lastX = xPosition;
            lastY = yPosition;
            updateMatrices ();
        }
        float rotSpeed = 0.01f;
        double lastX, lastY;
        bool[] buttons = new bool[10];
        protected override void HandleMouseButtonDelegate (IntPtr window, MouseButton button, InputAction action, Modifier mods) {

            if (action == InputAction.Press)
                buttons[(int)button] = true;
            else
                buttons[(int)button] = false;
        }

        static void Main (string[] args) {
            Program vke = new Program ();
            vke.Run ();
            vke.Destroy ();
        }

        protected override void Prepare () {


            for (int i = 0; i < swapChain.ImageCount; ++i) {

                cmds[i] = cmdPool.AllocateCommandBuffer ();
                cmds[i].Start ();

                renderPass.Begin (cmds[i], frameBuffers[i]);

                cmds[i].SetViewport (swapChain.Width, swapChain.Height);
                cmds[i].SetScissor (swapChain.Width, swapChain.Height);

                cmds[i].BindDescriptorSet (pipelineLayout, descriptorSet);
                cmds[i].BindPipeline (pipeline);

                cmds[i].BindVertexBuffer (vbo);
                cmds[i].BindIndexBuffer (ibo);
                cmds[i].DrawIndexed ((uint)indices.Length);

                renderPass.End (cmds[i]);

                cmds[i].End ();
            }
        }

        protected override void Destroy () {
            pipeline.Destroy ();
            pipelineLayout.Destroy ();
            dsLayout.Destroy ();
            for (int i = 0; i < swapChain.ImageCount; i++)
                frameBuffers[i].Destroy ();
            descriptorPool.Destroy ();
            renderPass.Destroy ();
            depthTexture.Destroy ();
            vbo.Destroy ();
            ibo.Destroy ();
            uboMats.Destroy ();
            base.Destroy ();
        }
    }
}
