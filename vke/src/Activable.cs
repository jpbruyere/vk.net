//
// Activable.cs
//
// Author:
//       Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// Copyright (c) 2019 jp
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Linq;
using VK;
using static VK.Vk;

namespace VKE {
	public enum ActivableState { Init, Activated, Disposed };
	public abstract class Activable : IDisposable { 
		protected uint references;
		protected ActivableState state;
		protected string name;

#if DEBUG && DEBUG_MARKER
		protected abstract VkDebugMarkerObjectNameInfoEXT DebugMarkerInfo { get; }
#endif

		public Device dev { get; private set; }

		#region CTOR
		protected Activable (Device dev) {
			this.dev = dev;
			this.name = GetType ().Name;
		}
		protected Activable (Device dev, string name) {
			this.dev = dev;
			this.name = name;
		}
		#endregion

		public void SetName (string name) {
			this.name = name;

#if DEBUG && DEBUG_MARKER
			VkDebugMarkerObjectNameInfoEXT dmo = DebugMarkerInfo;
			dmo.pObjectName = name.Pin();
			Utils.CheckResult (vkDebugMarkerSetObjectNameEXT (dev.VkDev, ref dmo));
			name.Unpin ();			
#endif
		}
		public virtual void Activate () {
			if (state == ActivableState.Disposed) 
				GC.ReRegisterForFinalize (this);
			state = ActivableState.Activated;
			references++;
#if DEBUG && DEBUG_MARKER
			SetName (this.name);
#endif
		}

		public override string ToString () {
			return name;
		}

#region IDisposable Support
		protected virtual void Dispose (bool disposing) {
			state = ActivableState.Disposed;
		}

		~Activable() {
			Dispose(false);
		}
		public void Dispose () {
			if (references>0)
				references--;
			if (references>0)
				return;
			Dispose (true);
			GC.SuppressFinalize(this);
		}
#endregion
	}
}
