// C# port of the work from J.M.P. van Waveren by jp Bruyère
//
// Original licence is http://www.apache.org/licenses/LICENSE-2.0
// C# port is under MIT licencing.
//
// Author:
//			 J.M.P. van Waveren 
//           JP Bruyère <jp_bruyere@hotmail.com>
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
//
// Description	:	Vulkan format properties and conversion from OpenGL.
// Author		:	J.M.P. van Waveren
// Date		:	07/17/2016
// Language	:	C99
// Format		:	Real tabs with the tab size equal to 4 spaces.
// Copyright	:	Copyright (c) 2016 Oculus VR, LLC. All Rights reserved.
// LICENSE
// =======
//
// Copyright (c) 2016 Oculus VR, LLC.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System.Diagnostics;
using VK;

namespace KTX
{
	public static class GLHelper {

		#region GL constants
		const uint GL_R8                                        = 0x8229;
		const uint GL_R8_SNORM                                  = 0x8F94;
		const uint GL_RG8_SNORM                                 = 0x8F95;
		const uint GL_RGB8_SNORM                                = 0x8F96;
		const uint GL_RGBA8_SNORM                               = 0x8F97;
		const uint GL_R8UI                                      = 0x8232;
		const uint GL_RG8UI                                     = 0x8238;
		const uint GL_RGB8UI                                    = 0x8D7D;
		const uint GL_RGBA8UI                                   = 0x8D7C;
		const uint GL_R8I                                       = 0x8231;
		const uint GL_RG8I                                      = 0x8237;
		const uint GL_RGB8I                                     = 0x8D8F;
		const uint GL_RGBA8I                                    = 0x8D8E;
		const uint GL_SR8                                       = 0x8FBD;
		const uint GL_SRG8                                      = 0x8FBE;
		const uint GL_SRGB8                                     = 0x8C41;
		const uint GL_SRGB8_ALPHA8                              = 0x8C43;
		const uint GL_R16                                       = 0x822A;
		const uint GL_RG16                                      = 0x822C;
		const uint GL_RGB16                                     = 0x8054;
		const uint GL_RGBA16                                    = 0x805B;
		const uint GL_R16_SNORM                                 = 0x8F98;
		const uint GL_RG16_SNORM                                = 0x8F99;
		const uint GL_RGB16_SNORM                               = 0x8F9A;
		const uint GL_RGBA16_SNORM                              = 0x8F9B;
		const uint GL_R16UI                                     = 0x8234;
		const uint GL_RG16UI                                    = 0x823A;
		const uint GL_RGB16UI                                   = 0x8D77;
		const uint GL_RGBA16UI                                  = 0x8D76;
		const uint GL_R16I                                      = 0x8233;
		const uint GL_RG16I                                     = 0x8239;
		const uint GL_RGB16I                                    = 0x8D89;
		const uint GL_RGBA16I                                   = 0x8D88;
		const uint GL_R16F                                      = 0x822D;
		const uint GL_RG16F                                     = 0x822F;
		const uint GL_RGB16F                                    = 0x881B;
		const uint GL_RGBA16F                                   = 0x881A;
		const uint GL_R32UI                                     = 0x8236;
		const uint GL_RG32UI                                    = 0x823C;
		const uint GL_RGB32UI                                   = 0x8D71;
		const uint GL_RGBA32UI                                  = 0x8D70;
		const uint GL_R32I                                      = 0x8235;
		const uint GL_RG32I                                     = 0x823B;
		const uint GL_RGB32I                                    = 0x8D83;
		const uint GL_RGBA32I                                   = 0x8D82;
		const uint GL_R32F                                      = 0x822E;
		const uint GL_RG32F                                     = 0x8230;
		const uint GL_RGB32F                                    = 0x8815;
		const uint GL_RGBA32F                                   = 0x8814;
		const uint GL_R3_G3_B2                                  = 0x2A10;
		const uint GL_RGB4                                      = 0x804F;
		const uint GL_RGB5                                      = 0x8050;
		const uint GL_RGB565                                    = 0x8D62;
		const uint GL_RGB10                                     = 0x8052;
		const uint GL_RGB12                                     = 0x8053;
		const uint GL_RGBA2                                     = 0x8055;
		const uint GL_RGBA4                                     = 0x8056;
		const uint GL_RGBA12                                    = 0x805A;
		const uint GL_RGB5_A1                                   = 0x8057;
		const uint GL_RGB10_A2                                  = 0x8059;
		const uint GL_RGB10_A2UI                                = 0x906F;
		const uint GL_R11F_G11F_B10F                            = 0x8C3A;
		const uint GL_RGB9_E5                                   = 0x8C3D;
		const uint GL_COMPRESSED_RGB_S3TC_DXT1_EXT              = 0x83F0;
		const uint GL_COMPRESSED_RGBA_S3TC_DXT1_EXT             = 0x83F1;
		const uint GL_COMPRESSED_RGBA_S3TC_DXT5_EXT             = 0x83F3;
		const uint GL_COMPRESSED_RGBA_S3TC_DXT3_EXT             = 0x83F2;
		const uint GL_COMPRESSED_SRGB_S3TC_DXT1_EXT             = 0x8C4C;
		const uint GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT1_EXT       = 0x8C4D;
		const uint GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT3_EXT       = 0x8C4E;
		const uint GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT5_EXT       = 0x8C4F;
		const uint GL_COMPRESSED_LUMINANCE_LATC1_EXT            = 0x8C70;
		const uint GL_COMPRESSED_LUMINANCE_ALPHA_LATC2_EXT      = 0x8C72;
		const uint GL_COMPRESSED_SIGNED_LUMINANCE_LATC1_EXT     = 0x8C71;
		const uint GL_COMPRESSED_SIGNED_LUMINANCE_ALPHA_LATC2_EXT = 0x8C73;
		const uint GL_COMPRESSED_RED_RGTC1                      = 0x8DBB;
		const uint GL_COMPRESSED_RG_RGTC2                       = 0x8DBD;
		const uint GL_COMPRESSED_SIGNED_RED_RGTC1               = 0x8DBC;
		const uint GL_COMPRESSED_SIGNED_RG_RGTC2                = 0x8DBE;
		const uint GL_COMPRESSED_RGB_BPTC_UNSIGNED_FLOAT        = 0x8E8F;
		const uint GL_COMPRESSED_RGB_BPTC_SIGNED_FLOAT          = 0x8E8E;
		const uint GL_COMPRESSED_RGBA_BPTC_UNORM                = 0x8E8C;
		const uint GL_COMPRESSED_SRGB_ALPHA_BPTC_UNORM          = 0x8E8D;
		const uint GL_ETC1_RGB8_OES                             = 0x8D64;
		const uint GL_COMPRESSED_RGB8_ETC2                      = 0x9274;
		const uint GL_COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2  = 0x9276;
		const uint GL_COMPRESSED_RGBA8_ETC2_EAC                 = 0x9278;
		const uint GL_COMPRESSED_SRGB8_ETC2                     = 0x9275;
		const uint GL_COMPRESSED_SRGB8_PUNCHTHROUGH_ALPHA1_ETC2 = 0x9277;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ETC2_EAC          = 0x9279;
		const uint GL_COMPRESSED_R11_EAC                        = 0x9270;
		const uint GL_COMPRESSED_RG11_EAC                       = 0x9272;
		const uint GL_COMPRESSED_SIGNED_R11_EAC                 = 0x9271;
		const uint GL_COMPRESSED_SIGNED_RG11_EAC                = 0x9273;
		const uint GL_COMPRESSED_RGB_PVRTC_2BPPV1_IMG           = 0x8C01;
		const uint GL_COMPRESSED_RGB_PVRTC_4BPPV1_IMG           = 0x8C00;
		const uint GL_COMPRESSED_RGBA_PVRTC_2BPPV1_IMG          = 0x8C03;
		const uint GL_COMPRESSED_RGBA_PVRTC_4BPPV1_IMG          = 0x8C02;
		const uint GL_COMPRESSED_RGBA_PVRTC_2BPPV2_IMG          = 0x9137;
		const uint GL_COMPRESSED_RGBA_PVRTC_4BPPV2_IMG          = 0x9138;
		const uint GL_COMPRESSED_SRGB_PVRTC_2BPPV1_EXT          = 0x8A54;
		const uint GL_COMPRESSED_SRGB_PVRTC_4BPPV1_EXT          = 0x8A55;
		const uint GL_COMPRESSED_SRGB_ALPHA_PVRTC_2BPPV1_EXT    = 0x8A56;
		const uint GL_COMPRESSED_SRGB_ALPHA_PVRTC_4BPPV1_EXT    = 0x8A57;
		const uint GL_COMPRESSED_SRGB_ALPHA_PVRTC_2BPPV2_IMG    = 0x93F0;
		const uint GL_COMPRESSED_SRGB_ALPHA_PVRTC_4BPPV2_IMG    = 0x93F1;
		const uint GL_COMPRESSED_RGBA_ASTC_4x4_KHR              = 0x93B0;
		const uint GL_COMPRESSED_RGBA_ASTC_5x4_KHR              = 0x93B1;
		const uint GL_COMPRESSED_RGBA_ASTC_5x5_KHR              = 0x93B2;
		const uint GL_COMPRESSED_RGBA_ASTC_6x5_KHR              = 0x93B3;
		const uint GL_COMPRESSED_RGBA_ASTC_6x6_KHR              = 0x93B4;
		const uint GL_COMPRESSED_RGBA_ASTC_8x5_KHR              = 0x93B5;
		const uint GL_COMPRESSED_RGBA_ASTC_8x6_KHR              = 0x93B6;
		const uint GL_COMPRESSED_RGBA_ASTC_8x8_KHR              = 0x93B7;
		const uint GL_COMPRESSED_RGBA_ASTC_10x5_KHR             = 0x93B8;
		const uint GL_COMPRESSED_RGBA_ASTC_10x6_KHR             = 0x93B9;
		const uint GL_COMPRESSED_RGBA_ASTC_10x8_KHR             = 0x93BA;
		const uint GL_COMPRESSED_RGBA_ASTC_10x10_KHR            = 0x93BB;
		const uint GL_COMPRESSED_RGBA_ASTC_12x10_KHR            = 0x93BC;
		const uint GL_COMPRESSED_RGBA_ASTC_12x12_KHR            = 0x93BD;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_4x4_KHR      = 0x93D0;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x4_KHR      = 0x93D1;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x5_KHR      = 0x93D2;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x5_KHR      = 0x93D3;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x6_KHR      = 0x93D4;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x5_KHR      = 0x93D5;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x6_KHR      = 0x93D6;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x8_KHR      = 0x93D7;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x5_KHR     = 0x93D8;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x6_KHR     = 0x93D9;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x8_KHR     = 0x93DA;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x10_KHR    = 0x93DB;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_12x10_KHR    = 0x93DC;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_12x12_KHR    = 0x93DD;
		const uint GL_COMPRESSED_RGBA_ASTC_3x3x3_OES            = 0x93C0;
		const uint GL_COMPRESSED_RGBA_ASTC_4x3x3_OES            = 0x93C1;
		const uint GL_COMPRESSED_RGBA_ASTC_4x4x3_OES            = 0x93C2;
		const uint GL_COMPRESSED_RGBA_ASTC_4x4x4_OES            = 0x93C3;
		const uint GL_COMPRESSED_RGBA_ASTC_5x4x4_OES            = 0x93C4;
		const uint GL_COMPRESSED_RGBA_ASTC_5x5x4_OES            = 0x93C5;
		const uint GL_COMPRESSED_RGBA_ASTC_5x5x5_OES            = 0x93C6;
		const uint GL_COMPRESSED_RGBA_ASTC_6x5x5_OES            = 0x93C7;
		const uint GL_COMPRESSED_RGBA_ASTC_6x6x5_OES            = 0x93C8;
		const uint GL_COMPRESSED_RGBA_ASTC_6x6x6_OES            = 0x93C9;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_3x3x3_OES    = 0x93E0;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_4x3x3_OES    = 0x93E1;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_4x4x3_OES    = 0x93E2;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_4x4x4_OES    = 0x93E3;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x4x4_OES    = 0x93E4;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x5x4_OES    = 0x93E5;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x5x5_OES    = 0x93E6;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x5x5_OES    = 0x93E7;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x6x5_OES    = 0x93E8;
		const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x6x6_OES    = 0x93E9;
		const uint GL_ATC_RGB_AMD                               = 0x8C92;
		const uint GL_ATC_RGBA_EXPLICIT_ALPHA_AMD               = 0x8C93;
		const uint GL_ATC_RGBA_INTERPOLATED_ALPHA_AMD           = 0x87EE;
		const uint GL_PALETTE4_RGB8_OES                         = 0x8B90;
		const uint GL_PALETTE4_RGBA8_OES                        = 0x8B91;
		const uint GL_PALETTE4_R5_G6_B5_OES                     = 0x8B92;
		const uint GL_PALETTE4_RGBA4_OES                        = 0x8B93;
		const uint GL_PALETTE4_RGB5_A1_OES                      = 0x8B94;
		const uint GL_PALETTE8_RGB8_OES                         = 0x8B95;
		const uint GL_PALETTE8_RGBA8_OES                        = 0x8B96;
		const uint GL_PALETTE8_R5_G6_B5_OES                     = 0x8B97;
		const uint GL_PALETTE8_RGBA4_OES                        = 0x8B98;
		const uint GL_PALETTE8_RGB5_A1_OES                      = 0x8B99;
		const uint GL_DEPTH_COMPONENT16                         = 0x81A5;
		const uint GL_DEPTH_COMPONENT24                         = 0x81A6;
		const uint GL_DEPTH_COMPONENT32                         = 0x81A7;
		const uint GL_DEPTH_COMPONENT32F                        = 0x8CAC;
		const uint GL_DEPTH_COMPONENT32F_NV                     = 0x8DAB;
		const uint GL_STENCIL_INDEX1                            = 0x8D46;
		const uint GL_STENCIL_INDEX4                            = 0x8D47;
		const uint GL_STENCIL_INDEX8                            = 0x8D48;
		const uint GL_STENCIL_INDEX16                           = 0x8D49;
		const uint GL_DEPTH24_STENCIL8                          = 0x88F0;
		const uint GL_DEPTH32F_STENCIL8                         = 0x8CAD;
		const uint GL_DEPTH32F_STENCIL8_NV                      = 0x8DAC;

		const uint GL_UNSIGNED_BYTE                             = 0x1401;
		const uint GL_RED                                       = 0x1903;
		const uint GL_RG                                        = 0x8227;
		const uint GL_RGB                                       = 0x1907;
		const uint GL_BGR                                       = 0x80E0;
		const uint GL_RGBA                                      = 0x1908;
		const uint GL_BGRA                                      = 0x80E1;
		const uint GL_RED_INTEGER                               = 0x8D94;
		const uint GL_RG_INTEGER                                = 0x8228;
		const uint GL_RGB_INTEGER                               = 0x8D98;
		const uint GL_BGR_INTEGER                               = 0x8D9A;
		const uint GL_RGBA_INTEGER                              = 0x8D99;
		const uint GL_BGRA_INTEGER                              = 0x8D9B;
		const uint GL_STENCIL_INDEX                             = 0x1901;
		const uint GL_DEPTH_COMPONENT                           = 0x1902;
		const uint GL_DEPTH_STENCIL                             = 0x84F9;
		const uint GL_BYTE                                      = 0x1400;
		const uint GL_UNSIGNED_SHORT                            = 0x1403;
		const uint GL_SHORT                                     = 0x1402;
		const uint GL_HALF_FLOAT                                = 0x140B;
		const uint GL_HALF_FLOAT_OES                            = 0x8D61;
		const uint GL_UNSIGNED_INT                              = 0x1405;
		const uint GL_INT                                       = 0x1404;
		const uint GL_FLOAT                                     = 0x1406;
		const uint GL_UNSIGNED_INT64 									  = 0x8BC2;
		const uint GL_INT64													  = 0x140E;
		const uint GL_DOUBLE                                    = 0x140A;
		const uint GL_UNSIGNED_BYTE_3_3_2                       = 0x8032;
		const uint GL_UNSIGNED_BYTE_2_3_3_REV                   = 0x8362;
		const uint GL_UNSIGNED_SHORT_5_6_5                      = 0x8363;
		const uint GL_UNSIGNED_SHORT_5_6_5_REV                  = 0x8364;
		const uint GL_UNSIGNED_SHORT_4_4_4_4                    = 0x8033;
		const uint GL_UNSIGNED_SHORT_4_4_4_4_REV                = 0x8365;
		const uint GL_UNSIGNED_SHORT_5_5_5_1                    = 0x8034;
		const uint GL_UNSIGNED_SHORT_1_5_5_5_REV                = 0x8366;
		const uint GL_UNSIGNED_INT_8_8_8_8                      = 0x8035;
		const uint GL_UNSIGNED_INT_8_8_8_8_REV                  = 0x8367;
		const uint GL_UNSIGNED_INT_10_10_10_2                   = 0x8036;
		const uint GL_UNSIGNED_INT_2_10_10_10_REV               = 0x8368;
		const uint GL_UNSIGNED_INT_10F_11F_11F_REV              = 0x8C3B;
		const uint GL_UNSIGNED_INT_5_9_9_9_REV                  = 0x8C3E;
		const uint GL_UNSIGNED_INT_24_8                         = 0x84FA;
		const uint GL_FLOAT_32_UNSIGNED_INT_24_8_REV            = 0x8DAD;
		#endregion

		public static VkFormat vkGetFormatFromOpenGLInternalFormat(uint internalFormat)
		{

			switch (internalFormat)
			{
				//
				// 8 bits per component
				//
				case GL_R8:											 return VkFormat.R8Unorm;                   // 1-component, 8-bit unsigned normalized
				case GL_R8_SNORM:                                    return VkFormat.R8Snorm;                   // 1-component, 8-bit signed normalized
				case GL_RG8_SNORM:                                   return VkFormat.R8g8Snorm;                 // 2-component, 8-bit signed normalized
				case GL_RGB8_SNORM:                                  return VkFormat.R8g8b8Snorm;               // 3-component, 8-bit signed normalized
				case GL_RGBA8_SNORM:                                 return VkFormat.R8g8b8a8Snorm;             // 4-component, 8-bit signed normalized
				
				case GL_R8UI:                                        return VkFormat.R8Uint;                    // 1-component, 8-bit unsigned integer
				case GL_RG8UI:                                       return VkFormat.R8g8Uint;                  // 2-component, 8-bit unsigned integer
				case GL_RGB8UI:                                      return VkFormat.R8g8b8Uint;                // 3-component, 8-bit unsigned integer
				case GL_RGBA8UI:                                     return VkFormat.R8g8b8a8Uint;              // 4-component, 8-bit unsigned integer
				
				case GL_R8I:                                         return VkFormat.R8Sint;                    // 1-component, 8-bit signed integer
				case GL_RG8I:                                        return VkFormat.R8g8Sint;                  // 2-component, 8-bit signed integer
				case GL_RGB8I:                                       return VkFormat.R8g8b8Sint;                // 3-component, 8-bit signed integer
				case GL_RGBA8I:                                      return VkFormat.R8g8b8a8Sint;              // 4-component, 8-bit signed integer
				
				case GL_SR8:                                         return VkFormat.R8Srgb;                    // 1-component, 8-bit sRGB
				case GL_SRG8:                                        return VkFormat.R8g8Srgb;                  // 2-component, 8-bit sRGB
				case GL_SRGB8:                                       return VkFormat.R8g8b8Srgb;                // 3-component, 8-bit sRGB
				case GL_SRGB8_ALPHA8:                                return VkFormat.R8g8b8a8Srgb;              // 4-component, 8-bit sRGB
				
				//
				// 16 bits per component
				//
				case GL_R16:                                         return VkFormat.R16Unorm;                  // 1-component, 16-bit unsigned normalized
				case GL_RG16:                                        return VkFormat.R16g16Unorm;               // 2-component, 16-bit unsigned normalized
				case GL_RGB16:                                       return VkFormat.R16g16b16Unorm;            // 3-component, 16-bit unsigned normalized
				case GL_RGBA16:                                      return VkFormat.R16g16b16a16Unorm;         // 4-component, 16-bit unsigned normalized
				
				case GL_R16_SNORM:                                   return VkFormat.R16Snorm;                  // 1-component, 16-bit signed normalized
				case GL_RG16_SNORM:                                  return VkFormat.R16g16Snorm;               // 2-component, 16-bit signed normalized
				case GL_RGB16_SNORM:                                 return VkFormat.R16g16b16Snorm;            // 3-component, 16-bit signed normalized
				case GL_RGBA16_SNORM:                                return VkFormat.R16g16b16a16Snorm;         // 4-component, 16-bit signed normalized
				
				case GL_R16UI:                                       return VkFormat.R16Uint;                   // 1-component, 16-bit unsigned integer
				case GL_RG16UI:                                      return VkFormat.R16g16Uint;                // 2-component, 16-bit unsigned integer
				case GL_RGB16UI:                                     return VkFormat.R16g16b16Uint;             // 3-component, 16-bit unsigned integer
				case GL_RGBA16UI:                                    return VkFormat.R16g16b16a16Uint;          // 4-component, 16-bit unsigned integer
				
				case GL_R16I:                                        return VkFormat.R16Sint;                   // 1-component, 16-bit signed integer
				case GL_RG16I:                                       return VkFormat.R16g16Sint;                // 2-component, 16-bit signed integer
				case GL_RGB16I:                                      return VkFormat.R16g16b16Sint;             // 3-component, 16-bit signed integer
				case GL_RGBA16I:                                     return VkFormat.R16g16b16a16Sint;          // 4-component, 16-bit signed integer
				
				case GL_R16F:                                        return VkFormat.R16Sfloat;                 // 1-component, 16-bit floating-point
				case GL_RG16F:                                       return VkFormat.R16g16Sfloat;              // 2-component, 16-bit floating-point
				case GL_RGB16F:                                      return VkFormat.R16g16b16Sfloat;           // 3-component, 16-bit floating-point
				case GL_RGBA16F:                                     return VkFormat.R16g16b16a16Sfloat;        // 4-component, 16-bit floating-point
				
				//
				// 32 bits per component
				//
				case GL_R32UI:                                       return VkFormat.R32Uint;                   // 1-component, 32-bit unsigned integer
				case GL_RG32UI:                                      return VkFormat.R32g32Uint;                // 2-component, 32-bit unsigned integer
				case GL_RGB32UI:                                     return VkFormat.R32g32b32Uint;             // 3-component, 32-bit unsigned integer
				case GL_RGBA32UI:                                    return VkFormat.R32g32b32a32Uint;          // 4-component, 32-bit unsigned integer
				
				case GL_R32I:                                        return VkFormat.R32Sint;                   // 1-component, 32-bit signed integer
				case GL_RG32I:                                       return VkFormat.R32g32Sint;                // 2-component, 32-bit signed integer
				case GL_RGB32I:                                      return VkFormat.R32g32b32Sint;             // 3-component, 32-bit signed integer
				case GL_RGBA32I:                                     return VkFormat.R32g32b32a32Sint;          // 4-component, 32-bit signed integer
				
				case GL_R32F:                                        return VkFormat.R32Sfloat;                 // 1-component, 32-bit floating-point
				case GL_RG32F:                                       return VkFormat.R32g32Sfloat;              // 2-component, 32-bit floating-point
				case GL_RGB32F:                                      return VkFormat.R32g32b32Sfloat;           // 3-component, 32-bit floating-point
				case GL_RGBA32F:                                     return VkFormat.R32g32b32a32Sfloat;        // 4-component, 32-bit floating-point
				
				//
				// Packed
				//
				case GL_R3_G3_B2:                                    return VkFormat.Undefined;                 // 3-component 3:3:2, unsigned normalized
				case GL_RGB4:                                        return VkFormat.Undefined;                 // 3-component 4:4:4, unsigned normalized
				case GL_RGB5:                                        return VkFormat.R5g5b5a1UnormPack16;       // 3-component 5:5:5, unsigned normalized
				case GL_RGB565:                                      return VkFormat.R5g6b5UnormPack16;         // 3-component 5:6:5, unsigned normalized
				case GL_RGB10:                                       return VkFormat.A2r10g10b10UnormPack32;    // 3-component 10:10:10, unsigned normalized
				case GL_RGB12:                                       return VkFormat.Undefined;                 // 3-component 12:12:12, unsigned normalized
				case GL_RGBA2:                                       return VkFormat.Undefined;                 // 4-component 2:2:2:2, unsigned normalized
				case GL_RGBA4:                                       return VkFormat.R4g4b4a4UnormPack16;       // 4-component 4:4:4:4, unsigned normalized
				case GL_RGBA12:                                      return VkFormat.Undefined;                 // 4-component 12:12:12:12, unsigned normalized
				case GL_RGB5_A1:                                     return VkFormat.A1r5g5b5UnormPack16;       // 4-component 5:5:5:1, unsigned normalized
				case GL_RGB10_A2:                                    return VkFormat.A2r10g10b10UnormPack32;    // 4-component 10:10:10:2, unsigned normalized
				case GL_RGB10_A2UI:                                  return VkFormat.A2r10g10b10UintPack32;     // 4-component 10:10:10:2, unsigned integer
				case GL_R11F_G11F_B10F:                              return VkFormat.B10g11r11UfloatPack32;     // 3-component 11:11:10, floating-point
				case GL_RGB9_E5:                                     return VkFormat.E5b9g9r9UfloatPack32;      // 3-component/exp 9:9:9/5, floating-point
				
				//
				// S3TC/DXT/BC
				//
				
				case GL_COMPRESSED_RGB_S3TC_DXT1_EXT:                return VkFormat.Bc1RgbUnormBlock;          // line through 3D space, 4x4 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_S3TC_DXT1_EXT:               return VkFormat.Bc1RgbaUnormBlock;         // line through 3D space plus 1-bit alpha, 4x4 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_S3TC_DXT5_EXT:               return VkFormat.Bc2UnormBlock;             // line through 3D space plus line through 1D space, 4x4 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_S3TC_DXT3_EXT:               return VkFormat.Bc3UnormBlock;             // line through 3D space plus 4-bit alpha, 4x4 blocks, unsigned normalized
				
				case GL_COMPRESSED_SRGB_S3TC_DXT1_EXT:               return VkFormat.Bc1RgbSrgbBlock;           // line through 3D space, 4x4 blocks, sRGB
				case GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT1_EXT:         return VkFormat.Bc1RgbaSrgbBlock;          // line through 3D space plus 1-bit alpha, 4x4 blocks, sRGB
				case GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT3_EXT:         return VkFormat.Bc2SrgbBlock;              // line through 3D space plus line through 1D space, 4x4 blocks, sRGB
				case GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT5_EXT:         return VkFormat.Bc3SrgbBlock;              // line through 3D space plus 4-bit alpha, 4x4 blocks, sRGB
				
				case GL_COMPRESSED_LUMINANCE_LATC1_EXT:              return VkFormat.Bc4UnormBlock;             // line through 1D space, 4x4 blocks, unsigned normalized
				case GL_COMPRESSED_LUMINANCE_ALPHA_LATC2_EXT:        return VkFormat.Bc5UnormBlock;             // two lines through 1D space, 4x4 blocks, unsigned normalized
				case GL_COMPRESSED_SIGNED_LUMINANCE_LATC1_EXT:       return VkFormat.Bc4SnormBlock;             // line through 1D space, 4x4 blocks, signed normalized
				case GL_COMPRESSED_SIGNED_LUMINANCE_ALPHA_LATC2_EXT: return VkFormat.Bc5SnormBlock;             // two lines through 1D space, 4x4 blocks, signed normalized
				
				case GL_COMPRESSED_RED_RGTC1:                        return VkFormat.Bc4UnormBlock;             // line through 1D space, 4x4 blocks, unsigned normalized
				case GL_COMPRESSED_RG_RGTC2:                         return VkFormat.Bc5UnormBlock;             // two lines through 1D space, 4x4 blocks, unsigned normalized
				case GL_COMPRESSED_SIGNED_RED_RGTC1:                 return VkFormat.Bc4SnormBlock;             // line through 1D space, 4x4 blocks, signed normalized
				case GL_COMPRESSED_SIGNED_RG_RGTC2:                  return VkFormat.Bc5SnormBlock;             // two lines through 1D space, 4x4 blocks, signed normalized
				
				case GL_COMPRESSED_RGB_BPTC_UNSIGNED_FLOAT:          return VkFormat.Bc6hUfloatBlock;           // 3-component, 4x4 blocks, unsigned floating-point
				case GL_COMPRESSED_RGB_BPTC_SIGNED_FLOAT:            return VkFormat.Bc6hSfloatBlock;           // 3-component, 4x4 blocks, signed floating-point
				case GL_COMPRESSED_RGBA_BPTC_UNORM:                  return VkFormat.Bc7UnormBlock;             // 4-component, 4x4 blocks, unsigned normalized
				case GL_COMPRESSED_SRGB_ALPHA_BPTC_UNORM:            return VkFormat.Bc7SrgbBlock;              // 4-component, 4x4 blocks, sRGB
				
				//
				// ETC
				//
				case GL_ETC1_RGB8_OES:                               return VkFormat.Etc2R8g8b8UnormBlock;      // 3-component ETC1, 4x4 blocks, unsigned normalized
				
				case GL_COMPRESSED_RGB8_ETC2:                        return VkFormat.Etc2R8g8b8UnormBlock;      // 3-component ETC2, 4x4 blocks, unsigned normalized
				case GL_COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2:    return VkFormat.Etc2R8g8b8a1UnormBlock;    // 4-component ETC2 with 1-bit alpha, 4x4 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA8_ETC2_EAC:                   return VkFormat.Etc2R8g8b8a8UnormBlock;    // 4-component ETC2, 4x4 blocks, unsigned normalized
				
				case GL_COMPRESSED_SRGB8_ETC2:                       return VkFormat.Etc2R8g8b8SrgbBlock;       // 3-component ETC2, 4x4 blocks, sRGB
				case GL_COMPRESSED_SRGB8_PUNCHTHROUGH_ALPHA1_ETC2:   return VkFormat.Etc2R8g8b8a1SrgbBlock;     // 4-component ETC2 with 1-bit alpha, 4x4 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ETC2_EAC:            return VkFormat.Etc2R8g8b8a8SrgbBlock;     // 4-component ETC2, 4x4 blocks, sRGB
				
				case GL_COMPRESSED_R11_EAC:                          return VkFormat.EacR11UnormBlock;          // 1-component ETC, 4x4 blocks, unsigned normalized
				case GL_COMPRESSED_RG11_EAC:                         return VkFormat.EacR11g11UnormBlock;       // 2-component ETC, 4x4 blocks, unsigned normalized
				case GL_COMPRESSED_SIGNED_R11_EAC:                   return VkFormat.EacR11SnormBlock;          // 1-component ETC, 4x4 blocks, signed normalized
				case GL_COMPRESSED_SIGNED_RG11_EAC:                  return VkFormat.EacR11g11SnormBlock;       // 2-component ETC, 4x4 blocks, signed normalized
				
				//
				// PVRTC
				//
				case GL_COMPRESSED_RGB_PVRTC_2BPPV1_IMG:             return VkFormat.Undefined;                 // 3-component PVRTC, 16x8 blocks, unsigned normalized
				case GL_COMPRESSED_RGB_PVRTC_4BPPV1_IMG:             return VkFormat.Undefined;                 // 3-component PVRTC, 8x8 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_PVRTC_2BPPV1_IMG:            return VkFormat.Undefined;                 // 4-component PVRTC, 16x8 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_PVRTC_4BPPV1_IMG:            return VkFormat.Undefined;                 // 4-component PVRTC, 8x8 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_PVRTC_2BPPV2_IMG:            return VkFormat.Undefined;                 // 4-component PVRTC, 8x4 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_PVRTC_4BPPV2_IMG:            return VkFormat.Undefined;                 // 4-component PVRTC, 4x4 blocks, unsigned normalized
				
				case GL_COMPRESSED_SRGB_PVRTC_2BPPV1_EXT:            return VkFormat.Undefined;                 // 3-component PVRTC, 16x8 blocks, sRGB
				case GL_COMPRESSED_SRGB_PVRTC_4BPPV1_EXT:            return VkFormat.Undefined;                 // 3-component PVRTC, 8x8 blocks, sRGB
				case GL_COMPRESSED_SRGB_ALPHA_PVRTC_2BPPV1_EXT:      return VkFormat.Undefined;                 // 4-component PVRTC, 16x8 blocks, sRGB
				case GL_COMPRESSED_SRGB_ALPHA_PVRTC_4BPPV1_EXT:      return VkFormat.Undefined;                 // 4-component PVRTC, 8x8 blocks, sRGB
				case GL_COMPRESSED_SRGB_ALPHA_PVRTC_2BPPV2_IMG:      return VkFormat.Undefined;                 // 4-component PVRTC, 8x4 blocks, sRGB
				case GL_COMPRESSED_SRGB_ALPHA_PVRTC_4BPPV2_IMG:      return VkFormat.Undefined;                 // 4-component PVRTC, 4x4 blocks, sRGB
				
				//
				// ASTC
				//
				case GL_COMPRESSED_RGBA_ASTC_4x4_KHR:                return VkFormat.Astc4x4UnormBlock;         // 4-component ASTC, 4x4 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_5x4_KHR:                return VkFormat.Astc5x4UnormBlock;         // 4-component ASTC, 5x4 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_5x5_KHR:                return VkFormat.Astc5x5UnormBlock;         // 4-component ASTC, 5x5 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_6x5_KHR:                return VkFormat.Astc6x5UnormBlock;         // 4-component ASTC, 6x5 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_6x6_KHR:                return VkFormat.Astc6x6UnormBlock;         // 4-component ASTC, 6x6 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_8x5_KHR:                return VkFormat.Astc8x5UnormBlock;         // 4-component ASTC, 8x5 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_8x6_KHR:                return VkFormat.Astc8x6UnormBlock;         // 4-component ASTC, 8x6 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_8x8_KHR:                return VkFormat.Astc8x8UnormBlock;         // 4-component ASTC, 8x8 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_10x5_KHR:               return VkFormat.Astc10x5UnormBlock;        // 4-component ASTC, 10x5 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_10x6_KHR:               return VkFormat.Astc10x6UnormBlock;        // 4-component ASTC, 10x6 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_10x8_KHR:               return VkFormat.Astc10x8UnormBlock;        // 4-component ASTC, 10x8 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_10x10_KHR:              return VkFormat.Astc10x10UnormBlock;       // 4-component ASTC, 10x10 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_12x10_KHR:              return VkFormat.Astc12x10UnormBlock;       // 4-component ASTC, 12x10 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_12x12_KHR:              return VkFormat.Astc12x12UnormBlock;       // 4-component ASTC, 12x12 blocks, unsigned normalized
				
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_4x4_KHR:        return VkFormat.Astc4x4SrgbBlock;          // 4-component ASTC, 4x4 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x4_KHR:        return VkFormat.Astc5x4SrgbBlock;          // 4-component ASTC, 5x4 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x5_KHR:        return VkFormat.Astc5x5SrgbBlock;          // 4-component ASTC, 5x5 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x5_KHR:        return VkFormat.Astc6x5SrgbBlock;          // 4-component ASTC, 6x5 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x6_KHR:        return VkFormat.Astc6x6SrgbBlock;          // 4-component ASTC, 6x6 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x5_KHR:        return VkFormat.Astc8x5SrgbBlock;          // 4-component ASTC, 8x5 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x6_KHR:        return VkFormat.Astc8x6SrgbBlock;          // 4-component ASTC, 8x6 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x8_KHR:        return VkFormat.Astc8x8SrgbBlock;          // 4-component ASTC, 8x8 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x5_KHR:       return VkFormat.Astc10x5SrgbBlock;         // 4-component ASTC, 10x5 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x6_KHR:       return VkFormat.Astc10x6SrgbBlock;         // 4-component ASTC, 10x6 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x8_KHR:       return VkFormat.Astc10x8SrgbBlock;         // 4-component ASTC, 10x8 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x10_KHR:      return VkFormat.Astc10x10SrgbBlock;        // 4-component ASTC, 10x10 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_12x10_KHR:      return VkFormat.Astc12x10SrgbBlock;        // 4-component ASTC, 12x10 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_12x12_KHR:      return VkFormat.Astc12x12SrgbBlock;        // 4-component ASTC, 12x12 blocks, sRGB
				
				case GL_COMPRESSED_RGBA_ASTC_3x3x3_OES:              return VkFormat.Undefined;                 // 4-component ASTC, 3x3x3 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_4x3x3_OES:              return VkFormat.Undefined;                 // 4-component ASTC, 4x3x3 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_4x4x3_OES:              return VkFormat.Undefined;                 // 4-component ASTC, 4x4x3 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_4x4x4_OES:              return VkFormat.Undefined;                 // 4-component ASTC, 4x4x4 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_5x4x4_OES:              return VkFormat.Undefined;                 // 4-component ASTC, 5x4x4 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_5x5x4_OES:              return VkFormat.Undefined;                 // 4-component ASTC, 5x5x4 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_5x5x5_OES:              return VkFormat.Undefined;                 // 4-component ASTC, 5x5x5 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_6x5x5_OES:              return VkFormat.Undefined;                 // 4-component ASTC, 6x5x5 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_6x6x5_OES:              return VkFormat.Undefined;                 // 4-component ASTC, 6x6x5 blocks, unsigned normalized
				case GL_COMPRESSED_RGBA_ASTC_6x6x6_OES:              return VkFormat.Undefined;                 // 4-component ASTC, 6x6x6 blocks, unsigned normalized
				
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_3x3x3_OES:      return VkFormat.Undefined;                 // 4-component ASTC, 3x3x3 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_4x3x3_OES:      return VkFormat.Undefined;                 // 4-component ASTC, 4x3x3 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_4x4x3_OES:      return VkFormat.Undefined;                 // 4-component ASTC, 4x4x3 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_4x4x4_OES:      return VkFormat.Undefined;                 // 4-component ASTC, 4x4x4 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x4x4_OES:      return VkFormat.Undefined;                 // 4-component ASTC, 5x4x4 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x5x4_OES:      return VkFormat.Undefined;                 // 4-component ASTC, 5x5x4 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x5x5_OES:      return VkFormat.Undefined;                 // 4-component ASTC, 5x5x5 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x5x5_OES:      return VkFormat.Undefined;                 // 4-component ASTC, 6x5x5 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x6x5_OES:      return VkFormat.Undefined;                 // 4-component ASTC, 6x6x5 blocks, sRGB
				case GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x6x6_OES:      return VkFormat.Undefined;                 // 4-component ASTC, 6x6x6 blocks, sRGB
				
				//
				// ATC
				//
				case GL_ATC_RGB_AMD:                                 return VkFormat.Undefined;                 // 3-component, 4x4 blocks, unsigned normalized
				case GL_ATC_RGBA_EXPLICIT_ALPHA_AMD:                 return VkFormat.Undefined;                 // 4-component, 4x4 blocks, unsigned normalized
				case GL_ATC_RGBA_INTERPOLATED_ALPHA_AMD:             return VkFormat.Undefined;                 // 4-component, 4x4 blocks, unsigned normalized
				
				//
				// Palletized
				//
				case GL_PALETTE4_RGB8_OES:                           return VkFormat.Undefined;                 // 3-component 8:8:8, 4-bit palette, unsigned normalized
				case GL_PALETTE4_RGBA8_OES:                          return VkFormat.Undefined;                 // 4-component 8:8:8:8, 4-bit palette, unsigned normalized
				case GL_PALETTE4_R5_G6_B5_OES:                       return VkFormat.Undefined;                 // 3-component 5:6:5, 4-bit palette, unsigned normalized
				case GL_PALETTE4_RGBA4_OES:                          return VkFormat.Undefined;                 // 4-component 4:4:4:4, 4-bit palette, unsigned normalized
				case GL_PALETTE4_RGB5_A1_OES:                        return VkFormat.Undefined;                 // 4-component 5:5:5:1, 4-bit palette, unsigned normalized
				case GL_PALETTE8_RGB8_OES:                           return VkFormat.Undefined;                 // 3-component 8:8:8, 8-bit palette, unsigned normalized
				case GL_PALETTE8_RGBA8_OES:                          return VkFormat.Undefined;                 // 4-component 8:8:8:8, 8-bit palette, unsigned normalized
				case GL_PALETTE8_R5_G6_B5_OES:                       return VkFormat.Undefined;                 // 3-component 5:6:5, 8-bit palette, unsigned normalized
				case GL_PALETTE8_RGBA4_OES:                          return VkFormat.Undefined;                 // 4-component 4:4:4:4, 8-bit palette, unsigned normalized
				case GL_PALETTE8_RGB5_A1_OES:                        return VkFormat.Undefined;                 // 4-component 5:5:5:1, 8-bit palette, unsigned normalized
				
				//
				// Depth/stencil
				//
				case GL_DEPTH_COMPONENT16:                           return VkFormat.D16Unorm;
				case GL_DEPTH_COMPONENT24:                           return VkFormat.X8D24UnormPack32;
				case GL_DEPTH_COMPONENT32:                           return VkFormat.Undefined;
				case GL_DEPTH_COMPONENT32F:                          return VkFormat.D32Sfloat;
				case GL_DEPTH_COMPONENT32F_NV:                       return VkFormat.D32Sfloat;
				case GL_STENCIL_INDEX1:                              return VkFormat.Undefined;
				case GL_STENCIL_INDEX4:                              return VkFormat.Undefined;
				case GL_STENCIL_INDEX8:                              return VkFormat.S8Uint;
				case GL_STENCIL_INDEX16:                             return VkFormat.Undefined;
				case GL_DEPTH24_STENCIL8:                            return VkFormat.D24UnormS8Uint;
				case GL_DEPTH32F_STENCIL8:                           return VkFormat.D32SfloatS8Uint;
				case GL_DEPTH32F_STENCIL8_NV:                        return VkFormat.D32SfloatS8Uint;
					
				default:                                             return VkFormat.Undefined;
			}
		}

		public static VkFormat vkGetFormatFromOpenGLFormat (uint format, uint type) {
			switch (type) {
				//
				// 8 bits per component
				//
				case GL_UNSIGNED_BYTE: {
						switch (format) {
							case GL_RED: 												return VkFormat.R8Unorm;
							case GL_RG: 												return VkFormat.R8g8Unorm;
							case GL_RGB: 												return VkFormat.R8g8b8Unorm;
							case GL_BGR: 												return VkFormat.B8g8r8Unorm;
							case GL_RGBA: 												return VkFormat.R8g8b8a8Unorm;
							case GL_BGRA: 												return VkFormat.B8g8r8a8Unorm;
							case GL_RED_INTEGER: 									return VkFormat.R8Uint;
							case GL_RG_INTEGER: 									return VkFormat.R8g8Uint;
							case GL_RGB_INTEGER: 									return VkFormat.R8g8b8Uint;
							case GL_BGR_INTEGER: 									return VkFormat.B8g8r8Uint;
							case GL_RGBA_INTEGER: 									return VkFormat.R8g8b8a8Uint;
							case GL_BGRA_INTEGER: 									return VkFormat.B8g8r8a8Uint;
							case GL_STENCIL_INDEX: 								return VkFormat.S8Uint;
							case GL_DEPTH_COMPONENT: 								return VkFormat.Undefined;
							case GL_DEPTH_STENCIL: 								return VkFormat.Undefined;
						}
						break;
					}
				case GL_BYTE: {
						switch (format) {
							case GL_RED: return VkFormat.R8Snorm;
							case GL_RG: return VkFormat.R8g8Snorm;
							case GL_RGB: return VkFormat.R8g8b8Snorm;
							case GL_BGR: return VkFormat.B8g8r8Snorm;
							case GL_RGBA: return VkFormat.R8g8b8a8Snorm;
							case GL_BGRA: return VkFormat.B8g8r8a8Snorm;
							case GL_RED_INTEGER: return VkFormat.R8Sint;
							case GL_RG_INTEGER: return VkFormat.R8g8Sint;
							case GL_RGB_INTEGER: return VkFormat.R8g8b8Sint;
							case GL_BGR_INTEGER: return VkFormat.B8g8r8Sint;
							case GL_RGBA_INTEGER: return VkFormat.R8g8b8a8Sint;
							case GL_BGRA_INTEGER: return VkFormat.B8g8r8a8Sint;
							case GL_STENCIL_INDEX: return VkFormat.Undefined;
							case GL_DEPTH_COMPONENT: return VkFormat.Undefined;
							case GL_DEPTH_STENCIL: return VkFormat.Undefined;
						}
						break;
					}

				//
				// 16 bits per component
				//
				case GL_UNSIGNED_SHORT: {
						switch (format) {
							case GL_RED: return VkFormat.R16Unorm;
							case GL_RG: return VkFormat.R16g16Unorm;
							case GL_RGB: return VkFormat.R16g16b16Unorm;
							case GL_BGR: return VkFormat.Undefined;
							case GL_RGBA: return VkFormat.R16g16b16a16Unorm;
							case GL_BGRA: return VkFormat.Undefined;
							case GL_RED_INTEGER: return VkFormat.R16Uint;
							case GL_RG_INTEGER: return VkFormat.R16g16Uint;
							case GL_RGB_INTEGER: return VkFormat.R16g16b16Uint;
							case GL_BGR_INTEGER: return VkFormat.Undefined;
							case GL_RGBA_INTEGER: return VkFormat.R16g16b16a16Uint;
							case GL_BGRA_INTEGER: return VkFormat.Undefined;
							case GL_STENCIL_INDEX: return VkFormat.Undefined;
							case GL_DEPTH_COMPONENT: return VkFormat.D16Unorm;
							case GL_DEPTH_STENCIL: return VkFormat.D16UnormS8Uint;
						}
						break;
					}
				case GL_SHORT: {
						switch (format) {
							case GL_RED: return VkFormat.R16Snorm;
							case GL_RG: return VkFormat.R16g16Snorm;
							case GL_RGB: return VkFormat.R16g16b16Snorm;
							case GL_BGR: return VkFormat.Undefined;
							case GL_RGBA: return VkFormat.R16g16b16a16Snorm;
							case GL_BGRA: return VkFormat.Undefined;
							case GL_RED_INTEGER: return VkFormat.R16Sint;
							case GL_RG_INTEGER: return VkFormat.R16g16Sint;
							case GL_RGB_INTEGER: return VkFormat.R16g16b16Sint;
							case GL_BGR_INTEGER: return VkFormat.Undefined;
							case GL_RGBA_INTEGER: return VkFormat.R16g16b16a16Sint;
							case GL_BGRA_INTEGER: return VkFormat.Undefined;
							case GL_STENCIL_INDEX: return VkFormat.Undefined;
							case GL_DEPTH_COMPONENT: return VkFormat.Undefined;
							case GL_DEPTH_STENCIL: return VkFormat.Undefined;
						}
						break;
					}
				case GL_HALF_FLOAT:
				case GL_HALF_FLOAT_OES: {
						switch (format) {
							case GL_RED: return VkFormat.R16Sfloat;
							case GL_RG: return VkFormat.R16g16Sfloat;
							case GL_RGB: return VkFormat.R16g16b16Sfloat;
							case GL_BGR: return VkFormat.Undefined;
							case GL_RGBA: return VkFormat.R16g16b16a16Sfloat;
							case GL_BGRA: return VkFormat.Undefined;
							case GL_RED_INTEGER: return VkFormat.Undefined;
							case GL_RG_INTEGER: return VkFormat.Undefined;
							case GL_RGB_INTEGER: return VkFormat.Undefined;
							case GL_BGR_INTEGER: return VkFormat.Undefined;
							case GL_RGBA_INTEGER: return VkFormat.Undefined;
							case GL_BGRA_INTEGER: return VkFormat.Undefined;
							case GL_STENCIL_INDEX: return VkFormat.Undefined;
							case GL_DEPTH_COMPONENT: return VkFormat.Undefined;
							case GL_DEPTH_STENCIL: return VkFormat.Undefined;
						}
						break;
					}

				//
				// 32 bits per component
				//
				case GL_UNSIGNED_INT: {
						switch (format) {
							case GL_RED: return VkFormat.R32Uint;
							case GL_RG: return VkFormat.R32g32Uint;
							case GL_RGB: return VkFormat.R32g32b32Uint;
							case GL_BGR: return VkFormat.Undefined;
							case GL_RGBA: return VkFormat.R32g32b32a32Uint;
							case GL_BGRA: return VkFormat.Undefined;
							case GL_RED_INTEGER: return VkFormat.R32Uint;
							case GL_RG_INTEGER: return VkFormat.R32g32Uint;
							case GL_RGB_INTEGER: return VkFormat.R32g32b32Uint;
							case GL_BGR_INTEGER: return VkFormat.Undefined;
							case GL_RGBA_INTEGER: return VkFormat.R32g32b32a32Uint;
							case GL_BGRA_INTEGER: return VkFormat.Undefined;
							case GL_STENCIL_INDEX: return VkFormat.Undefined;
							case GL_DEPTH_COMPONENT: return VkFormat.X8D24UnormPack32;
							case GL_DEPTH_STENCIL: return VkFormat.D24UnormS8Uint;
						}
						break;
					}
				case GL_INT: {
						switch (format) {
							case GL_RED: return VkFormat.R32Sint;
							case GL_RG: return VkFormat.R32g32Sint;
							case GL_RGB: return VkFormat.R32g32b32Sint;
							case GL_BGR: return VkFormat.Undefined;
							case GL_RGBA: return VkFormat.R32g32b32a32Sint;
							case GL_BGRA: return VkFormat.Undefined;
							case GL_RED_INTEGER: return VkFormat.R32Sint;
							case GL_RG_INTEGER: return VkFormat.R32g32Sint;
							case GL_RGB_INTEGER: return VkFormat.R32g32b32Sint;
							case GL_BGR_INTEGER: return VkFormat.Undefined;
							case GL_RGBA_INTEGER: return VkFormat.R32g32b32a32Sint;
							case GL_BGRA_INTEGER: return VkFormat.Undefined;
							case GL_STENCIL_INDEX: return VkFormat.Undefined;
							case GL_DEPTH_COMPONENT: return VkFormat.Undefined;
							case GL_DEPTH_STENCIL: return VkFormat.Undefined;
						}
						break;
					}
				case GL_FLOAT: {
						switch (format) {
							case GL_RED: return VkFormat.R32Sfloat;
							case GL_RG: return VkFormat.R32g32Sfloat;
							case GL_RGB: return VkFormat.R32g32b32Sfloat;
							case GL_BGR: return VkFormat.Undefined;
							case GL_RGBA: return VkFormat.R32g32b32a32Sfloat;
							case GL_BGRA: return VkFormat.Undefined;
							case GL_RED_INTEGER: return VkFormat.Undefined;
							case GL_RG_INTEGER: return VkFormat.Undefined;
							case GL_RGB_INTEGER: return VkFormat.Undefined;
							case GL_BGR_INTEGER: return VkFormat.Undefined;
							case GL_RGBA_INTEGER: return VkFormat.Undefined;
							case GL_BGRA_INTEGER: return VkFormat.Undefined;
							case GL_STENCIL_INDEX: return VkFormat.Undefined;
							case GL_DEPTH_COMPONENT: return VkFormat.D32Sfloat;
							case GL_DEPTH_STENCIL: return VkFormat.D32SfloatS8Uint;
						}
						break;
					}

				//
				// 64 bits per component
				//
				case GL_UNSIGNED_INT64: {
						switch (format) {
							case GL_RED: return VkFormat.R64Uint;
							case GL_RG: return VkFormat.R64g64Uint;
							case GL_RGB: return VkFormat.R64g64b64Uint;
							case GL_BGR: return VkFormat.Undefined;
							case GL_RGBA: return VkFormat.R64g64b64a64Uint;
							case GL_BGRA: return VkFormat.Undefined;
							case GL_RED_INTEGER: return VkFormat.Undefined;
							case GL_RG_INTEGER: return VkFormat.Undefined;
							case GL_RGB_INTEGER: return VkFormat.Undefined;
							case GL_BGR_INTEGER: return VkFormat.Undefined;
							case GL_RGBA_INTEGER: return VkFormat.Undefined;
							case GL_BGRA_INTEGER: return VkFormat.Undefined;
							case GL_STENCIL_INDEX: return VkFormat.Undefined;
							case GL_DEPTH_COMPONENT: return VkFormat.Undefined;
							case GL_DEPTH_STENCIL: return VkFormat.Undefined;
						}
						break;
					}
				case GL_INT64: {
						switch (format) {
							case GL_RED: return VkFormat.R64Sint;
							case GL_RG: return VkFormat.R64g64Sint;
							case GL_RGB: return VkFormat.R64g64b64Sint;
							case GL_BGR: return VkFormat.Undefined;
							case GL_RGBA: return VkFormat.R64g64b64a64Sint;
							case GL_BGRA: return VkFormat.Undefined;
							case GL_RED_INTEGER: return VkFormat.R64Sint;
							case GL_RG_INTEGER: return VkFormat.R64g64Sint;
							case GL_RGB_INTEGER: return VkFormat.R64g64b64Sint;
							case GL_BGR_INTEGER: return VkFormat.Undefined;
							case GL_RGBA_INTEGER: return VkFormat.R64g64b64a64Sint;
							case GL_BGRA_INTEGER: return VkFormat.Undefined;
							case GL_STENCIL_INDEX: return VkFormat.Undefined;
							case GL_DEPTH_COMPONENT: return VkFormat.Undefined;
							case GL_DEPTH_STENCIL: return VkFormat.Undefined;
						}
						break;
					}
				case GL_DOUBLE: {
						switch (format) {
							case GL_RED: return VkFormat.R64Sfloat;
							case GL_RG: return VkFormat.R64g64Sfloat;
							case GL_RGB: return VkFormat.R64g64b64Sfloat;
							case GL_BGR: return VkFormat.Undefined;
							case GL_RGBA: return VkFormat.R64g64b64a64Sfloat;
							case GL_BGRA: return VkFormat.Undefined;
							case GL_RED_INTEGER: return VkFormat.R64Sfloat;
							case GL_RG_INTEGER: return VkFormat.R64g64Sfloat;
							case GL_RGB_INTEGER: return VkFormat.R64g64b64Sfloat;
							case GL_BGR_INTEGER: return VkFormat.Undefined;
							case GL_RGBA_INTEGER: return VkFormat.R64g64b64a64Sfloat;
							case GL_BGRA_INTEGER: return VkFormat.Undefined;
							case GL_STENCIL_INDEX: return VkFormat.Undefined;
							case GL_DEPTH_COMPONENT: return VkFormat.Undefined;
							case GL_DEPTH_STENCIL: return VkFormat.Undefined;
						}
						break;
					}

				//
				// Packed
				//

				case GL_UNSIGNED_BYTE_3_3_2:
					Debug.Assert (format == GL_RGB || format == GL_RGB_INTEGER);
					return VkFormat.Undefined;
				case GL_UNSIGNED_BYTE_2_3_3_REV:
					Debug.Assert (format == GL_BGR || format == GL_BGR_INTEGER);
					return VkFormat.Undefined;
				case GL_UNSIGNED_SHORT_5_6_5:
					Debug.Assert (format == GL_RGB || format == GL_RGB_INTEGER);
					return VkFormat.R5g6b5UnormPack16;
				case GL_UNSIGNED_SHORT_5_6_5_REV:
					Debug.Assert (format == GL_BGR || format == GL_BGR_INTEGER);
					return VkFormat.B5g6r5UnormPack16;
				case GL_UNSIGNED_SHORT_4_4_4_4:
					Debug.Assert (format == GL_RGB || format == GL_BGRA || format == GL_RGB_INTEGER || format == GL_BGRA_INTEGER);
					return VkFormat.R4g4b4a4UnormPack16;
				case GL_UNSIGNED_SHORT_4_4_4_4_REV:
					Debug.Assert (format == GL_RGB || format == GL_BGRA || format == GL_RGB_INTEGER || format == GL_BGRA_INTEGER);
					return VkFormat.B4g4r4a4UnormPack16;
				case GL_UNSIGNED_SHORT_5_5_5_1:
					Debug.Assert (format == GL_RGB || format == GL_BGRA || format == GL_RGB_INTEGER || format == GL_BGRA_INTEGER);
					return VkFormat.R5g5b5a1UnormPack16;
				case GL_UNSIGNED_SHORT_1_5_5_5_REV:
					Debug.Assert (format == GL_RGB || format == GL_BGRA || format == GL_RGB_INTEGER || format == GL_BGRA_INTEGER);
					return VkFormat.A1r5g5b5UnormPack16;
				case GL_UNSIGNED_INT_8_8_8_8:
					Debug.Assert (format == GL_RGB || format == GL_BGRA || format == GL_RGB_INTEGER || format == GL_BGRA_INTEGER);
					return (format == GL_RGB_INTEGER || format == GL_BGRA_INTEGER) ? VkFormat.R8g8b8a8Uint : VkFormat.R8g8b8a8Unorm;
				case GL_UNSIGNED_INT_8_8_8_8_REV:
					Debug.Assert (format == GL_RGB || format == GL_BGRA || format == GL_RGB_INTEGER || format == GL_BGRA_INTEGER);
					return (format == GL_RGB_INTEGER || format == GL_BGRA_INTEGER) ? VkFormat.A8b8g8r8UintPack32 : VkFormat.A8b8g8r8UnormPack32;
				case GL_UNSIGNED_INT_10_10_10_2:
					Debug.Assert (format == GL_RGB || format == GL_BGRA || format == GL_RGB_INTEGER || format == GL_BGRA_INTEGER);
					return (format == GL_RGB_INTEGER || format == GL_BGRA_INTEGER) ? VkFormat.A2r10g10b10UintPack32 : VkFormat.A2r10g10b10UnormPack32;
				case GL_UNSIGNED_INT_2_10_10_10_REV:
					Debug.Assert (format == GL_RGB || format == GL_BGRA || format == GL_RGB_INTEGER || format == GL_BGRA_INTEGER);
					return (format == GL_RGB_INTEGER || format == GL_BGRA_INTEGER) ? VkFormat.A2b10g10r10UintPack32 : VkFormat.A2b10g10r10UnormPack32;
				case GL_UNSIGNED_INT_10F_11F_11F_REV:
					Debug.Assert (format == GL_RGB || format == GL_BGR);
					return VkFormat.B10g11r11UfloatPack32;
				case GL_UNSIGNED_INT_5_9_9_9_REV:
					Debug.Assert (format == GL_RGB || format == GL_BGR);
					return VkFormat.E5b9g9r9UfloatPack32;
				case GL_UNSIGNED_INT_24_8:
					Debug.Assert (format == GL_DEPTH_STENCIL);
					return VkFormat.D24UnormS8Uint;
				case GL_FLOAT_32_UNSIGNED_INT_24_8_REV:
					Debug.Assert (format == GL_DEPTH_STENCIL);
					return VkFormat.D32SfloatS8Uint;


			}

			return VkFormat.Undefined;
		}

		public static VkFormat vkGetFormatFromOpenGLType( uint type, uint numComponents, bool normalized )
		{
			switch (type) {
				//
				// 8 bits per component
				//
				case GL_UNSIGNED_BYTE: {
						switch (numComponents) {
							case 1: return normalized ? VkFormat.R8Unorm : VkFormat.R8Uint;
							case 2: return normalized ? VkFormat.R8g8Unorm : VkFormat.R8g8Uint;
							case 3: return normalized ? VkFormat.R8g8b8Unorm : VkFormat.R8g8b8Uint;
							case 4: return normalized ? VkFormat.R8g8b8a8Unorm : VkFormat.R8g8b8a8Uint;
						}
						break;
					}
				case GL_BYTE: {
						switch (numComponents) {
							case 1: return normalized ? VkFormat.R8Snorm : VkFormat.R8Sint;
							case 2: return normalized ? VkFormat.R8g8Snorm : VkFormat.R8g8Sint;
							case 3: return normalized ? VkFormat.R8g8b8Snorm : VkFormat.R8g8b8Sint;
							case 4: return normalized ? VkFormat.R8g8b8a8Snorm : VkFormat.R8g8b8a8Sint;
						}
						break;
					}

				//
				// 16 bits per component
				//
				case GL_UNSIGNED_SHORT: {
						switch (numComponents) {
							case 1: return normalized ? VkFormat.R16Unorm : VkFormat.R16Uint;
							case 2: return normalized ? VkFormat.R16g16Unorm : VkFormat.R16g16Uint;
							case 3: return normalized ? VkFormat.R16g16b16Unorm : VkFormat.R16g16b16Uint;
							case 4: return normalized ? VkFormat.R16g16b16a16Unorm : VkFormat.R16g16b16a16Uint;
						}
						break;
					}
				case GL_SHORT: {
						switch (numComponents) {
							case 1: return normalized ? VkFormat.R16Snorm : VkFormat.R16Sint;
							case 2: return normalized ? VkFormat.R16g16Snorm : VkFormat.R16g16Sint;
							case 3: return normalized ? VkFormat.R16g16b16Snorm : VkFormat.R16g16b16Sint;
							case 4: return normalized ? VkFormat.R16g16b16a16Snorm : VkFormat.R16g16b16a16Sint;
						}
						break;
					}
				case GL_HALF_FLOAT:
				case GL_HALF_FLOAT_OES: {
						switch (numComponents) {
							case 1: return VkFormat.R16Sfloat;
							case 2: return VkFormat.R16g16Sfloat;
							case 3: return VkFormat.R16g16b16Sfloat;
							case 4: return VkFormat.R16g16b16a16Sfloat;
						}
						break;
					}

				//
				// 32 bits per component
				//
				case GL_UNSIGNED_INT: {
						switch (numComponents) {
							case 1: return VkFormat.R32Uint;
							case 2: return VkFormat.R32g32Uint;
							case 3: return VkFormat.R32g32b32Uint;
							case 4: return VkFormat.R32g32b32a32Uint;
						}
						break;
					}
				case GL_INT: {
						switch (numComponents) {
							case 1: return VkFormat.R32Sint;
							case 2: return VkFormat.R32g32Sint;
							case 3: return VkFormat.R32g32b32Sint;
							case 4: return VkFormat.R32g32b32a32Sint;
						}
						break;
					}
				case GL_FLOAT: {
						switch (numComponents) {
							case 1: return VkFormat.R32Sfloat;
							case 2: return VkFormat.R32g32Sfloat;
							case 3: return VkFormat.R32g32b32Sfloat;
							case 4: return VkFormat.R32g32b32a32Sfloat;
						}
						break;
					}

				//
				// 64 bits per component
				//
				case GL_UNSIGNED_INT64: {
						switch (numComponents) {
							case 1: return VkFormat.R64Uint;
							case 2: return VkFormat.R64g64Uint;
							case 3: return VkFormat.R64g64b64Uint;
							case 4: return VkFormat.R64g64b64a64Uint;
						}
						break;
					}
				case GL_INT64: {
						switch (numComponents) {
							case 1: return VkFormat.R64Sint;
							case 2: return VkFormat.R64g64Sint;
							case 3: return VkFormat.R64g64b64Sint;
							case 4: return VkFormat.R64g64b64a64Sint;
						}
						break;
					}
				case GL_DOUBLE: {
						switch (numComponents) {
							case 1: return VkFormat.R64Sfloat;
							case 2: return VkFormat.R64g64Sfloat;
							case 3: return VkFormat.R64g64b64Sfloat;
							case 4: return VkFormat.R64g64b64a64Sfloat;
						}
						break;
					}

				//
				// Packed
				//
				case GL_UNSIGNED_BYTE_3_3_2: return VkFormat.Undefined;
				case GL_UNSIGNED_BYTE_2_3_3_REV: return VkFormat.Undefined;
				case GL_UNSIGNED_SHORT_5_6_5: return VkFormat.R5g6b5UnormPack16;
				case GL_UNSIGNED_SHORT_5_6_5_REV: return VkFormat.B5g6r5UnormPack16;
				case GL_UNSIGNED_SHORT_4_4_4_4: return VkFormat.R4g4b4a4UnormPack16;
				case GL_UNSIGNED_SHORT_4_4_4_4_REV: return VkFormat.B4g4r4a4UnormPack16;
				case GL_UNSIGNED_SHORT_5_5_5_1: return VkFormat.R5g5b5a1UnormPack16;
				case GL_UNSIGNED_SHORT_1_5_5_5_REV: return VkFormat.A1r5g5b5UnormPack16;
				case GL_UNSIGNED_INT_8_8_8_8: return normalized ? VkFormat.R8g8b8a8Unorm : VkFormat.R8g8b8a8Uint;
				case GL_UNSIGNED_INT_8_8_8_8_REV: return normalized ? VkFormat.A8b8g8r8UnormPack32 : VkFormat.A8b8g8r8UintPack32;
				case GL_UNSIGNED_INT_10_10_10_2: return normalized ? VkFormat.A2r10g10b10UnormPack32 : VkFormat.A2r10g10b10UintPack32;
				case GL_UNSIGNED_INT_2_10_10_10_REV: return normalized ? VkFormat.A2b10g10r10UnormPack32 : VkFormat.A2b10g10r10UintPack32;
				case GL_UNSIGNED_INT_10F_11F_11F_REV: return VkFormat.B10g11r11UfloatPack32;
				case GL_UNSIGNED_INT_5_9_9_9_REV: return VkFormat.E5b9g9r9UfloatPack32;
				case GL_UNSIGNED_INT_24_8: return VkFormat.D24UnormS8Uint;
				case GL_FLOAT_32_UNSIGNED_INT_24_8_REV: return VkFormat.D32SfloatS8Uint;
			}

			return VkFormat.Undefined;
		}
		


	}
}
