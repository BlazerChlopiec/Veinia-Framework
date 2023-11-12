using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
	public class Collectible : Component
	{
		ParticleEffect collectParticles;

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

				collectParticles = Globals.particleWorld.Add(new ParticleEffect()
				{
					Emitters = new List<ParticleEmitter>
				{
					new ParticleEmitter(new TextureRegion2D(texture), 100, TimeSpan.FromSeconds(.4f),
						Profile.BoxFill(50, 50))
					{
						AutoTrigger=false,
						Parameters = new ParticleReleaseParameters
						{
							Speed = new Range<float>(300, 500),
							Rotation = new Range<float>(-1f, 1f),
							Quantity = 4,
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
				},
				});
			}
			collectParticles.Trigger(transform.screenPos, layerDepth: .3f);
		}
	}
}