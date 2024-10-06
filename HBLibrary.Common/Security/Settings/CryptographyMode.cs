namespace HBLibrary.Common.Security.Settings;
public enum CryptographyMode {
#if WINDOWS
    DPApiUser,
    DPApiMachine,
#endif
    AES,
    RSA,
}
