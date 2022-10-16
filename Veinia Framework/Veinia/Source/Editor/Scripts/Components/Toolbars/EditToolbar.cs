namespace Veinia.Editor
{
	public class EditToolbar : Toolbar
	{
		EditorObjectEdit objectEdit;


		public EditToolbar(string toolbarName) : base(toolbarName)
		{

		}

		public override void OnInitialize(GameObject gameObject)
		{
			objectEdit = gameObject.level.FindComponentOfType<EditorObjectEdit>();
		}

		public override void OnFocus(GameObject gameObject)
		{
			objectEdit.allowEdit = true;
		}

		public override void OnLostFocus(GameObject gameObject)
		{
			objectEdit.allowEdit = false;
		}
	}
}