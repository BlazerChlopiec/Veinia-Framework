using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VeiniaFramework.Samples.Physics
{
	public class PhysicsGame : Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		Veinia veinia;


		public PhysicsGame()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			Window.AllowUserResizing = false;

			veinia = new Veinia(this, graphics);
			Globals.fps.FixedTimestep(false);
		}

		protected override void Initialize()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			var prefabs = new PhysicsPrefabs();
			var screen = new Screen(1280, 720, false);

			veinia.Initialize(GraphicsDevice, Content, Window, screen,
					unitSize: 100, Vector2.UnitY * -9.81f, prefabs);

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
