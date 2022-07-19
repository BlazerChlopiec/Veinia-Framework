using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Veinia.Editor
{
	public class EditorScene : Level
	{
		string editedLevel;


		public EditorScene(string editedLevel, PrefabManager prefabManager) : base(prefabManager)
		{
			this.editedLevel = editedLevel;
		}

		public override void LoadContents()
		{
			base.LoadContents();

			GameObject systems = Instantiate(Transform.Empty, new List<Component>
			{
				new EditorGrid(),
				new EditorControls(),
				new EditorObjectManager(prefabManager),
				new EditorJson(editedLevel),
			}, isStatic: true);

			GameObject toolbox = Instantiate(Transform.Empty, new List<Component>
			{
				new Toolbar(prefabManager)
			}, isStatic: true);
		}
	}
}
