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

			displayedToolbarContent = tabControl;

			ShowPrefabsInToolbars();
		}

		private void OnClickPrefab(Prefab prefab)
		{
			paintingToolbarBehaviour.ChangeCurrentPrefab(prefab.PrefabName);
			paintingToolbarBehaviour.CreateNewPreview();
		}


		private void FeedToolbarWithPrefabs()
		{
			for (int i = 0; i < prefabManager.prefabs.Count; i++)
			{
				var prefab = prefabManager.prefabs[i];
				paintingToolbarTabs[prefab.PaintingToolbarTab].Prefabs.Add(prefab);
			}
		}

		private void ShowPrefabsInToolbars()
		{
			int prefabButtonSize = 70;

			foreach (var tab in paintingToolbarTabs)
			{
				foreach (var prefab in tab.Prefabs)
				{
					var sprite = prefab.PrefabGameObject.GetComponent<Sprite>();

					var prefabButton = new ImageButton
					{
						Height = prefabButtonSize,
						Width = prefabButtonSize,
						Top = prefabButtonSize * tab.Prefabs.IndexOf(prefab),
						VerticalAlignment = VerticalAlignment.Top,
						Background = new TextureRegion(sprite.texture.ChangeColor(sprite.color), new Rectangle(0, 0, sprite.texture.Width, sprite.texture.Height)),
					};
					prefabButton.Click += (s, a) => OnClickPrefab(prefab);
					tab.Panel.Widgets.Add(prefabButton);

					if (prefab.ShowLabel)
					{
						var prefabTextOutline = new Label
						{
							Text = prefab.PrefabName,
							Top = prefabButtonSize * tab.Prefabs.IndexOf(prefab),
							TextColor = Color.Black,
							MaxWidth = prefabButtonSize,
						};
						tab.Panel.Widgets.Add(prefabTextOutline);

						var prefabText = new Label
						{
							Text = prefab.PrefabName,
							Top = prefabButtonSize * tab.Prefabs.IndexOf(prefab) - 1,
							Left = -1,
							MaxWidth = prefabButtonSize,
						};
						tab.Panel.Widgets.Add(prefabText);
					}
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
		public List<Prefab> Prefabs = new List<Prefab>();
		public Panel Panel { get; set; } = new Panel();
		public ScrollViewer Scroll { get; set; } = new ScrollViewer();
	}
}
