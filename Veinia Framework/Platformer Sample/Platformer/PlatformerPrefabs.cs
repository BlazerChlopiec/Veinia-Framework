using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Veinia.Platformer
{
	public class PlatformerPrefabs : PrefabManager
	{
		public override void LoadPrefabs(WorldTools tools)
		{
			base.LoadPrefabs(tools);

			Add("Block", new GameObject(new Transform(Vector2.Zero), new List<Component>
			{
				new Sprite("Platformer/Square", 0, Color.White, destinationSize: Vector2.One),
				new RectanglePhysics(Vector2.Zero, Vector2.One)
			}, tools, isStatic: true));
		}
	}
}
