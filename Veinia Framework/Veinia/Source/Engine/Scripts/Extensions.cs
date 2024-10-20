﻿using Apos.Camera;
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

		public static void LerpTo(this Camera camera, Vector2 worldPos, float lerpT)
		{
			camera.SetPosition(Vector2.Lerp(camera.GetPosition(), worldPos, lerpT * Time.deltaTime));
		}

		public static void SetPosition(this Camera camera, Vector2 worldPos) => camera.XY = Transform.ToScreenUnits(worldPos);
		public static Vector2 GetPosition(this Camera camera) => Transform.ToWorldUnits(camera.XY);
		public static Vector2 GetSize(this Camera camera)
		{
			var a = new Vector2(camera.ViewRect.Left, camera.ViewRect.Bottom);
			var b = new Vector2(camera.ViewRect.Right, camera.ViewRect.Top);
			return Transform.ScreenToWorldPos(b - a);
		}
		public static Rectangle OffsetByHalf(this Rectangle rect, float xOffset = 0, float yOffset = 0, float shrink = 0)
		{
			rect.Offset(-rect.Width / 2 + xOffset, -rect.Height / 2 - yOffset);
			rect.Inflate(-shrink, -shrink);
			return rect;
		}
		public static RectangleF OffsetByHalf(this RectangleF rect, float xOffset = 0, float yOffset = 0, float shrink = 0)
		{
			rect.Offset(-rect.Width / 2 + xOffset, -rect.Height / 2 - yOffset);
			rect.Inflate(-shrink, -shrink);
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
		public static GameObject ExtractComponentToNewGameObject<T1>(this GameObject gameObject, Transform transform) where T1 : Component
		{
			return new GameObject(transform, new List<Component>
			{
				(T1)gameObject.GetComponent<T1>().Clone()
			}, isStatic: true);
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
		public static Color ToNegative(this Color color)
		{
			color = new Color(255 - color.R, 255 - color.G, 255 - color.B);
			return color;
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