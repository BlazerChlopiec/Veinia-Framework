using System;

namespace VeiniaFramework.Samples.Physics
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			using (var game = new PhysicsGame())
				game.Run();
		}
	}
}
