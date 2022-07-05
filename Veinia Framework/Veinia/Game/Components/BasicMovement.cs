using Microsoft.Xna.Framework;
using MonoGame.Extended.Collisions;

public class BasicMovement : Component
{
	private float speed;

	Physics physics;


	public BasicMovement(float speed)
	{
		this.speed = speed;
	}

	public override void Initialize()
	{
		physics = GetComponent<Physics>();
	}

	public override void Update()
	{
		physics.velocity = Utils.SafeNormalize(new Vector2(Globals.input.horizontal, Globals.input.vertical)) * speed;
	}

	public void OnColliderStay(CollisionEventArgs collisionInfo)
	{
		GetComponent<Sprite>().color = Color.Aqua;
	}
}