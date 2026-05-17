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

		public static bool UseEncryption = true;
		public static bool RunningOnWeb;

		public static string LevelsFolder = "LevelData";

		public SceneFile sceneFile;


		public EditorJSON(string editedLevelName) => this.editedLevelName = editedLevelName;

		public override void Initialize()
		{
			EditorCheckboxes.Add("Encrypt Scene", UseEncryption, (e, o) => { UseEncryption = true; }, (e, o) => { UseEncryption = false; });

			editorObjectManager = FindComponentOfType<EditorObjectManager>();

			sceneFile = new SceneFile();

			Load();
		}

		public void Save()
		{
			if (editedLevelName == null || editedLevelName == string.Empty)
			{
				EditorScene.ErrorWindow("Warning", "The edited level has no name therefore we dont know how to save it! Add a name in the level constructor!");
				return;
			}

			sceneFile.objects = editorObjectManager.editorObjects;
			sceneFile.editorCamPosition = Globals.camera.GetPosition();
			sceneFile.editorCamScale = Globals.camera.Scale;

			object dataToSave = JsonConvert.SerializeObject(sceneFile);

			if (RunningOnWeb)
			{
				EditorScene.ErrorWindow("Run Console", "Level Printed In Console: " + editedLevelName);
				Say.Line(dataToSave);
				return;
			}
			else
			{
				if (UseEncryption) dataToSave = Encryption.Encrypt((string)dataToSave);
			}

			// game directory
			if (!Directory.Exists(LevelsFolder)) Directory.CreateDirectory(LevelsFolder);

			var gameWritePath = Path.Combine(LevelsFolder, editedLevelName);

			if (UseEncryption) File.WriteAllBytes(gameWritePath, (byte[])dataToSave);
			else File.WriteAllText(gameWritePath, (string)dataToSave);
			//

			// project directory
			var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
			var projectLevelFolder = Path.Combine(projectDirectory, LevelsFolder);
			if (!Directory.Exists(projectLevelFolder)) Directory.CreateDirectory(projectLevelFolder);

			var projectWritePath = Path.Combine(projectLevelFolder, editedLevelName);

			if (UseEncryption) File.WriteAllBytes(projectWritePath, (byte[])dataToSave);
			else File.WriteAllText(projectWritePath, (string)dataToSave);
			//
		}

		public void Load()
		{
			if (editedLevelName == null || editedLevelName == string.Empty)
			{
				EditorScene.ErrorWindow("Warning", "The edited level has no name therefore we dont know how to load it! Add a name in the level constructor!");
				return;
			}

			editorObjectManager.RemoveAll(); // when changing levels in editor

			var loadDir = Path.Combine(LevelsFolder, editedLevelName);
			if (!File.Exists(loadDir))
			{
				Say.Line("No Level File Found! " + loadDir);
				return;
			}
			var dataToLoad = UseEncryption ? Encryption.Decrypt(File.ReadAllBytes(loadDir))
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