using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace Veinia.Editor
{
	public class PaintingToolbarBehaviour : ToolbarBehaviour
	{
		PrefabManager prefabManager;

		public List<EditorObject> currentObjectLayer = new List<EditorObject>();

		EditorControls editorControls;
		EditorObjectManager editorObjectManager;

		bool markLayer;

		public string currentPrefabName { get; private set; }

		GameObject objectPreview;

		Vector2 mousePos;
		Vector2 mouseGridPos;


		public PaintingToolbarBehaviour(PrefabManager prefabManager) => this.prefabManager = prefabManager;

		public override void OnInitialize()
		{
			editorObjectManager = gameObject.level.FindComponentOfType<EditorObjectManager>();
			editorControls = gameObject.level.FindComponentOfType<EditorControls>();

			editorObjectManager.OnSpawn += (e) =>
			{
				var editorObject = e;
				if (editorObject.PrefabName == currentPrefabName) currentObjectLayer.Add(editorObject);
			};
			editorObjectManager.OnRemove += (e) =>
			{
				var editorObject = e;
				if (editorObject.PrefabName == currentPrefabName) currentObjectLayer.Remove(editorObject);
			};
			editorObjectManager.OnRemoveAll += () => { currentObjectLayer.Clear(); };

			var firstPrefab = prefabManager.prefabs[0];
			if (firstPrefab != null) ChangeCurrentPrefab(firstPrefab.PrefabName);

			EditorCheckboxes.Add("Mark Layer", defaultValue: false, (e, o) => { markLayer = true; }, (e, o) => { markLayer = false; });
		}

		private void SpawnPreview()
		{
			if (objectPreview != null) objectPreview.DestroyGameObject();

			objectPreview = prefabManager.Find(currentPrefabName).ExtractComponentToNewGameObject<Sprite>(Vector2.Zero);
			var sprite = objectPreview.GetComponent<Sprite>();
			sprite.color *= .5f;
			sprite.layer = .9f;

			objectPreview = gameObject.level.Instantiate(objectPreview);
		}

		public void ChangeCurrentPrefab(string newPrefabName)
		{
			currentPrefabName = newPrefabName;
			SpawnPreview();
			UpdatePreview();

			currentObjectLayer = editorObjectManager.editorObjects.FindAll(x => x.PrefabName == currentPrefabName);
		}

		public override void OnExitTab() => objectPreview.DestroyGameObject();
		public override void OnEnterTab() => SpawnPreview();

		public override void OnUpdate()
		{
			mousePos = Globals.input.GetMouseWorldPosition();
			mouseGridPos = new Vector2(MathF.Round(mousePos.X), MathF.Round(mousePos.Y));

			UpdatePreview();

			var swipe = Globals.input.GetKey(Keys.LeftShift);

			if (!editorControls.isDragging && !Globals.myraDesktop.IsMouseOverGUI)
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
			if (objectPreview != null)
			{
				if (!Globals.input.GetKey(Keys.LeftControl))
					objectPreview.transform.position = mouseGridPos;
				else
					objectPreview.transform.position = mousePos;
			}
		}

		public override void OnDraw(SpriteBatch sb)
		{
			if (markLayer)
				foreach (var item in currentObjectLayer)
				{
					sb.DrawRectangle(item.EditorPlacedSprite.rect.OffsetByHalf(), Color.Green, thickness: 4, layerDepth: .9f);
				}
		}
	}
}
