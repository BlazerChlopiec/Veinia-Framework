using Microsoft.Xna.Framework;

namespace Veinia
{
	public sealed class Time
	{
		public static float deltaTime;

		public static bool stop;

		private static int stopForFrames;
		private static bool frameStop;

		private static float stopForTime;
		private static bool timeStop;

		public static void Update(GameTime gameTime)
		{
			CalculateDelta(gameTime);

			if (stopForFrames > 0) stopForFrames--;
			if (stopForTime > 0) stopForTime -= deltaTime;

			if (stopForFrames <= 0 && stopForTime <= 0 && frameStop) { frameStop = false; stop = false; stopForFrames--; }
			if (stopForTime <= 0 && stopForFrames <= 0 && timeStop) { timeStop = false; stop = false; }
		}

		private static void CalculateDelta(GameTime gameTime)
		{
			// its important for the deltaTime to have maximum accuracy
			// to achieve it we use a 'TotalSeconds' double and convert it to a float (keeping the .14 accuracy of the double)
			// and assign it to our 'delta' (the final variable 'deltaTime' is a float for ease of use and avoiding converting errors) 
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds; // TotalSeconds is a double by default
			deltaTime = delta;

			Title.Add(deltaTime, " - Time.deltaTime", 2);
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
