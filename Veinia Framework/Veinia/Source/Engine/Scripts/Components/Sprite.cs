using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VeiniaFramework
{
	public class Sprite : Component, IDrawn
	{
		public Color color = Color.White;
		public Vector2 destinationSize;
		public Effect effect;
		public Texture2D texture { get; private set; }


		public Sprite(Texture2D texture, Color color, float pixelsPerUnit, Effect effect = null)
		{
			this.color = color;

			this.texture = texture;

			this.effect = effect;

			this.destinationSize = new Vector2(texture.Width, texture.Height) / (pixelsPerUnit / Transform.unitSize);
		}
		public Sprite(string path, Color color, float pixelsPerUnit, Effect effect = null)
		{
			this.color = color;

			texture = Globals.content.Load<Texture2D>(path);

			this.effect = effect;

			this.destinationSize = new Vector2(texture.Width, texture.Height) / (pixelsPerUnit / Transform.unitSize);
		}

		public virtual void Draw(SpriteBatch sb)
		{
			level.drawCommands.Add(new DrawCommand
			{
				command = delegate
				{
					sb.Draw(texture, rect, null, color, MathHelper.ToRadians(transform.rotation),
						 new Vector2(texture.Bounds.Width / 2, texture.Bounds.Height / 2),
						 SpriteEffects.None, layerDepth: 0);
				},
				Z = transform.Z,
				shader = effect
			});
		}

		public void ChangeTexture(string path) => texture = Globals.content.Load<Texture2D>(path);
		public void ChangeTexture(Texture2D texture) => this.texture = texture;


		public Rectangle rect => new Rectangle((int)transform.screenPos.X, (int)transform.screenPos.Y,
											   (int)(destinationSize.X * transform.scale.X),
											   (int)(destinationSize.Y * transform.scale.Y));
	}
}