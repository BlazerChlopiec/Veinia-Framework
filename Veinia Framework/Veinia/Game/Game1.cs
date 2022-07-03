using Apos.Tweens;
using Humper;
using Humper.Responses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;

public class Game1 : Game
{
	private GraphicsDeviceManager graphics;
	private SpriteBatch spriteBatch;

	FPS fps;
	Title title;
	Prefabs prefab;

	public Game1()
	{
		graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;

		fps = new FPS(this);
		fps.vSync(false, graphics);
		fps.ChangeFps(int.MaxValue);

		Window.AllowUserResizing = true;
	}

	protected override void Initialize()
	{
		// TODO: Add your initialization logic here

		spriteBatch = new SpriteBatch(GraphicsDevice);

		Globals.graphics = graphics;
		Globals.device = GraphicsDevice;
		Globals.content = Content;
		Globals.input = new Input();
		Globals.screen = new Screen(1280, 720); // window size
		Globals.loader = new Loader();
		Globals.fps = fps;
		Globals.camera = new OrthographicCamera(new BoxingViewportAdapter(Window, GraphicsDevice, 1920, 1080));
		Globals.world = new World(100000, 100000);

		title = new Title(Window);
		prefab = new Prefabs();

		Globals.loader.Load(new TestLevel("Test Level", prefab));

		base.Initialize();
	}

	protected override void LoadContent()
	{

	}

	protected override void Update(GameTime gameTime)
	{
		Time.CalculateDelta(gameTime);

		if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			Exit();

		// TODO: Add your update logic here

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

		//Globals.camera.Move(Utils.SafeNormalize(new Vector2(Globals.input.horizontal, Globals.input.vertical)) * 1000f * Globals.camera.Zoom * Time.deltaTime);

		Globals.input.Update();
		Globals.loader.currentLevel.Update();
		title.Update();
		TweenHelper.UpdateSetup(gameTime);

		fps.CalculateFps(gameTime);
		Title.Add(fps.currentFps, " FPS", 0);
		Title.Add(fps.isVSync, " - vSync", 1);

		if (Globals.input.GetKeyDown(Keys.Space))
		{
			//cre3ashes
			//Globals.loader.Unload(Globals.loader.currentLevel);
		}
		if (Globals.input.GetKeyDown(Keys.M))
		{
			if (Globals.loader.currentLevel is PerformanceTestLevel)
				Globals.loader.Load(new TestLevel("Test Level", prefab));
			else
				Globals.loader.Load(new PerformanceTestLevel("Performance Test", prefab));
		}

		if (Globals.input.GetKeyDown(Keys.G))
		{
			if (fps.currentFps == 30)
				fps.ChangeFps(int.MaxValue);
			else
				fps.ChangeFps(30);
		}
		if (Globals.input.GetKeyDown(Keys.H))
		{
			if (fps.currentFps == 120)
				fps.ChangeFps(int.MaxValue);
			else
				fps.ChangeFps(120);
		}
		if (Globals.input.GetKeyDown(Keys.J))
		{
			if (fps.currentFps == 60)
				fps.ChangeFps(int.MaxValue);
			else
				fps.ChangeFps(60);
		}

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.Black);

		spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: Globals.camera.GetViewMatrix());

		Globals.loader.currentLevel.Draw(spriteBatch);

		spriteBatch.End();

		base.Draw(gameTime);
	}
}
