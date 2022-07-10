using MonoGame.Extended.Collisions;

namespace Veinia.BlockBreaker
{
	public class GameOverBorder : Component
	{
		RectangleCollider physics;


		public override void Initialize()
		{
			physics = GetComponent<RectangleCollider>();

			physics.onCollisionEnter += OnCollisionEnter;
		}

		private void OnCollisionEnter(CollisionEventArgs collisionInfo)
		{
			Globals.loader.Reload();
		}
	}
}