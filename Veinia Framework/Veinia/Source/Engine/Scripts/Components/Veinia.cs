using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ViewportAdapters;
using Myra;
using Myra.Graphics2D.UI;
using VeiniaFramework.Editor;

namespace VeiniaFramework
{
	public class Veinia
	{
		Title title;
		Game game;


		public Veinia(Game game, GraphicsDeviceManager graphicsManager)
		{
			//this one takes effect only in a game's constructor
			this.game = game;
			Globals.graphicsManager = graphicsManager;
			Globals.fps = new FPS(game);
		}

		public void Initialize(GraphicsDevice graphicsDevice, ContentManager content, GameWindow window,
			int unitSize, float collisionRectScreenSize, Vector2 gameSize, PrefabManager prefabManager = null, bool fullscreen = false)
		{
			#region Veinia
			Transform.unitSize = unitSize;

			Globals.loader = new Loader(prefabManager);
			Globals.graphicsDevice = graphicsDevice;
			Globals.content = content;
			Globals.screen = new Screen((int)gameSize.X, (int)gameSize.Y); // window size
			Globals.viewportAdapter = new BoxingViewportAdapter(window, graphicsDevice, 1920, 1080);
			if (fullscreen) Globals.graphicsManager.ToggleFullScreen();
			Globals.camera = new OrthographicCamera(Globals.viewportAdapter);
			Globals.collisionComponent = new CollisionComponent(
										 new RectangleF(-collisionRectScreenSize, -collisionRectScreenSize,
														 collisionRectScreenSize * 2, collisionRectScreenSize * 2));
			title = new Title(window);
			#endregion

			#region GeonBit.UI
			UserInterface.Initialize(content, BuiltinThemes.editor);
			UserInterface.Active.ShowCursor = false;
			#endregion

			#region Myra.UI	
			MyraEnvironment.Game = game;
			Globals.myraDesktop = new Desktop
			{
				Opacity = .95f,
				HasExternalTextInput = true,
			};
			Globals.myraDesktop.MouseInfoGetter = () =>
			{
				MouseInfo info = Globals.myraDesktop.DefaultMouseInfoGetter();
				info.Position = (Mouse.GetState().Position.ToVector2() - new Vector2(Globals.viewportAdapter.Viewport.X, Globals.viewportAdapter.Viewport.Y)).ToPoint();
				return info;
			};

			window.TextInput += (s, a) => Globals.myraDesktop.OnChar(a.Character);
			window.AllowUserResizing = true;
			Globals.myraDesktop.Render();
			#endregion
		}

		public void Update(GameTime gameTime)
		{
			#region Veinia
			Time.Update(gameTime);
			Globals.fps.CalculateFps(gameTime);

			NextFrame.Update();

			Globals.input.Update();

			Globals.unscaledTweener.Update(Time.unscaledDeltaTime);

			if (!Time.stop)
			{
				Globals.tweener.Update(Time.deltaTime);
				Globals.loader.current?.Update();
				Globals.loader.current?.LateUpdate();
				Globals.collisionComponent.Update(gameTime);
			}

			Title.Add(Globals.fps.currentFps, " FPS", 0);
			Title.Add(Globals.fps.isVSync, " - vSync", 1);
			title.Update();
			#endregion

			#region GeonBit.UI
			UserInterface.Active.Update(gameTime);
			#endregion

			#region Debug
#if DEBUG
			if (Globals.input.GetKeyDown(Keys.F))
				Collider.showHitboxes = !Collider.showHitboxes;

			if (Globals.input.GetKeyDown(Keys.Tab))
			{
				if (Globals.loader.current is EditorScene)
					Globals.loader.DynamicalyLoad(Globals.loader.previous);

				else
					Globals.loader.DynamicalyLoad(new EditorScene(Globals.loader.current.levelPath));
			}
#endif
			#endregion
		}

		public void Draw(SpriteBatch spriteBatch, SamplerState samplerState = null)
		{
			#region Veinia
			spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: Globals.camera.GetViewMatrix(), samplerState: samplerState);
			Globals.loader.current?.Draw(spriteBatch);
			spriteBatch.End();
			#endregion

			#region GeonBit.UI
			UserInterface.Active.Draw(spriteBatch);
			#endregion

			#region Myra.UI
			Globals.myraDesktop.Render();
			#endregion

			#region Veinia
			spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: Globals.camera.GetViewMatrix(), samplerState: samplerState);
			Globals.loader.current?.DrawAfterUI(spriteBatch);
			spriteBatch.End();
			#endregion
		}
	}
}