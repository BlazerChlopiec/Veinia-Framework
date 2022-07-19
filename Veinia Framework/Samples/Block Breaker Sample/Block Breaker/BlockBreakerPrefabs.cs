using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Veinia.BlockBreaker
{
	public class BlockBreakerPrefabs : PrefabManager
	{
		public override void LoadPrefabs(WorldTools tools)
		{
			base.LoadPrefabs(tools);

			Add("Grass Tile", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Grass Tile", .1f, Color.White, pixelsPerUnit: 200),
				new RectangleCollider(Vector2.Zero, Vector2.One),
				new GrassTile(),
			}, isStatic: true));

			Add("Moving Tile", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Moving Tile", .1f, Color.White, pixelsPerUnit: 200),
				new RectangleCollider(Vector2.Zero, Vector2.One),
				new MovingBlock(),
			}, isStatic: true));

			Add("Metal Tile", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Metal Tile", .1f, Color.White, pixelsPerUnit: 200),
				new RectangleCollider(Vector2.Zero, Vector2.One),
				new MetalBlock(),
			}, isStatic: true));

			Add("Background", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Background", 0, Color.White, pixelsPerUnit: 200),
			}, isStatic: true));
		}
	}
}