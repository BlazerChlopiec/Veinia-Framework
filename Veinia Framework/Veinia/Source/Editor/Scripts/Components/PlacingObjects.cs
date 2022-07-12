using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Veinia.Editor
{
	public class PlacingObjects : Component
	{
		PrefabManager prefabs;
		List<EditorObject> objects = new List<EditorObject>();

		GameObject preview;


		public PlacingObjects(PrefabManager prefabs)
		{
			this.prefabs = prefabs;
		}

		public override void Initialize()
		{
			LoadEditorObjects();

			preview = Instantiate(Transform.Empty, new List<Component>
			{
				new Sprite("Block Breaker/Grass Tile",.99f, Color.White * .5f, Vector2.One)
			}, isStatic: false);
		}

		private void LoadEditorObjects()
		{
			// LOAD TODO HERE
			var editorObject = new EditorObject("Block", new Transform(0, 0));
			//

			PlaceObject(editorObject.prefabName, editorObject.transform);
		}

		public override void Update()
		{
			ShowPreview();

			if (Globals.input.GetMouseButtonDown(0) && !Globals.input.GetKey(Keys.LeftAlt))
			{
				var mousePos = Globals.input.GetMouseWorldPosition();

				if (!Globals.input.GetKey(Keys.LeftControl))
					mousePos = new Vector2(MathF.Round(mousePos.X), MathF.Round(mousePos.Y));

				PlaceObject("Block", new Transform(mousePos));
			}
		}

		private void ShowPreview()
		{
			var mousePos = Globals.input.GetMouseWorldPosition();

			if (!Globals.input.GetKey(Keys.LeftControl))
				mousePos = new Vector2(MathF.Round(mousePos.X), MathF.Round(mousePos.Y));

			preview.transform.position = mousePos;
		}

		private void PlaceObject(string prefabName, Transform position)
		{
			Title.Add("Object count " + parent.world.scene.Count, 7);

			var objectToSpawn = prefabs.Find(prefabName);
			var onlySprite = new GameObject(position, new List<Component>
			{
				objectToSpawn.GetComponent<Sprite>()
			}, isStatic: true);


			Instantiate(onlySprite);

			objects.Add(new EditorObject(prefabName, onlySprite.transform));
		}
	}
}
