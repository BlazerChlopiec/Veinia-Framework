using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class PerformanceTestLevel : Level
{
	public PerformanceTestLevel(string name, PrefabManager prefab) : base(name, prefab)
	{
	}

	public override void LoadContents()
	{
		base.LoadContents();

		for (int i = 0; i < 100000; i++) // performance test
		{
			Instantiate(
				new Transform(-3, 2),
				new List<Component>
				{
					new Sprite(path: "Test/test1", layer: 0, color: Color.Brown),
					new BasicMovement(5),
					new WeirdLoop(),
					new KillOnClick(Microsoft.Xna.Framework.Input.Keys.Tab),
				}, isStatic: false);
		}

		Instantiate(
			new Transform(0, 0),
			new List<Component>
			{
				new Sprite(path: "Test/test1", layer:0, color: Color.Red),
				new MouseFollow()
			}, isStatic: false);
	}
}