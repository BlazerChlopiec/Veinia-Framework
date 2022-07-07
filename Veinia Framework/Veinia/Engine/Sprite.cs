using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Sprite : Component, IDrawn
{
	public Color color = Color.White;
	public int layer;
	Vector2 destinationSize;
	public Texture2D texture;


	public Sprite(string path, int layer, Color color, Vector2 destinationSize)
	{
		this.layer = layer;
		this.color = color;

		texture = Globals.content.Load<Texture2D>(path);

		this.destinationSize = destinationSize * Transform.PIXELS_PER_UNIT;
	}
	public Sprite(string path, int layer, Color color)
	{
		this.layer = layer;
		this.color = color;

		texture = Globals.content.Load<Texture2D>(path);

		destinationSize = new Vector2(texture.Width, texture.Height);
	}

	public void Draw(SpriteBatch sb)
	{
		sb.Draw(texture, rect, null, color, 0f,
								 new Vector2(texture.Bounds.Width / 2, texture.Bounds.Height / 2),
								 SpriteEffects.None, layer);
	}

	public Rectangle rect => new Rectangle((int)transform.screenPos.X, (int)transform.screenPos.Y,
										   (int)(destinationSize.X * transform.scale.X),
										   (int)(destinationSize.Y * transform.scale.Y));

}