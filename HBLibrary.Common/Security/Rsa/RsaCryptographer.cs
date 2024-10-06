using HBLibrary.Common.Security.Keys;
using System.Security.Cryptography;

namespace HBLibrary.Common.Security.Rsa;
public class RsaCryptographer : IRsaCryptographer {
    public byte[] Decrypt(byte[] cipher, RsaKey key) {
        RSA rsa = RSA.Create(key.KeySize);
        if (key.IsPublic)
            throw new ArgumentException("Cannot decrypt with a public key.");

#if NET5_0_OR_GREATER
        rsa.ImportRSAPrivateKey(key.Key, out int bytesRead);
#elif NET472_OR_GREATER
        rsa.FromByteArray(key.Key);
#endif
        return rsa.Decrypt(cipher, RSAEncryptionPadding.OaepSHA512);
    }

    public byte[] Encrypt(byte[] data, RsaKey key) {
        RSA rsa = RSA.Create(key.KeySize);
        if (!key.IsPublic)
            throw new ArgumentException("Cannot encrypt with a private key.");

#if NET5_0_OR_GREATER
        rsa.ImportRSAPublicKey(key.Key, out int bytesRead);
#elif NET472_OR_GREATER
        rsa.FromByteArray(key.Key);
#endif
        return rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA512);
    }

    public RSA GetNewRSA(int keySizeInBits = 2048) {
        return RSA.Create(keySizeInBits);
    }

    public RsaKey GeneratePublicKey(RSA rsa) {
        return rsa.GeneratePublicKey();
    }

    public RsaKey GeneratePrivateKey(RSA rsa) {
        return rsa.GeneratePrivateKey();
    }
}
