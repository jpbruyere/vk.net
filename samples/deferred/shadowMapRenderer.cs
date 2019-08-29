/*shadow mapping greatly inspired from:
* Vulkan Example - Deferred shading with shadows from multiple light sources using geometry shader instancing
* Copyright (C) 2016 by Sascha Willems - www.saschawillems.de
* This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
*/
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using CVKL;
using CVKL.glTF;
using VK;
using static deferred.DeferredPbrRenderer;

namespace deferred {
	public class ShadowMapRenderer : IDisposable {
		Device dev;
		Queue gQueue;

		public static uint SHADOWMAP_SIZE = 4096;
		public static VkFormat SHADOWMAP_FORMAT = VkFormat.D32SfloatS8Uint;
		public static VkSampleCountFlags SHADOWMAP_NUM_SAMPLES = VkSampleCountFlags.SampleCount1;
		public bool updateShadowMap = true;

		public float depthBiasConstant = 1.5f;
		public float depthBiasSlope = 1.75f;
		float lightFOV = Utils.DegreesToRadians (60);
		float lightFarPlane;



		RenderPass shadowPass;
		Framebuffer fbShadowMap;
		public Image shadowMap { get; private set; }
		Pipeline shadowPipeline;
		DescriptorPool descriptorPool;
		DescriptorSetLayout descLayoutShadow;
		DescriptorSet dsShadow;
		DeferredPbrRenderer renderer;

		public ShadowMapRenderer (Queue gQueue, DeferredPbrRenderer renderer, float farPlane = 16f) {
			this.lightFarPlane = farPlane;
			this.gQueue = gQueue;
			this.dev = gQueue.Dev;
			this.renderer = renderer;

			descriptorPool = new DescriptorPool (dev, 1,
				new VkDescriptorPoolSize (VkDescriptorType.UniformBuffer, 2)
			);

			init ();
		}

		void init () {

			//Shadow map renderpass
			shadowPass = new RenderPass (dev, VkSampleCountFlags.SampleCount1);
			shadowPass.AddAttachment (SHADOWMAP_FORMAT, VkImageLayout.DepthStencilReadOnlyOptimal, SHADOWMAP_NUM_SAMPLES);
			shadowPass.ClearValues.Add (new VkClearValue { depthStencil = new VkClearDepthStencilValue (1.0f, 0) });

			SubPass subpass0 = new SubPass ();
			subpass0.SetDepthReference (0);
			shadowPass.AddSubpass (subpass0);

			shadowPass.AddDependency (Vk.SubpassExternal, 0,
				VkPipelineStageFlags.FragmentShader, VkPipelineStageFlags.EarlyFragmentTests,
				VkAccessFlags.ShaderRead, VkAccessFlags.DepthStencilAttachmentWrite);
			shadowPass.AddDependency (0, Vk.SubpassExternal,
				VkPipelineStageFlags.LateFragmentTests, VkPipelineStageFlags.FragmentShader,
				VkAccessFlags.DepthStencilAttachmentWrite, VkAccessFlags.ShaderRead);

			descLayoutShadow = new DescriptorSetLayout (dev,
				new VkDescriptorSetLayoutBinding (0, VkShaderStageFlags.Geometry, VkDescriptorType.UniformBuffer),//matrices
				new VkDescriptorSetLayoutBinding (1, VkShaderStageFlags.Geometry, VkDescriptorType.UniformBuffer));//lights

			GraphicPipelineConfig cfg = GraphicPipelineConfig.CreateDefault (VkPrimitiveTopology.TriangleList);
			cfg.rasterizationState.cullMode = VkCullModeFlags.Back;
			cfg.rasterizationState.depthBiasEnable = true;
			cfg.dynamicStates.Add (VkDynamicState.DepthBias);

			cfg.RenderPass = shadowPass;

			cfg.Layout = new PipelineLayout (dev, descLayoutShadow);
			cfg.Layout.AddPushConstants (
				new VkPushConstantRange (VkShaderStageFlags.Vertex|VkShaderStageFlags.Geometry, (uint)Marshal.SizeOf<Matrix4x4> ())
			);

			cfg.AddVertexBinding<PbrModel.Vertex> (0);
			cfg.AddVertexAttributes (0, VkFormat.R32g32b32Sfloat);

			cfg.AddShader (VkShaderStageFlags.Vertex, "#deferred.shadow.vert.spv");
			cfg.AddShader (VkShaderStageFlags.Geometry, "#deferred.shadow.geom.spv");

			shadowPipeline = new GraphicPipeline (cfg);

			//shadow map image
			shadowMap = new Image (dev, SHADOWMAP_FORMAT, VkImageUsageFlags.DepthStencilAttachment | VkImageUsageFlags.Sampled, VkMemoryPropertyFlags.DeviceLocal, SHADOWMAP_SIZE, SHADOWMAP_SIZE,
				VkImageType.Image2D, SHADOWMAP_NUM_SAMPLES, VkImageTiling.Optimal, 1, (uint)renderer.lights.Length);
			shadowMap.CreateView (VkImageViewType.ImageView2DArray, VkImageAspectFlags.Depth, shadowMap.CreateInfo.arrayLayers);
			shadowMap.CreateSampler (VkSamplerAddressMode.ClampToBorder);
			shadowMap.Descriptor.imageLayout = VkImageLayout.DepthStencilReadOnlyOptimal;

			fbShadowMap = new Framebuffer (shadowPass, SHADOWMAP_SIZE, SHADOWMAP_SIZE, (uint)renderer.lights.Length);
			fbShadowMap.attachments.Add (shadowMap);
			fbShadowMap.Activate ();

			dsShadow = descriptorPool.Allocate (descLayoutShadow);

			DescriptorSetWrites dsWrite = new DescriptorSetWrites (dsShadow, descLayoutShadow);
			dsWrite.Write (dev, renderer.uboMatrices.Descriptor, renderer.uboLights.Descriptor);
		}

		public void update_light_matrices () {
			Matrix4x4 proj = Matrix4x4.CreatePerspectiveFieldOfView (lightFOV, 1, 0.1f, lightFarPlane);
			for (int i = 0; i < renderer.lights.Length; i++) {
				Matrix4x4 view = Matrix4x4.CreateLookAt (renderer.lights[i].position.ToVector3 (), Vector3.Zero, Vector3.UnitY);
				renderer.lights[i].mvp = view * proj;
			}
			renderer.uboLights.Update (renderer.lights);
			dev.WaitIdle ();
		}

		public void update_shadow_map (CommandPool cmdPool) {
			update_light_matrices ();

			CommandBuffer cmd = cmdPool.AllocateAndStart ();

			shadowPass.Begin (cmd, fbShadowMap);

			cmd.SetViewport (SHADOWMAP_SIZE, SHADOWMAP_SIZE);
			cmd.SetScissor (SHADOWMAP_SIZE, SHADOWMAP_SIZE);

			cmd.BindDescriptorSet (shadowPipeline.Layout, dsShadow);

			Vk.vkCmdSetDepthBias (cmd.Handle, depthBiasConstant, 0.0f, depthBiasSlope);

			shadowPipeline.Bind (cmd);

			if (renderer.model != null) {
				renderer.model.Bind (cmd);
				renderer.model.DrawAll (cmd, shadowPipeline.Layout, true);
			}

			shadowPass.End (cmd);

			gQueue.EndSubmitAndWait (cmd);
			updateShadowMap = false;
		}


		public void Dispose () {
			shadowPipeline?.Dispose ();
			fbShadowMap?.Dispose ();
			shadowMap?.Dispose ();
			descriptorPool?.Dispose ();
		}
	}
}
