using System.Security.Cryptography;

#if WINDOWS
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

#endif
