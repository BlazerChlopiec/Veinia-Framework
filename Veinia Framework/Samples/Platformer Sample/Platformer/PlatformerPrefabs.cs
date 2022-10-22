using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Veinia.Platformer
{
	public class PlatformerPrefabs : PrefabManager
	{
		public override void LoadPrefabs()
		{
			base.LoadPrefabs();

			Add("Player", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Square", .3f, Color.Green, pixelsPerUnit: 200),
				new RectangleCollider(Vector2.Zero, Vector2.One),
				new Player(),
				new Physics(gravity: -30),
			}, isStatic: false), prefabTab: 0);

			Add("Coin", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Square", .2f, Color.Yellow, pixelsPerUnit: 400),
				new RectangleCollider(Vector2.Zero, Vector2.One/2, trigger: true),
				new Coin(),
			}, isStatic: false), prefabTab: 0);

			Add("Arrow", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Arrow", .2f, Color.LightBlue, pixelsPerUnit: 100),
				new Arrow(),
			}, isStatic: false), prefabTab: 0);

			var tiles = Globals.content.LoadAll<Texture2D>("Sprites/Tiles");
			foreach (var tile in tiles)
			{
				Add(tile.Key, new GameObject(Transform.Empty, new List<Component>
				{
					new Sprite(tile.Value, .1f, Color.White, pixelsPerUnit: 128),
					new RectangleCollider(Vector2.Zero, Vector2.One),
				}, isStatic: true), prefabTab: 1);
			}

			var decoration = Globals.content.LoadAll<Texture2D>("Sprites/Decoration");
			foreach (var deco in decoration)
			{
				Add(deco.Key, new GameObject(Transform.Empty, new List<Component>
				{
					new Sprite(deco.Value, .2f, Color.White, pixelsPerUnit: 100),
				}, isStatic: true), prefabTab: 2);
			}

			Add("Background", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Background", .01f, Color.White, pixelsPerUnit: 80),
				new SetCameraPosition()
			}, isStatic: true), prefabTab: 2);
		}
	}
}
