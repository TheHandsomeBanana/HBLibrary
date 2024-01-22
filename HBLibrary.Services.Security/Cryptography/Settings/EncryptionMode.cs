namespace HBLibrary.Services.Security.Cryptography.Settings; 
public enum EncryptionMode {
#if WINDOWS
    WindowsDataProtectionAPI,
#endif
    AES,
    RSA,
}
