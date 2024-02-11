using HBLibrary.Services.Security.Cryptography.Keys;

namespace HBLibrary.Services.Security.Cryptography.Aes;

public interface IAesCryptographer {
    byte[] Encrypt(byte[] data, AesKey key);
    byte[] Decrypt(byte[] cipher, AesKey key);
    AesKey GenerateKey(int keySize = 256);
}
