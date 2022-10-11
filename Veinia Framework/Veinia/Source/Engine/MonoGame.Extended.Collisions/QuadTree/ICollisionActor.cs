using Veinia;

namespace MonoGame.Extended.Collisions
{
	/// <summary>
	/// An actor that can be collided with.
	/// </summary>
	public interface ICollisionActor
	{
		IShapeF Bounds { get; }
		Collider collider { get; }


		void OnCollision(CollisionEventArgs collisionInfo);
		void OnTrigger(CollisionEventArgs collisionInfo);
	}
}