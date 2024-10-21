using HBLibrary.Core.Limiter;
using HBLibrary.Interface.IO;

namespace HBLibrary.IO;
public class FileEntryService : IFileEntryService {
    #region Copy
    public void CopyDirectory(string source, string target, CopyConflictAction action = CopyConflictAction.Skip) {
        if (PathValidator.ValidatePath(source) || !Directory.Exists(source))
            throw new ArgumentException("Path invalid.", nameof(source));

        if (PathValidator.ValidatePath(target) || !Directory.Exists(target))
            throw new ArgumentException("Path invalid.", nameof(target));

        CopyDirectoryInternal(source, target, action);
    }

    public void CopyFile(string source, string target, CopyConflictAction action = CopyConflictAction.Skip) {
        if (PathValidator.ValidatePath(source) || !File.Exists(source))
            throw new ArgumentException("Path invalid.", nameof(source));

        if (PathValidator.ValidatePath(target))
            throw new ArgumentException("Path invalid.", nameof(target));

        CopyFileInternal(source, target, action);
    }

    public Task CopyDirectoryAsync(string source, string target, CopyConflictAction action = CopyConflictAction.Skip) {
        if (PathValidator.ValidatePath(source) || !Directory.Exists(source))
            throw new ArgumentException("Path invalid.", nameof(source));

        if (PathValidator.ValidatePath(target) || !Directory.Exists(target))
            throw new ArgumentException("Path invalid.", nameof(target));

        return CopyDirectoryInternalAsync(source, target, action);
    }

    private static SemaphoreSlim semaphore = new SemaphoreSlim(4);
    /// <summary>
    /// <paramref name="maxConcurrency"/> will always be limited to the range of 1 to 20.
    /// <br></br>
    /// For HDDs: A maximum concurrency of around 4-8 is usually sufficient
    /// <br></br>
    /// For SSDs: Depending on the performance of your SSD a range of 10-20 should be optimal
    /// </summary>
    /// <param name="maxConcurrency"></param>
    public static void SetAsyncThrottle(int maxConcurrency) {
        semaphore.Dispose();

        Int32Limiter.LimitToRangeRef(ref maxConcurrency, 1, 20);
        semaphore = new SemaphoreSlim(maxConcurrency);
    }

    public Task CopyFileAsync(string source, string target, CopyConflictAction action = CopyConflictAction.Skip) {
        if (PathValidator.ValidatePath(source) || !File.Exists(source))
            throw new ArgumentException("Path invalid.", nameof(source));

        if (PathValidator.ValidatePath(target))
            throw new ArgumentException("Path invalid.", nameof(target));

        return CopyFileInternalAsync(source, FileSnapshot.GetOptimalBufferSize(source), target, action);
    }

    private static void CopyDirectoryInternal(string source, string target, CopyConflictAction action) {
        foreach (string file in Directory.GetFiles(source)) {
            string targetFile = Path.Combine(target, Path.GetFileName(file));
            CopyFileInternal(file, targetFile, action);
        }

        foreach (string directory in Directory.GetDirectories(source)) {
            string targetDirectory = Path.Combine(target, Path.GetFileName(directory));
            CopyDirectoryInternal(directory, targetDirectory, action);
        }
    }

    private static void CopyFileInternal(string source, string target, CopyConflictAction action) {
        string targetDir = Path.GetDirectoryName(target)!;
        Directory.CreateDirectory(targetDir);

        switch (action) {
            case CopyConflictAction.Skip:
                if (!File.Exists(target))
                    File.Copy(source, target);
                break;
            case CopyConflictAction.OverwriteAll:
                File.Copy(source, target, true);
                break;
            case CopyConflictAction.OverwriteModifiedOnly:
                if (File.Exists(target)) {
                    FileInfo sourceInfo = new FileInfo(source);
                    FileInfo targetInfo = new FileInfo(target);

                    if (sourceInfo.Length != targetInfo.Length)
                        File.Copy(source, target, true);
                }
                break;
            default:
                throw new NotSupportedException(action.ToString());
        }
    }

    private static async Task CopyDirectoryInternalAsync(string source, string target, CopyConflictAction action) {
        List<Task> copyTasks = [];
        foreach (string file in Directory.GetFiles(source)) {
            string targetFile = Path.Combine(target, Path.GetFileName(file));
            copyTasks.Add(CopyFileThrottledAsync(file, FileSnapshot.GetOptimalBufferSize(file), targetFile, action));
        }

        foreach (string directory in Directory.GetDirectories(source)) {
            string targetDirectory = Path.Combine(target, Path.GetFileName(directory));
            copyTasks.Add(CopyDirectoryInternalAsync(directory, targetDirectory, action));
        }

        await Task.WhenAll(copyTasks);
    }

    private static async Task CopyFileInternalAsync(string source, int bufferSize, string target, CopyConflictAction action) {
        string targetDir = Path.GetDirectoryName(target)!;
        Directory.CreateDirectory(targetDir);

        switch (action) {
            case CopyConflictAction.Skip:
                if (!File.Exists(target))
                    await CopyAsync(source, bufferSize, target);
                break;
            case CopyConflictAction.OverwriteAll:
                await CopyAsync(source, bufferSize, target);
                break;
            case CopyConflictAction.OverwriteModifiedOnly:
                if (!File.Exists(target) || new FileInfo(source).Length != new FileInfo(target).Length)
                    await CopyAsync(source, bufferSize, target);

                break;
        }
    }

    private static async Task CopyFileThrottledAsync(string source, int bufferSize, string target, CopyConflictAction action) {
        await semaphore.WaitAsync();

        string targetDir = Path.GetDirectoryName(target)!;
        Directory.CreateDirectory(targetDir);

        try {
            switch (action) {
                case CopyConflictAction.Skip:
                    if (!File.Exists(target))
                        await CopyAsync(source, bufferSize, target);
                    break;
                case CopyConflictAction.OverwriteAll:
                    await CopyAsync(source, bufferSize, target);
                    break;
                case CopyConflictAction.OverwriteModifiedOnly:
                    if (!File.Exists(target) || new FileInfo(source).Length != new FileInfo(target).Length)
                        await CopyAsync(source, bufferSize, target);
                    break;
            }
        }
        finally {
            semaphore.Release();
        }
    }

    private static Task CopyAsync(string source, int bufferSize, string target) {
        using (FileStream sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, true)) {
            using (FileStream destinationStream = new FileStream(target, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, true)) {
                return sourceStream.CopyToAsync(destinationStream);
            }
        }
    }
    #endregion Copy

    #region Move
    public void MoveDirectory(string source, string target, MoveOperationAction action = MoveOperationAction.Skip) {
        if (PathValidator.ValidatePath(source) || !Directory.Exists(source))
            throw new ArgumentException("Path invalid.", nameof(source));

        if (PathValidator.ValidatePath(target) || !Directory.Exists(target))
            throw new ArgumentException("Path invalid.", nameof(target));


        switch (action) {
            case MoveOperationAction.Skip:
                if (Directory.EnumerateDirectories(target).All(e => e != target))
                    Directory.Move(source, target);
                break;
            case MoveOperationAction.Overwrite:
                MoveDirectoryWithOverwrite(source, target);
                break;
        }
    }

    private static void MoveDirectoryWithOverwrite(string source, string target) {
        foreach (string file in Directory.GetFiles(source)) {
            string destFile = Path.Combine(target, Path.GetFileName(file));

#if NETFRAMEWORK
            if (File.Exists(destFile))
                File.Delete(destFile);

            File.Move(source, destFile);
#elif NET5_0_OR_GREATER
            File.Move(file, destFile, true); // Overwrites the file if it already exists
#endif
        }

        foreach (string directory in Directory.GetDirectories(source)) {
            string destDir = Path.Combine(target, Path.GetFileName(directory));
            MoveDirectoryWithOverwrite(directory, destDir); // Recursive call for subdirectories
        }
    }

    public void MoveFile(string source, string target, MoveOperationAction action = MoveOperationAction.Skip) {
        if (PathValidator.ValidatePath(source) || !File.Exists(source))
            throw new ArgumentException("Path invalid.", nameof(source));

        if (PathValidator.ValidatePath(target))
            throw new ArgumentException("Path invalid.", nameof(target));

        switch (action) {
            case MoveOperationAction.Skip:
                if (!File.Exists(target))
                    File.Move(source, target);
                break;
            case MoveOperationAction.Overwrite:
#if NETFRAMEWORK
                if (File.Exists(target))
                    File.Delete(target);

                File.Move(source, target);
#elif NET5_0_OR_GREATER
                File.Move(source, target, true); // Overwrites the file if it already exists
#endif                
                break;
        }
    }

    public Task MoveDirectoryAsync(string source, string target, MoveOperationAction action = MoveOperationAction.Skip) {
        if (PathValidator.ValidatePath(source) || !Directory.Exists(source))
            throw new ArgumentException("Path invalid.", nameof(source));

        if (PathValidator.ValidatePath(target) || !Directory.Exists(target))
            throw new ArgumentException("Path invalid.", nameof(target));

        return MoveDirectoryInternalAsync(source, target, action);
    }

    public Task MoveFileAsync(string source, string target, MoveOperationAction action = MoveOperationAction.Skip) {
        if (PathValidator.ValidatePath(source) || !File.Exists(source))
            throw new ArgumentException("Path invalid.", nameof(source));

        if (PathValidator.ValidatePath(target))
            throw new ArgumentException("Path invalid.", nameof(target));

        switch (action) {
            case MoveOperationAction.Skip:
                if (!File.Exists(target))
                    return Task.Run(() => File.Move(source, target));

                return Task.CompletedTask;
            case MoveOperationAction.Overwrite:
                return Task.Run(() => {
#if NETFRAMEWORK
                    if (File.Exists(target))
                        File.Delete(target);

                    File.Move(source, target);
#elif NET5_0_OR_GREATER
                    File.Move(source, target, true);
#endif
                });
            default:
                throw new NotSupportedException(action.ToString());
        }
    }

    private async Task MoveDirectoryInternalAsync(string source, string target, MoveOperationAction action) {
        List<Task> moveFileTasks = [];
        foreach (string file in Directory.GetFiles(source)) {
            string targetFile = Path.Combine(target, Path.GetFileName(file));
            moveFileTasks.Add(MoveFileAsync(source, targetFile, action));
        }
        await Task.WhenAll(moveFileTasks);

        List<Task> moveDirectoryTasks = [];
        foreach (string directory in Directory.GetDirectories(source)) {
            string targetDirectory = Path.Combine(target, Path.GetFileName(directory));
            moveFileTasks.Add(MoveDirectoryInternalAsync(directory, targetDirectory, action));
        }
        await Task.WhenAll(moveDirectoryTasks);
    }
    #endregion

    #region Replace
    public void ReplaceDirectory(string source, string target) {
        if (Directory.Exists(target))
            Directory.Delete(target, true);

        MoveDirectory(source, target);
    }

    public Task ReplaceDirectoryAsync(string source, string target) {
        if (Directory.Exists(target))
            Directory.Delete(target, true);

        return MoveDirectoryAsync(source, target);
    }
    #endregion
}
