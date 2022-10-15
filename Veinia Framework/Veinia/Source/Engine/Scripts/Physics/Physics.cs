using Microsoft.Xna.Framework;
using MonoGame.Extended.Collisions;

namespace Veinia
{
	public class Physics : Component
	{
		public Vector2 velocity;
		private Vector2 penetrationVector;
		private Vector2 penetrationVectorPerFrame;

		float gravity;
		bool removeVelocityBasedOnCollision;


		public Physics(float gravity = -9.81f, bool removeVelocityBasedOnCollision = true)
		{
			this.gravity = gravity;
			this.removeVelocityBasedOnCollision = removeVelocityBasedOnCollision;
		}

		public override void Update()
		{
			if (removeVelocityBasedOnCollision)
			{
				if (penetrationVectorPerFrame.Y > 0 && velocity.Y < 0) velocity.Y = .1f;
				if (penetrationVectorPerFrame.Y < 0 && velocity.Y > 0) velocity.Y = -.1f;
				if (penetrationVector.X > 0 && velocity.X > 0) velocity.X = .1f;
				if (penetrationVector.X < 0 && velocity.X < 0) velocity.X = -.1f;
			}

			velocity.Y += gravity * Time.deltaTime;
			transform.position += velocity * Time.deltaTime;
		}

		public override void OnCollide(Collider self, CollisionState state, CollisionEventArgs collisionInfo)
		{
			penetrationVector = collisionInfo.PenetrationVector;
			penetrationVectorPerFrame = collisionInfo.PenetrationVectorPerFrame;

			if (state == CollisionState.Exit)
			{
				penetrationVector = Vector2.Zero;
				penetrationVectorPerFrame = Vector2.Zero;
			}
		}
	}
}
