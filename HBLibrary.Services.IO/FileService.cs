using HBLibrary.Common.Extensions;
using HBLibrary.Services.IO.Operations;
using HBLibrary.Services.IO.Operations.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO;
public class FileService : IFileService {
    public FileOperationResponse Execute(FileOperationRequest operation) {
        DateTime executionStart = DateTime.Now;


        throw new NotImplementedException();
    }

    public async Task<FileOperationResponse> ExecuteAsync(FileOperationRequest operation, CancellationToken token = default) {
        DateTime executionStart = DateTime.Now;

        if (!operation.CanAsync)
            return new FileOperationErrorResponse($"{operation} can't execute async.") {
                ExecutionStart = executionStart,
                ExecutionEnd = DateTime.Now
            };

        switch (operation) {
            case DecryptFileOperationRequest decryptFileOperation:
                return new DecryptFileOperationResponse();
            case EncryptFileOperationRequest encryptFileOperation:
                return new EncryptFileOperationResponse();
            case CopyFileOperationRequest copyFileOperation:
                return new CopyFileOperationResponse();
            case ReadFileOperationRequest readFileOperation:
                return new ReadFileOperationResponse();
            case WriteFileOperationRequest writeFileOperation:
                return new WriteFileOperationResponse();
            default:
                throw new NotImplementedException();
        }
    }

    public string Read(FileSnapshot file, FileShare share = FileShare.None) {
        using FileStream fs = file.OpenStream(FileMode.Open, FileAccess.Read, share);
        using StreamReader sr = new StreamReader(fs);
        return sr.ReadToEnd();
    }

    public async Task<string> ReadAsync(FileSnapshot file, FileShare share = FileShare.None) {
        using FileStream fs = file.OpenStream(FileMode.Open, FileAccess.Read, share, true);
        using StreamReader sr = new StreamReader(fs);
        return await sr.ReadToEndAsync();
    }

    public byte[] ReadBytes(FileSnapshot file, FileShare share = FileShare.None) {
        using FileStream fs = file.OpenStream(FileMode.Open, FileAccess.Read, share);
        return fs.Read();
    }

    public async Task<byte[]> ReadBytesAsync(FileSnapshot file, FileShare share = FileShare.None) {
        using FileStream fs = file.OpenStream(FileMode.Open, FileAccess.Read, share, true);
        return await fs.ReadAsync();
    }

    public void Write(FileSnapshot file, string content, bool append = false, FileShare share = FileShare.None) {
        FileMode mode = append ? FileMode.Append : FileMode.Open;
        using FileStream fs = file.OpenStream(mode, FileAccess.Write, share);
        using StreamWriter sw = new StreamWriter(fs);
        sw.Write(content);
    }

    public async Task WriteAsync(FileSnapshot file, string content, bool append = false, FileShare share = FileShare.None) {
        FileMode mode = append ? FileMode.Append : FileMode.Open;
        using FileStream fs = file.OpenStream(mode, FileAccess.Write, share, true);
        using StreamWriter sw = new StreamWriter(fs);
        await sw.WriteAsync(content);
    }

    public void WriteBytes(FileSnapshot file, byte[] bytes, bool append = false, FileShare share = FileShare.None) {
        FileMode mode = append ? FileMode.Append : FileMode.Open;
        using FileStream fs = file.OpenStream(mode, FileAccess.Write, share);
        fs.Write(bytes);
    }

    public async Task WriteBytesAsync(FileSnapshot file, byte[] bytes, bool append = false, FileShare share = FileShare.None) {
        FileMode mode = append ? FileMode.Append : FileMode.Open;
        using FileStream fs = file.OpenStream(mode, FileAccess.Write, share, true);
        await fs.WriteAsync(bytes);
    }
}
