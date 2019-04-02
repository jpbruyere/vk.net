using System;
using System.Numerics;
using System.Runtime.InteropServices;

using Vulkan;


namespace VKE {
    public class Camera {
		public enum Type { Orthographic, Perspective, FieldOfView };
		public enum  Dynamic { FirstPerson, LookAt };

		float fov = Utils.DegreesToRadians(60);
		float width = 1024f, height = 1024f, zNear = 0.1f, zFar = 250.0f, zoom = 1.0f;

		Vector3 position = new Vector3 (0, 0, -4);

		Vector3 rotation = Vector3.Zero;

		Vector3 viewVector = Vector3.UnitZ;
		Dynamic dynamic = Camera.Dynamic.LookAt;
		Type camType = Type.Orthographic;

		public float moveSpeed = 0.1f, rotationSpeed = 0.01f, zoomSpeed = 0.01f;
		Vector3 upVector = -Vector3.UnitY;

		public Vector3 UpVector {
			get { return upVector; }
			set {
				upVector = value;
				updateView ();
			}
		}
		public Vector3 Target {
			get {
				return position + viewVector * zoom;;
			}
			set {
				viewVector = Vector3.Normalize (value - position);
			}
		}

		public Matrix4x4 Projection { private set; get; }
		public Matrix4x4 View { private set; get; }
		public Dynamic CamDynamic {
			get { return dynamic; }
			set {
				dynamic = value;
				updateView ();
			}
		}
		public Type CamType {
			get { return camType; }
			set {
				camType = value;
				updateProjection ();
			}
		}

		public float Width {
			get { return width; }
			set {
				width = value;
				updateProjection ();
			}
		}
		public float Height {
			get { return height; }
			set {
				height = value;
				updateProjection ();
			}
		}
		public float NearPlane {
			get { return zNear; }
			set {
				zNear = value;
				updateProjection ();
			}
		}
		public float FarPlane {
			get { return zFar; }
			set {
				zFar = value;
				updateProjection ();
			}
		}

		public Vector3 Position {
			get { return position; }
			set {
				position = value;
				updateView (); 
			}
		}
		public float FieldOfView {
			get { return fov; }
			set {
				fov = value;
				camType = Type.FieldOfView;
				updateProjection ();
			}
		
		}

		public Camera (Type type = Type.FieldOfView, Dynamic _dynamic = Dynamic.LookAt) {
			camType = type;
			dynamic = _dynamic;
			updateView ();
			updateProjection ();
		}

		public void Zoom (float factor) {
			position += Position * factor * zoomSpeed;
			updateView ();
		}
		public void Rotate (float x, float y, float z) {
			rotation.X += rotationSpeed * x;
			rotation.Y += rotationSpeed * y;
			rotation.Z += rotationSpeed * z;

			updateView ();
		}
		public void Move (float x, float y, float z) {
			position.X += moveSpeed * x;
			position.Y += moveSpeed * y;
			position.Z += moveSpeed * z;

			updateView ();
		}

		void updateView () {
			Matrix4x4 rot =
				Matrix4x4.CreateFromAxisAngle (Vector3.UnitX, rotation.X) *
				Matrix4x4.CreateFromAxisAngle (Vector3.UnitY, rotation.Y) *
				Matrix4x4.CreateFromAxisAngle (Vector3.UnitZ, rotation.Z);
			Matrix4x4 translate = Matrix4x4.CreateTranslation (position);

			switch (dynamic) {
				case Dynamic.FirstPerson:
					View = translate * rot;
					break;
				case Dynamic.LookAt:
					View = rot * translate;
					break;				
			}
		}
		void updateProjection () {
			switch (camType) {
				case Type.Orthographic:
					Projection = Matrix4x4.CreateOrthographic (20f, 20f, zNear, zFar);
					break;
				case Type.Perspective:
					Projection = Matrix4x4.CreatePerspective (20f, 20f, zNear, zFar);
					break;
				case Type.FieldOfView:
					Projection = Matrix4x4.CreatePerspectiveFieldOfView (fov, width / height, zNear, zFar);
					break;
			}

		}
	}
}
