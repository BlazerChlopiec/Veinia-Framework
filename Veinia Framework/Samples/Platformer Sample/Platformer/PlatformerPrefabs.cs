using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using tainicom.Aether.Physics2D.Dynamics;

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
				new Player(),
			}, isStatic: false), prefabTab: 0);


			var collectibles = Globals.content.LoadAll<Texture2D>("Sprites/Collectibles");
			foreach (var collectible in collectibles)
			{
				var prefab = new GameObject(Transform.Empty, new List<Component>
				{
					new Sprite(collectible.Value, .3f, Color.White, pixelsPerUnit: 16),
					new Collectible()
				}, isStatic: false);
				prefab.body = Globals.physicsWorld.CreateRectangle(.5f, .5f, 1, bodyType: BodyType.Static);
				prefab.body.SetIsSensor(true);
				prefab.body.Tag = "ground";

				Add(collectible.Key, prefab, prefabTab: 1);
			}

			var grassTiles = Globals.content.LoadAll<Texture2D>("Sprites/Grass");
			foreach (var grassTile in grassTiles)
			{
				var prefab = new GameObject(Transform.Empty, new List<Component>
				{
					new Sprite(grassTile.Value, .6f, Color.White, pixelsPerUnit: 16),
				}, isStatic: true);
				prefab.body = Globals.physicsWorld.CreateRectangle(1, 1, 1, bodyType: BodyType.Static);
				prefab.body.Tag = "ground";

				Add(grassTile.Key, prefab, prefabTab: 2);
			}

			var woodTiles = Globals.content.LoadAll<Texture2D>("Sprites/Wood");
			foreach (var woodTile in woodTiles)
			{
				var prefab = new GameObject(Transform.Empty, new List<Component>
				{
					new Sprite(woodTile.Value, .2f, Color.White, pixelsPerUnit: 16),
				}, isStatic: true);
				prefab.body = Globals.physicsWorld.CreateRectangle(1, 1, 1, bodyType: BodyType.Static);
				prefab.body.Tag = "ground";

				Add(woodTile.Key, prefab, prefabTab: 2);
			}

			var bricks = Globals.content.LoadAll<Texture2D>("Sprites/Bricks");
			foreach (var brick in bricks)
			{
				var prefab = new GameObject(Transform.Empty, new List<Component>
				{
					new Sprite(brick.Value, .1f, Color.White, pixelsPerUnit: 16),
				}, isStatic: true);
				prefab.body = Globals.physicsWorld.CreateRectangle(1, 1, 1, bodyType: BodyType.Static);
				prefab.body.Tag = "ground";

				Add(brick.Key, prefab, prefabTab: 3);
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
