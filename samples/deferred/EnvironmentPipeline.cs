using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using VK;

namespace CVKL {
	public class EnvironmentCube : GraphicPipeline {

		GPUBuffer vboSkybox;

		public Image cubemap { get; private set; }
		public Image lutBrdf { get; private set; }
		public Image irradianceCube { get; private set; }
		public Image prefilterCube { get; set; }

		public EnvironmentCube (string cubemapPath, DescriptorSet dsSkybox, PipelineLayout plLayout, Queue staggingQ, RenderPass renderPass, PipelineCache cache = null)
		: base (renderPass, cache, "EnvCube pipeline") {

			using (CommandPool cmdPool = new CommandPool (staggingQ.Dev, staggingQ.index)) {

				vboSkybox = new GPUBuffer<float> (staggingQ, cmdPool, VkBufferUsageFlags.VertexBuffer, box_vertices);

				cubemap = KTX.KTX.Load (staggingQ, cmdPool, cubemapPath,
					VkImageUsageFlags.Sampled, VkMemoryPropertyFlags.DeviceLocal, true);
				cubemap.CreateView (VkImageViewType.Cube, VkImageAspectFlags.Color, 6, 0, cubemap.CreateInfo.mipLevels);
				cubemap.CreateSampler (VkSamplerAddressMode.ClampToEdge);
				cubemap.SetName ("skybox Texture");
				cubemap.Descriptor.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;

				GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, renderPass.Samples, false);
				cfg.RenderPass = renderPass;
				cfg.Layout = plLayout;
				cfg.AddVertexBinding (0, 3 * sizeof (float));
				cfg.AddVertexAttributes (0, VkFormat.R32g32b32Sfloat);
				cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/skybox.vert.spv");
				cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/skybox.frag.spv");
				cfg.multisampleState.rasterizationSamples = Samples;

				layout = cfg.Layout;

				init (cfg);

				generateBRDFLUT (staggingQ, cmdPool);
				generateCubemaps (staggingQ, cmdPool);
			}

		}

		public void RecordDraw (CommandBuffer cmd) {
			Bind (cmd);
			cmd.BindVertexBuffer (vboSkybox);
			cmd.Draw (36);
		}

		#region skybox

		static float[] box_vertices = {
			 1.0f, 1.0f,-1.0f,  // +X side
			 1.0f, 1.0f, 1.0f,
			 1.0f,-1.0f, 1.0f,
			 1.0f,-1.0f, 1.0f,
			 1.0f,-1.0f,-1.0f,
			 1.0f, 1.0f,-1.0f,

			-1.0f,-1.0f,-1.0f, 	// +X side
			-1.0f,-1.0f, 1.0f,
			-1.0f, 1.0f, 1.0f,
			-1.0f, 1.0f, 1.0f,
			-1.0f, 1.0f,-1.0f,
			-1.0f,-1.0f,-1.0f,

			-1.0f, 1.0f,-1.0f,  // +Y side
			-1.0f, 1.0f, 1.0f,
			 1.0f, 1.0f, 1.0f,
			-1.0f, 1.0f,-1.0f,
			 1.0f, 1.0f, 1.0f,
			 1.0f, 1.0f,-1.0f,

			-1.0f,-1.0f,-1.0f,  // -Y side
			 1.0f,-1.0f,-1.0f,
			 1.0f,-1.0f, 1.0f,
			-1.0f,-1.0f,-1.0f,
			 1.0f,-1.0f, 1.0f,
			-1.0f,-1.0f, 1.0f,

			-1.0f, 1.0f, 1.0f,  // +Z side
			-1.0f,-1.0f, 1.0f,
			 1.0f, 1.0f, 1.0f,
			-1.0f,-1.0f, 1.0f,
			 1.0f,-1.0f, 1.0f,
			 1.0f, 1.0f, 1.0f,

			-1.0f,-1.0f,-1.0f,  // -Z side
			 1.0f, 1.0f,-1.0f,
			 1.0f,-1.0f,-1.0f,
			-1.0f,-1.0f,-1.0f,
			-1.0f, 1.0f,-1.0f,
			 1.0f, 1.0f,-1.0f,

		};
		#endregion

		void generateBRDFLUT (Queue staggingQ, CommandPool cmdPool) {
			const VkFormat format = VkFormat.R16g16Sfloat;
			const int dim = 512;

			lutBrdf = new Image (Dev, format, VkImageUsageFlags.ColorAttachment | VkImageUsageFlags.Sampled,
				VkMemoryPropertyFlags.DeviceLocal, dim, dim);
			lutBrdf.SetName ("lutBrdf");

			lutBrdf.CreateView ();
			lutBrdf.CreateSampler (VkSamplerAddressMode.ClampToEdge);

			GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, VkSampleCountFlags.SampleCount1, false);

			cfg.Layout = new PipelineLayout (Dev, new DescriptorSetLayout (Dev));
			cfg.RenderPass = new RenderPass (Dev);
			cfg.RenderPass.AddAttachment (format, VkImageLayout.ShaderReadOnlyOptimal);
			cfg.RenderPass.ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0, 0, 0) });
			cfg.RenderPass.AddSubpass (new SubPass (VkImageLayout.ColorAttachmentOptimal));
			cfg.AddShader (VkShaderStageFlags.Vertex, "shaders/genbrdflut.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Fragment, "shaders/genbrdflut.frag.spv");

			using (GraphicPipeline pl = new GraphicPipeline (cfg)) {
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

		public enum CBTarget { IRRADIANCE = 0, PREFILTEREDENV = 1 };

		public Image generateCubeMap (Queue staggingQ, CommandPool cmdPool, CBTarget target) {
			const float deltaPhi = (2.0f * (float)Math.PI) / 180.0f;
			const float deltaTheta = (0.5f * (float)Math.PI) / 64.0f;

			VkFormat format = VkFormat.R32g32b32a32Sfloat;
			uint dim = 64;

			if (target == CBTarget.PREFILTEREDENV) {
				format = VkFormat.R16g16b16a16Sfloat;
				dim = 512;
			}

			uint numMips = (uint)Math.Floor (Math.Log (dim, 2)) + 1;

			Image imgFbOffscreen = new Image (Dev, format, VkImageUsageFlags.TransferSrc | VkImageUsageFlags.ColorAttachment,
				VkMemoryPropertyFlags.DeviceLocal, dim, dim);
			imgFbOffscreen.SetName ("offscreenfb");
			imgFbOffscreen.CreateView ();

			Image cmap = new Image (Dev, format, VkImageUsageFlags.TransferDst | VkImageUsageFlags.Sampled,
				VkMemoryPropertyFlags.DeviceLocal, dim, dim, VkImageType.Image2D, VkSampleCountFlags.SampleCount1, VkImageTiling.Optimal,
				numMips, 6, 1, VkImageCreateFlags.CubeCompatible);
			if (target == CBTarget.PREFILTEREDENV)
				cmap.SetName ("prefilterenvmap");
			else
				cmap.SetName ("irradianceCube");
			cmap.CreateView (VkImageViewType.Cube, VkImageAspectFlags.Color, 6, 0, numMips);
			cmap.CreateSampler (VkSamplerAddressMode.ClampToEdge);

			DescriptorPool dsPool = new DescriptorPool (Dev, 2, new VkDescriptorPoolSize (VkDescriptorType.CombinedImageSampler));

			DescriptorSetLayout dsLayout = new DescriptorSetLayout (Dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Fragment, VkDescriptorType.CombinedImageSampler));


			GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList, VkSampleCountFlags.SampleCount1, false);
			cfg.Layout = new PipelineLayout (Dev, dsLayout);
			cfg.Layout.AddPushConstants (
				new VkPushConstantRange (VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment, (uint)Marshal.SizeOf<Matrix4x4> () + 8));

			cfg.RenderPass = new RenderPass (Dev);
			cfg.RenderPass.AddAttachment (format, VkImageLayout.ColorAttachmentOptimal);
			cfg.RenderPass.ClearValues.Add (new VkClearValue { color = new VkClearColorValue (0, 0, 0) });
			cfg.RenderPass.AddSubpass (new SubPass (VkImageLayout.ColorAttachmentOptimal));

			cfg.AddVertexBinding (0, 3 * sizeof (float));
			cfg.AddVertexAttributes (0, VkFormat.R32g32b32Sfloat);

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

			using (GraphicPipeline pl = new GraphicPipeline (cfg)) {

				DescriptorSet dset = dsPool.Allocate (dsLayout);
				DescriptorSetWrites dsUpdate = new DescriptorSetWrites (dsLayout);
				dsUpdate.Write (Dev, dset, cubemap.Descriptor);
				Dev.WaitIdle ();

				using (Framebuffer fb = new Framebuffer (pl.RenderPass, dim, dim, imgFbOffscreen)) {
					CommandBuffer cmd = cmdPool.AllocateCommandBuffer ();
					cmd.Start (VkCommandBufferUsageFlags.OneTimeSubmit);

					cmap.SetLayout (cmd, VkImageLayout.Undefined, VkImageLayout.TransferDstOptimal, subRes);

					float roughness = 0;

					cmd.SetScissor (dim, dim);
					cmd.SetViewport ((float)(dim), (float)dim);

					for (int m = 0; m < numMips; m++) {
						roughness = (float)m / ((float)numMips - 1f);

						for (int f = 0; f < 6; f++) {
							pl.RenderPass.Begin (cmd, fb);

							pl.Bind (cmd);

							float viewPortSize = (float)Math.Pow (0.5, m) * dim;
							cmd.SetViewport (viewPortSize, viewPortSize);
							cmd.PushConstant (pl.Layout, VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment,
								matrices[f] * Matrix4x4.CreatePerspectiveFieldOfView (Utils.DegreesToRadians (90), 1f, 0.1f, 512f));
							if (target == CBTarget.IRRADIANCE) {
								cmd.PushConstant (pl.Layout, VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment, deltaPhi, (uint)Marshal.SizeOf<Matrix4x4> ());
								cmd.PushConstant (pl.Layout, VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment, deltaTheta, (uint)Marshal.SizeOf<Matrix4x4> () + 4);
							} else {
								cmd.PushConstant (pl.Layout, VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment, roughness, (uint)Marshal.SizeOf<Matrix4x4> ());
								cmd.PushConstant (pl.Layout, VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment, 64u, (uint)Marshal.SizeOf<Matrix4x4> () + 4);
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

			base.Dispose (disposing);
		}
	}

}
