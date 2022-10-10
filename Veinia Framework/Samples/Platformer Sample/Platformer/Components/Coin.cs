using MonoGame.Extended.Collisions;

namespace Veinia.Platformer
{
	public class Coin : Component
	{
		public override void OnTrigger(Collider self, CollisionState state, CollisionEventArgs collisionInfo)
		{
			if (state == CollisionState.Enter)
				if (collisionInfo.Other.collider.GetComponent<Player>() != null) Collect();
		}

		private void Collect()
		{
			FindComponentOfType<UI>().coinCount++;

			DestroyGameObject();
		}
	}
}