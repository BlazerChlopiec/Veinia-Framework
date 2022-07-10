using System;
using Veinia.BlockBreaker;

namespace Veinia.BlockBreaker
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
