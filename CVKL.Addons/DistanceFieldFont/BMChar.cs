// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
namespace CVKL.DistanceFieldFont {
	public struct BMChar {
		public enum Channel {
			Blue = 0x01,
			Green = 0x02,
			Red = 0x04,
			Alpha = 0x08,
			All = 0x0f
		}
		public uint id;
		/// <summary>
		/// The left position of the character image in the texture.
		/// </summary>
		public uint x;
		public uint y;
		/// <summary>
		/// The width of the character image in the texture.
		/// </summary>
		public uint width;
		/// <summary>
		/// The height of the character image in the texture.
		/// </summary>
		public uint height;
		/// <summary>
		/// How much the current position should be offset when copying the image from the texture to the screen.
		/// </summary>
		public int xoffset;
		/// <summary>
		/// How much the current position should be offset when copying the image from the texture to the screen.
		/// </summary>
		public int yoffset;
		/// <summary>
		/// How much the current position should be advanced after drawing the character.
		/// </summary>
		public int xadvance;
		/// <summary>
		/// The texture page where the character image is found.
		/// </summary>
		public uint page;
		/// <summary>
		/// The texture channel where the character image is found (1 = blue, 2 = green, 4 = red, 8 = alpha, 15 = all channels).	 
		/// </summary>
		public Channel channel;
	}
}
