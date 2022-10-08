using System;

namespace Veinia.Platformer
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
