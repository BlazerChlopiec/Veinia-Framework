using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VeiniaFramework
{
	public class Sprite : Component, IDrawn
	{
		public Color color = Color.White;
		public Vector2 destinationSize;
		public Effect effect;
		public BlendState blendState;
		public Texture2D texture { get; private set; }

		private float pixelsPerUnit;


		public Sprite(Texture2D texture, Color? color = null, float? pixelsPerUnit = null, Effect effect = null, BlendState blendState = null)
		{
			this.color = color ?? Color.White;
			this.texture = texture;
			this.effect = effect;
			this.blendState = blendState;
			this.pixelsPerUnit = pixelsPerUnit ?? Transform.unitSize;

			destinationSize = new Vector2(texture.Width, texture.Height) / (this.pixelsPerUnit / Transform.unitSize);
		}
		public Sprite(string path, Color? color = null, float? pixelsPerUnit = null, Effect effect = null, BlendState blendState = null)
		{
			this.color = color ?? Color.White;
			this.effect = effect;
			this.blendState = blendState;
			this.pixelsPerUnit = pixelsPerUnit ?? Transform.unitSize;

			texture = Globals.content.Load<Texture2D>(path);

			destinationSize = new Vector2(texture.Width, texture.Height) / (this.pixelsPerUnit / Transform.unitSize);
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
				shader = effect,
				blendState = blendState,
			});
		}

		public void ChangeTexture(string path) => texture = Globals.content.Load<Texture2D>(path);
		public void ChangeTexture(Texture2D texture) => this.texture = texture;


		public Rectangle rect => new Rectangle((int)transform.screenPos.X, (int)transform.screenPos.Y,
											   (int)(destinationSize.X * transform.scale.X),
											   (int)(destinationSize.Y * transform.scale.Y));
	}
}