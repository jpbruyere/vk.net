// Copyright (c) 2017 Eric Mellino
// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;

namespace Vulkan
{
    public delegate IntPtr PFN_vkAllocationFunction(
         IntPtr pUserData,
         UIntPtr size,
         UIntPtr alignment,
         VkSystemAllocationScope allocationScope);

    public delegate IntPtr PFN_vkReallocationFunction(
         IntPtr pUserData,
         IntPtr pOriginal,
         UIntPtr size,
         UIntPtr alignment,
         VkSystemAllocationScope allocationScope);

    public delegate void PFN_vkFreeFunction(
         IntPtr pUserData,
         IntPtr pMemory);

    public delegate void PFN_vkInternalAllocationNotification(
         IntPtr pUserData,
         UIntPtr size,
         VkInternalAllocationType allocationType,
         VkSystemAllocationScope allocationScope);

    public delegate void PFN_vkInternalFreeNotification(
         IntPtr pUserData,
         UIntPtr size,
         VkInternalAllocationType allocationType,
         VkSystemAllocationScope allocationScope);

    public delegate void PFN_vkVoidFunction();

    public delegate VkBool32 PFN_vkDebugReportCallbackEXT(
        VkDebugReportFlagsEXT flags,
        VkDebugReportObjectTypeEXT objectType,
        ulong @object,
        UIntPtr location,
        int messageCode,
        IntPtr pLayerPrefix,
        IntPtr pMessage,
        IntPtr pUserData);

	public delegate VkBool32 PFN_vkDebugUtilsMessengerCallbackEXT (
		VkDebugUtilsMessageSeverityFlagsEXT messageSeverity,
		VkDebugUtilsMessageTypeFlagsEXT messageTypes,
		IntPtr pCallbackData,
		IntPtr pUserData);
}
