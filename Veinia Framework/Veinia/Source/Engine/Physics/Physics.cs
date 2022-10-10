using Microsoft.Xna.Framework;
using MonoGame.Extended.Collisions;

namespace Veinia
{
	public class Physics : Component
	{
		public Vector2 velocity;
		private Vector2 penetrationVector;

		float gravity;
		bool removeVelocityBasedOnCollision;

		Physics physics;


		public Physics(float gravity = -9.81f, bool removeVelocityBasedOnCollision = true)
		{
			this.gravity = gravity;
			this.removeVelocityBasedOnCollision = removeVelocityBasedOnCollision;
		}

		public override void Initialize() => physics = GetComponent<Physics>();

		public override void Update()
		{
			if (removeVelocityBasedOnCollision)
			{
				if (penetrationVector.Y > 0 && velocity.Y < 0) velocity.Y = .1f;
				if (penetrationVector.Y < 0 && velocity.Y > 0) velocity.Y = -.1f;
				if (penetrationVector.X > 0 && velocity.X > 0) velocity.X = .1f;
				if (penetrationVector.X < 0 && velocity.X < 0) velocity.X = -.1f;
			}

			velocity.Y += gravity * Time.deltaTime;
			transform.position += velocity * Time.deltaTime;
		}

		public override void OnCollide(Collider self, CollisionState state, CollisionEventArgs collisionInfo)
		{
			base.OnCollide(self, state, collisionInfo);

			penetrationVector = collisionInfo.PenetrationVector;
			if (state == CollisionState.Exit) penetrationVector = Vector2.Zero;
		}
	}
}
