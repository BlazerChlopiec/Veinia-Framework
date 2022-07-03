using Microsoft.Xna.Framework.Input;

public class MouseFollow : Component
{
	public override void Initialize()
	{
	}

	public override void Update()
	{
		transform.position = Globals.input.GetMouseWorldPosition();
	}
}
