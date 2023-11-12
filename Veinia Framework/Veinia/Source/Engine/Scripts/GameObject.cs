using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace VeiniaFramework
{
	public class GameObject
	{
		public List<Component> components;
		public Level level;
		public Transform transform;

		public Body body
		{
			get { return Body; }
			set { Body = value; Body.OnCollision += OnCollision; Body.OnSeparation += OnSeparation; Body.Position = transform.Position; Body.Rotation = MathHelper.ToRadians(transform.Rotation); }
		}

		private Body Body;

		public bool isStatic;
		public bool dontDestroyOnLoad;
		public bool isEnabled = true;

		//if your object is dontDestroyOnLoad prevent initializing it on a new scene (initialize once only)
		public bool dontDestroyOnLoadInitializedBefore;

		private bool isDestroyed;


		public GameObject(Transform transform, List<Component> components, Body body = default, bool isStatic = false, bool dontDestoryOnLoad = false)
		{
			this.transform = transform;
			this.components = components;
			if (body != null) this.body = body;
			this.isStatic = isStatic;
			this.dontDestroyOnLoad = dontDestoryOnLoad;

			components.Remove(components.Find(x => x is Transform)); // remove transform to make sure there aren't two transforms (prefab case)
			components.Add(transform); // the transform is added afterwards to components to ensure the gameobject having a transform
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

		public T1 GetComponent<T1>() where T1 : Component
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

			if (returnVal.Count == 0)
				return default;

			if (returnVal.Count > 1)
				throw new System.Exception("GetComponent<T1> - More than one matching components! " + typeof(T1));

			else
				return returnVal[0];
		}

		public Component AddComponent(Component component)
		{
			var compo = (Component)component.Clone();
			components.Add(compo);

			compo.gameObject = this;
			compo.transform = transform;
			compo.level = level;
			if (level.firstFrameCreated) compo.Initialize();

			return compo;
		}

		private void OnSeparation(Fixture sender, Fixture other, Contact contact)
		{
			foreach (var component in components)
				component.OnSeparate(sender, other, contact);
		}

		private bool OnCollision(Fixture sender, Fixture other, Contact contact)
		{
			bool[] temp = new bool[components.Count];
			foreach (var component in components)
				temp[components.IndexOf(component)] = component.OnCollide(sender, other, contact);

			return temp.Min();
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

			foreach (var component in components)
			{
				if (component is IDisposable)
				{
					IDisposable destroyable = (IDisposable)component;
					destroyable.Dispose();
				}
			}

			void Destroy()
			{
				level = null; // remove local world

				components.Clear();
				isDestroyed = true;
				isStatic = false;
				if (body != null && Globals.physicsWorld.BodyList.Contains(body)) Globals.physicsWorld.Remove(body);
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