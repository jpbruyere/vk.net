using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using CVKL;
using VK;

namespace ModelSample {
	class Program : VkWindow {
		static void Main (string[] args) {

			Instance.VALIDATION = true;
			Instance.DEBUG_UTILS = true;
			//Instance.RENDER_DOC_CAPTURE = true;

			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}

		struct Matrices {
			public Matrix4x4 projection;
			public Matrix4x4 view;
			public Matrix4x4 model;
			//public Vector4 lightPos;
		}

		public struct PushConstants {
			public Matrix4x4 matrix;
		}

		Matrices matrices;

		HostBuffer uboMats;

		DescriptorPool descriptorPool;
		DescriptorSet dsMatrices, dsTextures;
		DescriptorSetLayout descLayoutMatrix;
		DescriptorSetLayout descLayoutTextures;

		Framebuffer[] frameBuffers;

		GraphicPipeline pipeline;

		VkSampleCountFlags NUM_SAMPLES = VkSampleCountFlags.SampleCount1;

		float rotSpeed = 0.01f, zoomSpeed = 0.01f;
		float rotX, rotY, rotZ = 0f, zoom = 2f;

		SimpleModel helmet;
		CVKL.DebugUtils.Messenger dbgmsg;

		Program () : base () {
			if (Instance.DEBUG_UTILS)
				dbgmsg = new CVKL.DebugUtils.Messenger (instance);

			descriptorPool = new DescriptorPool (dev, 2,
				new VkDescriptorPoolSize (VkDescriptorType.UniformBuffer),
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler, 3));

			descLayoutMatrix = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Vertex, VkDescriptorType.UniformBuffer));

			descLayoutTextures = new DescriptorSetLayout (dev, 
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (2, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler)
			);

			VkPushConstantRange pushConstantRange = new VkPushConstantRange { 
				stageFlags = VkShaderStageFlags.Vertex,
				size = (uint)Marshal.SizeOf<PushConstants>(),
				offset = 0
			};

			GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, NUM_SAMPLES);
			cfg.rasterizationState.cullMode = VkCullModeFlags.Back;
			if (NUM_SAMPLES != VkSampleCountFlags.SampleCount1) {
				cfg.multisampleState.sampleShadingEnable = true;
				cfg.multisampleState.minSampleShading = 0.5f;
			}

			cfg.Layout = new PipelineLayout (dev, pushConstantRange, descLayoutMatrix, descLayoutTextures);
			cfg.RenderPass = new RenderPass (dev, swapChain.ColorFormat, dev.GetSuitableDepthFormat (), cfg.Samples);
			cfg.AddVertexBinding<Model.Vertex> (0);
			cfg.AddVertexAttributes (0, VkFormat.R32g32b32Sfloat, VkFormat.R32g32b32Sfloat, VkFormat.R32g32Sfloat);

			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/model.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/model.frag.spv");

			pipeline = new GraphicPipeline (cfg);

			helmet = new SimpleModel (presentQueue, "data/models/DamagedHelmet/glTF/DamagedHelmet.gltf");
			//helmet = new SimpleModel (presentQueue, "data/models/Hubble.glb");

			//helmet = new SimpleModel (presentQueue, "/mnt/devel/vulkan/Lugdunum/resources/models/Box.gltf");

			dsMatrices = descriptorPool.Allocate (descLayoutMatrix);
			dsTextures = descriptorPool.Allocate (descLayoutTextures);

			uboMats = new HostBuffer (dev, VkBufferUsageFlags.UniformBuffer, matrices);
			//matrices.lightPos = new Vector4 (0.0f, 0.0f, -2.0f, 1.0f);

			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (dsMatrices, descLayoutMatrix);
			uboUpdate.Write (dev, uboMats.Descriptor);

			DescriptorSetWrites texturesUpdate = new DescriptorSetWrites (dsTextures, descLayoutTextures);
			texturesUpdate.Write (dev,
				helmet.textures[0].Descriptor,
				helmet.textures[1].Descriptor,
				helmet.textures[2].Descriptor);

			uboMats.Map ();//permanent map
		}

		public override void UpdateView () {
			matrices.projection = Matrix4x4.CreatePerspectiveFieldOfView (Utils.DegreesToRadians (45f),
				(float)swapChain.Width / (float)swapChain.Height, 0.1f, 256.0f) * Camera.VKProjectionCorrection;
			matrices.view =
				Matrix4x4.CreateFromAxisAngle (Vector3.UnitZ, rotZ) *
				Matrix4x4.CreateFromAxisAngle (Vector3.UnitY, rotY) *
				Matrix4x4.CreateFromAxisAngle (Vector3.UnitX, rotX) *
				Matrix4x4.CreateTranslation (0, 0, -3f * zoom);
			matrices.model = Matrix4x4.Identity;
			uboMats.Update (matrices, (uint)Marshal.SizeOf<Matrices> ());
			updateViewRequested = false;
		}

		protected override void onMouseMove (double xPos, double yPos) {
			double diffX = lastMouseX - xPos;
			double diffY = lastMouseY - yPos;
			if (MouseButton[0]) {
				rotY -= rotSpeed * (float)diffX;
				rotX += rotSpeed * (float)diffY;
			} else if (MouseButton[1]) {
				zoom += zoomSpeed * (float)diffY;
			}
			updateViewRequested = true;
		}
		void buildCommandBuffers () {
			cmdPool.Reset (VkCommandPoolResetFlags.ReleaseResources);
			cmds = cmdPool.AllocateCommandBuffer (swapChain.ImageCount);

			for (int i = 0; i < swapChain.ImageCount; ++i) {
				Framebuffer fb = frameBuffers[i];
				cmds[i].Start ();

				pipeline.RenderPass.Begin (cmds[i], fb);

				cmds[i].SetViewport (swapChain.Width, swapChain.Height);
				cmds[i].SetScissor (swapChain.Width, swapChain.Height);

				cmds[i].BindDescriptorSet (pipeline.Layout, dsMatrices);
				cmds[i].BindDescriptorSet (pipeline.Layout, dsTextures, 1);

				PushConstants pc = new PushConstants { matrix = Matrix4x4.Identity };
				cmds[i].PushConstant (pipeline.Layout, VkShaderStageFlags.Vertex, pc, (uint)Marshal.SizeOf<Matrix4x4> ());

				cmds[i].BindPipeline (pipeline);

				helmet.DrawAll (cmds[i], pipeline.Layout);

				pipeline.RenderPass.End (cmds[i]);

				cmds[i].End ();
			}
		}

		protected override void OnResize () {

			if (frameBuffers != null)
				for (int i = 0; i < swapChain.ImageCount; ++i)
					frameBuffers[i]?.Dispose ();
			frameBuffers = new Framebuffer[swapChain.ImageCount];

			for (int i = 0; i < swapChain.ImageCount; ++i)
				frameBuffers[i] = new Framebuffer (pipeline.RenderPass, swapChain.Width, swapChain.Height,
					(pipeline.Samples == VkSampleCountFlags.SampleCount1) ? new Image[] {
						swapChain.images[i],
						null
					} : new Image[] {
						null,
						null,
						swapChain.images[i]
					});

			buildCommandBuffers ();
		}

		class SimpleModel : Model {
			public GPUBuffer vbo;
			public GPUBuffer ibo;
			public Image[] textures;

			public SimpleModel (Queue transferQ, string path) {
				dev = transferQ.Dev;

				using (CommandPool cmdPool = new CommandPool (dev, transferQ.index)) {
					using (CVKL.glTFLoader ctx = new CVKL.glTFLoader (path, transferQ, cmdPool)) {
						ulong vertexCount, indexCount;

						ctx.GetVertexCount (out vertexCount, out indexCount, out IndexBufferType);

						ulong vertSize = vertexCount * (ulong)Marshal.SizeOf<Vertex> ();
						ulong idxSize = indexCount * (IndexBufferType == VkIndexType.Uint16 ? 2ul : 4ul);
						ulong size = vertSize + idxSize;

						vbo = new GPUBuffer (dev, VkBufferUsageFlags.VertexBuffer | VkBufferUsageFlags.TransferDst, vertSize);
						ibo = new GPUBuffer (dev, VkBufferUsageFlags.IndexBuffer | VkBufferUsageFlags.TransferDst, idxSize);

						vbo.SetName ("vbo gltf");
						ibo.SetName ("ibo gltf");

						Meshes = new List<Mesh> (ctx.LoadMeshes<Vertex> (IndexBufferType, vbo, 0, ibo, 0));
						Scenes = new List<Scene> (ctx.LoadScenes (out defaultSceneIndex));

						textures = ctx.LoadImages ();
					}
				}
			}
			/// <summary> bind vertex and index buffers </summary>
			public virtual void Bind (CommandBuffer cmd) {
				cmd.BindVertexBuffer (vbo);
				cmd.BindIndexBuffer (ibo, IndexBufferType);
			}
			public void DrawAll (CommandBuffer cmd, PipelineLayout pipelineLayout) {
				//helmet.Meshes
				cmd.BindVertexBuffer (vbo);
				cmd.BindIndexBuffer (ibo, IndexBufferType);
				foreach (Mesh m in Meshes) {
					foreach (var p in m.Primitives) {
						cmd.DrawIndexed (p.indexCount,1,p.indexBase,p.vertexBase);
					}
				}

				//foreach (Scene sc in Scenes) {
				//	foreach (Node node in sc.Root.Children)
				//		RenderNode (cmd, pipelineLayout, node, sc.Root.localMatrix, shadowPass);
				//}
			}
		}

		protected override void Dispose (bool disposing) {
			if (disposing) {
				if (!isDisposed) {
					helmet.vbo.Dispose ();
					helmet.ibo.Dispose ();
					foreach (var t in helmet.textures) 
						t.Dispose ();

					pipeline.Dispose ();
					descLayoutMatrix.Dispose ();
					descLayoutTextures.Dispose ();
					for (int i = 0; i < swapChain.ImageCount; i++)
						frameBuffers[i]?.Dispose ();
					descriptorPool.Dispose ();
					uboMats.Dispose ();
					dbgmsg?.Dispose ();
				}
			}

			base.Dispose (disposing);
		}
	}
}
