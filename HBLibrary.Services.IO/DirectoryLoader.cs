using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace HBLibrary.Services.IO;
public static class DirectoryLoader {
    public static DirectoryContents LoadDirectoryContents(DirectorySnapshot directorySnapshot) {
        ImmutableArray<FileSnapshot> files = Directory.GetFiles(directorySnapshot.FullPath)
                                     .Select(filePath => new FileSnapshot(filePath))
                                     .ToImmutableArray();

        ImmutableArray<DirectoryContents> directories = Directory.GetDirectories(directorySnapshot.FullPath)
                                          .AsParallel()
                                          .Select(e => LoadDirectoryContents(new DirectorySnapshot(e)))
                                          .ToImmutableArray();

        return new DirectoryContents(directorySnapshot, files, directories);
    }

    public static FullDirectory LoadFullDirectory(DirectorySnapshot directorySnapshot) {
        FullDirectory root = new FullDirectory(directorySnapshot.FullPath);
        LoadDirectory(root);
        return root;
    }

    public static async Task<FullDirectory> LoadFullDirectoryAsync(DirectorySnapshot directorySnapshot) {
        FullDirectory root = new FullDirectory(directorySnapshot.FullPath);
        await Task.Run(() => LoadDirectory(root));
        return root;
    }

    private static void LoadDirectory(FullDirectory directoryNode) {
        directoryNode.Files = directoryNode.Directory.EnumerateFiles().ToImmutableArray();

        IEnumerable<DirectoryInfo> subdirectoryInfos = directoryNode.Directory.EnumerateDirectories();
        ConcurrentBag<FullDirectory> subdirectoryNodes = new ConcurrentBag<FullDirectory>();

        Parallel.ForEach(subdirectoryInfos, subDirectoryInfo => {
            var subDirectoryNode = new FullDirectory(subDirectoryInfo);
            LoadDirectory(subDirectoryNode);
            subdirectoryNodes.Add(subDirectoryNode);
        });

        directoryNode.Subdirectories = subdirectoryNodes.ToImmutableArray();
    }
}
