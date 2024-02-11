using System.Security.Cryptography;

#if WINDOWS
#pragma warning disable CA1416 // Validate platform compatibility

namespace HBLibrary.Services.Security.DataProtection;

public class DataProtectionService : IDataProtectionService {
    private byte[] entropy = { 9, 8, 7, 6, 5 };
    private DataProtectionScope scope = DataProtectionScope.CurrentUser;
    public void SetEntropy(byte[] entropy) {
        this.entropy = entropy;
    }

    public void SetScope(DataProtectionScope scope) {
        this.scope = scope;
    }

    public byte[] Protect(byte[] data) {
        return ProtectedData.Protect(data, entropy, scope);
    }

    public byte[] Unprotect(byte[] data) {
        return ProtectedData.Unprotect(data, entropy, scope);
    }
}
#pragma warning restore CA1416 // Validate platform compatibility

#endif
