using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace VeiniaFramework.Platformer
{
	public class PlatformerPrefabs : PrefabManager
	{
		public override void LoadPrefabs()
		{
			base.LoadPrefabs();

			Add("Player", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Player", .5f, Color.GreenYellow, pixelsPerUnit: 16),
				new RectangleCollider(Vector2.Zero, Vector2.One),
				new Player(),
				new Physics(gravity: -30),
			}, isStatic: false), prefabTab: 0);


			var collectibles = Globals.content.LoadAll<Texture2D>("Sprites/Collectibles");
			foreach (var collectible in collectibles)
			{
				Add(collectible.Key, new GameObject(Transform.Empty, new List<Component>
				{
					new Sprite(collectible.Value, .3f, Color.White, pixelsPerUnit: 16),
					new RectangleCollider(Vector2.Zero, Vector2.One/2, trigger: true),
					new Collectible()
				}, isStatic: false), prefabTab: 1);
			}

			var grassTiles = Globals.content.LoadAll<Texture2D>("Sprites/Grass");
			foreach (var grassTile in grassTiles)
			{
				Add(grassTile.Key, new GameObject(Transform.Empty, new List<Component>
				{
					new Sprite(grassTile.Value, .6f, Color.White, pixelsPerUnit: 16),
					new RectangleCollider(Vector2.Zero, Vector2.One),
				}, isStatic: true), prefabTab: 2);
			}

			var woodTiles = Globals.content.LoadAll<Texture2D>("Sprites/Wood");
			foreach (var woodTile in woodTiles)
			{
				Add(woodTile.Key, new GameObject(Transform.Empty, new List<Component>
				{
					new Sprite(woodTile.Value, .2f, Color.White, pixelsPerUnit: 16),
					new RectangleCollider(Vector2.Zero, Vector2.One),
				}, isStatic: true), prefabTab: 2);
			}

			var bricks = Globals.content.LoadAll<Texture2D>("Sprites/Bricks");
			foreach (var brick in bricks)
			{
				Add(brick.Key, new GameObject(Transform.Empty, new List<Component>
				{
					new Sprite(brick.Value, .1f, Color.White, pixelsPerUnit: 16),
					new RectangleCollider(Vector2.Zero, Vector2.One),
				}, isStatic: true), prefabTab: 3);
			}

			var decoration = Globals.content.LoadAll<Texture2D>("Sprites/Decoration");
			foreach (var deco in decoration)
			{
				Add(deco.Key, new GameObject(Transform.Empty, new List<Component>
				{
					new Sprite(deco.Value, .6f, Color.White, pixelsPerUnit: 16),
				}, isStatic: true), prefabTab: 4);
			}
		}
	}
}
