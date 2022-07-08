using Microsoft.Xna.Framework;

public static class Utils
{
	public static Vector2 SafeNormalize(Vector2 value)
	{
		// if the vector WOULD be normalized when its zero we would get NaN
		// so we need to only normalize the vector when its not Vector2.Zero
		if (value != Vector2.Zero)
		{
			value.Normalize();
		}

		return value;
	}

	public static Vector2 Vector2ToMatrix(Vector2 value)
	{
		value *= -1; // reverse the value (10 = -10, -10 = 10)

		value.X += Globals.screen.width / 2;
		value.Y += Globals.screen.height / 2;

		return value;
	}

	public static Vector2 PointToVector2(Point point)
	{
		return new Vector2(point.X, point.Y);
	}
	public static Point Vector2ToPoint(Vector2 vector2)
	{
		return new Point((int)vector2.X, (int)vector2.Y);
	}
}

