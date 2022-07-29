using GeonBit.UI;
using GeonBit.UI.Animators;
using GeonBit.UI.Entities;
using GeonBit.UI.Utils;
using Microsoft.Xna.Framework;
using System;

namespace Veinia.BlockBreaker
{
	public class UI : Component
	{
		public ProgressBar progressBar;


		public override void Initialize()
		{
			UserInterface.Active.ShowCursor = true;

			progressBar = new ProgressBar(0, (uint)FindComponentsOfType<Tile>().Count, new Vector2(300, 50), Anchor.TopCenter);
			progressBar.Value = FindComponentsOfType<Tile>().Count;
			progressBar.ClickThrough = true;
			progressBar.Caption.Scale = 1.5f;
			progressBar.Caption.TextStyle = FontStyle.Bold;

			UserInterface.Active.AddEntity(progressBar);
		}

		public override void Update()
		{
			var value = MathF.Round(progressBar.GetValueAsPercent() * 100);
			progressBar.Caption.Text = value.ToString() + "%";
		}

		public void ShowWinScreen()
		{
			Time.stop = true;

			var panel = new Panel(new Vector2(300, 300), PanelSkin.Default);

			var congratulations = new RichParagraph("{{GOLD}}Congratulations!",
									Anchor.TopCenter);

			var thanksForPlaying = new Paragraph("Thank you for playing!");


			var horizontalLine = new HorizontalLine();

			var restartButton = new Button("Restart", ButtonSkin.Default, offset: Vector2.UnitY * 20, anchor: Anchor.BottomCenter);
			restartButton.OnClick = (e) => { Globals.loader.Reload(); UserInterface.Active.SetCursor(CursorType.Default); };
			restartButton.OnMouseEnter = (e) => { UserInterface.Active.SetCursor(CursorType.Pointer); };
			restartButton.OnMouseLeave = (e) => { UserInterface.Active.SetCursor(CursorType.Default); };

			panel.AddChild(congratulations);
			panel.AddChild(horizontalLine);
			panel.AddChild(thanksForPlaying);
			panel.AddChild(restartButton);

			UserInterface.Active.AddEntity(panel);
		}

		public void ShowLoseScreen()
		{
			Time.stop = true;

			var panel = new Panel(new Vector2(300, 200), PanelSkin.Default, Anchor.Center);
			var gameOver = new RichParagraph("{{RED}}Game Over!",
									Anchor.TopCenter);

			var horizontalLine = new HorizontalLine();

			var restartButton = new Button("Restart", ButtonSkin.Default, Anchor.BottomCenter, offset: Vector2.UnitY * 20);
			restartButton.OnClick = (e) => { Globals.loader.Reload(); UserInterface.Active.SetCursor(CursorType.Default); };
			restartButton.OnMouseEnter = (e) => { UserInterface.Active.SetCursor(CursorType.Pointer); };
			restartButton.OnMouseLeave = (e) => { UserInterface.Active.SetCursor(CursorType.Default); };

			panel.AddChild(gameOver).AttachAnimator(new FloatUpDownAnimator());
			panel.AddChild(horizontalLine);
			panel.AddChild(restartButton);

			UserInterface.Active.AddEntity(panel);
		}
	}
}
