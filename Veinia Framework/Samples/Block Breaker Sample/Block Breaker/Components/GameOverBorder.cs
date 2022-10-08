using MonoGame.Extended.Collisions;

namespace Veinia.BlockBreaker
{
	public class GameOverBorder : Component
	{
		public override void OnCollide(Collider self, CollisionState state, CollisionEventArgs collisionInfo)
		{
			if (state == CollisionState.Enter) FindComponentOfType<UI>().ShowLoseScreen();
		}
	}
}