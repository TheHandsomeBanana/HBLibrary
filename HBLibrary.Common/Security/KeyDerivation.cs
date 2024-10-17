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

    public static SecureString DeriveNewSecureString(SecureString password, byte[] salt) {
        string? plainPassword = SStringConverter.SecureStringToString(password)!;

        using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(plainPassword, salt, 10000, HashAlgorithmName.SHA256)) {
            byte[] keyBytes = rfc2898DeriveBytes.GetBytes(32);

            SecureString derivedSecureString = new SecureString();
            foreach (char c in Convert.ToBase64String(keyBytes)) {
                derivedSecureString.AppendChar(c);
            }

            derivedSecureString.MakeReadOnly();

            plainPassword = null;

            return derivedSecureString;
        }
    }
    
    public static SecureString DeriveNewSecureString(string plainPassword, byte[] salt) {
        using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(plainPassword, salt, 10000, HashAlgorithmName.SHA256)) {
            byte[] keyBytes = rfc2898DeriveBytes.GetBytes(32);

            SecureString derivedSecureString = new SecureString();
            foreach (char c in Convert.ToBase64String(keyBytes)) {
                derivedSecureString.AppendChar(c);
            }

            derivedSecureString.MakeReadOnly();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            plainPassword = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            return derivedSecureString;
        }
    }
}
