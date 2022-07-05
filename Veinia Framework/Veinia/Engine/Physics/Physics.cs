using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;

public class Physics : Component, ICollisionActor, IDisposable
{
	protected Vector2 offset;
	public bool trigger;
	public Vector2 velocity;
	public OnCollisionStay onCollision;

	public IShapeF Bounds
	{
		get { return bounds; }
		set { bounds = value; }
	}
	IShapeF bounds;

	public Physics(bool trigger = false)
	{
		this.trigger = trigger;
	}

	public override void Initialize()
	{
		Bounds.Position = transform.screenPos + offset;
		Globals.collisionComponent.Insert(this);
	}

	public delegate void OnCollisionStay(CollisionEventArgs collisionInfo);
	public virtual void OnCollision(CollisionEventArgs collisionInfo)
	{
		if (onCollision != null) onCollision.Invoke(collisionInfo);
		//if (!collisionInfo.Other.Physics.trigger) transform.position -= Transform.ToWorldUnits(collisionInfo.PenetrationVector);
		if (!parent.isStatic) transform.position -= Transform.ToWorldUnits(collisionInfo.PenetrationVector);
		//velocity += Transform.ToWorldUnits(collisionInfo.PenetrationVector);
	}

	public override void Update()
	{
		transform.position += velocity * Time.deltaTime;

		Bounds.Position = transform.screenPos + offset;
	}

	public void Dispose()
	{
		Globals.collisionComponent.Remove(this);
	}
}