using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO;
public readonly struct DirectorySnapshot {
    public string Path { get; init; }
    public string FullPath { get; init; }
    public ImmutableArray<DirectorySnapshot> Subdirectories { get; } = [];
    public ImmutableArray<FileSnapshot> Files { get; } = [];

    public static DirectorySnapshot Load(string path) {
        if (!PathValidator.ValidatePath(path))
            throw new ArgumentException("The given path contains illegal characters", nameof(path));

        DirectoryInfo info = Directory.CreateDirectory(path);

        return new DirectorySnapshot {
            Path = path,
            FullPath = info.FullName
        };
    }

    /// <summary>
    /// Utilizes <see cref="DirectoryLoader"/> to create a full file entry image.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static DirectorySnapshot LoadFull(string path) {
        if (!PathValidator.ValidatePath(path))
            throw new ArgumentException("The given path contains illegal characters", nameof(path));

        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
            return new DirectorySnapshot(path);
        }

        return DirectoryLoader.LoadImmutableDirectory(path);
    }

    internal DirectorySnapshot(string path) {
        Path = path;
        FullPath = System.IO.Path.GetFullPath(path);
    }

    internal DirectorySnapshot(string path, ImmutableArray<FileSnapshot> files, ImmutableArray<DirectorySnapshot> directories) : this(path) {
        Files = files;
        Subdirectories = directories;
    }

    public DirectoryInfo GetDirectoryInfo() => new DirectoryInfo(FullPath);

    public string CombineToFile(string file) => System.IO.Path.Combine(FullPath, System.IO.Path.GetFileName(file));
}
