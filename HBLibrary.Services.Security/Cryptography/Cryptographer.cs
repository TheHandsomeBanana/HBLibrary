using HBLibrary.Services.Security.Cryptography.Aes;
using HBLibrary.Services.Security.Cryptography.Keys;
using HBLibrary.Services.Security.Cryptography.Rsa;
using HBLibrary.Services.Security.Cryptography.Settings;
using HBLibrary.Services.Security.DataProtection;
using HBLibrary.Services.Security.Exceptions;
using System.Text;

namespace HBLibrary.Services.Security.Cryptography;
public class Cryptographer : ICryptographer {
    private readonly static DataProtectionService dpService = new DataProtectionService();
    private readonly static AesCryptographer aesCryptographer = new AesCryptographer();
    private readonly static RsaCryptographer rsaCryptographer = new RsaCryptographer();


    public byte[] Decrypt(byte[] data, CryptographySettings settings) {
        switch (settings.Mode) {
#if WINDOWS
            case CryptographyMode.DPApiUser:
                if (settings.Key.Name != nameof(DPEntropy))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

#pragma warning disable CA1416 // Validate platform compatibility
                dpService.SetScope(System.Security.Cryptography.DataProtectionScope.CurrentUser);
                dpService.SetEntropy(settings.Key.Key);
                return dpService.Unprotect(data);

            case CryptographyMode.DPApiMachine:
                if (settings.Key.Name != nameof(DPEntropy))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

                dpService.SetScope(System.Security.Cryptography.DataProtectionScope.LocalMachine);
#pragma warning restore CA1416 // Validate platform compatibility
                dpService.SetEntropy(settings.Key.Key);
                return dpService.Unprotect(data);
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

#pragma warning disable CA1416 // Validate platform compatibility
                dpService.SetScope(System.Security.Cryptography.DataProtectionScope.CurrentUser);
                dpService.SetEntropy(settings.Key.Key);
                return dpService.Protect(data);

            case CryptographyMode.DPApiMachine:
                if (settings.Key.Name != nameof(DPEntropy))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

                dpService.SetScope(System.Security.Cryptography.DataProtectionScope.LocalMachine);
#pragma warning restore CA1416 // Validate platform compatibility
                dpService.SetEntropy(settings.Key.Key);
                return dpService.Protect(data);
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
