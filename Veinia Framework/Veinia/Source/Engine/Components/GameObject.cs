using System;
using System.Collections.Generic;

namespace Veinia
{
	public class GameObject
	{
		public List<Component> components;
		public Level level;
		public Transform transform;
		public bool isStatic;
		public bool isEnabled = true;

		private bool isDestroyed;

		public GameObject(Transform transform, List<Component> components, bool isStatic)
		{
			this.transform = transform;
			this.components = components;
			this.isStatic = isStatic;

			components.Remove(components.Find(x => x is Transform)); // remove transform to make sure there aren't two transforms (prefab case)
			components.Add(transform); // the spot is added afterwards to components to ensure the gameobject having a transform

			ChangeExecutionOrder<Collider>(index: components.Count);
		}

		private void ChangeExecutionOrder<T1>(int index) where T1 : Component
		{
			//if gameObject has a collider change its execute order to last
			var matchingComponents = GetAllComponents<T1>();
			if (matchingComponents.Count != 0)
			{
				for (int i = 0; i < matchingComponents.Count; i++)
				{
					components.Remove(matchingComponents[i]);

					// -1 because the list had one object removed on the upper line
					components.Insert(index - 1 - i, matchingComponents[i]);
				}
			}
		}

		public List<T1> GetAllComponents<T1>() where T1 : Component
		{
			List<T1> temp = new List<T1>();

			foreach (var component in components)
			{
				if (component is T1)
				{
					temp.Add((T1)component);
				}
			}

			return temp;
		}

		public T1 NullableGetComponent<T1>() where T1 : Component // allows nulls to be returned
		{
			if (isDestroyed) throw new System.Exception("GetComponent<T1> - The object is already destroyed! " + typeof(T1).ToString());

			List<T1> returnVal = new List<T1>();

			foreach (var item in components)
			{
				if (item is T1)
				{
					var newItem = (T1)(object)item;
					returnVal.Add(newItem);
				}
			}

			if (returnVal.Count > 1)
				throw new System.Exception("NullableGetComponent<T1> - More than two matching components! " + typeof(T1));
			if (returnVal.Count == 0)
				return default;

			return returnVal[0];
		}
		public T1 GetComponent<T1>() where T1 : Component // throws an exception on a null
		{
			if (isDestroyed) throw new System.Exception("GetComponent<T1> - The object is already destroyed!");

			List<T1> returnVal = new List<T1>();

			foreach (var item in components)
			{
				if (item is T1)
				{
					returnVal.Add((T1)item);
				}
			}

			if (returnVal.Count > 1)
				throw new System.Exception("GetComponent<T1> - More than two matching components! " + typeof(T1));

			if (returnVal.Count == 0)
				throw new System.Exception("GetComponent<T1> - The component doesn't exist in the GameObject! " + typeof(T1));
			else
				return returnVal[0];
		}

		public Component AddComponent(Component component)
		{
			var compo = (Component)component.Clone();
			components.Add(compo);

			compo.gameObject = this;
			compo.transform = transform;
			compo.isStatic = isStatic;
			compo.level = level;
			compo.Initialize();

			ChangeExecutionOrder<Collider>(index: components.Count);

			return compo;
		}

		public void RemoveComponent(Component component)
		{
			NextFrame.actions.Add(RemoveNextFrame);

			void RemoveNextFrame()
			{
				if (component is IDisposable)
				{
					IDisposable destroyable = (IDisposable)component;
					destroyable.Dispose();
				}

				components.Remove(components.Find(x => x == component));
			}
		}

		public void DestroyGameObject()
		{
			if (isDestroyed) return;

			NextFrame.actions.Add(Destroy);

			level.Remove(this);

			void Destroy()
			{
				level = null; // remove local world
				foreach (var component in components)
				{
					if (component is IDisposable)
					{
						IDisposable destroyable = (IDisposable)component;
						destroyable.Dispose();
					}
				}

				components.Clear();
				isDestroyed = true;
				isStatic = false;
			}
		}

		public void ToggleOn()
		{
			foreach (var component in components)
			{
				if (component is IToggleable)
				{
					IToggleable toggleable = (IToggleable)component;
					toggleable.ToggleOn();
				}
			}
		}

		public void ToggleOff()
		{
			foreach (var component in components)
			{
				if (component is IToggleable)
				{
					IToggleable toggleable = (IToggleable)component;
					toggleable.ToggleOff();
				}
			}
		}
	}
}