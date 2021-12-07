// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Vulkan;

namespace samples {
	public static class Utils {
		public static void CheckResult (VkResult result, string msg = "Call failed",
			[CallerMemberName] string caller = null,
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0) {
			if (result != VkResult.Success)
				throw new InvalidOperationException ($"[{sourceFilePath}:{sourceLineNumber}->{caller}]{msg}: {result}");
		}
	}
}
