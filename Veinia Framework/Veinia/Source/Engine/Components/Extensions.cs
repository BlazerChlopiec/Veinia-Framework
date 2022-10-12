using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Veinia
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

		public static void LerpTo(this OrthographicCamera camera, Vector2 worldPos, float lerpT)
		{
			Globals.camera.Position = Vector2.Lerp(camera.Position, Transform.ToScreenUnits(worldPos),
													lerpT * Time.deltaTime);
		}

		public static void SetPosition(this OrthographicCamera camera, Vector2 worldPos) => camera.Position = Transform.ToScreenUnits(worldPos);
		public static Vector2 GetPosition(this OrthographicCamera camera) => Transform.ToWorldUnits(camera.Position);
		public static float GetScaleX(this OrthographicCamera camera)
		{
			var left = Transform.ScreenToWorldPos(camera.BoundingRectangle.Left, 0);
			var right = Transform.ScreenToWorldPos(camera.BoundingRectangle.Right, 0);
			return right.X - left.X;
		}
		public static float GetScaleY(this OrthographicCamera camera)
		{
			var bottom = Transform.ScreenToWorldPos(0, camera.BoundingRectangle.Bottom);
			var top = Transform.ScreenToWorldPos(0, camera.BoundingRectangle.Top);
			return top.Y - bottom.Y;
		}

		public static CollisionComponent GetReloaded(this CollisionComponent collisionComponent)
		{
			var prevBoundary = collisionComponent.boundary;

			return new CollisionComponent(prevBoundary);
		}
		public static Rectangle OffsetByHalf(this Rectangle rect)
		{
			rect.Offset(-rect.Width / 2, -rect.Height / 2);
			return rect;
		}
		public static RectangleF OffsetByHalf(this RectangleF rect)
		{
			rect.Offset(-rect.Width / 2, -rect.Height / 2);
			return rect;
		}
		public static GameObject ExtractSpriteToNewGameObject(this GameObject gameObject, Vector2 newPosition)
		{
			return new GameObject(new Transform(newPosition), new List<Component>
			{
				(Sprite)gameObject.GetComponent<Sprite>().Clone()
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
	}
}