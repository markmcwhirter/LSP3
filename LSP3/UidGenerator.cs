namespace LSP3;

using System;
using System.Security.Cryptography;

public class UidGenerator
{
    private static readonly char[] AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
    private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

    public static string GenerateHtmlFriendlyUid(int length = 10)
    {
        var uid = new char[length];
        var randomBytes = new byte[length];

        Rng.GetBytes(randomBytes); // Cryptographically secure randomness

        for (int i = 0; i < length; i++)
        {
            // Convert random bytes to allowed characters
            uid[i] = AllowedChars[randomBytes[i] % AllowedChars.Length];
        }

        return new string(uid);
    }
}
