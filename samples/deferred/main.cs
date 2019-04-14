using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Glfw;
using VKE;
using VK;
using static VKE.Camera;
using Buffer = VKE.Buffer;

namespace ModelSample {
	class Program : VkWindow {
		static void Main (string[] args) {
			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}

	}
}
