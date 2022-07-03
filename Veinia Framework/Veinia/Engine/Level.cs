public class Level : WorldTools
{
	public readonly string name;
	protected Prefabs prefabs;

	public Level(string name, Prefabs prefabs)
	{
		this.name = name;
		this.prefabs = prefabs;
	}

	public virtual void LoadContents()
	{
		prefabs.AddDefaultPrefabs(this);
	}
}