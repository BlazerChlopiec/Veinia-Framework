using Microsoft.Xna.Framework;

namespace VeiniaFramework
{
	public sealed class Time
	{
		public static float time;

		public static float deltaTime;
		public static float unscaledDeltaTime;

		public static bool stop;

		private static int stopForFrames;
		private static bool frameStop;

		private static float stopForTime;
		private static bool timeStop;


		// update that runs even when stop = true
		public static void Update(GameTime gameTime)
		{
			if (!stop && !Veinia.PausedGameWhenInactiveWindow)
			{
				deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
				time += deltaTime;
			}
			else deltaTime = 0;


			unscaledDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (stopForFrames > 0) stopForFrames--;
			if (stopForTime > 0) stopForTime -= unscaledDeltaTime;

			if (stopForFrames <= 0 && stopForTime <= 0 && frameStop) { frameStop = false; stop = false; stopForFrames--; }
			if (stopForTime <= 0 && stopForFrames <= 0 && timeStop) { timeStop = false; stop = false; }

			Title.Add(deltaTime, " - Time.deltaTime", 3);
			Title.Add(unscaledDeltaTime, " - Time.unscaledDeltaTime", 4);
		}

		public static void StopForFrames(int frames)
		{
			stop = true;
			frameStop = true;
			stopForFrames = frames;
		}

		public static void StopForTime(float time)
		{
			stop = true;
			timeStop = true;
			stopForTime = time;
		}
	}
}
