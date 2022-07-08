using System;

namespace Veinia.RunningBlocks
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			using (var game = new RunningBlocksGame())
				game.Run();
		}
	}
}
