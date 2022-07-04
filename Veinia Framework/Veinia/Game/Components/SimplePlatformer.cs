using Microsoft.Xna.Framework.Input;

public class SimplePlatformer : Component
{
	private float speed;
	private float jumpForce;

	//Physics physics;


	public SimplePlatformer(float speed, float jumpForce)
	{
		this.speed = speed;
		this.jumpForce = jumpForce;
	}

	public override void Initialize()
	{
		//physics = GetComponent<Physics>();
	}

	public override void Update()
	{
		//physics.velocity.X = Globals.input.horizontal * speed;

		//if (Globals.input.GetKeyButton(Keys.W, Buttons.A) && physics.touchingBottom)
		//{
		//physics.velocity.Y = jumpForce;
		//}
		//if (Globals.input.GetKeyButtonUp(Keys.W, Buttons.A) && physics.velocity.Y > 0)
		//{
		//physics.velocity.Y *= 0.5f;
		//}

		Globals.camera.LerpTo(transform.position, 1f);
	}
}
