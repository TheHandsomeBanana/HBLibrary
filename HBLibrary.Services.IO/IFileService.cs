using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO;
public interface IFileService {
    FileOperationResult Execute(FileOperation operation);
    Task<FileOperationResult> ExecuteAsync(FileOperation operation);

    string Read(FileSnapshot file, FileShare share = FileShare.None);
    Task<string> ReadAsync(FileSnapshot file, FileShare share = FileShare.None);
    byte[] ReadBytes(FileSnapshot file, FileShare share = FileShare.None);
    Task<byte[]> ReadBytesAsync(FileSnapshot file, FileShare share = FileShare.None);

    void Write(FileSnapshot file, string content, bool append = false, FileShare share = FileShare.None);
    Task WriteAsync(FileSnapshot file, string content, bool append = false, FileShare share = FileShare.None);
    void WriteBytes(FileSnapshot file, byte[] bytes, bool append = false, FileShare share = FileShare.None);
    Task WriteBytesAsync(FileSnapshot file, byte[] bytes, bool append = false, FileShare share = FileShare.None);   
}
