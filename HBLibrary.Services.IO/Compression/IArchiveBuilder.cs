using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression;
public interface IArchiveBuilder {
    IArchiveBuilder AddFile(FileSnapshot file);
    IArchiveBuilder AddFiles(IEnumerable<FileSnapshot> files);
    IArchiveBuilder AddDirectory(DirectoryContents directory);
    IArchiveBuilder AddDirectories(IEnumerable<DirectoryContents> directories);
    IArchiveBuilder SetName(string targetArchive);
    IArchive Build();
}
