using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VeiniaFramework.RunningBlocks
{
	public class RunningBlocksGame : Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		Veinia veinia;


		public RunningBlocksGame()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;

			veinia = new Veinia(this, graphics);
			Globals.fps.vSync(true);
			Globals.fps.ChangeFps(int.MaxValue);
		}

		protected override void Initialize()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			var prefabs = new RunningBlocksPrefabs();

			veinia.Initialize(GraphicsDevice, Content, Window,
				unitSize: 100, collisionRectScreenSize: 10000, new Vector2(1920, 1080), prefabs, fullscreen: true);

			Globals.loader.storedLevels.Add(new StoredLevel { storedLevelType = typeof(LevelTemplate), storedLevelPath = "Level1.veinia" });
			Globals.loader.StoredLevelLoad(index: 0);

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
