using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class Screen
{
	public int width;
	public int height;

	Viewport viewport;

	public Screen(int width, int height)
	{
		this.width = width;
		this.height = height;

		viewport = Globals.device.Viewport;

		UpdateChanges();
	}

	public void SetSize(int X, int Y)
	{
		width = X;
		height = Y;

		UpdateChanges();
	}

	private void UpdateChanges()
	{
		Globals.graphics.PreferredBackBufferWidth = width;
		Globals.graphics.PreferredBackBufferHeight = height;
		Globals.graphics.ApplyChanges();
	}
}