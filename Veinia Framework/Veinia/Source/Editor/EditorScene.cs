using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Veinia.Editor
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
				new Grid(),
				new EditorControls(),
				new PlacingObjects(prefabManager),
			}, isStatic: true);

			GameObject background = Instantiate(Transform.Empty, new List<Component>
			{
				new Sprite("Editor/Background", 0.01f, Color.Red)
			}, isStatic: true);
		}
	}
}
