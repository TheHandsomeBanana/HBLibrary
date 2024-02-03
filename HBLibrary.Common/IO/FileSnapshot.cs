﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.IO;
public readonly struct FileSnapshot {
    public string Path { get; }
    public string FullPath { get; }
    public long Size { get; }
    public int OptimalBufferSize { get; }

    public FileSnapshot(string path) {
        if (!PathValidator.ValidatePath(path))
            throw new ArgumentException("The given path contains illegal characters", nameof(path));

        if (!File.Exists(path))
            File.Create(path).Dispose();

        Path = path;
        FileInfo temp = GetInfo();
        FullPath = temp.FullName;
        Size = temp.Length;
        OptimalBufferSize = GetOptimalBufferSize(Size);
    }

    public FileInfo GetInfo() => new FileInfo(Path);

    // https://github.com/dotnet/runtime/discussions/74405
    public static int GetOptimalBufferSize(long fileSize) {
        long fileSizeBytes = fileSize * 1024;

        if (fileSizeBytes <= 128 * 1024) // Up to 128 KB
            return 2048; // 2 KB
        else if (fileSizeBytes <= 512 * 1024) // Up to 512 KB
            return 65536; // 64 KB
        else if (fileSizeBytes <= 1 * 1024 * 1024) // Up to 1 MB
            return 81920; // ~80 KB
        else if (fileSizeBytes <= 10 * 1024 * 1024) // Up to 10 MB
            return 131072; // 128 KB
        else if (fileSizeBytes <= 32 * 1024 * 1024) // Up to 32 MB
            return 262144; // 256 KB
        else if (fileSizeBytes <= 100 * 1024 * 1024) // Up to 100 MB
            return 524288; // 512 KB
        else // Larger than 100 MB
            return 1048576; // 1 MB
    }

    public static int GetOptimalBufferSize(string file) {
        return GetOptimalBufferSize(new FileInfo(file).Length);
    }
}