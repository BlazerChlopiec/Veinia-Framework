using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace VeiniaFramework
{
	public class DrawCommand
	{
		public Action command;
		public float Z; // drawing order that works with multiple Begins()
		public bool drawWithoutSpriteBatch; // used for drawing with DrawUserPrimitives(), this Ends a spritebatch if it has begun

		public DrawOptions drawOptions;
	}

	public struct DrawOptions
	{
		public BlendState blendState;
		public DepthStencilState depthStencilState;
		public RasterizerState rasterizerState;
		public SamplerState samplerState;

		public Effect shader;

		// i wish it were c++ for this
		public Func<RenderTarget2D> renderTarget; // RenderTargetUsage.PreserveContents recommended
		public Matrix? transformMatrix; // if null set to Globals.camera.GetView()
	}
}