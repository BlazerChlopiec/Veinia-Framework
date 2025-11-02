using Microsoft.Xna.Framework;
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
		public TextButton freeMoveButton; // determine if IsPressed

		Vector2 startSelectionPos;
		Vector2 screenMousePos;
		Vector2 filterSelectionPoint;

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
				filterSelectionWindow?.Close();
			}

			if ((Globals.input.GetMouseDown(2) || Globals.input.GetKeyDown(Keys.C)) && !EditorControls.isTextBoxFocused && !Globals.myraDesktop.IsMouseOverGUI)
			{
				filterSelectionPoint = Globals.input.GetMouseScreenPosition();
				FilterSelection();
			}

			if (Globals.input.GetKeyDown(Keys.R) && !EditorControls.isTextBoxFocused)
				rotateButton.IsPressed = !rotateButton.IsPressed;

			if (Globals.input.GetKeyDown(Keys.F) && !EditorControls.isTextBoxFocused) freeMoveButton.IsPressed = !freeMoveButton.IsPressed;
			if (freeMoveButton.IsPressed)
			{
				FreeMove();
				if (Globals.input.GetMouseUp(0)) freeMoveButton.IsPressed = false;
			}


			// selecting one thing by clicking
			if (Globals.input.GetMouseUp(0) && !selectDragging && !editorControls.isDragging && !Globals.myraDesktop.IsMouseOverGUI && !EditorControls.isMouseOverGUIPreviousFrame)
			{
				selectedObjects.Clear();

				filterSelectionWindow?.Close();
				if (filterSelectionWindow == null) filterSelectionPoint = Globals.input.GetMouseScreenPosition();

				var oneSelection = editorObjectManager.editorObjects.Find(x => x.EditorPlacedSprite.rect.OffsetByHalf().Contains(screenMousePos));
				if (oneSelection != null)
				{
					editWindow?.Close();

					selectedObjects.Add(oneSelection);
				}
			}
			// mouse up when dragging
			if (Globals.input.GetMouseUp(0) && selectDragging)
			{
				selectDragging = false;

				foreach (var item in editorObjectManager.GetInsideRectangle(selectionRectangle.AllowNegativeSize()))
				{
					if (!selectedObjects.Contains(item))
					{
						editWindow?.Close();
						filterSelectionWindow?.Close();
						selectedObjects.Add(item);
					}
				}
			}

			if (Globals.input.GetKey(Keys.LeftControl) && Globals.input.GetKeyDown(Keys.D) && !EditorControls.isTextBoxFocused) Duplicate();
			if (Globals.input.GetKey(Keys.LeftAlt) && Globals.input.GetKeyDown(Keys.D) && !EditorControls.isTextBoxFocused) selectedObjects.Clear();
			if ((Globals.input.GetKeyDown(Keys.Delete) || Globals.input.GetMouseDown(1)) && !EditorControls.isTextBoxFocused && !Globals.myraDesktop.IsMouseOverGUI)
			{
				RemoveSelection();
				if (filterSelectionWindow != null) filterSelectionWindow.Close();
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

			if (Globals.input.GetKeyDown(Keys.E) && !EditorControls.isTextBoxFocused)
			{
				Edit();
			}

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

			if (Globals.input.GetKeyDown(Keys.Q) && selectedObjects.Count > 0)
			{
				RotateSelectedAround(-45);
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

		Window filterSelectionWindow;
		public void FilterSelection()
		{
			var panel = new Panel();
			var overlaps = editorObjectManager.OverlapsWithPoint(Transform.ScreenToWorldPos(filterSelectionPoint)).ToList();

			editWindow?.Close();

			selectedObjects.Clear();

			if (filterSelectionWindow != null) filterSelectionWindow.Close();

			filterSelectionWindow = new Window
			{
				Title = "Overlaps",
				Content = panel,
			};

			filterSelectionWindow.Height = 35 + 70 * overlaps.Count;
			filterSelectionWindow.Width = 100;
			filterSelectionWindow.Closed += delegate
			{
				filterSelectionWindow = null;
			};

			int overlapButtonSize = 70;

			foreach (var overlap in overlaps)
			{
				var tex = overlap.EditorPlacedSprite.texture;
				var overlapButton = new ImageButton
				{
					Height = overlapButtonSize,
					Width = overlapButtonSize,
					Rotation = overlap.Rotation,
					Top = overlapButtonSize * overlaps.IndexOf(overlap),
					VerticalAlignment = VerticalAlignment.Top,
					Background = new TextureRegion(tex.ChangeColor(overlap.EditorPlacedSprite.color), new Rectangle(0, 0, tex.Width, tex.Height)),
				};

				overlapButton.MouseEntered += (s, e) => { selectedObjects.Clear(); selectedObjects.Add(overlap); };
				overlapButton.Click += (s, a) => { filterSelectionWindow.Close(); };

				panel.Widgets.Add(overlapButton);
			}
			panel.Height = overlaps.Count * overlapButtonSize;

			filterSelectionWindow.Show(Globals.myraDesktop);
		}

		public void RemoveSelection()
		{
			foreach (var item in selectedObjects.ToArray())
			{
				selectedObjects.Remove(item);
				editorObjectManager.Remove(item);
			}
			editWindow?.Close();
			filterSelectionWindow?.Close();
		}

		public void Duplicate()
		{
			clipboard = selectedObjects.ToArray();
			selectedObjects.Clear();

			editWindow?.Close();
			filterSelectionWindow?.Close();

			foreach (var item in clipboard)
			{
				var spawnedClipboard = editorObjectManager.Spawn(item);
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
			Globals.camera.Scale = 1;
		}

		public void Edit()
		{
			if (selectedObjects.Count == 1)
				EditSingle();
			else if (selectedObjects.Count > 1)
				EditMultiple();
		}

		public void EditSingle()
		{
			if (editWindow != null || filterSelectionWindow != null) return;

			var obj = selectedObjects[0];
			if (obj == null) return;

			editWindow = Globals.myraDesktop.MakeEditWindow(obj);

			editWindow.Closed += delegate
			{
				editWindow = null;
			};
		}

		public void EditMultiple()
		{
			if (editWindow != null || filterSelectionWindow != null) return;

			var data = new MultipleEditData();
			var tolerance = 0.0001f;

			var first = selectedObjects[0];

			// prefabName simillarity
			if (selectedObjects.All(o => o.PrefabName == first.PrefabName))
				data.PrefabName = first.PrefabName;

			// position simillarity
			if (selectedObjects.All(o => Math.Abs(o.Position.X - first.Position.X) < tolerance))
				data.Position.X = first.Position.X;
			if (selectedObjects.All(o => Math.Abs(o.Position.Y - first.Position.Y) < tolerance))
				data.Position.Y = first.Position.Y;

			// z simillarity
			if (selectedObjects.All(o => Math.Abs(o.Z - first.Z) < tolerance))
				data.Z = first.Z;

			// rotation simillarity
			if (selectedObjects.All(o => Math.Abs(o.Rotation - first.Rotation) < tolerance))
				data.Rotation = first.Rotation;

			// scale simillarity
			if (selectedObjects.All(o => Math.Abs(o.Scale.X - first.Scale.X) < tolerance))
				data.Scale.X = first.Scale.X;
			if (selectedObjects.All(o => Math.Abs(o.Scale.Y - first.Scale.Y) < tolerance))
				data.Scale.Y = first.Scale.Y;

			// customData simillarity
			if (selectedObjects.All(o => o.customData == first.customData))
				data.customData = first.customData;


			editWindow = Globals.myraDesktop.MakeEditWindow(data, "Multiple Object Editor");

			PropertyGrid grid = (PropertyGrid)editWindow.Content;

			grid.PropertyChanged += delegate
			{
				foreach (var obj in selectedObjects)
				{
					obj.PrefabName = data.PrefabName == "<mixed>" ? obj.PrefabName : data.PrefabName;

					obj.Position = new Vector2(float.IsNaN(data.Position.X) ? obj.Position.X : data.Position.X,
											   float.IsNaN(data.Position.Y) ? obj.Position.Y : data.Position.Y);

					obj.Z = float.IsNaN(data.Z) ? obj.Z : data.Z;

					obj.Rotation = float.IsNaN(data.Rotation) ? obj.Rotation : data.Rotation;

					obj.Scale = new Vector2(float.IsNaN(data.Scale.X) ? obj.Scale.X : data.Scale.X,
											float.IsNaN(data.Scale.Y) ? obj.Scale.Y : data.Scale.Y);

					obj.customData = data.customData == "<mixed>" ? obj.customData : data.customData;
				}
			};

			editWindow.Closed += delegate
			{
				editWindow = null;
			};
		}

		public void FreeMove()
		{
			if (selectedObjects.Count == 1)
			{
				selectedObjects[0].Position = Globals.input.GetMouseWorldPosition();
			}
		}

		public override void OnDraw(SpriteBatch sb)
		{
			if (filterSelectionWindow != null)
			{
				sb.VeiniaCircle(gameObject.level, filterSelectionPoint, Color.Red, radius: .2f * Globals.camera.Scale, sides: 20, thickness: 10 * Globals.camera.Scale);
			}


			foreach (var selected in selectedObjects)
			{
				sb.VeiniaRectangleRotated(gameObject.level, selected.EditorPlacedSprite.rect.OffsetByHalf(), Color.Blue, thickness: 4 * Globals.camera.Scale, selected.Rotation);
			}


			if (selectDragging)
			{
				var difference = startSelectionPos - screenMousePos;
				selectionRectangle = new Rectangle((int)startSelectionPos.X, (int)startSelectionPos.Y, (int)-difference.X, (int)-difference.Y);

				sb.VeiniaRectangle(gameObject.level, selectionRectangle.AllowNegativeSize(), Color.Red, thickness: 10);
			}
		}
	}
}

public class MultipleEditData
{
	public Vector2 Position = new Vector2(float.NaN, float.NaN);
	public Vector2 Scale = new Vector2(float.NaN, float.NaN);
	public float Rotation = float.NaN;
	public string customData = "<mixed>";
	public string PrefabName = "<mixed>";
	public float Z = float.NaN;
}