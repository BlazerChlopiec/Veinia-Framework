using MonoGame.Extended.Collisions;

namespace Veinia.Platformer
{
	public class Collectible : Component
	{
		public override void OnTrigger(Collider self, CollisionState state, CollisionEventArgs collisionInfo)
		{
			if (state == CollisionState.Enter)
				if (collisionInfo.Other.collider.GetComponent<Player>() != null) Collect();
		}

		private void Collect() => DestroyGameObject();
	}
}