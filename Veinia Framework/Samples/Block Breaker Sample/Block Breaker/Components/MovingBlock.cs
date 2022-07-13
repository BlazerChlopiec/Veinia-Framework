using Microsoft.Xna.Framework;
using MonoGame.Extended.Tweening;

namespace Veinia.BlockBreaker
{
	public class MovingBlock : Block
	{
		Tween tween;


		public override void Initialize()
		{
			var yOffset = transform.position.Y + 1;

			tween = Globals.tweener.TweenTo(target: transform, expression: transform => transform.position,
											toValue: new Vector2(transform.position.X, yOffset), duration: 1.7f)
			.Easing(EasingFunctions.BackInOut)
			.AutoReverse()
			.RepeatForever();
		}

		public override void Update()
		{
			base.Update();

			if (hasBeenHit) tween.Cancel();
		}
	}
}
