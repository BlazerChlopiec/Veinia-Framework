using Microsoft.Xna.Framework;
using System;

namespace VeiniaFramework
{
	public sealed class FPS
	{
		// current frames per second of a game
		public float currentFps { get; private set; }
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
		}
	}

}