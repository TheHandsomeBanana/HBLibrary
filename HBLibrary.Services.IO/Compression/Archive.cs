using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression;
public class Archive : IArchive {
    public string Name { get; }
    public IEnumerable<string> FileNames { get; }
    public IEnumerable<string> DirectoryNames { get; }

    public ImmutableArray<FileSnapshot> Files { get; }
    public ImmutableArray<DirectoryContents> Directories { get; }


    public Archive(string name, IEnumerable<FileSnapshot> files, IEnumerable<DirectoryContents> directories) {
        Name = name;

        this.Files = files.ToImmutableArray();
        this.Directories = directories.ToImmutableArray();

        FileNames = files.Select(e => e.FullPath);
        DirectoryNames = directories.Select(e => e.DirectorySnapshot.FullPath);
    }

    public Archive(string name, ImmutableArray<FileSnapshot> files, ImmutableArray<DirectoryContents> directories) {
        Name = name;
        
        Files = files;
        Directories = directories;

        FileNames = files.Select(e => e.FullPath);
        DirectoryNames = directories.Select(e => e.DirectorySnapshot.FullPath);
    }
}
