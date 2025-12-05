using Microsoft.Xna.Framework.Graphics;
using System;

namespace VeiniaFramework
{
	public struct DrawCommand
	{
		public Action command;
		public float Z; // drawing order that works with multiple Begins()
		public Effect shader;
		public bool drawWithoutSpriteBatch; // used for drawing with DrawUserPrimitives(), this Ends a spritebatch if it has begun

		public BlendState blendState;
		public DepthStencilState depthStencilState;
		public RasterizerState rasterizerState;
	}
}