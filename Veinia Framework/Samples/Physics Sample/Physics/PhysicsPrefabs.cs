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

			var prefab = new GameObject(new Transform { Z = 0 }, new List<Component>
			{
				new Sprite("Sprites/Square", Color.White, pixelsPerUnit: 200),
				new RealtimeRectangle(friction: 2, bodyType: BodyType.Dynamic)
			}, isStatic: false);

			Add("Block", prefab);
		}
	}
}