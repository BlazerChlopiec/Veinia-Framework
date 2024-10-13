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
			Globals.camera.Scale = Vector2.One;
			Globals.camera.Rotation = 0;

			Globals.myraDesktop.Root = Myra;

			UserInterface.Active.ShowCursor = false;
			if (this is EditorScene) UserInterface.Active.ShowCursor = true;
			UserInterface.Active.RemoveAllEntities();

			Globals.particleWorld.Clear();

			prefabManager?.LoadPrefabs();
			if (loadObjectsFromPath && levelPath != string.Empty) LoadObjects(levelPath);
		}

		/// <summary>
		/// Loads objects from a text file.
		/// </summary>
		private void LoadObjects(string editorLevelName)
		{
#if DEBUG
			if (!File.Exists("LevelData/" + editorLevelName)) return;
#endif
			var deserializedText = File.ReadAllText("LevelData/" + editorLevelName);
			deserializedText = Encryption.Decrypt(deserializedText);
			var objects = JsonConvert.DeserializeObject<List<EditorObject>>(deserializedText);

			foreach (var item in objects)
			{
				if (prefabManager?.Find(item.PrefabName) == null)
				{
					throw new System.Exception("Prefabs that got deleted and are still on " + editorLevelName);
				}
				Instantiate(new Transform { position = item.Position, rotation = item.Rotation }, prefabManager?.Find(item.PrefabName));
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
				if (firstFrameCreated) item.Initialize();
			}

			scene.Add(sample);
			return sample;
		}
		public GameObject Instantiate(Transform transform, GameObject prefab)
		{
			var sampleBody = prefab.body == null ? null : prefab.body.DeepClone();
			if (sampleBody != null) sampleBody.Enabled = true;
			GameObject sample = new GameObject(transform, prefab.components.Clone(), sampleBody, prefab.isStatic, prefab.dontDestroyOnLoad);
			sample.level = this;

			if (firstFrameCreated && sample.dontDestroyOnLoad) sample.dontDestroyOnLoadInitializedBefore = true;

			foreach (var item in sample.components)
			{
				item.gameObject = sample;
				item.transform = sample.transform;
				item.level = sample.level;
				if (firstFrameCreated) item.Initialize();
			}

			scene.Add(sample);
			return sample;
		}
		public GameObject Instantiate(GameObject prefab)
		{
			var sampleBody = prefab.body == null ? null : prefab.body.DeepClone();
			if (sampleBody != null) sampleBody.Enabled = true;
			GameObject sample = new GameObject(prefab.transform, prefab.components.Clone(), sampleBody, prefab.isStatic, prefab.dontDestroyOnLoad);
			sample.level = this;

			if (firstFrameCreated && sample.dontDestroyOnLoad) sample.dontDestroyOnLoadInitializedBefore = true;

			foreach (var item in sample.components)
			{
				item.gameObject = sample;
				item.transform = sample.transform;
				item.level = sample.level;
				if (firstFrameCreated) item.Initialize();
			}
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
			List<T1> returnVal = new List<T1>();

			foreach (var item in scene)
			{
				T1 currentItem = item.GetComponent<T1>();
				if (currentItem == null) continue;
				else returnVal.Add(currentItem);
			}

			if (returnVal.Count == 0)
				return default;

			if (returnVal.Count > 1)
				Say.Line("FindComponentOfType<T1> - Found more than one component matching the requirements! " + typeof(T1));

			return returnVal[0];
		}

		/// <summary>
		/// Finds multiple components in a scene.
		/// </summary>
		public List<T1> FindComponentsOfType<T1>() where T1 : Component
		{
			List<T1> returnVal = new List<T1>();

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
		/// Initializes components in the first frame (this is used to make sure all objects are created before Initialize() as it may contain FindObjectsOfType<>)
		/// </summary>
		public void InitializeComponentsFirstFrame()
		{
			foreach (var gameObject in scene.ToArray())
			{
				if (!gameObject.isEnabled || gameObject.dontDestroyOnLoadInitializedBefore) continue;

				foreach (var component in gameObject.components.ToArray())
				{
					if (!component.isEnabled) continue;
					component.Initialize();
				}

				if (gameObject.dontDestroyOnLoad) gameObject.dontDestroyOnLoadInitializedBefore = true;
			}

			firstFrameCreated = true;
		}

		/// <summary>
		/// Updates components in the current scene.
		/// </summary>
		public void Update()
		{
			foreach (var gameObject in scene.ToArray())
			{
				if (!gameObject.isEnabled) continue;

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
		public void LateUpdate()
		{
			foreach (var gameObject in scene.ToArray())
			{
				if (!gameObject.isEnabled) continue;

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
		public void Draw(SpriteBatch sb)
		{
			foreach (var gameObject in scene)
			{
				if (!gameObject.isEnabled) continue;

				foreach (var component in gameObject.components)
				{
					if (!component.isEnabled) continue;
					if (component is IDrawn)
					{
						IDrawn drawn = (IDrawn)component;
						drawn.Draw(sb);
					}
				}
			}
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