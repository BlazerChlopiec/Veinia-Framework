﻿
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tweening;

namespace VeiniaFramework.BlockBreaker
{
	public class Transition : Component
	{
		Tween tween;

		public override void Initialize()
		{
			transform.scale = Vector2.Zero;
			tween = Globals.unscaledTweener.TweenTo(target: transform, expression: transform => transform.scale,
											toValue: new Vector2(12, 12), .5f)
			.Easing(EasingFunctions.CircleOut)
			.OnEnd((x) =>
			{
				Globals.loader.Reload();
				tween = Globals.unscaledTweener.TweenTo(target: transform, expression: transform => transform.scale,
								toValue: Vector2.Zero, .5f, delay: .1f)
				.Easing(EasingFunctions.SineIn);
			});
		}
	}
}