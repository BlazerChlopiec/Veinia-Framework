using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Properties;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Veinia.Editor
{
	public class EditorJson : Component
	{
		EditorObjectManager editorObjectManager;

		private string editedLevel;


		public EditorJson(string editedLevel) => this.editedLevel = editedLevel;

		public void Save()
		{
			string serializedText = JsonConvert.SerializeObject(editorObjectManager.editorObjects);

			File.WriteAllText(editedLevel, serializedText);
		}

		public void Load()
		{
			editorObjectManager.RemoveAll();

			if (!File.Exists(editedLevel)) return;
			var deserializedText = File.ReadAllText(editedLevel);
			var objects = JsonConvert.DeserializeObject<List<EditorObject>>(deserializedText);

			foreach (var item in objects)
			{
				editorObjectManager.Spawn(item.PrefabName, item.Position);
			}
		}

		public override void Initialize()
		{
			editorObjectManager = GetComponent<EditorObjectManager>();

			var panel = new Panel();
			var window = new Window
			{
				Title = "Editor Manager",
				Content = panel,
			};

			var saveButton = new TextButton { Text = "Save", HorizontalAlignment = HorizontalAlignment.Left };
			saveButton.Click += (s, e) => Save();
			panel.Widgets.Add(saveButton);

			var loadButton = new TextButton { Text = "Load", Left = 50, HorizontalAlignment = HorizontalAlignment.Left };
			loadButton.Click += (s, e) => Load();
			panel.Widgets.Add(loadButton);

			window.Show(Globals.desktop);

			Load();
		}

		public override void Update()
		{
			if (Globals.input.GetKey(Keys.LeftControl) && Globals.input.GetKeyDown(Keys.S)) Save();
		}
	}
}
