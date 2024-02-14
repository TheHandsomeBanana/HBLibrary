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
    public bool ExistedBeforehand { get; init; }

    public static DirectorySnapshot Create(string path) {
        if (!PathValidator.ValidatePath(path))
            throw new ArgumentException("The given path contains illegal characters", nameof(path));

        bool dirExisted = Directory.Exists(path);
        DirectoryInfo info = Directory.CreateDirectory(path);

        return new DirectorySnapshot {
            Path = path,
            FullPath = info.FullName,
            ExistedBeforehand = dirExisted,
        };
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
}
