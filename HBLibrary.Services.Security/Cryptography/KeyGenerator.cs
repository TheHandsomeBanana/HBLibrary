using HBLibrary.Services.Security.Cryptography.Keys;
using HBLibrary.Services.Security.Cryptography.Rsa;
using System.Security.Cryptography;

namespace HBLibrary.Services.Security.Cryptography;
public static class KeyGenerator {
    public static AesKey GenerateAesKey(int keySize = 256) {
        System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create();
        aes.KeySize = keySize;

        return new AesKey(aes.Key, aes.IV);
    }

#if NET5_0_OR_GREATER
    public static RsaKey[] GenerateRsaKeys(int keySizeInBits = 2048) {
        RSA rsa = RSA.Create(keySizeInBits);

        return [new RsaKey(rsa.ExportRSAPublicKey(), keySizeInBits, true), new RsaKey(rsa.ExportRSAPrivateKey(), keySizeInBits, false)];
    } 

    public static RsaKey GeneratePublicKey(RSA rsa) {
        return new RsaKey(rsa.ExportRSAPublicKey(), rsa.KeySize, true);
    }

    public static RsaKey GeneratePrivateKey(RSA rsa) {
        return new RsaKey(rsa.ExportRSAPrivateKey(), rsa.KeySize, false);
    }
#elif NET472_OR_GREATER
    public static RsaKey[] GenerateRsaKeys(int keySizeInBits = 2048) {
        RSA rsa = RSA.Create(keySizeInBits);

        return new RsaKey[] { new RsaKey(rsa.ToByteArray(false), keySizeInBits, true), new RsaKey(rsa.ToByteArray(true), keySizeInBits, false) };
    }

    public static RsaKey GeneratePublicKey(RSA rsa) {
        return new RsaKey(rsa.ToByteArray(false), rsa.KeySize, true);
    }

    public static RsaKey GeneratePrivateKey(RSA rsa) {
        return new RsaKey(rsa.ToByteArray(true), rsa.KeySize, false);
    }
#endif
}
