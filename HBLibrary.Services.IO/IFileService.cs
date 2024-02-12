using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HBLibrary.Services.IO.Operations.File;
using HBLibrary.Services.Security.Cryptography;
using HBLibrary.Services.Security.Cryptography.Settings;

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

    public string Decrypt(FileSnapshot file, ICryptographer cryptographer, CryptographySettings settings, Encoding encoding, FileShare share = FileShare.None);
    public Task<string> DecryptAsync(FileSnapshot file, ICryptographer cryptographer, CryptographySettings settings, Encoding encoding, FileShare share = FileShare.None);
    public byte[] DecryptBytes(FileSnapshot file, ICryptographer cryptographer, CryptographySettings settings, FileShare share = FileShare.None);
    public Task<byte[]> DecryptBytesAsync(FileSnapshot file, ICryptographer cryptographer, CryptographySettings settings, FileShare share = FileShare.None);

    public void Encrypt(FileSnapshot file, string content, ICryptographer cryptographer, CryptographySettings settings, Encoding encoding, FileShare share = FileShare.None);
    public Task EncryptAsync(FileSnapshot file, string content, ICryptographer cryptographer, CryptographySettings settings, Encoding encoding, FileShare share = FileShare.None);
    public void EncryptBytes(FileSnapshot file, byte[] content, ICryptographer cryptographer, CryptographySettings settings, FileShare share = FileShare.None);
    public Task EncryptBytesAsync(FileSnapshot file, byte[] content, ICryptographer cryptographer, CryptographySettings settings, FileShare share = FileShare.None);

    public enum FileContentType {
        Binary,
        RawText,
        RawJson,
        RawXml,
    }
}
