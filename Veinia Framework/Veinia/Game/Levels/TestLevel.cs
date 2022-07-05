using Humper.Responses;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Collections.Generic;

public class TestLevel : Level
{
	public TestLevel(string name, PrefabManager prefabManager) : base(name, prefabManager)
	{
	}

	public override void LoadContents()
	{
		base.LoadContents();

		Instantiate(
			new Transform(0, 0),
			new List<Component>
			{
				new Sprite(path: "Test/veina background", layer: 0, Color.Black),
			}, isStatic: false);

		Instantiate(
			new Transform(-1, 1),
			new List<Component>
			{
				new Sprite(path: "Test/test6", layer: 0, Color.White, destinationSize: new Vector2(1, 1)),
				new BasicMovement(5),
				//new RectanglePhysics(Vector2.Zero, Vector2.One, trigger:true),
				new CirclePhysics(Vector2.Zero, .5f)
			}, isStatic: false);
		Instantiate(
			new Transform(-3, 1),
			new List<Component>
			{
				new Sprite(path: "Test/test6", layer: 0, Color.White, destinationSize: new Vector2(1, 1)),
				new CirclePhysics(Vector2.Zero, .5f, trigger:true),
			}, isStatic: false);

		Instantiate(
			new Transform(0, 0),
			new List<Component>
			{
				new Sprite(path: "Test/test3", layer: 0, Color.White, destinationSize: new Vector2(1, 1)),
				new MouseFollow(),
				new SpawnOnMouse(),
				//new CirclePhysics(Vector2.Zero, .5f, trigger: false),
				new RectanglePhysics(Vector2.Zero, Vector2.One),
				//new NewPhysics(new CircleF(Point2.Zero, 100), Vector2.Zero),
				//new CustomCollider(CollisionResponses.Slide, Vector2.Zero, Vector2.One*.5f),
				//new HitboxOutline()
			}, isStatic: false);


		//Instantiate(
		//	new Transform(-2, 1),
		//	new List<Component>
		//	{
		//		new Sprite(path: "Test/test6", layer: 0, Color.White, destinationSize: new Vector2(1, 1)),
		//		new TweenSprite(),
		//	}, isStatic: false);

		//Instantiate(
		//	new Transform(-7, 3),
		//	new List<Component>
		//	{
		//		new Sprite(path: "Test/test1", layer: 0, Color.White, destinationSize: new Vector2(1, 1)),
		//		//new NewPhysics(new RectangleF(new Vector2(50, 50), new Vector2(100, 100)), Vector2.One),
		//		new CirclePhysics(Vector2.Zero, 1, trigger: false),

		//	}, isStatic: false);
		//Instantiate(
		//	new Transform(-2.5f, 7),
		//	new List<Component>
		//	{
		//		new Sprite(path: "Test/test1", layer: 0, Color.White, destinationSize: new Vector2(1, 1)),
		//		//new NewPhysics(new RectangleF(new Vector2(50, 50), new Vector2(100, 100)), -Vector2.One),
		//		new CirclePhysics(Vector2.Zero, 1, trigger: false),

		//	}, isStatic: false);

		GameObject testPrefab = new GameObject(
				new Transform(10, -2),
				new List<Component>
				{
				new Sprite(path: "Test/test1", layer: 0, Color.White, destinationSize: new Vector2(1, 1)),
				new RectanglePhysics(Vector2.Zero, Vector2.One, true)

				}, this, isStatic: true);
		for (int i = 0; i < 2; i++)
		{
			Instantiate(new Transform(i - 5, -2), testPrefab);
		}




		//GameObject player = Instantiate(new Transform(0, 2), prefabManager.Find("player"));
		//player.AddComponent(new Button());

		//for (int i = 0; i < 10000; i++)
		//{
		//	Instantiate(new Transform(i, 1), prefabManager.Find("killableBlock"));
		//}

		//Instantiate(new Transform(-4, -1), prefabManager.Find("block"));
		//Instantiate(new Transform(-3, -1), prefabManager.Find("block"));
		//Instantiate(new Transform(-2, -1), prefabManager.Find("block"));
		//Instantiate(new Transform(-1, -1), prefabManager.Find("block"));
		//Instantiate(new Transform(0, -1), prefabManager.Find("block"));
		//Instantiate(new Transform(1, -1), prefabManager.Find("block"));
		//Instantiate(new Transform(2, -1), prefabManager.Find("block"));
		//Instantiate(new Transform(3, -1), prefabManager.Find("block"));

		//GameObject chunkManager = Instantiate(
		//	new Transform(0, 0),
		//	new List<Component>
		//	{
		//		new Chunks()
		//	}, isStatic: false);

		//chunkManager.GetComponent<Chunks>().SetTarget(player.transform);
	}
}