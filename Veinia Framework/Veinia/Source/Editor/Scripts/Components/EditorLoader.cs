using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Veinia.Editor
{
	public class EditorLoader : Component
	{
		EditorObjectManager editorObjectManager;

		private string editedLevel;


		public EditorLoader(string editedLevel) => this.editedLevel = editedLevel;

		public override void Initialize()
		{
			editorObjectManager = FindComponentOfType<EditorObjectManager>();

			Load();
		}

		public void Save()
		{
			string serializedText = JsonConvert.SerializeObject(editorObjectManager.editorObjects);

			if (!Directory.Exists("Levels")) Directory.CreateDirectory("Levels");
			File.WriteAllText("Levels/" + editedLevel, serializedText);
		}

		public void Load()
		{
			editorObjectManager.RemoveAll();

			if (!File.Exists("Levels/" + editedLevel)) return;
			var deserializedText = File.ReadAllText("Levels/" + editedLevel);
			var objects = JsonConvert.DeserializeObject<List<EditorObject>>(deserializedText);

			foreach (var item in objects)
			{
				editorObjectManager.Spawn(item.PrefabName, item.Position);
			}
		}

		public override void Update()
		{
			if (Globals.input.GetKey(Keys.LeftControl) && Globals.input.GetKeyDown(Keys.S)) Save();
		}
	}
}
