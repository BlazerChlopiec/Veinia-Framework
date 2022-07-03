public class Loader
{
	public Level currentLevel;
	public Level previousLevel;

	public void Load(Level level)
	{
		if (currentLevel != null)
			Unload(currentLevel);

		currentLevel = level;
		currentLevel.LoadContents();

		Title.Add(currentLevel.name, 6);
	}

	public void Unload(Level level)
	{
		foreach (var gameObject in level.scene.ToArray())
		{
			gameObject.DestroyGameObject();
		}

		//crashes
		//previousLevel = currentLevel;
		//currentLevel = null;
	}
}