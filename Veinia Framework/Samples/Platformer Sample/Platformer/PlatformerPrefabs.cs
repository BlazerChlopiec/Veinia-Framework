using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Veinia.Platformer
{
	public class PlatformerPrefabs : PrefabManager
	{
		public override void LoadPrefabs()
		{
			base.LoadPrefabs();

			Add("Block", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Square", .1f, Color.White, pixelsPerUnit: 200),
				new RectangleCollider(Vector2.Zero, Vector2.One),
			}, isStatic: true));

			Add("Player", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Square", .1f, Color.Green, pixelsPerUnit: 200),
				new RectangleCollider(Vector2.Zero, Vector2.One),
				new Player(),
				new Physics(gravity: -30),
			}, isStatic: false));

			Add("Physics Block", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Square", .1f, Color.LightBlue, pixelsPerUnit: 200),
				new RectangleCollider(Vector2.Zero, Vector2.One),
				new Physics(gravity: -15),
			}, isStatic: false));

			Add("Coin", new GameObject(Transform.Empty, new List<Component>
			{
				new Sprite("Sprites/Square", .1f, Color.Yellow, pixelsPerUnit: 400),
				new RectangleCollider(Vector2.Zero, Vector2.One/2, trigger: true),
				new Coin(),
			}, isStatic: false));
		}
	}
}
