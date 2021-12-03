using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Vulkan;
using static Vulkan.Vk;
using Version = Vulkan.Version;

namespace tests
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
	public struct NewStuff
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)Vulkan.Vk.UuidSize)]
		public StringBuilder name;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4*16)]
		public float[,] calibrate;

		[MarshalAs(UnmanagedType.R4)]
		public float DMTI;

		[MarshalAs(UnmanagedType.R4)]
		public float DMTII;

		// Etc
	}

	class Program
	{

		static void Main(string[] args)
		{
			VkInstance instance;

			VkInstanceCreateInfo ci = VkInstanceCreateInfo.New();
			VkApplicationInfo ai = VkApplicationInfo.New();
			//ai.apiVersion = new Version (1,0,0);
			//ai.applicationVersion = new Version (1,0,0);


			VkClearColorValue clear = new VkClearColorValue(255,0,0);
			clear.floats[2] = 210;

			using (PinnedObjects pi = new PinnedObjects()) {
				ci.pApplicationInfo = ai.Pin(pi);

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
