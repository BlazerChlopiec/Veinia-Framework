namespace Veinia.Platformer
{
	public class SetCameraPosition : Component
	{
		public override void Update() => transform.position = Globals.camera.GetPosition();
	}
}
