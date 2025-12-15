using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VeiniaFramework
{
	public class Sprite : Component, IDrawn
	{
		public Color color = Color.White;
		public Vector2 destinationSize;
		public Rectangle? sourceRectangle;
		public Effect effect;
		public DrawOptions drawOptions;
		public Texture2D texture { get; private set; }

		private float pixelsPerUnit;


		public Sprite(Texture2D texture, Color? color = null, float? pixelsPerUnit = null, Rectangle? sourceRectangle = null, DrawOptions options = default)
		{
			this.color = color ?? Color.White;
			this.texture = texture;
			this.pixelsPerUnit = pixelsPerUnit ?? Transform.unitSize;
			this.sourceRectangle = sourceRectangle ?? texture.Bounds;
			this.drawOptions = options;

			destinationSize = new Vector2(this.sourceRectangle.Value.Width, this.sourceRectangle.Value.Height) / (this.pixelsPerUnit / Transform.unitSize);
		}
		public Sprite(string path, Color? color = null, float? pixelsPerUnit = null, Rectangle? sourceRectangle = null, DrawOptions options = default)
			: this(Globals.content.Load<Texture2D>(path), color, pixelsPerUnit, sourceRectangle, options)
		{
		}

		public virtual void Draw(SpriteBatch sb)
		{
			level.drawCommands.Add(new DrawCommand
			{
				command = delegate
				{
					sb.Draw(texture, rect, sourceRectangle, color, MathHelper.ToRadians(transform.rotation),
						 new Vector2(sourceRectangle.Value.Width / 2, sourceRectangle.Value.Height / 2),
						 SpriteEffects.None, layerDepth: 0);
				},
				Z = transform.Z,
				drawOptions = drawOptions
			});
		}

		public void ChangeTexture(string path) => texture = Globals.content.Load<Texture2D>(path);
		public void ChangeTexture(Texture2D texture) => this.texture = texture;


		public Rectangle rect => new Rectangle((int)transform.screenPos.X, (int)transform.screenPos.Y,
											   (int)(destinationSize.X * transform.scale.X),
											   (int)(destinationSize.Y * transform.scale.Y));
	}
}