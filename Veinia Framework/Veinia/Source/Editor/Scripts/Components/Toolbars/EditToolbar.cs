﻿using Myra.Graphics2D.UI;

namespace VeiniaFramework.Editor
{
	public class EditToolbar : Toolbar
	{
		//TextButton filterSelectionButton;


		public EditToolbar(string toolbarName, ToolbarBehaviour toolbarBehaviour) : base(toolbarName, toolbarBehaviour)
		{
		}

		public override void OnInitialize(GameObject gameObject)
		{
			var editToolbarBehaviour = (EditToolbarBehaviour)toolbarBehaviour;

			var panel = new Panel { Height = 100 };
			finalToolbarContent = panel;

			var removeSelectedButton = new TextButton { Text = "Remove Selected" };
			removeSelectedButton.Click += (o, e) =>
			{
				editToolbarBehaviour.RemoveSelection();
			};
			panel.Widgets.Add(removeSelectedButton);

			var deselectButton = new TextButton { Text = "Deselect", Top = 25 };
			deselectButton.Click += (o, e) =>
			{
				editToolbarBehaviour.selectedObjects.Clear();
			};
			panel.Widgets.Add(deselectButton);

			var duplicateButton = new TextButton { Text = "Duplicate", Top = 50 };
			duplicateButton.Click += (o, e) =>
			{
				editToolbarBehaviour.Duplicate();
			};
			panel.Widgets.Add(duplicateButton);

			//filterSelectionButton = new TextButton { Toggleable = true, Text = "Filter Selection", Top = 30 };

			//panel.Widgets.Add(filterSelectionButton);
		}
		//public override void OnUpdate()
		//{
		//	var editorObjectEdit = (EditorObjectEdit)toolbarBehaviour;
		//	if (editorObjectEdit.selectedObjects.Count == 1)
		//		filterSelectionButton.Text = "Filter Selection " + editorObjectEdit.selectedObjects[0].PrefabName;
		//	else filterSelectionButton.Text = "Filter Selection";
		//}
	}
}