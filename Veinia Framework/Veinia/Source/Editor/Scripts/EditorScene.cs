using GeonBit.UI;
using System;
using System.Collections.Generic;

namespace VeiniaFramework.Editor
{
	public class EditorScene : Level
	{
		public Type editedSceneType;

		public EditorScene(string levelPath, Type editedSceneType) : base(levelPath) => this.editedSceneType = editedSceneType;

		public override void CreateScene(bool loadObjectsFromPath)
		{
			// the gameObjects shouldn't be loaded as usual because later we only load their sprites
			base.CreateScene(loadObjectsFromPath: false);

			/* KEYBOARD BINDINGS
			 * 
			 * Toolbar Swap - 1-10
			 * Drag - LAlt + LMB 
			 * Swipe Motion - LShift + LMB
			 * Hide Grid - G
			 * Save - LCtrl + S
			 * Duplicate Selection - LCtrl + D
			 * Move Selection - WSAD
			 * Move Selection Slower - WSAD + LShift
			 * Remove Selection - RMB
			 */

			UserInterface.Active.ShowCursor = false;

			EditorCheckboxes.Add("Debug Draw", defaultValue: Globals.debugDraw, (e, o) => { Globals.debugDraw = true; }, (e, o) => { Globals.debugDraw = false; });

			GameObject systems = Instantiate(Transform.Empty, new List<Component>
			{
				new EditorControls(),
				new EditorGrid(),
				new EditorJSON(levelPath),
				new EditorObjectManager(prefabManager),
			}, isStatic: true);

			var toolbarManager = new ToolbarManager();
			if (prefabManager != null) toolbarManager.toolbars.Add(new PaintingToolbar("Painting", new PaintingToolbarBehaviour(prefabManager), prefabManager));
			toolbarManager.toolbars.Add(new EditToolbar("Edit", new EditToolbarBehaviour()));

			GameObject UI = Instantiate(Transform.Empty, new List<Component>
			{
				new EditorLabelManager(),
				new EditorCheckboxes(),
				new FPSWindow(),
			}, isStatic: true);
			UI.AddComponent(toolbarManager);
			if (levelPath != string.Empty) UI.AddComponent(new EditorManager());
		}
	}
}
