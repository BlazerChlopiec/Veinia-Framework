using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Collision.Shapes;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;

namespace VeiniaFramework
{
	public class PhysicsChain : PhysicsShape
	{
		public Vector2[] vertices
		{
			get { return verts; }
			set
			{
				verts = value;

				if (body != null)
				{
					body.Remove(fixture);
					MakeShape();
				}
			}
		}
		private Vector2[] verts;

		float friction;
		float restitution;
		bool isSensor;
		Category category;

		private Shape shape;

		private Fixture fixture;

		public PhysicsChain(Vector2[] vertices, float restitution = 0, bool isSensor = false, Category category = Category.None, BodyType bodyType = BodyType.Static, object tag = null, bool ignoreGravity = false, bool sleepingAllowed = true)
			: base(bodyType, tag, ignoreGravity, sleepingAllowed)
		{
			this.verts = vertices;
			this.restitution = restitution;
			this.isSensor = isSensor;
			this.category = category;
		}

		protected override void MakeShape()
		{
			var v = new Vertices(vertices);
			shape = new ChainShape(v, createLoop: true);
			fixture = body.CreateFixture(shape);

			fixture.Friction = friction;
			fixture.Restitution = restitution;
			fixture.IsSensor = isSensor;
			if (category != Category.None) fixture.CollisionCategories = category;
		}
	}
}