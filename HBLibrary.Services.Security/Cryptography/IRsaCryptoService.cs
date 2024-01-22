using HBLibrary.Services.Security.Cryptography.Keys;
using System.Security.Cryptography;

namespace HBLibrary.Services.Security.Cryptography; 
public interface IRsaCryptoService : ICryptoService {
    byte[] Encrypt(byte[] data, RsaKey key);
    byte[] Decrypt(byte[] cipher, RsaKey key);

    RSA GetNewRSA(int keySizeInBits = 2048);
    RsaKey GeneratePublicKey(RSA rsa);
    RsaKey GeneratePrivateKey(RSA rsa);
}
