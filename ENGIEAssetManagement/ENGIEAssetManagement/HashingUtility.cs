using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace ENGIEAssetManagement
{
    public class HashingUtility
    {
		// can access modifiers be reduced
		public static string GenerateSalt()
		{
			byte[] bytes = new byte[128 / 8];
			var rng = new RNGCryptoServiceProvider();
			rng.GetBytes(bytes); // fill bytes array with secure random values
			return Convert.ToBase64String(bytes);
		}

		public static string GenerateHash(string input, string salt)
		{

			var bytesToHash = Encoding.UTF8.GetBytes(input);
			var bytesSalt = Encoding.UTF8.GetBytes(salt);

			var byteResult = new Rfc2898DeriveBytes(bytesToHash, bytesSalt, 10000);
			return Convert.ToBase64String(byteResult.GetBytes(24));
		}
	}
}
