namespace DDES.Common.Services.Abstractions;

public interface IEncryptionService : IDisposable
{
    TModel? Decrypt<TModel>(string data);

    byte[] Encrypt<TModel>(TModel data, Guid clientId);

    byte[] GetPublicKey();
}