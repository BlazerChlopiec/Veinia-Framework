using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace VeiniaFramework
{
	public class Transform : Component
	{
		//pixels per unit is assigned in the veinia constructor
		public static int unitSize = 100;

		public static Transform Empty => new Transform(0, 0);

		public float rotation
		{
			get { if (transform != null && body != null) return MathHelper.ToDegrees(-body.Rotation); else return Rotation; }
			set { Rotation = value; if (transform != null && body != null) body.Rotation = MathHelper.ToRadians(-value); }
		}
		public float Rotation { get; set; }

		public Vector2 position
		{
			get { if (transform != null && body != null) return body.Position; else return Position; }
			set { Position = value; if (transform != null && body != null) body.Position = value; }
		}
		public Vector2 Position { get; set; }

		public Vector2 scale { get; set; } = Vector2.One;
		public Vector2 screenPos => Transform.WorldToScreenPos(position);
		public Vector2 up => new Vector2((float)MathF.Cos(MathHelper.ToRadians(rotation - 90)), -(float)MathF.Sin(MathHelper.ToRadians(rotation - 90)));
		public Vector2 right => new Vector2((float)MathF.Cos(MathHelper.ToRadians(rotation - 180)), -(float)MathF.Sin(MathHelper.ToRadians(rotation - 180)));

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

			var result = -((float)Math.Atan2(distanceY, distanceX));
			transform.rotation = MathHelper.ToDegrees(result) + 90;
		}
	}
}