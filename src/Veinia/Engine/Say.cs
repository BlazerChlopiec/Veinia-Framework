using System;
using System.Diagnostics;

namespace VeiniaFramework
{
	public class Say
	{
		public static void Line<T1>(T1 text) => Write(text.ToString());
		public static void Line<T1>(string desc, T1 text) => Write($"{desc}: {text},");

		private static void Write(string text)
		{
			if (OperatingSystem.IsBrowser()) Console.WriteLine(text);
			else Debug.WriteLine(text);
		}
	}
}