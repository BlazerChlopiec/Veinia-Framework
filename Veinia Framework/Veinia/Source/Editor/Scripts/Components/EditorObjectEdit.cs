using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System.Collections.Generic;

namespace Veinia.Editor
{
	public class EditorObjectEdit : ToolbarBehaviour
	{
		EditorObjectManager editorObjectManager;

		public List<EditorObject> selectedObjects = new List<EditorObject>();

		bool isDragging;

		Vector2 startSelectionPos;
		Vector2 screenMousePos;

		Rectangle selectionRectangle;


		public override void OnInitialize()
		{
			base.OnInitialize();

			editorObjectManager = gameObject.level.FindComponentOfType<EditorObjectManager>();
		}

		public override void OnUpdate()
		{
			if (Globals.myraDesktop.IsMouseOverGUI) return;

			screenMousePos = Globals.input.GetMouseScreenPosition();

			if (Globals.input.GetMouseButtonDown(0) && Globals.input.GetKey(Keys.LeftShift))
			{
				isDragging = true;
				startSelectionPos = Globals.input.GetMouseScreenPosition();
			}
			if (Globals.input.GetMouseButtonUp(0) && !isDragging && !gameObject.level.FindComponentOfType<EditorControls>().isDragging)
			{
				selectedObjects.Clear();

				var oneSelection = editorObjectManager.editorObjects.Find(x => x.EditorPlacedSprite.rect.OffsetByHalf().Contains(screenMousePos));
				if (oneSelection != null)
					selectedObjects.Add(oneSelection);
			}
			if (Globals.input.GetMouseButtonUp(0) && isDragging)
			{
				isDragging = false;

				selectedObjects = editorObjectManager.GetInsideRectangle(selectionRectangle.AllowNegativeSize());
			}

			if (Globals.input.GetKeyDown(Keys.W))
				foreach (var item in selectedObjects)
					item.Position += new Vector2(0, 1);

			if (Globals.input.GetKeyDown(Keys.S))
				foreach (var item in selectedObjects)
					item.Position += new Vector2(0, -1);

			if (Globals.input.GetKeyDown(Keys.A))
				foreach (var item in selectedObjects)
					item.Position += new Vector2(-1, 0);

			if (Globals.input.GetKeyDown(Keys.D))
				foreach (var item in selectedObjects)
					item.Position += new Vector2(1, 0);

			if (Globals.input.GetKeyDown(Keys.Delete) || Globals.input.GetMouseButtonDown(1))
				RemoveSelection();
		}

		public void RemoveSelection()
		{
			foreach (var item in selectedObjects.ToArray())
			{
				selectedObjects.Remove(item);
				editorObjectManager.Remove(item);
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
