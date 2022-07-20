using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;

namespace Veinia
{
	public class Component : ICloneable
	{
		public GameObject gameObject;
		public Transform transform;

		public bool isStatic;

		public object Clone() => MemberwiseClone();
		public virtual void Initialize() { }
		public virtual void Update() { }
		public virtual void LateUpdate() { }
		public virtual void OnCollide(CollisionState state, CollisionEventArgs collisionInfo) { }
		public virtual void OnTrigger(CollisionState state, CollisionEventArgs collisionInfo) { }
		public T1 FindComponentOfType<T1>() where T1 : Component => gameObject.level.FindComponentOfType<T1>();
		public List<T1> FindComponentsOfType<T1>() where T1 : Component => gameObject.level.FindComponentsOfType<T1>();
		public T1 NullableGetComponent<T1>() where T1 : Component => gameObject.NullableGetComponent<T1>();
		public T1 GetComponent<T1>() where T1 : Component => gameObject.GetComponent<T1>();
		public List<T1> GetAllComponents<T1>() where T1 : Component => gameObject.GetAllComponents<T1>();
		public Component AddComponent(Component component) => gameObject.AddComponent(component);
		public void RemoveComponent(Component component) => gameObject.RemoveComponent(component);
		public GameObject Instantiate(Transform transform, List<Component> components, bool isStatic) => gameObject.level.Instantiate(transform, components, isStatic);
		public GameObject Instantiate(Transform transform, GameObject newGameObject) => gameObject.level.Instantiate(transform, newGameObject);
		public GameObject Instantiate(GameObject newGameObject) => gameObject.level.Instantiate(newGameObject);
		public void DestroyGameObject() => gameObject.DestroyGameObject();
	}
}