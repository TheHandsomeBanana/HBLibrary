using HBLibrary.Interface.Security;
using System.Text;

namespace HBLibrary.Interface.IO;
public interface IFileService {
    string Read(FileSnapshot file, FileShare share = FileShare.None);
    Task<string> ReadAsync(FileSnapshot file, FileShare share = FileShare.None);
    byte[] ReadBytes(FileSnapshot file, FileShare share = FileShare.None);
    Task<byte[]> ReadBytesAsync(FileSnapshot file, FileShare share = FileShare.None);

    void Write(FileSnapshot file, string content, bool append = false, FileShare share = FileShare.None);
    Task WriteAsync(FileSnapshot file, string content, bool append = false, FileShare share = FileShare.None);
    void WriteBytes(FileSnapshot file, byte[] bytes, bool append = false, FileShare share = FileShare.None);
    Task WriteBytesAsync(FileSnapshot file, byte[] bytes, bool append = false, FileShare share = FileShare.None);


    string Decrypt(FileSnapshot file, ICryptographer cryptographer, CryptographyInput input, Encoding encoding, FileShare share = FileShare.None);
    byte[] DecryptBytes(FileSnapshot file, ICryptographer cryptographer, CryptographyInput input, FileShare share = FileShare.None);

    void Encrypt(FileSnapshot file, string content, ICryptographer cryptographer, CryptographyInput input, Encoding encoding, FileShare share = FileShare.None);
    void EncryptBytes(FileSnapshot file, byte[] content, ICryptographer cryptographer, CryptographyInput settings, FileShare share = FileShare.None);

    public enum FileContentType {
        Binary,
        RawText,
        RawJson,
        RawXml,
    }
}
