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

			UserInterface.Active.ShowCursor = false;

			GameObject systems = Instantiate(Transform.Empty, new List<Component>
			{
				new EditorControls(),
				new EditorGrid(),
				new EditorLoader(levelPath),
				new EditorObjectManager(prefabManager),
			}, isStatic: true);

			GameObject UI = Instantiate(Transform.Empty, new List<Component>
			{
				new EditorLabelManager(),
				new EditorCheckboxes(),
				new EditorManager(),
				new FPSWindow(),
				new ToolbarManager(new List<Toolbar>
				{
					new PaintingToolbar("Painting", new PaintingToolbarBehaviour(prefabManager), prefabManager),
					new EditToolbar("Edit", new EditToolbarBehaviour()),
				}),
			}, isStatic: true);
		}
	}
}
