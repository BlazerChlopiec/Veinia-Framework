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
				new EditorControls(),
				new EditorGrid(),
				new EditorLoader(levelPath),
				new EditorObjectManager(prefabManager),
				new EditorObjectPainter(prefabManager),
				new EditorObjectEdit()
			}, isStatic: true);

			GameObject UI = Instantiate(Transform.Empty, new List<Component>
			{
				new EditorLabelManager(),
				new EditorOptions(),
				new EditorManager(),
				new FPSWindow(),
				new ToolbarManager(new List<Toolbar> { new PrefabToolbar(prefabManager, "Painting"), new EditToolbar("Edit") }),
			}, isStatic: true);
		}
	}
}
