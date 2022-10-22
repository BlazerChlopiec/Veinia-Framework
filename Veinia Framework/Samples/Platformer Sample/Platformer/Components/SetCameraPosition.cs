namespace Veinia.Platformer
{
	public class SetCameraPosition : Component
	{
		public override void LateUpdate() => transform.position = Globals.camera.GetPosition();
	}
}
