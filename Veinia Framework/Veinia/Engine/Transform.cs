using Microsoft.Xna.Framework;
using MonoGame.Extended;

public class Transform : Component
{
	public readonly static int PIXELS_PER_UNIT = 100;

	public static Transform Empty { get; } = new Transform(0, 0);

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
		var unflippedScreen = world * PIXELS_PER_UNIT + new Vector2(Globals.camera.Origin.X, -Globals.camera.Origin.Y);
		return new Vector2(unflippedScreen.X, -unflippedScreen.Y);
	}
	public static Vector2 ScreenToWorldPos(Vector2 screen) => (new Vector2(screen.X, -screen.Y) - new Vector2(Globals.camera.Origin.X, -Globals.camera.Origin.Y)) / PIXELS_PER_UNIT;
	public static Vector2 ToScreenUnits(Vector2 world) => world.SetY(world.Y * -1) * PIXELS_PER_UNIT;
	public static Vector2 ToWorldUnits(Vector2 screen) => screen.SetY(screen.Y * -1) / PIXELS_PER_UNIT;
	public static float ToScreenUnits(float world) => world * PIXELS_PER_UNIT;
	public static float ToWorldUnits(float screen) => screen / PIXELS_PER_UNIT;
}