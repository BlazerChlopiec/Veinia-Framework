using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.ComponentModel;

namespace VeiniaFramework
{
	public class Transform : Component
	{
		//pixels per unit is assigned in the veinia constructor
		public static int unitSize = 100;

		public static Transform Empty => new Transform(0, 0);

		[Browsable(false)] public Transform parent;

		public Vector2 position
		{
			get
			{
				if (parent == null)
				{
					if (transform != null && body != null)
						return body.Position;
					return localPosition;
				}

				Vector2 rotatedLocal = localPosition.RotateAround(Vector2.Zero, parent.rotation);
				Vector2 worldPos = parent.position + rotatedLocal;

				if (transform != null && body != null)
					return body.Position + parent.position;

				return worldPos;
			}
			set
			{
				if (parent == null)
				{
					localPosition = value;
					if (transform != null && body != null)
						body.Position = value;
					return;
				}

				Vector2 offset = value - parent.position;
				Vector2 unrotated = offset.RotateAround(Vector2.Zero, -parent.rotation);
				localPosition = unrotated;

				if (transform != null && body != null)
					body.Position = value;
			}
		}
		Vector2 localPosition { get; set; }


		public float rotation
		{
			get
			{
				if (parent == null)
				{
					if (transform != null && body != null && gameObject.linkPhysicsRotationToTransform)
						return MathHelper.ToDegrees(-body.Rotation);
					return localRotation;
				}

				// Inherit parent’s rotation
				float parentRot = parent.rotation;

				if (transform != null && body != null && gameObject.linkPhysicsRotationToTransform)
					return MathHelper.ToDegrees(-body.Rotation) + parentRot;

				return localRotation + parentRot;
			}
			set
			{
				if (parent == null)
				{
					localRotation = value;
					if (transform != null && body != null && gameObject.linkPhysicsRotationToTransform)
						body.Rotation = MathHelper.ToRadians(-value);
					return;
				}

				float parentRot = parent.rotation;
				localRotation = value - parentRot;

				if (transform != null && body != null && gameObject.linkPhysicsRotationToTransform)
					body.Rotation = MathHelper.ToRadians(-value);
			}
		}
		float localRotation { get; set; }


		public Vector2 scale
		{
			get
			{
				if (parent == null) return localScale;
				return parent.scale * localScale;
			}
			set
			{
				if (parent == null) localScale = value;
				else localScale = value / parent.scale;
			}
		}

		Vector2 localScale { get; set; } = Vector2.One;

		public float Z { get; set; }

		public Vector2 screenPos => Transform.WorldToScreenPos(position);
		public Vector2 up => new Vector2((float)MathF.Cos(MathHelper.ToRadians(rotation - 90)), -(float)MathF.Sin(MathHelper.ToRadians(rotation - 90)));
		public Vector2 right => new Vector2((float)MathF.Cos(MathHelper.ToRadians(rotation)), -(float)MathF.Sin(MathHelper.ToRadians(rotation)));

		public Transform(float worldX, float worldY, float scaleX, float scaleY)
		{
			position = new Vector2(worldX, worldY);
			scale = new Vector2(scaleX, scaleY);
		}
		public Transform(float worldX, float worldY)
		{
			position = new Vector2(worldX, worldY);
		}
		public Transform(Vector2 position, Vector2 scale)
		{
			this.position = position;
			this.scale = scale;
		}
		public Transform(Vector2 position)
		{
			this.position = position;
		}
		public Transform() { }

		public static Vector2 WorldToScreenPos(Vector2 world)
		{
			var unflippedScreen = world * unitSize;
			return new Vector2(unflippedScreen.X, -unflippedScreen.Y);
		}
		public static Vector2 WorldToScreenPos(float x, float y)
		{
			var unflippedScreen = new Vector2(x, y) * unitSize;
			return new Vector2(unflippedScreen.X, -unflippedScreen.Y);
		}
		public static Vector2 ScreenToWorldPos(Vector2 screen)
		{
			var unflippedScreen = screen / unitSize;
			return new Vector2(unflippedScreen.X, -unflippedScreen.Y);
		}
		public static Vector2 ScreenToWorldPos(float x, float y)
		{
			var unflippedScreen = new Vector2(x, y) / unitSize;
			return new Vector2(unflippedScreen.X, -unflippedScreen.Y);
		}
		public static Vector2 ToScreenUnits(Vector2 world) => world.SetY(world.Y * -1) * unitSize;
		public static Vector2 ToWorldUnits(Vector2 screen) => screen.SetY(screen.Y * -1) / unitSize;
		public static float ToScreenUnits(float world) => world * unitSize;
		public static float ToWorldUnits(float screen) => screen / unitSize;

		public void LookAt(Vector2 worldPos)
		{
			var distanceX = worldPos.X - transform.position.X;
			var distanceY = worldPos.Y - transform.position.Y;

			var result = -(float)Math.Atan2(distanceY, distanceX);
			transform.rotation = MathHelper.ToDegrees(result) + 90;
		}

		public void RotateAround(Vector2 origin, float rotation)
		{
			transform.rotation += rotation;
			transform.position = transform.position.RotateAround(origin, rotation);
		}
	}
}