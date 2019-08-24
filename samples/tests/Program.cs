/* Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
 *
 * This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
 *
 * port of the 'High dynamic range rendering' sample from
 * Copyright by Sascha Willems - www.saschawillems.de
 * This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
*/

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

		DebugReport dbgReport;
		Image img;

		Program () : base () {
			dbgReport = new DebugReport (instance,
				VkDebugReportFlagsEXT.DebugEXT |
				VkDebugReportFlagsEXT.ErrorEXT |
				VkDebugReportFlagsEXT.WarningEXT |
				VkDebugReportFlagsEXT.PerformanceWarningEXT |
				VkDebugReportFlagsEXT.InformationEXT);
		}


		protected override void Dispose (bool disposing) {
			if (disposing) {
				if (!isDisposed) {
					dbgReport?.Dispose ();
				}
			}
			base.Dispose ();
		}
	}
}
