using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Veinia.Source
{
	public class EditorScene : Level
	{
		public EditorScene(PrefabManager prefabManager) : base(prefabManager)
		{
		}

		public override void LoadContents()
		{
			base.LoadContents();

			GameObject systems = Instantiate(Transform.Empty, new List<Component>
			{
				new MouseDrag()
			}, isStatic: true);

			GameObject background = Instantiate(Transform.Empty, new List<Component>
			{
				new Sprite("Editor/Background", 0, Color.Red)
			}, isStatic: true);
		}
	}
}
