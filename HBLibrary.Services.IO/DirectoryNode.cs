using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO;
public class DirectoryNode {
    public DirectoryInfo Directory { get; }
    public ImmutableArray<FileInfo> Files { get; internal set; } = [];
    public ImmutableArray<DirectoryNode> Subdirectories { get; internal set; } = [];

    internal DirectoryNode(string path) {
        Directory = new DirectoryInfo(path);
    }

    internal DirectoryNode(DirectoryInfo info) {
        Directory = info;
    }
}
