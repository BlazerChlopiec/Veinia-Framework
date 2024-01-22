using GeonBit.UI;
using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;

namespace VeiniaFramework.Editor
{
	public class EditorScene : Level
	{
		public Type editedSceneType;
		private static bool errorWindowAppeared;

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
			 * Deselect - LAlt + D
			 * Selection Overlap Menu - Q
			 */

			UserInterface.Active.ShowCursor = false;

			Globals.tweener.CancelAll();
			Globals.unscaledTweener.CancelAll();

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
			if (prefabManager == null) ErrorWindow("Warning", "There are no prefabs! Make a class that inherits PrefabManager and add it to Veinia.Initialize()! Check Samples For Reference.");
			if (levelPath != string.Empty) UI.AddComponent(new EditorManager());
		}

		public static void ErrorWindow(string title, string content)
		{
			if (errorWindowAppeared == true) return;
			errorWindowAppeared = true;

			var panel = new Panel();

			var window = new Window
			{
				Title = title,
				Content = panel,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			};
			window.DragDirection = DragDirection.None;
			window.CloseButton.Click += (s, e) => { errorWindowAppeared = false; if (Globals.loader.current is EditorScene) EditToolbarBehaviour.skipSelectionFrame = true; };
			window.Width = 400;

			var textBox = new TextBox { Text = content, TextColor = Color.Red, Wrap = true };
			textBox.AcceptsKeyboardFocus = false;

			panel.Widgets.Add(textBox);

			window.Show(Globals.myraDesktop, Point.Zero);
		}
	}
}