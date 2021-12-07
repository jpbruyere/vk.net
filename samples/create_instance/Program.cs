using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Vulkan;
using static Vulkan.Vk;
using Version = Vulkan.Version;

namespace tests
{
	class Program
	{

		static void Main(string[] args)
		{
			VkInstance instance;

			using (VkApplicationInfo ai = new VkApplicationInfo()) {
				using (VkInstanceCreateInfo ci = new VkInstanceCreateInfo {pApplicationInfo = ai}){
					Console.WriteLine (vkCreateInstance (ci, IntPtr.Zero, out instance));
				}
			}

			vkDestroyInstance (instance, IntPtr.Zero);
		}
	}
}
