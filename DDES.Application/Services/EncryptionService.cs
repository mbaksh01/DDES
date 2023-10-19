using Microsoft.Extensions.Logging;
using System.Buffers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DDES.Application.Services;

internal sealed class EncryptionService
{
    private readonly ILogger<EncryptionService> _logger;
    private readonly RSACryptoServiceProvider _rsa = new();
    private readonly RSACryptoServiceProvider _serverRSA = new();

    public EncryptionService(ILogger<EncryptionService> logger)
    {
        _logger = logger;
    }

    public TModel? Decrypt<TModel>(string data)
    {
        _logger.LogInformation("Decrypting message.");

        byte[] decryptedData = _rsa.Decrypt(Encoding.UTF8.GetBytes(data), false);

        data = Encoding.UTF8.GetString(decryptedData.AsSpan());

        _logger.LogInformation("Decrypted message.");

        return JsonSerializer.Deserialize<TModel>(data);
    }

    public byte[] Encrypt<TModel>(TModel data)
    {
        _logger.LogInformation("Encrypting message.");

        string serialisedData = JsonSerializer.Serialize(data);

        byte[] buffer = ArrayPool<byte>.Shared.Rent(serialisedData.Length);

        int numberOfEncodedBytes = Encoding.UTF8.GetBytes(serialisedData.AsSpan(), buffer);

        byte[] encryptedBytes = _serverRSA.Encrypt(buffer[..numberOfEncodedBytes], false);

        ArrayPool<byte>.Shared.Return(buffer);

        _logger.LogInformation("Encrypted message.");

        return encryptedBytes;
    }

    public byte[] GetPublicKey()
    {
        return _rsa.ExportRSAPublicKey();
    }

    public void SetServerPublicKey(byte[] serverPublicKey)
    {
        _serverRSA.ImportRSAPublicKey(serverPublicKey, out _);
    }

    public void Dispose()
    {
        _rsa.Dispose();
        _serverRSA.Dispose();
    }
}
