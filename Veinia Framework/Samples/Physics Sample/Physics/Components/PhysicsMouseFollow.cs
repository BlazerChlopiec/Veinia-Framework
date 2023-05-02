using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace VeiniaFramework.Samples.Physics
{
	public class PhysicsMovement : Component
	{
		public override void Initialize()
		{
			body = Globals.physicsWorld.CreateRectangle(1, 1, 1, bodyType: BodyType.Dynamic);
			body.IgnoreGravity = true;
		}

		public override void Update()
		{
			body.LinearVelocity = new Vector2(Globals.input.horizontal, Globals.input.vertical).SafeNormalize() * 5;
		}

		public override void OnCollide(Fixture sender, Fixture other, Contact contact)
		{
			Say.Line(other.Shape);
		}
	}
}
