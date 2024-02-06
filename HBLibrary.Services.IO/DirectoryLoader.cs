using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO;
public static class DirectoryLoader {
    public static DirectorySnapshot LoadImmutableDirectory(string path) {
        ImmutableArray<FileSnapshot> fileSnapshots = Directory.GetFiles(path)
                                     .Select(filePath => new FileSnapshot(filePath))
                                     .ToImmutableArray();

        ImmutableArray<DirectorySnapshot> directorySnapshots = Directory.GetDirectories(path)
                                          .AsParallel()
                                          .Select(LoadImmutableDirectory)
                                          .ToImmutableArray();

        return new DirectorySnapshot(path, fileSnapshots, directorySnapshots);
    }

    public static Task<DirectorySnapshot> LoadImmutableDirectoryAsync(string path) {
        return Task.Run(() => LoadImmutableDirectory(path));
    }

    public static DirectoryNode LoadDirectory(string path) {
        DirectoryNode root = new DirectoryNode(path);
        LoadDirectory(root);
        return root;
    }

    public static async Task<DirectoryNode> LoadDirectoryAsync(string path) {
        DirectoryNode root = new DirectoryNode(path);
        await Task.Run(() => LoadDirectory(root));
        return root;
    }

    private static void LoadDirectory(DirectoryNode directoryNode) {
        directoryNode.Files = directoryNode.Directory.EnumerateFiles().ToImmutableArray();

        IEnumerable<DirectoryInfo> subdirectoryInfos = directoryNode.Directory.EnumerateDirectories();
        ConcurrentBag<DirectoryNode> subdirectoryNodes = new ConcurrentBag<DirectoryNode>();

        Parallel.ForEach(subdirectoryInfos, subDirectoryInfo =>
        {
            var subDirectoryNode = new DirectoryNode(subDirectoryInfo);
            LoadDirectory(subDirectoryNode);
            subdirectoryNodes.Add(subDirectoryNode);
        });

        directoryNode.Subdirectories = subdirectoryNodes.ToImmutableArray();
    }
}
