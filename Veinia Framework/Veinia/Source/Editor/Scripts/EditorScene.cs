using GeonBit.UI;
using System.Collections.Generic;

namespace Veinia.Editor
{
	public class EditorScene : Level
	{
		public EditorScene(PrefabManager prefabManager, string levelPath) : base(prefabManager, levelPath)
		{
		}

		public override void CreateScene(bool loadObjectsFromPath)
		{
			// the gameObjects shouldn't be loaded as usual because later we only load their sprites
			base.CreateScene(loadObjectsFromPath: false);

			UserInterface.Active.ShowCursor = true;

			GameObject systems = Instantiate(Transform.Empty, new List<Component>
			{
				new EditorGrid(),
				new EditorControls(),
				new EditorObjectManager(prefabManager),
				new EditorJson(levelPath),
				new Toolbar(prefabManager),
			}, isStatic: true);
		}
	}
}
