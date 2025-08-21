using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace VeiniaFramework.Editor
{
	public class PaintingToolbarBehaviour : ToolbarBehaviour
	{
		PrefabManager prefabManager;

		public List<EditorObject> currentObjectLayer = new List<EditorObject>();

		EditorControls editorControls;
		EditorObjectManager editorObjectManager;

		bool markLayer;

		public static string currentPrefabName { get; private set; }

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

			// currentPrefabName is static so it remembers between editor sessions
			ChangeCurrentPrefab(currentPrefabName == null ? prefabManager.prefabs[0].PrefabName : currentPrefabName);

			EditorCheckboxes.Add("Mark Layer", defaultValue: false, (e, o) => { markLayer = true; }, (e, o) => { markLayer = false; });
		}

		public void CreateNewPreview()
		{
			if (objectPreview != null) objectPreview.DestroyGameObject();

			objectPreview = prefabManager.Find(currentPrefabName).ExtractComponentToNewGameObject<Sprite>(Transform.Empty);
			objectPreview = gameObject.level.Instantiate(objectPreview);

			var sprite = objectPreview.GetComponent<Sprite>();
			sprite.transform.Z = float.MaxValue;
			sprite.color *= .5f;

			UpdatePreview();
		}

		private void UpdatePreview()
		{
			if (objectPreview == null) return;

			if (!Globals.input.GetKey(Keys.LeftControl))
				objectPreview.transform.position = mouseGridPos;
			else
				objectPreview.transform.position = mousePos;
		}

		public void ChangeCurrentPrefab(string newPrefabName)
		{
			currentPrefabName = newPrefabName;

			currentObjectLayer = editorObjectManager.editorObjects.FindAll(x => x.PrefabName == currentPrefabName);
		}

		public override void OnExitTab(Toolbar newToolbar) => objectPreview.DestroyGameObject();
		public override void OnEnterTab() => CreateNewPreview();

		public override void OnUpdate()
		{
			mousePos = Globals.input.GetMouseWorldPosition();
			mouseGridPos = new Vector2(MathF.Round(mousePos.X), MathF.Round(mousePos.Y));

			UpdatePreview();

			var swipe = Globals.input.GetKey(Keys.LeftShift);

			if (!editorControls.isDragging && !Globals.myraDesktop.IsMouseOverGUI && !EditorControls.isMouseOverGUIPreviousFrame)
			{
				//placing
				if (Globals.input.GetMouseUp(0) || Globals.input.GetMouse(0) && swipe)
				{

					if (!Globals.input.GetKey(Keys.LeftControl))
					{
						if (editorObjectManager.PrefabOverlapsWithPoint(mouseGridPos, currentPrefabName) == null)
							editorObjectManager.Spawn(currentPrefabName, position: mouseGridPos);
					}
					else
					{
						if (editorObjectManager.PrefabOverlapsWithPoint(mousePos, currentPrefabName) == null)
							editorObjectManager.Spawn(currentPrefabName, position: mousePos);
					}
				}
				//

				//deleting
				if (Globals.input.GetMouseUp(1) || Globals.input.GetMouse(1) && swipe)
				{
					var overlap = editorObjectManager.PrefabOverlapsWithPoint(mousePos, currentPrefabName);
					if (overlap == null) overlap = editorObjectManager.PrefabOverlapsWithPoint(mouseGridPos, currentPrefabName);
					if (overlap != null) editorObjectManager.Remove(overlap);
				}
				//
			}
		}


		public override void OnDraw(SpriteBatch sb)
		{
			if (markLayer)
				foreach (var item in currentObjectLayer)
				{
					sb.VeiniaRectangle(gameObject.level, item.EditorPlacedSprite.rect.OffsetByHalf(), Color.Green, thickness: 4);
				}
		}
	}
}
