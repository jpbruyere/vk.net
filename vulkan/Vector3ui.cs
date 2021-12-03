// Copyright (c) 2013-2022  Bruyère Jean-Philippe jp_bruyere@hotmail.com
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)

using System;

namespace Vulkan
{
	public struct Vector3ui : IEquatable<Vector3ui>, IEquatable<uint>
    {
		public static readonly Vector3ui UnitX = new Vector3ui (1,0,0);
		public static readonly Vector3ui UnitY = new Vector3ui (0,1,0);
		public static readonly Vector3ui UnitZ = new Vector3ui (0,1,0);

		public uint X, Y, Z;

		#region CTOR
		public Vector3ui (uint x, uint y, uint z) {
			X = x;
			Y = y;
			Z = z;

		}
		public Vector3ui (uint pos) {
			X = pos;
			Y = pos;
			Z = pos;
		}
		#endregion

		/*public uint Length => (uint)Math.Sqrt (Math.Pow (X, 2) + Math.Pow (Y, 2));
		public double LengthD => Math.Sqrt (Math.Pow (X, 2) + Math.Pow (Y, 2));
		public Vector3ui Normalized {
			get {
				uint l = Length;
				return new Vector3ui (X / l, Y / l);
			}
		}*/
		//public static implicit operator PointD (Vector2i p) => new PointD (p.X, p.Y);
		public static implicit operator Vector3ui (uint i) => new Vector3ui (i, i, i);

		public static Vector3ui operator + (Vector3ui p1, Vector3ui p2) => new Vector3ui (p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
		public static Vector3ui operator + (Vector3ui p, uint i) => new Vector3ui (p.X + i, p.Y + i, p.Z + i);
		public static Vector3ui operator - (Vector3ui p1, Vector3ui p2) => new Vector3ui (p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
		public static Vector3ui operator - (Vector3ui p, uint i) => new Vector3ui (p.X - i, p.Y - i, p.Z - i);
		public static Vector3ui operator * (Vector3ui p1, Vector3ui p2) => new Vector3ui (p1.X * p2.X, p1.Y * p2.Y, p1.Z * p2.Z);
		public static Vector3ui operator * (Vector3ui p, uint d) => new Vector3ui (p.X * d, p.Y * d, p.Z * d);
		public static Vector3ui operator / (Vector3ui p1, Vector3ui p2) => new Vector3ui (p1.X / p2.X, p1.Y / p2.Y, p1.Z / p2.Z);
		public static Vector3ui operator / (Vector3ui p, uint d) => new Vector3ui (p.X / d, p.Y / d, p.Z / d);

		public static bool operator == (Vector3ui s1, Vector3ui s2) => s1.Equals (s2);
		public static bool operator != (Vector3ui s1, Vector3ui s2) => !s1.Equals (s2);
		public static bool operator == (Vector3ui s, uint i) => s.Equals(i);
		public static bool operator != (Vector3ui s, uint i) => !s.Equals (i);
		public static bool operator > (Vector3ui p1, Vector3ui p2) => p1.X > p2.X && p1.Y > p2.Y && p1.Z > p2.Z;
		public static bool operator > (Vector3ui s, uint i) => s.X > i && s.Y > i && s.Z > i;
		public static bool operator < (Vector3ui p1, Vector3ui p2) => p1.X < p2.X && p1.Y < p2.Y && p1.Z < p2.Z;
		public static bool operator < (Vector3ui s, uint i) => s.X < i && s.Y < i && s.Z < i;
		public static bool operator >= (Vector3ui p1, Vector3ui p2) => p1.X >= p2.X && p1.Y >= p2.Y && p1.Z >= p2.Z;
		public static bool operator >= (Vector3ui s, uint i) => s.X >= i && s.Y >= i && s.Z >= i;
		public static bool operator <= (Vector3ui p1, Vector3ui p2) => p1.X <= p2.X && p1.Y <= p2.Y && p1.Z <= p2.Z;
		public static bool operator <= (Vector3ui s, uint i) => s.X <= i && s.Y <= i && s.Z <= i;

		public bool Equals (Vector3ui other) => X == other.X && Y == other.Y && Z == other.Z;
		public bool Equals (uint other) => X == other && Y == other && Z == other;


		public override int GetHashCode () => HashCode.Combine (X, Y, Z);
		public override bool Equals (object obj) => obj is Vector3ui s ? Equals (s) : false;
		public override string ToString () => $"{X},{Y},{Z}";
		public static Vector3ui Parse (string s) {
			ReadOnlySpan<char> tmp = s.AsSpan ();
			if (tmp.Length == 0)
				return default (Vector3ui);
			int ioc = tmp.IndexOf (',');
			return ioc < 0 ? new Vector3ui (uint.Parse (tmp)) : new Vector3ui (
				uint.Parse (tmp.Slice (0, ioc)),
				uint.Parse (tmp.Slice (ioc + 1)),
				uint.Parse (tmp.Slice (ioc + 2)));
		}
    }
}
