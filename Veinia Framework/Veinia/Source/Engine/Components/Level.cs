using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Veinia.Editor;

namespace Veinia
{
	public class Level
	{
		/// <summary>
		/// scene list made for iterating and calling methods (Update, Draw, etc.)
		/// </summary>
		public List<GameObject> scene = new List<GameObject>();


		public Panel Myra = new Panel();
		protected PrefabManager prefabManager { get; private set; }
		public string levelPath { get; private set; }


		public Level(PrefabManager prefabManager)
		{
			this.prefabManager = prefabManager;
		}
		public Level(PrefabManager prefabManager, string levelPath)
		{
			this.prefabManager = prefabManager;
			this.levelPath = levelPath;
		}

		/// <summary>
		/// Loads default level contents (GameObjects, Properties, etc.)
		/// </summary>
		public virtual void CreateScene(bool loadObjectsFromPath = true)
		{
			Time.stop = false;
			Globals.camera.SetPosition(Vector2.Zero);
			Globals.camera.Zoom = 1;
			Globals.desktop.Root = Myra;
			UserInterface.Active.RemoveAllEntities();

			prefabManager.LoadPrefabs();
			if (loadObjectsFromPath) LoadObjects(levelPath);
		}

		/// <summary>
		/// Loads objects from a text file.
		/// </summary>
		private void LoadObjects(string editorLevelName)
		{
			if (!File.Exists(editorLevelName)) return;
			var deserializedText = File.ReadAllText(editorLevelName);
			var objects = JsonConvert.DeserializeObject<List<EditorObject>>(deserializedText);

			foreach (var item in objects)
			{
				if (prefabManager.Find(item.PrefabName) == null)
				{
					throw new System.Exception("Prefabs that don't got deleted and are still on " + editorLevelName);
				}
				Instantiate(new Transform(item.Position), prefabManager.Find(item.PrefabName));
			}
		}

		/// <summary>
		/// Creates an object and spawns it
		/// </summary>
		public GameObject Instantiate(Transform transform, List<Component> components, bool isStatic)
		{
			GameObject sample = new GameObject(transform, components, isStatic);
			sample.level = this;

			foreach (var item in sample.components)
			{
				item.gameObject = sample;
				item.transform = sample.transform;
				item.level = sample.level;
			}

			scene.Add(sample);

			return sample;
		}
		public GameObject Instantiate(Transform transform, GameObject prefab)
		{
			GameObject sample = new GameObject(transform, prefab.components.Clone(), prefab.isStatic);
			sample.level = this;

			foreach (var item in sample.components)
			{
				item.gameObject = sample;
				item.transform = sample.transform;
				item.level = sample.level;
			}

			scene.Add(sample);

			return sample;
		}
		public GameObject Instantiate(GameObject prefab)
		{
			GameObject sample = new GameObject(prefab.transform, prefab.components.Clone(), prefab.isStatic);
			sample.level = this;

			foreach (var item in sample.components)
			{
				item.gameObject = sample;
				item.transform = sample.transform;
				item.level = sample.level;
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
				T1 currentItem = item.NullableGetComponent<T1>();
				if (currentItem == null) continue;
				else returnVal.Add(currentItem);
			}

			if (returnVal.Count == 0)
				throw new System.Exception("FindComponentOfType<T1> - Found no component matching requirements! " + typeof(T1));
			if (returnVal.Count > 1)
				throw new System.Exception("FindComponentOfType<T1> - Found more than one matching requirements! " + typeof(T1));

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

			if (returnVal.Count == 1)
				Say.Line("FindComponentsOfType<T1> - Only one component! Use FindComponentOfType<T1> instead! " + typeof(T1));

			return returnVal;
		}


		/// <summary>
		/// Initiazes components in the current scene.
		/// </summary>
		public void InitiazeComponents()
		{
			foreach (var gameObject in scene.ToArray())
			{
				if (!gameObject.isEnabled) continue;

				foreach (var component in gameObject.components.ToArray())
				{
					if (!component.isEnabled) continue;
					component.Initialize();
				}
			}
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
	}
}