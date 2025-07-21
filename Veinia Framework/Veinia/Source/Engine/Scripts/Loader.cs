﻿using System;
using System.Collections.Generic;
using VeiniaFramework.Editor;

namespace VeiniaFramework
{
	public sealed class Loader
	{
		public PrefabManager prefabManager;

		public List<StoredLevel> storedLevels = new List<StoredLevel>();

		public Level current;
		public Level previous;

		public Loader(PrefabManager prefabManager) => this.prefabManager = prefabManager;


		public void StoredLevelLoad(int index)
		{
			var storedLevel = storedLevels[index];
			dynamic storedLevelInstance = Activator.CreateInstance(storedLevel.type);
			storedLevelInstance.levelPath = storedLevel.path;
			Convert.ChangeType(storedLevelInstance, storedLevel.type);

			storedLevelInstance.prefabManager = prefabManager;

			NextFrame.actions.Add(LoadScene);

			void LoadScene()
			{
				current?.Unload();
				previous = current;
				current = null;

				current = storedLevelInstance;
				current.CreateScene();
				current.InitializeComponentsFirstFrame();
			}
		}

		public void DynamicalyLoad(Level level)
		{
			level.prefabManager = prefabManager;

			NextFrame.actions.Add(LoadScene);

			void LoadScene()
			{
				current?.Unload();
				previous = current;
				current = null;

				current = level;
				current.CreateScene();
				current.InitializeComponentsFirstFrame();
			}
		}

		public int GetCurrentLevelIndex()
		{
			StoredLevel match;
			if (current is EditorScene)
			{
				EditorScene editor = (EditorScene)current;
				match = storedLevels.Find(x => x.path == current.levelPath && x.type == editor.editedSceneType);
			}
			else
				match = storedLevels.Find(x => x.path == current.levelPath && x.type == current.GetType());

			return match != null ? storedLevels.IndexOf(match) : default;
		}

		public void Reload()
		{
			NextFrame.actions.Add(ReloadScene);

			void ReloadScene()
			{
				current.Unload();
				previous = current;
				current = null;

				current = previous;
				current.CreateScene();
				current.InitializeComponentsFirstFrame();
			}
		}
	}

	public class StoredLevel
	{
		public string path;
		public Type type;

		public StoredLevel(string path, Type type)
		{
			this.path = path;
			this.type = type;
		}
	}
}