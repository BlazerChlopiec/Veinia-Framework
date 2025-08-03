using Apos.Camera;
using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using System;
using tainicom.Aether.Physics2D.Diagnostics;
using tainicom.Aether.Physics2D.Dynamics;
using VeiniaFramework.Editor;

namespace VeiniaFramework
{
	public class Veinia
	{
		Title title;
		Game game;
		DebugView debugView;

		public static bool isEditor { get; private set; }
		public static bool ShouldPauseWhenInactive { get; private set; }
		public bool pauseOnUnfocused;


		public Veinia(Game game, GraphicsDeviceManager graphicsManager)
		{
			//this one takes effect only in a game's constructor
			this.game = game;
			Globals.graphicsManager = graphicsManager;
			Globals.fps = new FPS(game);
		}

		public void Initialize(GraphicsDevice graphicsDevice, ContentManager content, GameWindow window,
							   Screen screen, int unitSize, Vector2? gravity = null, PrefabManager prefabManager = null)
		{
			#region Veinia
			Transform.unitSize = unitSize;

			Globals.loader = new Loader(prefabManager);
			Globals.graphicsDevice = graphicsDevice;
			Globals.content = content;
			Globals.screen = screen;
			Globals.camera = new Camera(new DensityViewport(graphicsDevice, window, 1920, 1080));
			Globals.physicsWorld = new World(gravity ?? new Vector2(0, -9.81f));
			Globals.frustumCulling = new FrustumCulling();

			window.ClientSizeChanged += (s, a) =>
			{
				screen.OnApplyChanges?.Invoke();

				Globals.graphicsManager.PreferredBackBufferWidth = Globals.graphicsDevice.PresentationParameters.BackBufferWidth;
				Globals.graphicsManager.PreferredBackBufferHeight = Globals.graphicsDevice.PresentationParameters.BackBufferHeight;
			};

			Globals.graphicsManager.DeviceReset += (s, a) =>
			{
				Globals.camera.ResetViewport();
				Globals.camera.SetViewport();
			};

			title = new Title(window);
			debugView = new DebugView(Globals.physicsWorld);
			debugView.LoadContent(graphicsDevice, content);
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
				var mouse = Mouse.GetState().Position.ToVector2() - Globals.camera.VirtualViewport.XY;
				info.Position = mouse.ToPoint();
				return info;
			};

			window.TextInput += (s, a) => Globals.myraDesktop.OnChar(a.Character);
			Globals.myraDesktop.Render();
			#endregion

			#region GeonBit.UI
			UserInterface.Initialize(content, BuiltinThemes.veinia_default);
			UserInterface.Active.ShowCursor = false;
			#endregion
		}

		public void Update(GameTime gameTime)
		{
			#region Veinia

			ShouldPauseWhenInactive = !pauseOnUnfocused || game.IsActive;

			Globals.fps.CalculateFps(gameTime);

			NextFrame.Update();

			Timers.ProcessTimers();

			Globals.unscaledTweener.Update(Time.unscaledDeltaTime);

			Globals.frustumCulling.Update();

			Time.Update(gameTime);

			if (!isEditor && ShouldPauseWhenInactive && !Time.stop
			  || isEditor && !Time.stop && game.IsActive)
			{
				if (game.IsActive) Globals.input.Update();

				Globals.particleWorld.Update();
				Globals.tweener.Update(Time.deltaTime);

				Globals.physicsWorld.Step(Time.deltaTime);

				Globals.myraDesktop.UpdateInput();
				Globals.myraDesktop.UpdateLayout();

				Globals.loader.current?.Update();
				Globals.loader.current?.LateUpdate();

				Globals.camera.shake.Update();
			}

			Title.Add(Globals.fps.currentFps, " FPS", 0);
			Title.Add(Globals.fps.vSync, " - vSync", 1);
			title.Update();
			#endregion

			#region GeonBit.UI
			UserInterface.Active.Update(gameTime);
			#endregion

			#region Debug
#if DEBUG
			if (Globals.input.GetKeyDown(Keys.Tab))
			{
				if (Globals.loader.current is EditorScene)
				{
					isEditor = false;
					var editorScene = (EditorScene)Globals.loader.current;
					dynamic editedLevelInstance = Activator.CreateInstance(editorScene.editedSceneType);
					editedLevelInstance.levelPath = editorScene.levelPath;
					Convert.ChangeType(editedLevelInstance, editorScene.editedSceneType);
					Globals.loader.DynamicalyLoad(editedLevelInstance);
				}
				else if (Globals.loader.current == null) EditorScene.ErrorWindow("Warning", "Aborting Editor! The current level does not exist! Use Globals.loader!");
				else
				{
					isEditor = true;
					if (!game.IsMouseVisible) game.IsMouseVisible = true;

					Globals.loader.DynamicalyLoad(new EditorScene(Globals.loader.current.levelPath, Globals.loader.current.GetType()));
				}
			}
#endif
			#endregion
		}

		public void Draw(SpriteBatch spriteBatch, SamplerState samplerState = null, BlendState blendState = null)
		{
			DrawWorld(spriteBatch, samplerState, blendState);
			DrawMyra();
			DrawGeon(spriteBatch);
			DrawDebugPhysics();
		}

		public void DrawWorld(SpriteBatch spriteBatch, SamplerState samplerState = null, BlendState blendState = null)
		{
			spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: Globals.camera.GetView(), samplerState: samplerState, blendState: blendState);
			Globals.loader.current?.Draw(spriteBatch);
			Globals.particleWorld.Draw(spriteBatch);
			spriteBatch.End();
		}
		public void DrawMyra() => Globals.myraDesktop.RenderVisual();
		public void DrawGeon(SpriteBatch spriteBatch) => UserInterface.Active.Draw(spriteBatch);
		public void DrawDebugPhysics()
		{
			if (Globals.debugDraw)
			{
				float zScale = Globals.camera.ZToScale(Globals.camera.Z, 0);
				var view = Globals.camera.VirtualViewport.Transform(
					Matrix.CreateTranslation(-Globals.camera.X / Transform.unitSize, Globals.camera.Y / Transform.unitSize, 0f) *
					Matrix.CreateRotationZ(-Globals.camera.Rotation) *
					Matrix.CreateScale(Globals.camera.Scale.X, -Globals.camera.Scale.Y, 1f) *
					Matrix.CreateScale(zScale, zScale, 1f) *
					Matrix.CreateTranslation(new Vector3(Globals.camera.VirtualViewport.Origin, 0f)) *
					Matrix.CreateTranslation(new Vector3(Globals.camera.shake.shakeOffset / Transform.unitSize, 0f)));

				debugView.RenderDebugData(Globals.camera.GetProjection() * Matrix.CreateScale(Transform.unitSize, Transform.unitSize, Transform.unitSize), view);
			}
		}
	}
}