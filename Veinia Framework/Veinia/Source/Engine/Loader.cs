using System;
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

			var storedLevelInstance = (Level)Activator.CreateInstance(storedLevel.type);
			storedLevelInstance.levelPath = storedLevel.path;

			DynamicalyLoad(storedLevelInstance);
		}

		public void DynamicalyLoad(Level level)
		{
			level.prefabManager = prefabManager;

			NextFrame.actions.Add(delegate
			{
				current?.Unload();
				previous = current;
				current = null;

				current = level;
				current.CreateScene();
				current.InitializeComponentsFirstFrame();

				if (FrustumCulling.autoCulling) FrustumCulling.Cull(current);
			});
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
			NextFrame.actions.Add(delegate
			{
				current.Unload();
				previous = current;
				current = null;

				current = previous;
				current.CreateScene();
				current.InitializeComponentsFirstFrame();
			});
		}

		public void LoadNextStored()
		{
			if (current == null && storedLevels.Count > 0) StoredLevelLoad(0);
			else
			{
				var index = storedLevels.IndexOf(storedLevels.Find(x => x.path == current.levelPath));
				index++;
				if (storedLevels.Count > index)
					StoredLevelLoad(index);
				else
					Say.Line("There is no more stored levels! index: " + index);
			}
		}

		public void AddStoredLevel(Level level)
		{
			storedLevels.Add(new StoredLevel(
				level.levelPath,
				level.GetType()
			));
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