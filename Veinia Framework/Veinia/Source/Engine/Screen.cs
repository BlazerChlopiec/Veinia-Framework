using Microsoft.Xna.Framework;
using System;

namespace VeiniaFramework
{
	public class Screen
	{
		public int Width { get; private set; }
		public int Height { get; private set; }
		public bool Fullscreen { get; private set; }
		public Vector2 Dimensions => new Vector2(Width, Height);

		public Action OnStateChanged; // resizing, fullscreen. etc


		public Screen(int width, int height, bool fullscreen)
		{
			this.Width = width;
			this.Height = height;
			this.Fullscreen = fullscreen;

			UpdateChanges();
		}

		public void SetResolution(int X, int Y)
		{
			Width = X;
			Height = Y;

			UpdateChanges();
		}

		public void SetFullscreen(bool fullscreen)
		{
			this.Fullscreen = fullscreen;
			UpdateChanges();
		}

		public void ToggleFullscreen()
		{
			Fullscreen = !Fullscreen;
			UpdateChanges();
		}

		private void UpdateChanges()
		{
			Globals.graphicsManager.PreferredBackBufferWidth = Width;
			Globals.graphicsManager.PreferredBackBufferHeight = Height;
			Globals.graphicsManager.IsFullScreen = Fullscreen;
			Globals.graphicsManager.ApplyChanges();

			Globals.camera?.VirtualViewport.OnClientSizeChanged(this, EventArgs.Empty);
			OnStateChanged?.Invoke();
		}

		// window resize
		public void ClientSizeChanged()
		{
			Width = Globals.graphicsDevice.PresentationParameters.BackBufferWidth;
			Height = Globals.graphicsDevice.PresentationParameters.BackBufferHeight;

			OnStateChanged?.Invoke();
		}

		public override string ToString() => $"{Width}x{Height}, fullscreen: {Fullscreen}";
	}
}