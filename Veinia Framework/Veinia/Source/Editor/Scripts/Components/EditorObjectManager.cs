using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;

namespace VeiniaFramework.Editor
{
	public class EditorObjectManager : Component, IDrawn
	{
		PrefabManager prefabManager;

		public List<EditorObject> editorObjects = new List<EditorObject>();

		public Action<EditorObject> OnSpawn;
		public Action<EditorObject> OnRemove;
		public Action OnRemoveAll;

		bool drawGizmos = true;


		public EditorObjectManager(PrefabManager prefabManager) => this.prefabManager = prefabManager;

		public override void Initialize() => EditorCheckboxes.Add("Draw Gizmos", defaultValue: true, (e, o) => { drawGizmos = true; }, (e, o) => { drawGizmos = false; });

		public EditorObject Spawn(string prefabName, Transform transform, string customData = null)
		{
			IDrawGizmos gizmo = null;
			foreach (var component in prefabManager.Find(prefabName).components)
			{
				if (component is IDrawGizmos) gizmo = (IDrawGizmos)component;
			}

			var extractedSpriteGameObject = prefabManager.Find(prefabName).ExtractComponentToNewGameObject<Sprite>(transform, isStatic: true);

			var newEditorObject = new EditorObject
			{
				PrefabName = prefabName,
				Position = transform.Position,
				Rotation = transform.Rotation,
				Z = transform.Z,
				Scale = transform.Scale,
				customData = customData,
				EditorPlacedSprite = Instantiate(extractedSpriteGameObject).GetComponent<Sprite>(),
				gizmo = gizmo,
			};

			editorObjects.Add(newEditorObject);
			OnSpawn?.Invoke(newEditorObject);

			UpdateObjectCountLabel();


			return newEditorObject;
		}

		public void Remove(EditorObject editorObject)
		{
			if (editorObject == null) return;

			editorObject.EditorPlacedSprite.DestroyGameObject();
			editorObjects.Remove(editorObject);

			OnRemove?.Invoke(editorObject);

			UpdateObjectCountLabel();
		}

		public void RemoveAll()
		{
			foreach (var editorObject in editorObjects.ToArray())
			{
				if (editorObject == null) return;

				editorObject.EditorPlacedSprite.DestroyGameObject();
				editorObjects.Remove(editorObject);
			}

			OnRemoveAll?.Invoke();

			UpdateObjectCountLabel();
		}

		private void UpdateObjectCountLabel() => EditorLabelManager.Add("ObjectCount", new Label { Text = "Object Count - " + editorObjects.Count, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Center });

		public EditorObject PrefabOverlapsWithPoint(Vector2 overlapPoint, string prefabName)
		{
			var overlap = editorObjects.Find(x => x.PrefabName == prefabName && x.EditorPlacedSprite.rect
												   .OffsetByHalf()
												   .Contains(Transform.WorldToScreenPos(overlapPoint)));

			return overlap;
		}

		public EditorObject[] OverlapsWithPoint(Vector2 overlapPoint)
		{
			return editorObjects.FindAll(x => x.EditorPlacedSprite.rect
												   .OffsetByHalf()
												   .Contains(Transform.WorldToScreenPos(overlapPoint))).ToArray();
		}

		public List<EditorObject> GetInsideRectangle(Rectangle rect) => editorObjects.FindAll(x => rect.Intersects(x.EditorPlacedSprite.rect.OffsetByHalf()));

		public void Draw(SpriteBatch sb)
		{
			if (drawGizmos)
				foreach (var item in editorObjects)
				{
					item.gizmo?.DrawGizmos(sb, item);
				}
		}
	}
}