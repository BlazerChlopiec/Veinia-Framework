using tainicom.Aether.Physics2D.Dynamics;

namespace VeiniaFramework
{
	public class PhysicsShape : Component
	{
		object tag;
		BodyType bodyType;
		bool ignoreGravity;
		bool sleepingAllowed;


		public PhysicsShape(BodyType bodyType = BodyType.Static, object tag = null, bool ignoreGravity = false, bool sleepingAllowed = true)
		{
			this.bodyType = bodyType;
			this.tag = tag;
			this.ignoreGravity = ignoreGravity;
			this.sleepingAllowed = sleepingAllowed;
		}

		public override void EarlyInitialize()
		{
			body = Globals.physicsWorld.CreateBody(bodyType: bodyType);
			body.Tag = tag;
			body.IgnoreGravity = ignoreGravity;
			body.SleepingAllowed = sleepingAllowed;

			MakeShape();
		}

		protected virtual void MakeShape() { }
	}
}