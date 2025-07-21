using Microsoft.Xna.Framework;
using System.Linq;
using tainicom.Aether.Physics2D.Dynamics;

namespace VeiniaFramework
{
	public class RealtimeRectangle : Component
	{
		float width;
		float height;
		float friction;
		float restitution;
		bool isSensor;
		BodyType bodyType;

		Vector2 oldScale;


		public RealtimeRectangle(float width = 1, float height = 1, BodyType bodyType = BodyType.Static, float friction = .2f, float restitution = 0, bool isSensor = false)
		{
			this.width = width;
			this.height = height;
			this.bodyType = bodyType;
			this.friction = friction;
			this.restitution = restitution;
			this.isSensor = isSensor;
		}

		public override void Initialize() => MakeRectangle();

		public override void Update()
		{
			if (transform.scale != oldScale && body != null)
			{
				foreach (var item in body.FixtureList.ToArray())
				{
					body.Remove(item);
				}
				MakeRectangle();
			}
			oldScale = transform.scale;
		}

		private void MakeRectangle()
		{
			body = Globals.physicsWorld.CreateRectangle(width * transform.scale.X, height * transform.scale.Y, 1, bodyType: bodyType);
			body.FixtureList[0].Friction = friction;
			body.FixtureList[0].Restitution = restitution;
			body.FixtureList[0].IsSensor = isSensor;
		}
	}
}