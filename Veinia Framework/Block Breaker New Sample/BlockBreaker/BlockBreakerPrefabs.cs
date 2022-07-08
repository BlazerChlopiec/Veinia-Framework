using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Veinia.BlockBreaker
{
	public class BlockBreakerPrefabs : PrefabManager
	{
		public override void LoadPrefabs(WorldTools tools)
		{
			base.LoadPrefabs(tools);

			Add("Block", new GameObject(Transform.Empty, new List<Component>
		{
			new Sprite("Pong/Grass Tile", 0, Color.White, Vector2.One),
			new RectanglePhysics(Vector2.Zero, Vector2.One),
			new Block(),
		}, tools, isStatic: true));
		}
	}
}