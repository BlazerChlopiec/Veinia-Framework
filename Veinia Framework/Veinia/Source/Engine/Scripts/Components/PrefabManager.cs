using System.Collections.Generic;

namespace Veinia
{
	public class PrefabManager
	{
		public List<Prefab> prefabs = new List<Prefab>();


		protected void Add(string name, GameObject gameObject, int prefabTab = 0)
		{
			Prefab prefabData = new Prefab
			{
				PrefabName = name,
				PrefabGameObject = gameObject,
				PaintingToolbarTab = prefabTab
			};

			prefabs.Add(prefabData);
		}

		public GameObject Find(string name)
		{
			var prefab = prefabs.Find(x => x.PrefabName == name).PrefabGameObject;
			if (prefab == null) throw new System.Exception("The Prefab you're looking for doesn't exist: " + name);
			return prefab;
		}

		public virtual void LoadPrefabs() => prefabs.Clear();

		public class Prefab
		{
			public string PrefabName { get; set; }
			public GameObject PrefabGameObject { get; set; }
			public int PaintingToolbarTab { get; set; }
		}
	}
}