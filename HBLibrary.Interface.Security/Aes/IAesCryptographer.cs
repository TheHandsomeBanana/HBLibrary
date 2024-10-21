
using HBLibrary.Interface.Security.Keys;

namespace HBLibrary.Interface.Security.Aes;

public interface IAesCryptographer
{
    byte[] Encrypt(byte[] data, AesKey key);
    byte[] Decrypt(byte[] cipher, AesKey key);
    AesKey GenerateKey(int keySize = 256);
}
