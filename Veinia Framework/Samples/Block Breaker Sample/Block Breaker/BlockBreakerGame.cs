using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Veinia.Editor;

namespace Veinia.BlockBreaker
{
	public class BlockBreakerGame : Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;
		private BlockBreakerPrefabs prefabs = new BlockBreakerPrefabs();

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

			veinia.Initialize(this, graphics, GraphicsDevice, Content, Window,
				pixelsPerUnit: 100, new Vector2(1920, 1080), fullscreen: true);

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

			if (Globals.input.GetKeyDown(Keys.Tab))
			{
				if (Globals.loader.currentLevel is EditorScene)
				{
					Globals.loader.Load(new Level1(prefabs, "level1.veinia"));
				}
				else
					Globals.loader.Load(new EditorScene("level1.veinia", prefabs));
			}

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