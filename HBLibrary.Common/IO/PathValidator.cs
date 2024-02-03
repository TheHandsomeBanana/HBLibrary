using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.IO;
public static class PathValidator {
    public static bool ValidatePath(string path) 
        => !string.IsNullOrEmpty(path) 
        && path.IndexOfAny(Path.GetInvalidPathChars()) == -1;

    public static bool PathExists(string path, out string? fullPath) {
        fullPath = null;

        if (!ValidatePath(path))
            return false;

        try {
            fullPath = Path.GetFullPath(path);
            return true;
        }
        catch (Exception) {
            return false;
        }
    }

    public static bool ValidateFile(string path) => ValidatePath(path) && File.Exists(path);
    public static bool FileExists(string path, out string? fullPath) => PathExists(path, out fullPath) && File.Exists(path);
    public static bool ValidateDirectory(string path) => ValidatePath(path) && Directory.Exists(path);
    public static bool DirectoryExists(string path, out string? fullPath) => PathExists(path, out fullPath) && Directory.Exists(path);
}
