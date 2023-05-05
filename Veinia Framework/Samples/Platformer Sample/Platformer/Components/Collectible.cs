using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace VeiniaFramework.Samples.Platformer
{
	public class Collectible : Component
	{
		public override bool OnCollide(Fixture sender, Fixture other, Contact contact)
		{
			var tag = (Player)other.Body.Tag;
			if (tag != null) Collect();

			return true;
		}

		private void Collect() => DestroyGameObject();
	}
}