using Myra.Graphics2D.UI;

namespace Veinia.Editor
{
	public class EditToolbar : Toolbar
	{
		EditorObjectEdit editorObjectEdit;


		public EditToolbar(string toolbarName) : base(toolbarName)
		{

		}

		public override void OnInitialize(GameObject gameObject)
		{
			editorObjectEdit = gameObject.level.FindComponentOfType<EditorObjectEdit>();
		}

		public override void OnFocus(GameObject gameObject)
		{
			editorObjectEdit.allowEdit = true;

			var panel = new Panel();
			content = panel;


			var removeSelectionButton = new TextButton { Text = "Remove Selection" };
			removeSelectionButton.Click += (o, e) => { editorObjectEdit.RemoveSelection(); };
			panel.Widgets.Add(removeSelectionButton);
		}

		public override void OnLostFocus(GameObject gameObject)
		{
			editorObjectEdit.allowEdit = false;
		}
	}
}