using Humper;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

public class Physics : Component
{
	public Vector2 velocity;
	public Vector2 convVelocity;


	ColliderTemplate box;

	public List<ColliderTemplate> worldColliders = new List<ColliderTemplate>();

	public bool touchingRight;
	public bool touchingLeft;
	public bool touchingBottom;
	public bool touchingTop;

	public float gravity = 1;

	public Physics(float gravity)
	{
		this.gravity = gravity;
	}

	public override void Initialize()
	{
		box = NullableGetComponent<ColliderTemplate>();

		if (box != null) box.automaticSteps = false;
	}

	public override void Update()
	{
		if (box != null) ResolveCollision();
		convVelocity = velocity / 50;
		ApplyGravity();
		ApplyVelocity();
		if (box != null) box.Step(); // phys step
	}

	private void ApplyVelocity() => transform.position += velocity * Time.deltaTime;
	private void ApplyGravity() => velocity.Y -= 9.81f * gravity * Time.deltaTime;

	public void ResolveCollision()
	{
		touchingTop = false;
		touchingRight = false;
		touchingLeft = false;
		touchingBottom = false;

		List<IHit> hits = box.GetHits();


		foreach (var item in hits)
		{
			if (item.Normal.X == -1)
			{
				touchingRight = true;
				velocity = new Vector2(Math.Clamp(velocity.X, int.MinValue, .1f), velocity.Y);
			}
			if (item.Normal.X == 1)
			{
				touchingLeft = true;
				velocity = new Vector2(Math.Clamp(velocity.X, -.1f, int.MaxValue), velocity.Y);
			}
			if (item.Normal.Y == -1)
			{
				touchingBottom = true;
				velocity = new Vector2(velocity.X, Math.Clamp(velocity.Y, -.1f, int.MaxValue));
			}
			if (item.Normal.Y == 1)
			{
				touchingTop = true;
				velocity = new Vector2(velocity.X, Math.Clamp(velocity.Y, int.MinValue, .1f));
			}
		}


		//if (NullableGetComponent<SimplePlatformer>() != null)
		//{
		//	Say.Line($"bottom = {touchingBottom} top = {touchingTop} left = {touchingLeft} right = {touchingRight}");

		//	foreach (var item in box.GetHits())
		//	{
		//		Say.Line(item.Normal + " item normal");
		//	}
		//}
	}
}