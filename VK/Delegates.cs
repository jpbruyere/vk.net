/* credit to https://github.com/mellinoe */
using System;

namespace VK
{
    public unsafe delegate void* PFN_vkAllocationFunction(
         void* pUserData,
         UIntPtr size,
         UIntPtr alignment,
         VkSystemAllocationScope allocationScope);

    public unsafe delegate void* PFN_vkReallocationFunction(
         void* pUserData,
         void* pOriginal,
         UIntPtr size,
         UIntPtr alignment,
         VkSystemAllocationScope allocationScope);

    public unsafe delegate void PFN_vkFreeFunction(
         void* pUserData,
         void* pMemory);

    public unsafe delegate void PFN_vkInternalAllocationNotification(
         void* pUserData,
         UIntPtr size,
         VkInternalAllocationType allocationType,
         VkSystemAllocationScope allocationScope);

    public unsafe delegate void PFN_vkInternalFreeNotification(
         void* pUserData,
         UIntPtr size,
         VkInternalAllocationType allocationType,
         VkSystemAllocationScope allocationScope);

    public unsafe delegate void PFN_vkVoidFunction();

    public unsafe delegate VkBool32 PFN_vkDebugReportCallbackEXT(
        VkDebugReportFlagsEXT flags,
        VkDebugReportObjectTypeEXT objectType,
        ulong @object,
        UIntPtr location,
        int messageCode,
        IntPtr pLayerPrefix,
        IntPtr pMessage,
        IntPtr pUserData);

	public unsafe delegate VkBool32 PFN_vkDebugUtilsMessengerCallbackEXT (
		VkDebugUtilsMessageSeverityFlagsEXT messageSeverity,
		VkDebugUtilsMessageTypeFlagsEXT messageTypes,
		IntPtr pCallbackData,
		IntPtr pUserData);
}
