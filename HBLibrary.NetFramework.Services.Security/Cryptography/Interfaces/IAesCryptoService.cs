﻿using HBLibrary.NetFramework.Services.Security.Cryptography.Keys;

namespace HBLibrary.NetFramework.Services.Security.Cryptography.Interfaces {
    public interface IAesCryptoService : ICryptoService {
        byte[] Encrypt(byte[] data, AesKey key);
        byte[] Decrypt(byte[] cipher, AesKey key);
        AesKey GenerateKey(int keySize = 256);
    }
}
