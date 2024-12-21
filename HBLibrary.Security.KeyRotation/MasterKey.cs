using HBLibrary.Core;
using HBLibrary.Interface.Security.KeyRotation;
using HBLibrary.Interface.Security.Keys;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Security.KeyRotation;
public sealed class MasterKey : IMasterKey {
    public SecureString Salt { get; private set; }
    public SecureString Value { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private MasterKey() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public static MasterKey Create(string key, byte[] salt) {
        SecureString secureSalt = GlobalEnvironment.Encoding.GetString(salt).StringToSecureString();
        SecureString secureKey = KeyDerivation.DeriveNewSecureString(key, salt);
        return new MasterKey { Value = secureKey, Salt = secureSalt };
    }

    public string ReadValue() {
        return Value.SecureStringToString()!;
    }

    public AesKey ConvertToAesKey() {
        return KeyGenerator.GenerateAesKey(Value, GlobalEnvironment.Encoding.GetBytes(Salt.SecureStringToString()!));
    }

    public void Dispose() {
        Salt.Dispose();
        Value.Dispose();
    }
}
