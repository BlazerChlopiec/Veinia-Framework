using Myra.Graphics2D.UI;

namespace Veinia.Editor
{
	public class EditToolbar : Toolbar
	{
		//TextButton filterSelectionButton;


		public EditToolbar(string toolbarName, ToolbarBehaviour toolbarBehaviour) : base(toolbarName, toolbarBehaviour)
		{
		}

		public override void OnInitialize(GameObject gameObject)
		{
			var panel = new Panel { Height = 100 };
			finalToolbarContent = panel;

			var removeSelectionButton = new TextButton { Text = "Remove Selection" };
			removeSelectionButton.Click += (o, e) =>
			{
				var editorObjectEdit = (EditToolbarBehaviour)toolbarBehaviour;
				editorObjectEdit.RemoveSelection();
			};
			panel.Widgets.Add(removeSelectionButton);

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