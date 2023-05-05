using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;

namespace VeiniaFramework.Samples.Physics
{
	public class PhysicsMovement : Component
	{
		public override void Initialize()
		{
			body = Globals.physicsWorld.CreateRectangle(1, 1, 1, bodyType: BodyType.Dynamic);
			body.IgnoreGravity = true;
		}

		public override void Update() => body.LinearVelocity = new Vector2(Globals.input.horizontal, Globals.input.vertical).SafeNormalize() * 10;
	}
}