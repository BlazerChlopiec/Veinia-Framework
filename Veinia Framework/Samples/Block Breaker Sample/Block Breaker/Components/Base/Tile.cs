﻿using Microsoft.Xna.Framework;
using MonoGame.Extended.Tweening;

namespace Veinia.BlockBreaker
{
	public class Tile : Component
	{
		protected bool hasBeenDestroyed;


		public virtual void Hit()
		{
			if (hasBeenDestroyed) return;

			var collider = GetComponent<Collider>();
			if (collider != null)
				RemoveComponent(collider);

			GetComponent<Sprite>().layer = 1;

			hasBeenDestroyed = true;

			Globals.tweener.TweenTo(target: transform, expression: transform => transform.xRotation, toValue: -10, duration: .2f)
				.Easing(EasingFunctions.BackIn);

			Globals.tweener.TweenTo(target: transform, expression: transform => transform.scale, toValue: Vector2.Zero, duration: .3f)
				.Easing(EasingFunctions.BackIn)
				.OnEnd((x) =>
				{
					DestroyGameObject();

					if (FindComponentsOfType<Tile>().Count == 0)
					{
						Globals.loader.Reload();
					}
				});
		}
	}
}