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
using VK;
using static VK.Vk;

namespace VKE {
    public class CommandBuffer {
        CommandPool pool;
        VkCommandBuffer handle;

        public VkCommandBuffer Handle => handle;
		public Device Device => pool?.dev;//this help

        internal CommandBuffer (VkDevice _dev, CommandPool _pool, VkCommandBuffer _buff)
        {
            pool = _pool;
            handle = _buff;
        }
		//TODO:unpin all pinned obj
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

			handle.Unpin ();
			signal.Unpin ();
			wait.Unpin ();

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
        public void SetViewport (float width, float height, float minDepth = 0.0f, float maxDepth = 1.0f) {
            VkViewport viewport = new VkViewport {
                height = height,
                width = width,
                minDepth = 0.0f,
                maxDepth = 1.0f,
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
        public void BindPipeline (Pipeline pipeline) {
            vkCmdBindPipeline (handle, VkPipelineBindPoint.Graphics, pipeline.handle);
        }
        public void BindDescriptorSet (PipelineLayout pipelineLayout, DescriptorSet descriptorSet, uint firstSet = 0) {
            vkCmdBindDescriptorSets (handle, VkPipelineBindPoint.Graphics, pipelineLayout.handle, firstSet, 1, ref descriptorSet.handle, 0, IntPtr.Zero);
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

#if DEBUG && DEBUG_MARKER
		public void BeginRegion (string name, float r = 1f, float g = 0.1f, float b=0.1f, float a = 1f) {
			VkDebugMarkerMarkerInfoEXT info = VkDebugMarkerMarkerInfoEXT.New;
			info.pMarkerName = name.Pin ();
			info.color_0 = r;
			info.color_1 = g;
			info.color_2 = b;
			vkCmdDebugMarkerBeginEXT (Handle, ref info);
			name.Unpin ();
		}
		public void InsertDebugMarker (string name, float r = 1f, float g = 0.1f, float b=0.1f, float a = 1f) {
			VkDebugMarkerMarkerInfoEXT info = VkDebugMarkerMarkerInfoEXT.New;
			info.pMarkerName = name.Pin ();
			info.color_0 = r;
			info.color_1 = g;
			info.color_2 = b;
			vkCmdDebugMarkerInsertEXT (Handle, ref info);
			name.Unpin ();
		}
		public void EndRegion () {
			vkCmdDebugMarkerEndEXT (Handle);
		}
#endif

		public void Free () {
            pool.FreeCommandBuffers (this);
        }
    }
}
