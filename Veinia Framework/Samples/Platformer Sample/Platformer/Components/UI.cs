using GeonBit.UI;
using GeonBit.UI.Entities;

namespace Veinia.Platformer
{
	public class UI : Component
	{
		public int coinCount;

		RichParagraph coinCounter;


		public override void Initialize()
		{
			coinCounter = new RichParagraph("Coins: {{GOLD}}" + coinCount, Anchor.TopLeft);

			UserInterface.Active.AddEntity(coinCounter);
		}

		public override void Update() => coinCounter.Text = "Coins: {{GOLD}}" + coinCount;
	}
}