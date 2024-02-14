using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO;
public readonly struct DirectorySnapshot {
    public string Path { get; init; }
    public string FullPath { get; init; }
    public bool IsNewDirectory { get; init; }

    public static DirectorySnapshot Create(string path) {
        if (!PathValidator.ValidatePath(path))
            throw new ArgumentException("The given path contains illegal characters", nameof(path));

        bool dirExisted = Directory.Exists(path);
        DirectoryInfo info = Directory.CreateDirectory(path);

        return new DirectorySnapshot {
            Path = path,
            FullPath = info.FullName,
            IsNewDirectory = !dirExisted,
        };
    }

    public static bool TryCreate(string path, out DirectorySnapshot? directory) {
        directory = null;

        if (!PathValidator.ValidatePath(path))
            return false;

        if (!File.Exists(path))
            File.Create(path).Dispose();

        directory = new DirectorySnapshot(path);
        return true;
    }

    internal DirectorySnapshot(string path) {
        Path = path;
        FullPath = System.IO.Path.GetFullPath(path);
    }

    public DirectoryInfo GetDirectoryInfo() => new DirectoryInfo(FullPath);

    public static implicit operator ValidPath(DirectorySnapshot directory) => new ValidPath(directory);

    public static explicit operator DirectorySnapshot(ValidPath path) {
        if (!path.IsDirectory)
            throw new InvalidCastException($"Path {path} is not a directory.");

        return new DirectorySnapshot(path.Path);
    }

    public ImmutableArray<FileSnapshot> GetFiles() {
        return Directory.EnumerateFiles(FullPath)
            .Select(e => new FileSnapshot(e))
            .ToImmutableArray();
    }

    public ImmutableArray<DirectorySnapshot> GetSubdirectories() {
        return Directory.EnumerateDirectories(FullPath)
            .Select(e => new DirectorySnapshot(e))
            .ToImmutableArray();
    }
}
