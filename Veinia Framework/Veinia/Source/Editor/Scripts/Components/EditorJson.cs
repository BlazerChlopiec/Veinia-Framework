using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Veinia.Editor
{
	public class EditorJson : Component
	{
		EditorObjectManager objectManager;

		private string editedLevel;


		public EditorJson(string editedLevel)
		{
			this.editedLevel = editedLevel;
		}

		public void Save(List<EditorObject> objects)
		{
			string serializedText = JsonConvert.SerializeObject(objects);

			File.WriteAllText(editedLevel, serializedText);
		}
		public void Load()
		{
			objectManager.editorObjects.Clear();

			if (!File.Exists(editedLevel)) return;
			var deserializedText = File.ReadAllText(editedLevel);
			var objects = JsonConvert.DeserializeObject<List<EditorObject>>(deserializedText);

			foreach (var item in objects)
			{
				objectManager.Spawn(item.PrefabName, item.Position);
			}
		}

		public override void Initialize()
		{
			objectManager = GetComponent<EditorObjectManager>();

			Load();
		}

		public override void Update()
		{
			if (Globals.input.GetKey(Keys.LeftControl) && Globals.input.GetKeyDown(Keys.S))
			{
				Save(objectManager.editorObjects);
			}
		}
	}
}
