using FontStashSharp.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;

public class KNITextureManager : ITexture2DManager
{
	private GraphicsDevice graphicsDevice;

	public KNITextureManager(GraphicsDevice graphicsDevice)
	{
		this.graphicsDevice = graphicsDevice;
	}

	public object CreateTexture(int width, int height)
	{
		return new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);
	}

	public Point GetTextureSize(object texture)
	{
		var tex = (Texture2D)texture;
		return new Point(tex.Width, tex.Height);
	}

	public void SetTextureData(object texture, Rectangle bounds, byte[] data)
	{
		var tex = (Texture2D)texture;

		var newBounds = new Microsoft.Xna.Framework.Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);

		tex.SetData(0, newBounds, data, 0, bounds.Width * bounds.Height * 4);
	}
}
