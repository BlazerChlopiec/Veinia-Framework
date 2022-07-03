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

		viewport = Globals.graphicsDevice.Viewport;

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
		Globals.graphicsManager.PreferredBackBufferWidth = width;
		Globals.graphicsManager.PreferredBackBufferHeight = height;
		Globals.graphicsManager.ApplyChanges();
	}
}