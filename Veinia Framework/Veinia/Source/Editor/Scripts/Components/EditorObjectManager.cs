using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;

namespace Veinia.Editor
{
	public class EditorObjectManager : Component
	{
		PrefabManager prefabManager;
		Label tileCount = new Label { HorizontalAlignment = HorizontalAlignment.Center };

		public List<EditorObject> editorObjects = new List<EditorObject>();

		public string currentPrefabName;

		GameObject objectPreview;



		public EditorObjectManager(PrefabManager prefabManager) => this.prefabManager = prefabManager;

		public override void Initialize()
		{
			var firstPrefab = prefabManager.prefabs[0];
			if (firstPrefab != null) ChangeCurrentPrefab(firstPrefab.prefabName);

			level.Myra.Widgets.Add(tileCount);
			tileCount.Text = "Object Count " + (editorObjects.Count);
			UpdateTitle();
		}

		private void SpawnPreview()
		{
			if (objectPreview != null) objectPreview.DestroyGameObject();

			objectPreview = prefabManager.Find(currentPrefabName).ExtractSpriteToNewGameObject(Vector2.Zero);
			var sprite = objectPreview.GetComponent<Sprite>();
			sprite.color *= .5f;
			sprite.layer = .9f;

			objectPreview = Instantiate(objectPreview);
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

			if (!Globals.input.GetKey(Keys.LeftAlt) && !Globals.myraDesktop.IsMouseOverGUI)
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

			objectPreview.transform.position = mousePos;
		}

		public void Spawn(string prefabName, Vector2 position)
		{
			var extractedSpriteGameObject = prefabManager.Find(prefabName).ExtractSpriteToNewGameObject(position);

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

		public void RemoveAll()
		{
			foreach (var editorObject in editorObjects.ToArray())
			{
				if (editorObject == null) return;

				editorObject.EditorPlacedSprite.DestroyGameObject();
				editorObjects.Remove(editorObject);
			}

			UpdateTitle();
		}

		private EditorObject OverlapsWithMouse(string prefabName)
		{
			var overlap = editorObjects.Find(x => x.PrefabName == prefabName && x.EditorPlacedSprite.rect
												   .OffsetByHalf()
												   .Contains(Globals.input.GetMouseScreenPosition()));

			return overlap;
		}

		private void UpdateTitle() => tileCount.Text = "Object Count " + (editorObjects.Count);
	}
}
