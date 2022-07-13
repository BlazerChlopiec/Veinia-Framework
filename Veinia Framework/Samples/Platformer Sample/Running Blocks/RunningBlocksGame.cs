using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Veinia.RunningBlocks
{
	public class RunningBlocksGame : Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		VeiniaInitializer veinia = new VeiniaInitializer();


		public RunningBlocksGame()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			var prefabs = new RunningBlocksPrefabs();

			veinia.Initialize(this, graphics, GraphicsDevice, Content, Window,
				pixelsPerUnit: 100, new Vector2(1920, 1080), prefabs, fullscreen: true);

			Globals.fps.vSync(false);
			Globals.fps.ChangeFps(int.MaxValue);

			Collider.showHitboxes = true;

			Globals.loader.Load(new Level1(prefabs, "RunningBlocksLevel1.veinia"));

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
			GraphicsDevice.Clear(Color.Black);

			veinia.Draw(spriteBatch);

			base.Draw(gameTime);
		}
	}
}
