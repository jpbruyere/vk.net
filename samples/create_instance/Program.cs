// Copyright (c) 2024  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using Vulkan;
using static Vulkan.Vk;

namespace tests
{
	class Program
	{

		static void Main(string[] args)
		{
			VkInstance instance;
			Vulkan.Version v = new Vulkan.Version(1,0,0);
#if AUTO_SET_STYPE
			Console.WriteLine ("AUTO_SET_STYPE: ON");
			using (VkApplicationInfo ai = new VkApplicationInfo(v, v, v)) {
				using (VkInstanceCreateInfo ci = new VkInstanceCreateInfo {pApplicationInfo = ai}){
					Console.WriteLine (vkCreateInstance (ci, IntPtr.Zero, out instance));
				}
			}
#else
			Console.WriteLine ("AUTO_SET_STYPE: OFF");
			using (VkApplicationInfo ai = VkApplicationInfo.New) {
				VkInstanceCreateInfo ci = VkInstanceCreateInfo.New;
				ci.pApplicationInfo = ai;
				Console.WriteLine (vkCreateInstance (ci, IntPtr.Zero, out instance));
				ci.Dispose();
			}
#endif
			vkDestroyInstance (instance, IntPtr.Zero);
		}
	}
}
