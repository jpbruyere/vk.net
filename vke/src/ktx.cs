using System;
using System.IO;

using Vulkan;
using VKE;

namespace KTX {

	public class KtxException : Exception {
		public KtxException (string message) : base (message) { }
	}

	public class KTX {
		static byte[] ktxSignature = { 0xAB, 0x4B, 0x54, 0x58, 0x20, 0x31, 0x31, 0xBB, 0x0D, 0x0A, 0x1A, 0x0A };

		public static Image Load (CommandBuffer cmd, string ktxPath, VkImageUsageFlags usage = VkImageUsageFlags.Sampled,
			VkMemoryPropertyFlags memoryProperty = VkMemoryPropertyFlags.DeviceLocal, bool generateMipmaps = true,
			VkImageTiling tiling = VkImageTiling.Optimal) {
			Image img = null;

			using (Stream ktxStream = File.Open (ktxPath, FileMode.Open, FileAccess.Read)) {
				using (BinaryReader br = new BinaryReader (ktxStream)) {
					if (!br.ReadBytes (12).AreEquals (ktxSignature))
						throw new KtxException ("Not a ktx file: " + ktxPath);

					UInt32 endianness = br.ReadUInt32 ();
					UInt32 glType = br.ReadUInt32 ();
					UInt32 glTypeSize = br.ReadUInt32 ();
					UInt32 glFormat = br.ReadUInt32 ();
					UInt32 glInternalFormat = br.ReadUInt32 ();
					UInt32 glBaseInternalFormat = br.ReadUInt32 ();
					UInt32 pixelWidth = br.ReadUInt32 ();
					UInt32 pixelHeight = br.ReadUInt32 ();
					UInt32 pixelDepth = Math.Min (1, br.ReadUInt32 ());
					UInt32 numberOfArrayElements = Math.Min (1, br.ReadUInt32 ());
					UInt32 numberOfFaces = br.ReadUInt32 ();
					UInt32 numberOfMipmapLevels = Math.Min (1, br.ReadUInt32 ());
					UInt32 bytesOfKeyValueData = br.ReadUInt32 ();

					uint requestedMipsLevels = numberOfMipmapLevels;
					if (numberOfMipmapLevels == 1)
						requestedMipsLevels = generateMipmaps ? (uint)Math.Floor (Math.Log (Math.Max (pixelWidth, pixelHeight))) + 1 : 1;


					VkFormat vkFormat = GLHelper.vkGetFormatFromOpenGLInternalFormat (glInternalFormat);
					if (vkFormat == VkFormat.Undefined) {
						vkFormat = GLHelper.vkGetFormatFromOpenGLFormat (glFormat, glType);
						if (vkFormat == VkFormat.Undefined)
							throw new KtxException ("Undefined format: " + ktxPath);
					}

					VkImageType imgType = VkImageType.Image2D;

					img = new Image (cmd.Device, vkFormat, usage, memoryProperty, pixelWidth, pixelHeight, imgType, VkSampleCountFlags.Count1,
						tiling, requestedMipsLevels, numberOfArrayElements, pixelDepth);


					byte[] keyValueDatas = br.ReadBytes ((int)bytesOfKeyValueData);


					if (memoryProperty.HasFlag (VkMemoryPropertyFlags.DeviceLocal)) {



						using (HostBuffer<byte> stagging = new HostBuffer<byte> (cmd.Device, VkBufferUsageFlags.TransferSrc, br.ReadBytes ((int)img.AllocatedDeviceMemorySize))) { 
						}


							VkImageSubresourceRange mipSubRange = new VkImageSubresourceRange (VkImageAspectFlags.Color, 0, 1, 0, numberOfArrayElements);

						for (int mips = 0; mips < numberOfMipmapLevels; mips++) {
							UInt32 imgSize = br.ReadUInt32 ();
							/*VkImageBlit imageBlit = new VkImageBlit {
								srcSubresource = new VkImageSubresourceLayers(VkImageAspectFlags.Color, numberOfArrayElements, (uint)mips - 1),
								srcOffsets_1 = new VkOffset3D((int)pixelWidth >> (mips - 1), (int)pixelHeight >> (mips - 1),1),
								dstSubresource = new VkImageSubresourceLayers (VkImageAspectFlags.Color, numberOfArrayElements, (uint)mips),
								dstOffsets_1 = new VkOffset3D ((int)pixelWidth >> mips, (int)pixelHeight >> mips, 1),
							};*/

							for (int layer = 0; layer < numberOfArrayElements; layer++) {
								for (int face = 0; face < numberOfFaces; face++) {
									for (int slice = 0; slice < pixelDepth; slice++) {
										/*for (int y = 0; y < pixelHeight; y++) {
											for (int x = 0; x < pixelWidth; x++) {
												//Uncompressed texture data matches a GL_UNPACK_ALIGNMENT of 4.

											}

										}*/
									}
									//Byte cubePadding[0-3]
								}
							}
							//Byte mipPadding[0-3]
						}
						if (requestedMipsLevels > numberOfMipmapLevels)
							img.BuildMipmaps (cmd);
					}
				}
			}

			return img;
		}
	}
}


/*VkFormatFeatureFlags phyFormatSupport = (tiling == VkImageTiling.Linear) ?
	dev.phy.GetFormatProperties (vkFormat).linearTilingFeatures :
	dev.phy.GetFormatProperties (vkFormat).optimalTilingFeatures;

VkFormatFeatureFlags requiredFlags = VkFormatFeatureFlags.None;
if (usage.HasFlag (VkImageUsageFlags.ColorAttachment))
	requiredFlags |= VkFormatFeatureFlags.ColorAttachment;

if (!phyFormatSupport.HasFlag (requiredFlags))
	throw new KtxException ("Unsupported format: " + ktxPath);*/