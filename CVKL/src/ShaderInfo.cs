// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using VK;

namespace CVKL {
	public class ShaderInfo : IDisposable {
		public VkShaderStageFlags StageFlags;
		public string SpirvPath;
		public FixedUtf8String EntryPoint;
		public SpecializationInfo SpecializationInfo;

		public ShaderInfo (VkShaderStageFlags _stageFlags, string _spirvPath, SpecializationInfo specializationInfo = null, string _entryPoint = "main") {
			StageFlags = _stageFlags;
			SpirvPath = _spirvPath;
			EntryPoint = new FixedUtf8String (_entryPoint);
			this.SpecializationInfo = specializationInfo;
		}

		public VkPipelineShaderStageCreateInfo GetStageCreateInfo (Device dev) {
			return new VkPipelineShaderStageCreateInfo {
				sType = VkStructureType.PipelineShaderStageCreateInfo,
				stage = StageFlags,
				pName = EntryPoint,
				module = dev.LoadSPIRVShader (SpirvPath),
				pSpecializationInfo = (SpecializationInfo == null) ? IntPtr.Zero : SpecializationInfo.InfosPtr
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
