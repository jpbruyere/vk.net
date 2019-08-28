using System;
using CVKL;
using VK;
using static VK.Vk;

namespace tests {
	class Program : VkWindow {
		static void Main (string[] args) {
			Instance.VALIDATION = true;
			Instance.DEBUG_UTILS = true;
			using (Program vke = new Program ()) {
				vke.Run ();
			}
		}


		Program () : base () {


		}


		protected override void Dispose (bool disposing) {
			if (disposing) {
				if (!isDisposed) {
				}
			}
			base.Dispose ();
		}
	}
}
