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
				new Sprite("Block Breaker/Grass Tile", .1f, Color.White, Vector2.One),
				new RectangleCollider(Vector2.Zero, Vector2.One),
				new Block(),
			}, isStatic: true));

			Add("Moving Block", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Block Breaker/Grass Tile", .1f, Color.Red, Vector2.One),
				new RectangleCollider(Vector2.Zero, Vector2.One),
				new MovingBlock(),
			}, isStatic: true));
		}
	}
}