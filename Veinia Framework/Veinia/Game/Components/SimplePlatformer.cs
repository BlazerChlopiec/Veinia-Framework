using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Collisions;

public class SimplePlatformer : Component
{
	private float speed;
	private float jumpForce;

	RectanglePhysics physics;
	CirclePhysics circlePhysics;


	public SimplePlatformer(float speed, float jumpForce)
	{
		this.speed = speed;
		this.jumpForce = jumpForce;
	}

	public override void Initialize()
	{
		physics = GetComponent<RectanglePhysics>();
		circlePhysics = GetComponent<CirclePhysics>();
		circlePhysics.onCollision += OnTouchBottom;
	}

	public override void Update()
	{
		physics.velocity.Y -= 30 * Time.deltaTime;
		physics.velocity.X = Globals.input.horizontal * speed;


		if (Globals.input.GetKeyButtonUp(Keys.W, Buttons.A) && physics.velocity.Y > 0)
		{
			physics.velocity.Y *= 0.5f;
		}

		Globals.camera.LerpTo(transform.position, 1f);
	}

	public void OnTouchBottom(CollisionEventArgs collisionInfo)
	{
		if (collisionInfo.Other.physics.trigger) return;
		physics.velocity.Y = -2;
		if (Globals.input.GetKeyButton(Keys.W, Buttons.A))
		{
			physics.velocity.Y = jumpForce;
		}
	}
}
