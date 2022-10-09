namespace Veinia.Platformer
{
	public class RotationTest : Component
	{
		public override void Update() => transform.LookAt(FindComponentOfType<Player>().transform.position);
	}
}