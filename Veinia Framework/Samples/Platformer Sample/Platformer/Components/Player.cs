using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace VeiniaFramework.Samples.Platformer
{
	public class Player : Component
	{
		private float speed = 8.5f;
		private float jumpForce = 13;
		private float accelerationSpeed = 20;

		bool isTouchingGround;
		Fixture groundCheck;

		public override void Initialize()
		{
			body = Globals.physicsWorld.CreateBody(bodyType: BodyType.Dynamic);

			// top bottom
			body.CreateEdge(new Vector2(.5f, -.5f), new Vector2(-.5f, -.5f));
			body.CreateEdge(new Vector2(.5f, .5f), new Vector2(-.5f, .5f));

			// left right
			body.CreateEdge(new Vector2(.5f, -.5f), new Vector2(.5f, .5f));
			body.CreateEdge(new Vector2(-.5f, -.5f), new Vector2(-.5f, .5f));

			body.Tag = this;
			body.FixedRotation = true;
			body.SetFriction(0);

			groundCheck = body.CreateRectangle(.9f, .1f, 1f, Vector2.UnitY * -.5f);
			groundCheck.IsSensor = true;
			groundCheck.CollidesWith = Category.Cat1;

			Globals.camera.XY = transform.position;
		}

		float smoothZoom;
		float smoothHorizontal;
		public override void Update()
		{
			body.LinearVelocity = Vector2.Lerp(body.LinearVelocity, Vector2.UnitX * Globals.input.horizontal * speed, accelerationSpeed * Time.deltaTime).SetY(body.LinearVelocity.Y);

			if ((Globals.input.GetKeyButtonDown(Keys.Space, Buttons.A) || Globals.input.GetKeyDown(Keys.W)) && isTouchingGround)
			{
				isTouchingGround = false;
				body.LinearVelocity = new Vector2(body.LinearVelocity.X, jumpForce);
			}
			if ((Globals.input.GetKeyButtonUp(Keys.Space, Buttons.A) || Globals.input.GetKeyUp(Keys.W)) && body.LinearVelocity.Y > 0)
			{
				float jumpCutoff = .7f;
				body.LinearVelocity *= new Vector2(1, jumpCutoff);
			}

			Globals.camera.LerpTo(transform.position, 10);

			float zoomAmount = 7;
			float zoomSpeed = .8f;
			smoothZoom = MathHelper.Lerp(smoothZoom, (MathF.Abs(smoothHorizontal) / zoomAmount), zoomSpeed * Time.deltaTime);
			Globals.camera.Scale = Vector2.One * (1f - smoothZoom);
		}

		public override bool OnCollide(Fixture sender, Fixture other, Contact contact)
		{
			if (sender == groundCheck) isTouchingGround = true;

			return true;
		}
	}
}