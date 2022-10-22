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

		bool isTouchingGround;

		Physics physics;


		public override void Initialize() => physics = GetComponent<Physics>();

		float smoothZoom;
		public override void Update()
		{
			Title.Add(isTouchingGround, 9);

			physics.velocity.X = Globals.input.horizontal * speed;

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
			if (state != CollisionState.Exit)
				isTouchingGround = collisionInfo.PenetrationVectorPerFrame.Y > 0 ? true : false;
		}
	}
}