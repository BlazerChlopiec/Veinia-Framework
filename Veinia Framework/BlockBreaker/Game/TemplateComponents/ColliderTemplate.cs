using Humper;
using Humper.Responses;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

public class ColliderTemplate : Component, IDisposable
{
	protected RectangleF firstFrameBox;
	protected IBox box;
	private IMovement movement;

	protected Vector2 offset;

	public bool automaticSteps = true;

	public RectangleF rect
	{
		get
		{
			if (box == null) return RectangleF.Empty;
			else
				return new RectangleF(box.Bounds.X - Transform.PIXELS_PER_UNIT / 2, box.Bounds.Y - Transform.PIXELS_PER_UNIT / 2, box.Bounds.Width, box.Bounds.Height);
		}
	}

	CollisionResponses collisionResponses;

	public ColliderTemplate(CollisionResponses type)
	{
		collisionResponses = type;
	}

	public override void Initialize()
	{
		box = Globals.world.Create(firstFrameBox.X, firstFrameBox.Y, firstFrameBox.Width, firstFrameBox.Height);
	}

	public override void Update()
	{
		if (automaticSteps)
			Step();
	}

	public void Step()
	{
		movement = box.Move(transform.screenPos.X + offset.X, transform.screenPos.Y + offset.Y, (collision) => collisionResponses);

		if (movement.HasCollided)
		{
			transform.position = Transform.ScreenToWorldPos(new Vector2(movement.Destination.X - offset.X, movement.Destination.Y - offset.Y));
		}
	}
	public List<IHit> GetHits()
	{
		if (movement == null || !parent.isEnabled) return new List<IHit>();
		List<IHit> temp = new List<IHit>();
		foreach (var item in movement.Hits)
		{
			temp.Add(item);
		}
		return temp;
	}

	public void Dispose()
	{
		Globals.world.Remove(box);
		box = null;
	}
}
