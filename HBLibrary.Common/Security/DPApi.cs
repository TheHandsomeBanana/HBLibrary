using System.Runtime.Versioning;


using System.

Security.Cryptography;


namespace HBLibrary.Common.Security;

#if WINDOWS
#if NET5_0_OR_GREATER
[SupportedOSPlatform("windows")]
#endif
public static class DPApi {
    public static byte[] Protect(byte[] data, string? optionalEntropy = null) {
        byte[]? entropy = optionalEntropy is not null ? GlobalEnvironment.Encoding.GetBytes(optionalEntropy) : null;
        return ProtectedData.Protect(data, entropy, DataProtectionScope.CurrentUser);
    }

    public static byte[] Unprotect(byte[] data, string? optionalEntropy = null) {
        byte[]? entropy = optionalEntropy is not null ? GlobalEnvironment.Encoding.GetBytes(optionalEntropy) : null;
        return ProtectedData.Unprotect(data, entropy, DataProtectionScope.CurrentUser);
    }
}
#endif