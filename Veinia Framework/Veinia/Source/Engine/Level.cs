using Veinia.Editor;

namespace Veinia
{
	public class Level : WorldTools
	{
		protected PrefabManager prefabManager;

		public Level(PrefabManager prefabManager)
		{
			this.prefabManager = prefabManager;
		}

		public virtual void LoadContents()
		{
			prefabManager.LoadPrefabs(tools: this);
		}

		protected void LoadEditorObjects()
		{
			// LOAD TODO HERE
			var editorObject = new EditorObject("Block", new Transform(0, 0));
			//

			Instantiate(editorObject.transform, prefabManager.Find(editorObject.prefabName));
		}
	}
}