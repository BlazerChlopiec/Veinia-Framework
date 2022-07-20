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

		private bool isColliding;
		private bool hasEnteredCollision;

		//public int priority { get; protected set; }

		CollisionEventArgs currectCollisionInfo; // collision exit gets called outside of onCollide so we need to store the info outside to be passed into onCollisionExit


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

		public virtual void OnCollision(CollisionEventArgs collisionInfo)
		{
			currectCollisionInfo = collisionInfo;
			if (!isColliding) { CallOnCollideOnEveryComponent(CollisionState.Enter, collisionInfo); hasEnteredCollision = true; }
			isColliding = true;

			CallOnCollideOnEveryComponent(CollisionState.Stay, collisionInfo);

			if (trigger || collisionInfo.Other.collider.trigger) return;


			if (!gameObject.isStatic) transform.position -= Transform.ToWorldUnits(collisionInfo.PenetrationVector);

			foreach (var item in GetAllComponents<Collider>())
			{
				item.Bounds.Position = transform.screenPos + item.offset;
			}
		}

		public override void LateUpdate()
		{
			if (!isColliding && hasEnteredCollision) { CallOnCollideOnEveryComponent(CollisionState.Exit, currectCollisionInfo); hasEnteredCollision = false; }
			isColliding = false;

			Bounds.Position = transform.screenPos + offset;
		}

		public void Dispose() => Globals.collisionComponent.Remove(this);

		public void ToggleOn() => Globals.collisionComponent.Insert(this);

		public void ToggleOff() => Globals.collisionComponent.Remove(this);

		public void CallOnCollideOnEveryComponent(CollisionState state, CollisionEventArgs collisionInfo)
		{
			if (!trigger)
			{
				foreach (var component in gameObject.components)
				{
					component.OnCollide(state, collisionInfo);
				}
			}
			else
			{
				foreach (var component in gameObject.components)
				{
					component.OnTrigger(state, collisionInfo);
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