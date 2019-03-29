using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using static Vulkan.VulkanNative;
namespace Vulkan
{
    public static class BindingsHelpers
    {
        public static unsafe StringHandle StringToHGlobalUtf8(string s)
        {
            Debug.Assert(s != null);
            int byteCount = Encoding.UTF8.GetByteCount(s);
            IntPtr retPtr = Marshal.AllocHGlobal(byteCount);
            fixed (char* stringPtr = s)
            {
                Encoding.UTF8.GetBytes(stringPtr, s.Length, (byte*)retPtr.ToPointer(), byteCount);
            }

            return new StringHandle() { Handle = retPtr };
        }

        public static void FreeHGlobal(StringHandle ptr)
        {
            Marshal.FreeHGlobal(ptr.Handle);
        }
    }

    public struct StringHandle
    {
        public IntPtr Handle;
    }
    public partial struct VkQueue
    {
        unsafe public VkResult submit (VkCommandBuffer cmd, VkSemaphore waitSemaphore, VkSemaphore signalSemaphore, VkFence waitFence)
        {
            VkSubmitInfo submitInfo = VkSubmitInfo.New ();
            VkPipelineStageFlags waitStageMask = VkPipelineStageFlags.ColorAttachmentOutput;
            submitInfo.pWaitDstStageMask = &waitStageMask;
            var pcs = waitSemaphore;
            submitInfo.pWaitSemaphores = &pcs;
            submitInfo.waitSemaphoreCount = 1;
            var rcs = signalSemaphore;
            submitInfo.pSignalSemaphores = &rcs;
            submitInfo.signalSemaphoreCount = 1;
            var cmdBuf = &cmd;
            submitInfo.pCommandBuffers = cmdBuf;
            submitInfo.commandBufferCount = 1;

            // Submit to the graphics queue passing a wait fence
            return vkQueueSubmit (this, 1, ref submitInfo, waitFence);

        }
    }
}
