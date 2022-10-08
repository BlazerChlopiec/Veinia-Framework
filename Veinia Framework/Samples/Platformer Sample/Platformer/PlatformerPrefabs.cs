using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Veinia.Platformer
{
	public class PlatformerPrefabs : PrefabManager
	{
		public override void LoadPrefabs()
		{
			base.LoadPrefabs();

			Add("White Block", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Square", .1f, Color.White, pixelsPerUnit: 200),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));
		}
	}
}
