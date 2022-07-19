using Microsoft.Xna.Framework;
using MonoGame.Extended.Tweening;

namespace Veinia.BlockBreaker
{
	public class MovingBlock : Tile
	{
		Tween tween;


		public override void Initialize()
		{
			var yOffset = 1f;

			transform.position = new Vector2(transform.position.X, transform.position.Y - yOffset / 2);

			tween = Globals.tweener.TweenTo(target: transform, expression: transform => transform.position,
											toValue: new Vector2(transform.position.X, transform.position.Y + yOffset), duration: 1.7f)
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
