namespace HBLibrary.Services.IO;
public static class PathValidator {
    public static bool ValidatePath(string path)
        => !string.IsNullOrEmpty(path)
        && path.IndexOfAny(Path.GetInvalidPathChars()) == -1;

    public static bool IsUNCPath(string path) => path.StartsWith("\\\\") && path.Length > 2;
}
