using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using System.Collections.Generic;
using System.Linq;

namespace VeiniaFramework.Editor
{
	public class EditToolbarBehaviour : ToolbarBehaviour
	{
		EditorObjectManager editorObjectManager;
		EditorControls editorControls;

		public List<EditorObject> selectedObjects = new List<EditorObject>();
		public EditorObject[] clipboard;

		bool isDragging;
		public static bool skipSelectionFrame;

		Vector2 startSelectionPos;
		Vector2 screenMousePos;
		Vector2 screenVisualCursorPos;

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

			if (Globals.input.GetMouseDown(0) && Globals.input.GetKey(Keys.LeftShift) && !Globals.myraDesktop.IsMouseOverGUI && !editorControls.isDragging)
			{
				isDragging = true;
				startSelectionPos = Globals.input.GetMouseScreenPosition();
			}

			if (isDragging)
				screenVisualCursorPos = screenMousePos;

			if (Globals.input.GetKeyDown(Keys.Q))
				SelectionOverlapWindow();
			if (Globals.input.GetKeyDown(Keys.F))
				EditorScene.ErrorWindow("", "");

			// selecting one thing by clicking
			if (Globals.input.GetMouseUp(0) && !isDragging && !editorControls.isDragging && !Globals.myraDesktop.IsMouseOverGUI && !skipSelectionFrame)
			{
				selectedObjects.Clear();

				screenVisualCursorPos = screenMousePos;

				var oneSelection = editorObjectManager.editorObjects.Find(x => x.EditorPlacedSprite.rect.OffsetByHalf().Contains(screenMousePos));
				if (oneSelection != null)
					selectedObjects.Add(oneSelection);
			}
			// mouse up when dragging
			if (Globals.input.GetMouseUp(0) && isDragging)
			{
				isDragging = false;

				foreach (var item in editorObjectManager.GetInsideRectangle(selectionRectangle.AllowNegativeSize()))
				{
					if (!selectedObjects.Contains(item))
						selectedObjects.Add(item);
				}
			}

			if (Globals.input.GetKey(Keys.LeftControl) && Globals.input.GetKeyDown(Keys.D)) Duplicate();

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

			if (Globals.input.GetKeyDown(Keys.Delete) || Globals.input.GetMouseDown(1))
			{
				RemoveSelection();
				selectionOverlapWindow.Close();
			}

			EditorLabelManager.Add("SelectedObjectCount", new Label { Text = "Selected Objects - " + selectedObjects.Count });

			skipSelectionFrame = false;
		}

		Window selectionOverlapWindow;
		public void SelectionOverlapWindow()
		{
			var panel = new Panel();
			var overlaps = editorObjectManager.OverlapsWithPoint(Transform.ScreenToWorldPos(screenVisualCursorPos)).ToList();

			if (selectionOverlapWindow != null) selectionOverlapWindow.Close();
			selectionOverlapWindow = new Window
			{
				Title = "Overlaps",
				Content = panel,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			};
			selectionOverlapWindow.DragDirection = DragDirection.None;
			selectionOverlapWindow.Height = 35 + 70 * overlaps.Count;
			selectionOverlapWindow.Width = 100;
			selectionOverlapWindow.CloseButton.Click += (s, e) => { skipSelectionFrame = true; };

			int overlapButtonSize = 70;

			foreach (var overlap in overlaps)
			{
				var tex = overlap.EditorPlacedSprite.texture;
				var overlapButton = new ImageButton
				{
					Height = overlapButtonSize,
					Width = overlapButtonSize,
					Top = overlapButtonSize * overlaps.IndexOf(overlap),
					VerticalAlignment = VerticalAlignment.Top,
					Background = new TextureRegion(tex.ChangeColor(overlap.EditorPlacedSprite.color), new Rectangle(0, 0, tex.Width, tex.Height)),
				};

				overlapButton.MouseEntered += (s, e) => { selectedObjects.Clear(); selectedObjects.Add(overlap); };
				overlapButton.Click += (s, a) => { selectionOverlapWindow.Close(); skipSelectionFrame = true; };
				overlapButton.MouseLeft += (s, e) => { if (selectedObjects.Contains(overlap) && skipSelectionFrame) selectedObjects.Remove(overlap); };

				panel.Widgets.Add(overlapButton);
			}
			panel.Height = overlaps.Count * overlapButtonSize;

			selectionOverlapWindow.Show(Globals.myraDesktop, Point.Zero);
		}

		public void RemoveSelection()
		{
			foreach (var item in selectedObjects.ToArray())
			{
				selectedObjects.Remove(item);
				editorObjectManager.Remove(item);
			}
		}

		public void Duplicate()
		{
			clipboard = selectedObjects.ToArray();
			selectedObjects.Clear();

			foreach (var item in clipboard)
			{
				var spawnedClipboard = editorObjectManager.Spawn(item.PrefabName, item.Position + Vector2.One);
				selectedObjects.Add(spawnedClipboard);
			}
		}

		public override void OnDraw(SpriteBatch sb)
		{
			sb.DrawCircle(new CircleF(screenVisualCursorPos.ToPoint(), 20), 5, Color.Red, 5, 1);

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
