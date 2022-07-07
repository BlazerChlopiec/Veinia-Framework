using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

public class CirclePhysics : Physics, IDrawn
{
	private float screenRadius;
	private Vector2 screenOffset;

	public CirclePhysics(Vector2 offset, float radius, bool trigger = false) : base(trigger)
	{
		screenOffset = Transform.ToScreenUnits(offset);
		screenRadius = Transform.ToScreenUnits(radius);
	}

	public override void Initialize()
	{
		offset = screenOffset;
		Bounds = new CircleF(Vector2.Zero, screenRadius);
		base.Initialize();
	}

	public void Draw(SpriteBatch sb)
	{
		if (showHitboxes)
			sb.DrawCircle((CircleF)Bounds, 50, Color.Red, 5, 1);
	}
}