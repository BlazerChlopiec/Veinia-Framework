public class Level : WorldTools
{
	public readonly string name;
	protected PrefabManager prefabManager;

	public Level(string name, PrefabManager prefabManager)
	{
		this.name = name;
		this.prefabManager = prefabManager;
	}

	public virtual void LoadContents()
	{
		prefabManager.LoadPrefabs(tools: this);
	}
}