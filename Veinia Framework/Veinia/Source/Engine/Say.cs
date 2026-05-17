using System;

namespace VeiniaFramework
{
	public class Say
	{
		public static void Line<T1>(T1 text) => Console.WriteLine(text.ToString());
		public static void Line<T1>(string desc, T1 text) => Console.WriteLine(desc + text.ToString());
	}
}