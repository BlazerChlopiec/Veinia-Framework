namespace Veinia.RunningBlocks
{
	public class FollowMouse : Component
	{
		public override void Update() => transform.position = Globals.input.GetMouseWorldPosition();
	}
}
