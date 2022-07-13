using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Veinia.Editor
{
	public class EditorObjectManager : Component
	{
		PrefabManager prefabManager;
		Toolbar toolbar;

		public List<EditorObject> editorObjects = new List<EditorObject>();

		public string currentPrefabName = "Block";

		GameObject preview;



		public EditorObjectManager(PrefabManager prefabManager)
		{
			this.prefabManager = prefabManager;
		}

		public override void Initialize()
		{
			toolbar = FindComponentOfType<Toolbar>();
			SpawnPreview();
		}

		private void SpawnPreview()
		{
			if (preview != null) preview.DestroyGameObject();

			preview = prefabManager.Find(currentPrefabName).ExcludeToOnlySpriteComponent(Vector2.Zero);
			var sprite = preview.GetComponent<Sprite>();
			sprite.color *= .5f;
			sprite.layer = .9f;

			preview = Instantiate(preview);
		}

		public void ChangeCurrentPrefab(string newPrefabName)
		{
			currentPrefabName = newPrefabName;
			SpawnPreview();
			UpdatePreview();
		}

		public override void Update()
		{
			UpdatePreview();

			if (!Globals.input.GetKey(Keys.LeftAlt) && !toolbar.hoveringOver)
			{
				//placing
				if (Globals.input.GetMouseButtonUp(0)
				 || Globals.input.GetMouseButton(0) && Globals.input.GetKey(Keys.LeftShift))
				{
					var mousePos = Globals.input.GetMouseWorldPosition();

					if (!Globals.input.GetKey(Keys.LeftControl))
						mousePos = new Vector2(MathF.Round(mousePos.X), MathF.Round(mousePos.Y));


					if (OverlapsWithMouse(currentPrefabName) == null)
						Spawn(currentPrefabName, mousePos);
				}
				//

				//deleting
				if (Globals.input.GetMouseButtonUp(1)
				 || Globals.input.GetMouseButton(1) && Globals.input.GetKey(Keys.LeftShift))
				{
					var overlap = OverlapsWithMouse(currentPrefabName);
					if (overlap != null) Remove(overlap);
				}
				//
			}
		}

		private void UpdatePreview()
		{
			var mousePos = Globals.input.GetMouseWorldPosition();

			if (!Globals.input.GetKey(Keys.LeftControl))
				mousePos = new Vector2(MathF.Round(mousePos.X), MathF.Round(mousePos.Y));

			preview.transform.position = mousePos;
		}

		public void Spawn(string prefabName, Vector2 position)
		{
			var extractedSpriteGameObject = prefabManager.Find(prefabName).ExcludeToOnlySpriteComponent(position);

			editorObjects.Add(new EditorObject
			{
				PrefabName = prefabName,
				Position = position,
				EditorPlacedSprite = Instantiate(extractedSpriteGameObject).GetComponent<Sprite>()
			});

			UpdateTitle();
		}

		private void Remove(EditorObject editorObject)
		{
			if (editorObject == null) return;

			editorObject.EditorPlacedSprite.DestroyGameObject();
			editorObjects.Remove(editorObject);

			UpdateTitle();
		}

		private EditorObject OverlapsWithMouse(string prefabName)
		{
			var overlap = editorObjects.Find(x => x.PrefabName == prefabName && x.EditorPlacedSprite.rect
												   .OffsetByHalf()
												   .Contains(Globals.input.GetMouseScreenPosition()));

			return overlap;
		}

		private void UpdateTitle() => Title.Add("Object Count " + (editorObjects.Count), 7);
	}
}
