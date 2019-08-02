//
// CommandBuffer.cs
//
// Author:
//       Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// Copyright (c) 2019 jp
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Runtime.InteropServices;
using VK;

using static VK.Vk;

namespace CVKL {
	/// <summary>
	/// Command buffer are not derived from activable, because their state is retained by the pool which create them.
	/// </summary>
    public class CommandBuffer {
		public enum States { Init, Record, Pending, Invalid };

        CommandPool pool;
        VkCommandBuffer handle;
		//States state;

        public VkCommandBuffer Handle => handle;
		public Device Device => pool?.Dev;//this help
		//public States State => state;

        internal CommandBuffer (VkDevice _dev, CommandPool _pool, VkCommandBuffer _buff)
        {
            pool = _pool;
            handle = _buff;
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
        public void BindPipeline (Pipeline pipeline, VkPipelineBindPoint bindPoint = VkPipelineBindPoint.Graphics) {
            vkCmdBindPipeline (handle, bindPoint, pipeline.Handle);
        }
		public void Dispatch (uint groupCountX, uint groupCountY = 1, uint groupCountZ = 1) {
			vkCmdDispatch (handle, groupCountX, groupCountY, groupCountZ);
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
        public void DrawIndexed (uint indexCount, uint instanceCount = 1, uint firstIndex = 0, int vertexOffset = 0, uint firstInstance = 1) {
            vkCmdDrawIndexed (Handle, indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);
        }
        public void Draw (uint vertexCount, uint instanceCount = 1, uint firstVertex = 0, uint firstInstance = 0) {
            vkCmdDraw (Handle, vertexCount, instanceCount, firstVertex, firstInstance);
        }
		public void PushConstant (PipelineLayout pipelineLayout, VkShaderStageFlags stageFlags, Object data, uint offset = 0) {
			vkCmdPushConstants (handle, pipelineLayout.handle, stageFlags, offset, (uint)Marshal.SizeOf (data), data.Pin ());
			data.Unpin ();
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
