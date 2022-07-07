using MonoGame.Extended.Collisions;

public class GameOverBorder : Component
{
	RectanglePhysics physics;

	public override void Initialize()
	{
		physics = GetComponent<RectanglePhysics>();

		physics.onCollisionEnter += OnCollisionEnter;
	}

	private void OnCollisionEnter(CollisionEventArgs collisionInfo)
	{
		Globals.loader.Reload();
	}
}