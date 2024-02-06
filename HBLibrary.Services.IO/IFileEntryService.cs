using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO;
public interface IFileEntryService {
    void CopyFile(string sourceFile, string targetFile, CopyOperationAction action = CopyOperationAction.Skip);
    void MoveFile(string sourceFile, string targetFile, MoveOperationAction action = MoveOperationAction.Skip);
    Task CopyFileAsync(string sourceFile, string targetFile, CopyOperationAction action = CopyOperationAction.Skip);
    Task MoveFileAsync(string sourceFile, string targetFile, MoveOperationAction action = MoveOperationAction.Skip);

    void CopyDirectory(string sourceDir, string targetDir, CopyOperationAction action = CopyOperationAction.Skip);
    void MoveDirectory(string sourceDir, string targetDir, MoveOperationAction action = MoveOperationAction.Skip);
    void ReplaceDirectory(string sourceDir, string targetDir);
    Task CopyDirectoryAsync(string sourceDir, string targetDir, CopyOperationAction action = CopyOperationAction.Skip);
    Task MoveDirectoryAsync(string sourceDir, string targetDir, MoveOperationAction action = MoveOperationAction.Skip);
    Task ReplaceDirectoryAsync(string sourceDir, string targetDir);
}

public enum CopyOperationAction {
    Skip,
    OverwriteAll,
    OverwriteModifiedOnly
}

public enum MoveOperationAction {
    Skip,
    Overwrite
}
