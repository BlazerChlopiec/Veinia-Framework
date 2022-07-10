using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;

namespace Veinia
{
	public class Collider : Component, ICollisionActor, IDisposable, IToggleable
	{
		public static bool showHitboxes;

		protected Vector2 offset;
		public bool trigger;

		private bool isColliding;
		private bool hasEnteredCollision;

		//public int priority { get; protected set; }

		CollisionEventArgs currectCollisionInfo; // collision exit gets called outside of onCollide so we need to store the info outside to be passed into onCollisionExit

		public CollisionEvent onCollisionStay;
		public CollisionEvent onCollisionEnter;
		public CollisionEvent onCollisionExit;


		public IShapeF Bounds
		{
			get { return bounds; }
			set { bounds = value; }
		}

		public Collider collider => this;

		IShapeF bounds;

		public Collider(bool trigger = false)
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

			if (trigger || collisionInfo.Other.collider.trigger) return;

			if (!parent.isStatic) transform.position -= Transform.ToWorldUnits(collisionInfo.PenetrationVector);

			Bounds.Position = transform.screenPos + offset;
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

			Bounds.Position = transform.screenPos + offset;
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
}