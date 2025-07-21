using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using VeiniaFramework.Editor;

namespace VeiniaFramework.Editor
{
	public class EditorJSON : Component
	{
		EditorObjectManager editorObjectManager;

		private string editedLevelName;

		public static bool encryptScene = true;

		public SceneFile sceneFile;


		public EditorJSON(string editedLevelName) => this.editedLevelName = editedLevelName;

		public override void Initialize()
		{
			EditorCheckboxes.Add("Encrypt Scene", encryptScene, (e, o) => { encryptScene = true; }, (e, o) => { encryptScene = false; });

			editorObjectManager = FindComponentOfType<EditorObjectManager>();

			sceneFile = new SceneFile();

			Load();
		}

		public void Save()
		{
			sceneFile.objects = editorObjectManager.editorObjects;

			object dataToSave;

			dataToSave = encryptScene ? Encryption.Encrypt(JsonConvert.SerializeObject(sceneFile)) : JsonConvert.SerializeObject(sceneFile);

			if (editedLevelName == null || editedLevelName == string.Empty)
			{
				EditorScene.ErrorWindow("Warning", "The edited level has no name therefore we dont know how to save it!");
				return;
			}

			//game directory
			if (!Directory.Exists("LevelData")) Directory.CreateDirectory("LevelData");

			if (encryptScene) File.WriteAllBytes("LevelData/" + editedLevelName, (byte[])dataToSave);
			else File.WriteAllText("LevelData/" + editedLevelName, (string)dataToSave);
			//

			//game project directory
			var currentGameDirectory = Environment.CurrentDirectory;
			var projectDirectory = Directory.GetParent(currentGameDirectory).Parent.Parent.FullName;
			if (!Directory.Exists(projectDirectory + "/LevelData")) Directory.CreateDirectory(projectDirectory + "/LevelData");

			if (encryptScene) File.WriteAllBytes(projectDirectory + "/LevelData/" + editedLevelName, (byte[])dataToSave);
			else File.WriteAllText(projectDirectory + "/LevelData/" + editedLevelName, (string)dataToSave);
			//
		}

		public void Load()
		{
			editorObjectManager.RemoveAll();

			if (!File.Exists("LevelData/" + editedLevelName)) return;
			var dataToLoad = EditorJSON.encryptScene ? Encryption.Decrypt(File.ReadAllBytes("LevelData/" + editedLevelName))
							: File.ReadAllText("LevelData/" + editedLevelName);

			sceneFile = JsonConvert.DeserializeObject<SceneFile>(dataToLoad);

			foreach (var item in sceneFile.objects)
			{
				editorObjectManager.Spawn(item.PrefabName, new Transform { position = item.Position, rotation = item.Rotation }, item.customData);
			}
		}

		public override void Update()
		{
			if (Globals.input.GetKey(Keys.LeftControl) && Globals.input.GetKeyDown(Keys.S)) Save();
		}
	}
}


public class SceneFile
{
	public List<EditorObject> objects;
}