﻿using HBLibrary.Services.Security.Cryptography.Keys;

namespace HBLibrary.Services.Security.Cryptography;

public interface ICryptoService {
    byte[] Encrypt(byte[] data, IKey key);
    byte[] Decrypt(byte[] cipher, IKey key);

    IKey[] GenerateKeys(int keySize);
}