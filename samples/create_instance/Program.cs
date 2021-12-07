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
#if AUTO_SET_STYPE
			Console.WriteLine ("AUTO_SET_STYPE: ON");
			using (VkApplicationInfo ai = new VkApplicationInfo()) {
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
