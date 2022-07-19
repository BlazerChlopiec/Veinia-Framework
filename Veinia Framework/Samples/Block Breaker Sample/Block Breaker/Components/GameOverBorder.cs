using MonoGame.Extended.Collisions;

namespace Veinia.BlockBreaker
{
	public class GameOverBorder : Component
	{
		public override void OnCollide(CollisionState state, CollisionEventArgs collisionInfo)
		{
			if (state == CollisionState.Enter) Globals.loader.Reload();
		}
	}
}