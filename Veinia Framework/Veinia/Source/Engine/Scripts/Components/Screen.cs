using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VeiniaFramework
{
	public class Screen
	{
		public int width { get; private set; }
		public int height { get; private set; }
		public Vector2 Resolution => new Vector2(width, height);


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

			Globals.viewportAdapter?.Reset();
		}
	}
}