using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;

namespace Veinia
{
	public class Collider : Component, ICollisionActor, IDisposable, IToggleable
	{
		public static bool showHitboxes;

		public Vector2 offset;
		public bool trigger;

		//public int priority { get; protected set; }

		CollisionEventArgs currentCollisionInfo; // collision exit gets called outside of onCollide so we need to store the info outside to be passed into onCollisionExit


		public IShapeF Bounds
		{
			get { return bounds; }
			set { bounds = value; }
		}

		public RectangleF rectangleBounds;

		public Collider collider => this;

		IShapeF bounds;

		public Collider(bool trigger = false)
		{
			this.trigger = trigger;
		}

		public override void Initialize()
		{
			Bounds.Position = transform.screenPos + offset;
			rectangleBounds.Position = transform.screenPos + offset;

			Globals.collisionComponent.Insert(this);
		}

		private bool isColliding;
		private bool hasEnteredCollision;
		public virtual void OnCollision(CollisionEventArgs collisionInfo)
		{
			currentCollisionInfo = collisionInfo;

			if (!isColliding && !hasEnteredCollision)
			{
				InvokeCollisionOnAllComponents(CollisionState.Enter, collisionInfo);
				hasEnteredCollision = true;
			}
			isColliding = true;

			InvokeCollisionOnAllComponents(CollisionState.Stay, collisionInfo);


			if (!gameObject.isStatic) transform.position -= Transform.ToWorldUnits(collisionInfo.PenetrationVector);

			foreach (var item in GetAllComponents<Collider>())
			{
				item.Bounds.Position = transform.screenPos + item.offset;
				item.rectangleBounds.Position = transform.screenPos + item.offset;
			}
		}

		private bool isTriggering;
		private bool hasEnteredTrigger;
		public virtual void OnTrigger(CollisionEventArgs collisionInfo)
		{
			currentCollisionInfo = collisionInfo;

			if (!isTriggering && !hasEnteredTrigger)
			{
				InvokeTriggerOnAllComponents(CollisionState.Enter, collisionInfo);
				hasEnteredTrigger = true;
			}
			isTriggering = true;

			InvokeTriggerOnAllComponents(CollisionState.Stay, collisionInfo);
		}

		public override void LateUpdate()
		{
			if (!isColliding && hasEnteredCollision)
			{
				InvokeCollisionOnAllComponents(CollisionState.Exit, currentCollisionInfo);
				hasEnteredCollision = false;
			}
			isColliding = false;

			if (!isTriggering && hasEnteredTrigger)
			{
				InvokeTriggerOnAllComponents(CollisionState.Exit, currentCollisionInfo);
				hasEnteredTrigger = false;
			}
			isTriggering = false;

			//update bounds position
			Bounds.Position = transform.screenPos + offset;
			rectangleBounds.Position = transform.screenPos + offset;
		}

		public void Dispose() => Globals.collisionComponent.Remove(this);

		public void ToggleOn() => Globals.collisionComponent.Insert(this);

		public void ToggleOff() => Globals.collisionComponent.Remove(this);

		public void InvokeCollisionOnAllComponents(CollisionState state, CollisionEventArgs collisionInfo)
		{
			var other = collisionInfo.Other.collider;

			// if a collider touched a collider
			if (!trigger && !other.trigger)
			{
				foreach (var component in gameObject.components)
				{
					component.OnCollide(self: this, state, collisionInfo);
				}
			}
		}

		public void InvokeTriggerOnAllComponents(CollisionState state, CollisionEventArgs collisionInfo)
		{
			var other = collisionInfo.Other.collider;

			// if a any collider or trigger touched a trigger
			if ((trigger && !other.trigger) || (!trigger && other.trigger))
			{
				foreach (var component in gameObject.components)
				{
					component.OnTrigger(self: this, state, collisionInfo);
				}
			}
		}
	}

	public enum CollisionState
	{
		Enter,
		Exit,
		Stay,
	}
}