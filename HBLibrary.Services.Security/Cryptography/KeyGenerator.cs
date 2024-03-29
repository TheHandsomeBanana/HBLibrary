﻿using HBLibrary.Services.Security.Cryptography.Keys;
using HBLibrary.Services.Security.Cryptography.Rsa;
using System.Security.Cryptography;

namespace HBLibrary.Services.Security.Cryptography;
public static class KeyGenerator {
    public static AesKey GenerateAesKey(int keySize = 256) {
        System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create();
        aes.KeySize = keySize;

        return new AesKey(aes.Key, aes.IV);
    }

    public static RsaKey[] GenerateRsaKeys(int keySizeInBits = 2048) {
        RSA rsa = RSA.Create(keySizeInBits);

        return [rsa.GeneratePublicKey(), rsa.GeneratePrivateKey()];
    }

#if NET5_0_OR_GREATER
    public static RsaKey GeneratePublicKey(this RSA rsa) {
        return new RsaKey(rsa.ExportRSAPublicKey(), rsa.KeySize, true);
    }

    public static RsaKey GeneratePrivateKey(this RSA rsa) {
        return new RsaKey(rsa.ExportRSAPrivateKey(), rsa.KeySize, false);
    }
#elif NET472_OR_GREATER
    public static RsaKey GeneratePublicKey(this RSA rsa) {
        return new RsaKey(rsa.ToByteArray(false), rsa.KeySize, true);
    }

    public static RsaKey GeneratePrivateKey(this RSA rsa) {
        return new RsaKey(rsa.ToByteArray(true), rsa.KeySize, false);
    }
#endif

#if WINDOWS
    public static DPEntropy GenerateDPApiKey(byte[] entropy) {
        return new DPEntropy(entropy);
    }
#endif
}
