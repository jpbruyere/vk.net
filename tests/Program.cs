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

			VkInstanceCreateInfo ci = VkInstanceCreateInfo.New();
			VkApplicationInfo ai = VkApplicationInfo.New();
			ai.apiVersion = new Version (1,0,0);
			ai.applicationVersion = new Version (1,0,0);
			ai.pApplicationName = "new application name";
			ai.pEngineName = "new engine name";


			VkClearColorValue clear = new VkClearColorValue(255,0,0);
			clear.floats[2] = 210;

			using (PinnedObjects pi = new PinnedObjects()) {
				ci.pApplicationInfo = ai;

				VkResult res = vkCreateInstance (ref ci, IntPtr.Zero, out instance);
				if (res != VkResult.Success) {
					Console.WriteLine ($"Error: {res}");
					return;
				}else
					Console.WriteLine ($"Success: {res}");

			}

			vkDestroyInstance (instance, IntPtr.Zero);
		}
	}
}
