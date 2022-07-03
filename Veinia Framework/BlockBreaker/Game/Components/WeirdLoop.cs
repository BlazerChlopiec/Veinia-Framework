using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

public class WeirdLoop : Component, IDrawn
{
	Texture2D texture;
	Color newColor;
	byte[] colorData = new byte[3];
	bool reverse;

	Sprite sprite;

	public void Draw(SpriteBatch sb)
	{
		sb.Draw(sprite.texture, new Vector2(100, 300), newColor);
	}

	public override void Initialize()
	{
		sprite = GetComponent<Sprite>();
		texture = sprite.texture;

		for (int i = 0; i < 2; i++)
		{
			colorData[i] = 0;
		}
		newColor = new Color(colorData[0], colorData[1], byte.MaxValue, byte.MaxValue);
	}

	public override void Update()
	{
		for (int i = 0; i < 2; i++)
		{
			if (!reverse)
				colorData[i]++;
			else
				colorData[i]--;
		}
		if (colorData[0] == byte.MaxValue)
		{
			reverse = !reverse;
		}
		if (colorData[0] == 0 && reverse)
		{
			reverse = !reverse;
		}
		newColor = new Color(colorData[0], colorData[1], byte.MaxValue, byte.MaxValue);

		if (Globals.input.GetKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
		{
			for (int i = 0; i < 2; i++)
			{
				colorData[i] = 0;
			}
			reverse = false;
		}
	}
}