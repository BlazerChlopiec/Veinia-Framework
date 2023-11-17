using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace VeiniaFramework.Editor
{
	public class EditorJSON : Component
	{
		EditorObjectManager editorObjectManager;

		private string editedLevelName;


		public EditorJSON(string editedLevelName) => this.editedLevelName = editedLevelName;

		public override void Initialize()
		{
			editorObjectManager = FindComponentOfType<EditorObjectManager>();

			Load();
		}

		public void Save()
		{
			string serializedText = JsonConvert.SerializeObject(editorObjectManager.editorObjects);
			serializedText = Encryption.Encrypt(serializedText);

			if (editedLevelName == null || editedLevelName == string.Empty)
			{
				EditorScene.ErrorWindow("Warning", "The edited level has no name therefore we dont know how to save it!");
				return;
			}

			//game directory
			if (!Directory.Exists("LevelData")) Directory.CreateDirectory("LevelData");
			File.WriteAllText("LevelData/" + editedLevelName, serializedText);
			//

			//game project directory
			var currentGameDirectory = Environment.CurrentDirectory;
			var projectDirectory = Directory.GetParent(currentGameDirectory).Parent.Parent.FullName;
			if (!Directory.Exists(projectDirectory + "/LevelData")) Directory.CreateDirectory(projectDirectory + "/LevelData");
			File.WriteAllText(projectDirectory + "/LevelData/" + editedLevelName, serializedText);
			//
		}

		public void Load()
		{
			editorObjectManager.RemoveAll();

			if (!File.Exists("LevelData/" + editedLevelName)) return;
			var deserializedText = File.ReadAllText("LevelData/" + editedLevelName);
			deserializedText = Encryption.Decrypt(deserializedText);
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
