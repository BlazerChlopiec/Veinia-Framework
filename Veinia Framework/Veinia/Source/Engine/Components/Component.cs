using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;

namespace Veinia
{
	public class Component : ICloneable
	{
		public GameObject gameObject;
		public Transform transform;
		public Level level;
		public bool isStatic { get { return gameObject.isStatic; } set { gameObject.isStatic = value; } }
		public bool isEnabled = true;

		public object Clone() => MemberwiseClone();
		public virtual void Initialize() { }
		public virtual void Update() { }
		public virtual void LateUpdate() { }
		public virtual void OnCollide(CollisionState state, CollisionEventArgs collisionInfo) { }
		public virtual void OnTrigger(CollisionState state, CollisionEventArgs collisionInfo) { }
		public T1 FindComponentOfType<T1>() where T1 : Component => level.FindComponentOfType<T1>();
		public List<T1> FindComponentsOfType<T1>() where T1 : Component => level.FindComponentsOfType<T1>();
		public T1 NullableGetComponent<T1>() where T1 : Component => gameObject.NullableGetComponent<T1>();
		public T1 GetComponent<T1>() where T1 : Component => gameObject.GetComponent<T1>();
		public List<T1> GetAllComponents<T1>() where T1 : Component => gameObject.GetAllComponents<T1>();
		public Component AddComponent(Component component) => gameObject.AddComponent(component);
		public void RemoveComponent(Component component) => gameObject.RemoveComponent(component);
		public GameObject Instantiate(Transform transform, List<Component> components, bool isStatic) => level.Instantiate(transform, components, isStatic);
		public GameObject Instantiate(Transform transform, GameObject newGameObject) => level.Instantiate(transform, newGameObject);
		public GameObject Instantiate(GameObject newGameObject) => level.Instantiate(newGameObject);
		public void DestroyGameObject() => gameObject.DestroyGameObject();
	}
}