using System.Collections.Immutable;

namespace HBLibrary.Services.IO;
public class DirectorySnapshot {
    public string Path { get; init; }
    public string FullPath { get; init; }
    public bool IsNewDirectory { get; init; }

    /// <summary>
    /// Set <paramref name="createNew"/> to <see langword="true"></see> to create a new directory if it does not exist yet.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="createNew"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static DirectorySnapshot Create(string path, bool createNew = false) {
        if (!PathValidator.ValidatePath(path))
            throw new ArgumentException("The given path contains illegal characters", nameof(path));

        bool dirExists = Directory.Exists(path);
        if (!dirExists) {
            if (!createNew)
                throw new DirectoryNotFoundException($"Directory {path} does not exist and {nameof(createNew)} is set to false.");
            else
                Directory.CreateDirectory(path);
        }

        return new DirectorySnapshot {
            Path = path,
            FullPath = new DirectoryInfo(path).FullName,
            IsNewDirectory = !dirExists,
        };
    }

    public static bool TryCreate(string path, out DirectorySnapshot? directory, bool createNew = false) {
        directory = null;

        if (!PathValidator.ValidatePath(path))
            return false;

        if (!File.Exists(path))
            File.Create(path).Dispose();

        directory = new DirectorySnapshot(path);
        return true;
    }

    internal DirectorySnapshot() {
        Path = "";
        FullPath = "";
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

    public DirectorySnapshot Combine(params string[] paths) {
        string[] allPaths = paths.Prepend(FullPath).ToArray();
        string newPath = System.IO.Path.Combine(allPaths);

        return Create(newPath, true);
    }

    public FileSnapshot CombineToFile(params string[] paths) {
        string[] allPaths = paths.Prepend(FullPath).ToArray();
        string newPath = System.IO.Path.Combine(allPaths);

        return FileSnapshot.Create(newPath, true);
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

    public override string ToString() {
        return FullPath;
    }
}
