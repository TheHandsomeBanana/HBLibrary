using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression;
internal class ArchiveBuilder : IArchiveBuilder {
    private string? name = null;
    private readonly List<FileSnapshot> files = [];
    private readonly List<DirectoryContents> directories = [];

    public IArchiveBuilder AddFile(FileSnapshot file) {
        this.files.Add(file); 
        return this;
    }

    public IArchiveBuilder AddFiles(IEnumerable<FileSnapshot> files) {
        this.files.AddRange(files);
        return this;
    }

    public IArchiveBuilder AddDirectory(DirectoryContents directory) {
        this.directories.Add(directory);
        return this;
    }

    public IArchiveBuilder AddDirectories(IEnumerable<DirectoryContents> directories) {
        this.directories.AddRange(directories);
        return this;
    }

    public IArchiveBuilder SetName(string name) {
        this.name = name;
        return this;
    }

    public IArchive Build() {
        if (name is null)
            throw new InvalidOperationException("Archive does not have a name.");

        Archive archive = new Archive(name, files, directories);

        name = null;
        files.Clear();
        directories.Clear();
        
        return archive;
    }   
}
