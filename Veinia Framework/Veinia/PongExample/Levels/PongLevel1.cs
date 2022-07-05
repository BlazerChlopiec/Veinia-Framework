using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class PongLevel1 : Level
{
	public PongLevel1(string name, PrefabManager prefabManager) : base(name, prefabManager)
	{
	}

	public override void LoadContents()
	{
		base.LoadContents();

		GameObject background = Instantiate(
			new Transform(Vector2.Zero),
			new List<Component>
			{
				new Sprite("Pong/Background", 0, Color.White, new Vector2(16*1.2f,9*1.2f)),
			}, isStatic: true);

		GameObject leftBorder = Instantiate(
			new Transform(-Vector2.UnitX * 9.7f),
			new List<Component>
			{
				new RectanglePhysics(Vector2.Zero, new Vector2(.3f, 10), trigger: true)
			}, isStatic: false);
		GameObject rightBorder = Instantiate(
			new Transform(Vector2.UnitX * 9.7f),
			new List<Component>
			{
				new RectanglePhysics(Vector2.Zero, new Vector2(.3f, 10), trigger: true)
			}, isStatic: false);
		GameObject topBorder = Instantiate(
			new Transform(Vector2.UnitY * 5.5f),
			new List<Component>
			{
				new RectanglePhysics(Vector2.Zero, new Vector2(18, .3f), trigger: true)
			}, isStatic: false);



		//LEVEL
		GameObject paddle = Instantiate(
			new Transform(Vector2.Zero),
			new List<Component>
			{
				new Sprite("Pong/Paddle", 0, Color.Blue),
				new Paddle(),
				new RectanglePhysics(Vector2.Zero, new Vector2(2.5f, .85f), trigger: true)
			}, isStatic: false);

		GameObject ball = Instantiate(
			new Transform(Vector2.Zero),
			new List<Component>
			{
				new Sprite("Pong/Square", 0, Color.Blue, new Vector2(.8f, .8f)),
				new Ball(),
				new CirclePhysics(Vector2.Zero, .4f)
			}, isStatic: false);

		for (int i = 0; i < 11; i++)
		{
			Instantiate(new Transform(i - 5, 0), prefabManager.Find("Block"));
		}

		//
	}
}