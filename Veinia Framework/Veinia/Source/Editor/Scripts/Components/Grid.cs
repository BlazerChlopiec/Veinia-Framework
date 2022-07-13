using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace Veinia.Editor
{
	public class Grid : Component, IDrawn
	{
		public void Draw(SpriteBatch sb)
		{
			var left = Transform.ScreenToWorldPos(Globals.camera.BoundingRectangle.Left, 0);
			var right = Transform.ScreenToWorldPos(Globals.camera.BoundingRectangle.Right, 0);
			int horizontalDifference = (int)MathF.Round(left.X) - (int)MathF.Round(right.X);

			var bottom = Transform.ScreenToWorldPos(0, Globals.camera.BoundingRectangle.Bottom);
			var top = Transform.ScreenToWorldPos(0, Globals.camera.BoundingRectangle.Top);
			int verticalDifference = (int)MathF.Round(bottom.Y) - (int)MathF.Round(top.Y);


			for (int x = 0; x < MathF.Abs(horizontalDifference) + 2; x++)
			{
				for (int y = 0; y < MathF.Abs(verticalDifference) + 2; y++)
				{
					var size = Transform.pixelsPerUnit;

					var currentRect = Transform.ScreenToWorldPos(Globals.camera.BoundingRectangle.Left - Transform.pixelsPerUnit + (x * Transform.pixelsPerUnit),
											 Globals.camera.BoundingRectangle.Bottom - (y * Transform.pixelsPerUnit));

					var currentWorldRectRounded = Transform.WorldToScreenPos(MathF.Round(currentRect.X), MathF.Round(currentRect.Y));


					sb.DrawRectangle(new RectangleF(currentWorldRectRounded.X, currentWorldRectRounded.Y,
									size, size).OffsetByHalf(), Color.White * .15f, 2, layerDepth: 1f);
				}
			}
		}
	}
}
