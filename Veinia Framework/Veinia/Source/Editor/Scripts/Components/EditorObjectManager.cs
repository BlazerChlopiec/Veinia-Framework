using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;

namespace Veinia.Editor
{
	public class EditorObjectManager : Component, IDrawn
	{
		PrefabManager prefabManager;
		Label tileCount = new Label { HorizontalAlignment = HorizontalAlignment.Center };

		public List<EditorObject> editorObjects = new List<EditorObject>();
		public List<EditorObject> currentlyEditedObjects = new List<EditorObject>();

		public string currentPrefabName { get; private set; }

		GameObject objectPreview;

		Vector2 mousePos;
		Vector2 mouseGridPos;



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

			objectPreview = prefabManager.Find(currentPrefabName).ExtractComponentToNewGameObject<Sprite>(Vector2.Zero);
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

			currentlyEditedObjects = editorObjects.FindAll(x => x.PrefabName == currentPrefabName);
		}

		public override void Update()
		{
			mousePos = Globals.input.GetMouseWorldPosition();
			mouseGridPos = new Vector2(MathF.Round(mousePos.X), MathF.Round(mousePos.Y));

			UpdatePreview();

			var swipe = Globals.input.GetKey(Keys.LeftShift);

			if (!Globals.input.GetKey(Keys.LeftAlt) && !Globals.myraDesktop.IsMouseOverGUI)
			{
				//placing
				if (Globals.input.GetMouseButtonUp(0) || Globals.input.GetMouseButton(0) && swipe)
				{

					if (!Globals.input.GetKey(Keys.LeftControl))
					{
						if (OverlapsWithPoint(mouseGridPos, currentPrefabName) == null)
							Spawn(currentPrefabName, mouseGridPos);
					}
					else
					{
						if (OverlapsWithPoint(mousePos, currentPrefabName) == null)
							Spawn(currentPrefabName, mousePos);
					}
				}
				//

				//deleting
				if (Globals.input.GetMouseButtonUp(1) || Globals.input.GetMouseButton(1) && swipe)
				{
					var overlap = OverlapsWithPoint(mousePos, currentPrefabName);
					if (overlap == null) overlap = OverlapsWithPoint(mouseGridPos, currentPrefabName);
					if (overlap != null) Remove(overlap);
				}
				//
			}
		}

		private void UpdatePreview()
		{
			if (!Globals.input.GetKey(Keys.LeftControl))
				objectPreview.transform.position = mouseGridPos;
			else
				objectPreview.transform.position = mousePos;
		}

		public void Spawn(string prefabName, Vector2 position)
		{
			var extractedSpriteGameObject = prefabManager.Find(prefabName).ExtractComponentToNewGameObject<Sprite>(position);

			var newEditorObject = new EditorObject
			{
				PrefabName = prefabName,
				Position = position,
				EditorPlacedSprite = Instantiate(extractedSpriteGameObject).GetComponent<Sprite>()
			};

			currentlyEditedObjects.Add(newEditorObject);
			editorObjects.Add(newEditorObject);

			UpdateTitle();
		}

		private void Remove(EditorObject editorObject)
		{
			if (editorObject == null) return;

			editorObject.EditorPlacedSprite.DestroyGameObject();
			currentlyEditedObjects.Remove(editorObject);
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

		private EditorObject OverlapsWithPoint(Vector2 overlapPoint, string prefabName)
		{
			var overlap = editorObjects.Find(x => x.PrefabName == prefabName && x.EditorPlacedSprite.rect
												   .OffsetByHalf()
												   .Contains(Transform.WorldToScreenPos(overlapPoint)));

			return overlap;
		}

		private void UpdateTitle() => tileCount.Text = "Object Count " + (editorObjects.Count);

		public void Draw(SpriteBatch sb)
		{
			foreach (var item in currentlyEditedObjects)
			{
				sb.DrawRectangle(item.EditorPlacedSprite.rect.OffsetByHalf(), Color.Green, thickness: 4, layerDepth: .9f);
			}
		}
	}
}
