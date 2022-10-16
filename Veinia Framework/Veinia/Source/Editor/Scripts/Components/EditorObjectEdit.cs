using Microsoft.Xna.Framework.Input;

namespace Veinia.Editor
{
	public class EditorObjectEdit : Component
	{
		EditorObjectManager editorObjectManager;

		public bool allowEdit;


		public override void Initialize()
		{
			editorObjectManager = FindComponentOfType<EditorObjectManager>();
		}

		public override void Update()
		{
			bool shift = Globals.input.GetKey(Keys.LeftShift);

			if (Globals.input.GetMouseButton(0) && shift)
			{
				// make a new function that check overlap between all prefabs
				var selected = editorObjectManager.OverlapsWithPoint(Globals.input.GetMouseWorldPosition(), "Block");
			}
		}
	}
}
