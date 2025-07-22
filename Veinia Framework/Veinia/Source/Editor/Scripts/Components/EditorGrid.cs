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
		public static float opacity = 0.05f;


		public override void Initialize() => EditorCheckboxes.Add("Hide Grid [G]", defaultValue: false, (e, o) => { hideGrid = true; }, (e, o) => { hideGrid = false; }, Keys.G);

		public void Draw(SpriteBatch sb)
		{
			if (hideGrid) return;

			var camera = Globals.camera;
			var unit = Transform.unitSize;
			var size = camera.GetSize();

			var leftBottom = Transform.WorldToScreenPos(Vector2.Round(camera.GetPosition() - Vector2.UnitX * (size.X / 2) - Vector2.UnitY * (size.Y / 2)) - Vector2.UnitX / 2);
			for (int i = 0; i < MathF.Round(size.Y) + 1; i++)
			{
				var yOffset = (-Vector2.UnitY * unit * i) + -Vector2.UnitY * unit / 2;
				var xOffset = (Vector2.UnitX * unit * (size.X + 1));
				sb.DrawLine(leftBottom + yOffset, leftBottom + xOffset + yOffset, Color.White * opacity, 2, layerDepth: .9f);
			}
			//sb.DrawRectangle(new RectangleF(leftBottom.X, leftBottom.Y, unit, unit), Color.Red, 5, 1);

			var rightTop = Transform.WorldToScreenPos(Vector2.Round(camera.GetPosition() + Vector2.UnitX * (size.X / 2) + Vector2.UnitY * (size.Y / 2)) + Vector2.UnitY / 2);
			for (int i = 0; i < MathF.Round(size.X) + 1; i++)
			{
				var xOffset = (-Vector2.UnitX * unit * i) + -Vector2.UnitX * unit / 2;
				var yOffset = (Vector2.UnitY * unit * (size.Y + 1));
				sb.DrawLine(rightTop + xOffset, rightTop + xOffset + yOffset, Color.White * opacity, 2, layerDepth: .9f);
			}
			//sb.DrawRectangle(new RectangleF(rightTop.X, rightTop.Y, unit, unit), Color.Red, 5, 1);
		}
	}
}
