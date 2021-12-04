// Copyright (c) 2022  Bruyère Jean-Philippe jp_bruyere@hotmail.com
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)

using System;

namespace Vulkan
{
	public struct Vector2i : IEquatable<Vector2i>, IEquatable<int>
    {
		public static readonly Vector2i UnitX = new Vector2i (1,0);
		public static readonly Vector2i UnitY = new Vector2i (0,1);

		public int X, Y;

		#region CTOR
		public Vector2i (int x, int y) {
			X = x;
			Y = y;
		}
		public Vector2i (int pos) {
			X = pos;
			Y = pos;
		}
		#endregion

		public int Length => (int)Math.Sqrt (Math.Pow (X, 2) + Math.Pow (Y, 2));
		public double LengthD => Math.Sqrt (Math.Pow (X, 2) + Math.Pow (Y, 2));
		public Vector2i Normalized {
			get {
				int l = Length;
				return new Vector2i (X / l, Y / l);
			}
		}
		//public static implicit operator PointD (Vector2i p) => new PointD (p.X, p.Y);
		public static implicit operator Vector2i (int i) => new Vector2i (i, i);

		public static Vector2i operator + (Vector2i p1, Vector2i p2) => new Vector2i (p1.X + p2.X, p1.Y + p2.Y);
		public static Vector2i operator + (Vector2i p, int i) => new Vector2i (p.X + i, p.Y + i);
		public static Vector2i operator - (Vector2i p1, Vector2i p2) => new Vector2i (p1.X - p2.X, p1.Y - p2.Y);
		public static Vector2i operator - (Vector2i p, int i) => new Vector2i (p.X - i, p.Y - i);
		public static Vector2i operator * (Vector2i p1, Vector2i p2) => new Vector2i (p1.X * p2.X, p1.Y * p2.Y);
		public static Vector2i operator * (Vector2i p, int d) => new Vector2i (p.X * d, p.Y * d);
		public static Vector2i operator / (Vector2i p1, Vector2i p2) => new Vector2i (p1.X / p2.X, p1.Y / p2.Y);
		public static Vector2i operator / (Vector2i p, int d) => new Vector2i (p.X / d, p.Y / d);

		public static bool operator == (Vector2i s1, Vector2i s2) => s1.Equals (s2);
		public static bool operator != (Vector2i s1, Vector2i s2) => !s1.Equals (s2);
		public static bool operator == (Vector2i s, int i) => s.Equals(i);
		public static bool operator != (Vector2i s, int i) => !s.Equals (i);
		public static bool operator > (Vector2i p1, Vector2i p2) => p1.X > p2.X && p1.Y > p2.Y;
		public static bool operator > (Vector2i s, int i) => s.X > i && s.Y > i;
		public static bool operator < (Vector2i p1, Vector2i p2) => p1.X < p2.X && p1.Y < p2.Y;
		public static bool operator < (Vector2i s, int i) => s.X < i && s.Y < i;
		public static bool operator >= (Vector2i p1, Vector2i p2) => p1.X >= p2.X && p1.Y >= p2.Y;
		public static bool operator >= (Vector2i s, int i) => s.X >= i && s.Y >= i;
		public static bool operator <= (Vector2i p1, Vector2i p2) => p1.X <= p2.X && p1.Y <= p2.Y;
		public static bool operator <= (Vector2i s, int i) => s.X <= i && s.Y <= i;

		public bool Equals (Vector2i other) => X == other.X && Y == other.Y;
		public bool Equals (int other) => X == other && Y == other;


		public override int GetHashCode () => HashCode.Combine (X, Y);
		public override bool Equals (object obj) => obj is Vector2i s ? Equals (s) : false;
		public override string ToString () => $"{X},{Y}";
		public static Vector2i Parse (string s) {
			ReadOnlySpan<char> tmp = s.AsSpan ();
			if (tmp.Length == 0)
				return default (Vector2i);
			int ioc = tmp.IndexOf (',');
			return ioc < 0 ? new Vector2i (int.Parse (tmp)) : new Vector2i (
				int.Parse (tmp.Slice (0, ioc)),
				int.Parse (tmp.Slice (ioc + 1)));
		}
    }
}
