using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Collision.Shapes;
using tainicom.Aether.Physics2D.Dynamics;

namespace VeiniaFramework
{
	public class PhysicsEdge : PhysicsShape
	{
		Vector2 point1;
		Vector2 point2;
		float friction;
		float restitution;
		bool isSensor;
		Category category;

		private Shape shape;

		private Fixture fixture;

		public PhysicsEdge(Vector2 point1, Vector2 point2, float restitution = 0, bool isSensor = false, Category category = Category.None, BodyType bodyType = BodyType.Static, object tag = null, bool ignoreGravity = false)
			: base(bodyType, tag, ignoreGravity)
		{
			this.point1 = point1;
			this.point2 = point2;
			this.restitution = restitution;
			this.isSensor = isSensor;
			this.category = category;
		}

		protected override void MakeShape()
		{
			shape = new EdgeShape(point1, point2);

			fixture = body.CreateFixture(shape);

			fixture.Friction = friction;
			fixture.Restitution = restitution;
			fixture.IsSensor = isSensor;
			if (category != Category.None) fixture.CollisionCategories = category;
		}
	}
}