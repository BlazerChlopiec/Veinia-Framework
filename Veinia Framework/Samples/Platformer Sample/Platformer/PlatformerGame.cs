﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Veinia.Platformer
{
	public class PlatformerGame : Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		VeiniaInitializer veinia;


		public PlatformerGame()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;

			veinia = new VeiniaInitializer(this, graphics);
			Globals.fps.vSync(true);
			Globals.fps.ChangeFps(int.MaxValue);
		}

		protected override void Initialize()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			var prefabs = new PlatformerPrefabs();

			veinia.Initialize(GraphicsDevice, Content, Window,
				unitSize: 100, collisionRectScreenSize: 10000, new Vector2(1280, 720), prefabs, fullscreen: false);

			Globals.loader.Load(new Level1(prefabs, "PlatformerLevel1.veinia"));

			base.Initialize();
		}

		protected override void LoadContent() { }

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			veinia.Update(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.White * .05f);

			veinia.Draw(spriteBatch);

			base.Draw(gameTime);
		}
	}
}