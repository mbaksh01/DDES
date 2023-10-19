using DDES.Server.Services.Abstractions;
using System.Buffers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DDES.Server.Services;

internal sealed class EncryptionService : IEncryptionService
{
    private readonly ILogger<EncryptionService> _logger;
    private readonly IClientService _clientService;
    private readonly RSACryptoServiceProvider _rsa = new();

    public EncryptionService(
        ILogger<EncryptionService> logger,
        IClientService clientService)
    {
        _clientService = clientService;
        _logger = logger;
    }

    public byte[] GetPublicKey()
    {
        return _rsa.ExportRSAPublicKey();
    }

    public byte[] Encrypt<TModel>(TModel data, Guid clientId)
    {
        _logger.LogInformation("Encrypting message.");

        using RSACryptoServiceProvider rsa = new();

        byte[] clientPublicKey = _clientService.GetPublicKey(clientId);

        rsa.ImportRSAPublicKey(clientPublicKey, out _);

        string serialisedData = JsonSerializer.Serialize(data);

        byte[] buffer = ArrayPool<byte>.Shared.Rent(serialisedData.Length);

        int numberOfEncodedBytes = Encoding.UTF8.GetBytes(serialisedData.AsSpan(), buffer);

        byte[] encryptedBytes = rsa.Encrypt(buffer[..numberOfEncodedBytes], false);

        ArrayPool<byte>.Shared.Return(buffer);

        _logger.LogInformation("Encrypted message.");

        return encryptedBytes;
    }

    public TModel? Decrypt<TModel>(string data)
    {
        _logger.LogInformation("Decrypting message.");

        byte[] decryptedData = _rsa.Decrypt(Encoding.UTF8.GetBytes(data), false);

        data = Encoding.UTF8.GetString(decryptedData.AsSpan());

        _logger.LogInformation("Decrypted message.");

        return JsonSerializer.Deserialize<TModel>(data);
    }

    public void Dispose()
    {
        _rsa.Dispose();
    }
}
