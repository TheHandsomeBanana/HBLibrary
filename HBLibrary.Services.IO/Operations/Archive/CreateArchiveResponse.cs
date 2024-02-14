using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations.Archive;
public class CreateArchiveResponse : IOOperationResponse {
    public long Size { get; internal set; }
    public ArchiveSizeUnit SizeUnit { get; internal set; }
    public string ArchiveSize => $"{Size} {SizeUnit}";
    public int FileLength { get; internal set; }
    public int DirectoryLength { get; internal set; }
    public int Length => FileLength + DirectoryLength;
}

public enum ArchiveSizeUnit {
    Byte,
    kB,
    MB,
    GB
}
