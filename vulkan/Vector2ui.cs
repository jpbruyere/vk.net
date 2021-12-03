// Copyright (c) 2013-2022  Bruyère Jean-Philippe jp_bruyere@hotmail.com
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)

using System;

namespace Vulkan
{
	public struct Vector2ui : IEquatable<Vector2ui>, IEquatable<uint>
    {
		public static readonly Vector2ui UnitX = new Vector2ui (1,0);
		public static readonly Vector2ui UnitY = new Vector2ui (0,1);

		public uint X, Y;

		#region CTOR
		public Vector2ui (uint x, uint y) {
			X = x;
			Y = y;
		}
		public Vector2ui (uint pos) {
			X = pos;
			Y = pos;
		}
		#endregion

		public uint Length => (uint)Math.Sqrt (Math.Pow (X, 2) + Math.Pow (Y, 2));
		public double LengthD => Math.Sqrt (Math.Pow (X, 2) + Math.Pow (Y, 2));
		public Vector2ui Normalized {
			get {
				uint l = Length;
				return new Vector2ui (X / l, Y / l);
			}
		}
		//public static implicit operator PointD (Vector2i p) => new PointD (p.X, p.Y);
		public static implicit operator Vector2ui (uint i) => new Vector2ui (i, i);

		public static Vector2ui operator + (Vector2ui p1, Vector2ui p2) => new Vector2ui (p1.X + p2.X, p1.Y + p2.Y);
		public static Vector2ui operator + (Vector2ui p, uint i) => new Vector2ui (p.X + i, p.Y + i);
		public static Vector2ui operator - (Vector2ui p1, Vector2ui p2) => new Vector2ui (p1.X - p2.X, p1.Y - p2.Y);
		public static Vector2ui operator - (Vector2ui p, uint i) => new Vector2ui (p.X - i, p.Y - i);
		public static Vector2ui operator * (Vector2ui p1, Vector2ui p2) => new Vector2ui (p1.X * p2.X, p1.Y * p2.Y);
		public static Vector2ui operator * (Vector2ui p, uint d) => new Vector2ui (p.X * d, p.Y * d);
		public static Vector2ui operator / (Vector2ui p1, Vector2ui p2) => new Vector2ui (p1.X / p2.X, p1.Y / p2.Y);
		public static Vector2ui operator / (Vector2ui p, uint d) => new Vector2ui (p.X / d, p.Y / d);

		public static bool operator == (Vector2ui s1, Vector2ui s2) => s1.Equals (s2);
		public static bool operator != (Vector2ui s1, Vector2ui s2) => !s1.Equals (s2);
		public static bool operator == (Vector2ui s, uint i) => s.Equals(i);
		public static bool operator != (Vector2ui s, uint i) => !s.Equals (i);
		public static bool operator > (Vector2ui p1, Vector2ui p2) => p1.X > p2.X && p1.Y > p2.Y;
		public static bool operator > (Vector2ui s, uint i) => s.X > i && s.Y > i;
		public static bool operator < (Vector2ui p1, Vector2ui p2) => p1.X < p2.X && p1.Y < p2.Y;
		public static bool operator < (Vector2ui s, uint i) => s.X < i && s.Y < i;
		public static bool operator >= (Vector2ui p1, Vector2ui p2) => p1.X >= p2.X && p1.Y >= p2.Y;
		public static bool operator >= (Vector2ui s, uint i) => s.X >= i && s.Y >= i;
		public static bool operator <= (Vector2ui p1, Vector2ui p2) => p1.X <= p2.X && p1.Y <= p2.Y;
		public static bool operator <= (Vector2ui s, uint i) => s.X <= i && s.Y <= i;

		public bool Equals (Vector2ui other) => X == other.X && Y == other.Y;
		public bool Equals (uint other) => X == other && Y == other;


		public override int GetHashCode () => HashCode.Combine (X, Y);
		public override bool Equals (object obj) => obj is Vector2ui s ? Equals (s) : false;
		public override string ToString () => $"{X},{Y}";
		public static Vector2ui Parse (string s) {
			ReadOnlySpan<char> tmp = s.AsSpan ();
			if (tmp.Length == 0)
				return default (Vector2ui);
			int ioc = tmp.IndexOf (',');
			return ioc < 0 ? new Vector2ui (uint.Parse (tmp)) : new Vector2ui (
				uint.Parse (tmp.Slice (0, ioc)),
				uint.Parse (tmp.Slice (ioc + 1)));
		}
    }
}
