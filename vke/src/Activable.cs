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
using VK;
using static VK.Vk;

namespace CVKL {
	/// <summary>
	/// Tristate status of activables, reflecting vulkan openrations
	/// </summary>
	public enum ActivableState {
		/// <summary>
		/// Class has been instanced, but no vulkan handle was created.
		/// </summary>
		Init,
		/// <summary>
		/// On the first activation, vulkan handle is created and ref count is one. Further activations will increment the reference count by one.
		/// </summary>
		Activated,
		/// <summary>
		/// Reference count is zero, handles have been destroyed. Such object may be reactivated.
		/// </summary>
		Disposed
	};
	/// <summary>
	/// Base class for most of the vulkan device's objects following the IDispose pattern. Each time an activable is used, it's reference count is incremented, and
	/// each time it is disposed, the count is decremented. When the count reach zero, the handle is destroyed and the finalizizer is unregistered. Once disposed, the
	/// objecte may still be reactivated.
	/// </summary>
	/// <remarks>
	/// Some of the activation trigger the first activation on creation, those activables have to be explicitly dispose at the end of the application.
	/// The activables that trigger activation only on usage does not require an additional dispose at the end.
	/// </remarks>
	public abstract class Activable : IDisposable {
		//count number of activation, only the first one will create a handle 
		protected uint references;
		//keep track of the current state of activation.
		protected ActivableState state;
		//With the debug marker extension, setting name to vulkan's object ease the debugging.
		protected string name;
		/// <summary>
		/// This property has to be implemented in every vulkan object. It should return the correct debug marker info.
		/// </summary>
		/// <value>The debug marker info.</value>
		protected abstract VkDebugMarkerObjectNameInfoEXT DebugMarkerInfo { get; }
		/// <summary>
		/// Vulkan logical device this activable is bound to.
		/// </summary>
		public Device Dev { get; private set; }

		#region CTOR
		protected Activable (Device dev) {
			this.Dev = dev;
			this.name = GetType ().Name;
		}
		protected Activable (Device dev, string name) {
			this.Dev = dev;
			this.name = name;
		}
		#endregion

		/// <summary>
		/// if debug marker extension is activated, this will set the name for debuggers
		/// </summary>
		public void SetName (string name) {
			this.name = name;

			if (!Dev.debugMarkersEnabled)
				return;

			VkDebugMarkerObjectNameInfoEXT dmo = DebugMarkerInfo;
			dmo.pObjectName = name.Pin();
			Utils.CheckResult (vkDebugMarkerSetObjectNameEXT (Dev.VkDev, ref dmo));
			name.Unpin ();			
		}
		/// <summary>
		/// Activation of the object, the reference count is incremented.
		/// </summary>
		public virtual void Activate () {
			references++;
			if (state == ActivableState.Activated)
				return;
			if (state == ActivableState.Disposed) 
				GC.ReRegisterForFinalize (this);
			state = ActivableState.Activated;
			SetName (name);
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
