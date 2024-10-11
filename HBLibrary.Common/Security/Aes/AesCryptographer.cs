using HBLibrary.Common.Security.Keys;
using System.Security.Cryptography;
using HBLibrary.Common.Extensions;

namespace HBLibrary.Common.Security.Aes;

public class AesCryptographer : IAesCryptographer {
    public byte[] Decrypt(byte[] cipher, AesKey key) {
        ICryptoTransform decryptor = System.Security.Cryptography.Aes.Create().CreateDecryptor(key.Key, key.IV);

        using (MemoryStream ms = new MemoryStream(cipher)) {
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
                using (MemoryStream decryptedStream = new MemoryStream()) {
                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    // Read the full content by looping until no more data is available
                    while ((bytesRead = cs.Read(buffer, 0, buffer.Length)) > 0) {
                        decryptedStream.Write(buffer, 0, bytesRead);
                    }

                    return decryptedStream.ToArray();
                }
            }
        }
    }

    public byte[] Encrypt(byte[] data, AesKey key) {
        ICryptoTransform encryptor = System.Security.Cryptography.Aes.Create().CreateEncryptor(key.Key, key.IV);

        using (MemoryStream ms = new MemoryStream()) {
            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                cs.Write(data);
            }

            return ms.ToArray();
        }
    }

    public AesKey GenerateKey(int keySize = 256) => KeyGenerator.GenerateAesKey(keySize);
}
