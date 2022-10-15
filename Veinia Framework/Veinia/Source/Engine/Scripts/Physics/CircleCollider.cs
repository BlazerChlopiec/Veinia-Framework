using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Veinia
{
	public class CircleCollider : Collider, IDrawn
	{
		private float screenRadius;
		private Vector2 screenOffset;

		public CircleCollider(Vector2 offset, float radius, bool trigger = false) : base(trigger)
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
				sb.DrawCircle((CircleF)Bounds, 50, Color.Red, 5, layerDepth: 1);
		}
	}
}