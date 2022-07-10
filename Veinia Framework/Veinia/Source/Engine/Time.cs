using Microsoft.Xna.Framework;

namespace Veinia
{
	public sealed class Time
	{
		public static float deltaTime;

		public static bool stop;


		public static void CalculateDelta(GameTime gameTime)
		{
			// its important for the deltaTime to have maximum accuracy
			// to achieve it we use a 'TotalSeconds' double and convert it to a float (keeping the .14 accuracy of the double)
			// and assign it to our 'delta' (the final variable 'deltaTime' is a float for ease of use and avoiding converting errors) 
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds; // TotalSeconds is a double by default
			deltaTime = delta;

			Title.Add(deltaTime, " - Time.deltaTime", 2);
		}
	}
}
