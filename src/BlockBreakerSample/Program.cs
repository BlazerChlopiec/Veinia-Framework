using System;

namespace VeiniaFramework.Samples.BlockBreaker
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			using (var game = new BlockBreakerGame())
				game.Run();
		}
	}
}
