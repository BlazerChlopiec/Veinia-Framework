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

		public VirtualCamera virtualCamera;
	}

	public class VirtualCamera
	{
		// both these values are Funcs because
		// A: renderTarget is dynamic and can change on resolution change so we need the current var at all times
		// B: transformMatrix gets recalculated each time by default

		// RenderTargetUsage.PreserveContents recommended
		public Func<RenderTarget2D> renderTarget;
		public Func<Matrix?> transformMatrix; // if null set to Globals.camera.GetView()
	}
}