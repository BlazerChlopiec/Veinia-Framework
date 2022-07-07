using Microsoft.Xna.Framework;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Tweening;

public class Paddle : Component
{
	private const float yOffset = -4.5f;
	private const float xLimit = 8;


	public override void Update()
	{
		var mouseControlls = new Vector2(Globals.input.GetMouseWorldPosition().X, yOffset);
		transform.position = new Vector2(MathHelper.Clamp(mouseControlls.X, -xLimit, xLimit), mouseControlls.Y);
	}
}