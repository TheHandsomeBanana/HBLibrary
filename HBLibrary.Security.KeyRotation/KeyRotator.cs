using HBLibrary.Interface.Security.KeyRotation;
using System.Security;

namespace HBLibrary.Security.KeyRotation;

public class KeyRotator : IKeyRotator {
    public async Task RotateKeysAsync(MasterKey masterKey, string keyFile) {
        KeyStorage storage = new KeyStorage(keyFile, masterKey);


    }

}
