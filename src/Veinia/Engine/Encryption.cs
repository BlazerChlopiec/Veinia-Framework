using System.Security.Cryptography;
using System.Text;

namespace VeiniaFramework
{
	public static class Encryption
	{
		private static byte[] KEY = new byte[] { 21, 149, 172, 138, 96, 26, 103, 141, 239, 44, 213, 33, 199, 16, 33, 121, 196, 164, 59, 58, 129, 211, 44, 211 };
		private static byte[] IV = new byte[] { 196, 215, 52, 8, 30, 3, 40, 93 };

		public static string Decrypt(byte[] s)
		{
			var tripleDes = TripleDES.Create();
			tripleDes.IV = IV;
			tripleDes.Key = KEY;

			ICryptoTransform transform = tripleDes.CreateDecryptor();

			var plainText = transform.TransformFinalBlock(s, 0, s.Length);
			return Encoding.UTF8.GetString(plainText);
		}

		public static byte[] Encrypt(string s)
		{
			var buffer = Encoding.UTF8.GetBytes(s);

			var tripleDes = TripleDES.Create();
			tripleDes.IV = IV;
			tripleDes.Key = KEY;

			ICryptoTransform transform = tripleDes.CreateEncryptor();

			return transform.TransformFinalBlock(buffer, 0, buffer.Length);
		}
	}
}
