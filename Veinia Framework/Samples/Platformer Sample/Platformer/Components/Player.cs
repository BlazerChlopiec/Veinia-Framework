using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace VeiniaFramework.Samples.Platformer
{
	public class Player : Component
	{
		private float speed = 8.5f;
		private float jumpForce = 13;
		private float accelerationSpeed = 20;

		Fixture groundCheck;
		int groundOverlaps;

		ParticleEffect jumpParticles;

		bool isTouchingGround => groundOverlaps >= 1;

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
			groundCheck.CollidesWith = Category.Cat2;

			Globals.camera.XY = transform.position;
		}

		float smoothZoom;
		float smoothHorizontal;
		public override void LateUpdate()
		{
			body.LinearVelocity = Vector2.Lerp(body.LinearVelocity, Vector2.UnitX * Globals.input.horizontal * speed, accelerationSpeed * Time.deltaTime).SetY(body.LinearVelocity.Y);

			if ((Globals.input.GetKeyButton(Keys.Space, Buttons.A) || Globals.input.GetKey(Keys.W)) && isTouchingGround && body.LinearVelocity.Y <= 0)
			{
				JumpParticles();
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

		private void JumpParticles()
		{
			if (jumpParticles == null)
			{
				var texture = new Texture2D(Globals.graphicsDevice, 1, 1);
				texture.SetData(new[] { Color.White });

				jumpParticles = Globals.particleWorld.Add(new ParticleEffect()
				{
					Emitters = new List<ParticleEmitter>
				{
					new ParticleEmitter(new TextureRegion2D(texture), 100, TimeSpan.FromSeconds(.5f),
						Profile.BoxFill(50, 50))
					{
						AutoTrigger=false,
						Parameters = new ParticleReleaseParameters
						{
							Speed = new Range<float>(150, 300),
							Rotation = new Range<float>(-1f, 1f),
							Quantity = 4,
						},
						Modifiers =
						{
							new AgeModifier
							{
								Interpolators =
								{
									new ColorInterpolator {  StartValue = new HslColor(74, .9f, .5f), EndValue = new HslColor(130, .41f, .5f) },
									new OpacityInterpolator{ StartValue = 1, EndValue = 0 },
									new ScaleInterpolator { StartValue = new Vector2(20,20), EndValue = Vector2.Zero },
								}
							},
							new RotationModifier {RotationRate = -2.1f},
							new LinearGravityModifier {Direction = -Vector2.UnitY, Strength = -1200},
						}
					}
				},
				});
			}
			jumpParticles.Trigger(transform.screenPos, layerDepth: .3f);
		}

		public override bool OnCollide(Fixture sender, Fixture other, Contact contact)
		{
			if (sender == groundCheck) groundOverlaps++;

			return true;
		}

		public override void OnSeparate(Fixture sender, Fixture other, Contact contact)
		{
			if (sender == groundCheck) groundOverlaps--;
		}
	}
}