﻿using HBLibrary.Interface.IO;
using System.Collections.Immutable;

namespace HBLibrary.IO;
public class DirectoryContents {
    public DirectorySnapshot DirectorySnapshot { get; }
    public ImmutableArray<DirectoryContents> Subdirectories { get; } = [];
    public ImmutableArray<FileSnapshot> Files { get; } = [];

    internal DirectoryContents(DirectorySnapshot directory, ImmutableArray<FileSnapshot> files, ImmutableArray<DirectoryContents> directories) {
        DirectorySnapshot = directory;
        Files = files;
        Subdirectories = directories;
    }

    /// <summary>
    /// Utilizes <see cref="DirectoryLoader"/> to create a full file entry image.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static DirectoryContents Create(DirectorySnapshot directory) {
        if (directory.IsNewDirectory)
            return new DirectoryContents(directory, [], []);

        return DirectoryLoader.LoadDirectoryContents(directory);
    }
}
