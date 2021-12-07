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

			using (VkApplicationInfo ai = new VkApplicationInfo(
				new Version (1,0,0),
				new Version (1,0,0),
				new Version (1,2,0)) {
					pApplicationName = "new application name"
				}){


				VkClearColorValue clear = new VkClearColorValue(255,0,0);
				clear.floats[2] = 210;
				using (VkInstanceCreateInfo ci = new VkInstanceCreateInfo() {
						pApplicationInfo = ai}){

					VkResult res = vkCreateInstance (ci, IntPtr.Zero, out instance);
					if (res != VkResult.Success) {
						Console.WriteLine ($"Error: {res}");
						return;
					}else
						Console.WriteLine ($"Success: {res}");

				}
			}

			vkDestroyInstance (instance, IntPtr.Zero);
		}
	}
}
