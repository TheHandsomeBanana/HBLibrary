using HBLibrary.Services.Security.Cryptography.Aes;
using HBLibrary.Services.Security.Cryptography.Keys;
using HBLibrary.Services.Security.Cryptography.Rsa;
using HBLibrary.Services.Security.Cryptography.Settings;
using HBLibrary.Services.Security.DataProtection;
using HBLibrary.Services.Security.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.Security.Cryptography;
public class Cryptographer : ICryptographer {
    public byte[] Decrypt(byte[] data, CryptographySettings settings) {
        switch (settings.Mode) {
#if WINDOWS
            case CryptographyMode.DPApiUser:
                if (settings.Key.Name != nameof(DPKey))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

                DataProtectionService dpService = new DataProtectionService();
#pragma warning disable CA1416 // Validate platform compatibility
                dpService.SetScope(System.Security.Cryptography.DataProtectionScope.CurrentUser);
                dpService.SetEntropy(settings.Key.Key);
                return dpService.Unprotect(data);

            case CryptographyMode.DPApiMachine:
                if (settings.Key.Name != nameof(DPKey))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

                dpService = new DataProtectionService();
                dpService.SetScope(System.Security.Cryptography.DataProtectionScope.LocalMachine);
#pragma warning restore CA1416 // Validate platform compatibility
                dpService.SetEntropy(settings.Key.Key);
                return dpService.Unprotect(data);
#endif
            case CryptographyMode.AES:
                if(settings.Key.Name != nameof(AesKey))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

                return new AesCryptographer().Decrypt(data, (AesKey)settings.Key);
            case CryptographyMode.RSA:
                if (settings.Key.Name != nameof(RsaKey))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

                return new RsaCryptographer().Decrypt(data, (RsaKey)settings.Key);
            default:
                throw new NotSupportedException(settings.Mode.ToString());
        }
    }

    public byte[] Encrypt(byte[] data, CryptographySettings settings) {
        switch (settings.Mode) {
#if WINDOWS
            case CryptographyMode.DPApiUser:
                if (settings.Key.Name != nameof(DPKey))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

                DataProtectionService dpService = new DataProtectionService();
#pragma warning disable CA1416 // Validate platform compatibility
                dpService.SetScope(System.Security.Cryptography.DataProtectionScope.CurrentUser);
                dpService.SetEntropy(settings.Key.Key);
                return dpService.Protect(data);

            case CryptographyMode.DPApiMachine:
                if (settings.Key.Name != nameof(DPKey))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

                dpService = new DataProtectionService();
                dpService.SetScope(System.Security.Cryptography.DataProtectionScope.LocalMachine);
#pragma warning restore CA1416 // Validate platform compatibility
                dpService.SetEntropy(settings.Key.Key);
                return dpService.Protect(data);
#endif
            case CryptographyMode.AES:
                if (settings.Key.Name != nameof(AesKey))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

                return new AesCryptographer().Encrypt(data, (AesKey)settings.Key);
            case CryptographyMode.RSA:
                if (settings.Key.Name != nameof(RsaKey))
                    CryptographerException.ThrowIncorrectKey(settings.Key.Name);

                return new RsaCryptographer().Encrypt(data, (RsaKey)settings.Key);
            default:
                throw new NotSupportedException(settings.Mode.ToString());
        }
    }
}
