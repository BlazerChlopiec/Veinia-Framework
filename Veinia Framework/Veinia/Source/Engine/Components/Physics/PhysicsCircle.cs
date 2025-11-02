using tainicom.Aether.Physics2D.Collision.Shapes;
using tainicom.Aether.Physics2D.Dynamics;

namespace VeiniaFramework
{
	public class PhysicsCircle : PhysicsShape
	{
		float radius;
		float friction;
		float restitution;
		bool isSensor;
		Category category;

		private Shape shape;

		private Fixture fixture;

		public PhysicsCircle(float radius = 1, float friction = .2f, float restitution = 0, bool isSensor = false, Category category = Category.None, BodyType bodyType = BodyType.Static, object tag = null, bool ignoreGravity = false)
			: base(bodyType, tag, ignoreGravity)
		{
			this.radius = radius;
			this.friction = friction;
			this.restitution = restitution;
			this.isSensor = isSensor;
			this.category = category;
		}

		protected override void MakeShape()
		{
			shape = new CircleShape(radius, 1f);

			fixture = body.CreateFixture(shape);

			fixture.Friction = friction;
			fixture.Restitution = restitution;
			fixture.IsSensor = isSensor;
			if (category != Category.None) fixture.CollisionCategories = category;
		}
	}
}