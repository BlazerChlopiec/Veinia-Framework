using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
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
			FeedToolboxWithPrefabs();


			var panel = new Panel();
			var scroll = new ScrollViewer();
			scroll.Content = panel;

			var window = new Window
			{
				Title = "Prefabs",
				Content = scroll,
			};
			window.CloseButton.RemoveFromParent();

			int topOffset = 0;

			foreach (var prefab in toolbarPrefabs)
			{
				var image = new ImageTextButton
				{
					Height = 100,
					Width = 100,
					Text = prefab.PrefabName,
					Top = topOffset + 100 * toolbarPrefabs.IndexOf(prefab),
					VerticalAlignment = VerticalAlignment.Top,
					Background = new TextureRegion(prefab.texture.ChangeColor(prefab.color), new Rectangle(0, 0, prefab.texture.Width, prefab.texture.Height)),
					TextColor = prefab.color.ToNegative(),
				};

				image.Click += (s, a) => OnClickPrefab(prefab);

				panel.Widgets.Add(image);
			}
			panel.Height = toolbarPrefabs.Count * 100 + topOffset;

			window.Show(Globals.myraDesktop, Point.Zero);
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
	}
}
