using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO;
public class FullDirectory {
    public DirectoryInfo Directory { get; }
    public ImmutableArray<FileInfo> Files { get; internal set; } = [];
    public ImmutableArray<FullDirectory> Subdirectories { get; internal set; } = [];

    internal FullDirectory(string path) {
        Directory = new DirectoryInfo(path);
    }

    internal FullDirectory(DirectoryInfo info) {
        Directory = info;
    }
}
