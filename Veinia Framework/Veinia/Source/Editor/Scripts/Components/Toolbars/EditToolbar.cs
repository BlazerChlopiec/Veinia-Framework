using Myra.Graphics2D.UI;

namespace Veinia.Editor
{
	public class EditToolbar : Toolbar
	{
		public EditToolbar(string toolbarName, ToolbarBehaviour toolbarBehaviour) : base(toolbarName, toolbarBehaviour)
		{
		}

		public override void OnInitialize(GameObject gameObject)
		{
			var panel = new Panel();
			content = panel;

			var removeSelectionButton = new TextButton { Text = "Remove Selection" };
			removeSelectionButton.Click += (o, e) =>
			{
				var editorObjectEdit = (EditorObjectEdit)toolbarBehaviour;
				editorObjectEdit.RemoveSelection();
			};
			panel.Widgets.Add(removeSelectionButton);
		}
	}
}