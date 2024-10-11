using System.Text;

namespace HBLibrary.Common.Security;
public interface ICryptographer {
    byte[] Encrypt(byte[] data, CryptographyInput input);
    byte[] Decrypt(byte[] data, CryptographyInput input);

    string EncryptString(string data, CryptographyInput input, Encoding encoding);
    string DecryptString(string data, CryptographyInput input, Encoding encoding);
}
