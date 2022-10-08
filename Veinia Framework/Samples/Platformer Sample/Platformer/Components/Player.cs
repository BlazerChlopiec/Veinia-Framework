using Microsoft.Xna.Framework.Input;

namespace Veinia.Platformer
{
	public class Player : Component
	{
		private const float speed = 10;
		private const float jumpForce = 13;

		Physics physics;


		public override void Initialize() => physics = GetComponent<Physics>();

		public override void Update()
		{
			physics.velocity.X = Globals.input.horizontal * speed;

			if (Globals.input.GetKeyDown(Keys.Space) || Globals.input.GetKeyDown(Keys.W))
			{
				physics.velocity.Y = jumpForce;
			}
			if ((Globals.input.GetKeyUp(Keys.Space) || Globals.input.GetKeyUp(Keys.W)) && physics.velocity.Y > 0)
			{
				physics.velocity.Y *= .7f;
			}

			Globals.camera.LerpTo(transform.position, 10);
		}
	}
}