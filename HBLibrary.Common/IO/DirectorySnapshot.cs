using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.IO;
public readonly struct DirectorySnapshot {
    public string Path { get; }
    public string FullPath { get; }

    public DirectorySnapshot(string path) {
        if (!PathValidator.ValidatePath(path))
            throw new ArgumentException("The given path contains illegal characters", nameof(path));

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
       
        Path = path;
        FullPath = System.IO.Path.GetFullPath(path);
    }

    public DirectoryInfo GetDirectoryInfo() => new DirectoryInfo(FullPath);

    public string CombineToFile(string file) => System.IO.Path.Combine(FullPath, System.IO.Path.GetFileName(file));
}
