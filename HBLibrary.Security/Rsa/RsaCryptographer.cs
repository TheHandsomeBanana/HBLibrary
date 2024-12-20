using HBLibrary.Interface.Security.Keys;
using HBLibrary.Interface.Security.Rsa;
using HBLibrary.Security;
using HBLibrary.Services.Security.Rsa;
using System.Security.Cryptography;

namespace HBLibrary.Security.Rsa;
public class RsaCryptographer : IRsaCryptographer {
    public byte[] Decrypt(byte[] cipher, RsaKey key) {
        if(key.IsDisposed) {
            throw new ObjectDisposedException(nameof(key));
        }

        RSA rsa = RSA.Create(key.KeySize!.Value);
        if (key.IsPublic!.Value)
            throw new ArgumentException("Cannot decrypt with a public key.");

#if NET5_0_OR_GREATER
        rsa.ImportRSAPrivateKey(key.Key!, out int bytesRead);
#elif NET472_OR_GREATER
        rsa.FromByteArray(key.Key!);
#endif
        return rsa.Decrypt(cipher, RSAEncryptionPadding.OaepSHA512);
    }

    public byte[] Encrypt(byte[] data, RsaKey key) {
        if (key.IsDisposed) {
            throw new ObjectDisposedException(nameof(key));
        }

        RSA rsa = RSA.Create(key.KeySize!.Value);
        if (!key.IsPublic!.Value)
            throw new ArgumentException("Cannot encrypt with a private key.");

#if NET5_0_OR_GREATER
        rsa.ImportRSAPublicKey(key.Key!, out int bytesRead);
#elif NET472_OR_GREATER
        rsa.FromByteArray(key.Key!);
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
