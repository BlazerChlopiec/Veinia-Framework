using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Veinia.Editor
{
	public class Toolbar : Component
	{
		PrefabManager prefabManager;
		EditorObjectManager editorObjectManager;

		List<EditorObject> tooltipPrefabs = new List<EditorObject>();

		Sprite sprite;

		public bool hoveringOver;

		private float scroll;


		public Toolbar(PrefabManager prefabManager)
		{
			this.prefabManager = prefabManager;
		}

		public override void Initialize()
		{
			sprite = GetComponent<Sprite>();
			editorObjectManager = FindComponentOfType<EditorObjectManager>();

			UpdateTransform();
			FeedToolboxWithPrefabs();
		}

		private void FeedToolboxWithPrefabs()
		{
			for (int i = 0; i < prefabManager.prefabs.Count; i++)
			{
				var extractedSpriteGameObject = prefabManager.Find(prefabManager.prefabs[i].prefabName)
												.ExcludeToOnlySpriteComponent(
												new Vector2(transform.position.X,
												-Globals.camera.GetScaleY() + Globals.camera.GetScaleY() / 5 * i));

				extractedSpriteGameObject.GetComponent<Sprite>().layer = 1f;
				tooltipPrefabs.Add(new EditorObject
				{
					EditorPlacedSprite = Instantiate(extractedSpriteGameObject).GetComponent<Sprite>(),
					PrefabName = prefabManager.prefabs[i].prefabName,
				});
			}
		}

		public override void Update()
		{
			UpdateTransform();
			UpdateToolboxPrefabs();
			CheckForIntersections();

			if (hoveringOver)
			{
				scroll += Globals.input.deltaScroll * 20;
				scroll = Math.Clamp(scroll, 0, float.MaxValue);
			}
		}

		private void CheckForIntersections()
		{
			var mousePos = Globals.input.GetMouseScreenPosition();

			if (sprite.rect.OffsetByHalf().Contains(mousePos))
				hoveringOver = true;
			else hoveringOver = false;

			foreach (var tooltipPrefab in tooltipPrefabs)
			{
				if (tooltipPrefab.EditorPlacedSprite.rect.OffsetByHalf().Contains(mousePos))
				{

					if (Globals.input.GetMouseButtonDown(0))
						editorObjectManager.ChangeCurrentPrefab(tooltipPrefab.PrefabName);
				}
			}
		}

		private void UpdateToolboxPrefabs()
		{
			for (int i = 0; i < tooltipPrefabs.Count; i++)
			{
				tooltipPrefabs[i].EditorPlacedSprite.destinationSize = new Vector2(100, 100);

				tooltipPrefabs[i].EditorPlacedSprite.transform.position = new Vector2(transform.position.X,
								transform.position.Y - transform.scale.Y / 2 + tooltipPrefabs[i].EditorPlacedSprite.transform.scale.Y + (Globals.camera.GetScaleY() / 5 * i) - scroll);
			}
		}

		private void UpdateTransform()
		{
			transform.scale = new Vector2(Globals.camera.GetScaleX() / 10, Globals.camera.GetScaleY());
			transform.position = Transform.ScreenToWorldPos(new Vector2(Globals.camera.BoundingRectangle.Left + sprite.rect.Width / 2,
																		Globals.camera.Center.Y));
		}
	}
}
