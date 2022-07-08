using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Veinia.BlockBreaker;

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

		Add("Block", new GameObject(Transform.Empty, new List<Component>
		{
			new Sprite("Pong/Grass Tile", 0, Color.White, Vector2.One),
			new RectanglePhysics(Vector2.Zero, Vector2.One),
			new Block(),
		}, tools, isStatic: true));

	}

	public struct Prefab
	{
		public string prefabName;
		public GameObject prefabGameObject;
	}
}