using HBLibrary.Services.Security.Cryptography.Keys;
using HBLibrary.Services.Security.Cryptography;
using System;
using System.Security.Cryptography;

namespace HBLibrary.Services.Security.Cryptography.Rsa;
public class RsaCryptoService : IRsaCryptoService {
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
        RsaKeyHelper.FromByteArray(rsa, key.Key);
#endif
        return rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA512);
    }

    public RSA GetNewRSA(int keySizeInBits = 2048) {
        return RSA.Create(keySizeInBits);
    }

    public RsaKey GeneratePublicKey(RSA rsa) {
#if NET5_0_OR_GREATER
        return new RsaKey(rsa.ExportRSAPublicKey(), rsa.KeySize, true);
#elif NET472_OR_GREATER
        return new RsaKey(rsa.ToByteArray(false), rsa.KeySize, true);
#endif
    }

    public RsaKey GeneratePrivateKey(RSA rsa) {
#if NET5_0_OR_GREATER
        return new RsaKey(rsa.ExportRSAPrivateKey(), rsa.KeySize, false);
#elif NET472_OR_GREATER
        return new RsaKey(rsa.ToByteArray(true), rsa.KeySize, true);
#endif
    }
}
