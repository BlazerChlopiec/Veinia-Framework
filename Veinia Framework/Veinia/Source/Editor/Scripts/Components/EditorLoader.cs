using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Veinia.Editor
{
	public class EditorLoader : Component
	{
		EditorObjectPainter editorObjectPainter;

		private string editedLevel;


		public EditorLoader(string editedLevel) => this.editedLevel = editedLevel;

		public override void Initialize()
		{
			editorObjectPainter = FindComponentOfType<EditorObjectPainter>();

			Load();
		}

		public void Save()
		{
			string serializedText = JsonConvert.SerializeObject(editorObjectPainter.editorObjects);

			File.WriteAllText(editedLevel, serializedText);
		}

		public void Load()
		{
			editorObjectPainter.RemoveAll();

			if (!File.Exists(editedLevel)) return;
			var deserializedText = File.ReadAllText(editedLevel);
			var objects = JsonConvert.DeserializeObject<List<EditorObject>>(deserializedText);

			foreach (var item in objects)
			{
				editorObjectPainter.Spawn(item.PrefabName, item.Position);
			}
		}

		public override void Update()
		{
			if (Globals.input.GetKey(Keys.LeftControl) && Globals.input.GetKeyDown(Keys.S)) Save();
		}
	}
}
