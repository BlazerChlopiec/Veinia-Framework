using tainicom.Aether.Physics2D.Dynamics;

namespace VeiniaFramework
{
	public class PhysicsShape : Component
	{
		object tag;
		BodyType bodyType;
		bool ignoreGravity;


		public PhysicsShape(BodyType bodyType = BodyType.Static, object tag = null, bool ignoreGravity = false)
		{
			this.bodyType = bodyType;
			this.tag = tag;
			this.ignoreGravity = ignoreGravity;
		}

		public override void EarlyInitialize()
		{
			body = Globals.physicsWorld.CreateBody(bodyType: bodyType);
			body.Tag = tag;
			body.IgnoreGravity = ignoreGravity;

			MakeShape();
		}

		protected virtual void MakeShape() { }
	}
}