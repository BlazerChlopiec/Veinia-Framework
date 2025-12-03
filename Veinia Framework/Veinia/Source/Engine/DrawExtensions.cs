using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace VeiniaFramework
{
	public static class DrawExtensions
	{
		public static void VeiniaPointWorld(this SpriteBatch sb, Level level, Vector2 position, Color? color = null, float size = 10, float z = float.MaxValue)
		{
			color = color ?? Color.White;

			level.drawCommands.Add(new DrawCommand
			{
				command = delegate
				{
					sb.DrawPoint(Transform.WorldToScreenPos(position), color.Value, size, 0);
				},
				Z = z
			});
		}

		public static void VeiniaTextWorld(this SpriteBatch sb, Level level, Vector2 position, string text, Color? color = null, float size = .5f, SpriteFont font = null, Effect effect = null, float z = float.MaxValue)
		{
			color = color ?? Color.White;
			if (text == null || text == string.Empty) return;

			level.drawCommands.Add(new DrawCommand
			{
				command = delegate
				{
					var spriteFont = font ?? Resources.Fonts[0];
					var textSize = spriteFont.MeasureString(text);
					var origin = new Vector2(textSize.X / 2f, textSize.Y / 2.5f);

					sb.DrawString(spriteFont, text, Transform.WorldToScreenPos(position), color.Value, 0f, origin, size, SpriteEffects.None, 0f);
				},
				Z = z,
				shader = effect
			});
		}

		public static void VeiniaLineWorld(this SpriteBatch sb, Level level, Vector2 point1, Vector2 point2, Color? color = null, float thickness = 10, float z = float.MaxValue)
		{
			color = color ?? Color.White;

			level.drawCommands.Add(new DrawCommand
			{
				command = delegate
				{
					sb.DrawLine(Transform.WorldToScreenPos(point1), Transform.WorldToScreenPos(point2), color.Value, thickness);
				},
				Z = z
			});
		}
		public static void VeiniaLine(this SpriteBatch sb, Level level, Vector2 point1, Vector2 point2, Color? color = null, float thickness = 10, float z = float.MaxValue)
		{
			color = color ?? Color.White;

			level.drawCommands.Add(new DrawCommand
			{
				command = delegate
				{
					sb.DrawLine(point1, point2, color.Value, thickness);
				},
				Z = z
			});
		}

		public static void VeiniaCircle(this SpriteBatch sb, Level level, Vector2 position, Color? color = null, float radius = 1, int sides = 10, float thickness = 1, float z = float.MaxValue)
		{
			color = color ?? Color.White;

			level.drawCommands.Add(new DrawCommand
			{
				command = delegate
				{
					sb.DrawCircle(new CircleF(position.ToPoint(), (radius * Transform.unitSize) / 2), sides, color.Value, thickness);
				},
				Z = z
			});
		}

		public static void VeiniaRectangle(this SpriteBatch sb, Level level, RectangleF rectangle, Color? color = null, float thickness = 1, float z = float.MaxValue)
		{
			color = color ?? Color.White;

			level.drawCommands.Add(new DrawCommand
			{
				command = delegate
				{
					sb.DrawRectangle(rectangle, color.Value, thickness);
				},
				Z = z
			});
		}

		public static void VeiniaRectangleRotated(this SpriteBatch sb, Level level, RectangleF rectangle, Color? color = null, float thickness = 1, float rotation = 0, float z = float.MaxValue)
		{
			color = color ?? Color.White;

			Vector2 center = new Vector2(rectangle.X + rectangle.Width / 2f, rectangle.Y + rectangle.Height / 2f);

			Vector2 topLeft = new Vector2(rectangle.Left, rectangle.Top);
			Vector2 topRight = new Vector2(rectangle.Right, rectangle.Top);
			Vector2 bottomRight = new Vector2(rectangle.Right, rectangle.Bottom);
			Vector2 bottomLeft = new Vector2(rectangle.Left, rectangle.Bottom);

			topLeft = topLeft.RotateAround(center, -rotation);
			topRight = topRight.RotateAround(center, -rotation);
			bottomRight = bottomRight.RotateAround(center, -rotation);
			bottomLeft = bottomLeft.RotateAround(center, -rotation);

			level.drawCommands.Add(new DrawCommand
			{
				command = delegate
				{
					sb.DrawLine(topLeft.X, topLeft.Y, topRight.X, topRight.Y, color.Value, thickness);
					sb.DrawLine(topRight.X, topRight.Y, bottomRight.X, bottomRight.Y, color.Value, thickness);
					sb.DrawLine(bottomRight.X, bottomRight.Y, bottomLeft.X, bottomLeft.Y, color.Value, thickness);
					sb.DrawLine(bottomLeft.X, bottomLeft.Y, topLeft.X, topLeft.Y, color.Value, thickness);
				},
				Z = z
			});
		}
	}
}