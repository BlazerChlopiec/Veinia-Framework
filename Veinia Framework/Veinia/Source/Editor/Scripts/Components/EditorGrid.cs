using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;

namespace VeiniaFramework.Editor
{
	public class EditorGrid : Component, IDrawn
	{
		bool hideGrid;
		float opacity = 0.05f;


		public override void Initialize() => EditorCheckboxes.Add("Hide Grid [G]", defaultValue: false, (e, o) => { hideGrid = true; }, (e, o) => { hideGrid = false; }, Keys.G);

		public void Draw(SpriteBatch sb)
		{
			if (hideGrid) return;

			var camera = Globals.camera;
			var unit = Transform.unitSize;

			//Say.Line(Globals.camera.GetScaleX() + "   " + Globals.camera.GetScaleY());
			//camera.ViewRect.Center
			var rect = camera.GetViewRect();

			var test = Transform.ScreenToWorldPos(Globals.camera.ViewRect.Left, Globals.camera.ViewRect.Bottom);

			var currentWorldRectRounded = Transform.WorldToScreenPos(MathF.Round(test.X), MathF.Round(test.Y));


			sb.DrawRectangle(new RectangleF(currentWorldRectRounded.X, currentWorldRectRounded.Y,
			unit, unit).OffsetByHalf(), Color.White, 2, layerDepth: 1);
		}
		private void OldGrid(SpriteBatch sb)
		{

			var left = Transform.ScreenToWorldPos(Globals.camera.ViewRect.Left, 0);
			var right = Transform.ScreenToWorldPos(Globals.camera.ViewRect.Right, 0);
			int horizontalDifference = (int)MathF.Round(left.X) - (int)MathF.Round(right.X);

			var bottom = Transform.ScreenToWorldPos(0, Globals.camera.ViewRect.Bottom);
			var top = Transform.ScreenToWorldPos(0, Globals.camera.ViewRect.Top);
			int verticalDifference = (int)MathF.Round(bottom.Y) - (int)MathF.Round(top.Y);

			for (int x = 0; x < MathF.Abs(horizontalDifference) + 2; x++)
			{
				for (int y = 0; y < MathF.Abs(verticalDifference) + 2; y++)
				{
					var size = Transform.unitSize;

					var currentRect = Transform.ScreenToWorldPos(Globals.camera.ViewRect.Left - Transform.unitSize + (x * Transform.unitSize),
											 Globals.camera.ViewRect.Bottom - (y * Transform.unitSize));

					var currentWorldRectRounded = Transform.WorldToScreenPos(MathF.Round(currentRect.X), MathF.Round(currentRect.Y));


					sb.DrawRectangle(new RectangleF(currentWorldRectRounded.X, currentWorldRectRounded.Y,
									size, size).OffsetByHalf(), Color.White * opacity, 2, layerDepth: .9f);
				}
			}

		}
	}
}
