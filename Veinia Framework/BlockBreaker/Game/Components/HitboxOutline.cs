using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System.Collections.Generic;

public class HitboxOutline : Component, IDrawn
{
	public int width = 3;
	Color outlineColor = Color.GreenYellow;
	public static bool showHitbox;

	public ColliderTemplate box;

	public void Draw(SpriteBatch sb)
	{
		if (Globals.showHitboxes)
			sb.DrawRectangle(box.rect, outlineColor, thickness: width, layerDepth: 1);
	}

	public override void Initialize()
	{
		box = GetComponent<ColliderTemplate>();
	}
}