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

			var panel = new Panel { Height = 100 };
			finalToolbarContent = panel;

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
				editToolbarBehaviour.SelectionOverlapWindow();
			};
			panel.Widgets.Add(filterSelectionButton);
		}
	}
}