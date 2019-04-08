//
// Utils.cs
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
using System.IO;
using System.Numerics;
using VK;

namespace Vulkan {
	[Flags]
	public enum VkFormatSizeFlag {
		SizePacked				= 0x00000001,
		SizeCompressed			= 0x00000002,
		SizePalettized			= 0x00000004,
		SizeDepth				= 0x00000008,
		SizeStencil				= 0x00000010,
	};
	
	public struct VkFormatSize {
		public VkFormatSizeFlag		flags;
		public uint		paletteSizeInBits;
		public uint		blockSizeInBits;
		public uint		blockWidth;			// in texels
		public uint		blockHeight;		// in texels
		public uint		blockDepth;			// in texels
	};


    public static partial class Utils {        
		public static void vkGetFormatSize(VkFormat format, out VkFormatSize pFormatSize )
		{
		    switch ( format )
		    {
		        case VkFormat.R4g4UnormPack8:
		            pFormatSize.flags = VkFormatSizeFlag.SizePacked;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 1 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.R4g4b4a4UnormPack16:
		        case VkFormat.B4g4r4a4UnormPack16:
		        case VkFormat.R5g6b5UnormPack16:
		        case VkFormat.B5g6r5UnormPack16:
		        case VkFormat.R5g5b5a1UnormPack16:
		        case VkFormat.B5g5r5a1UnormPack16:
		        case VkFormat.A1r5g5b5UnormPack16:
		            pFormatSize.flags = VkFormatSizeFlag.SizePacked;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 2 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.R8Unorm:
		        case VkFormat.R8Snorm:
		        case VkFormat.R8Uscaled:
		        case VkFormat.R8Sscaled:
		        case VkFormat.R8Uint:
		        case VkFormat.R8Sint:
		        case VkFormat.R8Srgb:
		            pFormatSize.flags = 0;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 1 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.R8g8Unorm:
		        case VkFormat.R8g8Snorm:
		        case VkFormat.R8g8Uscaled:
		        case VkFormat.R8g8Sscaled:
		        case VkFormat.R8g8Uint:
		        case VkFormat.R8g8Sint:
		        case VkFormat.R8g8Srgb:
		            pFormatSize.flags = 0;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 2 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.R8g8b8Unorm:
		        case VkFormat.R8g8b8Snorm:
		        case VkFormat.R8g8b8Uscaled:
		        case VkFormat.R8g8b8Sscaled:
		        case VkFormat.R8g8b8Uint:
		        case VkFormat.R8g8b8Sint:
		        case VkFormat.R8g8b8Srgb:
		        case VkFormat.B8g8r8Unorm:
		        case VkFormat.B8g8r8Snorm:
		        case VkFormat.B8g8r8Uscaled:
		        case VkFormat.B8g8r8Sscaled:
		        case VkFormat.B8g8r8Uint:
		        case VkFormat.B8g8r8Sint:
		        case VkFormat.B8g8r8Srgb:
		            pFormatSize.flags = 0;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 3 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.R8g8b8a8Unorm:
		        case VkFormat.R8g8b8a8Snorm:
		        case VkFormat.R8g8b8a8Uscaled:
		        case VkFormat.R8g8b8a8Sscaled:
		        case VkFormat.R8g8b8a8Uint:
		        case VkFormat.R8g8b8a8Sint:
		        case VkFormat.R8g8b8a8Srgb:
		        case VkFormat.B8g8r8a8Unorm:
		        case VkFormat.B8g8r8a8Snorm:
		        case VkFormat.B8g8r8a8Uscaled:
		        case VkFormat.B8g8r8a8Sscaled:
		        case VkFormat.B8g8r8a8Uint:
		        case VkFormat.B8g8r8a8Sint:
		        case VkFormat.B8g8r8a8Srgb:
		            pFormatSize.flags = 0;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 4 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.A8b8g8r8UnormPack32:
		        case VkFormat.A8b8g8r8SnormPack32:
		        case VkFormat.A8b8g8r8UscaledPack32:
		        case VkFormat.A8b8g8r8SscaledPack32:
		        case VkFormat.A8b8g8r8UintPack32:
		        case VkFormat.A8b8g8r8SintPack32:
		        case VkFormat.A8b8g8r8SrgbPack32:
		            pFormatSize.flags = VkFormatSizeFlag.SizePacked;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 4 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.A2r10g10b10UnormPack32:
		        case VkFormat.A2r10g10b10SnormPack32:
		        case VkFormat.A2r10g10b10UscaledPack32:
		        case VkFormat.A2r10g10b10SscaledPack32:
		        case VkFormat.A2r10g10b10UintPack32:
		        case VkFormat.A2r10g10b10SintPack32:
		        case VkFormat.A2b10g10r10UnormPack32:
		        case VkFormat.A2b10g10r10SnormPack32:
		        case VkFormat.A2b10g10r10UscaledPack32:
		        case VkFormat.A2b10g10r10SscaledPack32:
		        case VkFormat.A2b10g10r10UintPack32:
		        case VkFormat.A2b10g10r10SintPack32:
		            pFormatSize.flags = VkFormatSizeFlag.SizePacked;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 4 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.R16Unorm:
		        case VkFormat.R16Snorm:
		        case VkFormat.R16Uscaled:
		        case VkFormat.R16Sscaled:
		        case VkFormat.R16Uint:
		        case VkFormat.R16Sint:
		        case VkFormat.R16Sfloat:
		            pFormatSize.flags = 0;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 2 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.R16g16Unorm:
		        case VkFormat.R16g16Snorm:
		        case VkFormat.R16g16Uscaled:
		        case VkFormat.R16g16Sscaled:
		        case VkFormat.R16g16Uint:
		        case VkFormat.R16g16Sint:
		        case VkFormat.R16g16Sfloat:
		            pFormatSize.flags = 0;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 4 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.R16g16b16Unorm:
		        case VkFormat.R16g16b16Snorm:
		        case VkFormat.R16g16b16Uscaled:
		        case VkFormat.R16g16b16Sscaled:
		        case VkFormat.R16g16b16Uint:
		        case VkFormat.R16g16b16Sint:
		        case VkFormat.R16g16b16Sfloat:
		            pFormatSize.flags = 0;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 6 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.R16g16b16a16Unorm:
		        case VkFormat.R16g16b16a16Snorm:
		        case VkFormat.R16g16b16a16Uscaled:
		        case VkFormat.R16g16b16a16Sscaled:
		        case VkFormat.R16g16b16a16Uint:
		        case VkFormat.R16g16b16a16Sint:
		        case VkFormat.R16g16b16a16Sfloat:
		            pFormatSize.flags = 0;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 8 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.R32Uint:
		        case VkFormat.R32Sint:
		        case VkFormat.R32Sfloat:
		            pFormatSize.flags = 0;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 4 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.R32g32Uint:
		        case VkFormat.R32g32Sint:
		        case VkFormat.R32g32Sfloat:
		            pFormatSize.flags = 0;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 8 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.R32g32b32Uint:
		        case VkFormat.R32g32b32Sint:
		        case VkFormat.R32g32b32Sfloat:
		            pFormatSize.flags = 0;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 12 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.R32g32b32a32Uint:
		        case VkFormat.R32g32b32a32Sint:
		        case VkFormat.R32g32b32a32Sfloat:
		            pFormatSize.flags = 0;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 16 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.R64Uint:
		        case VkFormat.R64Sint:
		        case VkFormat.R64Sfloat:
		            pFormatSize.flags = 0;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 8 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.R64g64Uint:
		        case VkFormat.R64g64Sint:
		        case VkFormat.R64g64Sfloat:
		            pFormatSize.flags = 0;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 16 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.R64g64b64Uint:
		        case VkFormat.R64g64b64Sint:
		        case VkFormat.R64g64b64Sfloat:
		            pFormatSize.flags = 0;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 24 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.R64g64b64a64Uint:
		        case VkFormat.R64g64b64a64Sint:
		        case VkFormat.R64g64b64a64Sfloat:
		            pFormatSize.flags = 0;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 32 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.B10g11r11UfloatPack32:
		        case VkFormat.E5b9g9r9UfloatPack32:
		            pFormatSize.flags = VkFormatSizeFlag.SizePacked;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 4 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.D16Unorm:
		            pFormatSize.flags = VkFormatSizeFlag.SizeDepth;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 2 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.X8D24UnormPack32:
		            pFormatSize.flags = VkFormatSizeFlag.SizePacked | VkFormatSizeFlag.SizeDepth;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 4 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.D32Sfloat:
		            pFormatSize.flags = VkFormatSizeFlag.SizeDepth;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 4 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.S8Uint:
		            pFormatSize.flags = VkFormatSizeFlag.SizeStencil;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 1 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.D16UnormS8Uint:
		            pFormatSize.flags = VkFormatSizeFlag.SizeDepth | VkFormatSizeFlag.SizeStencil;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 3 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.D24UnormS8Uint:
		            pFormatSize.flags = VkFormatSizeFlag.SizeDepth | VkFormatSizeFlag.SizeStencil;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 4 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.D32SfloatS8Uint:
		            pFormatSize.flags = VkFormatSizeFlag.SizeDepth | VkFormatSizeFlag.SizeStencil;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 8 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.Bc1RgbUnormBlock:
		        case VkFormat.Bc1RgbSrgbBlock:
		        case VkFormat.Bc1RgbaUnormBlock:
		        case VkFormat.Bc1RgbaSrgbBlock:
		            pFormatSize.flags = VkFormatSizeFlag.SizeCompressed;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 8 * 8;
		            pFormatSize.blockWidth = 4;
		            pFormatSize.blockHeight = 4;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.Bc2UnormBlock:
		        case VkFormat.Bc2SrgbBlock:
		        case VkFormat.Bc3UnormBlock:
		        case VkFormat.Bc3SrgbBlock:
		        case VkFormat.Bc4UnormBlock:
		        case VkFormat.Bc4SnormBlock:
		        case VkFormat.Bc5UnormBlock:
		        case VkFormat.Bc5SnormBlock:
		        case VkFormat.Bc6hUfloatBlock:
		        case VkFormat.Bc6hSfloatBlock:
		        case VkFormat.Bc7UnormBlock:
		        case VkFormat.Bc7SrgbBlock:
		            pFormatSize.flags = VkFormatSizeFlag.SizeCompressed;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 16 * 8;
		            pFormatSize.blockWidth = 4;
		            pFormatSize.blockHeight = 4;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.Etc2R8g8b8UnormBlock:
		        case VkFormat.Etc2R8g8b8SrgbBlock:
		        case VkFormat.Etc2R8g8b8a1UnormBlock:
		        case VkFormat.Etc2R8g8b8a1SrgbBlock:
		            pFormatSize.flags = VkFormatSizeFlag.SizeCompressed;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 8 * 8;
		            pFormatSize.blockWidth = 4;
		            pFormatSize.blockHeight = 4;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.Etc2R8g8b8a8UnormBlock:
		        case VkFormat.Etc2R8g8b8a8SrgbBlock:
		        case VkFormat.EacR11UnormBlock:
		        case VkFormat.EacR11SnormBlock:
		        case VkFormat.EacR11g11UnormBlock:
		        case VkFormat.EacR11g11SnormBlock:
		            pFormatSize.flags = VkFormatSizeFlag.SizeCompressed;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 16 * 8;
		            pFormatSize.blockWidth = 4;
		            pFormatSize.blockHeight = 4;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.Astc4x4UnormBlock:
		        case VkFormat.Astc4x4SrgbBlock:
		            pFormatSize.flags = VkFormatSizeFlag.SizeCompressed;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 16 * 8;
		            pFormatSize.blockWidth = 4;
		            pFormatSize.blockHeight = 4;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.Astc5x4UnormBlock:
		        case VkFormat.Astc5x4SrgbBlock:
		            pFormatSize.flags = VkFormatSizeFlag.SizeCompressed;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 16 * 8;
		            pFormatSize.blockWidth = 5;
		            pFormatSize.blockHeight = 4;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.Astc5x5UnormBlock:
		        case VkFormat.Astc5x5SrgbBlock:
		            pFormatSize.flags = VkFormatSizeFlag.SizeCompressed;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 16 * 8;
		            pFormatSize.blockWidth = 5;
		            pFormatSize.blockHeight = 5;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.Astc6x5UnormBlock:
		        case VkFormat.Astc6x5SrgbBlock:
		            pFormatSize.flags = VkFormatSizeFlag.SizeCompressed;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 16 * 8;
		            pFormatSize.blockWidth = 6;
		            pFormatSize.blockHeight = 5;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.Astc6x6UnormBlock:
		        case VkFormat.Astc6x6SrgbBlock:
		            pFormatSize.flags = VkFormatSizeFlag.SizeCompressed;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 16 * 8;
		            pFormatSize.blockWidth = 6;
		            pFormatSize.blockHeight = 6;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.Astc8x5UnormBlock:
		        case VkFormat.Astc8x5SrgbBlock:
		            pFormatSize.flags = VkFormatSizeFlag.SizeCompressed;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 16 * 8;
		            pFormatSize.blockWidth = 8;
		            pFormatSize.blockHeight = 5;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.Astc8x6UnormBlock:
		        case VkFormat.Astc8x6SrgbBlock:
		            pFormatSize.flags = VkFormatSizeFlag.SizeCompressed;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 16 * 8;
		            pFormatSize.blockWidth = 8;
		            pFormatSize.blockHeight = 6;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.Astc8x8UnormBlock:
		        case VkFormat.Astc8x8SrgbBlock:
		            pFormatSize.flags = VkFormatSizeFlag.SizeCompressed;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 16 * 8;
		            pFormatSize.blockWidth = 8;
		            pFormatSize.blockHeight = 8;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.Astc10x5UnormBlock:
		        case VkFormat.Astc10x5SrgbBlock:
		            pFormatSize.flags = VkFormatSizeFlag.SizeCompressed;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 16 * 8;
		            pFormatSize.blockWidth = 10;
		            pFormatSize.blockHeight = 5;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.Astc10x6UnormBlock:
		        case VkFormat.Astc10x6SrgbBlock:
		            pFormatSize.flags = VkFormatSizeFlag.SizeCompressed;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 16 * 8;
		            pFormatSize.blockWidth = 10;
		            pFormatSize.blockHeight = 6;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.Astc10x8UnormBlock:
		        case VkFormat.Astc10x8SrgbBlock:
		            pFormatSize.flags = VkFormatSizeFlag.SizeCompressed;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 16 * 8;
		            pFormatSize.blockWidth = 10;
		            pFormatSize.blockHeight = 8;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.Astc10x10UnormBlock:
		        case VkFormat.Astc10x10SrgbBlock:
		            pFormatSize.flags = VkFormatSizeFlag.SizeCompressed;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 16 * 8;
		            pFormatSize.blockWidth = 10;
		            pFormatSize.blockHeight = 10;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.Astc12x10UnormBlock:
		        case VkFormat.Astc12x10SrgbBlock:
		            pFormatSize.flags = VkFormatSizeFlag.SizeCompressed;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 16 * 8;
		            pFormatSize.blockWidth = 12;
		            pFormatSize.blockHeight = 10;
		            pFormatSize.blockDepth = 1;
		            break;
		        case VkFormat.Astc12x12UnormBlock:
		        case VkFormat.Astc12x12SrgbBlock:
		            pFormatSize.flags = VkFormatSizeFlag.SizeCompressed;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 16 * 8;
		            pFormatSize.blockWidth = 12;
		            pFormatSize.blockHeight = 12;
		            pFormatSize.blockDepth = 1;
		            break;
		        default:
		            pFormatSize.flags = 0;
		            pFormatSize.paletteSizeInBits = 0;
		            pFormatSize.blockSizeInBits = 0 * 8;
		            pFormatSize.blockWidth = 1;
		            pFormatSize.blockHeight = 1;
		            pFormatSize.blockDepth = 1;
		            break;
		    }
		}
    }
}
