using HBLibrary.Services.Security.Cryptography.Settings;
using System.Text;

namespace HBLibrary.Services.Security.Cryptography;
public interface ICryptographer {
    byte[] Encrypt(byte[] data, CryptographySettings settings);
    byte[] Decrypt(byte[] data, CryptographySettings settings);

    string EncryptString(string data, CryptographySettings settings, Encoding encoding);
    string DecryptString(string data, CryptographySettings settings, Encoding encoding);
}
