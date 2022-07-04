using Apos.Tweens;
using Humper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ViewportAdapters;

public class Veinia
{
	Title title;
	PrefabManager prefabManager;


	public void Initialize(Game game, GraphicsDeviceManager graphicsManager, GraphicsDevice graphicsDevice, ContentManager content, GameWindow window)
	{
		Globals.graphicsManager = graphicsManager;
		Globals.graphicsDevice = graphicsDevice;
		Globals.content = content;
		Globals.fps = new FPS(game);
		Globals.input = new Input();
		Globals.screen = new Screen(1280, 720); // window size
		Globals.loader = new Loader();
		Globals.camera = new OrthographicCamera(new BoxingViewportAdapter(window, graphicsDevice, 1920, 1080));
		Globals.world = new World(100000, 100000);
		Globals.collisionComponent = new CollisionComponent(new RectangleF(-250000, -250000, 500000, 500000));

		title = new Title(window);
		prefabManager = new PrefabManager();

		Globals.fps.vSync(false, graphicsManager);
		Globals.fps.ChangeFps(144);

		Globals.loader.Load(new TestLevel("TestLevel", prefabManager));
	}

	public void Update(GameTime gameTime)
	{
		Time.CalculateDelta(gameTime);
		Globals.fps.CalculateFps(gameTime);

		Globals.collisionComponent.Update(gameTime);
		Globals.input.Update();
		Globals.loader.currentLevel.Update();
		TweenHelper.UpdateSetup(gameTime);

		Title.Add(Globals.fps.currentFps, " FPS", 0);
		Title.Add(Globals.fps.isVSync, " - vSync", 1);
		title.Update();


		//useful hotkeys
		if (Globals.input.GetKeyDown(Keys.Space))
		{
			//crashes hard :weary:
			Globals.loader.Unload(Globals.loader.currentLevel);
		}

		if (Globals.input.GetKeyDown(Keys.M))
		{
			if (Globals.loader.currentLevel is PerformanceTestLevel)
				Globals.loader.Load(new TestLevel("Test Level", prefabManager));
			else
				Globals.loader.Load(new PerformanceTestLevel("Performance Test", prefabManager));
		}
		if (Globals.input.GetKeyDown(Keys.F))
		{
			Globals.showHitboxes = !Globals.showHitboxes;
		}
		if (Globals.input.GetKey(Keys.OemMinus))
		{
			Globals.camera.ZoomIn(1f * Time.deltaTime);
		}
		if (Globals.input.GetKey(Keys.OemPlus))
		{
			Globals.camera.ZoomOut(1f * Time.deltaTime);
		}
		//
	}

	public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
	{
		Globals.graphicsDevice.Clear(Color.Black);

		spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: Globals.camera.GetViewMatrix());

		Globals.loader.currentLevel.Draw(spriteBatch);

		spriteBatch.End();
	}
}