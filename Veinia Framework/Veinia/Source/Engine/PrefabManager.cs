using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Veinia
{
	public class PrefabManager
	{
		public List<Prefab> prefabs = new List<Prefab>();


		protected void Add(string name, GameObject gameObject)
		{
			Prefab prefabData = new Prefab
			{
				prefabName = name,
				prefabGameObject = gameObject,
			};

			prefabs.Add(prefabData);
		}

		public GameObject Find(string name) => prefabs.Find(x => x.prefabName == name).prefabGameObject;

		public virtual void LoadPrefabs(WorldTools tools) => prefabs.Clear();

		public struct Prefab
		{
			public string prefabName;
			public GameObject prefabGameObject;
		}
	}
}