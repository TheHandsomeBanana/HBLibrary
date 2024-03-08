namespace HBLibrary.Services.IO;
public interface IFileEntryService {
    void CopyFile(string sourceFile, string targetFile, CopyConflictAction action = CopyConflictAction.Skip);
    void MoveFile(string sourceFile, string targetFile, MoveOperationAction action = MoveOperationAction.Skip);
    Task CopyFileAsync(string sourceFile, string targetFile, CopyConflictAction action = CopyConflictAction.Skip);
    Task MoveFileAsync(string sourceFile, string targetFile, MoveOperationAction action = MoveOperationAction.Skip);

    void CopyDirectory(string sourceDir, string targetDir, CopyConflictAction action = CopyConflictAction.Skip);
    void MoveDirectory(string sourceDir, string targetDir, MoveOperationAction action = MoveOperationAction.Skip);
    void ReplaceDirectory(string sourceDir, string targetDir);
    Task CopyDirectoryAsync(string sourceDir, string targetDir, CopyConflictAction action = CopyConflictAction.Skip);
    Task MoveDirectoryAsync(string sourceDir, string targetDir, MoveOperationAction action = MoveOperationAction.Skip);
    Task ReplaceDirectoryAsync(string sourceDir, string targetDir);
}

public enum CopyConflictAction {
    Skip,
    OverwriteAll,
    OverwriteModifiedOnly
}

public enum MoveOperationAction {
    Skip,
    Overwrite
}
