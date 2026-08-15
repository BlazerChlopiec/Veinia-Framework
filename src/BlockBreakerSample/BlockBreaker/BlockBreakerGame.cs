using Apos.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VeiniaFramework.Samples.BlockBreaker
{
	public class BlockBreakerGame : Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		Veinia veinia;


		public BlockBreakerGame()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = false;
			Window.AllowUserResizing = true;

			veinia = new Veinia(this, graphics);
			IsFixedTimeStep = false;
		}

		protected override void Initialize()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			var prefabs = new BlockBreakerPrefabs();
			var screen = new Screen(1280, 720, false);

			veinia.Initialize(GraphicsDevice, Content, Window, screen,
					unitSize: 100, Vector2.Zero, prefabs);

			Globals.camera = new Camera(new BoundingViewport(GraphicsDevice, Window, 1920, 1080));

			Globals.loader.storedLevels.Add(new StoredLevel("Level1.veinia", typeof(LevelTemplate)));
			Globals.loader.StoredLevelLoad(index: 0);

			base.Initialize();
		}

		protected override void LoadContent() { }

		protected override void Update(GameTime gameTime)
		{
			veinia.Update(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			Globals.camera.SetViewport();
			GraphicsDevice.Clear(Color.Black);
			veinia.Draw(spriteBatch);
			Globals.camera.ResetViewport();

			base.Draw(gameTime);
		}
	}
}