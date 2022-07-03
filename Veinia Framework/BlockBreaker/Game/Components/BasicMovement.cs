
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

public class BasicMovement : Component
{
	private float speed;


	public BasicMovement(float speed)
	{
		this.speed = speed;
	}

	public override void Update()
	{
		GetComponent<Physics>().velocity = Utils.SafeNormalize(new Vector2(Globals.input.horizontal, Globals.input.vertical)) * speed;
	}
}