using System.Diagnostics;

namespace Veinia
{
	public class Say
	{
		public static void Line<T1>(T1 text) => Debug.WriteLine(text.ToString());
		public static void Line<T1>(string desc, T1 text) => Debug.WriteLine(desc + text.ToString());
	}
}
