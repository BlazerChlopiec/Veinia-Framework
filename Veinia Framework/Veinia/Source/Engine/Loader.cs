namespace Veinia
{
	public class Loader
	{
		public Level currentLevel;
		public Level previousLevel;

		public void Load(Level level)
		{
			if (currentLevel != null)
				Unload();

			currentLevel = level;
			currentLevel.LoadContents();
			NextFrame.actions.Add(currentLevel.InitiazeComponents);
		}

		public void Unload()
		{
			Globals.tweener.CancelAll();

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
				currentLevel.LoadContents();
				currentLevel.InitiazeComponents();
			}
		}
	}
}