using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.IO;
public class DirectoryNode {
    public string Path { get; }
    public List<string> Files { get; } = [];
    public List<DirectoryNode> Subdirectories { get; } = [];

    public DirectoryNode(string path) {
        Path = path;
    }

    public DirectorySnapshot CreateSnapshot() {
        return new DirectorySnapshot(Path) {
            Files = Files.Select(e => new FileSnapshot(e)).ToImmutableArray(),
            SubDirectories = Subdirectories.Select(e => e.CreateSnapshot()).ToImmutableArray()
        };
    }
}
