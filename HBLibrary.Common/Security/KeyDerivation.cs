using System.Security;
using System.Security.Cryptography;

namespace HBLibrary.Common.Security;
public static class KeyDerivation {
    public static byte[] DeriveKey(string password, byte[] salt, int iterations, int keySize) {
        using Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA512);
        return rfc.GetBytes(keySize);
    }

    public static byte[] DeriveKey(SecureString password, byte[] salt, int iterations, int keySize) {
        using Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(SStringConverter.SecureStringToString(password)!, salt, iterations, HashAlgorithmName.SHA512);
        return rfc.GetBytes(keySize);
    }

    public static byte[] GenerateSalt(int size) {
        byte[] salt = new byte[size];
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();

        rng.GetBytes(salt);
        return salt;
    }
}
