using System;

namespace VeiniaFramework
{
	public class Screen
	{
		public int width { get; private set; }
		public int height { get; private set; }
		public bool fullscreen { get; private set; }


		public Screen(int width, int height, bool fullscreen)
		{
			this.width = width;
			this.height = height;
			this.fullscreen = fullscreen;

			UpdateChanges();
		}

		public void SetResolution(int X, int Y)
		{
			width = X;
			height = Y;

			UpdateChanges();
		}

		public void SetFullscreen(bool fullscreen)
		{
			this.fullscreen = fullscreen;
			UpdateChanges();
		}

		public void ToggleFullscreen()
		{
			fullscreen = !fullscreen;
			UpdateChanges();
		}

		private void UpdateChanges()
		{
			Globals.graphicsManager.PreferredBackBufferWidth = width;
			Globals.graphicsManager.PreferredBackBufferHeight = height;
			Globals.graphicsManager.IsFullScreen = fullscreen;
			Globals.graphicsManager.ApplyChanges();

			Globals.camera?.VirtualViewport.OnClientSizeChanged(this, EventArgs.Empty);

			Globals.viewportAdapter?.Reset();
		}
	}
}