using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving;
public class Archive {
    public string Name { get; }
    public List<FileSnapshot> Files { get; } = [];
    public List<DirectoryContents> Directories { get; } = [];

    public Archive(string name) {
        if (!PathValidator.ValidatePath(name))
            throw new ArgumentException("The given name contains illegal characters", nameof(name));

        Name = name;
    }


    public IEnumerable<string> GetFileNames() => Files.Select(e => e.FullPath);
    public IEnumerable<string> GetDirectoryNames() => Directories.Select(e => e.DirectorySnapshot.FullPath);
    public IEnumerable<string> GetEntries() => GetFileNames().Concat(GetDirectoryNames());

}
