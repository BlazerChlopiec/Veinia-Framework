using System;

namespace VeiniaFramework.Samples.Platformer
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			using (var game = new PlatformerGame())
				game.Run();
		}
	}
}
