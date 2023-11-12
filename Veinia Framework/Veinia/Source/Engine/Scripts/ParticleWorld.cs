using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Particles;
using System.Collections.Generic;

namespace VeiniaFramework
{
	public class ParticleWorld : IDrawn
	{
		public Dictionary<ParticleEffect, bool> particles = new Dictionary<ParticleEffect, bool>();

		public int ParticlesCount => particles.Count;

		public void QueueRemove(ParticleEffect particleEffect) => particles[particleEffect] = true;
		public void Clear() => particles.Clear();

		public ParticleEffect Add(ParticleEffect particleEffect)
		{
			particles.Add(particleEffect, false);
			return particleEffect;
		}

		public void Remove(ParticleEffect particleEffect)
		{
			particles.Remove(particleEffect);
			particleEffect.Dispose();
		}

		public void Update()
		{
			foreach (var p in particles)
			{
				p.Key.Update(Time.deltaTime);

				if (p.Value)
				{
					if (p.Key.ActiveParticles == 0)
					{
						Remove(p.Key);
					}
				}
			}
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (var p in particles)
			{
				sb.Draw(p.Key);
			}
		}
	}
}