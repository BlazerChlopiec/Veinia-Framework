using System;
using System.Collections.Generic;

namespace Veinia
{
	public class Component : ICloneable
	{
		public GameObject parent { get; set; }
		public Transform transform { get; set; }

		public bool isStatic;

		public object Clone() => MemberwiseClone();
		public virtual void Initialize() { }
		public virtual void Update() { }
		public T1 FindComponentOfType<T1>() where T1 : Component => parent.world.FindComponentOfType<T1>();
		public List<T1> FindComponentsOfType<T1>() where T1 : Component => parent.world.FindComponentsOfType<T1>();
		public T1 NullableGetComponent<T1>() where T1 : Component => parent.NullableGetComponent<T1>();
		public T1 GetComponent<T1>() where T1 : Component => parent.GetComponent<T1>();
		public Component AddComponent(Component component) => parent.AddComponent(component);
		public void RemoveComponent(Component component) => parent.RemoveComponent(component);
		public GameObject Instantiate(Transform transform, List<Component> components, bool isStatic) => parent.world.Instantiate(transform, components, isStatic);
		public GameObject Instantiate(Transform transform, GameObject gameObject) => parent.world.Instantiate(transform, gameObject);
		public GameObject Instantiate(GameObject gameObject) => parent.world.Instantiate(gameObject);
		public void DestroyGameObject() => parent.DestroyGameObject();
	}

}