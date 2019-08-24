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
		CVKL.DebugUtils.Messenger msg;

		Image img;

		Program () : base () {
			msg = new CVKL.DebugUtils.Messenger (instance);

			dbgReport = new DebugReport (instance,
				VkDebugReportFlagsEXT.DebugEXT |
				VkDebugReportFlagsEXT.ErrorEXT |
				VkDebugReportFlagsEXT.WarningEXT |
				VkDebugReportFlagsEXT.PerformanceWarningEXT |
				VkDebugReportFlagsEXT.InformationEXT);


			CommandBuffer cmd = cmdPool.AllocateAndStart (VkCommandBufferUsageFlags.OneTimeSubmit);

			using (StbImage stbi = new StbImage ("data/textures/texture.jpg")) {

				img = new Image (dev, VkFormat.R8g8b8a8Unorm, VkImageUsageFlags.TransferSrc | VkImageUsageFlags.TransferDst,
					VkMemoryPropertyFlags.DeviceLocal, (uint)stbi.Width, (uint)stbi.Height, VkImageType.Image2D,
					VkSampleCountFlags.SampleCount1, VkImageTiling.Optimal, Image.ComputeMipLevels (stbi.Width, stbi.Height));

				using (HostBuffer stag = new HostBuffer (dev, VkBufferUsageFlags.TransferSrc, (ulong)stbi.Size, stbi.Handle)) {

					stag.CopyTo (cmd, img, VkImageLayout.TransferSrcOptimal);
					img.BuildMipmaps (cmd);

					cmd.End ();
					presentQueue.Submit (cmd);
					presentQueue.WaitIdle ();
					cmd.Free ();

				}
			}


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
