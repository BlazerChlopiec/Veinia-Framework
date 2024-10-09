using GeonBit.UI;
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
			body = Globals.physicsWorld.CreateCircle(.4f, 0f, bodyType: BodyType.Dynamic);
			body.IgnoreGravity = true;
			body.Tag = this;
			body.SetRestitution(1);
			body.SetFriction(0);
		}

		public override void Update()
		{
			if (launched) transform.rotation += 200f * Time.deltaTime;

			if (Globals.input.GetMouseDown(0) && !launched)
			{
				launched = true;
				UserInterface.Active.RemoveEntity(FindComponentOfType<UI>().configButton);
				body.LinearVelocity = Vector2.One.SafeNormalize();
			}

#if DEBUG
			else if (Globals.input.GetMouseDown(0) && launched)
				body.LinearVelocity = (Globals.input.GetMouseWorldPosition() - transform.position).SafeNormalize() * speed;

#endif

			body.LinearVelocity = body.LinearVelocity.SafeNormalize() * speed;
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
		}
	}
}