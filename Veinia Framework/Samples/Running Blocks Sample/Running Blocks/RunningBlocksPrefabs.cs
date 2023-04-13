using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace VeiniaFramework.RunningBlocks
{
	public class RunningBlocksPrefabs : PrefabManager
	{
		public override void LoadPrefabs()
		{
			base.LoadPrefabs();

			Add("Block", new GameObject(new Transform(Vector2.Zero), new List<Component>
			{
				new Sprite("Sprites/Square", 0, Color.White, pixelsPerUnit: 200),
				new RectangleCollider(Vector2.Zero, Vector2.One)
			}, isStatic: false));
		}
	}
}
