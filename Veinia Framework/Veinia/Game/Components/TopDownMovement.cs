using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public class TopDownMovement : Component
{
	public Vector2 velocity;
	public float moveSpeed = 7f;

	//Physics physics;

	public override void Initialize()
	{
		//physics = GetComponent<Physics>();
	}

	public override void Update()
	{
		//physics.velocity.X = moveSpeed * Globals.input.horizontal * 50 * Time.deltaTime;
		//physics.velocity.Y = moveSpeed * Globals.input.vertical * 50 * Time.deltaTime;
	}
}