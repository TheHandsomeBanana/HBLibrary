namespace HBLibrary.Services.IO.Archiving;
public class Archive {
    public string Name { get; }
    public List<FileSnapshot> Files { get; } = [];
    public List<DirectorySnapshot> Directories { get; } = [];

    public Archive(string name) {
        if (!PathValidator.ValidatePath(name))
            throw new ArgumentException("The given name contains illegal characters", nameof(name));

        Name = name;
    }


    public IEnumerable<string> GetFileNames() => Files.Select(e => e.FullPath);
    public IEnumerable<string> GetDirectoryNames() => Directories.Select(e => e.FullPath);
    public IEnumerable<string> GetEntries() => GetFileNames().Concat(GetDirectoryNames());

    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <exception cref="ArgumentException"></exception>
    public void AddFile(string fileName) {
        Files.Add(FileSnapshot.Create(fileName));
    }
    public bool TryAddFile(string fileName) {
        bool created = FileSnapshot.TryCreate(fileName, out FileSnapshot? file);
        if (created)
            Files.Add(file!.Value);

        return created;
    }
    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <exception cref="ArgumentException"></exception>
    public void AddDirectory(string directoryName) {
        Directories.Add(DirectorySnapshot.Create(directoryName));

    }
    public bool TryAddDirectory(string directoryName) {
        bool created = DirectorySnapshot.TryCreate(directoryName, out DirectorySnapshot? directory);
        if (created)
            Directories.Add(directory!.Value);

        return created;
    }
}
