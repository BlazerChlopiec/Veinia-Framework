﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Veinia.BlockBreaker
{
	public class Paddle : Component
	{
		private const float yOffset = -4.5f;
		private const float xLimit = 8;

		Ball ball;


		public override void Initialize()
		{
			ball = FindComponentOfType<Ball>();
		}


		public override void Update()
		{
			var mouseControls = new Vector2(Globals.input.GetMouseWorldPosition().X, yOffset);
			transform.position = new Vector2(MathHelper.Clamp(mouseControls.X, -xLimit, xLimit), mouseControls.Y);

			if (!ball.launched) ball.transform.position = transform.position + Vector2.UnitY;
		}
	}
}