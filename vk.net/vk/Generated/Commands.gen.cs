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

        private static IntPtr vkCmdBeginRenderPass_ptr;

        [Generator.CalliRewrite]
        public static unsafe void vkCmdBeginRenderPass(VkCommandBuffer commandBuffer, ref VkRenderPassBeginInfo pRenderPassBegin, VkSubpassContents contents)
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
        public static unsafe void vkCmdDebugMarkerBeginEXT(VkCommandBuffer commandBuffer, IntPtr pMarkerInfo)
        {
            throw new NotImplementedException();
        }

        private static IntPtr vkCmdDebugMarkerEndEXT_ptr;

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

        private static void LoadFunctionPointers()
        {
            vkCreateInstance_ptr = s_nativeLib.LoadFunctionPointer("vkCreateInstance");
            vkDestroyInstance_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyInstance");
            vkEnumeratePhysicalDevices_ptr = s_nativeLib.LoadFunctionPointer("vkEnumeratePhysicalDevices");
            vkGetDeviceProcAddr_ptr = s_nativeLib.LoadFunctionPointer("vkGetDeviceProcAddr");
            vkGetInstanceProcAddr_ptr = s_nativeLib.LoadFunctionPointer("vkGetInstanceProcAddr");
            vkGetPhysicalDeviceProperties_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceProperties");
            vkGetPhysicalDeviceQueueFamilyProperties_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceQueueFamilyProperties");
            vkGetPhysicalDeviceMemoryProperties_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceMemoryProperties");
            vkGetPhysicalDeviceFeatures_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceFeatures");
            vkGetPhysicalDeviceFormatProperties_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceFormatProperties");
            vkGetPhysicalDeviceImageFormatProperties_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceImageFormatProperties");
            vkCreateDevice_ptr = s_nativeLib.LoadFunctionPointer("vkCreateDevice");
            vkDestroyDevice_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyDevice");
            vkEnumerateInstanceLayerProperties_ptr = s_nativeLib.LoadFunctionPointer("vkEnumerateInstanceLayerProperties");
            vkEnumerateInstanceExtensionProperties_ptr = s_nativeLib.LoadFunctionPointer("vkEnumerateInstanceExtensionProperties");
            vkEnumerateDeviceLayerProperties_ptr = s_nativeLib.LoadFunctionPointer("vkEnumerateDeviceLayerProperties");
            vkEnumerateDeviceExtensionProperties_ptr = s_nativeLib.LoadFunctionPointer("vkEnumerateDeviceExtensionProperties");
            vkGetDeviceQueue_ptr = s_nativeLib.LoadFunctionPointer("vkGetDeviceQueue");
            vkQueueSubmit_ptr = s_nativeLib.LoadFunctionPointer("vkQueueSubmit");
            vkQueueWaitIdle_ptr = s_nativeLib.LoadFunctionPointer("vkQueueWaitIdle");
            vkDeviceWaitIdle_ptr = s_nativeLib.LoadFunctionPointer("vkDeviceWaitIdle");
            vkAllocateMemory_ptr = s_nativeLib.LoadFunctionPointer("vkAllocateMemory");
            vkFreeMemory_ptr = s_nativeLib.LoadFunctionPointer("vkFreeMemory");
            vkMapMemory_ptr = s_nativeLib.LoadFunctionPointer("vkMapMemory");
            vkUnmapMemory_ptr = s_nativeLib.LoadFunctionPointer("vkUnmapMemory");
            vkFlushMappedMemoryRanges_ptr = s_nativeLib.LoadFunctionPointer("vkFlushMappedMemoryRanges");
            vkInvalidateMappedMemoryRanges_ptr = s_nativeLib.LoadFunctionPointer("vkInvalidateMappedMemoryRanges");
            vkGetDeviceMemoryCommitment_ptr = s_nativeLib.LoadFunctionPointer("vkGetDeviceMemoryCommitment");
            vkGetBufferMemoryRequirements_ptr = s_nativeLib.LoadFunctionPointer("vkGetBufferMemoryRequirements");
            vkBindBufferMemory_ptr = s_nativeLib.LoadFunctionPointer("vkBindBufferMemory");
            vkGetImageMemoryRequirements_ptr = s_nativeLib.LoadFunctionPointer("vkGetImageMemoryRequirements");
            vkBindImageMemory_ptr = s_nativeLib.LoadFunctionPointer("vkBindImageMemory");
            vkGetImageSparseMemoryRequirements_ptr = s_nativeLib.LoadFunctionPointer("vkGetImageSparseMemoryRequirements");
            vkGetPhysicalDeviceSparseImageFormatProperties_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceSparseImageFormatProperties");
            vkQueueBindSparse_ptr = s_nativeLib.LoadFunctionPointer("vkQueueBindSparse");
            vkCreateFence_ptr = s_nativeLib.LoadFunctionPointer("vkCreateFence");
            vkDestroyFence_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyFence");
            vkResetFences_ptr = s_nativeLib.LoadFunctionPointer("vkResetFences");
            vkGetFenceStatus_ptr = s_nativeLib.LoadFunctionPointer("vkGetFenceStatus");
            vkWaitForFences_ptr = s_nativeLib.LoadFunctionPointer("vkWaitForFences");
            vkCreateSemaphore_ptr = s_nativeLib.LoadFunctionPointer("vkCreateSemaphore");
            vkDestroySemaphore_ptr = s_nativeLib.LoadFunctionPointer("vkDestroySemaphore");
            vkCreateEvent_ptr = s_nativeLib.LoadFunctionPointer("vkCreateEvent");
            vkDestroyEvent_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyEvent");
            vkGetEventStatus_ptr = s_nativeLib.LoadFunctionPointer("vkGetEventStatus");
            vkSetEvent_ptr = s_nativeLib.LoadFunctionPointer("vkSetEvent");
            vkResetEvent_ptr = s_nativeLib.LoadFunctionPointer("vkResetEvent");
            vkCreateQueryPool_ptr = s_nativeLib.LoadFunctionPointer("vkCreateQueryPool");
            vkDestroyQueryPool_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyQueryPool");
            vkGetQueryPoolResults_ptr = s_nativeLib.LoadFunctionPointer("vkGetQueryPoolResults");
            vkCreateBuffer_ptr = s_nativeLib.LoadFunctionPointer("vkCreateBuffer");
            vkDestroyBuffer_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyBuffer");
            vkCreateBufferView_ptr = s_nativeLib.LoadFunctionPointer("vkCreateBufferView");
            vkDestroyBufferView_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyBufferView");
            vkCreateImage_ptr = s_nativeLib.LoadFunctionPointer("vkCreateImage");
            vkDestroyImage_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyImage");
            vkGetImageSubresourceLayout_ptr = s_nativeLib.LoadFunctionPointer("vkGetImageSubresourceLayout");
            vkCreateImageView_ptr = s_nativeLib.LoadFunctionPointer("vkCreateImageView");
            vkDestroyImageView_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyImageView");
            vkCreateShaderModule_ptr = s_nativeLib.LoadFunctionPointer("vkCreateShaderModule");
            vkDestroyShaderModule_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyShaderModule");
            vkCreatePipelineCache_ptr = s_nativeLib.LoadFunctionPointer("vkCreatePipelineCache");
            vkDestroyPipelineCache_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyPipelineCache");
            vkGetPipelineCacheData_ptr = s_nativeLib.LoadFunctionPointer("vkGetPipelineCacheData");
            vkMergePipelineCaches_ptr = s_nativeLib.LoadFunctionPointer("vkMergePipelineCaches");
            vkCreateGraphicsPipelines_ptr = s_nativeLib.LoadFunctionPointer("vkCreateGraphicsPipelines");
            vkCreateComputePipelines_ptr = s_nativeLib.LoadFunctionPointer("vkCreateComputePipelines");
            vkDestroyPipeline_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyPipeline");
            vkCreatePipelineLayout_ptr = s_nativeLib.LoadFunctionPointer("vkCreatePipelineLayout");
            vkDestroyPipelineLayout_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyPipelineLayout");
            vkCreateSampler_ptr = s_nativeLib.LoadFunctionPointer("vkCreateSampler");
            vkDestroySampler_ptr = s_nativeLib.LoadFunctionPointer("vkDestroySampler");
            vkCreateDescriptorSetLayout_ptr = s_nativeLib.LoadFunctionPointer("vkCreateDescriptorSetLayout");
            vkDestroyDescriptorSetLayout_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyDescriptorSetLayout");
            vkCreateDescriptorPool_ptr = s_nativeLib.LoadFunctionPointer("vkCreateDescriptorPool");
            vkDestroyDescriptorPool_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyDescriptorPool");
            vkResetDescriptorPool_ptr = s_nativeLib.LoadFunctionPointer("vkResetDescriptorPool");
            vkAllocateDescriptorSets_ptr = s_nativeLib.LoadFunctionPointer("vkAllocateDescriptorSets");
            vkFreeDescriptorSets_ptr = s_nativeLib.LoadFunctionPointer("vkFreeDescriptorSets");
            vkUpdateDescriptorSets_ptr = s_nativeLib.LoadFunctionPointer("vkUpdateDescriptorSets");
            vkCreateFramebuffer_ptr = s_nativeLib.LoadFunctionPointer("vkCreateFramebuffer");
            vkDestroyFramebuffer_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyFramebuffer");
            vkCreateRenderPass_ptr = s_nativeLib.LoadFunctionPointer("vkCreateRenderPass");
            vkDestroyRenderPass_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyRenderPass");
            vkGetRenderAreaGranularity_ptr = s_nativeLib.LoadFunctionPointer("vkGetRenderAreaGranularity");
            vkCreateCommandPool_ptr = s_nativeLib.LoadFunctionPointer("vkCreateCommandPool");
            vkDestroyCommandPool_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyCommandPool");
            vkResetCommandPool_ptr = s_nativeLib.LoadFunctionPointer("vkResetCommandPool");
            vkAllocateCommandBuffers_ptr = s_nativeLib.LoadFunctionPointer("vkAllocateCommandBuffers");
            vkFreeCommandBuffers_ptr = s_nativeLib.LoadFunctionPointer("vkFreeCommandBuffers");
            vkBeginCommandBuffer_ptr = s_nativeLib.LoadFunctionPointer("vkBeginCommandBuffer");
            vkEndCommandBuffer_ptr = s_nativeLib.LoadFunctionPointer("vkEndCommandBuffer");
            vkResetCommandBuffer_ptr = s_nativeLib.LoadFunctionPointer("vkResetCommandBuffer");
            vkCmdBindPipeline_ptr = s_nativeLib.LoadFunctionPointer("vkCmdBindPipeline");
            vkCmdSetViewport_ptr = s_nativeLib.LoadFunctionPointer("vkCmdSetViewport");
            vkCmdSetScissor_ptr = s_nativeLib.LoadFunctionPointer("vkCmdSetScissor");
            vkCmdSetLineWidth_ptr = s_nativeLib.LoadFunctionPointer("vkCmdSetLineWidth");
            vkCmdSetDepthBias_ptr = s_nativeLib.LoadFunctionPointer("vkCmdSetDepthBias");
            vkCmdSetBlendConstants_ptr = s_nativeLib.LoadFunctionPointer("vkCmdSetBlendConstants");
            vkCmdSetDepthBounds_ptr = s_nativeLib.LoadFunctionPointer("vkCmdSetDepthBounds");
            vkCmdSetStencilCompareMask_ptr = s_nativeLib.LoadFunctionPointer("vkCmdSetStencilCompareMask");
            vkCmdSetStencilWriteMask_ptr = s_nativeLib.LoadFunctionPointer("vkCmdSetStencilWriteMask");
            vkCmdSetStencilReference_ptr = s_nativeLib.LoadFunctionPointer("vkCmdSetStencilReference");
            vkCmdBindDescriptorSets_ptr = s_nativeLib.LoadFunctionPointer("vkCmdBindDescriptorSets");
            vkCmdBindIndexBuffer_ptr = s_nativeLib.LoadFunctionPointer("vkCmdBindIndexBuffer");
            vkCmdBindVertexBuffers_ptr = s_nativeLib.LoadFunctionPointer("vkCmdBindVertexBuffers");
            vkCmdDraw_ptr = s_nativeLib.LoadFunctionPointer("vkCmdDraw");
            vkCmdDrawIndexed_ptr = s_nativeLib.LoadFunctionPointer("vkCmdDrawIndexed");
            vkCmdDrawIndirect_ptr = s_nativeLib.LoadFunctionPointer("vkCmdDrawIndirect");
            vkCmdDrawIndexedIndirect_ptr = s_nativeLib.LoadFunctionPointer("vkCmdDrawIndexedIndirect");
            vkCmdDispatch_ptr = s_nativeLib.LoadFunctionPointer("vkCmdDispatch");
            vkCmdDispatchIndirect_ptr = s_nativeLib.LoadFunctionPointer("vkCmdDispatchIndirect");
            vkCmdCopyBuffer_ptr = s_nativeLib.LoadFunctionPointer("vkCmdCopyBuffer");
            vkCmdCopyImage_ptr = s_nativeLib.LoadFunctionPointer("vkCmdCopyImage");
            vkCmdBlitImage_ptr = s_nativeLib.LoadFunctionPointer("vkCmdBlitImage");
            vkCmdCopyBufferToImage_ptr = s_nativeLib.LoadFunctionPointer("vkCmdCopyBufferToImage");
            vkCmdCopyImageToBuffer_ptr = s_nativeLib.LoadFunctionPointer("vkCmdCopyImageToBuffer");
            vkCmdUpdateBuffer_ptr = s_nativeLib.LoadFunctionPointer("vkCmdUpdateBuffer");
            vkCmdFillBuffer_ptr = s_nativeLib.LoadFunctionPointer("vkCmdFillBuffer");
            vkCmdClearColorImage_ptr = s_nativeLib.LoadFunctionPointer("vkCmdClearColorImage");
            vkCmdClearDepthStencilImage_ptr = s_nativeLib.LoadFunctionPointer("vkCmdClearDepthStencilImage");
            vkCmdClearAttachments_ptr = s_nativeLib.LoadFunctionPointer("vkCmdClearAttachments");
            vkCmdResolveImage_ptr = s_nativeLib.LoadFunctionPointer("vkCmdResolveImage");
            vkCmdSetEvent_ptr = s_nativeLib.LoadFunctionPointer("vkCmdSetEvent");
            vkCmdResetEvent_ptr = s_nativeLib.LoadFunctionPointer("vkCmdResetEvent");
            vkCmdWaitEvents_ptr = s_nativeLib.LoadFunctionPointer("vkCmdWaitEvents");
            vkCmdPipelineBarrier_ptr = s_nativeLib.LoadFunctionPointer("vkCmdPipelineBarrier");
            vkCmdBeginQuery_ptr = s_nativeLib.LoadFunctionPointer("vkCmdBeginQuery");
            vkCmdEndQuery_ptr = s_nativeLib.LoadFunctionPointer("vkCmdEndQuery");
            vkCmdResetQueryPool_ptr = s_nativeLib.LoadFunctionPointer("vkCmdResetQueryPool");
            vkCmdWriteTimestamp_ptr = s_nativeLib.LoadFunctionPointer("vkCmdWriteTimestamp");
            vkCmdCopyQueryPoolResults_ptr = s_nativeLib.LoadFunctionPointer("vkCmdCopyQueryPoolResults");
            vkCmdPushConstants_ptr = s_nativeLib.LoadFunctionPointer("vkCmdPushConstants");
            vkCmdBeginRenderPass_ptr = s_nativeLib.LoadFunctionPointer("vkCmdBeginRenderPass");
            vkCmdNextSubpass_ptr = s_nativeLib.LoadFunctionPointer("vkCmdNextSubpass");
            vkCmdEndRenderPass_ptr = s_nativeLib.LoadFunctionPointer("vkCmdEndRenderPass");
            vkCmdExecuteCommands_ptr = s_nativeLib.LoadFunctionPointer("vkCmdExecuteCommands");
            vkCreateAndroidSurfaceKHR_ptr = s_nativeLib.LoadFunctionPointer("vkCreateAndroidSurfaceKHR");
            vkGetPhysicalDeviceDisplayPropertiesKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceDisplayPropertiesKHR");
            vkGetPhysicalDeviceDisplayPlanePropertiesKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceDisplayPlanePropertiesKHR");
            vkGetDisplayPlaneSupportedDisplaysKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetDisplayPlaneSupportedDisplaysKHR");
            vkGetDisplayModePropertiesKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetDisplayModePropertiesKHR");
            vkCreateDisplayModeKHR_ptr = s_nativeLib.LoadFunctionPointer("vkCreateDisplayModeKHR");
            vkGetDisplayPlaneCapabilitiesKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetDisplayPlaneCapabilitiesKHR");
            vkCreateDisplayPlaneSurfaceKHR_ptr = s_nativeLib.LoadFunctionPointer("vkCreateDisplayPlaneSurfaceKHR");
            vkCreateSharedSwapchainsKHR_ptr = s_nativeLib.LoadFunctionPointer("vkCreateSharedSwapchainsKHR");
            vkCreateMirSurfaceKHR_ptr = s_nativeLib.LoadFunctionPointer("vkCreateMirSurfaceKHR");
            vkGetPhysicalDeviceMirPresentationSupportKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceMirPresentationSupportKHR");
            vkDestroySurfaceKHR_ptr = s_nativeLib.LoadFunctionPointer("vkDestroySurfaceKHR");
            vkGetPhysicalDeviceSurfaceSupportKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceSurfaceSupportKHR");
            vkGetPhysicalDeviceSurfaceCapabilitiesKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceSurfaceCapabilitiesKHR");
            vkGetPhysicalDeviceSurfaceFormatsKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceSurfaceFormatsKHR");
            vkGetPhysicalDeviceSurfacePresentModesKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceSurfacePresentModesKHR");
            vkCreateSwapchainKHR_ptr = s_nativeLib.LoadFunctionPointer("vkCreateSwapchainKHR");
            vkDestroySwapchainKHR_ptr = s_nativeLib.LoadFunctionPointer("vkDestroySwapchainKHR");
            vkGetSwapchainImagesKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetSwapchainImagesKHR");
            vkAcquireNextImageKHR_ptr = s_nativeLib.LoadFunctionPointer("vkAcquireNextImageKHR");
            vkQueuePresentKHR_ptr = s_nativeLib.LoadFunctionPointer("vkQueuePresentKHR");
            vkCreateViSurfaceNN_ptr = s_nativeLib.LoadFunctionPointer("vkCreateViSurfaceNN");
            vkCreateWaylandSurfaceKHR_ptr = s_nativeLib.LoadFunctionPointer("vkCreateWaylandSurfaceKHR");
            vkGetPhysicalDeviceWaylandPresentationSupportKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceWaylandPresentationSupportKHR");
            vkCreateWin32SurfaceKHR_ptr = s_nativeLib.LoadFunctionPointer("vkCreateWin32SurfaceKHR");
            vkGetPhysicalDeviceWin32PresentationSupportKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceWin32PresentationSupportKHR");
            vkCreateXlibSurfaceKHR_ptr = s_nativeLib.LoadFunctionPointer("vkCreateXlibSurfaceKHR");
            vkGetPhysicalDeviceXlibPresentationSupportKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceXlibPresentationSupportKHR");
            vkCreateXcbSurfaceKHR_ptr = s_nativeLib.LoadFunctionPointer("vkCreateXcbSurfaceKHR");
            vkGetPhysicalDeviceXcbPresentationSupportKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceXcbPresentationSupportKHR");
            vkCreateDebugReportCallbackEXT_ptr = s_nativeLib.LoadFunctionPointer("vkCreateDebugReportCallbackEXT");
            vkDestroyDebugReportCallbackEXT_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyDebugReportCallbackEXT");
            vkDebugReportMessageEXT_ptr = s_nativeLib.LoadFunctionPointer("vkDebugReportMessageEXT");
            vkDebugMarkerSetObjectNameEXT_ptr = s_nativeLib.LoadFunctionPointer("vkDebugMarkerSetObjectNameEXT");
            vkDebugMarkerSetObjectTagEXT_ptr = s_nativeLib.LoadFunctionPointer("vkDebugMarkerSetObjectTagEXT");
            vkCmdDebugMarkerBeginEXT_ptr = s_nativeLib.LoadFunctionPointer("vkCmdDebugMarkerBeginEXT");
            vkCmdDebugMarkerEndEXT_ptr = s_nativeLib.LoadFunctionPointer("vkCmdDebugMarkerEndEXT");
            vkCmdDebugMarkerInsertEXT_ptr = s_nativeLib.LoadFunctionPointer("vkCmdDebugMarkerInsertEXT");
            vkGetPhysicalDeviceExternalImageFormatPropertiesNV_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceExternalImageFormatPropertiesNV");
            vkGetMemoryWin32HandleNV_ptr = s_nativeLib.LoadFunctionPointer("vkGetMemoryWin32HandleNV");
            vkCmdDrawIndirectCountAMD_ptr = s_nativeLib.LoadFunctionPointer("vkCmdDrawIndirectCountAMD");
            vkCmdDrawIndexedIndirectCountAMD_ptr = s_nativeLib.LoadFunctionPointer("vkCmdDrawIndexedIndirectCountAMD");
            vkCmdProcessCommandsNVX_ptr = s_nativeLib.LoadFunctionPointer("vkCmdProcessCommandsNVX");
            vkCmdReserveSpaceForCommandsNVX_ptr = s_nativeLib.LoadFunctionPointer("vkCmdReserveSpaceForCommandsNVX");
            vkCreateIndirectCommandsLayoutNVX_ptr = s_nativeLib.LoadFunctionPointer("vkCreateIndirectCommandsLayoutNVX");
            vkDestroyIndirectCommandsLayoutNVX_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyIndirectCommandsLayoutNVX");
            vkCreateObjectTableNVX_ptr = s_nativeLib.LoadFunctionPointer("vkCreateObjectTableNVX");
            vkDestroyObjectTableNVX_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyObjectTableNVX");
            vkRegisterObjectsNVX_ptr = s_nativeLib.LoadFunctionPointer("vkRegisterObjectsNVX");
            vkUnregisterObjectsNVX_ptr = s_nativeLib.LoadFunctionPointer("vkUnregisterObjectsNVX");
            vkGetPhysicalDeviceGeneratedCommandsPropertiesNVX_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceGeneratedCommandsPropertiesNVX");
            vkGetPhysicalDeviceFeatures2KHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceFeatures2KHR");
            vkGetPhysicalDeviceProperties2KHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceProperties2KHR");
            vkGetPhysicalDeviceFormatProperties2KHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceFormatProperties2KHR");
            vkGetPhysicalDeviceImageFormatProperties2KHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceImageFormatProperties2KHR");
            vkGetPhysicalDeviceQueueFamilyProperties2KHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceQueueFamilyProperties2KHR");
            vkGetPhysicalDeviceMemoryProperties2KHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceMemoryProperties2KHR");
            vkGetPhysicalDeviceSparseImageFormatProperties2KHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceSparseImageFormatProperties2KHR");
            vkCmdPushDescriptorSetKHR_ptr = s_nativeLib.LoadFunctionPointer("vkCmdPushDescriptorSetKHR");
            vkTrimCommandPoolKHR_ptr = s_nativeLib.LoadFunctionPointer("vkTrimCommandPoolKHR");
            vkGetPhysicalDeviceExternalBufferPropertiesKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceExternalBufferPropertiesKHR");
            vkGetMemoryWin32HandleKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetMemoryWin32HandleKHR");
            vkGetMemoryWin32HandlePropertiesKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetMemoryWin32HandlePropertiesKHR");
            vkGetMemoryFdKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetMemoryFdKHR");
            vkGetMemoryFdPropertiesKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetMemoryFdPropertiesKHR");
            vkGetPhysicalDeviceExternalSemaphorePropertiesKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceExternalSemaphorePropertiesKHR");
            vkGetSemaphoreWin32HandleKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetSemaphoreWin32HandleKHR");
            vkImportSemaphoreWin32HandleKHR_ptr = s_nativeLib.LoadFunctionPointer("vkImportSemaphoreWin32HandleKHR");
            vkGetSemaphoreFdKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetSemaphoreFdKHR");
            vkImportSemaphoreFdKHR_ptr = s_nativeLib.LoadFunctionPointer("vkImportSemaphoreFdKHR");
            vkGetPhysicalDeviceExternalFencePropertiesKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceExternalFencePropertiesKHR");
            vkGetFenceWin32HandleKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetFenceWin32HandleKHR");
            vkImportFenceWin32HandleKHR_ptr = s_nativeLib.LoadFunctionPointer("vkImportFenceWin32HandleKHR");
            vkGetFenceFdKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetFenceFdKHR");
            vkImportFenceFdKHR_ptr = s_nativeLib.LoadFunctionPointer("vkImportFenceFdKHR");
            vkReleaseDisplayEXT_ptr = s_nativeLib.LoadFunctionPointer("vkReleaseDisplayEXT");
            vkAcquireXlibDisplayEXT_ptr = s_nativeLib.LoadFunctionPointer("vkAcquireXlibDisplayEXT");
            vkGetRandROutputDisplayEXT_ptr = s_nativeLib.LoadFunctionPointer("vkGetRandROutputDisplayEXT");
            vkDisplayPowerControlEXT_ptr = s_nativeLib.LoadFunctionPointer("vkDisplayPowerControlEXT");
            vkRegisterDeviceEventEXT_ptr = s_nativeLib.LoadFunctionPointer("vkRegisterDeviceEventEXT");
            vkRegisterDisplayEventEXT_ptr = s_nativeLib.LoadFunctionPointer("vkRegisterDisplayEventEXT");
            vkGetSwapchainCounterEXT_ptr = s_nativeLib.LoadFunctionPointer("vkGetSwapchainCounterEXT");
            vkGetPhysicalDeviceSurfaceCapabilities2EXT_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceSurfaceCapabilities2EXT");
            vkEnumeratePhysicalDeviceGroupsKHX_ptr = s_nativeLib.LoadFunctionPointer("vkEnumeratePhysicalDeviceGroupsKHX");
            vkGetDeviceGroupPeerMemoryFeaturesKHX_ptr = s_nativeLib.LoadFunctionPointer("vkGetDeviceGroupPeerMemoryFeaturesKHX");
            vkBindBufferMemory2KHR_ptr = s_nativeLib.LoadFunctionPointer("vkBindBufferMemory2KHR");
            vkBindImageMemory2KHR_ptr = s_nativeLib.LoadFunctionPointer("vkBindImageMemory2KHR");
            vkCmdSetDeviceMaskKHX_ptr = s_nativeLib.LoadFunctionPointer("vkCmdSetDeviceMaskKHX");
            vkGetDeviceGroupPresentCapabilitiesKHX_ptr = s_nativeLib.LoadFunctionPointer("vkGetDeviceGroupPresentCapabilitiesKHX");
            vkGetDeviceGroupSurfacePresentModesKHX_ptr = s_nativeLib.LoadFunctionPointer("vkGetDeviceGroupSurfacePresentModesKHX");
            vkAcquireNextImage2KHX_ptr = s_nativeLib.LoadFunctionPointer("vkAcquireNextImage2KHX");
            vkCmdDispatchBaseKHX_ptr = s_nativeLib.LoadFunctionPointer("vkCmdDispatchBaseKHX");
            vkGetPhysicalDevicePresentRectanglesKHX_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDevicePresentRectanglesKHX");
            vkCreateDescriptorUpdateTemplateKHR_ptr = s_nativeLib.LoadFunctionPointer("vkCreateDescriptorUpdateTemplateKHR");
            vkDestroyDescriptorUpdateTemplateKHR_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyDescriptorUpdateTemplateKHR");
            vkUpdateDescriptorSetWithTemplateKHR_ptr = s_nativeLib.LoadFunctionPointer("vkUpdateDescriptorSetWithTemplateKHR");
            vkCmdPushDescriptorSetWithTemplateKHR_ptr = s_nativeLib.LoadFunctionPointer("vkCmdPushDescriptorSetWithTemplateKHR");
            vkSetHdrMetadataEXT_ptr = s_nativeLib.LoadFunctionPointer("vkSetHdrMetadataEXT");
            vkGetSwapchainStatusKHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetSwapchainStatusKHR");
            vkGetRefreshCycleDurationGOOGLE_ptr = s_nativeLib.LoadFunctionPointer("vkGetRefreshCycleDurationGOOGLE");
            vkGetPastPresentationTimingGOOGLE_ptr = s_nativeLib.LoadFunctionPointer("vkGetPastPresentationTimingGOOGLE");
            vkCreateIOSSurfaceMVK_ptr = s_nativeLib.LoadFunctionPointer("vkCreateIOSSurfaceMVK");
            vkCreateMacOSSurfaceMVK_ptr = s_nativeLib.LoadFunctionPointer("vkCreateMacOSSurfaceMVK");
            vkCmdSetViewportWScalingNV_ptr = s_nativeLib.LoadFunctionPointer("vkCmdSetViewportWScalingNV");
            vkCmdSetDiscardRectangleEXT_ptr = s_nativeLib.LoadFunctionPointer("vkCmdSetDiscardRectangleEXT");
            vkCmdSetSampleLocationsEXT_ptr = s_nativeLib.LoadFunctionPointer("vkCmdSetSampleLocationsEXT");
            vkGetPhysicalDeviceMultisamplePropertiesEXT_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceMultisamplePropertiesEXT");
            vkGetPhysicalDeviceSurfaceCapabilities2KHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceSurfaceCapabilities2KHR");
            vkGetPhysicalDeviceSurfaceFormats2KHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetPhysicalDeviceSurfaceFormats2KHR");
            vkGetBufferMemoryRequirements2KHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetBufferMemoryRequirements2KHR");
            vkGetImageMemoryRequirements2KHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetImageMemoryRequirements2KHR");
            vkGetImageSparseMemoryRequirements2KHR_ptr = s_nativeLib.LoadFunctionPointer("vkGetImageSparseMemoryRequirements2KHR");
            vkCreateSamplerYcbcrConversionKHR_ptr = s_nativeLib.LoadFunctionPointer("vkCreateSamplerYcbcrConversionKHR");
            vkDestroySamplerYcbcrConversionKHR_ptr = s_nativeLib.LoadFunctionPointer("vkDestroySamplerYcbcrConversionKHR");
            vkCreateValidationCacheEXT_ptr = s_nativeLib.LoadFunctionPointer("vkCreateValidationCacheEXT");
            vkDestroyValidationCacheEXT_ptr = s_nativeLib.LoadFunctionPointer("vkDestroyValidationCacheEXT");
            vkGetValidationCacheDataEXT_ptr = s_nativeLib.LoadFunctionPointer("vkGetValidationCacheDataEXT");
            vkMergeValidationCachesEXT_ptr = s_nativeLib.LoadFunctionPointer("vkMergeValidationCachesEXT");
            vkGetSwapchainGrallocUsageANDROID_ptr = s_nativeLib.LoadFunctionPointer("vkGetSwapchainGrallocUsageANDROID");
            vkAcquireImageANDROID_ptr = s_nativeLib.LoadFunctionPointer("vkAcquireImageANDROID");
            vkQueueSignalReleaseImageANDROID_ptr = s_nativeLib.LoadFunctionPointer("vkQueueSignalReleaseImageANDROID");
            vkGetShaderInfoAMD_ptr = s_nativeLib.LoadFunctionPointer("vkGetShaderInfoAMD");
            vkGetMemoryHostPointerPropertiesEXT_ptr = s_nativeLib.LoadFunctionPointer("vkGetMemoryHostPointerPropertiesEXT");
        }
    }
}
