using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class PrefabManager
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

	public void LoadPrefabs(WorldTools tools)
	{
		prefabs.Clear();

		Add("block", new GameObject(Transform.Empty, new List<Component>
		{
			new Sprite(path: "Test/test7", layer: 0, color: Color.White, new Vector2(1, 1)),
			new RectanglePhysics(Vector2.Zero, Vector2.One, trigger: false)
		}, tools, isStatic: true));

		Add("killableBlock", new GameObject(Transform.Empty, new List<Component>
		{
			new Sprite(path: "Test/veina tile", layer: 0, color: Color.White, new Vector2(1, 1)),
			new RectanglePhysics(Vector2.Zero, Vector2.One, trigger: false),
			new KillOnClick(Microsoft.Xna.Framework.Input.Keys.Tab),
		}, tools, isStatic: true));

		Add("player", new GameObject(Transform.Empty, new List<Component>
		{
			new Sprite(path: "Test/test1", layer: 0, Color.White),
			new SimplePlatformer(5, 15),
			new RectanglePhysics(Vector2.Zero, Vector2.One),
			new CirclePhysics(-Vector2.UnitY/1.8f, .05f, true)
		}, tools, isStatic: false));
	}

	public struct Prefab
	{
		public string prefabName;
		public GameObject prefabGameObject;
	}
}