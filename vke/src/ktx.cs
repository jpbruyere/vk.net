//
// ktx.cs
//
// Author:
//       Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// Copyright (c) 2019 Jean-Philippe Bruyère
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
using System.IO;

using VK;
using CVKL;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace KTX {

	public class KtxException : Exception {
		public KtxException (string message) : base (message) { }
	}

	public class KTX {
		static byte[] ktxSignature = { 0xAB, 0x4B, 0x54, 0x58, 0x20, 0x31, 0x31, 0xBB, 0x0D, 0x0A, 0x1A, 0x0A };

		public static Image Load (Queue staggingQ, CommandPool staggingCmdPool, string ktxPath, VkImageUsageFlags usage = VkImageUsageFlags.Sampled,
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
					UInt32 pixelDepth = Math.Max (1, br.ReadUInt32 ());
					UInt32 numberOfArrayElements = br.ReadUInt32 ();//only for array text, else 0
					UInt32 numberOfFaces = br.ReadUInt32 ();//only for cube map, else 1
					UInt32 numberOfMipmapLevels = Math.Max (1, br.ReadUInt32 ());
					UInt32 bytesOfKeyValueData = br.ReadUInt32 ();
											
					VkFormat vkFormat = GLHelper.vkGetFormatFromOpenGLInternalFormat (glInternalFormat);
					if (vkFormat == VkFormat.Undefined) {
						vkFormat = GLHelper.vkGetFormatFromOpenGLFormat (glFormat, glType);
						if (vkFormat == VkFormat.Undefined)
							throw new KtxException ("Undefined format: " + ktxPath);
					}
					VkFormatProperties formatProperties = staggingQ.Dev.phy.GetFormatProperties (vkFormat);
					VkFormatFeatureFlags phyFormatSupport = (tiling == VkImageTiling.Linear) ?
						formatProperties.linearTilingFeatures :
						formatProperties.optimalTilingFeatures;

					uint requestedMipsLevels = numberOfMipmapLevels;
					if (numberOfMipmapLevels == 1)
						requestedMipsLevels = (generateMipmaps && phyFormatSupport.HasFlag (VkFormatFeatureFlags.BlitSrc | VkFormatFeatureFlags.BlitDst)) ?
							(uint)Math.Floor (Math.Log (Math.Max (pixelWidth, pixelHeight))) + 1 : 1 ;
							
					if (tiling == VkImageTiling.Optimal)
						usage |= VkImageUsageFlags.TransferDst;
					if (generateMipmaps)
						usage |= (VkImageUsageFlags.TransferSrc | VkImageUsageFlags.TransferDst);

					VkImageCreateFlags createFlags = 0;

					VkImageType imgType =
						(pixelWidth == 0) ? throw new KtxException ("pixelWidth must be > 0") :
						(pixelHeight == 0) ? imgType = VkImageType.Image1D :
						(pixelDepth == 1) ? imgType = VkImageType.Image2D : imgType = VkImageType.Image3D;
						

					VkSampleCountFlags samples = VkSampleCountFlags.SampleCount1;

					if (numberOfFaces > 1) {
						if (imgType != VkImageType.Image2D)
							throw new KtxException ("cubemap faces must be 2D textures");
						createFlags = VkImageCreateFlags.CubeCompatible;
						samples = VkSampleCountFlags.SampleCount1;
						numberOfArrayElements = numberOfFaces;
					} else {
						numberOfFaces = 1;
						if (numberOfArrayElements == 0)
							numberOfArrayElements = 1;
					}

					if (!Image.CheckFormatIsSupported (usage, phyFormatSupport))
						throw new Exception ($"Unsupported image format: {vkFormat}, {tiling}, {usage}");

					img = new Image (staggingQ.Dev, vkFormat, usage, memoryProperty, pixelWidth, pixelHeight, imgType, samples,
						tiling, requestedMipsLevels, numberOfArrayElements, pixelDepth, createFlags);
						
					byte[] keyValueDatas = br.ReadBytes ((int)bytesOfKeyValueData);


					if (memoryProperty.HasFlag (VkMemoryPropertyFlags.DeviceLocal)) {
						ulong staggingSize = (ulong)ktxStream.Length;//img.AllocatedDeviceMemorySize;

						using (HostBuffer stagging = new HostBuffer (staggingQ.Dev, VkBufferUsageFlags.TransferSrc, staggingSize)) {
							stagging.Map ();

							CommandBuffer cmd = staggingCmdPool.AllocateAndStart (VkCommandBufferUsageFlags.OneTimeSubmit);
							img.SetLayout (cmd, VkImageAspectFlags.Color,
								VkImageLayout.Undefined, VkImageLayout.TransferDstOptimal,
								VkPipelineStageFlags.AllCommands, VkPipelineStageFlags.Transfer);

							List<VkBufferImageCopy> buffCopies = new List<VkBufferImageCopy> ();

							VkBufferImageCopy bufferCopyRegion = new VkBufferImageCopy {
								imageExtent = img.CreateInfo.extent,
								imageSubresource = new VkImageSubresourceLayers (VkImageAspectFlags.Color, img.CreateInfo.arrayLayers, 0)
							};

							ulong bufferOffset = 0;
							uint imgWidth = img.CreateInfo.extent.width;
							uint imgHeight = img.CreateInfo.extent.height;

							for (int mips = 0; mips < numberOfMipmapLevels; mips++) {
								UInt32 imgSize = br.ReadUInt32 ();

								bufferCopyRegion.bufferImageHeight = imgHeight;
								bufferCopyRegion.bufferRowLength = imgWidth;
								bufferCopyRegion.bufferOffset = bufferOffset;
								bufferCopyRegion.imageSubresource.mipLevel = (uint)mips;
								bufferCopyRegion.imageExtent.width = imgWidth;
								bufferCopyRegion.imageExtent.height = imgHeight;

								if (createFlags.HasFlag (VkImageCreateFlags.CubeCompatible)) {
									for (uint face = 0; face < numberOfFaces; face++) {
										Marshal.Copy (br.ReadBytes ((int)imgSize), 0, stagging.MappedData + (int)bufferOffset, (int)imgSize);
										uint faceOffset = imgSize + (imgSize % 4);//cube padding																				  
										bufferOffset += faceOffset;
									}
									buffCopies.Add (bufferCopyRegion);
									bufferCopyRegion.bufferOffset = bufferOffset;
								} else {
									Marshal.Copy (br.ReadBytes ((int)imgSize), 0, stagging.MappedData + (int)bufferOffset, (int)imgSize);
									buffCopies.Add (bufferCopyRegion);
									bufferOffset += imgSize;
								}

								imgWidth /= 2;
								imgHeight /= 2;
							}
							stagging.Unmap ();

							Vk.vkCmdCopyBufferToImage (cmd.Handle, stagging.handle, img.handle, VkImageLayout.TransferDstOptimal,
								(uint)buffCopies.Count, buffCopies.Pin());
							buffCopies.Unpin ();

							if (requestedMipsLevels > numberOfMipmapLevels)
								img.BuildMipmaps (cmd);								
							else
								img.SetLayout (cmd, VkImageAspectFlags.Color,
									VkImageLayout.TransferDstOptimal, VkImageLayout.ShaderReadOnlyOptimal,
									VkPipelineStageFlags.Transfer, VkPipelineStageFlags.TopOfPipe);

							cmd.End ();

							staggingQ.Submit (cmd);
							staggingQ.WaitIdle ();

							cmd.Free ();

						}
					} else { 
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