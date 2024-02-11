using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO;
public readonly struct FileSnapshot {
    public string Path { get; init; }
    public string FullPath { get; init; }
    public long Length { get; init; }
    public int OptimalBufferSize { get; init; }

    /// <summary>
    /// Creates a snapshot of the provided file or a new file.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static FileSnapshot Create(string path) {
        if (!PathValidator.ValidatePath(path))
            throw new ArgumentException("The given path contains illegal characters", nameof(path));

        if (!File.Exists(path))
            File.Create(path).Dispose();

        return new FileSnapshot(path);
    }

    public static bool TryCreate(string path, out FileSnapshot? file) {
        file = null;

        if (!PathValidator.ValidatePath(path))
            return false;

        if (!File.Exists(path))
            File.Create(path).Dispose();

        file = new FileSnapshot(path);
        return true;
    }

    /// <summary>
    /// Used for directory loader
    /// </summary>
    /// <param name="path"></param>
    internal FileSnapshot(string path) {
        Path = path;
        FileInfo temp = GetInfo();
        FullPath = temp.FullName;
        Length = temp.Length;
        OptimalBufferSize = GetOptimalBufferSize(Length);
    }

    internal FileSnapshot(FileInfo info) {
        Path = info.Name;
        FullPath = info.FullName;
        Length = info.Length;
        OptimalBufferSize = GetOptimalBufferSize(info.Length);
    }

    public FileInfo GetInfo() => new FileInfo(Path);

    // https://github.com/dotnet/runtime/discussions/74405
    public static int GetOptimalBufferSize(long fileSize) {
        long fileSizeBytes = fileSize * 1024;

        if (fileSizeBytes <= 128 * 1024) // Up to 128 KB
            return 2048; // 2 KB
        else if (fileSizeBytes <= 512 * 1024) // Up to 512 KB
            return 65536; // 64 KB
        else if (fileSizeBytes <= 1 * 1024 * 1024) // Up to 1 MB
            return 81920; // ~80 KB
        else if (fileSizeBytes <= 10 * 1024 * 1024) // Up to 10 MB
            return 131072; // 128 KB
        else if (fileSizeBytes <= 32 * 1024 * 1024) // Up to 32 MB
            return 262144; // 256 KB
        else if (fileSizeBytes <= 100 * 1024 * 1024) // Up to 100 MB
            return 524288; // 512 KB
        else // Larger than 100 MB
            return 1048576; // 1 MB
    }

    public static int GetOptimalBufferSize(string file) {
        return GetOptimalBufferSize(new FileInfo(file).Length);
    }

    public FileStream OpenStream(FileMode mode, FileAccess access, FileShare share, bool useAsync = false) {
        return new FileStream(FullPath, mode, access, share, OptimalBufferSize, useAsync);
    }

    public static implicit operator ValidPath(FileSnapshot file) => new ValidPath(file);

    public static explicit operator FileSnapshot(ValidPath path) {
        if (!path.IsFile)
            throw new InvalidCastException($"Path {path} is not a file.");

        return new FileSnapshot(path.Path);
    }
}
