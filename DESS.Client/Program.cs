using System.Security.Cryptography;
using System.Text;

// Create server public and private key.
RSACryptoServiceProvider server = new(2048);

// Create client public and private key.
RSACryptoServiceProvider client = new(2048);

string helloWorld = "Hello World";

byte[] helloWorld_ = server.Encrypt(Encoding.UTF8.GetBytes(helloWorld), false);

// import the client public key to use for encryption.
server.ImportRSAPublicKey(client.ExportRSAPublicKey(), out _);

// encrypt using clients public key
byte[] cipherText = server.Encrypt(Encoding.UTF8.GetBytes("This is sample text."), false);

// log
Console.WriteLine(Convert.ToBase64String(cipherText));

// TODO: Send message.

// decrypt using clients private key.
byte[] cipherText2 = client.Decrypt(cipherText, false);

// log
Console.WriteLine(Encoding.UTF8.GetString(cipherText2));

Console.WriteLine(Encoding.UTF8.GetString(server.Decrypt(helloWorld_, false)));
