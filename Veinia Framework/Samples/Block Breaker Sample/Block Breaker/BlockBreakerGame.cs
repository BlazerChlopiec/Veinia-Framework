﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Veinia.Editor;

namespace Veinia.BlockBreaker
{
	public class BlockBreakerGame : Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		VeiniaInitializer veinia = new VeiniaInitializer();


		public BlockBreakerGame()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;

			Window.AllowUserResizing = true;
		}

		protected override void Initialize()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			var prefabs = new BlockBreakerPrefabs();

			veinia.Initialize(this, graphics, GraphicsDevice, Content, Window,
					pixelsPerUnit: 100, new Vector2(1280, 720), prefabs, fullscreen: false);

			Globals.fps.vSync(true);
			Globals.fps.ChangeFps(int.MaxValue);

			Globals.loader.Load(new Level1(prefabs, "level1.veinia"));


			base.Initialize();
		}

		protected override void LoadContent() { }

		protected override void Update(GameTime gameTime)
		{
			veinia.Update(gameTime);

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			veinia.Draw(spriteBatch);

			base.Draw(gameTime);
		}
	}
}