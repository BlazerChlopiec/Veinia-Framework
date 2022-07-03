using Microsoft.Xna.Framework;
using System;

public sealed class FPS
{
	public float currentFps { get; private set; }
	public bool isVSync { get; private set; }
	private int targetFps = 60;


	public Game1 game;


	public FPS(Game1 GAME) => game = GAME;


	public void ChangeFps(int value)
	{
		if (value == int.MaxValue)
			game.IsFixedTimeStep = false; // isFixedTimeStep must be false when unlocking fps
		else
		{
			game.IsFixedTimeStep = true; // isFixedTimeStep must be true when limiting fps

			targetFps = value;
			game.TargetElapsedTime = TimeSpan.FromSeconds(1d / targetFps);
		}
	}

	public void vSync(bool value, GraphicsDeviceManager graphics)
	{
		graphics.SynchronizeWithVerticalRetrace = value; // change the vsync to 'value'

		isVSync = value;
	}

	public void CalculateFps(GameTime gameTime)
	{
		currentFps = 1f / (float)gameTime.ElapsedGameTime.TotalSeconds; // TotalSeconds is a double by default

		currentFps = MathF.Round(currentFps); // round fps as it may give results simillar to this - 144,00003
	}
}
