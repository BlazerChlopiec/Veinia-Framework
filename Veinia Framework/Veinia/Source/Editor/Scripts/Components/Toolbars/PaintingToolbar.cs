using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using System.Collections.Generic;
using System.Linq;

namespace VeiniaFramework.Editor
{
	public class PaintingToolbar : Toolbar
	{
		PrefabManager prefabManager;
		PaintingToolbarBehaviour paintingToolbarBehaviour;

		List<PaintingToolbarTab> paintingToolbarTabs = new List<PaintingToolbarTab>();


		public PaintingToolbar(string toolbarName, ToolbarBehaviour toolbarBehaviour, PrefabManager prefabManager) : base(toolbarName, toolbarBehaviour) => this.prefabManager = prefabManager;

		public override void OnInitialize(GameObject gameObject)
		{
			paintingToolbarBehaviour = (PaintingToolbarBehaviour)toolbarBehaviour;

			//create needed tabs
			for (int i = 0; i < prefabManager.prefabs.Max(x => x.PaintingToolbarTab) + 1; i++)
			{
				var newTab = new PaintingToolbarTab();
				newTab.Scroll.Content = newTab.Panel;
				paintingToolbarTabs.Add(newTab);
			}
			//

			FeedToolbarWithPrefabs();

			var tabControl = new TabControl { TabSelectorPosition = TabSelectorPosition.Right };
			for (int i = 0; i < paintingToolbarTabs.Count; i++)
				tabControl.Items.Add(new TabItem { Content = paintingToolbarTabs[i].Scroll, Text = i.ToString() });

			finalToolbarContent = tabControl;

			ShowPrefabsInToolbars();
		}

		private void OnClickPrefab(PaintingToolbarPrefab prefab)
		{
			paintingToolbarBehaviour.ChangeCurrentPrefab(prefab.PrefabName);
			paintingToolbarBehaviour.CreateNewPreview();
		}


		private void FeedToolbarWithPrefabs()
		{
			for (int i = 0; i < prefabManager.prefabs.Count; i++)
			{
				var prefab = prefabManager.prefabs[i];
				var sprite = prefab.PrefabGameObject.GetComponent<Sprite>();

				paintingToolbarTabs[prefab.PaintingToolbarTab].Prefabs.Add(new PaintingToolbarPrefab
				{
					PrefabName = prefab.PrefabName,
					Texture = sprite.texture,
					Color = sprite.color,
				});
			}
		}

		private void ShowPrefabsInToolbars()
		{
			int prefabButtonSize = 70;

			foreach (var tab in paintingToolbarTabs)
			{
				foreach (var prefab in tab.Prefabs)
				{
					var prefabButton = new ImageButton
					{
						Height = prefabButtonSize,
						Width = prefabButtonSize,
						Top = prefabButtonSize * tab.Prefabs.IndexOf(prefab),
						VerticalAlignment = VerticalAlignment.Top,
						Background = new TextureRegion(prefab.Texture.ChangeColor(prefab.Color), new Rectangle(0, 0, prefab.Texture.Width, prefab.Texture.Height)),
					};

					prefabButton.Click += (s, a) => OnClickPrefab(prefab);

					tab.Panel.Widgets.Add(prefabButton);
				}
				tab.Panel.Height = tab.Prefabs.Count * prefabButtonSize;
			}
		}
	}

	public class PaintingToolbarPrefab
	{
		public string PrefabName { get; set; }
		public Texture2D Texture { get; set; }
		public Color Color { get; set; }
	}

	public class PaintingToolbarTab
	{
		public List<PaintingToolbarPrefab> Prefabs = new List<PaintingToolbarPrefab>();
		public Panel Panel { get; set; } = new Panel();
		public ScrollViewer Scroll { get; set; } = new ScrollViewer();
	}
}
