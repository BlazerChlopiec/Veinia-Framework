using System;
using VeiniaFramework.BlockBreaker;

namespace VeiniaFramework.BlockBreaker
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
