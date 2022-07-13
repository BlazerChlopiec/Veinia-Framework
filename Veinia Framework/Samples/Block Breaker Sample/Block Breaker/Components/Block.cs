using Microsoft.Xna.Framework;
using MonoGame.Extended.Tweening;

namespace Veinia.BlockBreaker
{
	public class Block : Component
	{
		protected bool hasBeenHit;


		public virtual void Hit()
		{
			if (hasBeenHit) return;

			RemoveComponent(GetComponent<Collider>());

			GetComponent<Sprite>().layer = 1;

			hasBeenHit = true;

			Globals.tweener.TweenTo(target: transform, expression: transform => transform.xRotation, toValue: -10, duration: .2f)
				.Easing(EasingFunctions.BackIn);


			Globals.tweener.TweenTo(target: transform, expression: transform => transform.scale, toValue: Vector2.Zero, duration: .3f)
				.Easing(EasingFunctions.BackIn)
				.OnEnd((x) =>
				{
					DestroyGameObject();

					if (FindComponentsOfType<Block>().Count == 0)
					{
						Globals.loader.Reload();
					}
				});
		}
	}
}