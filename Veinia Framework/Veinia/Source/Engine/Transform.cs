using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Veinia
{
	public class Transform : Component
	{
		public static int pixelsPerUnit = 100;

		public static Transform Empty { get; } = new Transform(0, 0);

		public float xRotation { get; set; }
		public Vector2 scale { get; set; } = Vector2.One;
		public Vector2 position { get; set; }
		public Vector2 screenPos { get { return Transform.WorldToScreenPos(position); } }

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

		public static Vector2 WorldToScreenPos(Vector2 world)
		{
			var unflippedScreen = world * pixelsPerUnit + new Vector2(Globals.camera.Origin.X, -Globals.camera.Origin.Y);
			return new Vector2(unflippedScreen.X, -unflippedScreen.Y);
		}
		public static Vector2 ScreenToWorldPos(Vector2 screen) => (new Vector2(screen.X, -screen.Y) - new Vector2(Globals.camera.Origin.X, -Globals.camera.Origin.Y)) / pixelsPerUnit;
		public static Vector2 ToScreenUnits(Vector2 world) => world.SetY(world.Y * -1) * pixelsPerUnit;
		public static Vector2 ToWorldUnits(Vector2 screen) => screen.SetY(screen.Y * -1) / pixelsPerUnit;
		public static float ToScreenUnits(float world) => world * pixelsPerUnit;
		public static float ToWorldUnits(float screen) => screen / pixelsPerUnit;
	}
}