using Microsoft.Xna.Framework;

namespace Veinia.RunningBlocks
{
	public class SimpleTopDown : Component
	{
		Physics physics;

		private const float speed = 10;

		public override void Initialize()
		{
			physics = GetComponent<Physics>();
		}

		public override void Update()
		{
			physics.velocity = new Vector2(Globals.input.horizontal, Globals.input.vertical).SafeNormalize() * speed;
		}
	}
}
