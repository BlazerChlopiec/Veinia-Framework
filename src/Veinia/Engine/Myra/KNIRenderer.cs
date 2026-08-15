using FontStashSharp;
using FontStashSharp.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D;
using Myra.Platform;

public class KNIRenderer : IMyraRenderer
{
	SpriteBatch spriteBatch;
	GraphicsDevice graphics;
	KNITextureManager textureManager;

	RasterizerState UIRasterizerState;

	public KNIRenderer(ITexture2DManager textureManager, SpriteBatch spriteBatch, GraphicsDevice graphics)
	{
		this.spriteBatch = spriteBatch;
		this.graphics = graphics;
		this.textureManager = (KNITextureManager)textureManager;

		UIRasterizerState = new RasterizerState
		{
			ScissorTestEnable = true
		};
	}

	public ITexture2DManager TextureManager => textureManager;

	public RendererType RendererType => RendererType.Sprite;

	public System.Drawing.Rectangle Scissor
	{
		get => new System.Drawing.Rectangle(graphics.ScissorRectangle.X, graphics.ScissorRectangle.Y, graphics.ScissorRectangle.Width, graphics.ScissorRectangle.Height);
		set => graphics.ScissorRectangle = new Microsoft.Xna.Framework.Rectangle(value.X, value.Y, value.Width, value.Height);
	}

	public void Begin(TextureFiltering textureFiltering)
	{
		SamplerState samplerState = textureFiltering == TextureFiltering.Nearest ? SamplerState.PointClamp : SamplerState.LinearClamp;
		spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: samplerState, rasterizerState: UIRasterizerState);
	}

	public void DrawQuad(object texture, ref FontStashSharp.Interfaces.VertexPositionColorTexture topLeft,
		ref FontStashSharp.Interfaces.VertexPositionColorTexture topRight,
		ref FontStashSharp.Interfaces.VertexPositionColorTexture bottomLeft,
		ref FontStashSharp.Interfaces.VertexPositionColorTexture bottomRight)
	{
		// not using quad rendering
	}

	public void DrawSprite(object texture, System.Numerics.Vector2 pos, System.Drawing.Rectangle? src, FSColor color, float rotation, System.Numerics.Vector2 scale, float depth)
	{
		var xnaTexture = (Texture2D)texture;



		Rectangle? sourceRectangle = null;
		if (src.HasValue)
		{
			sourceRectangle = new Rectangle(src.Value.X, src.Value.Y, src.Value.Width, src.Value.Height);
		}

		Color col = new Color(color.R, color.G, color.B, color.A);

		var position = new Vector2(pos.X, pos.Y);
		spriteBatch.Draw(xnaTexture, position, sourceRectangle, col, rotation, Vector2.Zero, new Vector2(scale.X, scale.Y), SpriteEffects.None, depth);
	}

	public void End() => spriteBatch.End();
}