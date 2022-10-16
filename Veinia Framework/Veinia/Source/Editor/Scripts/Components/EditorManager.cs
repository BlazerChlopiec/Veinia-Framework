using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;

namespace Veinia.Editor
{
	public class EditorManager : Component
	{
		public override void Initialize()
		{
			var editorObjectManager = FindComponentOfType<EditorObjectManager>();
			var editorLoader = FindComponentOfType<EditorLoader>();

			var panel = new Panel();
			var window = new Window
			{
				Title = "Editor Manager",
				Content = panel,
				VerticalAlignment = VerticalAlignment.Bottom,
				HorizontalAlignment = HorizontalAlignment.Center,
			};
			window.CloseButton.RemoveFromParent();
			window.DragDirection = DragDirection.None;
			window.Width = 190;

			var saveButton = new TextButton { Text = "Save" };
			saveButton.Click += (s, e) => editorLoader.Save();
			panel.Widgets.Add(saveButton);

			var loadButton = new TextButton { Text = "Load", Left = 50 };
			loadButton.Click += (s, e) => editorLoader.Load();
			panel.Widgets.Add(loadButton);

			var removeAllButton = new TextButton { Text = "Remove All", Left = 100 };
			removeAllButton.Click += (s, e) => editorObjectManager.RemoveAll();
			panel.Widgets.Add(removeAllButton);

			window.Show(Globals.myraDesktop, Point.Zero);
		}
	}
}