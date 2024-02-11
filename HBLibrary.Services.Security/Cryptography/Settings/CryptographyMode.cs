namespace HBLibrary.Services.Security.Cryptography.Settings; 
public enum CryptographyMode {
#if WINDOWS
    DPApiUser,
    DPApiMachine,
#endif
    AES,
    RSA,
}
