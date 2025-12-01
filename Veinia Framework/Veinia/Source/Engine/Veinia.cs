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
		public static bool PausedGameWhenInactiveWindow { get; private set; }
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
			Globals.shapeDrawing = new ShapeDrawing(graphicsDevice);

			window.ClientSizeChanged += (s, a) => screen.ClientSizeChanged();

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

			PausedGameWhenInactiveWindow = pauseOnUnfocused && !game.IsActive;

			Globals.fps.CalculateFps(gameTime);

			NextFrame.Update();

			Timers.ProcessTimers();

			Globals.unscaledTweener.Update(Time.unscaledDeltaTime);

			Globals.frustumCulling.Update();

			Time.Update(gameTime);

			if (!isEditor && !PausedGameWhenInactiveWindow && !Time.stop
			  || isEditor && !Time.stop && game.IsActive)
			{
				if (game.IsActive) Globals.input.Update();

				Globals.tweener.Update(Time.deltaTime);


				var level = Globals.loader.current;
				if (level != null)
				{
					Globals.physicsWorld.Step(Time.deltaTime);

					level.AssignActiveScene();
					level.Update();
					level.LateUpdate();
				}

				Globals.particleWorld.Update();

				Globals.camera.shake.Update();
				Globals.shapeDrawing.UpdateBasicEffect();
			}

			title.Update();
			#endregion

			#region GeonBit.UI
			UserInterface.Active.Update(gameTime);
			#endregion

			#region Myra.UI
			if (!PausedGameWhenInactiveWindow)
			{
				Globals.myraDesktop.UpdateInput();
				Globals.myraDesktop.UpdateLayout();
			}
			#endregion

			#region Debug
#if DEBUG
			if (Globals.input.GetKeyDown(Keys.Tab))
				ToggleEditor(Globals.loader.current);
#endif
			#endregion
		}

		public void ToggleEditor(Level level = null)
		{
			if (level is EditorScene)
			{
				isEditor = false;

				var editorScene = (EditorScene)Globals.loader.current;

				if (editorScene.editedSceneType == null)
				{
					EditorScene.ErrorWindow("Warning", "Cant play! No level type loaded! Use Globals.loader.DynamicalyLoad() after Veinia.Initialize() or Globals.loader.AddStoredLevels()");
					return;
				}

				var editedLevelInstance = (Level)Activator.CreateInstance(editorScene.editedSceneType);
				editedLevelInstance.levelPath = editorScene.levelPath;

				Globals.loader.DynamicalyLoad(editedLevelInstance);
			}
			else
			{
				isEditor = true;
				if (!game.IsMouseVisible) game.IsMouseVisible = true;

				var editorScene = new EditorScene(level != null ? level.levelPath : null, level?.GetType());
				Globals.loader.DynamicalyLoad(editorScene);
			}
		}

		public void Draw(SpriteBatch spriteBatch, SamplerState samplerState = null, BlendState blendState = null)
		{
			DrawWorld(spriteBatch, samplerState, blendState, transformMatrix: Globals.camera.GetView());
			DrawMyra();
			DrawGeon(spriteBatch);
			DrawDebugPhysics();
		}

		public void DrawWorld(SpriteBatch spriteBatch, SamplerState samplerState = null, BlendState blendState = null, Matrix? transformMatrix = null)
		{
			Globals.loader.current?.Draw(spriteBatch, samplerState, blendState, transformMatrix);
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
					Matrix.CreateScale(1f / Globals.camera.Scale, 1f / -Globals.camera.Scale, 1f) *
					Matrix.CreateScale(zScale, zScale, 1f) *
					Matrix.CreateTranslation(new Vector3(Globals.camera.VirtualViewport.Origin, 0f)) *
					Matrix.CreateTranslation(new Vector3(Globals.camera.shake.shakeOffset / Transform.unitSize, 0f)));

				debugView.RenderDebugData(Globals.camera.GetProjection() * Matrix.CreateScale(Transform.unitSize, Transform.unitSize, Transform.unitSize), view);
			}
		}
	}
}