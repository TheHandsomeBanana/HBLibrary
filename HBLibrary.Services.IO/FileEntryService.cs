using HBLibrary.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO;
public class FileEntryService : IFileEntryService {
    #region Copy
    public void CopyDirectory(DirectorySnapshot source, DirectorySnapshot target, CopyOperationAction action = CopyOperationAction.Skip) {
        CopyDirectoryInternal(source.FullPath, target.CombineToFile(source.Path), action);
    }

    private void CopyDirectoryInternal(string source, string target, CopyOperationAction action) {
        foreach (string file in Directory.GetFiles(source)) {
            string targetFile = Path.Combine(target, Path.GetFileName(file));
            CopyFileInternal(file, targetFile, action);
        }

        foreach (string directory in Directory.GetDirectories(source)) {
            string targetDirectory = Path.Combine(target, Path.GetFileName(directory));
            CopyDirectoryInternal(directory, targetDirectory, action);
        }
    }


    public void CopyFile(FileSnapshot source, DirectorySnapshot target, CopyOperationAction action = CopyOperationAction.Skip) {
        CopyFileInternal(source.FullPath, target.CombineToFile(source.Path), action);
    }

    private static void CopyFileInternal(string source, string target, CopyOperationAction action) {
        switch (action) {
            case CopyOperationAction.Skip:
                if (!File.Exists(target))
                    File.Copy(source, target);
                break;
            case CopyOperationAction.OverwriteAll:
                File.Copy(source, target, true);
                break;
            case CopyOperationAction.OverwriteModifiedOnly:
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

    public Task CopyDirectoryAsync(DirectorySnapshot source, DirectorySnapshot target, CopyOperationAction action = CopyOperationAction.Skip) {
        return CopyDirectoryInternalAsync(source.FullPath, target.FullPath, action);
    }

    public Task CopyFileAsync(FileSnapshot source, DirectorySnapshot target, CopyOperationAction action = CopyOperationAction.Skip) {
        return CopyFileInternalAsync(source.FullPath, source.OptimalBufferSize, target.CombineToFile(source.Path), action);
    }

    private static async Task CopyDirectoryInternalAsync(string source, string target, CopyOperationAction action) {
        List<Task> copyFileTasks = [];
        foreach (string file in Directory.GetFiles(source)) {
            string targetFile = Path.Combine(target, Path.GetFileName(file));
            copyFileTasks.Add(CopyFileInternalAsync(file, FileSnapshot.GetOptimalBufferSize(file), targetFile, action));
        }
        await Task.WhenAll(copyFileTasks);

        List<Task> copyDirectoryTasks = [];
        foreach (string directory in Directory.GetDirectories(source)) {
            string targetDirectory = Path.Combine(target, Path.GetFileName(directory));
            copyFileTasks.Add(CopyDirectoryInternalAsync(directory, targetDirectory, action));
        }
        await Task.WhenAll(copyDirectoryTasks);
    }

    private static async Task CopyFileInternalAsync(string source, int bufferSize, string target, CopyOperationAction action) {
        switch (action) {
            case CopyOperationAction.Skip:
                if (!File.Exists(target)) 
                    await CopyAsync(source, bufferSize, target);
                break;
            case CopyOperationAction.OverwriteAll:
                await CopyAsync(source, bufferSize, target);
                break;
            case CopyOperationAction.OverwriteModifiedOnly:
                if(!File.Exists(target) || new FileInfo(source).Length != new FileInfo(target).Length)
                    await CopyAsync(source, bufferSize, target);

                break;
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
    public void MoveDirectory(DirectorySnapshot source, DirectorySnapshot target, MoveOperationAction action = MoveOperationAction.Skip) {
        Directory.Move(source.FullPath, target.FullPath);
    }

    public void MoveFile(FileSnapshot source, DirectorySnapshot target, MoveOperationAction action = MoveOperationAction.Skip) {
        Directory.Move(source.FullPath, target.FullPath);
    }

    public async Task MoveDirectoryAsync(DirectorySnapshot source, DirectorySnapshot target, MoveOperationAction action = MoveOperationAction.Skip) {
        await MoveDirectoryInternalAsync(source.FullPath, target.FullPath);
    }

    public async Task MoveFileAsync(FileSnapshot source, DirectorySnapshot target, MoveOperationAction action = MoveOperationAction.Skip) {
        await Task.Run(() => File.Move(source.FullPath, target.CombineToFile(source.Path)));
    }

    private async Task MoveDirectoryInternalAsync(string source, string target) {
        List<Task> moveFileTasks = [];
        foreach (string file in Directory.GetFiles(source)) {
            string targetFile = Path.Combine(target, Path.GetFileName(file));
            moveFileTasks.Add(Task.Run(() => File.Move(source, targetFile)));
        }
        await Task.WhenAll(moveFileTasks);

        List<Task> moveDirectoryTasks = [];
        foreach (string directory in Directory.GetDirectories(source)) {
            string targetDirectory = Path.Combine(target, Path.GetFileName(directory));
            moveFileTasks.Add(MoveDirectoryInternalAsync(directory, targetDirectory));
        }
        await Task.WhenAll(moveDirectoryTasks);
    }
    #endregion

    #region Replace
    public void ReplaceDirectory(DirectorySnapshot source, DirectorySnapshot target) {
        Directory.Delete(target.FullPath, true);
        MoveDirectory(source, target);
    }

    public void ReplaceFile(FileSnapshot source, FileSnapshot target) {
        File.Delete(target.FullPath);
        File.Move(source.FullPath, target.FullPath);
    }

    public Task ReplaceDirectoryAsync(DirectorySnapshot source, DirectorySnapshot target) {
        Directory.Delete(target.FullPath, true);
        return MoveDirectoryAsync(source, target);
    }

    public Task ReplaceFileAsync(FileSnapshot source, FileSnapshot target) {
        File.Delete(target.FullPath);
        return Task.Run(() => File.Move(source.FullPath, target.FullPath));
    }
    #endregion
}
