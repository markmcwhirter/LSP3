using System.Security.Cryptography;

namespace LSP3;


public static class EncryptionHelper
{
	public const string DefaultPassword = "079857&^%&567464^%$654^%4654(*^987()87098)*";

	public static string EncryptString(string plainText, string password)
	{
		byte[] salt = GenerateSalt();

		byte[] key = DeriveKeyFromPassword(password, salt);

		using (Aes aes = Aes.Create())
		{
			aes.Key = key;
			aes.IV = new byte[aes.BlockSize / 8];  // Generate random IV 
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;

			using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
			using (MemoryStream msEncrypt = new MemoryStream())
			using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
			{
				using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
				{
					// Write the salt to the beginning of the stream
					swEncrypt.Write(Convert.ToBase64String(salt));
					swEncrypt.Write("-");
					swEncrypt.Write(plainText);
				}
				byte[] encrypted = msEncrypt.ToArray();
				return Convert.ToBase64String(encrypted);
			}
		}
	}

	private static byte[] GenerateSalt()
	{
		byte[] salt = new byte[32]; // Recommended salt size
		using (var rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(salt);
		}
		return salt;
	}

	private static byte[] DeriveKeyFromPassword(string password, byte[] salt)
	{
		using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, 100000)) // High iterations for security
		{
			return rfc2898.GetBytes(32); // Get a 256-bit key (AES-256)
		}
	}
}