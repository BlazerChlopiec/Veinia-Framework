using Microsoft.Xna.Framework;

namespace Veinia.BlockBreaker
{
	public class Paddle : Component
	{
		private const float yOffset = -4.5f;
		private const float xLimit = 8;


		public override void Update()
		{
			var mouseControls = new Vector2(Globals.input.GetMouseWorldPosition().X, yOffset);
			transform.position = new Vector2(MathHelper.Clamp(mouseControls.X, -xLimit, xLimit), mouseControls.Y);
		}
	}
}