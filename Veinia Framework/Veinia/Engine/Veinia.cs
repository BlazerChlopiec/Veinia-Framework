using Apos.Tweens;
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
		Globals.collisionComponent = new CollisionComponent(new RectangleF(-250000, -250000, 500000, 500000));

		title = new Title(window);
		prefabManager = new PrefabManager();

		Globals.fps.vSync(true, graphicsManager);
		Globals.fps.ChangeFps(60);

		Globals.loader.Load(new PongLevel1("PongLevel1", prefabManager));
	}

	public void Update(GameTime gameTime)
	{
		Time.CalculateDelta(gameTime);
		Globals.fps.CalculateFps(gameTime);

		Globals.input.Update();
		Globals.loader.currentLevel.Update();
		Globals.collisionComponent.Update(gameTime);
		TweenHelper.UpdateSetup(gameTime);

		Title.Add(Globals.fps.currentFps, " FPS", 0);
		Title.Add(Globals.fps.isVSync, " - vSync", 1);
		title.Update();

		Globals.loader.currentLevel.LateUpdate();

		//useful hotkeys
		if (Globals.input.GetKeyDown(Keys.Space))
		{
			//crashes hard :weary:
			Globals.loader.Unload(Globals.loader.currentLevel);
		}

		if (Globals.input.GetKeyDown(Keys.F))
		{
			Globals.showHitboxes = !Globals.showHitboxes;
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