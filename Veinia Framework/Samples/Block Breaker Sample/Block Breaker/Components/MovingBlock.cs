using Microsoft.Xna.Framework;
using MonoGame.Extended.Tweening;
using System;

namespace Veinia.BlockBreaker
{
	public class MovingBlock : Tile
	{
		Tween tween;


		public override void Initialize()
		{
			var yOffset = 1f;

			Random random = new Random();

			transform.position = new Vector2(transform.position.X, transform.position.Y - yOffset / 2);

			//random between 1f & 2f
			var duration = (float)random.Next(100, 200) / 100;
			tween = Globals.tweener.TweenTo(target: transform, expression: transform => transform.position,
											toValue: new Vector2(transform.position.X, transform.position.Y + yOffset), duration)
			.Easing(EasingFunctions.BackInOut)
			.AutoReverse()
			.RepeatForever();
		}

		public override void Update()
		{
			base.Update();

			if (hasBeenDestroyed) tween.Cancel();
		}
	}
}
