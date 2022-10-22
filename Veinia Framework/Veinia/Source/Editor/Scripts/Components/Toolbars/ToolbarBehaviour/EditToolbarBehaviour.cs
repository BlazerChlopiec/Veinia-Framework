using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Myra.Graphics2D.UI;
using System.Collections.Generic;

namespace Veinia.Editor
{
	public class EditToolbarBehaviour : ToolbarBehaviour
	{
		EditorObjectManager editorObjectManager;
		EditorControls editorControls;

		public List<EditorObject> selectedObjects = new List<EditorObject>();
		public EditorObject[] clipboard;

		bool isDragging;

		Vector2 startSelectionPos;
		Vector2 screenMousePos;

		Rectangle selectionRectangle;


		public override void OnInitialize()
		{
			editorObjectManager = gameObject.level.FindComponentOfType<EditorObjectManager>();
			editorControls = gameObject.level.FindComponentOfType<EditorControls>();

			EditorLabelManager.Add("SelectedObjectCount", new Label { Text = "Selected Objects - " + selectedObjects.Count, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Center, Top = 25 });

			editorObjectManager.OnRemoveAll += () => { selectedObjects.Clear(); };
		}

		public override void OnExitTab() => selectedObjects.Clear();

		public override void OnUpdate()
		{
			screenMousePos = Globals.input.GetMouseScreenPosition();

			if (Globals.input.GetMouseButtonDown(0) && Globals.input.GetKey(Keys.LeftShift) && !Globals.myraDesktop.IsMouseOverGUI && !editorControls.isDragging)
			{
				isDragging = true;
				startSelectionPos = Globals.input.GetMouseScreenPosition();
			}

			//mouse up
			if (Globals.input.GetMouseButtonUp(0) && !isDragging && !editorControls.isDragging && !Globals.myraDesktop.IsMouseOverGUI)
			{
				selectedObjects.Clear();

				var oneSelection = editorObjectManager.editorObjects.Find(x => x.EditorPlacedSprite.rect.OffsetByHalf().Contains(screenMousePos));
				if (oneSelection != null)
					selectedObjects.Add(oneSelection);
			}
			//mouse up when dragging
			if (Globals.input.GetMouseButtonUp(0) && isDragging)
			{
				isDragging = false;

				foreach (var item in editorObjectManager.GetInsideRectangle(selectionRectangle.AllowNegativeSize()))
				{
					if (!selectedObjects.Contains(item))
						selectedObjects.Add(item);
				}
			}

			if (Globals.input.GetKey(Keys.LeftControl) && Globals.input.GetKeyDown(Keys.C)) Copy();
			if (Globals.input.GetKey(Keys.LeftControl) && Globals.input.GetKeyDown(Keys.V)) Paste();

			if (!Globals.input.GetKey(Keys.LeftControl))
			{
				float shiftMultiplier = Globals.input.GetKey(Keys.LeftShift) ? .05f : 1f;

				if (Globals.input.GetKeyDown(Keys.W))
					foreach (var item in selectedObjects)
						item.Position += new Vector2(0, 1) * shiftMultiplier;

				if (Globals.input.GetKeyDown(Keys.S))
					foreach (var item in selectedObjects)
						item.Position += new Vector2(0, -1) * shiftMultiplier;

				if (Globals.input.GetKeyDown(Keys.A))
					foreach (var item in selectedObjects)
						item.Position += new Vector2(-1, 0) * shiftMultiplier;

				if (Globals.input.GetKeyDown(Keys.D))
					foreach (var item in selectedObjects)
						item.Position += new Vector2(1, 0) * shiftMultiplier;
			}

			if (Globals.input.GetKeyDown(Keys.Delete) || Globals.input.GetMouseButtonDown(1))
				RemoveSelection();

			EditorLabelManager.Add("SelectedObjectCount", new Label { Text = "Selected Objects - " + selectedObjects.Count });
		}

		public void RemoveSelection()
		{
			foreach (var item in selectedObjects.ToArray())
			{
				selectedObjects.Remove(item);
				editorObjectManager.Remove(item);
			}
		}

		public void Copy() => clipboard = selectedObjects.ToArray();

		public void Paste()
		{
			selectedObjects.Clear();

			foreach (var item in clipboard)
			{
				var spawnedClipboard = editorObjectManager.Spawn(item.PrefabName, item.Position + Vector2.One);
				selectedObjects.Add(spawnedClipboard);
			}
		}

		public override void OnDraw(SpriteBatch sb)
		{
			foreach (var selected in selectedObjects)
				sb.DrawRectangle(selected.EditorPlacedSprite.rect.OffsetByHalf(), Color.Blue, 10, .99f);

			if (isDragging)
			{
				var difference = startSelectionPos - screenMousePos;
				selectionRectangle = new Rectangle((int)startSelectionPos.X, (int)startSelectionPos.Y, (int)-difference.X, (int)-difference.Y);

				sb.DrawRectangle(selectionRectangle.AllowNegativeSize(), Color.Red, 10, 1);
			}
		}
	}
}
