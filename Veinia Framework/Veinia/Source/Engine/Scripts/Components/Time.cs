using Microsoft.Xna.Framework;

namespace Veinia
{
	public sealed class Time
	{
		public static float deltaTime;

		public static bool stop;
		private static int stopForFrames;

		public static void Update(GameTime gameTime)
		{
			CalculateDelta(gameTime);

			stopForFrames--;
			if (stopForFrames == 0) { stop = false; stopForFrames--; }
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
			stopForFrames = frames;
		}
	}
}
