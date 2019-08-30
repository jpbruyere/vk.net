// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)

using System;
using System.Runtime.InteropServices;
using VK;

using static VK.Vk;

namespace CVKL {
	/// <summary>
	/// Command buffers are objects used to record commands which can be subsequently submitted to a device queue for execution.
	/// There are two levels of command buffers 
	/// - primary command buffers, which can execute secondary command buffers, and which are submitted to queues
	/// - secondary command buffers, which can be executed by primary command buffers, and which are not directly submitted to queues.
	/// Command buffer are not derived from activable, because their state is retained by the pool which create them.
	/// </summary>
	public class CommandBuffer {
		public enum States { Init, Record, Executable, Pending, Invalid };

        CommandPool pool;
        VkCommandBuffer handle;

        public VkCommandBuffer Handle => handle;
		public Device Device => pool?.Dev;//this help
		//public States State { get; internal set; }

        internal CommandBuffer (VkDevice _dev, CommandPool _pool, VkCommandBuffer _buff)
        {
            pool = _pool;
            handle = _buff;

			//State = States.Init;
        }


        public void Submit (VkQueue queue, VkSemaphore wait = default(VkSemaphore), VkSemaphore signal = default (VkSemaphore), VkFence fence = default(VkFence)) {
            VkSubmitInfo submit_info = VkSubmitInfo.New();

			IntPtr dstStageMask = Marshal.AllocHGlobal (sizeof(uint));
			Marshal.WriteInt32 (dstStageMask, (int)VkPipelineStageFlags.ColorAttachmentOutput);

            submit_info.pWaitDstStageMask = dstStageMask;
            if (signal != VkSemaphore.Null) {
                submit_info.signalSemaphoreCount = 1;
                submit_info.pSignalSemaphores = signal.Pin();
            }
            if (wait != VkSemaphore.Null) {
                submit_info.waitSemaphoreCount = 1;
                submit_info.pWaitSemaphores = wait.Pin();
            }

            submit_info.commandBufferCount = 1;
            submit_info.pCommandBuffers = handle.Pin();

            Utils.CheckResult (vkQueueSubmit (queue, 1, ref submit_info, fence));

			if (signal != VkSemaphore.Null)
				signal.Unpin ();
			if (wait != VkSemaphore.Null)
				wait.Unpin ();
			handle.Unpin ();

			Marshal.FreeHGlobal (dstStageMask);
        }
        public void Start (VkCommandBufferUsageFlags usage = 0) {
            VkCommandBufferBeginInfo cmdBufInfo = new VkCommandBufferBeginInfo (usage);
            Utils.CheckResult (vkBeginCommandBuffer (handle, ref cmdBufInfo));
        }
        public void End () {
            Utils.CheckResult (vkEndCommandBuffer (handle));
        }
        /// <summary>
        /// Update dynamic viewport state
        /// </summary>
        public void SetViewport (float width, float height, float x = 0f, float y = 0f, float minDepth = 0.0f, float maxDepth = 1.0f) {
            VkViewport viewport = new VkViewport {
				x = x,
				y = y,
                height = height,
                width = width,
                minDepth = minDepth,
                maxDepth = maxDepth,
            };
            vkCmdSetViewport (handle, 0, 1, ref viewport);
        }
        /// <summary>
        /// Update dynamic scissor state
        /// </summary>
        public void SetScissor (uint width, uint height, int offsetX = 0, int offsetY = 0) {
            VkRect2D scissor = new VkRect2D (offsetX, offsetY, width, height);
            vkCmdSetScissor (handle, 0, 1, ref scissor);
        }
        public void BindPipeline (Pipeline pipeline, VkPipelineBindPoint bindPoint) {
            vkCmdBindPipeline (handle, bindPoint, pipeline.Handle);
        }
		public void Dispatch (uint groupCountX, uint groupCountY = 1, uint groupCountZ = 1) {
			vkCmdDispatch (handle, groupCountX, groupCountY, groupCountZ);
		}
		public void BindPipeline (Pipeline pl) {
			pl.Bind (this);
		}
		/// <summary>
		/// bind pipeline and descriptor set with default pipeline layout
		/// </summary>
		/// <param name="pl">pipeline to bind</param>
		/// <param name="ds">descriptor set</param>
		/// <param name="firstset">first set to bind</param>
		public void BindPipeline (Pipeline pl, DescriptorSet ds, uint firstset = 0) {
			pl.Bind (this);
			pl.BindDescriptorSet (this, ds, firstset);
		}
		public void BindDescriptorSet (PipelineLayout pipelineLayout, DescriptorSet descriptorSet, uint firstSet = 0) {
            vkCmdBindDescriptorSets (handle, VkPipelineBindPoint.Graphics, pipelineLayout.handle, firstSet, 1, ref descriptorSet.handle, 0, IntPtr.Zero);
        }
		public void BindDescriptorSet (VkPipelineBindPoint bindPoint, PipelineLayout pipelineLayout, DescriptorSet descriptorSet, uint firstSet = 0) {
			vkCmdBindDescriptorSets (handle, bindPoint, pipelineLayout.handle, firstSet, 1, ref descriptorSet.handle, 0, IntPtr.Zero);
		}
		public void BindVertexBuffer (Buffer vertices, uint binding = 0, ulong offset = 0) {
            vkCmdBindVertexBuffers (handle, binding, 1, ref vertices.handle, ref offset);
        }
        public void BindIndexBuffer (Buffer indices, VkIndexType indexType = VkIndexType.Uint32, ulong offset = 0) {
            vkCmdBindIndexBuffer (handle, indices.handle, offset, indexType);
        }
        public void DrawIndexed (uint indexCount, uint instanceCount = 1, uint firstIndex = 0, int vertexOffset = 0, uint firstInstance = 0) {
            vkCmdDrawIndexed (Handle, indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);
        }
        public void Draw (uint vertexCount, uint instanceCount = 1, uint firstVertex = 0, uint firstInstance = 0) {
            vkCmdDraw (Handle, vertexCount, instanceCount, firstVertex, firstInstance);
        }
		public void PushConstant (PipelineLayout pipelineLayout, VkShaderStageFlags stageFlags, Object data, uint offset = 0) {
			vkCmdPushConstants (handle, pipelineLayout.handle, stageFlags, offset, (uint)Marshal.SizeOf (data), data.Pin ());
			data.Unpin ();
		}
		public void PushConstant (Pipeline pipeline, object obj, int rangeIndex = 0, uint offset = 0) {
			PushConstant (pipeline.Layout, pipeline.Layout.PushConstantRanges[rangeIndex].stageFlags, obj, offset);
		}
		public void PushConstant (PipelineLayout pipelineLayout, VkShaderStageFlags stageFlags, uint size, Object data, uint offset = 0) {
			vkCmdPushConstants (handle, pipelineLayout.handle, stageFlags, offset, size, data.Pin ());
			data.Unpin ();
		}
		public void SetMemoryBarrier (VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask,
			VkAccessFlags srcAccessMask, VkAccessFlags dstAccessMask, VkDependencyFlags dependencyFlags = VkDependencyFlags.ByRegion) {
			VkMemoryBarrier memoryBarrier = VkMemoryBarrier.New ();
			memoryBarrier.srcAccessMask = srcAccessMask;
			memoryBarrier.dstAccessMask = dstAccessMask;
			Vk.vkCmdPipelineBarrier (Handle, srcStageMask, dstStageMask,
				dependencyFlags, 1, ref memoryBarrier, 0, IntPtr.Zero, 0, IntPtr.Zero);
		}
		public void BeginRegion (string name, float r = 1f, float g = 0.1f, float b=0.1f, float a = 1f) {
			if (!Device.debugMarkersEnabled)
				return;
			VkDebugMarkerMarkerInfoEXT info = VkDebugMarkerMarkerInfoEXT.New();
			info.pMarkerName = name.Pin ();
			unsafe {
				info.color[0] = r;
				info.color[1] = g;
				info.color[2] = b;
				info.color[3] = a;
			}
			vkCmdDebugMarkerBeginEXT (Handle, ref info);
			name.Unpin ();
		}
		public void InsertDebugMarker (string name, float r = 1f, float g = 0.1f, float b=0.1f, float a = 1f) {
			if (!Device.debugMarkersEnabled)
				return;
			VkDebugMarkerMarkerInfoEXT info = VkDebugMarkerMarkerInfoEXT.New();
			info.pMarkerName = name.Pin ();
			unsafe {
				info.color[0] = r;
				info.color[1] = g;
				info.color[2] = b;
				info.color[3] = a;
			}
			vkCmdDebugMarkerInsertEXT (Handle, ref info);
			name.Unpin ();
		}
		public void EndRegion () {
			if (Device.debugMarkersEnabled)
				vkCmdDebugMarkerEndEXT (Handle);
		}

		public void Free () {
            pool.FreeCommandBuffers (this);
        }
    }
}
