using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Particles;
using System.Collections.Generic;

namespace VeiniaFramework
{
	public class ParticleWorld
	{
		public List<ParticleData> particles = new List<ParticleData>();

		public int ParticlesCount => particles.Count;


		public void QueueRemove(ParticleEffect particleEffect) => Find(particleEffect).queuedRemove = true;
		public void Clear() => particles.Clear();

		public ParticleEffect Add(ParticleEffect particleEffect, float Z = 0)
		{
			particles.Add(new ParticleData { effect = particleEffect, z = Z });
			return particleEffect;
		}

		public void Remove(ParticleEffect particleEffect)
		{
			var effect = Find(particleEffect);
			particles.Remove(effect);
			particleEffect.Dispose();
		}

		public void Update()
		{
			foreach (var p in particles.ToArray())
			{
				p.effect.Update(Time.deltaTime);

				if (p.queuedRemove)
				{
					if (p.effect.ActiveParticles == 0)
					{
						Remove(p.effect);
					}
				}
			}
		}

		public void Draw(SpriteBatch sb, Level level)
		{
			foreach (var p in particles)
			{
				level.drawCommands.Add(new DrawCommand
				{
					command = delegate
					{
						sb.Draw(p.effect);
					},
					Z = p.z
				});
			}
		}

		private ParticleData Find(ParticleEffect particleEffect) => particles.Find(x => x.effect == particleEffect);
	}
}

public class ParticleData
{
	public ParticleEffect effect;
	public bool queuedRemove;
	public float z;
}