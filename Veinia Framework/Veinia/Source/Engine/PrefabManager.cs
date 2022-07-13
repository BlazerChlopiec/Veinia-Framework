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

		public GameObject Find(string name)
		{
			var prefab = prefabs.Find(x => x.prefabName == name).prefabGameObject;
			if (prefab == null) throw new System.Exception("The Prefab you're looking for doesn't exist: " + name);
			return prefab;
		}

		public virtual void LoadPrefabs(WorldTools tools) => prefabs.Clear();

		public struct Prefab
		{
			public string prefabName;
			public GameObject prefabGameObject;
		}
	}
}