using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DDES.Common.Helpers;

public static class EncryptionHelper
{
    private const string EncryptionKey = "df1c39f3-8a3a-4ac7-9a5c-5b86b7d54f6d";

    private static readonly byte[] EncryptionSalt =
        "Pro-Supply-Chain-2023"u8.ToArray();

    /// <summary>
    /// Encrypts the <paramref name="unencryptedText"/> using a shared key and
    /// salt.
    /// </summary>
    /// <param name="unencryptedText">String to encrypt.</param>
    /// <returns>The <see cref="unencryptedText"/> as encrypted text.</returns>
    public static string Encrypt<TModel>(TModel model)
    {
        byte[] modelBytes = JsonSerializer.SerializeToUtf8Bytes(model);

        using Aes encryptor = Aes.Create();

        using Rfc2898DeriveBytes pdb = new(
            EncryptionKey,
            EncryptionSalt,
            1000,
            HashAlgorithmName.SHA1);

        encryptor.Key = pdb.GetBytes(32);
        encryptor.IV = pdb.GetBytes(16);

        using MemoryStream ms = new();

        using CryptoStream cs = new(
            ms,
            encryptor.CreateEncryptor(),
            CryptoStreamMode.Write);

        cs.Write(modelBytes);
        cs.Close();

        return Convert.ToBase64String(ms.ToArray());
    }


    /// <summary>
    /// Decrypts the <see cref="encryptedText"/> using a shared key and salt.
    /// </summary>
    /// <param name="encryptedText">String to decrypt.</param>
    /// <returns>The <paramref name="encryptedText"/> decrypted.</returns>
    public static TModel? Decrypt<TModel>(string encryptedText)
    {
        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

        using Aes encryptor = Aes.Create();

        using Rfc2898DeriveBytes pdb = new(
            EncryptionKey,
            EncryptionSalt,
            1000,
            HashAlgorithmName.SHA1);

        encryptor.Key = pdb.GetBytes(32);
        encryptor.IV = pdb.GetBytes(16);

        using MemoryStream ms = new();

        using CryptoStream cs = new(
            ms,
            encryptor.CreateDecryptor(),
            CryptoStreamMode.Write);

        cs.Write(encryptedBytes, 0, encryptedBytes.Length);
        cs.Close();

        string unencryptedValue = Encoding.UTF8.GetString(ms.ToArray());

        if (string.IsNullOrWhiteSpace(unencryptedValue))
        {
            return default;
        }

        return JsonSerializer.Deserialize<TModel>(unencryptedValue);
    }
}