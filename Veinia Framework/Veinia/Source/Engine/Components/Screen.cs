using Microsoft.Xna.Framework.Graphics;

namespace Veinia
{
	public class Screen
	{
		public int width;
		public int height;


		public Screen(int width, int height)
		{
			this.width = width;
			this.height = height;

			UpdateChanges();
		}

		public void SetSize(int X, int Y)
		{
			width = X;
			height = Y;

			UpdateChanges();
		}

		private void UpdateChanges()
		{
			Globals.graphicsManager.PreferredBackBufferWidth = width;
			Globals.graphicsManager.PreferredBackBufferHeight = height;
			Globals.graphicsManager.ApplyChanges();
		}
	}
}