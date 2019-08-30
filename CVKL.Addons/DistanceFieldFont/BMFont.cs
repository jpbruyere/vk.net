// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using VK;

namespace CVKL.DistanceFieldFont {
	/// <summary>
	/// BMF font. (http://www.angelcode.com/products/bmfont/doc/file_format.html)
	/// </summary>
	public class BMFont {
		/// <summary>
		/// distance in pixels between each line of text.
		/// </summary>
		public readonly ushort lineHeight;
		/// <summary>
		/// The number of pixels from the absolute top of the line to the base of the characters.
		/// </summary>
		public readonly ushort @base;
		public readonly ushort width, height;

		public readonly Dictionary<int, BMChar> CharMap = new Dictionary<int, BMChar> ();
		public readonly Dictionary<int, string> PageImagePathes = new Dictionary<int, string> ();

		readonly string fntFilePath;

		public BMFont (string path) {
			fntFilePath = path;
			object thisFont = this;


			using (BMFontStreamReader bmf = new BMFontStreamReader (path)) {
				while (!bmf.EndOfStream) {

					string w = bmf.ReadWord ();
					if (string.IsNullOrEmpty (w))
						continue;

					if (string.Equals (w, "info", StringComparison.OrdinalIgnoreCase) ||
						string.Equals (w, "common", StringComparison.OrdinalIgnoreCase))
						bmf.ReadDatas (ref thisFont);
					else if (string.Equals (w, "page", StringComparison.OrdinalIgnoreCase)) {
						int p = bmf.ReadPageDatas (out string pageImg);
						PageImagePathes.Add (p, pageImg);
					} else if (string.Equals (w, "chars", StringComparison.OrdinalIgnoreCase))
						bmf.ReadLine ();//skip char count
					else if (string.Equals (w, "char", StringComparison.OrdinalIgnoreCase)) {
						BMChar c = bmf.NextBMChar;
						CharMap.Add ((int)c.id, c);
					} else if (string.Equals (w, "kernings", StringComparison.OrdinalIgnoreCase))
						bmf.ReadLine ();//skip
					else if (string.Equals (w, "kerning", StringComparison.OrdinalIgnoreCase))
						bmf.ReadLine ();//skip
				}
			}
		}

		public Image GetPageTexture (int page, Queue staggingQ, CommandPool cmdPool,
			VkMemoryPropertyFlags imgProp = VkMemoryPropertyFlags.DeviceLocal, bool genMipMaps = true, VkImageTiling tiling = VkImageTiling.Optimal) {

			string path = Path.Combine (Path.GetDirectoryName (fntFilePath), PageImagePathes[page]);

			if (path.EndsWith ("ktx", StringComparison.OrdinalIgnoreCase))
				return KTX.KTX.Load (staggingQ, cmdPool, path,
					VkImageUsageFlags.Sampled, imgProp, genMipMaps, tiling);
			return Image.Load (staggingQ.Dev, staggingQ, cmdPool, path, VkFormat.R8g8b8a8Unorm, imgProp, tiling, genMipMaps);
		}

	}



	internal class BMFontStreamReader : StreamReader {
		public BMFontStreamReader (string bmPath) : base (bmPath) { }

		public char PeekedChar => (char)Peek ();
		public char ReadedChar => (char)Read ();
		public bool EndOfLine => PeekedChar == '\n';

		public string ReadWord () {
			string word = "";
			SkipWhiteSpacesAndLineBreak ();
			while (!EndOfStream) {
				if (!Char.IsLetterOrDigit (PeekedChar))
					break;
				word += ReadedChar;
			}
			return word;
		}
		public void SkipWhiteSpaces () {
			while (!EndOfStream) {
				if (!char.IsWhiteSpace (PeekedChar) || PeekedChar == '\n')
					break;
				Read ();
			}
		}
		public void SkipWhiteSpacesAndLineBreak () {
			while (!EndOfStream) {
				if (!char.IsWhiteSpace (PeekedChar))
					break;
				Read ();
			}
		}

		public string NextValue {
			get {
				SkipWhiteSpaces ();
				if (ReadedChar != '=')
					throw new Exception ("expecting '='");
				SkipWhiteSpaces ();
				if (PeekedChar == '"') {
					Read ();
					return ReadUntil ('"');
				}
				return ReadUntilSpace ();
			}
		}
		public string ReadUntil (char limit) {
			string word = "";
			while (!EndOfStream) {
				char c = ReadedChar;
				if (c == limit)
					break;
				word += c;
			}
			return word;
		}
		public string ReadUntilSpace () {
			string word = "";
			while (!EndOfStream) {
				if (char.IsWhiteSpace (PeekedChar))
					break;
				word += ReadedChar;
			}
			return word;
		}

		public void ReadField (ref object obj, string fieldName) {
			FieldInfo fi = obj.GetType ().GetField (fieldName);
			if (fi == null) {
				System.Diagnostics.Debug.WriteLine ($"BMFont property not handled: {fieldName}");
				string tmp = NextValue;
				return;
			}
			if (fi.FieldType == typeof (ushort)) {
				fi.SetValue (obj, ushort.Parse (NextValue));
				return;
			}
			if (fi.FieldType == typeof (short)) {
				fi.SetValue (obj, short.Parse (NextValue));
				return;
			}
			if (fi.FieldType == typeof (int)) {
				fi.SetValue (obj, int.Parse (NextValue));
				return;
			}
			if (fi.FieldType == typeof (uint)) {
				fi.SetValue (obj, uint.Parse (NextValue));
				return;
			}
			if (fi.FieldType == typeof (string)) {
				fi.SetValue (obj, NextValue);
				return;
			}
		}
		public void ReadDatas (ref object obj) {
			while (!EndOfLine && !EndOfStream) {
				string field = ReadWord ();
				ReadField (ref obj, field);
			}

		}
		public int ReadPageDatas (out string pageImgPath) {
			int idx = 0;
			pageImgPath = "";

			while (!EndOfLine && !EndOfStream) {
				string field = ReadWord ();
				if (string.Equals (field, "id", StringComparison.OrdinalIgnoreCase))
					idx = int.Parse (NextValue);
				else if (string.Equals (field, "file", StringComparison.OrdinalIgnoreCase))
					pageImgPath = NextValue;
				else
					Console.WriteLine ($"BMFont page property not handled: {field} = {NextValue}");
			}
			return idx;
		}
		public BMChar NextBMChar {
			get {
				object c = default (BMChar);

				while (!EndOfLine && !EndOfStream) {
					string field = ReadWord ();
					ReadField (ref c, field);
					SkipWhiteSpaces ();//trailing spaces
				}

				return (BMChar)c;
			}

		}
	}

}
