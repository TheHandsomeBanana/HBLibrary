
using HBLibrary.Common.Security;
using HBLibrary.Common.Security.Aes;
using HBLibrary.Common.Security.Exceptions;
using HBLibrary.Common.Security.Keys;
using HBLibrary.Common.Security.Rsa;
using HBLibrary.Common.Security.Settings;
using System.Security.Cryptography;
using System.Text;

namespace HBLibrary.Common.Security;
public class Cryptographer : ICryptographer {
    private readonly static AesCryptographer aesCryptographer = new AesCryptographer();
    private readonly static RsaCryptographer rsaCryptographer = new RsaCryptographer();


    public byte[] Decrypt(byte[] data, CryptographySettings settings) {
        switch (settings.Mode) {
#if WINDOWS
            case CryptographyMode.DPApiUser:
                if (settings.Key.Name != nameof(DPEntropy))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

                return ProtectedData.Unprotect(data, settings.Key.Key, DataProtectionScope.CurrentUser);

            case CryptographyMode.DPApiMachine:
                if (settings.Key.Name != nameof(DPEntropy))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

                return ProtectedData.Unprotect(data, settings.Key.Key, DataProtectionScope.LocalMachine);
#endif
            case CryptographyMode.AES:
                if (settings.Key.Name != nameof(AesKey))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

                return aesCryptographer.Decrypt(data, (AesKey)settings.Key);
            case CryptographyMode.RSA:
                if (settings.Key.Name != nameof(RsaKey))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

                return rsaCryptographer.Decrypt(data, (RsaKey)settings.Key);
            default:
                throw new NotSupportedException(settings.Mode.ToString());
        }
    }

    public string DecryptString(string data, CryptographySettings settings, Encoding encoding) {
        byte[] dataBytes = encoding.GetBytes(data);
        byte[] decrypted = Decrypt(dataBytes, settings);
        return encoding.GetString(decrypted);
    }

    public byte[] Encrypt(byte[] data, CryptographySettings settings) {
        switch (settings.Mode) {
#if WINDOWS
            case CryptographyMode.DPApiUser:
                if (settings.Key.Name != nameof(DPEntropy))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

                return ProtectedData.Protect(data, settings.Key.Key, DataProtectionScope.CurrentUser);

            case CryptographyMode.DPApiMachine:
                if (settings.Key.Name != nameof(DPEntropy))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

                return ProtectedData.Protect(data, settings.Key.Key, DataProtectionScope.LocalMachine);
#endif
            case CryptographyMode.AES:
                if (settings.Key.Name != nameof(AesKey))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

                return aesCryptographer.Encrypt(data, (AesKey)settings.Key);
            case CryptographyMode.RSA:
                if (settings.Key.Name != nameof(RsaKey))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

                return rsaCryptographer.Encrypt(data, (RsaKey)settings.Key);
            default:
                throw new NotSupportedException(settings.Mode.ToString());
        }
    }

    public string EncryptString(string data, CryptographySettings settings, Encoding encoding) {
        byte[] dataBytes = encoding.GetBytes(data);
        byte[] encrypted = Encrypt(dataBytes, settings);
        return encoding.GetString(encrypted);
    }
}
