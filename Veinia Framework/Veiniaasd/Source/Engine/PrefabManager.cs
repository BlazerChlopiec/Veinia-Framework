using System.Collections.Generic;
using System.Linq;
using VeiniaFramework;

namespace VeiniaFramework
{
	public class PrefabManager
	{
		public List<Prefab> prefabs = new List<Prefab>();
		public List<Prefab> editorPrefabs => prefabs.Where(p => p.SerializeInEditor).ToList();


		protected void Add(string name, GameObject gameObject, bool serializeInEditor = true, int prefabTab = 0, bool showLabel = false)
		{
			Prefab prefabData = new Prefab
			{
				PrefabName = name,
				PrefabGameObject = gameObject,
				SerializeInEditor = serializeInEditor,
				PaintingToolbarTab = prefabTab,
				ShowLabel = showLabel
			};

			prefabs.Add(prefabData);
		}

		public GameObject Find(string name)
		{
			var prefab = prefabs.Find(x => x.PrefabName == name)?.PrefabGameObject;
			return prefab ?? throw new System.Exception("The Prefab you're looking for doesn't exist: " + name);
		}

		public virtual void LoadPrefabs() => prefabs.Clear();
	}
}

public class Prefab
{
	public string PrefabName { get; set; }
	public GameObject PrefabGameObject { get; set; }
	public bool SerializeInEditor { get; set; } = true; // editor prefab?
	public int PaintingToolbarTab { get; set; }
	public bool ShowLabel { get; set; } // show prefab name in editor painting
}