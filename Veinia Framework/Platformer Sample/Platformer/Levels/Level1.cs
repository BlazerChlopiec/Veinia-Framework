using Microsoft.Xna.Framework;

namespace Veinia.Platformer
{
	public class Level1 : Level
	{
		public Level1(string name, PrefabManager prefabManager) : base(name, prefabManager)
		{
		}

		public override void LoadContents()
		{
			base.LoadContents();

			Instantiate(new Transform(Vector2.Zero), prefabManager.Find("Block"));
		}
	}
}
