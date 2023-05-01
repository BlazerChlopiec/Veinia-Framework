using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace VeiniaFramework.Platformer
{
	public class Collectible : Component
	{
		public override void OnCollide(Fixture sender, Fixture other, Contact contact)
		{
			var tag = (Player)other.Body.Tag;
			if (tag != null) Collect();
		}

		private void Collect() => DestroyGameObject();
	}
}