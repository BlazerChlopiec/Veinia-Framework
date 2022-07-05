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

	public Physics physics => this;

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

		if (trigger || collisionInfo.Other.physics.trigger) return;

		if (collisionInfo.Other.physics.parent.isStatic)
		{
			if (!parent.isStatic)
			{
				Bounds.Position -= collisionInfo.PenetrationVector;
				transform.position = Transform.ScreenToWorldPos(Bounds.Position - offset);
			}
		}
		else if (!collisionInfo.Other.physics.parent.isStatic)
		{
			if (!parent.isStatic) transform.position -= Transform.ToWorldUnits(collisionInfo.PenetrationVector);
			//else transform.position = Transform.ScreenToWorldPos(Bounds.Position - offset);
			////velocity += Transform.ToWorldUnits(collisionInfo.PenetrationVector);
		}
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