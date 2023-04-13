using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VeiniaFramework
{
	public class Sprite : Component, IDrawn, IDrawnAfterUI
	{
		public Color color = Color.White;
		public float layer;
		public Vector2 destinationSize;
		public Texture2D texture { get; private set; }
		public RenderOrder order;


		public Sprite(Texture2D texture, float layer, Color color, float pixelsPerUnit, RenderOrder order = RenderOrder.Standard)
		{
			this.layer = layer;
			this.color = color;
			this.order = order;

			this.texture = texture;

			this.destinationSize = new Vector2(texture.Width, texture.Height) / (pixelsPerUnit / Transform.unitSize);
		}
		public Sprite(string path, float layer, Color color, float pixelsPerUnit, RenderOrder order = RenderOrder.Standard)
		{
			this.layer = layer;
			this.color = color;
			this.order = order;

			texture = Globals.content.Load<Texture2D>(path);

			this.destinationSize = new Vector2(texture.Width, texture.Height) / (pixelsPerUnit / Transform.unitSize);
		}

		public void Draw(SpriteBatch sb)
		{
			if (order != RenderOrder.Standard) return;

			sb.Draw(texture, rect, null, color, MathHelper.ToRadians(transform.rotation),
									 new Vector2(texture.Bounds.Width / 2, texture.Bounds.Height / 2),
									 SpriteEffects.None, layer);
		}

		public void DrawAfterUI(SpriteBatch sb)
		{
			if (order != RenderOrder.AfterUI) return;

			sb.Draw(texture, rect, null, color, MathHelper.ToRadians(transform.rotation),
									 new Vector2(texture.Bounds.Width / 2, texture.Bounds.Height / 2),
									 SpriteEffects.None, layer);
		}

		public void ChangeTexture(string path) => texture = Globals.content.Load<Texture2D>(path);
		public void ChangeTexture(Texture2D texture) => this.texture = texture;


		public Rectangle rect => new Rectangle((int)transform.screenPos.X, (int)transform.screenPos.Y,
											   (int)(destinationSize.X * transform.scale.X),
											   (int)(destinationSize.Y * transform.scale.Y));
	}

	public enum RenderOrder
	{
		Standard,
		AfterUI,
	}
}