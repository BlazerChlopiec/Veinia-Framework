using Humper.Responses;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class Prefabs
{
	public List<Prefab> prefabs = new List<Prefab>();

	private void Add(string name, GameObject gameObject)
	{
		Prefab prefabData = new Prefab
		{
			prefabName = name,
			prefabGameObject = gameObject,
		};

		prefabs.Add(prefabData);
	}

	public GameObject Find(string name) => prefabs.Find(x => x.prefabName == name).prefabGameObject;

	public void AddDefaultPrefabs(WorldTools tools)
	{
		prefabs.Clear();

		Add("block", new GameObject(Transform.Empty, new List<Component>
		{
			new Sprite(path: "Test/test7", layer: 0, color: Color.White, new Vector2(1, 1)),
			new SpriteCollider(CollisionResponses.None),
			new HitboxOutline(),
		}, tools, isStatic: true));

		Add("killableBlock", new GameObject(Transform.Empty, new List<Component>
		{
			new Sprite(path: "Test/veina tile", layer: 0, color: Color.White, new Vector2(1, 1)),
			new SpriteCollider(CollisionResponses.None),
			new HitboxOutline(),
			new KillOnClick(Microsoft.Xna.Framework.Input.Keys.Tab),
		}, tools, isStatic: true));

		Add("player", new GameObject(Transform.Empty, new List<Component>
		{
			new Sprite(path: "Test/test1", layer: 0, Color.White),
			new Physics(5),
			new SpriteCollider(CollisionResponses.Slide),
			new SimplePlatformer(5, 15),
			new HitboxOutline(),
		}, tools, isStatic: true));
	}

	public class Prefab
	{
		public string prefabName;
		public GameObject prefabGameObject;
	}
}