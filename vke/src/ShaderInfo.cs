//
// FrameBuffer.cs
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

namespace VKE {
	public class ShaderInfo : IDisposable {
		public VkShaderStageFlags StageFlags;
		public string SpirvPath;
		public FixedUtf8String EntryPoint;

		public ShaderInfo (VkShaderStageFlags _stageFlags, string _spirvPath, string _entryPoint = "main") {
			StageFlags = _stageFlags;
			SpirvPath = _spirvPath;
			EntryPoint = new FixedUtf8String (_entryPoint);
		}

		public VkPipelineShaderStageCreateInfo GetStageCreateInfo (Device dev) {
			return new VkPipelineShaderStageCreateInfo {
				sType = VkStructureType.PipelineShaderStageCreateInfo,
				stage = StageFlags,
				pName = EntryPoint,
				module = dev.LoadSPIRVShader (SpirvPath),
			};			
		}

		#region IDisposable Support
		private bool disposedValue = false; // Pour détecter les appels redondants

		protected virtual void Dispose (bool disposing) {
			if (!disposedValue) {
				if (disposing)
					EntryPoint.Dispose ();

				disposedValue = true;
			}
		}
		public void Dispose () {
			Dispose (true);
		}
		#endregion
	}
}
