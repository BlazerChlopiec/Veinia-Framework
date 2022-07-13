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

			Globals.camera.SetPosition(Vector2.Zero);
			Globals.camera.Zoom = 1;

			GameObject systems = Instantiate(Transform.Empty, new List<Component>
			{
				new Grid(),
				new EditorControls(),
				new EditorObjectManager(prefabManager),
				new EditorJson(editedLevel),
			}, isStatic: true);

			GameObject toolbox = Instantiate(Transform.Empty, new List<Component>
			{
				new Sprite("Editor/Square", .95f, Color.Black, destinationSize: new Vector2(1.2f, 1)),
				new Toolbar(prefabManager)
			}, isStatic: true);
		}
	}
}
