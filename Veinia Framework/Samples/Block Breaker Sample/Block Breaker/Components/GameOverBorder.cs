using MonoGame.Extended.Collisions;

namespace VeiniaFramework.BlockBreaker
{
	public class GameOverBorder : Component
	{
		public override void OnCollide(Collider self, CollisionState state, CollisionEventArgs collisionInfo)
		{
			if (state == CollisionState.Enter) FindComponentOfType<UI>().ShowLoseScreen();
		}
	}
}