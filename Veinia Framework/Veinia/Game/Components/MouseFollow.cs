public class MouseFollow : Component
{
	public override void Update()
	{
		transform.position = Globals.input.GetMouseWorldPosition();
	}
}
