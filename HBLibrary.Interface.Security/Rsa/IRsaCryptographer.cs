using HBLibrary.Interface.Security.Keys;
using System.Security.Cryptography;

namespace HBLibrary.Interface.Security.Rsa;
public interface IRsaCryptographer {
    byte[] Encrypt(byte[] data, RsaKey key);
    byte[] Decrypt(byte[] cipher, RsaKey key);

    RSA GetNewRSA(int keySizeInBits = 2048);
    RsaKey GeneratePublicKey(RSA rsa);
    RsaKey GeneratePrivateKey(RSA rsa);
}
