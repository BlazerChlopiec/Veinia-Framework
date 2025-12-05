using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using tainicom.Aether.Physics2D.Dynamics;
using VeiniaFramework.Editor;

namespace VeiniaFramework
{
	public class Level
	{
		/// <summary>
		/// scene list made for all gameObjects
		/// </summary>
		public List<GameObject> scene = new List<GameObject>();

		/// <summary>
		/// scene list made for iterating and calling methods (Update, Draw, etc.)
		/// </summary>
		public List<GameObject> activeScene = new List<GameObject>();

		public bool firstFrameCreated { get; private set; }


		public Panel Myra = new Panel();
		public PrefabManager prefabManager { get; set; }
		public string levelPath;

		public Level(string levelPath) => this.levelPath = levelPath;
		public Level() { }

		/// <summary>
		/// Loads default level contents (GameObjects, Properties, etc.)
		/// </summary>
		public virtual void CreateScene(bool loadObjectsFromPath = true)
		{
			Globals.camera.SetPosition(Vector2.Zero);
			Globals.camera.Scale = 1f;
			Globals.camera.Rotation = 0;
			Globals.camera.ResetLookaheads();
			Globals.camera.shake.Reset();

			Globals.myraDesktop.Root = Myra;

			UserInterface.Active.ShowCursor = false;
			if (this is EditorScene) UserInterface.Active.ShowCursor = true;
			UserInterface.Active.RemoveAllEntities();

			Globals.particleWorld.Clear();

			prefabManager?.LoadPrefabs();
			if (loadObjectsFromPath && levelPath != string.Empty && levelPath != null) LoadObjects(levelPath);
		}

		/// <summary>
		/// Loads objects from a text file.
		/// </summary>
		private void LoadObjects(string editorLevelName)
		{
#if DEBUG
			if (!File.Exists("LevelData/" + editorLevelName)) return;
#endif
			string dataToLoad = EditorJSON.encryptScene ? Encryption.Decrypt(File.ReadAllBytes("LevelData/" + editorLevelName))
								: File.ReadAllText("LevelData/" + editorLevelName);

			var sceneFile = JsonConvert.DeserializeObject<SceneFile>(dataToLoad);

			foreach (var item in sceneFile.objects)
			{
				var prefab = prefabManager?.Find(item.PrefabName);
				if (prefab == null)
				{
					throw new System.Exception("Prefabs that got deleted and are still on " + editorLevelName);
				}
				var sample = Instantiate(new Transform { position = item.Position, rotation = item.Rotation, scale = item.Scale, Z = item.Z }, prefab);
				sample.customData = item.customData;

				if (item.ShouldSerializeColor()) sample.GetComponent<Sprite>().color = item.Color;
			}
		}

		/// <summary>
		/// Creates an object and spawns it
		/// </summary>
		public GameObject Instantiate(Transform transform, List<Component> components, Body body = default, string customData = null, bool isStatic = false, bool dontDestroyOnLoad = false)
		{
			var sampleBody = body == null ? null : body.DeepClone();
			if (sampleBody != null) sampleBody.Enabled = true;

			GameObject sample = new GameObject(transform, components, sampleBody, customData, isStatic, dontDestroyOnLoad);
			sample.level = this;

			for (int i = 0; i < sample.components.Count; i++)
			{
				var comp = sample.components[i];

				comp.gameObject = sample;
				comp.transform = sample.transform;
				comp.level = sample.level;
			}

			if (firstFrameCreated)
			{
				sample.EarlyInitialize();
				sample.Initialize();

				sample.isInitialized = true;
			}

			scene.Add(sample);
			return sample;
		}
		public GameObject Instantiate(Transform transform, GameObject prefab)
		{
			return Instantiate(transform, prefab.components.Clone(), prefab.body == null ? null : prefab.body.DeepClone(), prefab.customData, prefab.isStatic, prefab.dontDestroyOnLoad);
		}
		public GameObject Instantiate(GameObject prefab)
		{
			return Instantiate((Transform)prefab.transform.Clone(), prefab.components, prefab.body == null ? null : prefab.body.DeepClone(), prefab.customData, prefab.isStatic, prefab.dontDestroyOnLoad);
		}

		/// <summary>
		/// Removes the target object from the scene
		/// </summary>
		public void Remove(GameObject target) => scene.Remove(target);


		/// <summary>
		/// Initializes components in the first frame (this is used to make sure all objects are created before Initialize() as it may contain FindObjectsOfType<>)
		/// </summary>
		public void InitializeComponentsFirstFrame()
		{
			firstFrameCreated = true;

			for (int i = 0; i < scene.Count; i++)
			{
				var obj = scene[i];

				if (!obj.isEnabled || obj.isInitialized && obj.dontDestroyOnLoad) continue;

				obj.EarlyInitialize();
				obj.Initialize();

				obj.isInitialized = true;
			}
		}

		/// <summary>
		/// Assigns which objects should be queued for method calls in a frame
		/// </summary>
		public void AssignActiveScene()
		{
			activeScene.Clear();

			for (int i = 0; i < scene.Count; i++)
			{
				var obj = scene[i];

				if (!obj.isEnabled) continue;
				if (obj.frustumCulled && obj.isStatic) continue;

				activeScene.Add(obj);
			}
		}

		/// <summary>
		/// Updates objects in the current scene.
		/// </summary>
		public virtual void Update()
		{
			for (int i = 0; i < activeScene.Count; i++)
			{
				activeScene[i].Update();
			}
		}

		/// <summary>
		/// Updates objects in the current scene after the normal update.
		/// </summary>
		public virtual void LateUpdate()
		{
			for (int i = 0; i < activeScene.Count; i++)
			{
				activeScene[i].LateUpdate();
			}
		}

		/// <summary>
		/// Draws objects in the current scene.
		/// </summary>
		public List<DrawCommand> drawCommands = new List<DrawCommand>();
		public void Draw(SpriteBatch sb, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Matrix? transformMatrix = null)
		{
			for (int i = 0; i < activeScene.Count; i++)
			{
				activeScene[i].Draw(sb);
			}

			Globals.particleWorld.Draw(sb, this); // makes drawCommands

			drawCommands.Sort((a, b) =>
			{
				// compare by z
				int zCompare = a.Z.CompareTo(b.Z);
				if (zCompare != 0)
					return zCompare;

				// if z the same - compare by shaders (grouping shaders together to avoid more Begin())
				if (a.shader == b.shader)
					return 0;
				if (a.shader == null)
					return -1;
				if (b.shader == null)
					return 1;

				return a.shader.GetHashCode().CompareTo(b.shader.GetHashCode());
			});


			DrawCommand prevCommand = default;
			bool beginCalled = false;
			int begins = 0;

			foreach (var cmd in drawCommands)
			{
				var targetBlendState = cmd.blendState ?? blendState;
				var targetDepthStencilState = cmd.depthStencilState ?? depthStencilState;
				var targetRasterizerState = cmd.rasterizerState ?? rasterizerState;

				if (cmd.shader != prevCommand.shader && beginCalled // if new shader
				 || cmd.blendState != prevCommand.blendState && beginCalled // or new BlendState
				 || cmd.depthStencilState != prevCommand.depthStencilState && beginCalled // or new DepthStencilState
				 || cmd.rasterizerState != prevCommand.rasterizerState && beginCalled // or new RasterizerState
				 || cmd.drawWithoutSpriteBatch && beginCalled) // or using DrawUserPrimitives()
				{
					sb.End();
					beginCalled = false;
				}
				if (!beginCalled && !cmd.drawWithoutSpriteBatch)
				{
					begins++;
					sb.Begin(SpriteSortMode.Deferred, targetBlendState, samplerState, targetDepthStencilState, targetRasterizerState, cmd.shader, transformMatrix);
					beginCalled = true;
				}

				if (cmd.drawWithoutSpriteBatch)
				{
					// blendState for drawing with DrawUserPrimitives
					if (targetBlendState != null) Globals.graphicsDevice.BlendState = targetBlendState;

					// depthStencilState for drawing with DrawUserPrimitives
					if (depthStencilState != null) Globals.graphicsDevice.DepthStencilState = depthStencilState;

					// rasterizerState for drawing with DrawUserPrimitives
					if (rasterizerState != null) Globals.graphicsDevice.RasterizerState = rasterizerState;
				}

				prevCommand = cmd;
				cmd.command.Invoke();
			}

			drawCommands.Clear();

			if (beginCalled)
			{
				sb.End();
				beginCalled = false;
			}

			Title.Add(begins, " - SpriteBatch Begins", 5);
		}

		/// <summary>
		/// Unloads current level
		/// </summary>
		public virtual void Unload()
		{
			firstFrameCreated = false;

			foreach (var obj in scene.ToArray())
			{
				if (!obj.dontDestroyOnLoad)
					obj.DestroyGameObject();
			}
			Globals.physicsWorld.Clear();
		}

		/// <summary>
		/// Finds a component in the scene.
		/// </summary>
		public T1 FindComponentOfType<T1>() where T1 : Component
		{
			var returnVal = new List<T1>();

			foreach (var item in scene)
			{
				T1 currentItem = item.GetComponent<T1>();
				if (currentItem == null) continue;
				else returnVal.Add(currentItem);
			}

			if (returnVal.Count == 0)
			{
				Say.Line("FindComponentOfType<T1> - Found no components matching requirements! " + typeof(T1));
				return default;
			}

			if (returnVal.Count > 1)
				Say.Line("FindComponentOfType<T1> - Found more than one component matching the requirements! " + typeof(T1));

			return returnVal[0];
		}

		/// <summary>
		/// Finds multiple components in a scene.
		/// </summary>
		public List<T1> FindComponentsOfType<T1>() where T1 : Component
		{
			var returnVal = new List<T1>();

			foreach (var item in scene)
			{
				var matchingComponents = item.GetAllComponents<T1>();

				matchingComponents.AddAllTo(returnVal);
			}
			if (returnVal.Count == 0)
				Say.Line("FindComponentsOfType<T1> - Found no components matching requirements! " + typeof(T1));

			return returnVal;
		}

		/// <summary>
		/// Finds an object in the scene by customData.
		/// </summary>
		public GameObject FindObjectByData(object match)
		{
			var returnVal = new List<GameObject>();

			foreach (var item in scene)
			{
				GameObject currentItem = null;
				if (item.customData != null && item.customData.Equals(match))
					currentItem = item;
				if (currentItem == null) continue;
				else returnVal.Add(currentItem);
			}

			if (returnVal.Count == 0)
			{
				Say.Line("FindObjectByData - Found no object matching requirements! Query: " + (string)match);
				return default;
			}

			if (returnVal.Count > 1)
				Say.Line("FindObjectByData - Found more than one object matching the requirements! Query:" + (string)match);

			return returnVal[0];
		}

		/// <summary>
		/// Finds objects in the scene by customData.
		/// </summary>
		public List<GameObject> FindObjectsByData(object match)
		{
			var returnVal = new List<GameObject>();

			foreach (var item in scene)
			{
				GameObject currentItem = null;
				if (item.customData != null && item.customData.Equals(match))
					currentItem = item;
				if (currentItem == null) continue;
				else returnVal.Add(currentItem);
			}
			if (returnVal.Count == 0)
				Say.Line("FindObjectsByData - Found no object matching requirements! Query: " + (string)match);

			return returnVal;
		}

		/// <summary>
		/// Finds component in the scene by customData
		/// </summary>
		public T1 FindComponentByData<T1>(object match) where T1 : Component => FindObjectByData(match).GetComponent<T1>();

		/// <summary>
		/// Finds components in the scene by customData
		/// </summary>
		public List<T1> FindComponentsByData<T1>(object match) where T1 : Component
		{
			var objects = FindObjectsByData(match);

			var returnVal = new List<T1>();

			foreach (var item in objects)
			{
				var currentItem = item.GetComponent<T1>();
				if (currentItem != null)
				{
					returnVal.Add(currentItem);
				}
			}

			return returnVal;
		}
	}
}