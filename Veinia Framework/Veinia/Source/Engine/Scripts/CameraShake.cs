using Microsoft.Xna.Framework;
using System;

namespace VeiniaFramework
{
	public class CameraShake
	{
		public Vector2 shakeOffset;

		static float durationRemaining = 0f;
		static float duration;
		static float magnitude;

		Random random;

		public CameraShake() => random = new Random();


		public static void Shake(float duration = .5f, float magnitude = 1f)
		{
			durationRemaining = duration;
			CameraShake.duration = duration;
			CameraShake.magnitude = magnitude;
		}

		public void DrawUpdate()
		{
			if (durationRemaining > 0f)
			{
				durationRemaining -= Time.deltaTime;

				// Linear fade out
				float fade = durationRemaining / duration;
				float currentMagnitude = magnitude * fade;

				float offsetX = (float)(random.NextDouble() * 2.0 - 1.0) * currentMagnitude;
				float offsetY = (float)(random.NextDouble() * 2.0 - 1.0) * currentMagnitude;

				shakeOffset = new Vector2(offsetX, offsetY) * Transform.unitSize;
			}
			else
			{
				shakeOffset = new Vector2(0f, 0f);
			}
		}
	}
}