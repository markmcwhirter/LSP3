using Microsoft.AspNetCore.DataProtection.KeyManagement;

using System.Security.Cryptography;
using System.Text;

namespace LSP3;


public static class EncryptionHelper
{
	public const string DefaultPassword = "ThisIsTheLSPDefaultPassword";

	private static readonly SymmetricAlgorithm _algorithm = Aes.Create();


	public static string Encrypt(string plainText)
	{
		byte[] _key = Encoding.UTF8.GetBytes(DefaultPassword); // Get UTF-8 bytes

		using var encryptor = _algorithm.CreateEncryptor(_key, _algorithm.IV);

		// Use MemoryStream and CryptoStream for efficient encryption
		using var memoryStream = new MemoryStream();
		using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
		{
			// Ensure proper string encoding (UTF8 recommended)
			using var writer = new StreamWriter(cryptoStream, Encoding.UTF8);
			writer.Write(plainText);
		}

		// Include IV in the output for correct decryption
		var iv = _algorithm.IV;
		var encrypted = memoryStream.ToArray();
		var combined = new byte[iv.Length + encrypted.Length];
		Array.Copy(iv, 0, combined, 0, iv.Length);
		Array.Copy(encrypted, 0, combined, iv.Length, encrypted.Length);

		// Return Base64 encoded result for safe string representation
		return Convert.ToBase64String(combined);
	}

	public static string Decrypt(string cipherText)
	{
		var combined = Convert.FromBase64String(cipherText);
		var iv = new byte[_algorithm.IV.Length];
		var encrypted = new byte[combined.Length - iv.Length];

		Array.Copy(combined, 0, iv, 0, iv.Length);
		Array.Copy(combined, iv.Length, encrypted, 0, encrypted.Length);

		byte[] _key = Convert.FromBase64String(DefaultPassword);
		using var decryptor = _algorithm.CreateDecryptor(_key, iv);

		using var memoryStream = new MemoryStream(encrypted);
		using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
		using var reader = new StreamReader(cryptoStream, Encoding.UTF8);
		return reader.ReadToEnd();
	}


}