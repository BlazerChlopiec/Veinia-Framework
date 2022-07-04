using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

public class RectanglePhysics : NewPhysics, IDrawn
{
	public RectanglePhysics(Vector2 offset, Vector2 size, bool trigger = false) : base(trigger)
	{
		size = Transform.ToScreenUnits(size.SetY(size.Y * -1));
		this.offset = Transform.ToScreenUnits(offset) - size / 2;
		Bounds = new RectangleF(Vector2.Zero, size);
	}

	public void Draw(SpriteBatch sb)
	{
		if (Globals.showHitboxes)
			sb.DrawRectangle((RectangleF)Bounds, Color.Red, 5, 1);
	}
}