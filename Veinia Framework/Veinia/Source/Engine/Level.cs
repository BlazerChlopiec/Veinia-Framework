using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Veinia.Editor;

namespace Veinia
{
	public class Level : WorldTools
	{
		protected PrefabManager prefabManager;

		public string editorLevelName;


		public Level(PrefabManager prefabManager)
		{
			this.prefabManager = prefabManager;
		}
		public Level(PrefabManager prefabManager, string editorLevelName)
		{
			this.prefabManager = prefabManager;
			this.editorLevelName = editorLevelName;
		}

		public virtual void LoadContents()
		{
			Globals.camera.SetPosition(Vector2.Zero);
			Globals.camera.Zoom = 1;

			Globals.desktop.Root = null;
			prefabManager.LoadPrefabs(tools: this);
			LoadEditorObjects(editorLevelName);
		}

		private void LoadEditorObjects(string editorLevelName)
		{
			if (!File.Exists(editorLevelName)) return;
			var deserializedText = File.ReadAllText(editorLevelName);
			var objects = JsonConvert.DeserializeObject<List<EditorObject>>(deserializedText);

			foreach (var item in objects)
			{
				if (prefabManager.Find(item.PrefabName) == null)
				{
					throw new System.Exception("Prefabs that don't got deleted and are still on " + editorLevelName);
				}
				Instantiate(new Transform(item.Position), prefabManager.Find(item.PrefabName));
			}
		}
	}
}