using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

public class CirclePhysics : Physics, IDrawn
{
	public CirclePhysics(Vector2 offset, float radius, bool trigger = false) : base(trigger)
	{
		this.offset = Transform.ToScreenUnits(offset);
		Bounds = new CircleF(Vector2.Zero, Transform.ToScreenUnits(radius));
	}

	public void Draw(SpriteBatch sb)
	{
		if (Globals.showHitboxes)
			sb.DrawCircle((CircleF)Bounds, 50, Color.Red, 5, 1);
	}
}