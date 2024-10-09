using GeonBit.UI;
using GeonBit.UI.Animators;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace VeiniaFramework.Samples.BlockBreaker
{
	public class UI : Component
	{
		public ProgressBar progressBar;
		public Button configButton;


		public override void Initialize()
		{
			UserInterface.Active.ShowCursor = true;

			progressBar = new ProgressBar(0, (uint)FindComponentsOfType<Tile>().Count, new Vector2(300, 50), Anchor.TopCenter);
			progressBar.Value = FindComponentsOfType<Tile>().Count;
			progressBar.ClickThrough = true;
			progressBar.Caption.Scale = 1.5f;
			progressBar.Caption.TextStyle = FontStyle.Bold;

			configButton = new Button("Config", ButtonSkin.Default, Anchor.TopLeft, offset: new Vector2(20, 20), size: new Vector2(150, 50));
			configButton.OnClick = (e) => { ShowConfigScreen(); };
			configButton.OnMouseEnter = (e) => { UserInterface.Active.SetCursor(CursorType.Pointer); };
			configButton.OnMouseLeave = (e) => { UserInterface.Active.SetCursor(CursorType.Default); };

			UserInterface.Active.AddEntity(progressBar);
			UserInterface.Active.AddEntity(configButton);
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

			var restartButton = new Button("Restart", ButtonSkin.Default, offset: Vector2.UnitY * 20, anchor: Anchor.BottomCenter);
			restartButton.OnClick = (e) =>
			{
				UserInterface.Active.SetCursor(CursorType.Default);
				restartButton.Locked = true;

				SpawnTransition();
			};

			restartButton.OnMouseEnter = (e) => { UserInterface.Active.SetCursor(CursorType.Pointer); };
			restartButton.OnMouseLeave = (e) => { UserInterface.Active.SetCursor(CursorType.Default); };

			panel.AddChild(congratulations);
			panel.AddChild(thanksForPlaying);
			panel.AddChild(restartButton);

			UserInterface.Active.AddEntity(panel);
		}

		public void ShowLoseScreen()
		{
			Time.stop = true;

			var panel = new Panel(new Vector2(300, 200), PanelSkin.Default, Anchor.Center);
			var gameOver = new RichParagraph("{{RED}}Game Over!");

			var restartButton = new Button("Restart", ButtonSkin.Default, Anchor.BottomCenter, offset: Vector2.UnitY * 20);
			restartButton.OnClick = (e) =>
			{
				UserInterface.Active.SetCursor(CursorType.Default);

				SpawnTransition();
			};
			restartButton.OnMouseEnter = (e) => { UserInterface.Active.SetCursor(CursorType.Pointer); };
			restartButton.OnMouseLeave = (e) => { UserInterface.Active.SetCursor(CursorType.Default); };

			panel.AddChild(gameOver).AttachAnimator(new FloatUpDownAnimator());
			panel.AddChild(restartButton);

			UserInterface.Active.AddEntity(panel);
		}


		public void ShowConfigScreen()
		{
			Time.stop = true;

			configButton.Enabled = false;

			var ball = FindComponentOfType<Ball>();

			var panel = new Panel(new Vector2(350, 250), PanelSkin.Default, Anchor.Center);
			var config = new Paragraph("Configuration",
									Anchor.TopCenter);


			var sliderName = new Paragraph("Ball Speed");
			var slider = new Slider(0, 30, SliderSkin.Default);
			slider.OnValueChange = (e) => { sliderName.Text = "Ball Speed: " + slider.Value; ball.speed = slider.Value; };
			slider.Value = (int)ball.speed;

			var resumeButton = new Button("Resume", ButtonSkin.Default, Anchor.BottomCenter, offset: Vector2.UnitY * 20);
			resumeButton.OnClick = (e) => { UserInterface.Active.RemoveEntity(panel); Time.stop = false; configButton.Enabled = true; UserInterface.Active.SetCursor(CursorType.Default); };
			resumeButton.OnMouseEnter = (e) => { UserInterface.Active.SetCursor(CursorType.Pointer); };
			resumeButton.OnMouseLeave = (e) => { UserInterface.Active.SetCursor(CursorType.Default); };

			panel.AddChild(config);
			panel.AddChild(sliderName);
			panel.AddChild(slider);
			panel.AddChild(resumeButton);

			UserInterface.Active.AddEntity(panel);
		}

		private void SpawnTransition()
		{
			GameObject transition = Instantiate(
				new Transform(Vector2.Zero),
				new List<Component>
				{
					new Transition("Sprites/Transition")
				}, isStatic: false, dontDestroyOnLoad: true);
		}
	}
}
