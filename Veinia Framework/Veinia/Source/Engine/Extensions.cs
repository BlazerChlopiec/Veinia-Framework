using Microsoft.Xna.Framework;
using MonoGame.Extended;
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
	}
}