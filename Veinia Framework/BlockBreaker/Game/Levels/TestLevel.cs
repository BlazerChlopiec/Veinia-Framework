using Humper.Responses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;

public class TestLevel : Level
{
	public TestLevel(string name, Prefabs prefabs) : base(name, prefabs)
	{
	}

	public override void LoadContents()
	{
		base.LoadContents();

		Instantiate(
			new Transform(0, 0),
			new List<Component>
			{
				new Sprite(path: "Test/veina background", layer: 0, Color.White),
			}, isStatic: false);

		Instantiate(
			new Transform(0, 0),
			new List<Component>
			{
				new Sprite(path: "Test/test3", layer: 0, Color.White, destinationSize: new Vector2(1, 1)),
				new MouseFollow(),
				new SpawnOnMouse(),
				new CustomCollider(CollisionResponses.None, Vector2.Zero, Vector2.One*.5f),
				new HitboxOutline()
			}, isStatic: false);


		Instantiate(
			new Transform(-2, 1),
			new List<Component>
			{
				new Sprite(path: "Test/test6", layer: 0, Color.White, destinationSize: new Vector2(1, 1)),
				new TweenSprite(),
				new SpriteCollider(CollisionResponses.None),
				new HitboxOutline(),
			}, isStatic: false);


		GameObject player = Instantiate(new Transform(0, 2), prefabs.Find("player"));
		player.AddComponent(new Button());

		for (int i = 0; i < 10000; i++)
		{
			Instantiate(new Transform(i, 1), prefabs.Find("killableBlock"));
		}

		Instantiate(new Transform(-4, -2), prefabs.Find("block"));
		Instantiate(new Transform(-3, -2), prefabs.Find("block"));
		Instantiate(new Transform(-2, -2), prefabs.Find("block"));
		Instantiate(new Transform(-1, -2), prefabs.Find("block"));
		Instantiate(new Transform(0, -2), prefabs.Find("block"));
		Instantiate(new Transform(1, -2), prefabs.Find("block"));
		Instantiate(new Transform(2, -2), prefabs.Find("block"));
		Instantiate(new Transform(3, -2), prefabs.Find("block"));

		GameObject chunkManager = Instantiate(
			new Transform(0, 0),
			new List<Component>
			{
				new Chunks()
			}, isStatic: false);

		chunkManager.GetComponent<Chunks>().SetTarget(player.transform);
	}
}