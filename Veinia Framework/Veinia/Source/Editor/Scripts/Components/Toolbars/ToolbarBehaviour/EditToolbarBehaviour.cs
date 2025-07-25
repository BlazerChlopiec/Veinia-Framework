﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Properties;
using System;
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

		Window editWindow;

		bool selectDragging;
		public TextButton rotateButton; // determine if IsPressed

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

		public override void OnExitTab(Toolbar newToolbar)
		{
			if (newToolbar is PaintingToolbar && selectedObjects.Count > 0)
			{
				var paintingToolbarBehaviour = (PaintingToolbarBehaviour)newToolbar.toolbarBehaviour;
				paintingToolbarBehaviour.ChangeCurrentPrefab(selectedObjects[0].PrefabName);
				selectedObjects.Clear();
			}
			else selectedObjects.Clear();
		}

		public override void OnUpdate()
		{
			screenMousePos = Globals.input.GetMouseScreenPosition();

			if (Globals.input.GetMouseDown(0) && Globals.input.GetKey(Keys.LeftShift) && !Globals.myraDesktop.IsMouseOverGUI && !editorControls.isDragging && !EditorControls.disableDragMove)
			{
				selectDragging = true;
				startSelectionPos = Globals.input.GetMouseScreenPosition();
			}

			if (Globals.input.GetKeyDown(Keys.Escape))
			{
				editWindow?.Close();
			}

			if (Globals.input.GetKeyDown(Keys.Q) && !EditorControls.isTextBoxFocused)
				SelectionOverlapWindow();

			if (Globals.input.GetKeyDown(Keys.R) && !EditorControls.isTextBoxFocused)
				rotateButton.IsPressed = !rotateButton.IsPressed;


			// selecting one thing by clicking
			if (Globals.input.GetMouseUp(0) && !selectDragging && !editorControls.isDragging && !Globals.myraDesktop.IsMouseOverGUI && !EditorControls.isMouseOverGUIPreviousFrame)
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

			if (Globals.input.GetKey(Keys.LeftControl) && Globals.input.GetKeyDown(Keys.D) && !EditorControls.isTextBoxFocused) Duplicate();
			if (Globals.input.GetKey(Keys.LeftAlt) && Globals.input.GetKeyDown(Keys.D) && !EditorControls.isTextBoxFocused) selectedObjects.Clear();
			if ((Globals.input.GetKeyDown(Keys.Delete) || Globals.input.GetMouseDown(1)) && !EditorControls.isTextBoxFocused && !Globals.myraDesktop.IsMouseOverGUI)
			{
				RemoveSelection();
				if (selectionOverlapWindow != null) selectionOverlapWindow.Close();
			}

			if (!Globals.input.GetKey(Keys.LeftControl) && !EditorControls.isTextBoxFocused)
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

			if (Globals.input.GetKeyDown(Keys.E) && !EditorControls.isTextBoxFocused) Edit();

			EditorControls.disableDragMove = rotateButton.IsPressed && selectedObjects.Count > 0;
			if (rotateButton.IsPressed && editorControls.isDragging && Globals.input.GetMouse(0) && !Globals.input.GetKey(Keys.LeftControl))
			{
				if (Globals.input.GetKey(Keys.LeftShift) && MathF.Abs(Globals.input.mouseX) > .1f)
				{
					var amount = Globals.input.mouseX > 0 ? 22.5f : -22.5f;
					RotateSelectedAround(amount);
				}
				else if (!Globals.input.GetKey(Keys.LeftControl))
				{
					RotateSelectedAround(Globals.input.mouseX);
				}
			}
			if (Globals.input.GetKey(Keys.LeftControl) && Globals.input.GetKey(Keys.R))
			{
				ResetRotation();
			}

			EditorLabelManager.Add("SelectedObjectCount", new Label { Text = "Selected Objects - " + selectedObjects.Count });
		}

		private void RotateSelectedAround(float amount)
		{
			if (selectedObjects.Count == 0) return;

			var origin = new Vector2(selectedObjects.Average(x => x.Position.X), selectedObjects.Average(x => x.Position.Y));
			foreach (var item in selectedObjects)
			{
				item.Position = item.Position.RotateAround(origin, amount);
				item.Rotation += amount;
			}
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
				overlapButton.Click += (s, a) => { selectionOverlapWindow.Close(); selectionOverlapWindow = null; };
				overlapButton.MouseLeft += (s, e) => { if (selectedObjects.Contains(overlap)) selectedObjects.Remove(overlap); };

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
					position = item.Position,
					rotation = item.Rotation,
					scale = item.Scale,
				}, item.customData);
				selectedObjects.Add(spawnedClipboard);
			}
		}

		public void ResetRotation()
		{
			selectedObjects.ForEach(x => x.Rotation = 0);
			rotateButton.IsPressed = false;
		}

		public void ResetCamera()
		{
			Globals.camera.SetPosition(Vector2.Zero);
			Globals.camera.SetScale(1);
		}

		public void Edit()
		{
			if (selectedObjects.Count == 0) return;

			var obj = selectedObjects[0];
			if (obj == null) return;

			PropertyGrid propertyGrid = new PropertyGrid
			{
				Object = obj,
				Width = 350
			};

			editWindow = new Window
			{
				Title = "Object Editor",
				Content = propertyGrid
			};

			editWindow.Show(Globals.myraDesktop);
		}

		public override void OnDraw(SpriteBatch sb)
		{
			if (selectionOverlapWindow != null)
				sb.DrawCircle(new CircleF(overlapsVisualPos.ToPoint(), 20), 5, Color.Red, 5, 1);

			foreach (var selected in selectedObjects)
				sb.DrawRectangleRotation(selected.EditorPlacedSprite.rect.OffsetByHalf(), Color.Blue, 5, selected.Rotation, .99f);

			if (selectDragging)
			{
				var difference = startSelectionPos - screenMousePos;
				selectionRectangle = new Rectangle((int)startSelectionPos.X, (int)startSelectionPos.Y, (int)-difference.X, (int)-difference.Y);

				sb.DrawRectangle(selectionRectangle.AllowNegativeSize(), Color.Red, 10, 1);
			}
		}
	}
}