using FontStashSharp;
using FontStashSharp.Interfaces;
using Myra.Graphics2D;
using Myra.Platform;
using System.Drawing;
using System.Numerics;

public class KNIRenderer : IMyraRenderer
{
	public ITexture2DManager TextureManager => throw new System.NotImplementedException();

	public RendererType RendererType => throw new System.NotImplementedException();

	public Rectangle Scissor { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

	public void Begin(TextureFiltering textureFiltering)
	{
		throw new System.NotImplementedException();
	}

	public void DrawQuad(object texture, ref VertexPositionColorTexture topLeft, ref VertexPositionColorTexture topRight, ref VertexPositionColorTexture bottomLeft, ref VertexPositionColorTexture bottomRight)
	{
		throw new System.NotImplementedException();
	}

	public void DrawSprite(object texture, Vector2 pos, Rectangle? src, FSColor color, float rotation, Vector2 scale, float depth)
	{
		throw new System.NotImplementedException();
	}

	public void End()
	{
		throw new System.NotImplementedException();
	}
}