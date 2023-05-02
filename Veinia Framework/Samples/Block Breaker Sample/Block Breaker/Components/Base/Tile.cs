using Microsoft.Xna.Framework;
using MonoGame.Extended.Tweening;
using tainicom.Aether.Physics2D.Dynamics;

namespace VeiniaFramework.Samples.BlockBreaker
{
	public class Tile : Component
	{
		protected bool hasBeenDestroyed;


		public override void Initialize()
		{
			body = Globals.physicsWorld.CreateRectangle(1, 1, 1, bodyType: BodyType.Kinematic);
			body.Tag = this;
		}

		public virtual void Hit()
		{
			NextFrame.actions.Add(RemovePhysics);
			void RemovePhysics() => Globals.physicsWorld.Remove(body);

			if (hasBeenDestroyed) return;
			hasBeenDestroyed = true;

			var UI = FindComponentOfType<UI>();
			UI.progressBar.Value--;

			GetComponent<Sprite>().layer = 1;

			Globals.tweener.TweenTo(target: transform, expression: transform => transform.rotation, toValue: -10, duration: .2f)
				.Easing(EasingFunctions.BackIn);

			Globals.tweener.TweenTo(target: transform, expression: transform => transform.scale, toValue: Vector2.Zero, duration: .3f)
				.Easing(EasingFunctions.BackIn)
				.OnEnd((x) =>
				{
					DestroyGameObject();

					if (FindComponentsOfType<Tile>().Count == 0)
					{
						UI.ShowWinScreen();
					}
				});
		}
	}
}
