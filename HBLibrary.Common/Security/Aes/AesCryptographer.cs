

using HBLibrary.Common.Extensions;
using HBLibrary.Common.Security.Keys;


using System.Security.Cryptography;

namespace HBLibrary.Common.Security.Aes;

public class AesCryptographer : IAesCryptographer {
    public byte[] Decrypt(byte[] cipher, AesKey key) {
        ICryptoTransform decryptor = System.Security.Cryptography.Aes.Create().CreateDecryptor(key.Key, key.IV);

        using (MemoryStream ms = new MemoryStream(cipher)) {
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
                byte[] cipherBuffer = cs.Read(cipher.Length);

                // Remove trailing null chars
                int i = cipherBuffer.Length - 1;
                while (i >= 0 && cipherBuffer[i] == '\0')
                    i--;

                byte[] targetBuffer = new byte[i + 1];
                Array.Copy(cipherBuffer, targetBuffer, i + 1);
                return targetBuffer;
            }
        }
    }

    public byte[] Encrypt(byte[] data, AesKey key) {
        ICryptoTransform encryptor = System.Security.Cryptography.Aes.Create().CreateEncryptor(key.Key, key.IV);

        using (MemoryStream ms = new MemoryStream()) {
            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                cs.Write(data);
                cs.FlushFinalBlock();
            }

            return ms.ToArray();
        }
    }

    public async Task<byte[]> DecryptAsync(byte[] cipher, AesKey key) {
        ICryptoTransform decryptor = System.Security.Cryptography.Aes.Create().CreateDecryptor(key.Key, key.IV);

        using (MemoryStream ms = new MemoryStream(cipher)) {
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
                byte[] cipherBuffer = await cs.ReadAsync(cipher.Length);

                int i = cipherBuffer.Length - 1;
                while (i >= 0 && cipherBuffer[i] == '\0')
                    i--;

                byte[] targetBuffer = new byte[i + 1];
                Array.Copy(cipherBuffer, targetBuffer, i + 1);
                return targetBuffer;
            }
        }
    }

    public async Task<byte[]> EncryptAsync(byte[] data, AesKey key) {
        ICryptoTransform encryptor = System.Security.Cryptography.Aes.Create().CreateEncryptor(key.Key, key.IV);

        using (MemoryStream ms = new MemoryStream()) {
            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                await cs.WriteAsync(data);
            }

            return ms.ToArray();
        }
    }

    public AesKey GenerateKey(int keySize = 256) => KeyGenerator.GenerateAesKey(keySize);
}
