using System;

namespace VeiniaFramework.Platformer
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
