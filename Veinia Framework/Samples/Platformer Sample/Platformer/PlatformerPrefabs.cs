using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Veinia.Platformer
{
	public class PlatformerPrefabs : PrefabManager
	{
		public override void LoadPrefabs()
		{
			base.LoadPrefabs();

			Add("Tile (1)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Tiles/Tile (1)", .1f, Color.White, pixelsPerUnit: 128),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));
			Add("Tile (2)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Tiles/Tile (2)", .1f, Color.White, pixelsPerUnit: 128),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));
			Add("Tile (3)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Tiles/Tile (3)", .1f, Color.White, pixelsPerUnit: 128),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));
			Add("Tile (4)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Tiles/Tile (4)", .1f, Color.White, pixelsPerUnit: 128),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));
			Add("Tile (5)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Tiles/Tile (5)", .1f, Color.White, pixelsPerUnit: 128),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));
			Add("Tile (6)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Tiles/Tile (6)", .1f, Color.White, pixelsPerUnit: 128),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));

			Add("Tile (7)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Tiles/Tile (7)", .1f, Color.White, pixelsPerUnit: 128),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));
			Add("Tile (8)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Tiles/Tile (8)", .1f, Color.White, pixelsPerUnit: 128),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));
			Add("Tile (9)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Tiles/Tile (9)", .1f, Color.White, pixelsPerUnit: 128),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));
			Add("Tile (10)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Tiles/Tile (10)", .1f, Color.White, pixelsPerUnit: 128),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));
			Add("Tile (11)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Tiles/Tile (11)", .1f, Color.White, pixelsPerUnit: 128),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));
			Add("Tile (12)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Tiles/Tile (12)", .1f, Color.White, pixelsPerUnit: 128),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));
			Add("Tile (13)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Tiles/Tile (13)", .1f, Color.White, pixelsPerUnit: 128),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));
			Add("Tile (14)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Tiles/Tile (14)", .1f, Color.White, pixelsPerUnit: 128),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));
			Add("Tile (15)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Tiles/Tile (15)", .1f, Color.White, pixelsPerUnit: 128),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));
			Add("Tile (16)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Tiles/Tile (16)", .1f, Color.White, pixelsPerUnit: 128),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));

			Add("Bones (1)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Decoration/Bones (1)", .1f, Color.White, pixelsPerUnit: 100),
			}, isStatic: true));
			Add("Bones (2)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Decoration/Bones (2)", .1f, Color.White, pixelsPerUnit: 100),
			}, isStatic: true));
			Add("Bones (3)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Decoration/Bones (3)", .1f, Color.White, pixelsPerUnit: 100),
			}, isStatic: true));
			Add("Bones (4)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Decoration/Bones (4)", .1f, Color.White, pixelsPerUnit: 100),
			}, isStatic: true));
			Add("ArrowSign", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Decoration/ArrowSign", .1f, Color.White, pixelsPerUnit: 100),
			}, isStatic: true));
			Add("Bush (1)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Decoration/Bush (1)", .1f, Color.White, pixelsPerUnit: 100),
			}, isStatic: true));
			Add("Bush (2)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Decoration/Bush (2)", .1f, Color.White, pixelsPerUnit: 100),
			}, isStatic: true));
			Add("DeadBush", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Decoration/DeadBush", .1f, Color.White, pixelsPerUnit: 100),
			}, isStatic: true));
			Add("Crate", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Decoration/Crate", .1f, Color.White, pixelsPerUnit: 100),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));
			Add("Sign", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Decoration/Sign", .1f, Color.White, pixelsPerUnit: 100),
			}, isStatic: true));
			Add("Skeleton", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Decoration/Skeleton", .1f, Color.White, pixelsPerUnit: 100),
			}, isStatic: true));
			Add("Tree", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Decoration/Tree", .1f, Color.White, pixelsPerUnit: 100),
			}, isStatic: true));
			Add("TombStone (1)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Decoration/TombStone (1)", .1f, Color.White, pixelsPerUnit: 100),
			}, isStatic: true));
			Add("TombStone (2)", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Decoration/TombStone (2)", .1f, Color.White, pixelsPerUnit: 100),
			}, isStatic: true));
			Add("Background", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Background", .01f, Color.White, pixelsPerUnit: 80),
				new SetCameraPosition()
			}, isStatic: true));

			Add("Player", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Square", .3f, Color.Green, pixelsPerUnit: 200),
				new RectangleCollider(Vector2.Zero, Vector2.One),
				new Player(),
				new Physics(gravity: -30),
			}, isStatic: false));

			Add("Coin", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Square", .2f, Color.Yellow, pixelsPerUnit: 400),
				new RectangleCollider(Vector2.Zero, Vector2.One/2, trigger: true),
				new Coin(),
			}, isStatic: false));

			Add("Arrow", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Arrow", .2f, Color.LightBlue, pixelsPerUnit: 100),
				new Arrow(),
			}, isStatic: false));
		}
	}
}
