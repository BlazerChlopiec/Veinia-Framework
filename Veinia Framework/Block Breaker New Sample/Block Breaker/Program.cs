using System;
using Veinia.BlockBreaker;

namespace Block_Breaker_New_Sample
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
