using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class SpawnOnMouse : Component
{
	public override void Initialize()
	{

	}

	public override void Update()
	{
		if (Globals.input.GetMouseButtonDown(1))
		{
			Instantiate(
				new Transform(Globals.input.GetMouseWorldPosition()),
				new List<Component>
				{
					new Sprite(path: "Test/test2", layer: 0, Color.Red),
					new RectanglePhysics(Vector2.Zero, Vector2.One)
				}, isStatic: false);
		}
		if (Globals.input.GetMouseButtonDown(0))
		{
			Instantiate(
				new Transform(Globals.input.GetMouseWorldPosition()),
				new List<Component>
				{
					new Sprite(path: "Test/test4", layer: 0, Color.Blue),
					new RectanglePhysics(Vector2.Zero, Vector2.One)
				}, isStatic: false);
		}
	}
}
