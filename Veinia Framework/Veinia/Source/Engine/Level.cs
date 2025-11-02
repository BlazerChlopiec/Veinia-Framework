using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using tainicom.Aether.Physics2D.Dynamics;
using VeiniaFramework.Editor;

namespace VeiniaFramework
{
	public class Level
	{
		/// <summary>
		/// scene list made for iterating and calling methods (Update, Draw, etc.)
		/// </summary>
		public List<GameObject> scene = new List<GameObject>();
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
				Instantiate(new Transform { position = item.Position, rotation = item.Rotation, scale = item.Scale, Z = item.Z }, prefab).customData = item.customData;
			}
		}

		/// <summary>
		/// Creates an object and spawns it
		/// </summary>
		public GameObject Instantiate(Transform transform, List<Component> components, Body body = default, bool isStatic = false, bool dontDestroyOnLoad = false)
		{
			var sampleBody = body == null ? null : body.DeepClone();
			if (sampleBody != null) sampleBody.Enabled = true;
			GameObject sample = new GameObject(transform, components, sampleBody, isStatic, dontDestroyOnLoad);
			sample.level = this;

			if (firstFrameCreated && sample.dontDestroyOnLoad) sample.dontDestroyOnLoadInitializedBefore = true;

			foreach (var item in sample.components)
			{
				item.gameObject = sample;
				item.transform = sample.transform;
				item.level = sample.level;

				if (firstFrameCreated)
					item.EarlyInitialize();
			}

			foreach (var item in sample.components)
				if (firstFrameCreated)
					item.Initialize();

			scene.Add(sample);
			return sample;
		}
		public GameObject Instantiate(Transform transform, GameObject prefab)
		{
			var sampleBody = prefab.body == null ? null : prefab.body.DeepClone();
			if (sampleBody != null) sampleBody.Enabled = true;
			GameObject sample = new GameObject(transform, prefab.components.Clone(), sampleBody, prefab.isStatic, prefab.dontDestroyOnLoad);
			sample.level = this;
			sample.customData = prefab.customData;

			if (firstFrameCreated && sample.dontDestroyOnLoad) sample.dontDestroyOnLoadInitializedBefore = true;

			foreach (var item in sample.components)
			{
				item.gameObject = sample;
				item.transform = sample.transform;
				item.level = sample.level;

				if (firstFrameCreated)
					item.EarlyInitialize();
			}

			foreach (var item in sample.components)
				if (firstFrameCreated)
					item.Initialize();

			scene.Add(sample);
			return sample;
		}
		public GameObject Instantiate(GameObject prefab)
		{
			var sampleBody = prefab.body == null ? null : prefab.body.DeepClone();
			if (sampleBody != null) sampleBody.Enabled = true;
			GameObject sample = new GameObject((Transform)prefab.transform.Clone(), prefab.components.Clone(), sampleBody, prefab.isStatic, prefab.dontDestroyOnLoad);
			sample.level = this;

			if (firstFrameCreated && sample.dontDestroyOnLoad) sample.dontDestroyOnLoadInitializedBefore = true;

			foreach (var item in sample.components)
			{
				item.gameObject = sample;
				item.transform = sample.transform;
				item.level = sample.level;

				if (firstFrameCreated)
					item.EarlyInitialize();
			}

			foreach (var item in sample.components)
				if (firstFrameCreated)
					item.Initialize();

			scene.Add(sample);
			return sample;
		}

		/// <summary>
		/// Removes the target object from the scene
		/// </summary>
		public void Remove(GameObject target) => scene.Remove(target);

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


		/// <summary>
		/// Initializes components in the first frame (this is used to make sure all objects are created before Initialize() as it may contain FindObjectsOfType<>)
		/// </summary>
		public void InitializeComponentsFirstFrame()
		{
			firstFrameCreated = true;

			foreach (var gameObject in scene.ToArray())
			{
				if (!gameObject.isEnabled || gameObject.dontDestroyOnLoadInitializedBefore) continue;

				foreach (var component in gameObject.components.ToArray())
				{
					if (!component.isEnabled) continue;
					component.EarlyInitialize();
					component.Initialize();
				}

				if (gameObject.dontDestroyOnLoad) gameObject.dontDestroyOnLoadInitializedBefore = true;
			}
		}

		/// <summary>
		/// Updates components in the current scene.
		/// </summary>
		public virtual void Update()
		{
			foreach (var gameObject in scene.ToArray())
			{
				if (!gameObject.isEnabled) continue;

				if (gameObject.frustumCulled && gameObject.isStatic) continue;

				foreach (var component in gameObject.components.ToArray())
				{
					if (!component.isEnabled) continue;
					component.Update();
				}
			}
		}

		/// <summary>
		/// Updates components in the current scene after the normal update.
		/// </summary>
		public virtual void LateUpdate()
		{
			foreach (var gameObject in scene.ToArray())
			{
				if (!gameObject.isEnabled) continue;

				if (gameObject.frustumCulled && gameObject.isStatic) continue;

				foreach (var component in gameObject.components.ToArray())
				{
					if (!component.isEnabled) continue;
					component.LateUpdate();
				}
			}
		}

		/// <summary>
		/// Draws the components in the current scene.
		/// </summary>
		public List<DrawCommand> drawCommands = new List<DrawCommand>();
		public void Draw(SpriteBatch sb, SamplerState samplerState = null, BlendState blendState = null, Matrix? transformMatrix = null)
		{
			foreach (var gameObject in scene)
			{
				if (!gameObject.isEnabled) continue;
				if (gameObject.frustumCulled && gameObject.isStatic) continue;

				foreach (var component in gameObject.components)
				{
					if (!component.isEnabled) continue;
					if (component is IDrawn)
					{
						IDrawn drawn = (IDrawn)component;
						drawn.Draw(sb); // makes drawCommands
					}
				}
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
				var targetBlendState = cmd.blendState == null ? blendState : cmd.blendState;

				if (cmd.shader != prevCommand.shader && beginCalled // if new shader
				 || cmd.blendState != prevCommand.blendState && beginCalled // or new BlendState
				 || cmd.drawWithoutSpriteBatch && beginCalled) // or using DrawUserPrimitives()
				{
					sb.End();
					beginCalled = false;
				}
				if (!beginCalled && !cmd.drawWithoutSpriteBatch)
				{
					begins++;
					sb.Begin(SpriteSortMode.Deferred, targetBlendState, samplerState, effect: cmd.shader, transformMatrix: transformMatrix);
					beginCalled = true;
				}

				if (cmd.drawWithoutSpriteBatch && targetBlendState != null)
				{
					// blendState for drawing with DrawUserPrimitives
					Globals.graphicsDevice.BlendState = targetBlendState;
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

		public virtual void Unload()
		{
			firstFrameCreated = false;

			foreach (var item in scene.ToArray())
			{
				if (!item.dontDestroyOnLoad)
					item.DestroyGameObject();
			}
			Globals.physicsWorld.Clear();
		}
	}
}