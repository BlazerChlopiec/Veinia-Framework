namespace Veinia
{
	public sealed class Loader
	{
		public Level current;
		public Level previous;

		public void Load(Level level)
		{
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
}