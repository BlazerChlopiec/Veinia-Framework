using Microsoft.Xna.Framework;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Tweening;
using System;

namespace Veinia.BlockBreaker
{
	public class Ball : Component
	{
		Paddle paddle;
		CirclePhysics physics;


		private const float speed = 10;
		bool launched;


		public override void Initialize()
		{
			paddle = FindComponentOfType<Paddle>();
			physics = GetComponent<CirclePhysics>();

			physics.onCollisionEnter += CollisionEnter;
		}

		public override void Update()
		{
			SetToPaddlePos();

			if (launched) transform.xRotation += 200f * Time.deltaTime;

			if (Globals.input.GetMouseButtonDown(0) && !launched)
			{
				launched = true;
				physics.velocity = Utils.SafeNormalize(Vector2.One) * speed;
			}

			//else if (Globals.input.GetMouseButtonDown(0) && launched)
			//{
			//	physics.velocity = Utils.SafeNormalize(Globals.input.GetMouseWorldPosition() - transform.position) * speed;
			//}
		}

		private void SetToPaddlePos()
		{
			if (!launched) transform.position = paddle.transform.position + Vector2.UnitY;
		}

		private void CollisionEnter(CollisionEventArgs collisionInfo)
		{
			var block = collisionInfo.Other.physics.NullableGetComponent<Block>();
			if (block != null) block.Hit();

			Globals.tweener.TweenTo(target: transform, expression: transform => transform.scale, toValue: new Vector2(1.3f, 1f), duration: .01f)
				.Easing(EasingFunctions.BackInOut)
				.OnEnd((x) =>
				{
					Globals.tweener.TweenTo(target: transform, expression: transform => transform.scale, toValue: Vector2.One, duration: .4f)
					.Easing(EasingFunctions.BackInOut);
				});

			if (Math.Abs(collisionInfo.PenetrationVector.Y) > Math.Abs(collisionInfo.PenetrationVector.X))
			{
				//touched Y
				physics.velocity *= new Vector2(1, -1);
			}

			if (Math.Abs(collisionInfo.PenetrationVector.X) > Math.Abs(collisionInfo.PenetrationVector.Y))
			{
				//touched X
				physics.velocity *= new Vector2(-1, 1);
			}
		}
	}
}