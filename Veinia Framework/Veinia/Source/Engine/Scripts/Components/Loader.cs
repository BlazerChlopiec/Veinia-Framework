using System;
using System.Collections.Generic;

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
			dynamic storedLevelInstance = Activator.CreateInstance(storedLevel.storedLevelType);
			storedLevelInstance.levelPath = storedLevel.storedLevelPath;
			Convert.ChangeType(storedLevelInstance, storedLevel.storedLevelType);

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
		public string storedLevelPath;
		public Type storedLevelType;
	}
}