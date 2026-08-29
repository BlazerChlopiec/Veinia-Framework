using Microsoft.Xna.Framework;

namespace VeiniaFramework.Samples.Physics
{
	public class MovingSquare : Component
	{
		public override void Update()
		{
			body.LinearVelocity = new Vector2(Globals.input.horizontal, Globals.input.vertical).SafeNormalize() * 10;

			transform.scale += Vector2.UnitX * Time.deltaTime;
		}
	}
}