using HBLibrary.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO;
public class FileEntryService : IFileEntryService {
    #region Copy
    public async Task CopyDirectoryAsync(DirectorySnapshot source, DirectorySnapshot target, CopyOperationAction action = CopyOperationAction.Skip) {
        await CopyDirectoryInternalAsync(source.FullPath, target.FullPath);
    }

    public async Task CopyFileAsync(FileSnapshot source, DirectorySnapshot target, CopyOperationAction action = CopyOperationAction.Skip) {
        await CopyFileInternalAsync(source.FullPath, source.OptimalBufferSize, target.CombineToFile(source.Path));
    }

    private async Task CopyDirectoryInternalAsync(string source, string target) {
        List<Task> copyFileTasks = [];
        foreach (string file in Directory.GetFiles(source)) {
            string targetFile = Path.Combine(target, Path.GetFileName(file));
            copyFileTasks.Add(CopyFileInternalAsync(file, FileSnapshot.GetOptimalBufferSize(file), targetFile));
        }
        await Task.WhenAll(copyFileTasks);

        List<Task> copyDirectoryTasks = [];
        foreach (string directory in Directory.GetDirectories(source)) {
            string targetDirectory = Path.Combine(target, Path.GetFileName(directory));
            copyFileTasks.Add(CopyDirectoryInternalAsync(directory, targetDirectory));
        }
        await Task.WhenAll(copyDirectoryTasks);
    }

    private async Task CopyFileInternalAsync(string source, int bufferSize, string target) {
        using FileStream sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, true);
        using FileStream destinationStream = new FileStream(target, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, true);

        await sourceStream.CopyToAsync(destinationStream);
    }
    #endregion Copy

    #region Move
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

    public Task ReplaceDirectoryAsync(DirectorySnapshot source, DirectorySnapshot target) {
        throw new NotImplementedException();
    }

    public Task ReplaceFileAsync(FileSnapshot source, FileSnapshot target) {
        throw new NotImplementedException();
    }
}
