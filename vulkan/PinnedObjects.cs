// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Vulkan {
	/// <summary>
	/// Keep pinned object gc handles and free them on dispose.
	/// </summary>
	public class PinnedObjects : IDisposable {
		public readonly List<GCHandle> Handles = new List<GCHandle> ();

		public void Dispose () {
			for (int i = Handles.Count - 1; i >= 0; i--)
				Handles[i].Free ();
			GC.SuppressFinalize (this);
		}
	}
}
