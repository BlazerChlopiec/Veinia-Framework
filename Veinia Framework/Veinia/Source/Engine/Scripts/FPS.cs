using Microsoft.Xna.Framework;
using System;

namespace VeiniaFramework
{
	public sealed class FPS
	{
		// current frames per second of a game
		public float currentFps { get; private set; }


		public float averageFps { get; private set; }
		int frameCount;
		float timeAccumulator;

		private int targetFps = 60;

		public bool vSync
		{
			get { return Globals.graphicsManager.SynchronizeWithVerticalRetrace; }
			set
			{
				Globals.graphicsManager.SynchronizeWithVerticalRetrace = value;
				Globals.graphicsManager.ApplyChanges();
			}
		}

		public bool fixedTimestep { get { return game.IsFixedTimeStep; } set { game.IsFixedTimeStep = value; } }

		public Game game;


		public FPS(Game game) => this.game = game;

		public void ChangeFps(int value)
		{
			targetFps = value;
			game.TargetElapsedTime = TimeSpan.FromSeconds(1d / targetFps);
		}

		public void CalculateFps(GameTime gameTime)
		{
			currentFps = 1f / (float)gameTime.ElapsedGameTime.TotalSeconds; // TotalSeconds is a double by default

			currentFps = MathF.Round(currentFps); // round fps as it may give results simillar to this - 144,00003


			timeAccumulator += Time.deltaTime;
			frameCount++;

			if (timeAccumulator >= 1f)
			{
				averageFps = frameCount / timeAccumulator;

				// Round result for display
				averageFps = MathF.Round(averageFps);

				// Reset for next interval
				frameCount = 0;
				timeAccumulator -= 1f;
			}

			Title.Add(averageFps, " - Avg. FPS", 0);
			Title.Add(vSync, " - vSync", 1);
		}
	}
}