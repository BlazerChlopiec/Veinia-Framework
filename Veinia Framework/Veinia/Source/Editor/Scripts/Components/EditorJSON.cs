﻿using Microsoft.Xna.Framework.Input;
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

		public SceneFile sceneFile;


		public EditorJSON(string editedLevelName) => this.editedLevelName = editedLevelName;

		public override void Initialize()
		{
			editorObjectManager = FindComponentOfType<EditorObjectManager>();

			sceneFile = new SceneFile();

			Load();
		}

		public void Save()
		{
			sceneFile.objects = editorObjectManager.editorObjects;

			string serializedText = JsonConvert.SerializeObject(sceneFile);
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
			sceneFile = JsonConvert.DeserializeObject<SceneFile>(deserializedText);

			foreach (var item in sceneFile.objects)
			{
				editorObjectManager.Spawn(item.PrefabName, new Transform { position = item.Position, rotation = item.Rotation });
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