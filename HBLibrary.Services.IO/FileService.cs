using HBLibrary.Common.Extensions;
using HBLibrary.Services.Security.Cryptography;
using HBLibrary.Services.Security.Cryptography.Settings;
using System.Text;

namespace HBLibrary.Services.IO;
public class FileService : IFileService {
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
        FileMode mode = append ? FileMode.Append : FileMode.Create;
        using FileStream fs = file.OpenStream(mode, FileAccess.Write, share);
        using StreamWriter sw = new StreamWriter(fs);
        sw.Write(content);
    }

    public async Task WriteAsync(FileSnapshot file, string content, bool append = false, FileShare share = FileShare.None) {
        FileMode mode = append ? FileMode.Append : FileMode.Create;
        using FileStream fs = file.OpenStream(mode, FileAccess.Write, share, true);
        using StreamWriter sw = new StreamWriter(fs);
        await sw.WriteAsync(content);
    }

    public void WriteBytes(FileSnapshot file, byte[] content, bool append = false, FileShare share = FileShare.None) {
        FileMode mode = append ? FileMode.Append : FileMode.Create;
        using FileStream fs = file.OpenStream(mode, FileAccess.Write, share);
        fs.Write(content);
    }

    public async Task WriteBytesAsync(FileSnapshot file, byte[] content, bool append = false, FileShare share = FileShare.None) {
        FileMode mode = append ? FileMode.Append : FileMode.Create;
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
