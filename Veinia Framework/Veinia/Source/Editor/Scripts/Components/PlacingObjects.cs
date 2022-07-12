using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Veinia.Editor
{
	public class PlacingObjects : Component
	{
		PrefabManager prefabManager;
		public List<EditorObject> objects = new List<EditorObject>();

		public string currentPrefab = "Block";

		GameObject preview;


		public PlacingObjects(PrefabManager prefabs)
		{
			this.prefabManager = prefabs;
		}

		public override void Initialize()
		{
			preview = GetOnlySprite(prefabManager.Find(currentPrefab), Transform.Empty);
			preview.GetComponent<Sprite>().color = Color.White * .5f;
			Instantiate(preview);
		}

		public override void Update()
		{
			ShowPreview();

			if (!Globals.input.GetKey(Keys.LeftAlt))
			{
				if (Globals.input.GetMouseButtonUp(0)
				 || Globals.input.GetMouseButton(0) && Globals.input.GetKey(Keys.LeftShift))
				{
					var mousePos = Globals.input.GetMouseWorldPosition();

					if (!Globals.input.GetKey(Keys.LeftControl))
						mousePos = new Vector2(MathF.Round(mousePos.X), MathF.Round(mousePos.Y));


					if (!CheckForDuplicates(currentPrefab, mousePos))
						PlaceObject(currentPrefab, new Transform(mousePos));
				}
			}
		}

		private void ShowPreview()
		{
			var mousePos = Globals.input.GetMouseWorldPosition();

			if (!Globals.input.GetKey(Keys.LeftControl))
				mousePos = new Vector2(MathF.Round(mousePos.X), MathF.Round(mousePos.Y));

			preview.transform.position = mousePos;
		}

		public void PlaceObject(string prefabName, Transform position)
		{
			Title.Add("Object Count " + objects.Count, 7);

			var objectToSpawn = GetOnlySprite(prefabManager.Find(prefabName), position);

			Instantiate(objectToSpawn);

			objects.Add(new EditorObject
			{
				PrefabName = prefabName,
				Position = objectToSpawn.transform.position
			});
		}

		private bool CheckForDuplicates(string prefabName, Vector2 position)
		{
			if (objects.Find(x => x.Position == position) == null)
				return false;
			else return true;
		}

		private GameObject GetOnlySprite(GameObject gameObject, Transform transform)
		{
			return new GameObject(transform, new List<Component>
			{
				(Sprite)gameObject.GetComponent<Sprite>().Clone()
			}, isStatic: true);
		}
	}
}
