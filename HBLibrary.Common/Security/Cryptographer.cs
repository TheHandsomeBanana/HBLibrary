using HBLibrary.Common.Security.Aes;
using HBLibrary.Common.Security.Exceptions;
using HBLibrary.Common.Security.Keys;
using HBLibrary.Common.Security.Rsa;
using System.Security.Cryptography;
using System.Text;

namespace HBLibrary.Common.Security;
public class Cryptographer : ICryptographer {
    private readonly static AesCryptographer aesCryptographer = new AesCryptographer();
    private readonly static RsaCryptographer rsaCryptographer = new RsaCryptographer();

    public byte[] Decrypt(byte[] data, CryptographyInput input) {
        switch (input.Mode) {
            case CryptographyMode.AES:
                if (input.Key.Name != nameof(AesKey))
                    CryptographerException.ThrowIncorrectKey(input.Key.Name);

                return aesCryptographer.Decrypt(data, (AesKey)input.Key);
            case CryptographyMode.RSA:
                if (input.Key.Name != nameof(RsaKey))
                    CryptographerException.ThrowIncorrectKey(input.Key.Name);

                return rsaCryptographer.Decrypt(data, (RsaKey)input.Key);
            default:
                throw new NotSupportedException(input.Mode.ToString());
        }
    }

    public string DecryptString(string data, CryptographyInput input, Encoding encoding) {
        byte[] dataBytes = encoding.GetBytes(data);
        byte[] decrypted = Decrypt(dataBytes, input);
        return encoding.GetString(decrypted);
    }

    public byte[] Encrypt(byte[] data, CryptographyInput input) {
        switch (input.Mode) {
            case CryptographyMode.AES:
                if (input.Key.Name != nameof(AesKey))
                    CryptographerException.ThrowIncorrectKey(input.Key.Name);

                return aesCryptographer.Encrypt(data, (AesKey)input.Key);
            case CryptographyMode.RSA:
                if (input.Key.Name != nameof(RsaKey))
                    CryptographerException.ThrowIncorrectKey(input.Key.Name);

                return rsaCryptographer.Encrypt(data, (RsaKey)input.Key);
            default:
                throw new NotSupportedException(input.Mode.ToString());
        }
    }

    public string EncryptString(string data, CryptographyInput input, Encoding encoding) {
        byte[] dataBytes = encoding.GetBytes(data);
        byte[] encrypted = Encrypt(dataBytes, input);
        return encoding.GetString(encrypted);
    }
}
