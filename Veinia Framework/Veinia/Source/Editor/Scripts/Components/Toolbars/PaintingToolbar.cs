using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using System.Collections.Generic;

namespace Veinia.Editor
{
	public class PaintingToolbar : Toolbar
	{
		PrefabManager prefabManager;
		PaintingToolbarBehaviour editorObjectPainter;
		List<ToolbarPrefab> toolbarPrefabs = new List<ToolbarPrefab>();


		public PaintingToolbar(string toolbarName, ToolbarBehaviour toolbarBehaviour, PrefabManager prefabManager) : base(toolbarName, toolbarBehaviour)
		{
			this.prefabManager = prefabManager;
		}

		public override void OnInitialize(GameObject gameObject)
		{
			editorObjectPainter = (PaintingToolbarBehaviour)toolbarBehaviour;

			FeedToolbarWithPrefabs();

			var tabControl = new TabControl { TabSelectorPosition = TabSelectorPosition.Right };
			var panel = new Panel();
			var scroll = new ScrollViewer();
			scroll.Content = panel;
			content = scroll;
			tabControl.Items.Add(new TabItem { Content = scroll, Text = "Tiles" });
			tabControl.Items.Add(new TabItem { Content = scroll, Text = "Deco" });
			content = tabControl;

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
		}

		private void OnClickPrefab(ToolbarPrefab prefab) => editorObjectPainter.ChangeCurrentPrefab(prefab.PrefabName);

		private void FeedToolbarWithPrefabs()
		{
			for (int i = 0; i < prefabManager.prefabs.Count; i++)
			{
				var sprite = prefabManager.prefabs[i].PrefabGameObject.GetComponent<Sprite>();

				toolbarPrefabs.Add(new ToolbarPrefab
				{
					PrefabName = prefabManager.prefabs[i].PrefabName,
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
