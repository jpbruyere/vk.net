//
// BoundingBox
//
// Authors:
//       Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
// 		 https://github.com/mellinoe
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
using System.Numerics;
/* code derived from https://github.com/mellinoe*/
namespace CVKL {
	public struct BoundingBox {
		public Vector3 min;
		public Vector3 max;
		public bool isValid;
		public BoundingBox (Vector3 min, Vector3 max, bool isValid = false) {
			this.min = min;
			this.max = max;
			this.isValid = isValid;
		}
		public BoundingBox getAABB (Matrix4x4 m) {
			if (!isValid)
				return default (BoundingBox);
			Vector3 mini = new Vector3 (m.M41, m.M42, m.M43);
			Vector3 maxi = mini;
			Vector3 v0, v1;

			Vector3 right = new Vector3 (m.M11, m.M12, m.M13);
			v0 = right * this.min.X;
			v1 = right * this.max.X;
			mini += Vector3.Min (v0, v1);
			maxi += Vector3.Max (v0, v1);

			Vector3 up = new Vector3 (m.M21, m.M22, m.M23);
			v0 = up * this.min.Y;
			v1 = up * this.max.Y;
			mini += Vector3.Min (v0, v1);
			maxi += Vector3.Max (v0, v1);

			Vector3 back = new Vector3 (m.M31, m.M32, m.M33);
			v0 = back * this.min.Z;
			v1 = back * this.max.Z;
			mini += Vector3.Min (v0, v1);
			maxi += Vector3.Max (v0, v1);

			return new BoundingBox (mini, maxi, true);
		}

		public float Width => max.X - min.X;
		public float Height => max.Y - min.Y;
		public float Depth => max.Z - min.Z;

		public Vector3 Center => new Vector3 (Width / 2f + min.X, Height / 2f + min.Y, Depth / 2f + min.Z);

		public static BoundingBox operator +(BoundingBox bb1, BoundingBox bb2) {
			return bb1.isValid ? bb2.isValid ? new BoundingBox (Vector3.Min (bb1.min, bb2.min), Vector3.Min (bb1.max, bb2.max),true) : bb1 : bb2.isValid ? bb2 : default(BoundingBox);
		}
		public override string ToString () => isValid ? string.Format ($" {min}->{max}") : "Invalid";
	}
}
