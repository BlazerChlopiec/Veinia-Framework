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
		for (int i = 0; i < level.scene.Count; i++)
		{
			level.scene[i].DestroyGameObject();
		}
		previousLevel = currentLevel;
		currentLevel = null;
	}
}