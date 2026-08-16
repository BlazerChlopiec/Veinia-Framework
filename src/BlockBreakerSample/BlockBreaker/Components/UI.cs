using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace VeiniaFramework.Samples.BlockBreaker
{
	public class UI : Component
	{
		public void ResetGameWithTransition()
		{
			Time.stop = true;

			var trans = Instantiate(
				new Transform(Vector2.Zero),
				new List<Component>
				{
					new Transition(() => Globals.loader.Reload(),"Sprites/Transition")
				}, isStatic: false, dontDestroyOnLoad: true);
		}
	}
}