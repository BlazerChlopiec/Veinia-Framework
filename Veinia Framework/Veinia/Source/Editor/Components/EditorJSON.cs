using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.IO;

namespace VeiniaFramework.Editor
{
	public class EditorJSON : Component
	{
		EditorObjectManager editorObjectManager;

		private string editedLevelName;

		public static bool encryptScene = true;

		public static string LevelsFolder = "LevelData";

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
			sceneFile.editorCamPosition = Globals.camera.GetPosition();
			sceneFile.editorCamScale = Globals.camera.Scale;

			object dataToSave;

			dataToSave = encryptScene ? Encryption.Encrypt(JsonConvert.SerializeObject(sceneFile)) : JsonConvert.SerializeObject(sceneFile);

			if (editedLevelName == null || editedLevelName == string.Empty)
			{
				EditorScene.ErrorWindow("Warning", "The edited level has no name therefore we dont know how to save it! Add a name in the level constructor!");
				return;
			}

			// game directory
			if (!Directory.Exists(LevelsFolder)) Directory.CreateDirectory(LevelsFolder);

			var gameWritePath = Path.Combine(LevelsFolder, editedLevelName);

			if (encryptScene) File.WriteAllBytes(gameWritePath, (byte[])dataToSave);
			else File.WriteAllText(gameWritePath, (string)dataToSave);
			//

			// project directory
			var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
			var projectLevelFolder = Path.Combine(projectDirectory, LevelsFolder);
			if (!Directory.Exists(projectLevelFolder)) Directory.CreateDirectory(projectLevelFolder);

			var projectWritePath = Path.Combine(projectLevelFolder, editedLevelName);

			if (encryptScene) File.WriteAllBytes(projectWritePath, (byte[])dataToSave);
			else File.WriteAllText(projectWritePath, (string)dataToSave);
			//
		}

		public void Load()
		{
			editorObjectManager.RemoveAll();

			var loadDir = Path.Combine(LevelsFolder, editedLevelName);

			if (!File.Exists(loadDir)) return;
			var dataToLoad = encryptScene ? Encryption.Decrypt(File.ReadAllBytes(loadDir))
							: File.ReadAllText(loadDir);

			sceneFile = JsonConvert.DeserializeObject<SceneFile>(dataToLoad);

			foreach (var item in sceneFile.objects)
			{
				editorObjectManager.Spawn(item);
			}

			Globals.camera.SetPosition(sceneFile.editorCamPosition ?? Vector2.Zero);
			Globals.camera.Scale = sceneFile.editorCamScale ?? 1;
		}

		public override void Update()
		{
			if (Globals.input.GetKey(Keys.LeftControl) && Globals.input.GetKeyDown(Keys.S)) Save();
		}
	}
}