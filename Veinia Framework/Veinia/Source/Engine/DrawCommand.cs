using Microsoft.Xna.Framework.Graphics;
using System;

namespace VeiniaFramework
{
	public struct DrawCommand
	{
		public Action command;
		public float Z; // drawing order that works with multiple Begins()
		public bool drawWithoutSpriteBatch; // used for drawing with DrawUserPrimitives(), this Ends a spritebatch if it has begun
		public Effect shader;

		public DrawOptions options;
	}


	public struct DrawOptions
	{
		public BlendState blendState;
		public DepthStencilState depthStencilState;
		public RasterizerState rasterizerState;
		public SamplerState samplerState;
	}
}