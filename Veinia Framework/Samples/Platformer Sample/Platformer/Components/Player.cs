using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Collisions;
using System;

namespace Veinia.Platformer
{
	public class Player : Component
	{
		private const float speed = 8.5f;
		private const float jumpForce = 13;
		private const float accelerationSpeed = 20;

		bool isTouchingGround;

		Physics physics;


		public override void Initialize() => physics = GetComponent<Physics>();

		float smoothZoom;
		float smoothHorizontal;
		public override void Update()
		{
			smoothHorizontal = MathHelper.Lerp(smoothHorizontal, Globals.input.horizontal, accelerationSpeed * Time.deltaTime);
			physics.velocity.X = smoothHorizontal * speed;

			if ((Globals.input.GetKeyButtonDown(Keys.Space, Buttons.A) || Globals.input.GetKeyDown(Keys.W)) && isTouchingGround)
			{
				isTouchingGround = false;
				physics.velocity.Y = jumpForce;
			}
			if ((Globals.input.GetKeyButtonUp(Keys.Space, Buttons.A) || Globals.input.GetKeyUp(Keys.W)) && physics.velocity.Y > 0)
			{
				physics.velocity.Y *= .7f;
			}

			Globals.camera.LerpTo(transform.position, 10);

			smoothZoom = MathHelper.Lerp(smoothZoom, (MathF.Abs(Globals.input.horizontal) / 10), 1.5f * Time.deltaTime);
			Globals.camera.Zoom = 1 - smoothZoom;
		}

		public override void OnCollide(Collider self, CollisionState state, CollisionEventArgs collisionInfo)
		{
			if (state == CollisionState.Exit) isTouchingGround = false;
			else isTouchingGround = collisionInfo.PenetrationVectorPerFrame.Y > 0 ? true : false;
		}
	}
}