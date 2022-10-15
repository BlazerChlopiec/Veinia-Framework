namespace Veinia
{
	public class Loader
	{
		public Level currentLevel;
		public Level previousLevel;

		public void Load(Level level)
		{
			NextFrame.actions.Add(LoadScene);

			void LoadScene()
			{
				if (currentLevel != null)
					Unload();
				currentLevel = level;
				currentLevel.CreateScene();
				currentLevel.InitiazeComponents();
			}
		}

		public void Unload()
		{
			Globals.tweener.CancelAll();
			Globals.collisionComponent = Globals.collisionComponent.GetReloaded();

			foreach (var item in currentLevel.scene.ToArray())
			{
				item.DestroyGameObject();
			}

			previousLevel = currentLevel;
			currentLevel = null;
		}

		public void Reload()
		{
			NextFrame.actions.Add(ReloadScene);

			void ReloadScene()
			{
				Unload();

				currentLevel = previousLevel;
				currentLevel.CreateScene();
				currentLevel.InitiazeComponents();
			}
		}
	}
}