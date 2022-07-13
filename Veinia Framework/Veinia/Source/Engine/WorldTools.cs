using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Veinia
{
	public class WorldTools
	{
		//scene lists made for iterating and calling proper methods
		public List<GameObject> scene = new List<GameObject>();


		//creates an object and spawns it
		public GameObject Instantiate(Transform transform, List<Component> components, bool isStatic)
		{
			GameObject sample = new GameObject(transform, components, isStatic);
			sample.world = this;

			foreach (var item in sample.components)
			{
				item.parent = sample;
				item.transform = sample.transform;
				item.isStatic = sample.isStatic;
			}

			scene.Add(sample);

			return sample;
		}
		public GameObject Instantiate(Transform transform, GameObject prefab)
		{
			GameObject sample = new GameObject(transform, prefab.components.Clone(), prefab.isStatic);
			sample.world = this;

			foreach (var item in sample.components)
			{
				item.parent = sample;
				item.transform = sample.transform;
				item.isStatic = sample.isStatic;
			}

			scene.Add(sample);

			return sample;
		}
		public GameObject Instantiate(GameObject prefab)
		{
			GameObject sample = new GameObject(prefab.transform, prefab.components.Clone(), prefab.isStatic);
			sample.world = this;

			foreach (var item in sample.components)
			{
				item.parent = sample;
				item.transform = sample.transform;
				item.isStatic = sample.isStatic;
			}

			scene.Add(sample);

			return sample;
		}

		public void Remove(GameObject target) => scene.Remove(target);

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

		public void InitiazeComponents()
		{
			foreach (var gameObject in scene.ToArray())
			{
				if (!gameObject.isEnabled) continue;

				foreach (var component in gameObject.components.ToArray())
				{
					component.Initialize();
				}
			}
		}

		public void Update()
		{
			foreach (var gameObject in scene.ToArray())
			{
				if (!gameObject.isEnabled) continue;

				foreach (var component in gameObject.components.ToArray())
				{
					component.Update();
				}
			}
		}
		public void LateUpdate()
		{
			foreach (var gameObject in scene.ToArray())
			{
				if (!gameObject.isEnabled) continue;

				foreach (var component in gameObject.components.ToArray())
				{
					component.LateUpdate();
				}
			}
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (var gameObject in scene)
			{
				if (!gameObject.isEnabled) continue;

				foreach (var component in gameObject.components)
				{
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