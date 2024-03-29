﻿using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace VeiniaFramework.Samples.BlockBreaker
{
	public class GameOverBorder : Component
	{
		public override bool OnCollide(Fixture sender, Fixture other, Contact contact)
		{
			var tag = (Ball)other.Body.Tag;
			if (tag != null) FindComponentOfType<UI>().ShowLoseScreen();

			return true;
		}
	}
}