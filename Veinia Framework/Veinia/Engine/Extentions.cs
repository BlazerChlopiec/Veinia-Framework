using Microsoft.Xna.Framework;
using MonoGame.Extended;

public static class Extentions
{
	public static void LerpTo(this OrthographicCamera camera, Vector2 worldPos, float lerpT)
	{
		Globals.camera.Position = Vector2.Lerp(Globals.camera.Position,
												 Transform.ToScreenUnits(worldPos),
												 lerpT * Time.deltaTime);
	}
}