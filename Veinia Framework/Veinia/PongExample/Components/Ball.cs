using Microsoft.Xna.Framework;
using MonoGame.Extended.Collisions;
using System;

public class Ball : Component
{
	Paddle paddle;
	CirclePhysics physics;

	private float speed = 7;

	bool launched;


	public override void Initialize()
	{
		paddle = FindComponentOfType<Paddle>();
		physics = GetComponent<CirclePhysics>();

		physics.onCollisionEnter += CollisionEnter;
	}

	public override void Update()
	{
		if (!launched) transform.position = paddle.transform.position + Vector2.UnitY;

		if (Globals.input.GetMouseButtonDown(0) && !launched)
		{
			launched = true;
			physics.velocity = Vector2.One * speed;
		}
	}

	private void CollisionEnter(CollisionEventArgs collisionInfo)
	{
		Say.Line(collisionInfo.PenetrationVector);

		if (Math.Abs(collisionInfo.PenetrationVector.Y) > Math.Abs(collisionInfo.PenetrationVector.X))
		{
			//touched Y
			physics.velocity *= new Vector2(1, -1);
		}

		if (Math.Abs(collisionInfo.PenetrationVector.X) > Math.Abs(collisionInfo.PenetrationVector.Y))
		{
			//touched X
			physics.velocity *= new Vector2(-1, 1);
		}
	}
}