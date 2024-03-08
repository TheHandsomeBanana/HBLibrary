using HBLibrary.Services.IO.Operations.File;

namespace HBLibrary.Services.IO;
public interface IFileService {
    FileOperationResponse Execute(FileOperationRequest operation);
    Task<FileOperationResponse> ExecuteAsync(FileOperationRequest operation, CancellationToken token = default);

    string Read(FileSnapshot file, FileShare share = FileShare.None);
    Task<string> ReadAsync(FileSnapshot file, FileShare share = FileShare.None);
    byte[] ReadBytes(FileSnapshot file, FileShare share = FileShare.None);
    Task<byte[]> ReadBytesAsync(FileSnapshot file, FileShare share = FileShare.None);

    void Write(FileSnapshot file, string content, bool append = false, FileShare share = FileShare.None);
    Task WriteAsync(FileSnapshot file, string content, bool append = false, FileShare share = FileShare.None);
    void WriteBytes(FileSnapshot file, byte[] bytes, bool append = false, FileShare share = FileShare.None);
    Task WriteBytesAsync(FileSnapshot file, byte[] bytes, bool append = false, FileShare share = FileShare.None);

    // Not quite sure yet if this should be exposed by the interface

    //string Decrypt(FileSnapshot file, ICryptographer cryptographer, CryptographySettings settings, Encoding encoding, FileShare share = FileShare.None);
    //Task<string> DecryptAsync(FileSnapshot file, ICryptographer cryptographer, CryptographySettings settings, Encoding encoding, FileShare share = FileShare.None);
    //byte[] DecryptBytes(FileSnapshot file, ICryptographer cryptographer, CryptographySettings settings, FileShare share = FileShare.None);
    //Task<byte[]> DecryptBytesAsync(FileSnapshot file, ICryptographer cryptographer, CryptographySettings settings, FileShare share = FileShare.None);

    //void Encrypt(FileSnapshot file, string content, ICryptographer cryptographer, CryptographySettings settings, Encoding encoding, FileShare share = FileShare.None);
    //Task EncryptAsync(FileSnapshot file, string content, ICryptographer cryptographer, CryptographySettings settings, Encoding encoding, FileShare share = FileShare.None);
    //void EncryptBytes(FileSnapshot file, byte[] content, ICryptographer cryptographer, CryptographySettings settings, FileShare share = FileShare.None);
    //Task EncryptBytesAsync(FileSnapshot file, byte[] content, ICryptographer cryptographer, CryptographySettings settings, FileShare share = FileShare.None);

    public enum FileContentType {
        Binary,
        RawText,
        RawJson,
        RawXml,
    }
}
