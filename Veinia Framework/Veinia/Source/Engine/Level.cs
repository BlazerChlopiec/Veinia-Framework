public class Level : WorldTools
{
	protected PrefabManager prefabManager;

	public Level(PrefabManager prefabManager)
	{
		this.prefabManager = prefabManager;
	}

	public virtual void LoadContents()
	{
		prefabManager.LoadPrefabs(tools: this);
	}
}