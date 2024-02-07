namespace HBLibrary.Services.Security.Cryptography.Settings; 
public enum CryptographyMode {
#if WINDOWS
    WindowsDataProtectionAPI,
#endif
    AES,
    RSA,
}
