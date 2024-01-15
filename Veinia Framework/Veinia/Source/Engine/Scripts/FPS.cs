using Microsoft.Xna.Framework;
using System;

namespace VeiniaFramework
{
	public sealed class FPS
	{
		public float currentFps { get; private set; }
		public bool isVSync { get; private set; } = true;
		public bool isFixedTimestep { get; private set; } = true;
		private int targetFps = 60;

		public Game game;


		public FPS(Game game) => this.game = game;

		public void ChangeFps(int value)
		{
			targetFps = value;
			game.TargetElapsedTime = TimeSpan.FromSeconds(1d / targetFps);
		}

		public void vSync(bool value)
		{
			Globals.graphicsManager.SynchronizeWithVerticalRetrace = value;
			Globals.graphicsManager.ApplyChanges();

			isVSync = value;
		}

		public void FixedTimestep(bool value)
		{
			game.IsFixedTimeStep = value;

			isFixedTimestep = value;
		}

		public void CalculateFps(GameTime gameTime)
		{
			currentFps = 1f / (float)gameTime.ElapsedGameTime.TotalSeconds; // TotalSeconds is a double by default

			currentFps = MathF.Round(currentFps); // round fps as it may give results simillar to this - 144,00003
		}
	}

}