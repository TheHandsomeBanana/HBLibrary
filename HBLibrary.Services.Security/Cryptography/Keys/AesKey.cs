﻿namespace HBLibrary.Services.Security.Cryptography.Keys; 
public class AesKey : IKey {
    public byte[] Key { get; set; }
    public byte[] IV { get; set; }

    public string Name => nameof(AesKey);

    public AesKey(byte[] key, byte[] iV) {
        Key = key;
        IV = iV;
    }
}
