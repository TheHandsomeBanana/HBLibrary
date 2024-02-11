using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO;
public readonly struct ValidPath {
    public string Path { get; }
    public string? FullPath { get; }
    public bool IsFile { get; }
    public bool IsDirectory { get; }
    public bool Exists => IsFile || IsDirectory;
    public bool IsUNC { get; }


    private ValidPath(string path, string? fullPath, bool isFile, bool isDirectory, bool isUNC) {
        Path = path;
        FullPath = fullPath;
        IsFile = isFile;
        IsDirectory = isDirectory;
        IsUNC = isUNC;
    }

    internal ValidPath(FileSnapshot file) {
        Path = file.Path;
        FullPath = file.FullPath;
        IsFile = true;
        IsDirectory = false;
        IsUNC = PathValidator.IsUNCPath(file.FullPath);
    }

    internal ValidPath(DirectorySnapshot directory) {
        Path = directory.Path;
        FullPath = directory.FullPath;
        IsDirectory = true;
        IsFile = false;
        IsUNC = PathValidator.IsUNCPath(directory.FullPath);
    }

    /// <summary>
    /// Use when path should exist on file system.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="validPath"></param>
    /// <returns>True if the path is valid and refers to an existing file or directory.</returns>
    public static bool TryCreate(string path, out ValidPath? validPath) {
        validPath = null;
        if(!PathValidator.ValidatePath(path)) 
            return false;

        bool isFile = File.Exists(path);
        bool isDirectory = Directory.Exists(path);
        bool isUNC = PathValidator.IsUNCPath(path);

        if (!isFile && !isDirectory)
            return false;

        string fullPath = System.IO.Path.GetFullPath(path);

        validPath = new ValidPath(path, fullPath, isFile, isDirectory, isUNC);
        return true;
    }

    /// <summary>
    /// Use when path is not existing on file system.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="willBeFile"></param>
    /// <param name="validPath"></param>
    /// <returns>True when the path is valid.</returns>
    public static bool TryCreate(string path, bool willBeFile, out ValidPath? validPath) {
        validPath = null;
        if (!PathValidator.ValidatePath(path))
            return false;

        bool isFile = willBeFile;
        bool isDirectory = !willBeFile;
        bool isUNC = PathValidator.IsUNCPath(path);

        if (!isFile && !isDirectory)
            return false;

        string? fullPath = null;
        if (System.IO.Path.IsPathRooted(path) || isUNC)
            fullPath = path;

        validPath = new ValidPath(path, fullPath, isFile, isDirectory, isUNC);
        return true;
    }
}
