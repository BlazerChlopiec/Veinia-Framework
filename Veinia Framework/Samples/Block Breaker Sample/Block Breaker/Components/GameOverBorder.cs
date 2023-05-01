using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace VeiniaFramework.BlockBreaker
{
	public class GameOverBorder : Component
	{
		public override void OnCollide(Fixture sender, Fixture other, Contact contact)
		{
			FindComponentOfType<UI>().ShowLoseScreen();
		}
	}
}