using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;

public class Physics : Component, ICollisionActor
{
	protected Vector2 offset;
	public bool trigger;
	public Vector2 velocity;
	public Action onCollision;


	public IShapeF Bounds
	{
		get { return bounds; }
		set { bounds = value; }
	}
	IShapeF bounds;

	public Physics physics => this;


	public Physics(bool trigger = false)
	{
		this.trigger = trigger;
	}

	public override void Initialize()
	{
		Say.Line(Bounds.Position);
		Bounds.Position = transform.screenPos + offset;
		Globals.collisionComponent.Insert(this);
	}

	public virtual void OnCollision(CollisionEventArgs collisionInfo)
	{
		if (onCollision != null) onCollision.Invoke();
		//if (!collisionInfo.Other.Physics.trigger) transform.position -= Transform.ToWorldUnits(collisionInfo.PenetrationVector);
		if (!parent.isStatic) transform.position -= Transform.ToWorldUnits(collisionInfo.PenetrationVector);
	}

	public override void Update()
	{
		transform.position += velocity * Time.deltaTime;
		Bounds.Position = transform.screenPos + offset;
	}
}