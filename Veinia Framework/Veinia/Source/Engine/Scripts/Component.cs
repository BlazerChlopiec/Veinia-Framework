using System;
using System.Collections.Generic;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace VeiniaFramework
{
	public class Component : ICloneable
	{
		public GameObject gameObject;
		public Transform transform;
		public Level level;
		public Body body { get { return gameObject.body; } set { gameObject.body = value; } }
		public bool isStatic { get { return gameObject.isStatic; } set { gameObject.isStatic = value; } }
		public bool dontDestroyOnLoad { get { return gameObject.dontDestroyOnLoad; } set { gameObject.dontDestroyOnLoad = value; } }
		public string customData { get { return gameObject.customData; } set { gameObject.customData = value; } }
		public bool isEnabled = true;

		public object Clone() => MemberwiseClone();
		public virtual void Initialize() { }
		public virtual void Update() { }
		public virtual void LateUpdate() { }
		public virtual bool OnCollide(Fixture sender, Fixture other, Contact contact) => true;
		public virtual void OnSeparate(Fixture sender, Fixture other, Contact contact) { }
		public T1 FindComponentOfType<T1>() where T1 : Component => level.FindComponentOfType<T1>();
		public List<T1> FindComponentsOfType<T1>() where T1 : Component => level.FindComponentsOfType<T1>();
		public T1 GetComponent<T1>() where T1 : Component => gameObject.GetComponent<T1>();
		public List<T1> GetAllComponents<T1>() where T1 : Component => gameObject.GetAllComponents<T1>();
		public Component AddComponent(Component component) => gameObject.AddComponent(component);
		public void RemoveComponent(Component component) => gameObject.RemoveComponent(component);
		public GameObject Instantiate(Transform transform, List<Component> components, Body body = default, bool isStatic = false, bool dontDestroyOnLoad = false) => level.Instantiate(transform, components, body, isStatic, dontDestroyOnLoad);
		public GameObject Instantiate(Transform transform, GameObject newGameObject) => level.Instantiate(transform, newGameObject);
		public GameObject Instantiate(GameObject newGameObject) => level.Instantiate(newGameObject);
		public void DestroyGameObject() => gameObject.DestroyGameObject();
	}
}