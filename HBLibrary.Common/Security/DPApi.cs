using System.Runtime.Versioning;

/* Unmerged change from project 'HBLibrary.Common (net8.0)'
Before:
using System.
/* Unmerged change from project 'HBLibrary.Common (net8.0)'
After:
using System.Security.Cryptography;
using System.
/* Unmerged change from project 'HBLibrary.Common (net8.0)'
*/
using System.
/* Unmerged change from project 'HBLibrary.Common (net8.0)'
Before:
using System.Security.Cryptography;
/* Unmerged change from project 'HBLibrary.Common (net8.0)'
After:
/* Unmerged change from project 'HBLibrary.Common (net8.0)'
*/
Security.Cryptography;
/* Unmerged change from project 'HBLibrary.Common (net8.0)'
Before:
using System.Threading.Tasks;
using System.Runtime.Versioning;
After:
using System.Text;
using System.Threading.Tasks;
*/


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