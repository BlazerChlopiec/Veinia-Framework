using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VeiniaFramework
{
	public class Sprite : Component, IDrawn
	{
		public Color color = Color.White;
		public float layer;
		public Vector2 destinationSize;
		public Texture2D texture { get; private set; }


		public Sprite(Texture2D texture, float layer, Color color, float pixelsPerUnit)
		{
			this.layer = layer;
			this.color = color;

			this.texture = texture;

			this.destinationSize = new Vector2(texture.Width, texture.Height) / (pixelsPerUnit / Transform.unitSize);
		}
		public Sprite(string path, float layer, Color color, float pixelsPerUnit)
		{
			this.layer = layer;
			this.color = color;

			texture = Globals.content.Load<Texture2D>(path);

			this.destinationSize = new Vector2(texture.Width, texture.Height) / (pixelsPerUnit / Transform.unitSize);
		}

		public virtual void Draw(SpriteBatch sb)
		{
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
}