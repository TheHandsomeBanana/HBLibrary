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
}
