using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Veinia.Platformer
{
	public class Level1 : Level
	{
		public Level1(PrefabManager prefabManager) : base(prefabManager)
		{
		}

		public Level1(PrefabManager prefabManager, string levelPath) : base(prefabManager, levelPath)
		{
		}

		public override void CreateScene(bool loadObjectsFromPath = true)
		{
			base.CreateScene();

			Instantiate(Transform.Empty, new List<Component>
			{
				new UI(),
			}, isStatic: true);

			Instantiate(new Transform(1, 2), new List<Component>
			{
				new Sprite("Sprites/Arrow", .1f, Color.LightBlue, pixelsPerUnit: 100),
				new RotationTest(),
			}, isStatic: false);
		}
	}
}
