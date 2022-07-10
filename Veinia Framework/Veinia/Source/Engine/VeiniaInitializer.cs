using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Tweening;
using MonoGame.Extended.ViewportAdapters;
using Veinia;

namespace Veinia
{
	public class VeiniaInitializer
	{
		Title title;

		public void Initialize(Game game, GraphicsDeviceManager graphicsManager, GraphicsDevice graphicsDevice, ContentManager content, GameWindow window,
			int pixelsPerUnit, Vector2 gameSize, bool fullscreen = false)
		{
			Transform.pixelsPerUnit = pixelsPerUnit;

			Globals.graphicsManager = graphicsManager;
			Globals.graphicsDevice = graphicsDevice;
			Globals.content = content;
			Globals.fps = new FPS(game);
			Globals.tweener = new Tweener();
			Globals.input = new Input();
			Globals.screen = new Screen((int)gameSize.X, (int)gameSize.Y); // window size
			if (fullscreen) Globals.graphicsManager.ToggleFullScreen();
			Globals.loader = new Loader();
			Globals.camera = new OrthographicCamera(new BoxingViewportAdapter(window, graphicsDevice, 1920, 1080));
			Globals.collisionComponent = new CollisionComponent(new RectangleF(-250000, -250000, 500000, 500000));

			title = new Title(window);
		}

		public void Update(GameTime gameTime)
		{
			Time.CalculateDelta(gameTime);
			Globals.fps.CalculateFps(gameTime);

			NextFrame.Update();

			Globals.input.Update();

			if (!Time.stop)
			{
				Globals.tweener.Update(Time.deltaTime);
				if (Globals.loader.currentLevel != null) Globals.loader.currentLevel.Update();
				Globals.collisionComponent.Update(gameTime);
			}

			Title.Add(Globals.fps.currentFps, " FPS", 0);
			Title.Add(Globals.fps.isVSync, " - vSync", 1);
			title.Update();


			//useful hotkeys
			if (Globals.input.GetKeyDown(Keys.Space))
			{
				Globals.loader.Unload();
			}

			if (Globals.input.GetKeyDown(Keys.F))
			{
				Collider.showHitboxes = !Collider.showHitboxes;
			}
			//
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: Globals.camera.GetViewMatrix());

			if (Globals.loader.currentLevel != null) Globals.loader.currentLevel.Draw(spriteBatch);

			spriteBatch.End();
		}
	}
}