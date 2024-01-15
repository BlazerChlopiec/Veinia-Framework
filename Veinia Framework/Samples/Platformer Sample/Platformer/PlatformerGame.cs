using Apos.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VeiniaFramework.Samples.Platformer
{
	public class PlatformerGame : Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		Veinia veinia;


		public PlatformerGame()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			Window.AllowUserResizing = true;

			veinia = new Veinia(this, graphics);
			Globals.fps.fixedTimestep = false;
		}

		protected override void Initialize()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			var prefabs = new PlatformerPrefabs();
			var screen = new Screen(1280, 720, false);

			veinia.Initialize(GraphicsDevice, Content, Window, screen,
					unitSize: 100, Vector2.UnitY * -20, prefabs);

			Globals.camera = new Camera(new BoundingViewport(GraphicsDevice, Window, 1920, 1080));

			Globals.loader.storedLevels.Add(new StoredLevel { storedLevelType = typeof(Level), storedLevelPath = "Level1.veinia" });
			Globals.loader.StoredLevelLoad(index: 0);

			Time.StopForFrames(5);

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
			Globals.graphicsDevice.Clear(Color.Black); // black bars for the BoundingViewport
			Globals.camera.SetViewport();

			GraphicsDevice.Clear(new Color(5, 36, 12));

			veinia.Draw(spriteBatch, samplerState: SamplerState.PointClamp);
			Globals.camera.ResetViewport();

			base.Draw(gameTime);
		}
	}
}
