using Microsoft.Xna.Framework;
using MonoGame.Extended.Tweening;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace VeiniaFramework.Samples.BlockBreaker
{
	public class Ball : Component
	{
		public float speed = 10;
		public bool launched;


		public override void Initialize()
		{
			body = Globals.physicsWorld.CreateCircle(.4f, 1f, bodyType: BodyType.Dynamic);
			body.IgnoreGravity = true;
			body.Tag = this;
		}

		public override void Update()
		{
			if (launched) transform.rotation += 200f * Time.deltaTime;

			if (Globals.input.GetMouseButtonDown(0) && !launched)
			{
				launched = true;
				FindComponentOfType<UI>().configButton.Enabled = false;
				body.LinearVelocity = Vector2.One.SafeNormalize() * speed;
			}

			else if (Globals.input.GetMouseButtonDown(0) && launched)
			{
				body.LinearVelocity = (Globals.input.GetMouseWorldPosition() - transform.position).SafeNormalize() * speed;
			}
		}

		public override bool OnCollide(Fixture sender, Fixture other, Contact contact)
		{
			var tag = (Tile)other.Body.Tag;
			if (tag == null) return true;
			tag.Hit();

			Globals.tweener.TweenTo(target: transform, expression: transform => transform.scale, toValue: new Vector2(1.3f, 1f), duration: .01f)
				.Easing(EasingFunctions.BackInOut)
				.OnEnd((x) =>
				{
					Globals.tweener.TweenTo(target: transform, expression: transform => transform.scale, toValue: Vector2.One, duration: .4f)
					.Easing(EasingFunctions.BackInOut);
				});

			return true;
			//if (Math.Abs(contact..Y) > Math.Abs(collisionInfo.PenetrationVector.X))
			//{
			//	//touched Y
			//	physics.velocity *= new Vector2(1, -1);
			//}

			//if (Math.Abs(collisionInfo.PenetrationVector.X) > Math.Abs(collisionInfo.PenetrationVector.Y))
			//{
			//	//touched X
			//	physics.velocity *= new Vector2(-1, 1);
			//}
		}
	}
}