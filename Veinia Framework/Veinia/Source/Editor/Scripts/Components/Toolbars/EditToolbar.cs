using Myra.Graphics2D.UI;

namespace VeiniaFramework.Editor
{
	public class EditToolbar : Toolbar
	{
		public EditToolbar(string toolbarName, ToolbarBehaviour toolbarBehaviour) : base(toolbarName, toolbarBehaviour)
		{
		}

		public override void OnInitialize(GameObject gameObject)
		{
			var editToolbarBehaviour = (EditToolbarBehaviour)toolbarBehaviour;

			var panel = new Panel { Height = 205 };
			displayedToolbarContent = panel;

			var removeSelectedButton = new TextButton { Text = "Remove Selected [RMB]" };
			removeSelectedButton.Click += (o, e) =>
			{
				editToolbarBehaviour.RemoveSelection();
			};
			panel.Widgets.Add(removeSelectedButton);

			var deselectButton = new TextButton { Text = "Deselect [LAlt+D]", Top = 25 };
			deselectButton.Click += (o, e) =>
			{
				editToolbarBehaviour.selectedObjects.Clear();
			};
			panel.Widgets.Add(deselectButton);

			var duplicateButton = new TextButton { Text = "Duplicate [Ctrl+D]", Top = 50 };
			duplicateButton.Click += (o, e) =>
			{
				editToolbarBehaviour.Duplicate();
			};
			panel.Widgets.Add(duplicateButton);

			var filterSelectionButton = new TextButton { Text = "Filter Selection [Q]", Top = 75 };
			filterSelectionButton.Click += (o, e) =>
			{
				editToolbarBehaviour.FilterSelection();
			};
			panel.Widgets.Add(filterSelectionButton);

			var rotateButton = new TextButton { Text = "Rotate [R]", Top = 100, Toggleable = true };
			editToolbarBehaviour.rotateButton = rotateButton;
			panel.Widgets.Add(rotateButton);

			var resetRotationButton = new TextButton { Text = "Reset Rotation [Ctrl+R]", Top = 125 };
			resetRotationButton.Click += (o, e) =>
			{
				editToolbarBehaviour.ResetRotation();
			};
			panel.Widgets.Add(resetRotationButton);

			var editButton = new TextButton { Text = "Edit [E]", Top = 150 };
			editButton.Click += (o, e) =>
			{
				editToolbarBehaviour.Edit();
			};
			panel.Widgets.Add(editButton);

			//
			panel.Widgets.Add(new HorizontalSeparator { Top = 175, VerticalAlignment = VerticalAlignment.Top });
			//

			var resetCamera = new TextButton { Text = "Reset Camera", Top = 185 };
			resetCamera.Click += (o, e) =>
			{
				editToolbarBehaviour.ResetCamera();
			};
			panel.Widgets.Add(resetCamera);
		}
	}
}