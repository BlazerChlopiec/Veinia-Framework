using System;
using System.Collections.Generic;

public class Component : ICloneable
{
	public GameObject parent { get; set; }
	public Transform transform { get; set; }

	public bool isStatic;

	public object Clone() => MemberwiseClone();
	public virtual void Initialize() { }
	public virtual void Update() { }
	public virtual void LateUpdate() { }

	public T1 FindComponentOfType<T1>() => parent.world.FindComponentOfType<T1>();
	public List<T1> FindComponentsOfType<T1>() => parent.world.FindComponentsOfType<T1>();
	public T1 NullableGetComponent<T1>() => parent.NullableGetComponent<T1>();
	public T1 GetComponent<T1>() => parent.GetComponent<T1>();
	public Component AddComponent(Component component) => parent.AddComponent(component);
	public GameObject Instantiate(Transform transform, List<Component> components, bool isStatic) => parent.world.Instantiate(transform, components, isStatic);
	public GameObject Instantiate(Transform transform, GameObject gameObject) => gameObject.world.Instantiate(transform, gameObject);
	public void DestroyGameObject() => parent.DestroyGameObject();
}
