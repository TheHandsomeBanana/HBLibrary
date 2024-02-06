using HBLibrary.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

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
