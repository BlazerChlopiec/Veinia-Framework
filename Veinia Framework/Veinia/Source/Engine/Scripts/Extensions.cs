using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VeiniaFramework
{
	public static class Extensions
	{
		public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
		{
			return listToClone.Select(item => (T)item.Clone()).ToList();
		}

		public static void AddAllTo<T>(this List<T> list, List<T> destination)
		{
			foreach (var item in list)
			{
				destination.Add(item);
			}
		}
		public static Rectangle OffsetNew(this Rectangle rect, Vector2 offset)
		{
			return new Rectangle(rect.X + (int)offset.X, rect.Y + (int)offset.Y, rect.Width, rect.Height);
		}
		public static Rectangle OffsetByHalf(this Rectangle rect)
		{
			rect.Offset(-rect.Width / 2, -rect.Height / 2);
			return rect;
		}
		public static Rectangle AllowNegativeSize(this Rectangle rect)
		{
			if (rect.Width < 0)
			{
				rect.X += rect.Width;
				rect.Width = Math.Abs(rect.Width);
			}
			if (rect.Height < 0)
			{
				rect.Y += rect.Height;
				rect.Height = Math.Abs(rect.Height);
			}
			return rect;
		}
		public static GameObject ExtractComponentToNewGameObject<T1>(this GameObject gameObject, Transform transform, bool isStatic = false) where T1 : Component
		{
			return new GameObject(transform, new List<Component>
			{
				(T1)gameObject.GetComponent<T1>().Clone()
			}, isStatic: isStatic);
		}
		public static Vector2 SafeNormalize(this Vector2 value)
		{
			// if the vector WOULD be normalized when its zero we would get NaN
			// so we need to only normalize the vector when its not Vector2.Zero
			if (value != Vector2.Zero)
			{
				value.Normalize();
			}

			return value;
		}
		public static Vector2 ClampLength(this Vector2 value, float maxMagnitude)
		{
			if (value.LengthSquared() > maxMagnitude * maxMagnitude)
			{
				return value.SafeNormalize() * maxMagnitude;
			}
			return value;
		}
		public static Vector2 RotateAround(this Vector2 vector, Vector2 origin, float rotation)
		{
			var rot = MathHelper.ToRadians(-rotation);
			float cos = MathF.Cos(rot);
			float sin = MathF.Sin(rot);
			Vector2 translated = vector - origin;

			float rotatedX = translated.X * cos - translated.Y * sin;
			float rotatedY = translated.X * sin + translated.Y * cos;

			return new Vector2(rotatedX, rotatedY) + origin;
		}
		public static Vector2 ReplaceY(this Vector2 a, float newY) => new Vector2(a.X, newY);
		public static Vector2 AddToY(this Vector2 a, float addY) => new Vector2(a.X, a.Y + addY);
		public static Vector2 GetWithoutY(this Vector2 a) => new Vector2(a.X, 0);
		public static Vector2 GetWithoutX(this Vector2 a) => new Vector2(0, a.Y);
		public static Vector3 ToVector3(this Vector2 a) => new Vector3(a.X, a.Y, 0);
		public static Vector3 ToVector3(this Vector2 a, float z) => new Vector3(a.X, a.Y, z);
		public static Vector2 ToVector2(this Vector3 a) => new Vector2(a.X, a.Y);
		public static Texture2D ChangeColor(this Texture2D texture, Color newColor, bool ignoreWhite = true)
		{
			if (newColor == Color.White && ignoreWhite) return texture;

			var colorData = new Color[texture.Width * texture.Height];
			texture.GetData(colorData);

			for (int i = 0; i < colorData.Length; i++)
			{
				var newColorHsl = newColor.ToHsl();
				var oldColorHsl = colorData[i].ToHsl();

				if (oldColorHsl.L < newColorHsl.L)
					colorData[i] = new HslColor(newColorHsl.H, newColorHsl.S, oldColorHsl.L).ToRgb();
				else
					colorData[i] = new HslColor(newColorHsl.H, newColorHsl.S, newColorHsl.L).ToRgb();
			}

			var temp = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height);
			temp.SetData(colorData);

			return temp;
		}
		public static float Clamp01(this float value)
		{
			return MathHelper.Clamp(value, 0f, 1f);
		}
		public static float NextFloat(this Random rng, float minValue, float maxValue)
		{
			return (float)(rng.NextDouble() * (maxValue - minValue) + minValue);
		}
		public static Color ToNegative(this Color color)
		{
			color = new Color(255 - color.R, 255 - color.G, 255 - color.B);
			return color;
		}
		public static Color Alpha01(this Color color, float alpha01)
		{
			byte alpha = (byte)(MathHelper.Clamp(alpha01, 0f, 1f) * 255f);
			return new Color(color.R, color.G, color.B, alpha);
		}
		public static Vector2 Round(this Vector2 vector, int decimalDigits)
		{
			return new Vector2((float)Math.Round(vector.X, decimalDigits), (float)Math.Round(vector.Y, decimalDigits));
		}
		public static Vector2 FlipY(this Vector2 vector)
		{
			return new Vector2(vector.X, -vector.Y);
		}
		public static Dictionary<string, T1> LoadAll<T1>(this ContentManager content, string contentFolder)
		{
			DirectoryInfo dir = new DirectoryInfo(content.RootDirectory + "/" + contentFolder);
			if (!dir.Exists)
				throw new DirectoryNotFoundException();
			Dictionary<string, T1> result = new Dictionary<string, T1>();

			FileInfo[] files = dir.GetFiles("*.*");
			foreach (FileInfo file in files)
			{
				string key = Path.GetFileNameWithoutExtension(file.Name);
				result[key] = content.Load<T1>(contentFolder + "/" + key);
			}

			return result;
		}
	}
}