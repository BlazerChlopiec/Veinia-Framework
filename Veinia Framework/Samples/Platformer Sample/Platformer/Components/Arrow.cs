namespace Veinia.Platformer
{
	public class Arrow : Component
	{
		public override void Update()
		{
			var player = FindComponentOfType<Player>();
			if (player != null) transform.LookAt(player.transform.position);
		}
	}
}