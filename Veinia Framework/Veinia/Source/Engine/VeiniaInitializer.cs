using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ViewportAdapters;
using Myra;
using Myra.Graphics2D.UI;
using Veinia.Editor;

namespace Veinia
{
	public class VeiniaInitializer
	{
		Title title;
		PrefabManager prefabManager;
		Game game;

		public VeiniaInitializer(Game game, GraphicsDeviceManager graphicsManager)
		{
			//this one takes effect only in a game's constructor
			this.game = game;
			Globals.graphicsManager = graphicsManager;
			Globals.fps = new FPS(game);
		}

		public void Initialize(GraphicsDevice graphicsDevice, ContentManager content, GameWindow window,
			int pixelsPerUnit, Vector2 gameSize, PrefabManager prefabManager, bool fullscreen = false)
		{
			//MYRA UI
			MyraEnvironment.Game = game;

			Globals.desktop = new Desktop
			{
				HasExternalTextInput = true,
			};
			window.TextInput += (s, a) => Globals.desktop.OnChar(a.Character);
			//

			Transform.pixelsPerUnit = pixelsPerUnit;

			this.prefabManager = prefabManager;


			Globals.window = window;
			Globals.graphicsDevice = graphicsDevice;
			Globals.content = content;
			Globals.screen = new Screen((int)gameSize.X, (int)gameSize.Y); // window size
			if (fullscreen) Globals.graphicsManager.ToggleFullScreen();
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
				if (Globals.loader.currentLevel != null) Globals.loader.currentLevel.LateUpdate();
				Globals.collisionComponent.Update(gameTime);
			}

			Title.Add(Globals.fps.currentFps, " FPS", 0);
			Title.Add(Globals.fps.isVSync, " - vSync", 1);
			title.Update();


#if DEBUG
			if (Globals.input.GetKeyDown(Keys.F))
			{
				Collider.showHitboxes = !Collider.showHitboxes;
			}

			if (Globals.input.GetKeyDown(Keys.Tab))
			{
				if (Globals.loader.currentLevel is EditorScene)
				{
					Globals.loader.Load(Globals.loader.previousLevel);
				}
				else
					Globals.loader.Load(new EditorScene(Globals.loader.currentLevel.editorLevelName, prefabManager));
			}
#endif
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: Globals.camera.GetViewMatrix());

			if (Globals.loader.currentLevel != null) Globals.loader.currentLevel.Draw(spriteBatch);

			spriteBatch.End();

			//UI
			Globals.desktop.Render();
		}
	}
}