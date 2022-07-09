using Microsoft.Xna.Framework;

namespace Veinia.RunningBlocks
{
	public class SimpleTopDown : Component
	{
		Physics physics;

		private const float speed = 30;

		public override void Initialize()
		{
			physics = GetComponent<Physics>();
		}

		public override void Update()
		{
			physics.velocity = Utils.SafeNormalize(new Vector2(Globals.input.horizontal, Globals.input.vertical))
								* speed * 50 * Time.deltaTime;
		}
	}
}
