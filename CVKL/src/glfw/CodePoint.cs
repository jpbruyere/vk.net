// Copyright (c) 2019 Andrew Armstrong/FacticiusVir
// Copyright (c) 2019 Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System.Runtime.InteropServices;
using System.Text;

namespace Glfw
{
	/// <summary>
	/// Represents a native UTF32 codepoint.
	/// </summary>
	[StructLayout (LayoutKind.Explicit)]
	public struct CodePoint
    {
		/// <summary>
		/// The numeric value of the codepoint.
		/// </summary>
		[FieldOffset (0)] public readonly uint Value;
		[FieldOffset (0)] readonly byte byte0;
		[FieldOffset (1)] readonly byte byte1;
		[FieldOffset (2)] readonly byte byte2;
		[FieldOffset (3)] readonly byte byte3;
		/// <summary>
		/// Casts the codepoint to System.Char.
		/// </summary>
		/// <returns>
		/// The character representation of the codepoint.
		/// </returns>
		public char ToChar() => Encoding.UTF32.GetChars (new byte[] { byte0, byte0, byte0, byte0 })[0];
        /// <summary>
        /// Converts the value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string containing the character representation of the codepoint.
        /// </returns>
        public override string ToString() => ToChar().ToString();
    }
}
