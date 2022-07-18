using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Properties;
using System;
using System.Collections.Generic;

namespace Veinia.Editor
{
	public class Toolbar : Component
	{
		PrefabManager prefabManager;
		EditorObjectManager editorObjectManager;
		List<ToolbarPrefab> toolbarPrefabs = new List<ToolbarPrefab>();


		public Toolbar(PrefabManager prefabManager) => this.prefabManager = prefabManager;

		public override void Initialize()
		{
			editorObjectManager = FindComponentOfType<EditorObjectManager>();

			var panel = new Panel();
			var scroll = new ScrollViewer();
			scroll.Content = panel;

			var window = new Window
			{
				Title = "Prefabs",
				Content = scroll,
			};

			FeedToolboxWithPrefabs();

			foreach (var prefab in toolbarPrefabs)
			{
				prefab.image = new Image
				{
					MaxHeight = 100,
					MaxWidth = 100,
					Top = 100 * toolbarPrefabs.IndexOf(prefab),
					ResizeMode = ImageResizeMode.Stretch,
					VerticalAlignment = VerticalAlignment.Top,
					Color = prefab.color,
					Renderable = new TextureRegion(prefab.texture, new Rectangle(0, 0, prefab.texture.Width, prefab.texture.Height)),
				};
				prefab.image.TouchDown += (s, a) => OnClickPrefab(prefab);

				panel.Widgets.Add(prefab.image);
			}
			panel.Height = toolbarPrefabs.Count * 100;

			window.Show(Globals.desktop);
		}

		private void OnClickPrefab(ToolbarPrefab prefab) => editorObjectManager.ChangeCurrentPrefab(prefab.PrefabName);

		private void FeedToolboxWithPrefabs()
		{
			for (int i = 0; i < prefabManager.prefabs.Count; i++)
			{
				var sprite = prefabManager.prefabs[i].prefabGameObject.GetComponent<Sprite>();

				toolbarPrefabs.Add(new ToolbarPrefab
				{
					PrefabName = prefabManager.prefabs[i].prefabName,
					texture = sprite.texture,
					color = sprite.color,
				});
			}
		}
	}

	public class ToolbarPrefab
	{
		public string PrefabName { get; set; }
		public Texture2D texture { get; set; }
		public Color color { get; set; }
		public Image image;
	}
}
