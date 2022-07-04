using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;

public class NewPhysics : Component, ICollisionActor
{
	protected Vector2 offset;
	public bool trigger;
	public Action onCollision;

	public IShapeF Bounds
	{
		get { return bounds; }
		set { bounds = value; }
	}

	public NewPhysics Physics => this;

	IShapeF bounds;


	public NewPhysics(bool trigger = false)
	{
		this.trigger = trigger;
	}

	public override void Initialize()
	{
		Bounds.Position = transform.screenPos + offset;
		Globals.collisionComponent.Insert(this);
	}

	public virtual void OnCollision(CollisionEventArgs collisionInfo)
	{
		if (onCollision != null) onCollision.Invoke();
		if (!collisionInfo.Other.Physics.trigger) transform.position -= Transform.ToWorldUnits(collisionInfo.PenetrationVector);
		//Bounds.Position -= collisionInfo.PenetrationVector;
		//Say.Line(collisionInfo.PenetrationVector);
	}

	public override void Update()
	{
		Bounds.Position = transform.screenPos + offset;
		transform.position += Vector2.One * Time.deltaTime;
		//Bounds.Position += new Vector2(Transform.ToScreenUnits(direction.X), -Transform.ToScreenUnits(direction.Y)) * Time.deltaTime;
		//transform.position = Transform.ScreenToWorldPos(Bounds.Position);
	}
}