using Microsoft.Xna.Framework;
using System.Collections.Generic;
using tainicom.Aether.Physics2D.Dynamics;

namespace VeiniaFramework.Samples.Physics
{
	public class PhysicsPrefabs : PrefabManager
	{
		public override void LoadPrefabs()
		{
			base.LoadPrefabs();

			var prefab = new GameObject(new Transform(Vector2.Zero), new List<Component>
			{
				new Sprite("Sprites/Square", 0, Color.White, pixelsPerUnit: 200),
			}, isStatic: false);
			prefab.body = Globals.physicsWorld.CreateRectangle(1, 1, 1, bodyType: BodyType.Dynamic);
			prefab.body.FixtureList[0].Friction = 2;
			prefab.body.Enabled = false;

			Add("Block", prefab);
		}
	}
}