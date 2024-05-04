using HBLibrary.Common.Extensions;
using HBLibrary.Services.IO.Obsolete.Operations.File;
using HBLibrary.Services.Security.Cryptography;
using HBLibrary.Services.Security.Cryptography.Settings;
using System.Text;

namespace HBLibrary.Services.IO;
public class FileService : IFileService {
    public FileOperationResponse Execute(FileOperationRequest operation) {
        DateTime executionStart = DateTime.Now;

        if (!operation.CanAsync)
            return new FileOperationResponse() {
                ErrorMessage = $"{operation} can't execute async.",
                ExecutionStart = executionStart,
                ExecutionEnd = DateTime.Now,
            };

        FileOperationResponse fileOperationResponse;
        bool success = true;
        Exception? e = null;

        switch (operation) {
            case DecryptFileRequest decryptRequest:
                byte[] content = [];

                try {
                    content = DecryptBytes(decryptRequest.File, decryptRequest.Cryptographer, decryptRequest.Settings, decryptRequest.Share);
                }
                catch (Exception ex) {
                    e = ex;
                    success = false;
                }

                fileOperationResponse = new DecryptFileResponse() {
                    Result = content,
                    ResultString = decryptRequest.Encoding.GetString(content),
                    File = decryptRequest.File
                };
                break;
            case EncryptFileRequest encryptRequest:
                try {
                    if (encryptRequest.StringContent is not null)
                        Encrypt(encryptRequest.File, encryptRequest.StringContent, encryptRequest.Cryptographer, encryptRequest.Settings, encryptRequest.Encoding, encryptRequest.Share);
                    else
                        EncryptBytes(encryptRequest.File, encryptRequest.Content, encryptRequest.Cryptographer, encryptRequest.Settings, encryptRequest.Share);
                }
                catch (Exception ex) {
                    e = ex;
                    success = false;
                }

                fileOperationResponse = new EncryptFileResponse {
                    File = encryptRequest.File,
                };
                break;
            case CopyFileRequest copyRequest:
                try {
                    copyRequest.FileEntryService.CopyFile(copyRequest.File.FullPath, copyRequest.TargetFile, copyRequest.ConflictAction);
                }
                catch (Exception ex) {
                    e = ex;
                    success = false;
                }

                fileOperationResponse = new CopyFileResponse {
                    File = copyRequest.File,
                };

                break;
            case ReadFileRequest readRequest:
                content = [];

                try {
                    content = ReadBytes(readRequest.File, readRequest.Share);
                }
                catch (Exception ex) {
                    e = ex;
                    success = false;
                }

                fileOperationResponse = new ReadFileResponse {
                    Result = content,
                    ResultString = readRequest.Encoding.GetString(content),
                    File = readRequest.File
                };
                break;
            case WriteFileRequest writeRequest:
                try {
                    if (writeRequest.StringContent is not null)
                        Write(writeRequest.File, writeRequest.StringContent, writeRequest.Append, writeRequest.Share);
                    else
                        WriteBytes(writeRequest.File, writeRequest.Content, writeRequest.Append, writeRequest.Share);
                }
                catch (Exception ex) {
                    e = ex;
                    success = false;
                }

                fileOperationResponse = new WriteFileResponse {
                    File = writeRequest.File,
                };

                break;
            default:
                throw new NotSupportedException(operation.GetType().Name);
        }

        fileOperationResponse.ExecutionStart = executionStart;
        fileOperationResponse.Success = success;
        fileOperationResponse.Exception = e;
        fileOperationResponse.ErrorMessage = e?.Message;
        fileOperationResponse.ExecutionEnd = DateTime.Now;
        return fileOperationResponse;
    }

    public async Task<FileOperationResponse> ExecuteAsync(FileOperationRequest operation, CancellationToken token = default) {
        DateTime executionStart = DateTime.Now;

        if (!operation.CanAsync)
            return new FileOperationResponse {
                ErrorMessage = $"{operation} can't execute async.",
                ExecutionStart = executionStart,
                ExecutionEnd = DateTime.Now,
            };

        FileOperationResponse fileOperationResponse;
        bool success = true;
        Exception? e = null;

        switch (operation) {
            case DecryptFileRequest decryptRequest:
                byte[] content = [];

                try {
                    content = await DecryptBytesAsync(decryptRequest.File, decryptRequest.Cryptographer, decryptRequest.Settings, decryptRequest.Share);
                }
                catch (Exception ex) {
                    e = ex;
                    success = false;
                }

                fileOperationResponse = new DecryptFileResponse() {
                    Result = content,
                    ResultString = decryptRequest.Encoding.GetString(content),
                    File = decryptRequest.File
                };
                break;
            case EncryptFileRequest encryptRequest:
                try {
                    if (encryptRequest.StringContent is not null)
                        await EncryptAsync(encryptRequest.File, encryptRequest.StringContent, encryptRequest.Cryptographer, encryptRequest.Settings, encryptRequest.Encoding, encryptRequest.Share);
                    else
                        await EncryptBytesAsync(encryptRequest.File, encryptRequest.Content, encryptRequest.Cryptographer, encryptRequest.Settings, encryptRequest.Share);
                }
                catch (Exception ex) {
                    e = ex;
                    success = false;
                }

                fileOperationResponse = new EncryptFileResponse {
                    File = encryptRequest.File,
                };
                break;
            case CopyFileRequest copyRequest:
                try {
                    await copyRequest.FileEntryService.CopyFileAsync(copyRequest.File.FullPath, copyRequest.TargetFile, copyRequest.ConflictAction);
                }
                catch (Exception ex) {
                    e = ex;
                    success = false;
                }

                fileOperationResponse = new CopyFileResponse {
                    File = copyRequest.File,
                };

                break;
            case ReadFileRequest readRequest:
                content = [];

                try {
                    content = await ReadBytesAsync(readRequest.File, readRequest.Share);
                }
                catch (Exception ex) {
                    e = ex;
                    success = false;
                }

                fileOperationResponse = new ReadFileResponse {
                    Result = content,
                    ResultString = readRequest.Encoding.GetString(content),
                    File = readRequest.File
                };
                break;
            case WriteFileRequest writeRequest:
                try {
                    if (writeRequest.StringContent is not null)
                        await WriteAsync(writeRequest.File, writeRequest.StringContent, writeRequest.Append, writeRequest.Share);
                    else
                        await WriteBytesAsync(writeRequest.File, writeRequest.Content, writeRequest.Append, writeRequest.Share);
                }
                catch (Exception ex) {
                    e = ex;
                    success = false;
                }

                fileOperationResponse = new WriteFileResponse {
                    File = writeRequest.File,
                };

                break;
            default:
                throw new NotSupportedException(operation.GetType().Name);
        }

        fileOperationResponse.ExecutionStart = executionStart;
        fileOperationResponse.Success = success;
        fileOperationResponse.Exception = e;
        fileOperationResponse.ErrorMessage = e?.Message;
        fileOperationResponse.ExecutionEnd = DateTime.Now;
        return fileOperationResponse;
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

    public void WriteBytes(FileSnapshot file, byte[] content, bool append = false, FileShare share = FileShare.None) {
        FileMode mode = append ? FileMode.Append : FileMode.Open;
        using FileStream fs = file.OpenStream(mode, FileAccess.Write, share);
        fs.Write(content);
    }

    public async Task WriteBytesAsync(FileSnapshot file, byte[] content, bool append = false, FileShare share = FileShare.None) {
        FileMode mode = append ? FileMode.Append : FileMode.Open;
        using FileStream fs = file.OpenStream(mode, FileAccess.Write, share, true);
        await fs.WriteAsync(content);
    }

    public string Decrypt(FileSnapshot file, ICryptographer cryptographer, CryptographySettings settings, Encoding encoding, FileShare share = FileShare.None) {
        string encrypted = Read(file, share);
        return cryptographer.DecryptString(encrypted, settings, encoding);
    }

    public async Task<string> DecryptAsync(FileSnapshot file, ICryptographer cryptographer, CryptographySettings settings, Encoding encoding, FileShare share = FileShare.None) {
        string encrypted = await ReadAsync(file, share);
        return cryptographer.DecryptString(encrypted, settings, encoding);
    }

    public byte[] DecryptBytes(FileSnapshot file, ICryptographer cryptographer, CryptographySettings settings, FileShare share = FileShare.None) {
        byte[] bytes = ReadBytes(file, share);
        return cryptographer.Decrypt(bytes, settings);
    }

    public async Task<byte[]> DecryptBytesAsync(FileSnapshot file, ICryptographer cryptographer, CryptographySettings settings, FileShare share = FileShare.None) {
        byte[] bytes = await ReadBytesAsync(file, share);
        return cryptographer.Decrypt(bytes, settings);
    }

    public void Encrypt(FileSnapshot file, string content, ICryptographer cryptographer, CryptographySettings settings, Encoding encoding, FileShare share = FileShare.None) {
        string encrypted = cryptographer.EncryptString(content, settings, encoding);
        Write(file, encrypted, false, share);
    }

    public Task EncryptAsync(FileSnapshot file, string content, ICryptographer cryptographer, CryptographySettings settings, Encoding encoding, FileShare share = FileShare.None) {
        string encrypted = cryptographer.EncryptString(content, settings, encoding);
        return WriteAsync(file, encrypted, false, share);
    }

    public void EncryptBytes(FileSnapshot file, byte[] content, ICryptographer cryptographer, CryptographySettings settings, FileShare share = FileShare.None) {
        byte[] encrypted = cryptographer.Encrypt(content, settings);
        WriteBytes(file, encrypted, false, share);
    }

    public Task EncryptBytesAsync(FileSnapshot file, byte[] content, ICryptographer cryptographer, CryptographySettings settings, FileShare share = FileShare.None) {
        byte[] encrypted = cryptographer.Encrypt(content, settings);
        return WriteBytesAsync(file, encrypted, false, share);
    }
}
