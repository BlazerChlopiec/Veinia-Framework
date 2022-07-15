using Microsoft.Xna.Framework;

namespace Veinia
{
	public class Physics : Component
	{
		public Vector2 velocity;

		public override void Update()
		{
			transform.position += velocity * Time.deltaTime;


		}
	}
}
