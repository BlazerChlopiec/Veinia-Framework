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

		bool selectDragging;
		public static bool skipSelectionFrame;
		public TextButton rotateButton; // determine if IsPressed

		private float rotationSensitivity = .5f;

		Vector2 startSelectionPos;
		Vector2 screenMousePos;
		Vector2 overlapsVisualPos;

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
				selectDragging = true;
				startSelectionPos = Globals.input.GetMouseScreenPosition();
			}

			if (Globals.input.GetKeyDown(Keys.Q))
				SelectionOverlapWindow();

			if (Globals.input.GetKeyDown(Keys.R))
				rotateButton.IsPressed = !rotateButton.IsPressed;

			// selecting one thing by clicking
			if (Globals.input.GetMouseUp(0) && !selectDragging && !editorControls.isDragging && !Globals.myraDesktop.IsMouseOverGUI && !skipSelectionFrame)
			{
				selectedObjects.Clear();

				var oneSelection = editorObjectManager.editorObjects.Find(x => x.EditorPlacedSprite.rect.OffsetByHalf().Contains(screenMousePos));
				if (oneSelection != null)
					selectedObjects.Add(oneSelection);
			}
			// mouse up when dragging
			if (Globals.input.GetMouseUp(0) && selectDragging)
			{
				selectDragging = false;

				foreach (var item in editorObjectManager.GetInsideRectangle(selectionRectangle.AllowNegativeSize()))
				{
					if (!selectedObjects.Contains(item))
						selectedObjects.Add(item);
				}
			}

			if (Globals.input.GetKey(Keys.LeftControl) && Globals.input.GetKeyDown(Keys.D)) Duplicate();
			if (Globals.input.GetKey(Keys.LeftAlt) && Globals.input.GetKeyDown(Keys.D)) selectedObjects.Clear();
			if (Globals.input.GetKeyDown(Keys.Delete) || Globals.input.GetMouseDown(1))
			{
				RemoveSelection();
				if (selectionOverlapWindow != null) selectionOverlapWindow.Close();
			}

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

			EditorControls.disableDragMove = rotateButton.IsPressed && selectedObjects.Count > 0;
			if (rotateButton.IsPressed && editorControls.isDragging && Globals.input.GetMouse(0))
			{
				selectedObjects.ForEach(x => x.Rotation -= Globals.input.mouseX * rotationSensitivity);
			}

			EditorLabelManager.Add("SelectedObjectCount", new Label { Text = "Selected Objects - " + selectedObjects.Count });

			skipSelectionFrame = false;
		}

		Window selectionOverlapWindow;
		public void SelectionOverlapWindow()
		{
			overlapsVisualPos = Globals.input.GetMouseScreenPosition();

			var panel = new Panel();
			var overlaps = editorObjectManager.OverlapsWithPoint(Transform.ScreenToWorldPos(overlapsVisualPos)).ToList();

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
			selectionOverlapWindow.CloseButton.Click += (s, e) =>
			{
				skipSelectionFrame = true;
				selectionOverlapWindow = null;
			};

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
				overlapButton.Click += (s, a) => { selectionOverlapWindow.Close(); selectionOverlapWindow = null; skipSelectionFrame = true; };
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
				var spawnedClipboard = editorObjectManager.Spawn(item.PrefabName, new Transform
				{
					position = item.Position + Vector2.One,
					rotation = item.Rotation
				});
				selectedObjects.Add(spawnedClipboard);
			}
		}

		public void ResetRotation() => selectedObjects.ForEach(x => x.Rotation = 0);

		public override void OnDraw(SpriteBatch sb)
		{
			if (selectionOverlapWindow != null)
				sb.DrawCircle(new CircleF(overlapsVisualPos.ToPoint(), 20), 5, Color.Red, 5, 1);

			foreach (var selected in selectedObjects)
				sb.DrawRectangle(selected.EditorPlacedSprite.rect.OffsetByHalf(), Color.Blue, 10, .99f);

			if (selectDragging)
			{
				var difference = startSelectionPos - screenMousePos;
				selectionRectangle = new Rectangle((int)startSelectionPos.X, (int)startSelectionPos.Y, (int)-difference.X, (int)-difference.Y);

				sb.DrawRectangle(selectionRectangle.AllowNegativeSize(), Color.Red, 10, 1);
			}
		}
	}
}