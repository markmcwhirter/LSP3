﻿using System.Security.Cryptography;
using System.Text;

namespace LSP3;

public class EncryptionService
{
    private readonly byte[] IV =
    {
        0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
        0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
    };

    readonly string PASSPHRASE = "098asdf07s9d8f790a87sdf9a87sd98a7sdf987asdf987asd9f87as98g7a987sgd";

    private static byte[] DeriveKeyFromPassword(string password)
    {
        var emptySalt = Array.Empty<byte>();
        var iterations = 1000;
        var desiredKeyLength = 16; // 16 bytes equal 128 bits.
        var hashMethod = HashAlgorithmName.SHA384;
        return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password),
                                         emptySalt,
                                         iterations,
                                         hashMethod,
                                         desiredKeyLength);
    }

    public async Task<byte[]> EncryptAsync(string clearText)
    {
        using Aes aes = Aes.Create();
        aes.Key = DeriveKeyFromPassword(PASSPHRASE);
        aes.IV = IV;

        using MemoryStream output = new();
        using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);

        await cryptoStream.WriteAsync(Encoding.Unicode.GetBytes(clearText));
        await cryptoStream.FlushFinalBlockAsync();

        return output.ToArray();
    }

    public async Task<string> DecryptAsync(byte[] encrypted)
    {
        using Aes aes = Aes.Create();
        aes.Key = DeriveKeyFromPassword(PASSPHRASE);
        aes.IV = IV;

        using MemoryStream input = new(encrypted);
        using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);

        using MemoryStream output = new();
        await cryptoStream.CopyToAsync(output);

        return Encoding.Unicode.GetString(output.ToArray());
    }
}