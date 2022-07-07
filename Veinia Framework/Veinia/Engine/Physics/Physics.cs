using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;

public class Physics : Component, ICollisionActor, IDisposable, IToggleable
{
	public static bool showHitboxes;

	protected Vector2 offset;
	public bool trigger;
	public Vector2 velocity;

	private bool isColliding;
	private bool hasEnteredCollision;

	CollisionEventArgs currectCollisionInfo; // collision exit gets called outside of onCollide so we need to store the info outside to be passed into onCollisionExit

	public CollisionEvent onCollisionStay;
	public CollisionEvent onCollisionEnter;
	public CollisionEvent onCollisionExit;

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

	public delegate void CollisionEvent(CollisionEventArgs collisionInfo);
	public virtual void OnCollision(CollisionEventArgs collisionInfo)
	{
		currectCollisionInfo = collisionInfo;
		if (!isColliding) OnCollisionEnter(currectCollisionInfo);
		isColliding = true;

		if (onCollisionStay != null) onCollisionStay.Invoke(collisionInfo);

		if (trigger || collisionInfo.Other.physics.trigger) return;

		if (!parent.isStatic) transform.position -= Transform.ToWorldUnits(collisionInfo.PenetrationVector);


	}
	public void OnCollisionEnter(CollisionEventArgs collisionInfo)
	{
		hasEnteredCollision = true;
		if (onCollisionEnter != null) onCollisionEnter.Invoke(collisionInfo);
	}
	public void OnCollisionExit(CollisionEventArgs collisionInfo)
	{
		hasEnteredCollision = false;
		if (onCollisionExit != null) onCollisionExit.Invoke(collisionInfo);
	}

	public override void Update()
	{
		if (!isColliding && hasEnteredCollision) OnCollisionExit(currectCollisionInfo);
		isColliding = false;

		transform.position += velocity * Time.deltaTime;

		Bounds.Position = transform.screenPos + offset;

		//if (oldIsColliding && !isColliding) OnCollisionExit(currectCollisionInfo);
		//if (isColliding && !oldIsColliding) OnCollisionEnter(currectCollisionInfo);
	}

	public void Dispose()
	{
		Globals.collisionComponent.Remove(this);
	}

	public void ToggleOn()
	{
		Globals.collisionComponent.Insert(this);
	}

	public void ToggleOff()
	{
		Globals.collisionComponent.Remove(this);
	}
}