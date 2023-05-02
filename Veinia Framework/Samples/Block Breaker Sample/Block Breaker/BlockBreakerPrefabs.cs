using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace VeiniaFramework.Samples.BlockBreaker
{
	public class BlockBreakerPrefabs : PrefabManager
	{
		public override void LoadPrefabs()
		{
			base.LoadPrefabs();

			Add("Grass Tile", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Grass Tile", .1f, Color.White, pixelsPerUnit: 200),
				new GrassTile(),
			}, isStatic: true));

			Add("Moving Tile", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Moving Tile", .1f, Color.White, pixelsPerUnit: 200),
				new MovingBlock(),
			}, isStatic: true));

			Add("Metal Tile", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Metal Tile", .1f, Color.White, pixelsPerUnit: 200),
				new MetalBlock(),
			}, isStatic: true));

			Add("Background", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Background", 0, Color.White, pixelsPerUnit: 200),
			}, isStatic: true));
		}
	}
}