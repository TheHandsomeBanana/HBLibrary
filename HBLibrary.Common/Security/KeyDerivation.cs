using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Security;
public static class KeyDerivation {
    public static byte[] DeriveKey(string password, byte[] salt, int iterations, int keySize) {
        using Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA512);
        return rfc.GetBytes(keySize);
    }

    public static byte[] GenerateSalt(int size) {
        byte[] salt = new byte[size];
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();

        rng.GetBytes(salt);
        return salt;
    }
}
