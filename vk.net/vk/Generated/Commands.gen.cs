// This file is generated.

using System;
using System.Runtime.InteropServices;

namespace Vulkan
{
    public static unsafe partial class VulkanNative
    {
        private static IntPtr vkAcquireImageANDROID_ptr;

        private static IntPtr vkAcquireNextImage2KHX_ptr;        

        ///<remarks>Success codes:VK_SUCCESS, VK_TIMEOUT, VK_NOT_READY, VK_SUBOPTIMAL_KHR. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST, VK_ERROR_OUT_OF_DATE_KHR, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkAcquireNextImage2KHX(VkDevice device, ref VkAcquireNextImageInfoKHX pAcquireInfo, ref uint pImageIndex)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS, VK_TIMEOUT, VK_NOT_READY, VK_SUBOPTIMAL_KHR. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST, VK_ERROR_OUT_OF_DATE_KHR, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkAcquireNextImage2KHX(VkDevice device, ref VkAcquireNextImageInfoKHX pAcquireInfo, IntPtr pImageIndex)
        {
            throw new NotImplementedException();
        }
        
        ///<remarks>Success codes:VK_SUCCESS, VK_TIMEOUT, VK_NOT_READY, VK_SUBOPTIMAL_KHR. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST, VK_ERROR_OUT_OF_DATE_KHR, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkAcquireNextImage2KHX(VkDevice device, IntPtr pAcquireInfo, ref uint pImageIndex)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS, VK_TIMEOUT, VK_NOT_READY, VK_SUBOPTIMAL_KHR. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST, VK_ERROR_OUT_OF_DATE_KHR, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkAcquireNextImage2KHX(VkDevice device, IntPtr pAcquireInfo, IntPtr pImageIndex)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkAcquireNextImageKHR_ptr;

        ///<remarks>Success codes:VK_SUCCESS, VK_TIMEOUT, VK_NOT_READY, VK_SUBOPTIMAL_KHR. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST, VK_ERROR_OUT_OF_DATE_KHR, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkAcquireNextImageKHR(VkDevice device, VkSwapchainKHR swapchain, ulong timeout, VkSemaphore semaphore, VkFence fence, ref uint pImageIndex)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS, VK_TIMEOUT, VK_NOT_READY, VK_SUBOPTIMAL_KHR. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST, VK_ERROR_OUT_OF_DATE_KHR, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkAcquireNextImageKHR(VkDevice device, VkSwapchainKHR swapchain, ulong timeout, VkSemaphore semaphore, VkFence fence, IntPtr pImageIndex)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkAcquireXlibDisplayEXT_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_INITIALIZATION_FAILED</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkAcquireXlibDisplayEXT(VkPhysicalDevice physicalDevice, ref Xlib.Display dpy, VkDisplayKHR display)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_INITIALIZATION_FAILED</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkAcquireXlibDisplayEXT(VkPhysicalDevice physicalDevice, IntPtr dpy, VkDisplayKHR display)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkAllocateCommandBuffers_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkAllocateCommandBuffers(VkDevice device, ref VkCommandBufferAllocateInfo pAllocateInfo, out VkCommandBuffer pCommandBuffers)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkAllocateDescriptorSets_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_FRAGMENTED_POOL, VK_ERROR_OUT_OF_POOL_MEMORY_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkAllocateDescriptorSets(VkDevice device, ref VkDescriptorSetAllocateInfo pAllocateInfo, out VkDescriptorSet pDescriptorSets)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_FRAGMENTED_POOL, VK_ERROR_OUT_OF_POOL_MEMORY_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkAllocateDescriptorSets(VkDevice device, IntPtr pAllocateInfo, out VkDescriptorSet pDescriptorSets)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkAllocateMemory_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_INVALID_EXTERNAL_HANDLE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkAllocateMemory(VkDevice device, ref VkMemoryAllocateInfo pAllocateInfo, ref VkAllocationCallbacks pAllocator, out VkDeviceMemory pMemory)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_INVALID_EXTERNAL_HANDLE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkAllocateMemory(VkDevice device, ref VkMemoryAllocateInfo pAllocateInfo, IntPtr pAllocator, out VkDeviceMemory pMemory)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_INVALID_EXTERNAL_HANDLE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkAllocateMemory(VkDevice device, IntPtr pAllocateInfo, ref VkAllocationCallbacks pAllocator, out VkDeviceMemory pMemory)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_INVALID_EXTERNAL_HANDLE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkAllocateMemory(VkDevice device, IntPtr pAllocateInfo, IntPtr? pAllocator, out VkDeviceMemory pMemory)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkBeginCommandBuffer_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkBeginCommandBuffer(VkCommandBuffer commandBuffer, ref VkCommandBufferBeginInfo pBeginInfo)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkBeginCommandBuffer(VkCommandBuffer commandBuffer, IntPtr pBeginInfo)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkBindBufferMemory_ptr;
        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkBindBufferMemory(VkDevice device, VkBuffer buffer, VkDeviceMemory memory, ulong memoryOffset)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkBindBufferMemory2KHR_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkBindBufferMemory2KHR(VkDevice device, uint bindInfoCount, ref VkBindBufferMemoryInfoKHR pBindInfos)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkBindBufferMemory2KHR(VkDevice device, uint bindInfoCount, IntPtr pBindInfos)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkBindBufferMemory2KHR(VkDevice device, uint bindInfoCount, VkBindBufferMemoryInfoKHR[] pBindInfos)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkBindImageMemory_ptr;
        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkBindImageMemory(VkDevice device, VkImage image, VkDeviceMemory memory, ulong memoryOffset)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkBindImageMemory2KHR_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkBindImageMemory2KHR(VkDevice device, uint bindInfoCount, ref VkBindImageMemoryInfoKHR pBindInfos)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkBindImageMemory2KHR(VkDevice device, uint bindInfoCount, IntPtr pBindInfos)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkBindImageMemory2KHR(VkDevice device, uint bindInfoCount, VkBindImageMemoryInfoKHR[] pBindInfos)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdBeginQuery_ptr;
        [Generator.CalliRewrite]
        public static void vkCmdBeginQuery(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint query, VkQueryControlFlags flags)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdBeginRenderPass_ptr;

        [Generator.CalliRewrite]
        public static void vkCmdBeginRenderPass(VkCommandBuffer commandBuffer, ref VkRenderPassBeginInfo pRenderPassBegin, VkSubpassContents contents)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdBeginRenderPass(VkCommandBuffer commandBuffer, IntPtr pRenderPassBegin, VkSubpassContents contents)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdBindDescriptorSets_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdBindDescriptorSets(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipelineLayout layout, uint firstSet, uint descriptorSetCount, ref VkDescriptorSet pDescriptorSets, uint dynamicOffsetCount, ref uint pDynamicOffsets)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdBindDescriptorSets(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipelineLayout layout, uint firstSet, uint descriptorSetCount, ref VkDescriptorSet pDescriptorSets, uint dynamicOffsetCount, IntPtr pDynamicOffsets)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdBindDescriptorSets(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipelineLayout layout, uint firstSet, uint descriptorSetCount, IntPtr pDescriptorSets, uint dynamicOffsetCount, ref uint pDynamicOffsets)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdBindDescriptorSets(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipelineLayout layout, uint firstSet, uint descriptorSetCount, IntPtr pDescriptorSets, uint dynamicOffsetCount, IntPtr pDynamicOffsets)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdBindIndexBuffer_ptr;
        [Generator.CalliRewrite]
        public static unsafe void vkCmdBindIndexBuffer(VkCommandBuffer commandBuffer, VkBuffer buffer, ulong offset, VkIndexType indexType)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdBindPipeline_ptr;
        [Generator.CalliRewrite]
        public static unsafe void vkCmdBindPipeline(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipeline pipeline)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdBindVertexBuffers_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdBindVertexBuffers(VkCommandBuffer commandBuffer, uint firstBinding, uint bindingCount, ref VkBuffer pBuffers, ref ulong pOffsets)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdBindVertexBuffers(VkCommandBuffer commandBuffer, uint firstBinding, uint bindingCount, ref VkBuffer pBuffers, IntPtr pOffsets)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdBindVertexBuffers(VkCommandBuffer commandBuffer, uint firstBinding, uint bindingCount, IntPtr pBuffers, ref ulong pOffsets)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdBindVertexBuffers(VkCommandBuffer commandBuffer, uint firstBinding, uint bindingCount, IntPtr pBuffers, IntPtr pOffsets)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdBlitImage_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdBlitImage(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkImage dstImage, VkImageLayout dstImageLayout, uint regionCount, ref VkImageBlit pRegions, VkFilter filter)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdBlitImage(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkImage dstImage, VkImageLayout dstImageLayout, uint regionCount, IntPtr pRegions, VkFilter filter)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdClearAttachments_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdClearAttachments(VkCommandBuffer commandBuffer, uint attachmentCount, ref VkClearAttachment pAttachments, uint rectCount, ref VkClearRect pRects)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdClearAttachments(VkCommandBuffer commandBuffer, uint attachmentCount, ref VkClearAttachment pAttachments, uint rectCount, IntPtr pRects)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdClearAttachments(VkCommandBuffer commandBuffer, uint attachmentCount, IntPtr pAttachments, uint rectCount, ref VkClearRect pRects)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdClearAttachments(VkCommandBuffer commandBuffer, uint attachmentCount, IntPtr pAttachments, uint rectCount, IntPtr pRects)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdClearColorImage_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdClearColorImage(VkCommandBuffer commandBuffer, VkImage image, VkImageLayout imageLayout, ref VkClearColorValue pColor, uint rangeCount, ref VkImageSubresourceRange pRanges)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdClearColorImage(VkCommandBuffer commandBuffer, VkImage image, VkImageLayout imageLayout, ref VkClearColorValue pColor, uint rangeCount, IntPtr pRanges)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdClearColorImage(VkCommandBuffer commandBuffer, VkImage image, VkImageLayout imageLayout, IntPtr pColor, uint rangeCount, ref VkImageSubresourceRange pRanges)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdClearColorImage(VkCommandBuffer commandBuffer, VkImage image, VkImageLayout imageLayout, IntPtr pColor, uint rangeCount, IntPtr pRanges)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdClearDepthStencilImage_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdClearDepthStencilImage(VkCommandBuffer commandBuffer, VkImage image, VkImageLayout imageLayout, ref VkClearDepthStencilValue pDepthStencil, uint rangeCount, ref VkImageSubresourceRange pRanges)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdClearDepthStencilImage(VkCommandBuffer commandBuffer, VkImage image, VkImageLayout imageLayout, ref VkClearDepthStencilValue pDepthStencil, uint rangeCount, IntPtr pRanges)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdClearDepthStencilImage(VkCommandBuffer commandBuffer, VkImage image, VkImageLayout imageLayout, IntPtr pDepthStencil, uint rangeCount, ref VkImageSubresourceRange pRanges)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdClearDepthStencilImage(VkCommandBuffer commandBuffer, VkImage image, VkImageLayout imageLayout, IntPtr pDepthStencil, uint rangeCount, IntPtr pRanges)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdCopyBuffer_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdCopyBuffer(VkCommandBuffer commandBuffer, VkBuffer srcBuffer, VkBuffer dstBuffer, uint regionCount, ref VkBufferCopy pRegions)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdCopyBuffer(VkCommandBuffer commandBuffer, VkBuffer srcBuffer, VkBuffer dstBuffer, uint regionCount, IntPtr pRegions)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdCopyBufferToImage_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdCopyBufferToImage(VkCommandBuffer commandBuffer, VkBuffer srcBuffer, VkImage dstImage, VkImageLayout dstImageLayout, uint regionCount, ref VkBufferImageCopy pRegions)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdCopyBufferToImage(VkCommandBuffer commandBuffer, VkBuffer srcBuffer, VkImage dstImage, VkImageLayout dstImageLayout, uint regionCount, IntPtr pRegions)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdCopyImage_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdCopyImage(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkImage dstImage, VkImageLayout dstImageLayout, uint regionCount, ref VkImageCopy pRegions)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdCopyImage(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkImage dstImage, VkImageLayout dstImageLayout, uint regionCount, IntPtr pRegions)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdCopyImageToBuffer_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdCopyImageToBuffer(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkBuffer dstBuffer, uint regionCount, ref VkBufferImageCopy pRegions)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdCopyImageToBuffer(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkBuffer dstBuffer, uint regionCount, IntPtr pRegions)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdCopyQueryPoolResults_ptr;

        private static IntPtr vkCmdDebugMarkerBeginEXT_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdDebugMarkerBeginEXT(VkCommandBuffer commandBuffer, ref VkDebugMarkerMarkerInfoEXT pMarkerInfo)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static void vkCmdDebugMarkerBeginEXT(VkCommandBuffer commandBuffer, IntPtr pMarkerInfo)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdDebugMarkerEndEXT_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdDebugMarkerEndEXT(VkCommandBuffer commandBuffer)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdDebugMarkerInsertEXT_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdDebugMarkerInsertEXT(VkCommandBuffer commandBuffer, ref VkDebugMarkerMarkerInfoEXT pMarkerInfo)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdDebugMarkerInsertEXT(VkCommandBuffer commandBuffer, IntPtr pMarkerInfo)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdDispatch_ptr;

        private static IntPtr vkCmdDispatchBaseKHX_ptr;

        private static IntPtr vkCmdDispatchIndirect_ptr;

        private static IntPtr vkCmdDraw_ptr;

		[Generator.CalliRewrite]
        public static unsafe void vkCmdDraw(VkCommandBuffer commandBuffer, uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdDrawIndexed_ptr;
        [Generator.CalliRewrite]
        public static unsafe void vkCmdDrawIndexed(VkCommandBuffer commandBuffer, uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdDrawIndexedIndirect_ptr;

        private static IntPtr vkCmdDrawIndexedIndirectCountAMD_ptr;

        private static IntPtr vkCmdDrawIndirect_ptr;

        private static IntPtr vkCmdDrawIndirectCountAMD_ptr;

        private static IntPtr vkCmdEndQuery_ptr;
        [Generator.CalliRewrite]
        public static unsafe void vkCmdEndQuery(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint query)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdEndRenderPass_ptr;
        [Generator.CalliRewrite]
        public static unsafe void vkCmdEndRenderPass(VkCommandBuffer commandBuffer)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdExecuteCommands_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdExecuteCommands(VkCommandBuffer commandBuffer, uint commandBufferCount, ref VkCommandBuffer pCommandBuffers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdExecuteCommands(VkCommandBuffer commandBuffer, uint commandBufferCount, IntPtr pCommandBuffers)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdFillBuffer_ptr;

        private static IntPtr vkCmdNextSubpass_ptr;

        private static IntPtr vkCmdPipelineBarrier_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdPipelineBarrier(VkCommandBuffer commandBuffer, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, VkDependencyFlags dependencyFlags, uint memoryBarrierCount, ref VkMemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, ref VkBufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, ref VkImageMemoryBarrier pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdPipelineBarrier(VkCommandBuffer commandBuffer, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, VkDependencyFlags dependencyFlags, uint memoryBarrierCount, ref VkMemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, ref VkBufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, IntPtr pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdPipelineBarrier(VkCommandBuffer commandBuffer, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, VkDependencyFlags dependencyFlags, uint memoryBarrierCount, ref VkMemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, IntPtr pBufferMemoryBarriers, uint imageMemoryBarrierCount, ref VkImageMemoryBarrier pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdPipelineBarrier(VkCommandBuffer commandBuffer, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, VkDependencyFlags dependencyFlags, uint memoryBarrierCount, ref VkMemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, IntPtr pBufferMemoryBarriers, uint imageMemoryBarrierCount, IntPtr pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdPipelineBarrier(VkCommandBuffer commandBuffer, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, VkDependencyFlags dependencyFlags, uint memoryBarrierCount, IntPtr pMemoryBarriers, uint bufferMemoryBarrierCount, ref VkBufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, ref VkImageMemoryBarrier pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdPipelineBarrier(VkCommandBuffer commandBuffer, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, VkDependencyFlags dependencyFlags, uint memoryBarrierCount, IntPtr pMemoryBarriers, uint bufferMemoryBarrierCount, ref VkBufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, IntPtr pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdPipelineBarrier(VkCommandBuffer commandBuffer, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, VkDependencyFlags dependencyFlags, uint memoryBarrierCount, IntPtr pMemoryBarriers, uint bufferMemoryBarrierCount, IntPtr pBufferMemoryBarriers, uint imageMemoryBarrierCount, ref VkImageMemoryBarrier pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdPipelineBarrier(VkCommandBuffer commandBuffer, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, VkDependencyFlags dependencyFlags, uint memoryBarrierCount, IntPtr pMemoryBarriers, uint bufferMemoryBarrierCount, IntPtr pBufferMemoryBarriers, uint imageMemoryBarrierCount, IntPtr pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdProcessCommandsNVX_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdProcessCommandsNVX(VkCommandBuffer commandBuffer, ref VkCmdProcessCommandsInfoNVX pProcessCommandsInfo)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdProcessCommandsNVX(VkCommandBuffer commandBuffer, IntPtr pProcessCommandsInfo)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdPushConstants_ptr;

		[Generator.CalliRewrite]
		public static unsafe void vkCmdPushConstants (VkCommandBuffer commandBuffer, VkPipelineLayout layout, VkShaderStageFlags stageFlags, uint offset, uint size, IntPtr pValues) {
			throw new NotImplementedException ();
		}

		private static IntPtr vkCmdPushDescriptorSetKHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdPushDescriptorSetKHR(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipelineLayout layout, uint set, uint descriptorWriteCount, ref VkWriteDescriptorSet pDescriptorWrites)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdPushDescriptorSetKHR(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipelineLayout layout, uint set, uint descriptorWriteCount, IntPtr pDescriptorWrites)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdPushDescriptorSetWithTemplateKHR_ptr;

        private static IntPtr vkCmdReserveSpaceForCommandsNVX_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdReserveSpaceForCommandsNVX(VkCommandBuffer commandBuffer, ref VkCmdReserveSpaceForCommandsInfoNVX pReserveSpaceInfo)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdReserveSpaceForCommandsNVX(VkCommandBuffer commandBuffer, IntPtr pReserveSpaceInfo)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdResetEvent_ptr;

        private static IntPtr vkCmdResetQueryPool_ptr;

        private static IntPtr vkCmdResolveImage_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdResolveImage(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkImage dstImage, VkImageLayout dstImageLayout, uint regionCount, ref VkImageResolve pRegions)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdResolveImage(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkImage dstImage, VkImageLayout dstImageLayout, uint regionCount, IntPtr pRegions)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdSetBlendConstants_ptr;

        private static IntPtr vkCmdSetDepthBias_ptr;

        private static IntPtr vkCmdSetDepthBounds_ptr;

        private static IntPtr vkCmdSetDeviceMaskKHX_ptr;

        private static IntPtr vkCmdSetDiscardRectangleEXT_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdSetDiscardRectangleEXT(VkCommandBuffer commandBuffer, uint firstDiscardRectangle, uint discardRectangleCount, ref VkRect2D pDiscardRectangles)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdSetDiscardRectangleEXT(VkCommandBuffer commandBuffer, uint firstDiscardRectangle, uint discardRectangleCount, IntPtr pDiscardRectangles)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdSetEvent_ptr;

        private static IntPtr vkCmdSetLineWidth_ptr;

        private static IntPtr vkCmdSetSampleLocationsEXT_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdSetSampleLocationsEXT(VkCommandBuffer commandBuffer, ref VkSampleLocationsInfoEXT pSampleLocationsInfo)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdSetSampleLocationsEXT(VkCommandBuffer commandBuffer, IntPtr pSampleLocationsInfo)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdSetScissor_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdSetScissor(VkCommandBuffer commandBuffer, uint firstScissor, uint scissorCount, ref VkRect2D pScissors)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdSetScissor(VkCommandBuffer commandBuffer, uint firstScissor, uint scissorCount, IntPtr pScissors)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdSetStencilCompareMask_ptr;

        private static IntPtr vkCmdSetStencilReference_ptr;

        private static IntPtr vkCmdSetStencilWriteMask_ptr;

        private static IntPtr vkCmdSetViewport_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdSetViewport(VkCommandBuffer commandBuffer, uint firstViewport, uint viewportCount, ref VkViewport pViewports)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdSetViewport(VkCommandBuffer commandBuffer, uint firstViewport, uint viewportCount, IntPtr pViewports)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdSetViewportWScalingNV_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdSetViewportWScalingNV(VkCommandBuffer commandBuffer, uint firstViewport, uint viewportCount, ref VkViewportWScalingNV pViewportWScalings)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdSetViewportWScalingNV(VkCommandBuffer commandBuffer, uint firstViewport, uint viewportCount, IntPtr pViewportWScalings)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdUpdateBuffer_ptr;

        private static IntPtr vkCmdWaitEvents_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint eventCount, ref VkEvent pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint memoryBarrierCount, ref VkMemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, ref VkBufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, ref VkImageMemoryBarrier pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint eventCount, ref VkEvent pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint memoryBarrierCount, ref VkMemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, ref VkBufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, IntPtr pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint eventCount, ref VkEvent pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint memoryBarrierCount, ref VkMemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, IntPtr pBufferMemoryBarriers, uint imageMemoryBarrierCount, ref VkImageMemoryBarrier pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint eventCount, ref VkEvent pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint memoryBarrierCount, ref VkMemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, IntPtr pBufferMemoryBarriers, uint imageMemoryBarrierCount, IntPtr pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint eventCount, ref VkEvent pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint memoryBarrierCount, IntPtr pMemoryBarriers, uint bufferMemoryBarrierCount, ref VkBufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, ref VkImageMemoryBarrier pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint eventCount, ref VkEvent pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint memoryBarrierCount, IntPtr pMemoryBarriers, uint bufferMemoryBarrierCount, ref VkBufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, IntPtr pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint eventCount, ref VkEvent pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint memoryBarrierCount, IntPtr pMemoryBarriers, uint bufferMemoryBarrierCount, IntPtr pBufferMemoryBarriers, uint imageMemoryBarrierCount, ref VkImageMemoryBarrier pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint eventCount, ref VkEvent pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint memoryBarrierCount, IntPtr pMemoryBarriers, uint bufferMemoryBarrierCount, IntPtr pBufferMemoryBarriers, uint imageMemoryBarrierCount, IntPtr pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint eventCount, IntPtr pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint memoryBarrierCount, ref VkMemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, ref VkBufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, ref VkImageMemoryBarrier pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint eventCount, IntPtr pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint memoryBarrierCount, ref VkMemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, ref VkBufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, IntPtr pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint eventCount, IntPtr pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint memoryBarrierCount, ref VkMemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, IntPtr pBufferMemoryBarriers, uint imageMemoryBarrierCount, ref VkImageMemoryBarrier pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint eventCount, IntPtr pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint memoryBarrierCount, ref VkMemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, IntPtr pBufferMemoryBarriers, uint imageMemoryBarrierCount, IntPtr pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint eventCount, IntPtr pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint memoryBarrierCount, IntPtr pMemoryBarriers, uint bufferMemoryBarrierCount, ref VkBufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, ref VkImageMemoryBarrier pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint eventCount, IntPtr pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint memoryBarrierCount, IntPtr pMemoryBarriers, uint bufferMemoryBarrierCount, ref VkBufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, IntPtr pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint eventCount, IntPtr pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint memoryBarrierCount, IntPtr pMemoryBarriers, uint bufferMemoryBarrierCount, IntPtr pBufferMemoryBarriers, uint imageMemoryBarrierCount, ref VkImageMemoryBarrier pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint eventCount, IntPtr pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint memoryBarrierCount, IntPtr pMemoryBarriers, uint bufferMemoryBarrierCount, IntPtr pBufferMemoryBarriers, uint imageMemoryBarrierCount, IntPtr pImageMemoryBarriers)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdWriteTimestamp_ptr;

        private static IntPtr vkCreateAndroidSurfaceKHR_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateAndroidSurfaceKHR(VkInstance instance, ref VkAndroidSurfaceCreateInfoKHR pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateAndroidSurfaceKHR(VkInstance instance, ref VkAndroidSurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateAndroidSurfaceKHR(VkInstance instance, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateAndroidSurfaceKHR(VkInstance instance, IntPtr pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateBuffer_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateBuffer(VkDevice device, ref VkBufferCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkBuffer pBuffer)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateBuffer(VkDevice device, ref VkBufferCreateInfo pCreateInfo, IntPtr pAllocator, out VkBuffer pBuffer)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateBuffer(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkBuffer pBuffer)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateBuffer(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkBuffer pBuffer)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateBufferView_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateBufferView(VkDevice device, ref VkBufferViewCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkBufferView pView)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateBufferView(VkDevice device, ref VkBufferViewCreateInfo pCreateInfo, IntPtr pAllocator, out VkBufferView pView)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateBufferView(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkBufferView pView)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateBufferView(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkBufferView pView)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateCommandPool_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateCommandPool(VkDevice device, ref VkCommandPoolCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkCommandPool pCommandPool)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateCommandPool(VkDevice device, ref VkCommandPoolCreateInfo pCreateInfo, IntPtr pAllocator, out VkCommandPool pCommandPool)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateCommandPool(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkCommandPool pCommandPool)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateCommandPool(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkCommandPool pCommandPool)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateComputePipelines_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INVALID_SHADER_NV</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateComputePipelines(VkDevice device, VkPipelineCache pipelineCache, uint createInfoCount, ref VkComputePipelineCreateInfo pCreateInfos, ref VkAllocationCallbacks pAllocator, out VkPipeline pPipelines)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INVALID_SHADER_NV</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateComputePipelines(VkDevice device, VkPipelineCache pipelineCache, uint createInfoCount, ref VkComputePipelineCreateInfo pCreateInfos, IntPtr pAllocator, out VkPipeline pPipelines)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INVALID_SHADER_NV</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateComputePipelines(VkDevice device, VkPipelineCache pipelineCache, uint createInfoCount, IntPtr pCreateInfos, ref VkAllocationCallbacks pAllocator, out VkPipeline pPipelines)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INVALID_SHADER_NV</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateComputePipelines(VkDevice device, VkPipelineCache pipelineCache, uint createInfoCount, IntPtr pCreateInfos, IntPtr pAllocator, out VkPipeline pPipelines)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INVALID_SHADER_NV</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateComputePipelines(VkDevice device, VkPipelineCache pipelineCache, uint createInfoCount, VkComputePipelineCreateInfo[] pCreateInfos, ref VkAllocationCallbacks pAllocator, out VkPipeline pPipelines)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INVALID_SHADER_NV</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateComputePipelines(VkDevice device, VkPipelineCache pipelineCache, uint createInfoCount, VkComputePipelineCreateInfo[] pCreateInfos, IntPtr pAllocator, out VkPipeline pPipelines)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateDebugReportCallbackEXT_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDebugReportCallbackEXT(VkInstance instance, ref VkDebugReportCallbackCreateInfoEXT pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkDebugReportCallbackEXT pCallback)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDebugReportCallbackEXT(VkInstance instance, ref VkDebugReportCallbackCreateInfoEXT pCreateInfo, IntPtr pAllocator, out VkDebugReportCallbackEXT pCallback)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDebugReportCallbackEXT(VkInstance instance, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkDebugReportCallbackEXT pCallback)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDebugReportCallbackEXT(VkInstance instance, IntPtr pCreateInfo, IntPtr pAllocator, out VkDebugReportCallbackEXT pCallback)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateDescriptorPool_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDescriptorPool(VkDevice device, ref VkDescriptorPoolCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkDescriptorPool pDescriptorPool)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDescriptorPool(VkDevice device, ref VkDescriptorPoolCreateInfo pCreateInfo, IntPtr pAllocator, out VkDescriptorPool pDescriptorPool)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDescriptorPool(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkDescriptorPool pDescriptorPool)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDescriptorPool(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkDescriptorPool pDescriptorPool)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateDescriptorSetLayout_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDescriptorSetLayout(VkDevice device, ref VkDescriptorSetLayoutCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkDescriptorSetLayout pSetLayout)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDescriptorSetLayout(VkDevice device, ref VkDescriptorSetLayoutCreateInfo pCreateInfo, IntPtr pAllocator, out VkDescriptorSetLayout pSetLayout)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDescriptorSetLayout(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkDescriptorSetLayout pSetLayout)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDescriptorSetLayout(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkDescriptorSetLayout pSetLayout)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateDescriptorUpdateTemplateKHR_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDescriptorUpdateTemplateKHR(VkDevice device, ref VkDescriptorUpdateTemplateCreateInfoKHR pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkDescriptorUpdateTemplateKHR pDescriptorUpdateTemplate)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDescriptorUpdateTemplateKHR(VkDevice device, ref VkDescriptorUpdateTemplateCreateInfoKHR pCreateInfo, IntPtr pAllocator, out VkDescriptorUpdateTemplateKHR pDescriptorUpdateTemplate)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDescriptorUpdateTemplateKHR(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkDescriptorUpdateTemplateKHR pDescriptorUpdateTemplate)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDescriptorUpdateTemplateKHR(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkDescriptorUpdateTemplateKHR pDescriptorUpdateTemplate)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateDevice_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED, VK_ERROR_EXTENSION_NOT_PRESENT, VK_ERROR_FEATURE_NOT_PRESENT, VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_DEVICE_LOST</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDevice(VkPhysicalDevice physicalDevice, ref VkDeviceCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkDevice pDevice)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED, VK_ERROR_EXTENSION_NOT_PRESENT, VK_ERROR_FEATURE_NOT_PRESENT, VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_DEVICE_LOST</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDevice(VkPhysicalDevice physicalDevice, ref VkDeviceCreateInfo pCreateInfo, IntPtr pAllocator, out VkDevice pDevice)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED, VK_ERROR_EXTENSION_NOT_PRESENT, VK_ERROR_FEATURE_NOT_PRESENT, VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_DEVICE_LOST</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDevice(VkPhysicalDevice physicalDevice, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkDevice pDevice)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED, VK_ERROR_EXTENSION_NOT_PRESENT, VK_ERROR_FEATURE_NOT_PRESENT, VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_DEVICE_LOST</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDevice(VkPhysicalDevice physicalDevice, IntPtr pCreateInfo, IntPtr pAllocator, out VkDevice pDevice)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateDisplayModeKHR_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDisplayModeKHR(VkPhysicalDevice physicalDevice, VkDisplayKHR display, ref VkDisplayModeCreateInfoKHR pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkDisplayModeKHR pMode)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDisplayModeKHR(VkPhysicalDevice physicalDevice, VkDisplayKHR display, ref VkDisplayModeCreateInfoKHR pCreateInfo, IntPtr pAllocator, out VkDisplayModeKHR pMode)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDisplayModeKHR(VkPhysicalDevice physicalDevice, VkDisplayKHR display, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkDisplayModeKHR pMode)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDisplayModeKHR(VkPhysicalDevice physicalDevice, VkDisplayKHR display, IntPtr pCreateInfo, IntPtr pAllocator, out VkDisplayModeKHR pMode)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateDisplayPlaneSurfaceKHR_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDisplayPlaneSurfaceKHR(VkInstance instance, ref VkDisplaySurfaceCreateInfoKHR pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDisplayPlaneSurfaceKHR(VkInstance instance, ref VkDisplaySurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDisplayPlaneSurfaceKHR(VkInstance instance, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateDisplayPlaneSurfaceKHR(VkInstance instance, IntPtr pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateEvent_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateEvent(VkDevice device, ref VkEventCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkEvent pEvent)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateEvent(VkDevice device, ref VkEventCreateInfo pCreateInfo, IntPtr pAllocator, out VkEvent pEvent)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateEvent(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkEvent pEvent)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateEvent(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkEvent pEvent)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateFence_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateFence(VkDevice device, ref VkFenceCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkFence pFence)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateFence(VkDevice device, ref VkFenceCreateInfo pCreateInfo, IntPtr pAllocator, out VkFence pFence)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateFence(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkFence pFence)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateFence(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkFence pFence)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateFramebuffer_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateFramebuffer(VkDevice device, ref VkFramebufferCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkFramebuffer pFramebuffer)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateFramebuffer(VkDevice device, ref VkFramebufferCreateInfo pCreateInfo, IntPtr pAllocator, out VkFramebuffer pFramebuffer)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateFramebuffer(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkFramebuffer pFramebuffer)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateFramebuffer(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkFramebuffer pFramebuffer)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateGraphicsPipelines_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INVALID_SHADER_NV</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateGraphicsPipelines(VkDevice device, VkPipelineCache pipelineCache, uint createInfoCount, ref VkGraphicsPipelineCreateInfo pCreateInfos, ref VkAllocationCallbacks pAllocator, out VkPipeline pPipelines)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INVALID_SHADER_NV</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateGraphicsPipelines(VkDevice device, VkPipelineCache pipelineCache, uint createInfoCount, ref VkGraphicsPipelineCreateInfo pCreateInfos, IntPtr pAllocator, out VkPipeline pPipelines)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INVALID_SHADER_NV</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateGraphicsPipelines(VkDevice device, VkPipelineCache pipelineCache, uint createInfoCount, IntPtr pCreateInfos, ref VkAllocationCallbacks pAllocator, out VkPipeline pPipelines)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INVALID_SHADER_NV</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateGraphicsPipelines(VkDevice device, VkPipelineCache pipelineCache, uint createInfoCount, IntPtr pCreateInfos, IntPtr pAllocator, out VkPipeline pPipelines)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INVALID_SHADER_NV</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateGraphicsPipelines(VkDevice device, VkPipelineCache pipelineCache, uint createInfoCount, VkGraphicsPipelineCreateInfo[] pCreateInfos, ref VkAllocationCallbacks pAllocator, out VkPipeline pPipelines)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INVALID_SHADER_NV</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateGraphicsPipelines(VkDevice device, VkPipelineCache pipelineCache, uint createInfoCount, VkGraphicsPipelineCreateInfo[] pCreateInfos, IntPtr pAllocator, out VkPipeline pPipelines)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateImage_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateImage(VkDevice device, ref VkImageCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkImage pImage)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateImage(VkDevice device, ref VkImageCreateInfo pCreateInfo, IntPtr pAllocator, out VkImage pImage)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateImage(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkImage pImage)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateImage(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkImage pImage)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateImageView_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateImageView(VkDevice device, ref VkImageViewCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkImageView pView)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateImageView(VkDevice device, ref VkImageViewCreateInfo pCreateInfo, IntPtr pAllocator, out VkImageView pView)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateImageView(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkImageView pView)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateImageView(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkImageView pView)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateIndirectCommandsLayoutNVX_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateIndirectCommandsLayoutNVX(VkDevice device, ref VkIndirectCommandsLayoutCreateInfoNVX pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkIndirectCommandsLayoutNVX pIndirectCommandsLayout)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateIndirectCommandsLayoutNVX(VkDevice device, ref VkIndirectCommandsLayoutCreateInfoNVX pCreateInfo, IntPtr pAllocator, out VkIndirectCommandsLayoutNVX pIndirectCommandsLayout)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateIndirectCommandsLayoutNVX(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkIndirectCommandsLayoutNVX pIndirectCommandsLayout)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateIndirectCommandsLayoutNVX(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkIndirectCommandsLayoutNVX pIndirectCommandsLayout)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateInstance_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED, VK_ERROR_LAYER_NOT_PRESENT, VK_ERROR_EXTENSION_NOT_PRESENT, VK_ERROR_INCOMPATIBLE_DRIVER</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateInstance(ref VkInstanceCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkInstance pInstance)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED, VK_ERROR_LAYER_NOT_PRESENT, VK_ERROR_EXTENSION_NOT_PRESENT, VK_ERROR_INCOMPATIBLE_DRIVER</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateInstance(ref VkInstanceCreateInfo pCreateInfo, IntPtr pAllocator, out VkInstance pInstance)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED, VK_ERROR_LAYER_NOT_PRESENT, VK_ERROR_EXTENSION_NOT_PRESENT, VK_ERROR_INCOMPATIBLE_DRIVER</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateInstance(IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkInstance pInstance)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED, VK_ERROR_LAYER_NOT_PRESENT, VK_ERROR_EXTENSION_NOT_PRESENT, VK_ERROR_INCOMPATIBLE_DRIVER</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateInstance(IntPtr pCreateInfo, IntPtr pAllocator, out VkInstance pInstance)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateIOSSurfaceMVK_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateIOSSurfaceMVK(VkInstance instance, ref VkIOSSurfaceCreateInfoMVK pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateIOSSurfaceMVK(VkInstance instance, ref VkIOSSurfaceCreateInfoMVK pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateIOSSurfaceMVK(VkInstance instance, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateIOSSurfaceMVK(VkInstance instance, IntPtr pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateMacOSSurfaceMVK_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateMacOSSurfaceMVK(VkInstance instance, ref VkMacOSSurfaceCreateInfoMVK pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateMacOSSurfaceMVK(VkInstance instance, ref VkMacOSSurfaceCreateInfoMVK pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateMacOSSurfaceMVK(VkInstance instance, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateMacOSSurfaceMVK(VkInstance instance, IntPtr pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateMirSurfaceKHR_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateMirSurfaceKHR(VkInstance instance, ref VkMirSurfaceCreateInfoKHR pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateMirSurfaceKHR(VkInstance instance, ref VkMirSurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateMirSurfaceKHR(VkInstance instance, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateMirSurfaceKHR(VkInstance instance, IntPtr pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateObjectTableNVX_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateObjectTableNVX(VkDevice device, ref VkObjectTableCreateInfoNVX pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkObjectTableNVX pObjectTable)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateObjectTableNVX(VkDevice device, ref VkObjectTableCreateInfoNVX pCreateInfo, IntPtr pAllocator, out VkObjectTableNVX pObjectTable)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateObjectTableNVX(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkObjectTableNVX pObjectTable)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateObjectTableNVX(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkObjectTableNVX pObjectTable)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreatePipelineCache_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreatePipelineCache(VkDevice device, ref VkPipelineCacheCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkPipelineCache pPipelineCache)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreatePipelineCache(VkDevice device, ref VkPipelineCacheCreateInfo pCreateInfo, IntPtr pAllocator, out VkPipelineCache pPipelineCache)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreatePipelineCache(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkPipelineCache pPipelineCache)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreatePipelineCache(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkPipelineCache pPipelineCache)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreatePipelineLayout_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreatePipelineLayout(VkDevice device, ref VkPipelineLayoutCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkPipelineLayout pPipelineLayout)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreatePipelineLayout(VkDevice device, ref VkPipelineLayoutCreateInfo pCreateInfo, IntPtr pAllocator, out VkPipelineLayout pPipelineLayout)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreatePipelineLayout(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkPipelineLayout pPipelineLayout)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreatePipelineLayout(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkPipelineLayout pPipelineLayout)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateQueryPool_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateQueryPool(VkDevice device, ref VkQueryPoolCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkQueryPool pQueryPool)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateQueryPool(VkDevice device, ref VkQueryPoolCreateInfo pCreateInfo, IntPtr pAllocator, out VkQueryPool pQueryPool)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateQueryPool(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkQueryPool pQueryPool)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateQueryPool(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkQueryPool pQueryPool)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateRenderPass_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateRenderPass(VkDevice device, ref VkRenderPassCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkRenderPass pRenderPass)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateRenderPass(VkDevice device, ref VkRenderPassCreateInfo pCreateInfo, IntPtr pAllocator, out VkRenderPass pRenderPass)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateRenderPass(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkRenderPass pRenderPass)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateRenderPass(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkRenderPass pRenderPass)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateSampler_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_TOO_MANY_OBJECTS</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSampler(VkDevice device, ref VkSamplerCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSampler pSampler)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_TOO_MANY_OBJECTS</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSampler(VkDevice device, ref VkSamplerCreateInfo pCreateInfo, IntPtr pAllocator, out VkSampler pSampler)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_TOO_MANY_OBJECTS</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSampler(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSampler pSampler)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_TOO_MANY_OBJECTS</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSampler(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkSampler pSampler)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateSamplerYcbcrConversionKHR_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSamplerYcbcrConversionKHR(VkDevice device, ref VkSamplerYcbcrConversionCreateInfoKHR pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSamplerYcbcrConversionKHR pYcbcrConversion)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSamplerYcbcrConversionKHR(VkDevice device, ref VkSamplerYcbcrConversionCreateInfoKHR pCreateInfo, IntPtr pAllocator, out VkSamplerYcbcrConversionKHR pYcbcrConversion)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSamplerYcbcrConversionKHR(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSamplerYcbcrConversionKHR pYcbcrConversion)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSamplerYcbcrConversionKHR(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkSamplerYcbcrConversionKHR pYcbcrConversion)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateSemaphore_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSemaphore(VkDevice device, ref VkSemaphoreCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSemaphore pSemaphore)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSemaphore(VkDevice device, ref VkSemaphoreCreateInfo pCreateInfo, IntPtr pAllocator, out VkSemaphore pSemaphore)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSemaphore(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSemaphore pSemaphore)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSemaphore(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkSemaphore pSemaphore)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateShaderModule_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INVALID_SHADER_NV</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateShaderModule(VkDevice device, ref VkShaderModuleCreateInfo pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkShaderModule pShaderModule)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INVALID_SHADER_NV</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateShaderModule(VkDevice device, ref VkShaderModuleCreateInfo pCreateInfo, IntPtr pAllocator, out VkShaderModule pShaderModule)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INVALID_SHADER_NV</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateShaderModule(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkShaderModule pShaderModule)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INVALID_SHADER_NV</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateShaderModule(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkShaderModule pShaderModule)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateSharedSwapchainsKHR_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INCOMPATIBLE_DISPLAY_KHR, VK_ERROR_DEVICE_LOST, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSharedSwapchainsKHR(VkDevice device, uint swapchainCount, ref VkSwapchainCreateInfoKHR pCreateInfos, ref VkAllocationCallbacks pAllocator, out VkSwapchainKHR pSwapchains)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INCOMPATIBLE_DISPLAY_KHR, VK_ERROR_DEVICE_LOST, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSharedSwapchainsKHR(VkDevice device, uint swapchainCount, ref VkSwapchainCreateInfoKHR pCreateInfos, IntPtr pAllocator, out VkSwapchainKHR pSwapchains)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INCOMPATIBLE_DISPLAY_KHR, VK_ERROR_DEVICE_LOST, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSharedSwapchainsKHR(VkDevice device, uint swapchainCount, IntPtr pCreateInfos, ref VkAllocationCallbacks pAllocator, out VkSwapchainKHR pSwapchains)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INCOMPATIBLE_DISPLAY_KHR, VK_ERROR_DEVICE_LOST, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSharedSwapchainsKHR(VkDevice device, uint swapchainCount, IntPtr pCreateInfos, IntPtr pAllocator, out VkSwapchainKHR pSwapchains)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INCOMPATIBLE_DISPLAY_KHR, VK_ERROR_DEVICE_LOST, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSharedSwapchainsKHR(VkDevice device, uint swapchainCount, VkSwapchainCreateInfoKHR[] pCreateInfos, ref VkAllocationCallbacks pAllocator, out VkSwapchainKHR pSwapchains)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INCOMPATIBLE_DISPLAY_KHR, VK_ERROR_DEVICE_LOST, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSharedSwapchainsKHR(VkDevice device, uint swapchainCount, VkSwapchainCreateInfoKHR[] pCreateInfos, IntPtr pAllocator, out VkSwapchainKHR pSwapchains)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateSwapchainKHR_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST, VK_ERROR_SURFACE_LOST_KHR, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSwapchainKHR(VkDevice device, ref VkSwapchainCreateInfoKHR pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSwapchainKHR pSwapchain)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST, VK_ERROR_SURFACE_LOST_KHR, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSwapchainKHR(VkDevice device, ref VkSwapchainCreateInfoKHR pCreateInfo, IntPtr pAllocator, out VkSwapchainKHR pSwapchain)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST, VK_ERROR_SURFACE_LOST_KHR, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSwapchainKHR(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSwapchainKHR pSwapchain)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST, VK_ERROR_SURFACE_LOST_KHR, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateSwapchainKHR(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkSwapchainKHR pSwapchain)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateValidationCacheEXT_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateValidationCacheEXT(VkDevice device, ref VkValidationCacheCreateInfoEXT pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkValidationCacheEXT pValidationCache)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateValidationCacheEXT(VkDevice device, ref VkValidationCacheCreateInfoEXT pCreateInfo, IntPtr pAllocator, out VkValidationCacheEXT pValidationCache)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateValidationCacheEXT(VkDevice device, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkValidationCacheEXT pValidationCache)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateValidationCacheEXT(VkDevice device, IntPtr pCreateInfo, IntPtr pAllocator, out VkValidationCacheEXT pValidationCache)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateViSurfaceNN_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateViSurfaceNN(VkInstance instance, ref VkViSurfaceCreateInfoNN pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateViSurfaceNN(VkInstance instance, ref VkViSurfaceCreateInfoNN pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateViSurfaceNN(VkInstance instance, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_NATIVE_WINDOW_IN_USE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateViSurfaceNN(VkInstance instance, IntPtr pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateWaylandSurfaceKHR_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateWaylandSurfaceKHR(VkInstance instance, ref VkWaylandSurfaceCreateInfoKHR pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateWaylandSurfaceKHR(VkInstance instance, ref VkWaylandSurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateWaylandSurfaceKHR(VkInstance instance, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateWaylandSurfaceKHR(VkInstance instance, IntPtr pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateWin32SurfaceKHR_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateWin32SurfaceKHR(VkInstance instance, ref VkWin32SurfaceCreateInfoKHR pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateWin32SurfaceKHR(VkInstance instance, ref VkWin32SurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateWin32SurfaceKHR(VkInstance instance, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateWin32SurfaceKHR(VkInstance instance, IntPtr pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateXcbSurfaceKHR_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateXcbSurfaceKHR(VkInstance instance, ref VkXcbSurfaceCreateInfoKHR pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateXcbSurfaceKHR(VkInstance instance, ref VkXcbSurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateXcbSurfaceKHR(VkInstance instance, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateXcbSurfaceKHR(VkInstance instance, IntPtr pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCreateXlibSurfaceKHR_ptr;









        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateXlibSurfaceKHR(VkInstance instance, ref VkXlibSurfaceCreateInfoKHR pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateXlibSurfaceKHR(VkInstance instance, ref VkXlibSurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateXlibSurfaceKHR(VkInstance instance, IntPtr pCreateInfo, ref VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkCreateXlibSurfaceKHR(VkInstance instance, IntPtr pCreateInfo, IntPtr pAllocator, out VkSurfaceKHR pSurface)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDebugMarkerSetObjectNameEXT_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkDebugMarkerSetObjectNameEXT(VkDevice device, ref VkDebugMarkerObjectNameInfoEXT pNameInfo)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkDebugMarkerSetObjectNameEXT(VkDevice device, IntPtr pNameInfo)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDebugMarkerSetObjectTagEXT_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkDebugMarkerSetObjectTagEXT(VkDevice device, ref VkDebugMarkerObjectTagInfoEXT pTagInfo)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkDebugMarkerSetObjectTagEXT(VkDevice device, IntPtr pTagInfo)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDebugReportMessageEXT_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDebugReportMessageEXT(VkInstance instance, VkDebugReportFlagsEXT flags, VkDebugReportObjectTypeEXT objectType, ulong @object, UIntPtr location, int messageCode, string pLayerPrefix, string pMessage)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyBuffer_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyBuffer(VkDevice device, VkBuffer buffer, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyBuffer(VkDevice device, VkBuffer buffer, IntPtr? pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyBufferView_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyBufferView(VkDevice device, VkBufferView bufferView, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyBufferView(VkDevice device, VkBufferView bufferView, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyCommandPool_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyCommandPool(VkDevice device, VkCommandPool commandPool, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyCommandPool(VkDevice device, VkCommandPool commandPool, IntPtr? pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyDebugReportCallbackEXT_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyDebugReportCallbackEXT(VkInstance instance, VkDebugReportCallbackEXT callback, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyDebugReportCallbackEXT(VkInstance instance, VkDebugReportCallbackEXT callback, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyDescriptorPool_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyDescriptorPool(VkDevice device, VkDescriptorPool descriptorPool, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyDescriptorPool(VkDevice device, VkDescriptorPool descriptorPool, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyDescriptorSetLayout_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout descriptorSetLayout, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout descriptorSetLayout, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyDescriptorUpdateTemplateKHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyDescriptorUpdateTemplateKHR(VkDevice device, VkDescriptorUpdateTemplateKHR descriptorUpdateTemplate, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyDescriptorUpdateTemplateKHR(VkDevice device, VkDescriptorUpdateTemplateKHR descriptorUpdateTemplate, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyDevice_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyDevice(VkDevice device, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyDevice(VkDevice device, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyEvent_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyEvent(VkDevice device, VkEvent @event, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyEvent(VkDevice device, VkEvent @event, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyFence_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyFence(VkDevice device, VkFence fence, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyFence(VkDevice device, VkFence fence, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyFramebuffer_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyFramebuffer(VkDevice device, VkFramebuffer framebuffer, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyFramebuffer(VkDevice device, VkFramebuffer framebuffer, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyImage_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyImage(VkDevice device, VkImage image, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyImage(VkDevice device, VkImage image, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyImageView_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyImageView(VkDevice device, VkImageView imageView, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyImageView(VkDevice device, VkImageView imageView, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyIndirectCommandsLayoutNVX_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyIndirectCommandsLayoutNVX(VkDevice device, VkIndirectCommandsLayoutNVX indirectCommandsLayout, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyIndirectCommandsLayoutNVX(VkDevice device, VkIndirectCommandsLayoutNVX indirectCommandsLayout, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyInstance_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyInstance(VkInstance instance, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyInstance(VkInstance instance, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyObjectTableNVX_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyObjectTableNVX(VkDevice device, VkObjectTableNVX objectTable, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyObjectTableNVX(VkDevice device, VkObjectTableNVX objectTable, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyPipeline_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyPipeline(VkDevice device, VkPipeline pipeline, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyPipeline(VkDevice device, VkPipeline pipeline, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyPipelineCache_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyPipelineCache(VkDevice device, VkPipelineCache pipelineCache, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyPipelineCache(VkDevice device, VkPipelineCache pipelineCache, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyPipelineLayout_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyPipelineLayout(VkDevice device, VkPipelineLayout pipelineLayout, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyPipelineLayout(VkDevice device, VkPipelineLayout pipelineLayout, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyQueryPool_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyQueryPool(VkDevice device, VkQueryPool queryPool, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyQueryPool(VkDevice device, VkQueryPool queryPool, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyRenderPass_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyRenderPass(VkDevice device, VkRenderPass renderPass, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyRenderPass(VkDevice device, VkRenderPass renderPass, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroySampler_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroySampler(VkDevice device, VkSampler sampler, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroySampler(VkDevice device, VkSampler sampler, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroySamplerYcbcrConversionKHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroySamplerYcbcrConversionKHR(VkDevice device, VkSamplerYcbcrConversionKHR ycbcrConversion, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroySamplerYcbcrConversionKHR(VkDevice device, VkSamplerYcbcrConversionKHR ycbcrConversion, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroySemaphore_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroySemaphore(VkDevice device, VkSemaphore semaphore, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroySemaphore(VkDevice device, VkSemaphore semaphore, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyShaderModule_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyShaderModule(VkDevice device, VkShaderModule shaderModule, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyShaderModule(VkDevice device, VkShaderModule shaderModule, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroySurfaceKHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroySurfaceKHR(VkInstance instance, VkSurfaceKHR surface, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroySurfaceKHR(VkInstance instance, VkSurfaceKHR surface, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroySwapchainKHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroySwapchainKHR(VkDevice device, VkSwapchainKHR swapchain, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroySwapchainKHR(VkDevice device, VkSwapchainKHR swapchain, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDestroyValidationCacheEXT_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyValidationCacheEXT(VkDevice device, VkValidationCacheEXT validationCache, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkDestroyValidationCacheEXT(VkDevice device, VkValidationCacheEXT validationCache, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDeviceWaitIdle_ptr;
        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkDeviceWaitIdle(VkDevice device)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkDisplayPowerControlEXT_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkDisplayPowerControlEXT(VkDevice device, VkDisplayKHR display, ref VkDisplayPowerInfoEXT pDisplayPowerInfo)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkDisplayPowerControlEXT(VkDevice device, VkDisplayKHR display, IntPtr pDisplayPowerInfo)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkEndCommandBuffer_ptr;
        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEndCommandBuffer(VkCommandBuffer commandBuffer)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkEnumerateDeviceExtensionProperties_ptr;
        
        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_LAYER_NOT_PRESENT</remarks>
        [Generator.CalliRewrite]
        public static VkResult vkEnumerateDeviceExtensionProperties(VkPhysicalDevice physicalDevice, IntPtr pLayerName, out uint pPropertyCount, IntPtr? pProperties)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_LAYER_NOT_PRESENT</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumerateDeviceExtensionProperties(VkPhysicalDevice physicalDevice, byte* pLayerName, IntPtr pPropertyCount, ref VkExtensionProperties pProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkEnumerateDeviceLayerProperties_ptr;

        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumerateDeviceLayerProperties(VkPhysicalDevice physicalDevice, ref uint pPropertyCount, ref VkLayerProperties pProperties)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumerateDeviceLayerProperties(VkPhysicalDevice physicalDevice, ref uint pPropertyCount, IntPtr pProperties)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumerateDeviceLayerProperties(VkPhysicalDevice physicalDevice, IntPtr pPropertyCount, ref VkLayerProperties pProperties)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumerateDeviceLayerProperties(VkPhysicalDevice physicalDevice, IntPtr pPropertyCount, IntPtr pProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkEnumerateInstanceExtensionProperties_ptr;













        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_LAYER_NOT_PRESENT</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumerateInstanceExtensionProperties(string pLayerName, ref uint pPropertyCount, ref VkExtensionProperties pProperties)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_LAYER_NOT_PRESENT</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumerateInstanceExtensionProperties(string pLayerName, ref uint pPropertyCount, IntPtr pProperties)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_LAYER_NOT_PRESENT</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumerateInstanceExtensionProperties(string pLayerName, IntPtr pPropertyCount, ref VkExtensionProperties pProperties)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_LAYER_NOT_PRESENT</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumerateInstanceExtensionProperties(string pLayerName, IntPtr pPropertyCount, IntPtr pProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkEnumerateInstanceLayerProperties_ptr;




        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumerateInstanceLayerProperties(ref uint pPropertyCount, ref VkLayerProperties pProperties)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumerateInstanceLayerProperties(ref uint pPropertyCount, IntPtr pProperties)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumerateInstanceLayerProperties(IntPtr pPropertyCount, ref VkLayerProperties pProperties)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumerateInstanceLayerProperties(IntPtr pPropertyCount, IntPtr pProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkEnumeratePhysicalDeviceGroupsKHX_ptr;




        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumeratePhysicalDeviceGroupsKHX(VkInstance instance, ref uint pPhysicalDeviceGroupCount, ref VkPhysicalDeviceGroupPropertiesKHX pPhysicalDeviceGroupProperties)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumeratePhysicalDeviceGroupsKHX(VkInstance instance, ref uint pPhysicalDeviceGroupCount, IntPtr pPhysicalDeviceGroupProperties)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumeratePhysicalDeviceGroupsKHX(VkInstance instance, IntPtr pPhysicalDeviceGroupCount, ref VkPhysicalDeviceGroupPropertiesKHX pPhysicalDeviceGroupProperties)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumeratePhysicalDeviceGroupsKHX(VkInstance instance, IntPtr pPhysicalDeviceGroupCount, IntPtr pPhysicalDeviceGroupProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkEnumeratePhysicalDevices_ptr;




        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumeratePhysicalDevices(VkInstance instance, ref uint pPhysicalDeviceCount, ref VkPhysicalDevice pPhysicalDevices)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumeratePhysicalDevices(VkInstance instance, ref uint pPhysicalDeviceCount, IntPtr pPhysicalDevices)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumeratePhysicalDevices(VkInstance instance, IntPtr pPhysicalDeviceCount, ref VkPhysicalDevice pPhysicalDevices)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_INITIALIZATION_FAILED</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkEnumeratePhysicalDevices(VkInstance instance, IntPtr pPhysicalDeviceCount, IntPtr pPhysicalDevices)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkFlushMappedMemoryRanges_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkFlushMappedMemoryRanges(VkDevice device, uint memoryRangeCount, ref VkMappedMemoryRange pMemoryRanges)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkFlushMappedMemoryRanges(VkDevice device, uint memoryRangeCount, IntPtr pMemoryRanges)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkFreeCommandBuffers_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkFreeCommandBuffers(VkDevice device, VkCommandPool commandPool, uint commandBufferCount, ref VkCommandBuffer pCommandBuffers)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkFreeCommandBuffers(VkDevice device, VkCommandPool commandPool, uint commandBufferCount, IntPtr pCommandBuffers)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkFreeDescriptorSets_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkFreeDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, uint descriptorSetCount, ref VkDescriptorSet pDescriptorSets)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkFreeDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, uint descriptorSetCount, IntPtr pDescriptorSets)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkFreeMemory_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkFreeMemory(VkDevice device, VkDeviceMemory memory, ref VkAllocationCallbacks pAllocator)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkFreeMemory(VkDevice device, VkDeviceMemory memory, IntPtr pAllocator)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetBufferMemoryRequirements_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetBufferMemoryRequirements(VkDevice device, VkBuffer buffer, out VkMemoryRequirements pMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetBufferMemoryRequirements2KHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetBufferMemoryRequirements2KHR(VkDevice device, ref VkBufferMemoryRequirementsInfo2KHR pInfo, out VkMemoryRequirements2KHR pMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkGetBufferMemoryRequirements2KHR(VkDevice device, IntPtr pInfo, out VkMemoryRequirements2KHR pMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetDeviceGroupPeerMemoryFeaturesKHX_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetDeviceGroupPeerMemoryFeaturesKHX(VkDevice device, uint heapIndex, uint localDeviceIndex, uint remoteDeviceIndex, out VkPeerMemoryFeatureFlagsKHX pPeerMemoryFeatures)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetDeviceGroupPresentCapabilitiesKHX_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetDeviceGroupPresentCapabilitiesKHX(VkDevice device, out VkDeviceGroupPresentCapabilitiesKHX pDeviceGroupPresentCapabilities)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetDeviceGroupSurfacePresentModesKHX_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetDeviceGroupSurfacePresentModesKHX(VkDevice device, VkSurfaceKHR surface, out VkDeviceGroupPresentModeFlagsKHX pModes)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetDeviceMemoryCommitment_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetDeviceMemoryCommitment(VkDevice device, VkDeviceMemory memory, out ulong pCommittedMemoryInBytes)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetDeviceProcAddr_ptr;

        [Generator.CalliRewrite]
        public static unsafe IntPtr vkGetDeviceProcAddr(VkDevice device, out byte pName)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetDeviceQueue_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetDeviceQueue(VkDevice device, uint queueFamilyIndex, uint queueIndex, out VkQueue pQueue)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetDisplayModePropertiesKHR_ptr;



        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetDisplayModePropertiesKHR(VkPhysicalDevice physicalDevice, VkDisplayKHR display, ref uint pPropertyCount, out VkDisplayModePropertiesKHR pProperties)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetDisplayModePropertiesKHR(VkPhysicalDevice physicalDevice, VkDisplayKHR display, IntPtr pPropertyCount, out VkDisplayModePropertiesKHR pProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetDisplayPlaneCapabilitiesKHR_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetDisplayPlaneCapabilitiesKHR(VkPhysicalDevice physicalDevice, VkDisplayModeKHR mode, uint planeIndex, out VkDisplayPlaneCapabilitiesKHR pCapabilities)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetDisplayPlaneSupportedDisplaysKHR_ptr;



        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetDisplayPlaneSupportedDisplaysKHR(VkPhysicalDevice physicalDevice, uint planeIndex, ref uint pDisplayCount, out VkDisplayKHR pDisplays)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetDisplayPlaneSupportedDisplaysKHR(VkPhysicalDevice physicalDevice, uint planeIndex, IntPtr pDisplayCount, out VkDisplayKHR pDisplays)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetEventStatus_ptr;

        private static IntPtr vkGetFenceFdKHR_ptr;



        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetFenceFdKHR(VkDevice device, ref VkFenceGetFdInfoKHR pGetFdInfo, out int pFd)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetFenceFdKHR(VkDevice device, IntPtr pGetFdInfo, out int pFd)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetFenceStatus_ptr;

        private static IntPtr vkGetFenceWin32HandleKHR_ptr;



        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetFenceWin32HandleKHR(VkDevice device, ref VkFenceGetWin32HandleInfoKHR pGetWin32HandleInfo, out Win32.HANDLE pHandle)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetFenceWin32HandleKHR(VkDevice device, IntPtr pGetWin32HandleInfo, out Win32.HANDLE pHandle)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetImageMemoryRequirements_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetImageMemoryRequirements(VkDevice device, VkImage image, out VkMemoryRequirements pMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetImageMemoryRequirements2KHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetImageMemoryRequirements2KHR(VkDevice device, ref VkImageMemoryRequirementsInfo2KHR pInfo, out VkMemoryRequirements2KHR pMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkGetImageMemoryRequirements2KHR(VkDevice device, IntPtr pInfo, out VkMemoryRequirements2KHR pMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetImageSparseMemoryRequirements_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetImageSparseMemoryRequirements(VkDevice device, VkImage image, ref uint pSparseMemoryRequirementCount, out VkSparseImageMemoryRequirements pSparseMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkGetImageSparseMemoryRequirements(VkDevice device, VkImage image, IntPtr pSparseMemoryRequirementCount, out VkSparseImageMemoryRequirements pSparseMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetImageSparseMemoryRequirements2KHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetImageSparseMemoryRequirements2KHR(VkDevice device, ref VkImageSparseMemoryRequirementsInfo2KHR pInfo, ref uint pSparseMemoryRequirementCount, out VkSparseImageMemoryRequirements2KHR pSparseMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkGetImageSparseMemoryRequirements2KHR(VkDevice device, ref VkImageSparseMemoryRequirementsInfo2KHR pInfo, IntPtr pSparseMemoryRequirementCount, out VkSparseImageMemoryRequirements2KHR pSparseMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkGetImageSparseMemoryRequirements2KHR(VkDevice device, IntPtr pInfo, ref uint pSparseMemoryRequirementCount, out VkSparseImageMemoryRequirements2KHR pSparseMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkGetImageSparseMemoryRequirements2KHR(VkDevice device, IntPtr pInfo, IntPtr pSparseMemoryRequirementCount, out VkSparseImageMemoryRequirements2KHR pSparseMemoryRequirements)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetImageSubresourceLayout_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetImageSubresourceLayout(VkDevice device, VkImage image, ref VkImageSubresource pSubresource, out VkSubresourceLayout pLayout)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkGetImageSubresourceLayout(VkDevice device, VkImage image, IntPtr pSubresource, out VkSubresourceLayout pLayout)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetInstanceProcAddr_ptr;

        [Generator.CalliRewrite]
        public static unsafe IntPtr vkGetInstanceProcAddr(VkInstance instance, IntPtr Name)
        {
            throw new NotImplementedException();
        }
        public static unsafe IntPtr vkGetInstanceProcAddr(VkInstance instance, byte* Name)
        {
            throw new NotImplementedException();
        }
        private static IntPtr vkGetMemoryFdKHR_ptr;



        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetMemoryFdKHR(VkDevice device, ref VkMemoryGetFdInfoKHR pGetFdInfo, out int pFd)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetMemoryFdKHR(VkDevice device, IntPtr pGetFdInfo, out int pFd)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetMemoryFdPropertiesKHR_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_INVALID_EXTERNAL_HANDLE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetMemoryFdPropertiesKHR(VkDevice device, VkExternalMemoryHandleTypeFlagsKHR handleType, int fd, out VkMemoryFdPropertiesKHR pMemoryFdProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetMemoryHostPointerPropertiesEXT_ptr;


        private static IntPtr vkGetMemoryWin32HandleKHR_ptr;



        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetMemoryWin32HandleKHR(VkDevice device, ref VkMemoryGetWin32HandleInfoKHR pGetWin32HandleInfo, out Win32.HANDLE pHandle)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetMemoryWin32HandleKHR(VkDevice device, IntPtr pGetWin32HandleInfo, out Win32.HANDLE pHandle)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetMemoryWin32HandleNV_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetMemoryWin32HandleNV(VkDevice device, VkDeviceMemory memory, VkExternalMemoryHandleTypeFlagsNV handleType, out Win32.HANDLE pHandle)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetMemoryWin32HandlePropertiesKHR_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_INVALID_EXTERNAL_HANDLE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetMemoryWin32HandlePropertiesKHR(VkDevice device, VkExternalMemoryHandleTypeFlagsKHR handleType, Win32.HANDLE handle, out VkMemoryWin32HandlePropertiesKHR pMemoryWin32HandleProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPastPresentationTimingGOOGLE_ptr;



        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_DEVICE_LOST, VK_ERROR_OUT_OF_DATE_KHR, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPastPresentationTimingGOOGLE(VkDevice device, VkSwapchainKHR swapchain, ref uint pPresentationTimingCount, out VkPastPresentationTimingGOOGLE pPresentationTimings)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_DEVICE_LOST, VK_ERROR_OUT_OF_DATE_KHR, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPastPresentationTimingGOOGLE(VkDevice device, VkSwapchainKHR swapchain, IntPtr pPresentationTimingCount, out VkPastPresentationTimingGOOGLE pPresentationTimings)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceDisplayPlanePropertiesKHR_ptr;



        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceDisplayPlanePropertiesKHR(VkPhysicalDevice physicalDevice, ref uint pPropertyCount, out VkDisplayPlanePropertiesKHR pProperties)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceDisplayPlanePropertiesKHR(VkPhysicalDevice physicalDevice, IntPtr pPropertyCount, out VkDisplayPlanePropertiesKHR pProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceDisplayPropertiesKHR_ptr;



        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceDisplayPropertiesKHR(VkPhysicalDevice physicalDevice, ref uint pPropertyCount, out VkDisplayPropertiesKHR pProperties)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceDisplayPropertiesKHR(VkPhysicalDevice physicalDevice, IntPtr pPropertyCount, out VkDisplayPropertiesKHR pProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceExternalBufferPropertiesKHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceExternalBufferPropertiesKHR(VkPhysicalDevice physicalDevice, ref VkPhysicalDeviceExternalBufferInfoKHR pExternalBufferInfo, out VkExternalBufferPropertiesKHR pExternalBufferProperties)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceExternalBufferPropertiesKHR(VkPhysicalDevice physicalDevice, IntPtr pExternalBufferInfo, out VkExternalBufferPropertiesKHR pExternalBufferProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceExternalFencePropertiesKHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceExternalFencePropertiesKHR(VkPhysicalDevice physicalDevice, ref VkPhysicalDeviceExternalFenceInfoKHR pExternalFenceInfo, out VkExternalFencePropertiesKHR pExternalFenceProperties)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceExternalFencePropertiesKHR(VkPhysicalDevice physicalDevice, IntPtr pExternalFenceInfo, out VkExternalFencePropertiesKHR pExternalFenceProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceExternalImageFormatPropertiesNV_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_FORMAT_NOT_SUPPORTED</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceExternalImageFormatPropertiesNV(VkPhysicalDevice physicalDevice, VkFormat format, VkImageType type, VkImageTiling tiling, VkImageUsageFlags usage, VkImageCreateFlags flags, VkExternalMemoryHandleTypeFlagsNV externalHandleType, out VkExternalImageFormatPropertiesNV pExternalImageFormatProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceExternalSemaphorePropertiesKHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceExternalSemaphorePropertiesKHR(VkPhysicalDevice physicalDevice, ref VkPhysicalDeviceExternalSemaphoreInfoKHR pExternalSemaphoreInfo, out VkExternalSemaphorePropertiesKHR pExternalSemaphoreProperties)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceExternalSemaphorePropertiesKHR(VkPhysicalDevice physicalDevice, IntPtr pExternalSemaphoreInfo, out VkExternalSemaphorePropertiesKHR pExternalSemaphoreProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceFeatures_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceFeatures(VkPhysicalDevice physicalDevice, out VkPhysicalDeviceFeatures pFeatures)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceFeatures2KHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceFeatures2KHR(VkPhysicalDevice physicalDevice, out VkPhysicalDeviceFeatures2KHR pFeatures)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceFormatProperties_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceFormatProperties(VkPhysicalDevice physicalDevice, VkFormat format, out VkFormatProperties pFormatProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceFormatProperties2KHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceFormatProperties2KHR(VkPhysicalDevice physicalDevice, VkFormat format, out VkFormatProperties2KHR pFormatProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceGeneratedCommandsPropertiesNVX_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceGeneratedCommandsPropertiesNVX(VkPhysicalDevice physicalDevice, ref VkDeviceGeneratedCommandsFeaturesNVX pFeatures, out VkDeviceGeneratedCommandsLimitsNVX pLimits)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceGeneratedCommandsPropertiesNVX(VkPhysicalDevice physicalDevice, IntPtr pFeatures, out VkDeviceGeneratedCommandsLimitsNVX pLimits)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceImageFormatProperties_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_FORMAT_NOT_SUPPORTED</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceImageFormatProperties(VkPhysicalDevice physicalDevice, VkFormat format, VkImageType type, VkImageTiling tiling, VkImageUsageFlags usage, VkImageCreateFlags flags, out VkImageFormatProperties pImageFormatProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceImageFormatProperties2KHR_ptr;



        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_FORMAT_NOT_SUPPORTED</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceImageFormatProperties2KHR(VkPhysicalDevice physicalDevice, ref VkPhysicalDeviceImageFormatInfo2KHR pImageFormatInfo, out VkImageFormatProperties2KHR pImageFormatProperties)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_FORMAT_NOT_SUPPORTED</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceImageFormatProperties2KHR(VkPhysicalDevice physicalDevice, IntPtr pImageFormatInfo, out VkImageFormatProperties2KHR pImageFormatProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceMemoryProperties_ptr;


        private static IntPtr vkGetPhysicalDeviceMemoryProperties2KHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceMemoryProperties2KHR(VkPhysicalDevice physicalDevice, out VkPhysicalDeviceMemoryProperties2KHR pMemoryProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceMirPresentationSupportKHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe VkBool32 vkGetPhysicalDeviceMirPresentationSupportKHR(VkPhysicalDevice physicalDevice, uint queueFamilyIndex, out Mir.MirConnection connection)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceMultisamplePropertiesEXT_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceMultisamplePropertiesEXT(VkPhysicalDevice physicalDevice, VkSampleCountFlags samples, out VkMultisamplePropertiesEXT pMultisampleProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDevicePresentRectanglesKHX_ptr;



        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDevicePresentRectanglesKHX(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, ref uint pRectCount, out VkRect2D pRects)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDevicePresentRectanglesKHX(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, IntPtr pRectCount, out VkRect2D pRects)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceProperties_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceProperties(VkPhysicalDevice physicalDevice, out VkPhysicalDeviceProperties pProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceProperties2KHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceProperties2KHR(VkPhysicalDevice physicalDevice, out VkPhysicalDeviceProperties2KHR pProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceQueueFamilyProperties_ptr;

		[Generator.CalliRewrite]
		public static unsafe void vkGetPhysicalDeviceQueueFamilyProperties (VkPhysicalDevice physicalDevice, ref uint pQueueFamilyPropertyCount, out VkQueueFamilyProperties pQueueFamilyProperties) {
			throw new NotImplementedException ();
		}

		[Generator.CalliRewrite]
		public static unsafe void vkGetPhysicalDeviceQueueFamilyProperties (VkPhysicalDevice physicalDevice, ref uint pQueueFamilyPropertyCount, IntPtr? pQueueFamilyProperties) {
			throw new NotImplementedException ();
		}

		[Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceQueueFamilyProperties(VkPhysicalDevice physicalDevice, IntPtr pQueueFamilyPropertyCount, out VkQueueFamilyProperties pQueueFamilyProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceQueueFamilyProperties2KHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceQueueFamilyProperties2KHR(VkPhysicalDevice physicalDevice, ref uint pQueueFamilyPropertyCount, out VkQueueFamilyProperties2KHR pQueueFamilyProperties)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceQueueFamilyProperties2KHR(VkPhysicalDevice physicalDevice, IntPtr pQueueFamilyPropertyCount, out VkQueueFamilyProperties2KHR pQueueFamilyProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceSparseImageFormatProperties_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceSparseImageFormatProperties(VkPhysicalDevice physicalDevice, VkFormat format, VkImageType type, VkSampleCountFlags samples, VkImageUsageFlags usage, VkImageTiling tiling, ref uint pPropertyCount, out VkSparseImageFormatProperties pProperties)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceSparseImageFormatProperties(VkPhysicalDevice physicalDevice, VkFormat format, VkImageType type, VkSampleCountFlags samples, VkImageUsageFlags usage, VkImageTiling tiling, IntPtr pPropertyCount, out VkSparseImageFormatProperties pProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceSparseImageFormatProperties2KHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceSparseImageFormatProperties2KHR(VkPhysicalDevice physicalDevice, ref VkPhysicalDeviceSparseImageFormatInfo2KHR pFormatInfo, ref uint pPropertyCount, out VkSparseImageFormatProperties2KHR pProperties)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceSparseImageFormatProperties2KHR(VkPhysicalDevice physicalDevice, ref VkPhysicalDeviceSparseImageFormatInfo2KHR pFormatInfo, IntPtr pPropertyCount, out VkSparseImageFormatProperties2KHR pProperties)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceSparseImageFormatProperties2KHR(VkPhysicalDevice physicalDevice, IntPtr pFormatInfo, ref uint pPropertyCount, out VkSparseImageFormatProperties2KHR pProperties)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkGetPhysicalDeviceSparseImageFormatProperties2KHR(VkPhysicalDevice physicalDevice, IntPtr pFormatInfo, IntPtr pPropertyCount, out VkSparseImageFormatProperties2KHR pProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceSurfaceCapabilities2EXT_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceSurfaceCapabilities2EXT(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out VkSurfaceCapabilities2EXT pSurfaceCapabilities)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceSurfaceCapabilities2KHR_ptr;



        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceSurfaceCapabilities2KHR(VkPhysicalDevice physicalDevice, ref VkPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, out VkSurfaceCapabilities2KHR pSurfaceCapabilities)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceSurfaceCapabilities2KHR(VkPhysicalDevice physicalDevice, IntPtr pSurfaceInfo, out VkSurfaceCapabilities2KHR pSurfaceCapabilities)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceSurfaceCapabilitiesKHR_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceSurfaceCapabilitiesKHR(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out VkSurfaceCapabilitiesKHR pSurfaceCapabilities)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceSurfaceFormats2KHR_ptr;









        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceSurfaceFormats2KHR(VkPhysicalDevice physicalDevice, ref VkPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, ref uint pSurfaceFormatCount, out VkSurfaceFormat2KHR pSurfaceFormats)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceSurfaceFormats2KHR(VkPhysicalDevice physicalDevice, ref VkPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, IntPtr pSurfaceFormatCount, out VkSurfaceFormat2KHR pSurfaceFormats)
        {
            throw new NotImplementedException();
        }




        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceSurfaceFormats2KHR(VkPhysicalDevice physicalDevice, IntPtr pSurfaceInfo, ref uint pSurfaceFormatCount, out VkSurfaceFormat2KHR pSurfaceFormats)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceSurfaceFormats2KHR(VkPhysicalDevice physicalDevice, IntPtr pSurfaceInfo, IntPtr pSurfaceFormatCount, out VkSurfaceFormat2KHR pSurfaceFormats)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceSurfaceFormatsKHR_ptr;



		///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_SURFACE_LOST_KHR</remarks>
		[Generator.CalliRewrite]
		public static unsafe VkResult vkGetPhysicalDeviceSurfaceFormatsKHR (VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, ref uint pSurfaceFormatCount, out VkSurfaceFormatKHR pSurfaceFormats) {
			throw new NotImplementedException ();
		}

		///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_SURFACE_LOST_KHR</remarks>
		[Generator.CalliRewrite]
		public static unsafe VkResult vkGetPhysicalDeviceSurfaceFormatsKHR (VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, ref uint pSurfaceFormatCount, IntPtr pSurfaceFormats) {
			throw new NotImplementedException ();
		}


		///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_SURFACE_LOST_KHR</remarks>
		[Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceSurfaceFormatsKHR(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, IntPtr pSurfaceFormatCount, out VkSurfaceFormatKHR pSurfaceFormats)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceSurfacePresentModesKHR_ptr;



		///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_SURFACE_LOST_KHR</remarks>
		[Generator.CalliRewrite]
		public static unsafe VkResult vkGetPhysicalDeviceSurfacePresentModesKHR (VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, ref uint pPresentModeCount, out VkPresentModeKHR pPresentModes) {
			throw new NotImplementedException ();
		}

		///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_SURFACE_LOST_KHR</remarks>
		[Generator.CalliRewrite]
		public static unsafe VkResult vkGetPhysicalDeviceSurfacePresentModesKHR (VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, ref uint pPresentModeCount, IntPtr pPresentModes) {
			throw new NotImplementedException ();
		}

		///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_SURFACE_LOST_KHR</remarks>
		[Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceSurfacePresentModesKHR(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, IntPtr pPresentModeCount, out VkPresentModeKHR pPresentModes)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceSurfaceSupportKHR_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetPhysicalDeviceSurfaceSupportKHR(VkPhysicalDevice physicalDevice, uint queueFamilyIndex, VkSurfaceKHR surface, out VkBool32 pSupported)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceWaylandPresentationSupportKHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe VkBool32 vkGetPhysicalDeviceWaylandPresentationSupportKHR(VkPhysicalDevice physicalDevice, uint queueFamilyIndex, out Wayland.wl_display display)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceWin32PresentationSupportKHR_ptr;

        private static IntPtr vkGetPhysicalDeviceXcbPresentationSupportKHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe VkBool32 vkGetPhysicalDeviceXcbPresentationSupportKHR(VkPhysicalDevice physicalDevice, uint queueFamilyIndex, ref Xcb.xcb_connection_t connection, Xcb.xcb_visualid_t visual_id)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe VkBool32 vkGetPhysicalDeviceXcbPresentationSupportKHR(VkPhysicalDevice physicalDevice, uint queueFamilyIndex, IntPtr connection, Xcb.xcb_visualid_t visual_id)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPhysicalDeviceXlibPresentationSupportKHR_ptr;

        [Generator.CalliRewrite]
        public static unsafe VkBool32 vkGetPhysicalDeviceXlibPresentationSupportKHR(VkPhysicalDevice physicalDevice, uint queueFamilyIndex, ref Xlib.Display dpy, Xlib.VisualID visualID)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe VkBool32 vkGetPhysicalDeviceXlibPresentationSupportKHR(VkPhysicalDevice physicalDevice, uint queueFamilyIndex, IntPtr dpy, Xlib.VisualID visualID)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetPipelineCacheData_ptr;



        private static IntPtr vkGetQueryPoolResults_ptr;

        [Generator.CalliRewrite]
        public static unsafe VkBool32 vkGetQueryPoolResults(VkDevice device, VkQueryPool queryPool, uint firstQuery, uint queryCount, ulong dataSize, IntPtr pData, ulong stride, VkQueryResultFlags flags)
        {
            throw new NotImplementedException();
        }


        private static IntPtr vkGetRandROutputDisplayEXT_ptr;



        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetRandROutputDisplayEXT(VkPhysicalDevice physicalDevice, ref Xlib.Display dpy, IntPtr rrOutput, out VkDisplayKHR pDisplay)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetRandROutputDisplayEXT(VkPhysicalDevice physicalDevice, IntPtr dpy, IntPtr rrOutput, out VkDisplayKHR pDisplay)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetRefreshCycleDurationGOOGLE_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_DEVICE_LOST, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetRefreshCycleDurationGOOGLE(VkDevice device, VkSwapchainKHR swapchain, out VkRefreshCycleDurationGOOGLE pDisplayTimingProperties)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetRenderAreaGranularity_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkGetRenderAreaGranularity(VkDevice device, VkRenderPass renderPass, out VkExtent2D pGranularity)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetSemaphoreFdKHR_ptr;



        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetSemaphoreFdKHR(VkDevice device, ref VkSemaphoreGetFdInfoKHR pGetFdInfo, out int pFd)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetSemaphoreFdKHR(VkDevice device, IntPtr pGetFdInfo, out int pFd)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetSemaphoreWin32HandleKHR_ptr;



        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetSemaphoreWin32HandleKHR(VkDevice device, ref VkSemaphoreGetWin32HandleInfoKHR pGetWin32HandleInfo, out Win32.HANDLE pHandle)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_TOO_MANY_OBJECTS, VK_ERROR_OUT_OF_HOST_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetSemaphoreWin32HandleKHR(VkDevice device, IntPtr pGetWin32HandleInfo, out Win32.HANDLE pHandle)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetShaderInfoAMD_ptr;



        private static IntPtr vkGetSwapchainCounterEXT_ptr;

        ///<remarks>Success codes:VK_SUCCESS, VK_ERROR_DEVICE_LOST, VK_ERROR_OUT_OF_DATE_KHR. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetSwapchainCounterEXT(VkDevice device, VkSwapchainKHR swapchain, VkSurfaceCounterFlagsEXT counter, out ulong pCounterValue)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetSwapchainGrallocUsageANDROID_ptr;

        [Generator.CalliRewrite]
        public static unsafe VkResult vkGetSwapchainGrallocUsageANDROID(VkDevice device, VkFormat format, VkImageUsageFlags imageUsage, out int grallocUsage)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetSwapchainImagesKHR_ptr;



		///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
		[Generator.CalliRewrite]
		public static unsafe VkResult vkGetSwapchainImagesKHR (VkDevice device, VkSwapchainKHR swapchain, ref uint pSwapchainImageCount, out VkImage pSwapchainImages) {
			throw new NotImplementedException ();
		}

		///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
		[Generator.CalliRewrite]
		public static unsafe VkResult vkGetSwapchainImagesKHR (VkDevice device, VkSwapchainKHR swapchain, ref uint pSwapchainImageCount, IntPtr pSwapchainImages) {
			throw new NotImplementedException ();
		}

		///<remarks>Success codes:VK_SUCCESS, VK_INCOMPLETE. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
		[Generator.CalliRewrite]
        public static unsafe VkResult vkGetSwapchainImagesKHR(VkDevice device, VkSwapchainKHR swapchain, IntPtr pSwapchainImageCount, out VkImage pSwapchainImages)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkGetSwapchainStatusKHR_ptr;

        private static IntPtr vkGetValidationCacheDataEXT_ptr;



        private static IntPtr vkImportFenceFdKHR_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_INVALID_EXTERNAL_HANDLE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkImportFenceFdKHR(VkDevice device, ref VkImportFenceFdInfoKHR pImportFenceFdInfo)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_INVALID_EXTERNAL_HANDLE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkImportFenceFdKHR(VkDevice device, IntPtr pImportFenceFdInfo)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkImportFenceWin32HandleKHR_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_INVALID_EXTERNAL_HANDLE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkImportFenceWin32HandleKHR(VkDevice device, ref VkImportFenceWin32HandleInfoKHR pImportFenceWin32HandleInfo)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_INVALID_EXTERNAL_HANDLE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkImportFenceWin32HandleKHR(VkDevice device, IntPtr pImportFenceWin32HandleInfo)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkImportSemaphoreFdKHR_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_INVALID_EXTERNAL_HANDLE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkImportSemaphoreFdKHR(VkDevice device, ref VkImportSemaphoreFdInfoKHR pImportSemaphoreFdInfo)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_INVALID_EXTERNAL_HANDLE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkImportSemaphoreFdKHR(VkDevice device, IntPtr pImportSemaphoreFdInfo)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkImportSemaphoreWin32HandleKHR_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_INVALID_EXTERNAL_HANDLE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkImportSemaphoreWin32HandleKHR(VkDevice device, ref VkImportSemaphoreWin32HandleInfoKHR pImportSemaphoreWin32HandleInfo)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_INVALID_EXTERNAL_HANDLE_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkImportSemaphoreWin32HandleKHR(VkDevice device, IntPtr pImportSemaphoreWin32HandleInfo)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkInvalidateMappedMemoryRanges_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkInvalidateMappedMemoryRanges(VkDevice device, uint memoryRangeCount, ref VkMappedMemoryRange pMemoryRanges)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkInvalidateMappedMemoryRanges(VkDevice device, uint memoryRangeCount, IntPtr pMemoryRanges)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkMapMemory_ptr;

        private static IntPtr vkMergePipelineCaches_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkMergePipelineCaches(VkDevice device, VkPipelineCache dstCache, uint srcCacheCount, ref VkPipelineCache pSrcCaches)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkMergePipelineCaches(VkDevice device, VkPipelineCache dstCache, uint srcCacheCount, IntPtr pSrcCaches)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkMergeValidationCachesEXT_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkMergeValidationCachesEXT(VkDevice device, VkValidationCacheEXT dstCache, uint srcCacheCount, ref VkValidationCacheEXT pSrcCaches)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkMergeValidationCachesEXT(VkDevice device, VkValidationCacheEXT dstCache, uint srcCacheCount, IntPtr pSrcCaches)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkQueueBindSparse_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkQueueBindSparse(VkQueue queue, uint bindInfoCount, ref VkBindSparseInfo pBindInfo, VkFence fence)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkQueueBindSparse(VkQueue queue, uint bindInfoCount, IntPtr pBindInfo, VkFence fence)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkQueuePresentKHR_ptr;

        ///<remarks>Success codes:VK_SUCCESS, VK_SUBOPTIMAL_KHR. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST, VK_ERROR_OUT_OF_DATE_KHR, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkQueuePresentKHR(VkQueue queue, ref VkPresentInfoKHR pPresentInfo)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS, VK_SUBOPTIMAL_KHR. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST, VK_ERROR_OUT_OF_DATE_KHR, VK_ERROR_SURFACE_LOST_KHR</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkQueuePresentKHR(VkQueue queue, IntPtr pPresentInfo)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkQueueSignalReleaseImageANDROID_ptr;

        [Generator.CalliRewrite]
        public static unsafe VkResult vkQueueSignalReleaseImageANDROID(VkQueue queue, uint waitSemaphoreCount, ref VkSemaphore pWaitSemaphores, VkImage image, ref int pNativeFenceFd)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe VkResult vkQueueSignalReleaseImageANDROID(VkQueue queue, uint waitSemaphoreCount, ref VkSemaphore pWaitSemaphores, VkImage image, IntPtr pNativeFenceFd)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe VkResult vkQueueSignalReleaseImageANDROID(VkQueue queue, uint waitSemaphoreCount, IntPtr pWaitSemaphores, VkImage image, ref int pNativeFenceFd)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe VkResult vkQueueSignalReleaseImageANDROID(VkQueue queue, uint waitSemaphoreCount, IntPtr pWaitSemaphores, VkImage image, IntPtr pNativeFenceFd)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkQueueSubmit_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkQueueSubmit(VkQueue queue, uint submitCount, ref VkSubmitInfo pSubmits, VkFence fence)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkQueueSubmit(VkQueue queue, uint submitCount, IntPtr pSubmits, VkFence fence)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkQueueWaitIdle_ptr;

        [Generator.CalliRewrite]
        public static unsafe VkResult vkQueueWaitIdle (VkQueue queue) {
            throw new NotImplementedException ();
        }

        private static IntPtr vkRegisterDeviceEventEXT_ptr;













        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkRegisterDeviceEventEXT(VkDevice device, ref VkDeviceEventInfoEXT pDeviceEventInfo, ref VkAllocationCallbacks pAllocator, ref VkFence pFence)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkRegisterDeviceEventEXT(VkDevice device, ref VkDeviceEventInfoEXT pDeviceEventInfo, ref VkAllocationCallbacks pAllocator, IntPtr pFence)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkRegisterDeviceEventEXT(VkDevice device, ref VkDeviceEventInfoEXT pDeviceEventInfo, IntPtr pAllocator, ref VkFence pFence)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkRegisterDeviceEventEXT(VkDevice device, ref VkDeviceEventInfoEXT pDeviceEventInfo, IntPtr pAllocator, IntPtr pFence)
        {
            throw new NotImplementedException();
        }





        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkRegisterDeviceEventEXT(VkDevice device, IntPtr pDeviceEventInfo, ref VkAllocationCallbacks pAllocator, ref VkFence pFence)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkRegisterDeviceEventEXT(VkDevice device, IntPtr pDeviceEventInfo, ref VkAllocationCallbacks pAllocator, IntPtr pFence)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkRegisterDeviceEventEXT(VkDevice device, IntPtr pDeviceEventInfo, IntPtr pAllocator, ref VkFence pFence)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkRegisterDeviceEventEXT(VkDevice device, IntPtr pDeviceEventInfo, IntPtr pAllocator, IntPtr pFence)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkRegisterDisplayEventEXT_ptr;













        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkRegisterDisplayEventEXT(VkDevice device, VkDisplayKHR display, ref VkDisplayEventInfoEXT pDisplayEventInfo, ref VkAllocationCallbacks pAllocator, ref VkFence pFence)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkRegisterDisplayEventEXT(VkDevice device, VkDisplayKHR display, ref VkDisplayEventInfoEXT pDisplayEventInfo, ref VkAllocationCallbacks pAllocator, IntPtr pFence)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkRegisterDisplayEventEXT(VkDevice device, VkDisplayKHR display, ref VkDisplayEventInfoEXT pDisplayEventInfo, IntPtr pAllocator, ref VkFence pFence)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkRegisterDisplayEventEXT(VkDevice device, VkDisplayKHR display, ref VkDisplayEventInfoEXT pDisplayEventInfo, IntPtr pAllocator, IntPtr pFence)
        {
            throw new NotImplementedException();
        }





        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkRegisterDisplayEventEXT(VkDevice device, VkDisplayKHR display, IntPtr pDisplayEventInfo, ref VkAllocationCallbacks pAllocator, ref VkFence pFence)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkRegisterDisplayEventEXT(VkDevice device, VkDisplayKHR display, IntPtr pDisplayEventInfo, ref VkAllocationCallbacks pAllocator, IntPtr pFence)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkRegisterDisplayEventEXT(VkDevice device, VkDisplayKHR display, IntPtr pDisplayEventInfo, IntPtr pAllocator, ref VkFence pFence)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkRegisterDisplayEventEXT(VkDevice device, VkDisplayKHR display, IntPtr pDisplayEventInfo, IntPtr pAllocator, IntPtr pFence)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkRegisterObjectsNVX_ptr;







        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkRegisterObjectsNVX(VkDevice device, VkObjectTableNVX objectTable, uint objectCount, IntPtr ppObjectTableEntries, ref uint pObjectIndices)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkRegisterObjectsNVX(VkDevice device, VkObjectTableNVX objectTable, uint objectCount, IntPtr ppObjectTableEntries, IntPtr pObjectIndices)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkReleaseDisplayEXT_ptr;

        private static IntPtr vkResetCommandBuffer_ptr;

        private static IntPtr vkResetCommandPool_ptr;

        private static IntPtr vkResetDescriptorPool_ptr;
        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkResetDescriptorPool(VkDevice device, VkDescriptorPool descriptorPool, uint flags)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkResetEvent_ptr;

        private static IntPtr vkResetFences_ptr;

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkResetFences(VkDevice device, uint fenceCount, ref VkFence pFences)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkResetFences(VkDevice device, uint fenceCount, IntPtr pFences)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkSetEvent_ptr;

        private static IntPtr vkSetHdrMetadataEXT_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkSetHdrMetadataEXT(VkDevice device, uint swapchainCount, ref VkSwapchainKHR pSwapchains, ref VkHdrMetadataEXT pMetadata)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkSetHdrMetadataEXT(VkDevice device, uint swapchainCount, ref VkSwapchainKHR pSwapchains, IntPtr pMetadata)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkSetHdrMetadataEXT(VkDevice device, uint swapchainCount, IntPtr pSwapchains, ref VkHdrMetadataEXT pMetadata)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkSetHdrMetadataEXT(VkDevice device, uint swapchainCount, IntPtr pSwapchains, IntPtr pMetadata)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkTrimCommandPoolKHR_ptr;

        private static IntPtr vkUnmapMemory_ptr;
        [Generator.CalliRewrite]
        public static unsafe void vkUnmapMemory(VkDevice device, VkDeviceMemory memory)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkUnregisterObjectsNVX_ptr;




        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkUnregisterObjectsNVX(VkDevice device, VkObjectTableNVX objectTable, uint objectCount, ref VkObjectEntryTypeNVX pObjectEntryTypes, ref uint pObjectIndices)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkUnregisterObjectsNVX(VkDevice device, VkObjectTableNVX objectTable, uint objectCount, ref VkObjectEntryTypeNVX pObjectEntryTypes, IntPtr pObjectIndices)
        {
            throw new NotImplementedException();
        }


        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkUnregisterObjectsNVX(VkDevice device, VkObjectTableNVX objectTable, uint objectCount, IntPtr pObjectEntryTypes, ref uint pObjectIndices)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkUnregisterObjectsNVX(VkDevice device, VkObjectTableNVX objectTable, uint objectCount, IntPtr pObjectEntryTypes, IntPtr pObjectIndices)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkUpdateDescriptorSets_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkUpdateDescriptorSets(VkDevice device, uint descriptorWriteCount, ref VkWriteDescriptorSet pDescriptorWrites, uint descriptorCopyCount, ref VkCopyDescriptorSet pDescriptorCopies)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkUpdateDescriptorSets(VkDevice device, uint descriptorWriteCount, ref VkWriteDescriptorSet pDescriptorWrites, uint descriptorCopyCount, IntPtr pDescriptorCopies)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkUpdateDescriptorSets(VkDevice device, uint descriptorWriteCount, IntPtr pDescriptorWrites, uint descriptorCopyCount, ref VkCopyDescriptorSet pDescriptorCopies)
        {
            throw new NotImplementedException();
        }

        [Generator.CalliRewrite]
        public static unsafe void vkUpdateDescriptorSets(VkDevice device, uint descriptorWriteCount, IntPtr pDescriptorWrites, uint descriptorCopyCount, IntPtr pDescriptorCopies)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkUpdateDescriptorSetWithTemplateKHR_ptr;

        private static IntPtr vkWaitForFences_ptr;

        ///<remarks>Success codes:VK_SUCCESS, VK_TIMEOUT. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkWaitForFences(VkDevice device, uint fenceCount, ref VkFence pFences, VkBool32 waitAll, ulong timeout)
        {
            throw new NotImplementedException();
        }

        ///<remarks>Success codes:VK_SUCCESS, VK_TIMEOUT. Error codes:VK_ERROR_OUT_OF_HOST_MEMORY, VK_ERROR_OUT_OF_DEVICE_MEMORY, VK_ERROR_DEVICE_LOST</remarks>
        [Generator.CalliRewrite]
        public static unsafe VkResult vkWaitForFences(VkDevice device, uint fenceCount, IntPtr pFences, VkBool32 waitAll, ulong timeout)
        {
            throw new NotImplementedException();
        }
		public static IntPtr GetDelegate (VkInstance inst, string name) {
			byte[] n = System.Text.Encoding.UTF8.GetBytes(name +'\0');
			GCHandle hnd = GCHandle.Alloc (n, GCHandleType.Pinned);
			IntPtr del = vkGetInstanceProcAddr (inst, hnd.AddrOfPinnedObject());
			if (del == IntPtr.Zero)
				Console.WriteLine ("instance function pointer not found for " + name);
			hnd.Free ();
			return del;
        }
		public static void LoadInstanceFunctionPointers (VkInstance inst) {
			vkCreateDebugReportCallbackEXT_ptr = GetDelegate (inst, "vkCreateDebugReportCallbackEXT"); 
			vkDestroyDebugReportCallbackEXT_ptr = GetDelegate (inst, "vkDestroyDebugReportCallbackEXT");			 
            vkEnumeratePhysicalDevices_ptr = GetDelegate (inst, "vkEnumeratePhysicalDevices");
            vkGetPhysicalDeviceProperties_ptr = GetDelegate (inst, "vkGetPhysicalDeviceProperties");
            vkGetPhysicalDeviceQueueFamilyProperties_ptr = GetDelegate (inst, "vkGetPhysicalDeviceQueueFamilyProperties");
            vkGetPhysicalDeviceMemoryProperties_ptr = GetDelegate (inst, "vkGetPhysicalDeviceMemoryProperties");
            vkGetPhysicalDeviceFeatures_ptr = GetDelegate (inst, "vkGetPhysicalDeviceFeatures");
            vkGetPhysicalDeviceFormatProperties_ptr = GetDelegate (inst, "vkGetPhysicalDeviceFormatProperties");
            vkGetPhysicalDeviceImageFormatProperties_ptr = GetDelegate (inst, "vkGetPhysicalDeviceImageFormatProperties");
            vkCreateDevice_ptr = GetDelegate (inst, "vkCreateDevice");
            vkDestroyDevice_ptr = GetDelegate (inst, "vkDestroyDevice");
            vkEnumerateInstanceLayerProperties_ptr = GetDelegate (inst, "vkEnumerateInstanceLayerProperties");
            vkEnumerateInstanceExtensionProperties_ptr = GetDelegate (inst, "vkEnumerateInstanceExtensionProperties");
            vkEnumerateDeviceLayerProperties_ptr = GetDelegate (inst, "vkEnumerateDeviceLayerProperties");
            vkEnumerateDeviceExtensionProperties_ptr = GetDelegate (inst, "vkEnumerateDeviceExtensionProperties");
            vkGetDeviceQueue_ptr = GetDelegate (inst, "vkGetDeviceQueue");
            vkQueueSubmit_ptr = GetDelegate (inst, "vkQueueSubmit");
            vkQueueWaitIdle_ptr = GetDelegate (inst, "vkQueueWaitIdle");
            vkDeviceWaitIdle_ptr = GetDelegate (inst, "vkDeviceWaitIdle");
            vkAllocateMemory_ptr = GetDelegate (inst, "vkAllocateMemory");
            vkFreeMemory_ptr = GetDelegate (inst, "vkFreeMemory");
            vkMapMemory_ptr = GetDelegate (inst, "vkMapMemory");
            vkUnmapMemory_ptr = GetDelegate (inst, "vkUnmapMemory");
            vkFlushMappedMemoryRanges_ptr = GetDelegate (inst, "vkFlushMappedMemoryRanges");
            vkInvalidateMappedMemoryRanges_ptr = GetDelegate (inst, "vkInvalidateMappedMemoryRanges");
            vkGetDeviceMemoryCommitment_ptr = GetDelegate (inst, "vkGetDeviceMemoryCommitment");
            vkGetBufferMemoryRequirements_ptr = GetDelegate (inst, "vkGetBufferMemoryRequirements");
            vkBindBufferMemory_ptr = GetDelegate (inst, "vkBindBufferMemory");
            vkGetImageMemoryRequirements_ptr = GetDelegate (inst, "vkGetImageMemoryRequirements");
            vkBindImageMemory_ptr = GetDelegate (inst, "vkBindImageMemory");
            vkGetImageSparseMemoryRequirements_ptr = GetDelegate (inst, "vkGetImageSparseMemoryRequirements");
            vkGetPhysicalDeviceSparseImageFormatProperties_ptr = GetDelegate (inst, "vkGetPhysicalDeviceSparseImageFormatProperties");
            vkQueueBindSparse_ptr = GetDelegate (inst, "vkQueueBindSparse");
            vkCreateFence_ptr = GetDelegate (inst, "vkCreateFence");
            vkDestroyFence_ptr = GetDelegate (inst, "vkDestroyFence");
            vkResetFences_ptr = GetDelegate (inst, "vkResetFences");
            vkGetFenceStatus_ptr = GetDelegate (inst, "vkGetFenceStatus");
            vkWaitForFences_ptr = GetDelegate (inst, "vkWaitForFences");
            vkCreateSemaphore_ptr = GetDelegate (inst, "vkCreateSemaphore");
            vkDestroySemaphore_ptr = GetDelegate (inst, "vkDestroySemaphore");
            vkCreateEvent_ptr = GetDelegate (inst, "vkCreateEvent");
            vkDestroyEvent_ptr = GetDelegate (inst, "vkDestroyEvent");
            vkGetEventStatus_ptr = GetDelegate (inst, "vkGetEventStatus");
            vkSetEvent_ptr = GetDelegate (inst, "vkSetEvent");
            vkResetEvent_ptr = GetDelegate (inst, "vkResetEvent");
            vkCreateQueryPool_ptr = GetDelegate (inst, "vkCreateQueryPool");
            vkDestroyQueryPool_ptr = GetDelegate (inst, "vkDestroyQueryPool");
            vkGetQueryPoolResults_ptr = GetDelegate (inst, "vkGetQueryPoolResults");
            vkCreateBuffer_ptr = GetDelegate (inst, "vkCreateBuffer");
            vkDestroyBuffer_ptr = GetDelegate (inst, "vkDestroyBuffer");
            vkCreateBufferView_ptr = GetDelegate (inst, "vkCreateBufferView");
            vkDestroyBufferView_ptr = GetDelegate (inst, "vkDestroyBufferView");
            vkCreateImage_ptr = GetDelegate (inst, "vkCreateImage");
            vkDestroyImage_ptr = GetDelegate (inst, "vkDestroyImage");
            vkGetImageSubresourceLayout_ptr = GetDelegate (inst, "vkGetImageSubresourceLayout");
            vkCreateImageView_ptr = GetDelegate (inst, "vkCreateImageView");
            vkDestroyImageView_ptr = GetDelegate (inst, "vkDestroyImageView");
            vkCreateShaderModule_ptr = GetDelegate (inst, "vkCreateShaderModule");
            vkDestroyShaderModule_ptr = GetDelegate (inst, "vkDestroyShaderModule");
            vkCreatePipelineCache_ptr = GetDelegate (inst, "vkCreatePipelineCache");
            vkDestroyPipelineCache_ptr = GetDelegate (inst, "vkDestroyPipelineCache");
            vkGetPipelineCacheData_ptr = GetDelegate (inst, "vkGetPipelineCacheData");
            vkMergePipelineCaches_ptr = GetDelegate (inst, "vkMergePipelineCaches");
            vkCreateGraphicsPipelines_ptr = GetDelegate (inst, "vkCreateGraphicsPipelines");
            vkCreateComputePipelines_ptr = GetDelegate (inst, "vkCreateComputePipelines");
            vkDestroyPipeline_ptr = GetDelegate (inst, "vkDestroyPipeline");
            vkCreatePipelineLayout_ptr = GetDelegate (inst, "vkCreatePipelineLayout");
            vkDestroyPipelineLayout_ptr = GetDelegate (inst, "vkDestroyPipelineLayout");
            vkCreateSampler_ptr = GetDelegate (inst, "vkCreateSampler");
            vkDestroySampler_ptr = GetDelegate (inst, "vkDestroySampler");
            vkCreateDescriptorSetLayout_ptr = GetDelegate (inst, "vkCreateDescriptorSetLayout");
            vkDestroyDescriptorSetLayout_ptr = GetDelegate (inst, "vkDestroyDescriptorSetLayout");
            vkCreateDescriptorPool_ptr = GetDelegate (inst, "vkCreateDescriptorPool");
            vkDestroyDescriptorPool_ptr = GetDelegate (inst, "vkDestroyDescriptorPool");
            vkResetDescriptorPool_ptr = GetDelegate (inst, "vkResetDescriptorPool");
            vkAllocateDescriptorSets_ptr = GetDelegate (inst, "vkAllocateDescriptorSets");
            vkFreeDescriptorSets_ptr = GetDelegate (inst, "vkFreeDescriptorSets");
            vkUpdateDescriptorSets_ptr = GetDelegate (inst, "vkUpdateDescriptorSets");
            vkCreateFramebuffer_ptr = GetDelegate (inst, "vkCreateFramebuffer");
            vkDestroyFramebuffer_ptr = GetDelegate (inst, "vkDestroyFramebuffer");
            vkCreateRenderPass_ptr = GetDelegate (inst, "vkCreateRenderPass");
            vkDestroyRenderPass_ptr = GetDelegate (inst, "vkDestroyRenderPass");
            vkGetRenderAreaGranularity_ptr = GetDelegate (inst, "vkGetRenderAreaGranularity");
            vkCreateCommandPool_ptr = GetDelegate (inst, "vkCreateCommandPool");
            vkDestroyCommandPool_ptr = GetDelegate (inst, "vkDestroyCommandPool");
            vkResetCommandPool_ptr = GetDelegate (inst, "vkResetCommandPool");
            vkAllocateCommandBuffers_ptr = GetDelegate (inst, "vkAllocateCommandBuffers");
            vkFreeCommandBuffers_ptr = GetDelegate (inst, "vkFreeCommandBuffers");
            vkBeginCommandBuffer_ptr = GetDelegate (inst, "vkBeginCommandBuffer");
            vkEndCommandBuffer_ptr = GetDelegate (inst, "vkEndCommandBuffer");
            vkResetCommandBuffer_ptr = GetDelegate (inst, "vkResetCommandBuffer");
            vkCmdBindPipeline_ptr = GetDelegate (inst, "vkCmdBindPipeline");
            vkCmdSetViewport_ptr = GetDelegate (inst, "vkCmdSetViewport");
            vkCmdSetScissor_ptr = GetDelegate (inst, "vkCmdSetScissor");
            vkCmdSetLineWidth_ptr = GetDelegate (inst, "vkCmdSetLineWidth");
            vkCmdSetDepthBias_ptr = GetDelegate (inst, "vkCmdSetDepthBias");
            vkCmdSetBlendConstants_ptr = GetDelegate (inst, "vkCmdSetBlendConstants");
            vkCmdSetDepthBounds_ptr = GetDelegate (inst, "vkCmdSetDepthBounds");
            vkCmdSetStencilCompareMask_ptr = GetDelegate (inst, "vkCmdSetStencilCompareMask");
            vkCmdSetStencilWriteMask_ptr = GetDelegate (inst, "vkCmdSetStencilWriteMask");
            vkCmdSetStencilReference_ptr = GetDelegate (inst, "vkCmdSetStencilReference");
            vkCmdBindDescriptorSets_ptr = GetDelegate (inst, "vkCmdBindDescriptorSets");
            vkCmdBindIndexBuffer_ptr = GetDelegate (inst, "vkCmdBindIndexBuffer");
            vkCmdBindVertexBuffers_ptr = GetDelegate (inst, "vkCmdBindVertexBuffers");
            vkCmdDraw_ptr = GetDelegate (inst, "vkCmdDraw");
            vkCmdDrawIndexed_ptr = GetDelegate (inst, "vkCmdDrawIndexed");
            vkCmdDrawIndirect_ptr = GetDelegate (inst, "vkCmdDrawIndirect");
            vkCmdDrawIndexedIndirect_ptr = GetDelegate (inst, "vkCmdDrawIndexedIndirect");
            vkCmdDispatch_ptr = GetDelegate (inst, "vkCmdDispatch");
            vkCmdDispatchIndirect_ptr = GetDelegate (inst, "vkCmdDispatchIndirect");
            vkCmdCopyBuffer_ptr = GetDelegate (inst, "vkCmdCopyBuffer");
            vkCmdCopyImage_ptr = GetDelegate (inst, "vkCmdCopyImage");
            vkCmdBlitImage_ptr = GetDelegate (inst, "vkCmdBlitImage");
            vkCmdCopyBufferToImage_ptr = GetDelegate (inst, "vkCmdCopyBufferToImage");
            vkCmdCopyImageToBuffer_ptr = GetDelegate (inst, "vkCmdCopyImageToBuffer");
            vkCmdUpdateBuffer_ptr = GetDelegate (inst, "vkCmdUpdateBuffer");
            vkCmdFillBuffer_ptr = GetDelegate (inst, "vkCmdFillBuffer");
            vkCmdClearColorImage_ptr = GetDelegate (inst, "vkCmdClearColorImage");
            vkCmdClearDepthStencilImage_ptr = GetDelegate (inst, "vkCmdClearDepthStencilImage");
            vkCmdClearAttachments_ptr = GetDelegate (inst, "vkCmdClearAttachments");
            vkCmdResolveImage_ptr = GetDelegate (inst, "vkCmdResolveImage");
            vkCmdSetEvent_ptr = GetDelegate (inst, "vkCmdSetEvent");
            vkCmdResetEvent_ptr = GetDelegate (inst, "vkCmdResetEvent");
            vkCmdWaitEvents_ptr = GetDelegate (inst, "vkCmdWaitEvents");
            vkCmdPipelineBarrier_ptr = GetDelegate (inst, "vkCmdPipelineBarrier");
            vkCmdBeginQuery_ptr = GetDelegate (inst, "vkCmdBeginQuery");
            vkCmdEndQuery_ptr = GetDelegate (inst, "vkCmdEndQuery");
            vkCmdResetQueryPool_ptr = GetDelegate (inst, "vkCmdResetQueryPool");
            vkCmdWriteTimestamp_ptr = GetDelegate (inst, "vkCmdWriteTimestamp");
            vkCmdCopyQueryPoolResults_ptr = GetDelegate (inst, "vkCmdCopyQueryPoolResults");
            vkCmdPushConstants_ptr = GetDelegate (inst, "vkCmdPushConstants");
            vkCmdBeginRenderPass_ptr = GetDelegate (inst, "vkCmdBeginRenderPass");
            vkCmdNextSubpass_ptr = GetDelegate (inst, "vkCmdNextSubpass");
            vkCmdEndRenderPass_ptr = GetDelegate (inst, "vkCmdEndRenderPass");
            vkCmdExecuteCommands_ptr = GetDelegate (inst, "vkCmdExecuteCommands");
            vkCreateAndroidSurfaceKHR_ptr = GetDelegate (inst, "vkCreateAndroidSurfaceKHR");
            vkGetPhysicalDeviceDisplayPropertiesKHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceDisplayPropertiesKHR");
            vkGetPhysicalDeviceDisplayPlanePropertiesKHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceDisplayPlanePropertiesKHR");
            vkGetDisplayPlaneSupportedDisplaysKHR_ptr = GetDelegate (inst, "vkGetDisplayPlaneSupportedDisplaysKHR");
            vkGetDisplayModePropertiesKHR_ptr = GetDelegate (inst, "vkGetDisplayModePropertiesKHR");
            vkCreateDisplayModeKHR_ptr = GetDelegate (inst, "vkCreateDisplayModeKHR");
            vkGetDisplayPlaneCapabilitiesKHR_ptr = GetDelegate (inst, "vkGetDisplayPlaneCapabilitiesKHR");
            vkCreateDisplayPlaneSurfaceKHR_ptr = GetDelegate (inst, "vkCreateDisplayPlaneSurfaceKHR");
            vkCreateSharedSwapchainsKHR_ptr = GetDelegate (inst, "vkCreateSharedSwapchainsKHR");
            vkCreateMirSurfaceKHR_ptr = GetDelegate (inst, "vkCreateMirSurfaceKHR");
            vkGetPhysicalDeviceMirPresentationSupportKHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceMirPresentationSupportKHR");
            vkDestroySurfaceKHR_ptr = GetDelegate (inst, "vkDestroySurfaceKHR");
            vkGetPhysicalDeviceSurfaceSupportKHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceSurfaceSupportKHR");
            vkGetPhysicalDeviceSurfaceCapabilitiesKHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceSurfaceCapabilitiesKHR");
            vkGetPhysicalDeviceSurfaceFormatsKHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceSurfaceFormatsKHR");
            vkGetPhysicalDeviceSurfacePresentModesKHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceSurfacePresentModesKHR");
            vkCreateSwapchainKHR_ptr = GetDelegate (inst, "vkCreateSwapchainKHR");
            vkDestroySwapchainKHR_ptr = GetDelegate (inst, "vkDestroySwapchainKHR");
            vkGetSwapchainImagesKHR_ptr = GetDelegate (inst, "vkGetSwapchainImagesKHR");
            vkAcquireNextImageKHR_ptr = GetDelegate (inst, "vkAcquireNextImageKHR");
            vkQueuePresentKHR_ptr = GetDelegate (inst, "vkQueuePresentKHR");
            vkCreateViSurfaceNN_ptr = GetDelegate (inst, "vkCreateViSurfaceNN");
            vkCreateWaylandSurfaceKHR_ptr = GetDelegate (inst, "vkCreateWaylandSurfaceKHR");
            vkGetPhysicalDeviceWaylandPresentationSupportKHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceWaylandPresentationSupportKHR");
            vkCreateWin32SurfaceKHR_ptr = GetDelegate (inst, "vkCreateWin32SurfaceKHR");
            vkGetPhysicalDeviceWin32PresentationSupportKHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceWin32PresentationSupportKHR");
            vkCreateXlibSurfaceKHR_ptr = GetDelegate (inst, "vkCreateXlibSurfaceKHR");
            vkGetPhysicalDeviceXlibPresentationSupportKHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceXlibPresentationSupportKHR");
            vkCreateXcbSurfaceKHR_ptr = GetDelegate (inst, "vkCreateXcbSurfaceKHR");
            vkGetPhysicalDeviceXcbPresentationSupportKHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceXcbPresentationSupportKHR");
            vkDebugReportMessageEXT_ptr = GetDelegate (inst, "vkDebugReportMessageEXT");
            vkDebugMarkerSetObjectNameEXT_ptr = GetDelegate (inst, "vkDebugMarkerSetObjectNameEXT");
            vkDebugMarkerSetObjectTagEXT_ptr = GetDelegate (inst, "vkDebugMarkerSetObjectTagEXT");
            vkCmdDebugMarkerBeginEXT_ptr = GetDelegate (inst, "vkCmdDebugMarkerBeginEXT");
            vkCmdDebugMarkerEndEXT_ptr = GetDelegate (inst, "vkCmdDebugMarkerEndEXT");
            vkCmdDebugMarkerInsertEXT_ptr = GetDelegate (inst, "vkCmdDebugMarkerInsertEXT");
            vkGetPhysicalDeviceExternalImageFormatPropertiesNV_ptr = GetDelegate (inst, "vkGetPhysicalDeviceExternalImageFormatPropertiesNV");
            vkGetMemoryWin32HandleNV_ptr = GetDelegate (inst, "vkGetMemoryWin32HandleNV");
            vkCmdDrawIndirectCountAMD_ptr = GetDelegate (inst, "vkCmdDrawIndirectCountAMD");
            vkCmdDrawIndexedIndirectCountAMD_ptr = GetDelegate (inst, "vkCmdDrawIndexedIndirectCountAMD");
            vkCmdProcessCommandsNVX_ptr = GetDelegate (inst, "vkCmdProcessCommandsNVX");
            vkCmdReserveSpaceForCommandsNVX_ptr = GetDelegate (inst, "vkCmdReserveSpaceForCommandsNVX");
            vkCreateIndirectCommandsLayoutNVX_ptr = GetDelegate (inst, "vkCreateIndirectCommandsLayoutNVX");
            vkDestroyIndirectCommandsLayoutNVX_ptr = GetDelegate (inst, "vkDestroyIndirectCommandsLayoutNVX");
            vkCreateObjectTableNVX_ptr = GetDelegate (inst, "vkCreateObjectTableNVX");
            vkDestroyObjectTableNVX_ptr = GetDelegate (inst, "vkDestroyObjectTableNVX");
            vkRegisterObjectsNVX_ptr = GetDelegate (inst, "vkRegisterObjectsNVX");
            vkUnregisterObjectsNVX_ptr = GetDelegate (inst, "vkUnregisterObjectsNVX");
            vkGetPhysicalDeviceGeneratedCommandsPropertiesNVX_ptr = GetDelegate (inst, "vkGetPhysicalDeviceGeneratedCommandsPropertiesNVX");
            vkGetPhysicalDeviceFeatures2KHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceFeatures2KHR");
            vkGetPhysicalDeviceProperties2KHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceProperties2KHR");
            vkGetPhysicalDeviceFormatProperties2KHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceFormatProperties2KHR");
            vkGetPhysicalDeviceImageFormatProperties2KHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceImageFormatProperties2KHR");
            vkGetPhysicalDeviceQueueFamilyProperties2KHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceQueueFamilyProperties2KHR");
            vkGetPhysicalDeviceMemoryProperties2KHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceMemoryProperties2KHR");
            vkGetPhysicalDeviceSparseImageFormatProperties2KHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceSparseImageFormatProperties2KHR");
            vkCmdPushDescriptorSetKHR_ptr = GetDelegate (inst, "vkCmdPushDescriptorSetKHR");
            vkTrimCommandPoolKHR_ptr = GetDelegate (inst, "vkTrimCommandPoolKHR");
            vkGetPhysicalDeviceExternalBufferPropertiesKHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceExternalBufferPropertiesKHR");
            vkGetMemoryWin32HandleKHR_ptr = GetDelegate (inst, "vkGetMemoryWin32HandleKHR");
            vkGetMemoryWin32HandlePropertiesKHR_ptr = GetDelegate (inst, "vkGetMemoryWin32HandlePropertiesKHR");
            vkGetMemoryFdKHR_ptr = GetDelegate (inst, "vkGetMemoryFdKHR");
            vkGetMemoryFdPropertiesKHR_ptr = GetDelegate (inst, "vkGetMemoryFdPropertiesKHR");
            vkGetPhysicalDeviceExternalSemaphorePropertiesKHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceExternalSemaphorePropertiesKHR");
            vkGetSemaphoreWin32HandleKHR_ptr = GetDelegate (inst, "vkGetSemaphoreWin32HandleKHR");
            vkImportSemaphoreWin32HandleKHR_ptr = GetDelegate (inst, "vkImportSemaphoreWin32HandleKHR");
            vkGetSemaphoreFdKHR_ptr = GetDelegate (inst, "vkGetSemaphoreFdKHR");
            vkImportSemaphoreFdKHR_ptr = GetDelegate (inst, "vkImportSemaphoreFdKHR");
            vkGetPhysicalDeviceExternalFencePropertiesKHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceExternalFencePropertiesKHR");
            vkGetFenceWin32HandleKHR_ptr = GetDelegate (inst, "vkGetFenceWin32HandleKHR");
            vkImportFenceWin32HandleKHR_ptr = GetDelegate (inst, "vkImportFenceWin32HandleKHR");
            vkGetFenceFdKHR_ptr = GetDelegate (inst, "vkGetFenceFdKHR");
            vkImportFenceFdKHR_ptr = GetDelegate (inst, "vkImportFenceFdKHR");
            vkReleaseDisplayEXT_ptr = GetDelegate (inst, "vkReleaseDisplayEXT");
            vkAcquireXlibDisplayEXT_ptr = GetDelegate (inst, "vkAcquireXlibDisplayEXT");
            vkGetRandROutputDisplayEXT_ptr = GetDelegate (inst, "vkGetRandROutputDisplayEXT");
            vkDisplayPowerControlEXT_ptr = GetDelegate (inst, "vkDisplayPowerControlEXT");
            vkRegisterDeviceEventEXT_ptr = GetDelegate (inst, "vkRegisterDeviceEventEXT");
            vkRegisterDisplayEventEXT_ptr = GetDelegate (inst, "vkRegisterDisplayEventEXT");
            vkGetSwapchainCounterEXT_ptr = GetDelegate (inst, "vkGetSwapchainCounterEXT");
            vkGetPhysicalDeviceSurfaceCapabilities2EXT_ptr = GetDelegate (inst, "vkGetPhysicalDeviceSurfaceCapabilities2EXT");
            vkEnumeratePhysicalDeviceGroupsKHX_ptr = GetDelegate (inst, "vkEnumeratePhysicalDeviceGroupsKHX");
            vkGetDeviceGroupPeerMemoryFeaturesKHX_ptr = GetDelegate (inst, "vkGetDeviceGroupPeerMemoryFeaturesKHX");
            vkBindBufferMemory2KHR_ptr = GetDelegate (inst, "vkBindBufferMemory2KHR");
            vkBindImageMemory2KHR_ptr = GetDelegate (inst, "vkBindImageMemory2KHR");
            vkCmdSetDeviceMaskKHX_ptr = GetDelegate (inst, "vkCmdSetDeviceMaskKHX");
            vkGetDeviceGroupPresentCapabilitiesKHX_ptr = GetDelegate (inst, "vkGetDeviceGroupPresentCapabilitiesKHX");
            vkGetDeviceGroupSurfacePresentModesKHX_ptr = GetDelegate (inst, "vkGetDeviceGroupSurfacePresentModesKHX");
            vkAcquireNextImage2KHX_ptr = GetDelegate (inst, "vkAcquireNextImage2KHX");
            vkCmdDispatchBaseKHX_ptr = GetDelegate (inst, "vkCmdDispatchBaseKHX");
            vkGetPhysicalDevicePresentRectanglesKHX_ptr = GetDelegate (inst, "vkGetPhysicalDevicePresentRectanglesKHX");
            vkCreateDescriptorUpdateTemplateKHR_ptr = GetDelegate (inst, "vkCreateDescriptorUpdateTemplateKHR");
            vkDestroyDescriptorUpdateTemplateKHR_ptr = GetDelegate (inst, "vkDestroyDescriptorUpdateTemplateKHR");
            vkUpdateDescriptorSetWithTemplateKHR_ptr = GetDelegate (inst, "vkUpdateDescriptorSetWithTemplateKHR");
            vkCmdPushDescriptorSetWithTemplateKHR_ptr = GetDelegate (inst, "vkCmdPushDescriptorSetWithTemplateKHR");
            vkSetHdrMetadataEXT_ptr = GetDelegate (inst, "vkSetHdrMetadataEXT");
            vkGetSwapchainStatusKHR_ptr = GetDelegate (inst, "vkGetSwapchainStatusKHR");
            vkGetRefreshCycleDurationGOOGLE_ptr = GetDelegate (inst, "vkGetRefreshCycleDurationGOOGLE");
            vkGetPastPresentationTimingGOOGLE_ptr = GetDelegate (inst, "vkGetPastPresentationTimingGOOGLE");
            vkCreateIOSSurfaceMVK_ptr = GetDelegate (inst, "vkCreateIOSSurfaceMVK");
            vkCreateMacOSSurfaceMVK_ptr = GetDelegate (inst, "vkCreateMacOSSurfaceMVK");
            vkCmdSetViewportWScalingNV_ptr = GetDelegate (inst, "vkCmdSetViewportWScalingNV");
            vkCmdSetDiscardRectangleEXT_ptr = GetDelegate (inst, "vkCmdSetDiscardRectangleEXT");
            vkCmdSetSampleLocationsEXT_ptr = GetDelegate (inst, "vkCmdSetSampleLocationsEXT");
            vkGetPhysicalDeviceMultisamplePropertiesEXT_ptr = GetDelegate (inst, "vkGetPhysicalDeviceMultisamplePropertiesEXT");
            vkGetPhysicalDeviceSurfaceCapabilities2KHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceSurfaceCapabilities2KHR");
            vkGetPhysicalDeviceSurfaceFormats2KHR_ptr = GetDelegate (inst, "vkGetPhysicalDeviceSurfaceFormats2KHR");
            vkGetBufferMemoryRequirements2KHR_ptr = GetDelegate (inst, "vkGetBufferMemoryRequirements2KHR");
            vkGetImageMemoryRequirements2KHR_ptr = GetDelegate (inst, "vkGetImageMemoryRequirements2KHR");
            vkGetImageSparseMemoryRequirements2KHR_ptr = GetDelegate (inst, "vkGetImageSparseMemoryRequirements2KHR");
            vkCreateSamplerYcbcrConversionKHR_ptr = GetDelegate (inst, "vkCreateSamplerYcbcrConversionKHR");
            vkDestroySamplerYcbcrConversionKHR_ptr = GetDelegate (inst, "vkDestroySamplerYcbcrConversionKHR");
            vkCreateValidationCacheEXT_ptr = GetDelegate (inst, "vkCreateValidationCacheEXT");
            vkDestroyValidationCacheEXT_ptr = GetDelegate (inst, "vkDestroyValidationCacheEXT");
            vkGetValidationCacheDataEXT_ptr = GetDelegate (inst, "vkGetValidationCacheDataEXT");
            vkMergeValidationCachesEXT_ptr = GetDelegate (inst, "vkMergeValidationCachesEXT");
            vkGetSwapchainGrallocUsageANDROID_ptr = GetDelegate (inst, "vkGetSwapchainGrallocUsageANDROID");
            vkAcquireImageANDROID_ptr = GetDelegate (inst, "vkAcquireImageANDROID");
            vkQueueSignalReleaseImageANDROID_ptr = GetDelegate (inst, "vkQueueSignalReleaseImageANDROID");
            vkGetShaderInfoAMD_ptr = GetDelegate (inst, "vkGetShaderInfoAMD");
            vkGetMemoryHostPointerPropertiesEXT_ptr = GetDelegate (inst, "vkGetMemoryHostPointerPropertiesEXT");
		}
        private static void LoadFunctionPointers()
        {
            vkCreateInstance_ptr = s_nativeLib.LoadFunctionPointer("vkCreateInstance");
            vkDestroyInstance_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyInstance");
            vkGetDeviceProcAddr_ptr = s_nativeLib.LoadFunctionPointer("vkGetDeviceProcAddr");
            vkGetInstanceProcAddr_ptr = s_nativeLib.LoadFunctionPointer("vkGetInstanceProcAddr");            
        }
    }
}
