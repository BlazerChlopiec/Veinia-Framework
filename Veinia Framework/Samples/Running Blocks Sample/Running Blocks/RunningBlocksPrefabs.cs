using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Veinia.RunningBlocks
{
	public class RunningBlocksPrefabs : PrefabManager
	{
		public override void LoadPrefabs(WorldTools tools)
		{
			base.LoadPrefabs(tools);

			Add("Block", new GameObject(new Transform(Vector2.Zero), new List<Component>
			{
				new Sprite("Sprites/Square", 0, Color.White, pixelsPerUnit: 100),
				new RectangleCollider(Vector2.Zero, Vector2.One)
			}, isStatic: false));
		}
	}
}
