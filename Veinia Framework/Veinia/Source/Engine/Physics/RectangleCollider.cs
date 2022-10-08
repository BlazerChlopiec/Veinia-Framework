using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Veinia
{
	public class RectangleCollider : Collider, IDrawn
	{
		private Vector2 screenSize;
		private Vector2 screenOffset;

		public RectangleCollider(Vector2 offset, Vector2 size, bool trigger = false) : base(trigger)
		{
			screenSize = Transform.ToScreenUnits(size.SetY(size.Y * -1));
			screenOffset = Transform.ToScreenUnits(offset);
		}

		public override void Initialize()
		{
			offset = screenOffset - screenSize / 2;
			rectangleBounds = new RectangleF(Vector2.Zero, screenSize);
			Bounds = rectangleBounds;

			base.Initialize();
		}

		public void Draw(SpriteBatch sb)
		{
			if (showHitboxes)
				sb.DrawRectangle((RectangleF)Bounds, Color.Red, 5, layerDepth: 1);
		}
	}
}