using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;
using VK;
using VKE;

namespace PbrSample {

	class EnvironmentCube : Pipeline {
		DescriptorPool descriptorPool;
		DescriptorSetLayout descLayoutMain;
		public DescriptorSet dsSkybox;

		GPUBuffer vboSkybox;

		public Image cubemap { get; private set; }
		public Image lutBrdf { get; private set; }
		public Image irradianceCube { get; private set; }
		public Image prefilterCube { get; private set; }

		public EnvironmentCube (Queue staggingQ, RenderPass renderPass)
		: base (renderPass, "EnvCube pipeline") {
			descriptorPool = new DescriptorPool (dev, 1,
				new VkDescriptorPoolSize (VkDescriptorType.UniformBuffer, 1),
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler, 1)
			);

			descLayoutMain = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Vertex, VkDescriptorType.UniformBuffer),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler));

			dsSkybox = descriptorPool.Allocate (descLayoutMain);

			using (CommandPool cmdPool = new CommandPool (staggingQ.Dev, staggingQ.index)) {

				vboSkybox = new GPUBuffer<float> (staggingQ, cmdPool, VkBufferUsageFlags.VertexBuffer, box_vertices);

				cubemap = KTX.KTX.Load (staggingQ, cmdPool, cubemapPathes[4],
					VkImageUsageFlags.Sampled, VkMemoryPropertyFlags.DeviceLocal, true);
				cubemap.CreateView (VkImageViewType.Cube, VkImageAspectFlags.Color, 6);
				cubemap.CreateSampler ();
				cubemap.SetName ("skybox Texture");

				PipelineConfig cfg = PipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, renderPass.Samples);
				cfg.RenderPass = renderPass;
				cfg.Layout = new PipelineLayout (dev, descLayoutMain);
				cfg.AddVertexBinding (0, 5 * sizeof (float));
				cfg.SetVertexAttributes (0, VkFormat.R32g32b32Sfloat, VkFormat.R32g32Sfloat);
				cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/skybox.vert.spv");
				cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/skybox.frag.spv");
				cfg.depthStencilState.depthTestEnable = false;
				cfg.depthStencilState.depthWriteEnable = false;

				layout = cfg.Layout;

				init (cfg);

				generateBRDFLUT (staggingQ, cmdPool);
				generateCubemaps (staggingQ, cmdPool);
			}
		
		}

		public void WriteDesc (VkDescriptorBufferInfo matrixDesc) { 
			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (descLayoutMain);
			uboUpdate.Write (dev, dsSkybox, matrixDesc, cubemap.Descriptor);
		}

		public void RecordDraw (CommandBuffer cmd) {
		 	Bind (cmd);
			cmd.BindDescriptorSet (Layout, dsSkybox);
			cmd.BindVertexBuffer (vboSkybox);
			cmd.Draw (36);
		}

		#region skybox
		public List<string> cubemapPathes = new List<string>() {
			"../data/textures/papermill.ktx",
			"../data/textures/cubemap_yokohama_bc3_unorm.ktx",
			"../data/textures/gcanyon_cube.ktx",
			"../data/textures/pisa_cube.ktx",
			"../data/textures/uffizi_cube.ktx",
		};
		static float[] box_vertices = {
			-1.0f,-1.0f,-1.0f,    0.0f, 1.0f,  // -X side
			-1.0f,-1.0f, 1.0f,    1.0f, 1.0f,
			-1.0f, 1.0f, 1.0f,    1.0f, 0.0f,
			-1.0f, 1.0f, 1.0f,    1.0f, 0.0f,
			-1.0f, 1.0f,-1.0f,    0.0f, 0.0f,
			-1.0f,-1.0f,-1.0f,    0.0f, 1.0f,

			 1.0f, 1.0f,-1.0f,    1.0f, 0.0f,  // +X side
			 1.0f, 1.0f, 1.0f,    0.0f, 0.0f,
			 1.0f,-1.0f, 1.0f,    0.0f, 1.0f,
			 1.0f,-1.0f, 1.0f,    0.0f, 1.0f,
			 1.0f,-1.0f,-1.0f,    1.0f, 1.0f,
			 1.0f, 1.0f,-1.0f,    1.0f, 0.0f,

			-1.0f,-1.0f,-1.0f,    1.0f, 0.0f,  // -Y side
			 1.0f,-1.0f,-1.0f,    1.0f, 1.0f,
			 1.0f,-1.0f, 1.0f,    0.0f, 1.0f,
			-1.0f,-1.0f,-1.0f,    1.0f, 0.0f,
			 1.0f,-1.0f, 1.0f,    0.0f, 1.0f,
			-1.0f,-1.0f, 1.0f,    0.0f, 0.0f,

			-1.0f, 1.0f,-1.0f,    1.0f, 0.0f,  // +Y side
			-1.0f, 1.0f, 1.0f,    0.0f, 0.0f,
			 1.0f, 1.0f, 1.0f,    0.0f, 1.0f,
			-1.0f, 1.0f,-1.0f,    1.0f, 0.0f,
			 1.0f, 1.0f, 1.0f,    0.0f, 1.0f,
			 1.0f, 1.0f,-1.0f,    1.0f, 1.0f,

			-1.0f,-1.0f,-1.0f,    1.0f, 1.0f,  // -Z side
			 1.0f, 1.0f,-1.0f,    0.0f, 0.0f,
			 1.0f,-1.0f,-1.0f,    0.0f, 1.0f,
			-1.0f,-1.0f,-1.0f,    1.0f, 1.0f,
			-1.0f, 1.0f,-1.0f,    1.0f, 0.0f,
			 1.0f, 1.0f,-1.0f,    0.0f, 0.0f,

			-1.0f, 1.0f, 1.0f,    0.0f, 0.0f,  // +Z side
			-1.0f,-1.0f, 1.0f,    0.0f, 1.0f,
			 1.0f, 1.0f, 1.0f,    1.0f, 0.0f,
			-1.0f,-1.0f, 1.0f,    0.0f, 1.0f,
			 1.0f,-1.0f, 1.0f,    1.0f, 1.0f,
			 1.0f, 1.0f, 1.0f,    1.0f, 0.0f,
		};
		#endregion

		void generateBRDFLUT (Queue staggingQ, CommandPool cmdPool) {
			const VkFormat format = VkFormat.R16g16Sfloat;
			const int dim = 512;

			lutBrdf = new Image (dev, format, VkImageUsageFlags.ColorAttachment | VkImageUsageFlags.Sampled,
				VkMemoryPropertyFlags.DeviceLocal, dim, dim);
			lutBrdf.SetName ("lutBrdf");

			lutBrdf.CreateView ();
			lutBrdf.CreateSampler (VkSamplerAddressMode.ClampToEdge);

			PipelineConfig cfg = PipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, VkSampleCountFlags.SampleCount1, false);

			cfg.Layout = new PipelineLayout (dev, new DescriptorSetLayout (dev));
			cfg.RenderPass = new RenderPass (dev);
			cfg.RenderPass.AddAttachment (format, VkImageLayout.ShaderReadOnlyOptimal);
			cfg.RenderPass.ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0, 0, 0) });
			cfg.RenderPass.AddSubpass (new SubPass (VkImageLayout.ColorAttachmentOptimal));
			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/genbrdflut.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/genbrdflut.frag.spv");

			using (Pipeline pl = new Pipeline (cfg)) {
				using (Framebuffer fb = new Framebuffer (cfg.RenderPass, dim, dim, lutBrdf)) {
					CommandBuffer cmd = cmdPool.AllocateCommandBuffer ();
					cmd.Start (VkCommandBufferUsageFlags.OneTimeSubmit);
					pl.RenderPass.Begin (cmd, fb);
					cmd.SetViewport (dim, dim);
					cmd.SetScissor (dim, dim);
					pl.Bind (cmd);
					cmd.Draw (3, 1, 0, 0);
					pl.RenderPass.End (cmd);
					cmd.End ();

					staggingQ.Submit (cmd);
					staggingQ.WaitIdle ();

					cmd.Free ();
				}
			}
			lutBrdf.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
		}

		enum CBTarget { IRRADIANCE = 0, PREFILTEREDENV = 1 };

		Image generateCubeMap (Queue staggingQ, CommandPool cmdPool, CBTarget target) {
			const float deltaPhi = (2.0f * (float)Math.PI) / 180.0f;
			const float deltaTheta = (0.5f * (float)Math.PI) / 64.0f;

			VkFormat format = VkFormat.R32g32b32a32Sfloat;
			uint dim = 64;

			if (target == CBTarget.PREFILTEREDENV) {
				format = VkFormat.R16g16b16a16Sfloat;
				dim = 512;
			}

			uint numMips = (uint)Math.Floor (Math.Log (dim)) + 1;

			Image imgFbOffscreen = new Image (dev, format, VkImageUsageFlags.TransferSrc | VkImageUsageFlags.ColorAttachment,
				VkMemoryPropertyFlags.DeviceLocal, dim, dim);
			imgFbOffscreen.SetName ("offscreenfb");
			imgFbOffscreen.CreateView ();

			Image cmap = new Image (dev, format, VkImageUsageFlags.TransferDst | VkImageUsageFlags.Sampled,
				VkMemoryPropertyFlags.DeviceLocal, dim, dim, VkImageType.Image2D, VkSampleCountFlags.SampleCount1, VkImageTiling.Optimal,
				numMips, 6,  1, VkImageCreateFlags.CubeCompatible);
			if (target == CBTarget.PREFILTEREDENV)
				cmap.SetName ("prefilterenvmap");
			else
				cmap.SetName ("irradianceCube");
			cmap.CreateView (VkImageViewType.Cube, VkImageAspectFlags.Color, 6, 0, numMips);
			cmap.CreateSampler (VkSamplerAddressMode.ClampToEdge);

			DescriptorPool dsPool = new DescriptorPool (dev, 2,	new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler));

			DescriptorSetLayout dsLayout = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler));

			DescriptorSet dset = dsPool.Allocate (dsLayout);

			PipelineConfig cfg = PipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, VkSampleCountFlags.SampleCount1, false);

			DescriptorSetWrites dsUpdate = new DescriptorSetWrites (dsLayout);
			dsUpdate.Write (dev, dset, cubemap.Descriptor);

			cfg.Layout = new PipelineLayout (dev, dsLayout);
			cfg.Layout.AddPushConstants (
				new VkPushConstantRange (VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment, (uint)Marshal.SizeOf<Matrix4x4> () + 8));

			cfg.RenderPass = new RenderPass (dev);
			cfg.RenderPass.AddAttachment (format, VkImageLayout.ColorAttachmentOptimal);
			cfg.RenderPass.ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0, 0, 0) });
			cfg.RenderPass.AddSubpass (new SubPass (VkImageLayout.ColorAttachmentOptimal));

			cfg.AddVertexBinding (0, 5 * sizeof (float));
			cfg.SetVertexAttributes (0, VkFormat.R32g32b32Sfloat);

			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/filtercube.vert.spv");
			if (target == CBTarget.PREFILTEREDENV)
				cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/prefilterenvmap.frag.spv");
			else
				cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/irradiancecube.frag.spv");

			Matrix4x4[] matrices = {
				// POSITIVE_X
				Matrix4x4.CreateRotationX(Utils.DegreesToRadians(180)) * Matrix4x4.CreateRotationY(Utils.DegreesToRadians(90)),
				// NEGATIVE_X
				Matrix4x4.CreateRotationX(Utils.DegreesToRadians(180)) * Matrix4x4.CreateRotationY(Utils.DegreesToRadians(-90)),
				// POSITIVE_Y
				Matrix4x4.CreateRotationX(Utils.DegreesToRadians(-90)),
				// NEGATIVE_Y
				Matrix4x4.CreateRotationX(Utils.DegreesToRadians(90)),
				// POSITIVE_Z
				Matrix4x4.CreateRotationX(Utils.DegreesToRadians(180)),
				// NEGATIVE_Z
				Matrix4x4.CreateRotationZ(Utils.DegreesToRadians(180))
			};

			VkImageSubresourceRange subRes = new VkImageSubresourceRange (VkImageAspectFlags.Color, 0, numMips, 0, 6);

			using (Pipeline pl = new Pipeline (cfg)) {
				using (Framebuffer fb = new Framebuffer (pl.RenderPass, dim, dim, imgFbOffscreen)) {
					CommandBuffer cmd = cmdPool.AllocateCommandBuffer ();
					cmd.Start (VkCommandBufferUsageFlags.OneTimeSubmit);

					cmap.SetLayout (cmd, VkImageLayout.Undefined, VkImageLayout.TransferDstOptimal, subRes);
						
					float roughness = 0;

					cmd.SetScissor (dim, dim);
					cmd.SetViewport ((float)(dim), (float)dim);

					for (int m = 0; m < numMips; m++) {
						roughness = (float)m / (numMips - 1);
						for (int f = 0; f < 6; f++) {


							pl.RenderPass.Begin (cmd, fb);

							pl.Bind (cmd);

							float viewPortSize = (float)Math.Pow (0.5, m) * dim;
							cmd.SetViewport (viewPortSize, viewPortSize);


							cmd.PushConstant (pl.Layout, VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment,
								matrices[f] *
								Matrix4x4.CreatePerspectiveFieldOfView (Utils.DegreesToRadians (90), 1f, 0.1f, 512f)) ;
							if (target == CBTarget.IRRADIANCE) {
								cmd.PushConstant (pl.Layout, VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment, deltaPhi, (uint)Marshal.SizeOf<Matrix4x4> ());
								cmd.PushConstant (pl.Layout, VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment, deltaTheta, (uint)Marshal.SizeOf<Matrix4x4> () + 4);
							} else {
								cmd.PushConstant (pl.Layout, VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment, roughness, (uint)Marshal.SizeOf<Matrix4x4> ());
								cmd.PushConstant (pl.Layout, VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment, 32u, (uint)Marshal.SizeOf<Matrix4x4> () + 4);
							}
							cmd.BindDescriptorSet (pl.Layout, dset);
							cmd.BindVertexBuffer (vboSkybox);
							cmd.Draw (36);

							pl.RenderPass.End (cmd);

							imgFbOffscreen.SetLayout (cmd, VkImageAspectFlags.Color,
								VkImageLayout.ColorAttachmentOptimal, VkImageLayout.TransferSrcOptimal);

							VkImageCopy region = new VkImageCopy ();
							region.srcSubresource = new VkImageSubresourceLayers (VkImageAspectFlags.Color, 1);
							region.dstSubresource = new VkImageSubresourceLayers (VkImageAspectFlags.Color, 1, (uint)m, (uint)f);
							region.extent = new VkExtent3D { width = (uint)viewPortSize, height = (uint)viewPortSize, depth = 1 };

							Vk.vkCmdCopyImage (cmd.Handle,
								imgFbOffscreen.Handle, VkImageLayout.TransferSrcOptimal,
								cmap.Handle, VkImageLayout.TransferDstOptimal,
								1, region.Pin ());
							region.Unpin ();

							imgFbOffscreen.SetLayout (cmd, VkImageAspectFlags.Color,
								VkImageLayout.TransferSrcOptimal, VkImageLayout.ColorAttachmentOptimal);

						}
					}

					cmap.SetLayout (cmd, VkImageLayout.TransferDstOptimal, VkImageLayout.ShaderReadOnlyOptimal, subRes);

					cmd.End ();

					staggingQ.Submit (cmd);
					staggingQ.WaitIdle ();

					cmd.Free ();
				}
			}
			cmap.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;

			dsLayout.Dispose ();
			imgFbOffscreen.Dispose ();
			dsPool.Dispose ();

			return cmap;
		}

		void generateCubemaps (Queue staggingQ, CommandPool cmdPool) {
			irradianceCube = generateCubeMap (staggingQ, cmdPool, CBTarget.IRRADIANCE);
			prefilterCube = generateCubeMap (staggingQ, cmdPool, CBTarget.PREFILTEREDENV);
		}

		protected override void Dispose (bool disposing) {
			vboSkybox.Dispose ();
			cubemap.Dispose ();
			lutBrdf.Dispose ();
			irradianceCube.Dispose ();
			prefilterCube.Dispose ();

			descLayoutMain.Dispose ();
			descriptorPool.Dispose ();

			base.Dispose (disposing);
		}
	}


	class PBRPipeline : Pipeline {	
		public struct Matrices {
			public Matrix4x4 projection;
			public Matrix4x4 view;
			public Matrix4x4 model;
			public Vector4 lightPos;
			public float gamma;
			public float exposure;
		}

		public Matrices matrices = new Matrices {
			lightPos = new Vector4 (1.0f, 0.0f, 0.0f, 1.0f),
			gamma = 1.0f,
			exposure = 2.0f,
		};

		public HostBuffer uboMats;

		DescriptorPool descriptorPool;
		DescriptorSetLayout descLayoutMain;
		DescriptorSetLayout descLayoutTextures;
		DescriptorSet dsMain;

		Model model;
		EnvironmentCube envCube;

		public PBRPipeline (Queue staggingQ, RenderPass renderPass) :
			base (renderPass, "pbr pipeline") {

			descriptorPool = new DescriptorPool (dev, 2,
				new VkDescriptorPoolSize (VkDescriptorType.UniformBuffer, 2),
				new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler, 8)
			);

			descLayoutMain = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment, VkDescriptorType.UniformBuffer),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (2, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (3, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler));

			descLayoutTextures = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (2, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (3, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler),
				new VkDescriptorSetLayoutBinding (4, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler)
			);

			dsMain = descriptorPool.Allocate (descLayoutMain);

			PipelineConfig cfg = PipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, renderPass.Samples);

			cfg.Layout = new PipelineLayout (dev, descLayoutMain, descLayoutTextures);
			cfg.Layout.AddPushConstants (
				new VkPushConstantRange (VkShaderStageFlags.Vertex, (uint)Marshal.SizeOf<Matrix4x4> ()),
				new VkPushConstantRange (VkShaderStageFlags.Fragment, (uint)Marshal.SizeOf<Model.PbrMaterial> (), 64)
			);
			cfg.RenderPass = renderPass;
			cfg.AddVertexBinding<Model.Vertex> (0);
			cfg.SetVertexAttributes (0, VkFormat.R32g32b32Sfloat, VkFormat.R32g32b32Sfloat, VkFormat.R32g32Sfloat);

			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/pbrtest.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/pbrtest.frag.spv");

			layout = cfg.Layout;

			init (cfg);

			envCube = new EnvironmentCube (staggingQ, RenderPass);

			uboMats = new HostBuffer (dev, VkBufferUsageFlags.UniformBuffer, (ulong)Marshal.SizeOf<Matrices> () * 2);
			uboMats.Map ();//permanent map

			DescriptorSetWrites uboUpdate = new DescriptorSetWrites (descLayoutMain);
			uboUpdate.Write (dev, dsMain, uboMats.Descriptor,
				envCube.lutBrdf.Descriptor,
				envCube.irradianceCube.Descriptor,
				envCube.prefilterCube.Descriptor);
			uboMats.Descriptor.offset = (ulong)Marshal.SizeOf<Matrices> ();
			envCube.WriteDesc (uboMats.Descriptor);

			model = new Model (staggingQ, "../data/models/DamagedHelmet/glTF/DamagedHelmet.gltf");
			//model = new Model (dev, presentQueue, "../data/models/icosphere.gltf");
			//model = new Model (dev, presentQueue, cmdPool, "../data/models/cube.gltf");
			model.WriteMaterialsDescriptorSets (descLayoutTextures,
				ShaderBinding.Color,
				ShaderBinding.Normal,
				ShaderBinding.AmbientOcclusion,
				ShaderBinding.MetalRoughness,
				ShaderBinding.Emissive);
		}

		public void RecordDraw (CommandBuffer cmd) {		
			envCube.RecordDraw (cmd);
			drawModel (cmd);
		}
		void drawModel (CommandBuffer cmd) {
			Bind (cmd);
			cmd.BindDescriptorSet (Layout, dsMain);
			model.Bind (cmd);
			model.DrawAll (cmd, Layout);
		}
		void drawShadedModelArray (CommandBuffer cmd) {
			Bind (cmd);
			cmd.BindDescriptorSet (Layout, dsMain);
			model.Bind (cmd);
			model.DrawAll (cmd, Layout);


			Matrix4x4 modelMat = Matrix4x4.Identity;
			Model.PbrMaterial material = new Model.PbrMaterial (1, 0, Model.AlphaMode.Opaque, 0.5f, ShaderBinding.None);
			Model.Primitive p = model.Scenes[0].Root.Children[0].Mesh.Primitives[0];

			for (int metalFact = 0; metalFact < 10; metalFact++) {

				material.metallicFactor = (float)metalFact / 10;

				for (int roughFact = 0; roughFact < 10; roughFact++) {

					material.roughnessFactor = (float)roughFact / 10;

					modelMat = Matrix4x4.CreateTranslation (-roughFact, metalFact, 0);
				

					cmd.PushConstant (Layout, VkShaderStageFlags.Vertex, modelMat);
					cmd.PushConstant (Layout, VkShaderStageFlags.Fragment, material, (uint)Marshal.SizeOf<Matrix4x4> ());

					cmd.DrawIndexed (p.indexCount, 1, p.indexBase, p.vertexBase, 0);
				}
			}


		}

		protected override void Dispose (bool disposing) {
			model.Dispose ();
			envCube.Dispose ();

			descLayoutMain.Dispose ();
			descLayoutTextures.Dispose ();
			descriptorPool.Dispose ();

			uboMats.Dispose ();

			base.Dispose (disposing);
		}
	}


	class Program : VkWindow, Crow.IValueChange {	
		static void Main (string[] args) {
			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}

#if DEBUG && DEBUG_MARKER
		public override string[] EnabledExtensions => new string[] { "VK_KHR_swapchain", "VK_EXT_debug_marker" };
#endif

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
			UpdateFrequency = 20;
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
