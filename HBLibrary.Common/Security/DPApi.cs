using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Versioning;

namespace HBLibrary.Common.Security;

#if WINDOWS
#if NET5_0_OR_GREATER
[SupportedOSPlatform("windows")]
#endif
public static class DPApi {
    public static byte[] Protect(byte[] data, string? optionalEntropy = null) {
        byte[]? entropy = optionalEntropy is not null ? Encoding.UTF8.GetBytes(optionalEntropy) : null;
        return ProtectedData.Protect(data, entropy, DataProtectionScope.LocalMachine);
    }

    public static byte[] Unprotect(byte[] data, string? optionalEntropy = null) {
        byte[]? entropy = optionalEntropy is not null ? Encoding.UTF8.GetBytes(optionalEntropy) : null;
        return ProtectedData.Unprotect(data, entropy, DataProtectionScope.LocalMachine);
    }
}
#endif