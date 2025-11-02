using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;

namespace VeiniaFramework.Editor
{
	public class LevelSelector : Component
	{
		public override void Initialize()
		{
			var panel = new Panel();
			var window = new Window
			{
				Title = "Level Selector",
				Content = panel,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
			};
			window.DragDirection = DragDirection.None;
			window.CloseButton.Click += (sender, e) => { RemoveComponent(this); };

			var yMenu = new VerticalMenu { };
			foreach (var level in Globals.loader.storedLevels)
			{
				var item = new MenuItem { Text = level.path };
				item.Selected += (sender, e) => { Globals.loader.DynamicalyLoad(new EditorScene(level.path, level.type)); };
				yMenu.Items.Add(item);
			}
			panel.Widgets.Add(yMenu);

			window.Show(Globals.myraDesktop, Point.Zero);
		}
	}
}
