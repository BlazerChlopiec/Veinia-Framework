using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Veinia.Editor
{
	public class EditorJson : Component
	{
		PlacingObjects placingObjects;

		private string editedLevel = "level1.veinia";


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
			placingObjects.objects.Clear();

			if (!File.Exists(editedLevel)) return;
			var deserializedText = File.ReadAllText(editedLevel);
			var objects = JsonConvert.DeserializeObject<List<EditorObject>>(deserializedText);

			foreach (var item in objects)
			{
				placingObjects.PlaceObject(item.PrefabName, new Transform(item.Position));
			}
		}

		public override void Initialize()
		{
			placingObjects = GetComponent<PlacingObjects>();

			Load();
		}

		public override void Update()
		{
			if (Globals.input.GetKey(Keys.LeftControl) && Globals.input.GetKeyDown(Keys.S))
			{
				Save(placingObjects.objects);
			}
		}
	}
}
