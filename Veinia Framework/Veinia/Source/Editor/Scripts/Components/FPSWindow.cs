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

			var fpsInputBox = new TextBox { Enabled = Globals.fps.fixedTimestep };

			var applyButton = new TextButton { Text = "Apply", Top = 30, Enabled = Globals.fps.fixedTimestep };

			applyButton.Click += (o, a) =>
			{
				int fpsStringParse;
				if (int.TryParse(fpsInputBox.Text, out fpsStringParse))
					Globals.fps.ChangeFps(fpsStringParse);

				fpsInputBox.Text = string.Empty;
			};

			var set60Button = new TextButton { Text = "Set 60", Top = 30, Left = 55, Enabled = Globals.fps.fixedTimestep };
			set60Button.Click += (o, a) =>
			{
				Globals.fps.ChangeFps(60);
				fpsInputBox.Text = string.Empty;
			};

			var vSyncCheckbox = new CheckBox { Top = 75, Text = " vSync", IsChecked = Globals.fps.vSync, Enabled = !Globals.fps.fixedTimestep, Opacity = UpdateVSyncOpacity() };
			vSyncCheckbox.TouchDown += (o, e) =>
			{
				Globals.fps.vSync = !vSyncCheckbox.IsChecked;
			};

			var fixedCheckbox = new CheckBox { Top = 55, Text = " Fixed", IsChecked = Globals.fps.fixedTimestep };
			fixedCheckbox.TouchDown += (o, e) =>
			{
				// test if fixedTimestep is getting checked right now
				var getsChecked = !fixedCheckbox.IsChecked;

				Globals.fps.fixedTimestep = getsChecked;
				applyButton.Enabled = getsChecked;
				fpsInputBox.Enabled = getsChecked;
				set60Button.Enabled = getsChecked;

				// fixedTimestep disables vSync
				if (getsChecked) Globals.fps.vSync = false;
				else Globals.fps.vSync = vSyncCheckbox.IsChecked;

				vSyncCheckbox.Enabled = fixedCheckbox.IsChecked;
				vSyncCheckbox.Opacity = UpdateVSyncOpacity();
			};

			panel.Widgets.Add(fpsInputBox);
			panel.Widgets.Add(applyButton);
			panel.Widgets.Add(set60Button);
			panel.Widgets.Add(vSyncCheckbox);
			panel.Widgets.Add(fixedCheckbox);

			window.Show(Globals.myraDesktop, Point.Zero);
		}

		public override void Update() => window.Title = $"FPS - {Globals.fps.currentFps}";

		private float UpdateVSyncOpacity() => Globals.fps.fixedTimestep ? .3f : 1f;
	}
}