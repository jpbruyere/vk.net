//
// SpecializationConstant.cs
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
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using VK;

using static VK.Vk;

namespace CVKL {
	/// <summary>
	/// Activable holding the pipeline cache handle. Activation is triggered by usage, so disposing pipelines that use this
	/// cache is enough to have the cache disposed correctly. 
	/// </summary>
	/// <remarks>
	/// Restore and Saving of the cache may be controled through the two static members:
	/// 	- `SaveOnDispose`
	/// 	- `LoadOnActivation`
	/// </remarks>
	public sealed class PipelineCache : Activable {
		/// <summary>
		/// If true, cache will be saved on dispose
		/// </summary>
		public static bool SaveOnDispose;
		/// <summary>
		/// If true, cache will be restore on activation
		/// </summary>
		public static bool LoadOnActivation;

		internal VkPipelineCache handle;
		readonly string globalConfigPath;
		readonly string cacheFile;

		#region CTOR
		public PipelineCache (Device dev, string cacheFile = "pipelines.bin", string name = "pipeline cache") : base(dev, name) {
			string configRoot = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.UserProfile), ".config");
			string appName = Assembly.GetEntryAssembly ().GetName ().Name;
			globalConfigPath = Path.Combine (configRoot, appName);

			if (!Directory.Exists (globalConfigPath))
				Directory.CreateDirectory (globalConfigPath);

			this.cacheFile = cacheFile;
        }
		#endregion

		public override void Activate () {
			string path = Path.Combine (globalConfigPath, cacheFile);

			if (state != ActivableState.Activated) {
				VkPipelineCacheCreateInfo info = VkPipelineCacheCreateInfo.New ();							

				if (File.Exists (path) && LoadOnActivation) {
					using (FileStream fs = File.Open (path, FileMode.Open)) {
						using (BinaryReader br = new BinaryReader (fs)) {
							int length = (int)br.BaseStream.Length;
							info.pInitialData = Marshal.AllocHGlobal (length);
							info.initialDataSize = (UIntPtr)br.BaseStream.Length;
							Marshal.Copy(br.ReadBytes (length),0, info.pInitialData, length);
						}
					}
				}

				Utils.CheckResult (vkCreatePipelineCache (Dev.VkDev, ref info, IntPtr.Zero, out handle));

				if (info.pInitialData != IntPtr.Zero)
					Marshal.FreeHGlobal (info.pInitialData);
			}
			base.Activate ();
		}

		protected override VkDebugMarkerObjectNameInfoEXT DebugMarkerInfo => throw new NotImplementedException ();

		public void Delete () {
			string path = Path.Combine (globalConfigPath, cacheFile);
			if (File.Exists (path))
				File.Delete (path);
		}

		public void Save () {
			if (state != ActivableState.Activated)
				return;

			string path = Path.Combine (globalConfigPath, cacheFile);

			if (File.Exists (path))
				File.Delete (path);

			UIntPtr dataSize;
			Utils.CheckResult (vkGetPipelineCacheData (Dev.VkDev, handle, out dataSize, IntPtr.Zero));
			byte[] pData = new byte[(int)dataSize];
			Utils.CheckResult (vkGetPipelineCacheData (Dev.VkDev, handle, out dataSize, pData.Pin ()));
			pData.Unpin ();

			using (FileStream fs = File.Open (path, FileMode.CreateNew)) 
				using (BinaryWriter br = new BinaryWriter (fs)) 
					br.Write (pData, 0, (int)dataSize);			
		}


		#region IDisposable Support
		protected override void Dispose (bool disposing) {
			if (!disposing)
				System.Diagnostics.Debug.WriteLine ($"CVKL PipelineCache '{name}' disposed by finalizer");
			if (state == ActivableState.Activated) {
				if (SaveOnDispose)
					Save ();
				vkDestroyPipelineCache (Dev.VkDev, handle, IntPtr.Zero);
			}
			base.Dispose (disposing);
		}
		#endregion

	}
}
