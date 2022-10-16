using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace Veinia.Editor
{
	public class EditorObjectPainter : Component, IDrawn
	{
		PrefabManager prefabManager;

		public List<EditorObject> currentObjectLayer = new List<EditorObject>();

		EditorControls editorControls;
		EditorObjectManager editorObjectManager;

		public bool allowPainting = false;
		bool drawCurrentlyEditedObjectOutlines = true;

		public string currentPrefabName { get; private set; }

		GameObject objectPreview;

		Vector2 mousePos;
		Vector2 mouseGridPos;



		public EditorObjectPainter(PrefabManager prefabManager) => this.prefabManager = prefabManager;

		public override void Initialize()
		{
			editorObjectManager = GetComponent<EditorObjectManager>();
			editorControls = GetComponent<EditorControls>();

			editorObjectManager.OnSpawn += (e) =>
			{
				var editorObject = (EditorObject)e;
				if (editorObject.PrefabName == currentPrefabName) currentObjectLayer.Add(editorObject);
			};
			editorObjectManager.OnRemove += (e) =>
			{
				var editorObject = (EditorObject)e;
				if (editorObject.PrefabName == currentPrefabName) currentObjectLayer.Remove(editorObject);
			};
			editorObjectManager.OnRemoveAll += () => { currentObjectLayer.Clear(); };

			var firstPrefab = prefabManager.prefabs[0];
			if (firstPrefab != null) ChangeCurrentPrefab(firstPrefab.prefabName);

			EditorOptions.AddOption("Mark Layer", defaultValue: true, (e, o) => { drawCurrentlyEditedObjectOutlines = true; }, (e, o) => { drawCurrentlyEditedObjectOutlines = false; });
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

			currentObjectLayer = editorObjectManager.editorObjects.FindAll(x => x.PrefabName == currentPrefabName);
		}

		public override void Update()
		{
			objectPreview.isEnabled = allowPainting;
			if (!allowPainting) return;

			mousePos = Globals.input.GetMouseWorldPosition();
			mouseGridPos = new Vector2(MathF.Round(mousePos.X), MathF.Round(mousePos.Y));

			UpdatePreview();

			var swipe = Globals.input.GetKey(Keys.LeftShift);

			if (!editorControls.drag && !Globals.myraDesktop.IsMouseOverGUI)
			{
				//placing
				if (Globals.input.GetMouseButtonUp(0) || Globals.input.GetMouseButton(0) && swipe)
				{

					if (!Globals.input.GetKey(Keys.LeftControl))
					{
						if (editorObjectManager.PrefabOverlapsWithPoint(mouseGridPos, currentPrefabName) == null)
							editorObjectManager.Spawn(currentPrefabName, mouseGridPos);
					}
					else
					{
						if (editorObjectManager.PrefabOverlapsWithPoint(mousePos, currentPrefabName) == null)
							editorObjectManager.Spawn(currentPrefabName, mousePos);
					}
				}
				//

				//deleting
				if (Globals.input.GetMouseButtonUp(1) || Globals.input.GetMouseButton(1) && swipe)
				{
					var overlap = editorObjectManager.PrefabOverlapsWithPoint(mousePos, currentPrefabName);
					if (overlap == null) overlap = editorObjectManager.PrefabOverlapsWithPoint(mouseGridPos, currentPrefabName);
					if (overlap != null) editorObjectManager.Remove(overlap);
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

		public void Draw(SpriteBatch sb)
		{
			if (drawCurrentlyEditedObjectOutlines)
				foreach (var item in currentObjectLayer)
				{
					sb.DrawRectangle(item.EditorPlacedSprite.rect.OffsetByHalf(), Color.Green, thickness: 4, layerDepth: .9f);
				}
		}
	}
}
