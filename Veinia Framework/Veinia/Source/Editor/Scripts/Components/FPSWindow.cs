using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;

namespace VeiniaFramework.Editor
{
	public class FPSWindow : Component
	{
		Window window;

		public override void Initialize()
		{
			var panel = new Panel();

			window = new Window
			{
				Title = $"FPS - {Globals.fps.currentFps}",
				Content = panel,
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Center
			};
			window.DragDirection = DragDirection.None;
			window.CloseButton.RemoveFromParent();
			window.Height = 130;
			window.Width = 115;

			var fpsInputBox = new TextBox { Enabled = Globals.fps.isFixedTimestep };

			var applyButton = new TextButton { Text = "Apply", Top = 30, Enabled = Globals.fps.isFixedTimestep };

			applyButton.Click += (o, a) =>
			{
				int fpsStringParse;
				if (int.TryParse(fpsInputBox.Text, out fpsStringParse))
					Globals.fps.ChangeFps(fpsStringParse);

				fpsInputBox.Text = string.Empty;
			};

			var set60Button = new TextButton { Text = "Set 60", Top = 30, Left = 55, Enabled = Globals.fps.isFixedTimestep };
			set60Button.Click += (o, a) =>
			{
				Globals.fps.ChangeFps(60);
				fpsInputBox.Text = string.Empty;
			};

			var vSync = new CheckBox { Top = 75, Text = " vSync", IsChecked = Globals.fps.isVSync, Enabled = !Globals.fps.isFixedTimestep, Opacity = Globals.fps.isFixedTimestep ? .3f : 1f };
			vSync.TouchDown += (o, e) =>
			{
				Globals.fps.vSync(!vSync.IsChecked);
			};

			var fixedTimestep = new CheckBox { Top = 55, Text = " Fixed", IsChecked = Globals.fps.isFixedTimestep };
			fixedTimestep.TouchDown += (o, e) =>
			{
				Globals.fps.FixedTimestep(!fixedTimestep.IsChecked);
				applyButton.Enabled = !fixedTimestep.IsChecked;
				fpsInputBox.Enabled = !fixedTimestep.IsChecked;
				set60Button.Enabled = !fixedTimestep.IsChecked;

				vSync.Enabled = fixedTimestep.IsChecked;
				vSync.IsChecked = false;
				Globals.fps.vSync(false);
				vSync.Opacity = Globals.fps.isFixedTimestep ? .3f : 1f;
			};

			panel.Widgets.Add(fpsInputBox);
			panel.Widgets.Add(applyButton);
			panel.Widgets.Add(set60Button);
			panel.Widgets.Add(vSync);
			panel.Widgets.Add(fixedTimestep);

			window.Show(Globals.myraDesktop, Point.Zero);
		}

		public override void Update() => window.Title = $"FPS - {Globals.fps.currentFps}";
	}
}