using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Data;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using System.Collections.Generic;

namespace VeiniaFramework.Samples.Platformer
{
	public class Collectible : Component
	{
		ParticleData collectParticles;

		public override bool OnCollide(Fixture sender, Fixture other, Contact contact)
		{
			var tag = (Player)other.Body.Tag;
			if (tag != null && collectParticles == null) Collect();

			return true;
		}

		private void Collect()
		{
			CollectParticles();
			DestroyGameObject();
		}

		private void CollectParticles()
		{
			if (collectParticles == null)
			{
				var texture = new Texture2D(Globals.graphicsDevice, 1, 1);
				texture.SetData(new[] { Color.White });

				collectParticles = Globals.particleWorld.Add(new ParticleEffect("collect")
				{
					AutoTrigger = false,
					Emitters = new List<ParticleEmitter>
					{
						new ParticleEmitter
						{
							TextureRegion = new Texture2DRegion(texture),
							Profile = Profile.BoxFill(50, 50),
							LifeSpan = .4f,
							Parameters = new ParticleReleaseParameters
							{
								Speed = new ParticleFloatParameter(300, 500),
								Rotation = new ParticleFloatParameter(-1f, 1f),
								Quantity = new ParticleInt32Parameter(4),
							},
							Modifiers =
							{
								new AgeModifier
								{
									Interpolators =
									{
										new ColorInterpolator {  StartValue = new HslColor(56, 1f, .5f), EndValue = new HslColor(56, 1f, .5f) },
										new OpacityInterpolator{ StartValue = 1, EndValue = 0 },
										new ScaleInterpolator { StartValue = new Vector2(50,50), EndValue = Vector2.Zero },
									}
								},
								new RotationModifier {RotationRate = -2.1f},
							}
						}
					}
				}, Z: 1f);
			}
			collectParticles.effect.Trigger(transform.screenPos, layerDepth: .3f);
		}
	}
}