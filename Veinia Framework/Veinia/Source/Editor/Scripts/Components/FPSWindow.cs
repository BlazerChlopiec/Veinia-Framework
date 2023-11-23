using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;

namespace VeiniaFramework.Editor
{
	public class FPSWindow : Component
	{
		public override void Initialize()
		{
			var panel = new Panel();

			var window = new Window
			{
				Title = "FPS",
				Content = panel,
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Center
			};
			window.DragDirection = DragDirection.None;
			window.CloseButton.RemoveFromParent();
			window.Height = 100;
			window.Width = 120;

			var textBox = new TextBox { };
			var vSync = new CheckBox { Top = 55, Text = " vSync", IsChecked = Globals.fps.isVSync };
			vSync.TouchDown += (o, e) => { Globals.fps.vSync(!vSync.IsChecked); };

			var applyButton = new TextButton { Text = "Apply", Top = 30 };

			int result;
			applyButton.Click += (o, a) =>
			{
				if (int.TryParse(textBox.Text, out result))
					Globals.fps.ChangeFps(result);
			};

			var resetButton = new TextButton { Text = "Reset", Top = 30, Left = 70 };
			resetButton.Click += (o, a) => { Globals.fps.ChangeFps(int.MaxValue); };

			panel.Widgets.Add(textBox);
			panel.Widgets.Add(vSync);
			panel.Widgets.Add(applyButton);
			panel.Widgets.Add(resetButton);

			window.Show(Globals.myraDesktop, Point.Zero);
		}
	}
}